using DemoChat.Audio.Interfaces;
using DemoChat.Audio.Models;
using DemoChat.Chat.Models;
using DemoChat.Repository;
using DemoChat.Utilities;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AudioToText;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.TextToAudio;
using NAudio.Wave;

namespace DemoChat.Audio;
public class AudioService : IAudioService
{
    //public AudioFile? Record { get; set; }

    private readonly ILogger<AudioService> _logger;
    private readonly IRepository _repository;
    private readonly Kernel _kernel;

    public AudioService(ILogger<AudioService> logger, [FromKeyedServices("audio")] Kernel kernel, IRepository repository)
    {
        _logger = logger;
        _kernel = kernel;
        _repository = repository;
    }

    public async Task<string> TranscribeAudio(AudioFile audioFile, CancellationToken cancellationToken)
    {
        var audioToTextService = _kernel.GetRequiredService<IAudioToTextService>();

        OpenAIAudioToTextExecutionSettings executionSettings = new(audioFile.Name)
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
        var audioFileBytes = await File.ReadAllBytesAsync(audioFile.Path, cancellationToken);
        var audioFileBinaryData = BinaryData.FromBytes(audioFileBytes);
        AudioContent audioContent = new(audioFileBinaryData, audioFile.MimeType);

        // Convert audio to text
        var textContent = await audioToTextService.GetTextContentAsync(audioContent, executionSettings);
        _logger.LogInformation("Text Transcribed");

        if (!string.IsNullOrEmpty(textContent.Text))
        {
            audioFile.SetContent(textContent.Text);
        }
        
        await _repository.UpdateAsync(audioFile, cancellationToken);

        return textContent.Text!;
    }

    public async Task<AudioFile> RecordAudio(string chatSessionId, CancellationToken cancellationToken)
    {
        var mimeType = "wav";
        (string outputPath, string fileName) = GenerateAudioFileDetails(AuthorRole.User, chatSessionId, mimeType);

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
                    var audioFile = new AudioFile(chatSessionId, fileName, outputPath, mimeType, AuthorRole.User ,DateTime.UtcNow); //Extension for enums to handle for repository
                    await _repository.AddAsync(audioFile, cancellationToken);

                    _logger.LogInformation($"Recorded audio saved in Database");

                    return audioFile;
                }
            }
        }
    }

    //public async Task<AudioFile> RecordAudio(string chatSessionId, CancellationToken cancellationToken)
    //{
    //    (string fileName, string outputPath) = GenerateAudioFileDetails(false, "wav");

    //    var waveFormat = new WaveFormat(44100, 16, 2);
    //    using (var waveIn = new WaveInEvent())
    //    {
    //        waveIn.WaveFormat = waveFormat;
    //        using (var writer = new WaveFileWriter(outputPath, waveIn.WaveFormat))
    //        {
    //            // Set up the event handler to write to the file when data is available
    //            waveIn.DataAvailable += (sender, e) =>
    //            {
    //                writer.Write(e.Buffer, 0, e.BytesRecorded);
    //            };

    //            // Start recording

    //            waveIn.StartRecording();
    //            Console.WriteLine("Recording... Press any key to stop.");
    //            Console.ReadKey();
    //            waveIn.StopRecording();

    //            // Save the record to the database
    //            var audioFile = new AudioFile(chatSessionId, fileName, outputPath, AuthorRole.User, DateTime.UtcNow);
    //            await _repository.AddAsync(audioFile, cancellationToken);

    //            _logger.LogInformation($"Recorded audio saved in Database");

    //            return audioFile;
    //        }
    //    }
    //}

    public async Task<AudioFile?> GenerateAudio(string chatSessionId, string content, CancellationToken cancellationToken)
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
        var audioContent = await textToAudioService.GetAudioContentAsync(content, executionSettings);
        if (audioContent != null)
        {
            string audioMimeType = "mp3";
            try
            {
                (string outputPath, string fileName) = GenerateAudioFileDetails(AuthorRole.Assistant, chatSessionId, audioMimeType);

                var audioFileBytes = audioContent.Data.HasValue ? audioContent.Data.Value.ToArray() : [];

                await File.WriteAllBytesAsync(outputPath, audioFileBytes, cancellationToken);
                _logger.LogInformation($"Audio file saved at {outputPath}");

                var audioFile = new AudioFile(chatSessionId, fileName, outputPath, audioMimeType, AuthorRole.Assistant, DateTime.UtcNow);
                audioFile.SetContent(content);
                audioFile = await _repository.AddAsync(audioFile, cancellationToken);
                _logger.LogInformation($"Response saved in Database with ID: {audioFile.Id}");

                return audioFile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error with {ex.Message}");
                throw new Exception($"Error: {ex.Message}", ex);
            }
        }

        throw new ArgumentNullException($"The Audio Content cannot be null");
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

    private (string, string) GenerateAudioFileDetails(AuthorRole authorRole, string chatSessionId, string mimeType)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string sessionFolderName = "Sessions";
        string sessionName = $"session-{chatSessionId}";

        string subfolderName = string.Empty;
        string fileName = string.Empty;
        string audioPath = string.Empty;
        string outputPath = string.Empty;

        if (authorRole == AuthorRole.Assistant)
        {
            subfolderName = "AssistantAudio";
            audioPath = Path.Combine(baseDirectory, sessionFolderName, sessionName, subfolderName);

            fileName = $"audio-assistant-{Guid.NewGuid()}.{mimeType}";
            outputPath = Path.Combine(audioPath, fileName);
        }

        if (authorRole == AuthorRole.User)
        {
            subfolderName = "UserAudio";
            audioPath = Path.Combine(baseDirectory, sessionFolderName, sessionName, subfolderName);

            fileName = $"audio-user-{Guid.NewGuid()}.{mimeType}";
            outputPath = Path.Combine(audioPath, fileName);
        }

        if (!Directory.Exists(audioPath))
        {
            //_logger.LogInformation($"Creating directory {fullPath}");
            Directory.CreateDirectory(audioPath);
        }

        return (outputPath, fileName);
    }
}

