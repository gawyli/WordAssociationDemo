using MePoC.Audio.Interfaces;
using MePoC.Models;
using MePoC.Repository.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AudioToText;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.TextToAudio;
using NAudio.Wave;

namespace MePoC.Audio;
public class AudioService : IAudioService
{
    public Record? Record { get; set; }

    private readonly ILogger<AudioService> _logger;
    private readonly IRepository _repository;
    private readonly Kernel _kernel;

    public AudioService(ILogger<AudioService> logger, [FromKeyedServices("audio")] Kernel kernel, IRepository repository)
    {
        _logger = logger;
        _kernel = kernel;
        _repository = repository;
    }

    public async Task<string> TranscribeAudio(Record record, CancellationToken cancellationToken)
    {
        var audioToTextService = _kernel.GetRequiredService<IAudioToTextService>();

        OpenAIAudioToTextExecutionSettings executionSettings = new(record.Name)
        {
            Language = "en",           // The language of the audio data as two-letter ISO-639-1 language code (e.g. 'en' or 'es').
            Prompt = "",              // An optional text to guide the model's style or continue a previous audio segment.
                                      // The prompt should match the audio language.
            ResponseFormat = "json", // The format to return the transcribed text in.
                                     // Supported formats are json, text, srt, verbose_json, or vtt. Default is 'json'.
            Temperature = 0.3f,     // The randomness of the generated text.
                                    // Select a value from 0.0 to 1.0. 0 is the default.
        };

        // Read audio content from a file
        var audioFileBytes = await File.ReadAllBytesAsync(record.Path, cancellationToken);
        var audioFileBinaryData = BinaryData.FromBytes(audioFileBytes);
        AudioContent audioContent = new(audioFileBinaryData);

        // Convert audio to text
        var textContent = await audioToTextService.GetTextContentAsync(audioContent, executionSettings);
        _logger.LogInformation($"Transcribed text");

        return textContent.Text!;
    }

    public async Task<Record> RecordAudioTest(CancellationToken cancellationToken)
    {
        (string fileName, string outputPath) = GenerateFileDetails(false, "wav");

        var waveFormat = new WaveFormat(44100, 16, 2);
        using (var waveIn = new WaveInEvent())
        {
            waveIn.WaveFormat = waveFormat;
            using (var writer = new WaveFileWriter(outputPath, waveIn.WaveFormat))
            {
                // Set up the event handler to write to the file when data is available
                waveIn.DataAvailable += (sender, e) =>
                {
                    writer.Write(e.Buffer, 0, e.BytesRecorded);
                };

                var tcs = new TaskCompletionSource<bool>();

                // Handle cancellation
                using (cancellationToken.Register(() =>
                {
                    waveIn.StopRecording();
                    tcs.TrySetResult(true);
                }))
                {
                    // Start recording
                    waveIn.StartRecording();
                    Console.WriteLine("Recording... Press 'spacebar' to stop.");

                    // Stop recording when 'S' is pressed
                    await Task.Run(() =>
                    {
                        var key = Console.ReadKey(intercept: true);
                        if (key.Key == ConsoleKey.Spacebar)
                        {
                            waveIn.StopRecording();
                            tcs.TrySetResult(true);
                        }
                    });

                    await tcs.Task;

                    // Ensure all data is written
                    writer.Flush();

                    // Save the record to the database
                    this.Record = new Record(fileName, outputPath, DateTime.UtcNow);
                    await _repository.AddAsync(Record, cancellationToken);

                    _logger.LogInformation($"Recorded audio saved in Database");
                }
            }
        }

        return this.Record;
    }

    public async Task<Record> RecordAudio(CancellationToken cancellationToken)
    {
        (string fileName, string outputPath) = GenerateFileDetails(false, "wav");

        var waveFormat = new WaveFormat(44100, 16, 2);
        using (var waveIn = new WaveInEvent())
        {
            waveIn.WaveFormat = waveFormat;
            using (var writer = new WaveFileWriter(outputPath, waveIn.WaveFormat))
            {
                // Set up the event handler to write to the file when data is available
                waveIn.DataAvailable += (sender, e) =>
                {
                    writer.Write(e.Buffer, 0, e.BytesRecorded);
                };

                // Start recording

                waveIn.StartRecording();
                Console.WriteLine("Recording... Press any key to stop.");
                Console.ReadKey();
                waveIn.StopRecording();

                // Save the record to the database
                this.Record = new Record(fileName, outputPath, DateTime.UtcNow);
                await _repository.AddAsync(Record, cancellationToken);

                _logger.LogInformation($"Recorded audio saved in Database");
            }
        }

        return this.Record;
    }

    public async Task<Response?> GenerateAudio(string text, CancellationToken cancellationToken)
    {
        var textToAudioService = _kernel.GetRequiredService<ITextToAudioService>();

        // Set execution settings (optional)
        OpenAITextToAudioExecutionSettings executionSettings = new()
        {
            Voice = "alloy", // The voice to use when generating the audio.
                             // Supported voices are alloy, echo, fable, onyx, nova, and shimmer.
            ResponseFormat = "mp3", // The format to audio in.
                                    // Supported formats are mp3, opus, aac, and flac.
            Speed = 1.0f // The speed of the generated audio.
                         // Select a value from 0.25 to 4.0. 1.0 is the default.
        };

        // Convert text to audio
        var audioContent = await textToAudioService.GetAudioContentAsync(text, executionSettings);
        if (audioContent != null)
        {
            string audioMimeType;

            if (String.IsNullOrEmpty(audioContent.MimeType))
            {
                _logger.LogWarning("Audio Mime Type is empty");
                audioMimeType = "mp3";
            }
            else
            {
                audioMimeType = audioContent.MimeType;
            }


            (string fileName, string outputPath) = GenerateFileDetails(true, audioMimeType);   // Response is true because TextToAudio is used ONLY for responses

            try
            {
                var audioFileBytes = audioContent.Data.HasValue ? audioContent.Data.Value.ToArray() : [];

                // Save audio content to a file
                await File.WriteAllBytesAsync(outputPath, audioFileBytes);
                _logger.LogInformation($"Audio file saved at {outputPath}");

                var response = new Response(text, fileName, outputPath, DateTime.UtcNow);
                var id = await _repository.AddAsync(response, cancellationToken);
                _logger.LogInformation($"Response saved in Database with ID: {id}");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error with {ex.Message}");
            }
        }

        return null;
    }

    public void PlayAudio(string path)
    {
        using (var audioFile = new AudioFileReader(path))
        using (var outputDevice = new WaveOutEvent())
        {
            outputDevice.Init(audioFile);
            outputDevice.Play();
            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                Thread.Sleep(1000);
            }
        }
    }

    private (string, string) GenerateFileDetails(bool isResponse, string mimeType)
    {

        string today = DateTime.UtcNow.ToString("yyyyMMdd");
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string subfolderName = isResponse ? "Responses" : "Records";
        string sessionFolderName = $"session{today}";

        // Build the full path to the subfolder
        string fullPath = Path.Combine(baseDirectory, subfolderName, sessionFolderName);

        if (!Directory.Exists(fullPath))
        {
            _logger.LogInformation($"Creating directory {fullPath}");
            Directory.CreateDirectory(fullPath);
        }

        string fileName = isResponse ? $"audio-response{Guid.NewGuid()}.{mimeType}" : $"audio-{Guid.NewGuid()}.{mimeType}";
        string outputPath = Path.Combine(fullPath, fileName);

        return (fileName, outputPath);
    }
}

