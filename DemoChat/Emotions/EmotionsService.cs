using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Audio.Models;
using DemoChat.Chat.Models;
using DemoChat.Emotions.Interfaces;
using DemoChat.Emotions.Models;
using DemoChat.Games.Models;
using DemoChat.Hume.Interfaces;
using DemoChat.Hume.Models;
using DemoChat.Repository;
using ICSharpCode.SharpZipLib.Zip;

namespace DemoChat.Emotions;
public class EmotionsService : IEmotionsService
{
    private readonly ILogger<EmotionsService> _logger;
    private readonly IHumeService _humeService;
    private readonly IRepository _repository;

    public EmotionsService(ILogger<EmotionsService> logger, IHumeService humeService, IRepository repository)
    {
        _logger = logger;
        _humeService = humeService;
        _repository = repository;
    }

    public async Task<string> CreateEmotionsSession(string gameSessionId, CancellationToken cancellationToken)
    {
        var emotionsSession = new EmotionsSession(gameSessionId);
        emotionsSession = await _repository.AddAsync(emotionsSession, cancellationToken);
        return emotionsSession.Id;

    }

    public async Task<string> CreateEmotionsJobInference(string emotionsSessionId, CancellationToken cancellationToken)
    {
        var emotionsSession = await _repository.GetByIdAsync<EmotionsSession>(emotionsSessionId, cancellationToken);

        var audioFilesPaths = await GetAudioFilesPath(emotionsSession.GameSessionId, cancellationToken);
        var zipFilePath = CreateZipFile(emotionsSession.Id, audioFilesPaths);

        var jobId = await _humeService.CreateJobInference(zipFilePath, cancellationToken);

        emotionsSession.JobId = jobId;
        emotionsSession.ZipFilePath = zipFilePath;

        await _repository.UpdateAsync(emotionsSession, cancellationToken);

        return emotionsSession.JobId;
    }

    public async Task<string> GetEmotionsJobStatus(string jobId, CancellationToken cancellationToken)
    {
        return await _humeService.GetJobInferenceStatus(jobId, cancellationToken);
    }

    private async Task<string[]> GetAudioFilesPath(string gameSessionId, CancellationToken cancellationToken)
    {

        // TODO: implement specifications?
        // Specification 
        // Get all user audioFiles path for this gameSessionId 

        // Temp solution
        var gameSession = await _repository.GetByIdAsync<GameSession>(gameSessionId, cancellationToken);
        var audioFiles = await _repository.ListAsync<AudioFile>(cancellationToken); // This can load a lot of unneeded files
        var userAudioFilesPath = audioFiles.Where(x => x.ChatSessionId == gameSession.ChatSessionId && x.Name.StartsWith("audio-user")).Select(x => x.Path).ToArray<string>();

        return userAudioFilesPath;
    }

    private string CreateZipFile(string emotionsSessionId, string[] audioFilePaths)
    {
        var zipFilePath = GenerateZipFileDetails(emotionsSessionId);
        using (var zipStream = new ZipOutputStream(System.IO.File.Create(zipFilePath)))
        {
            zipStream.SetLevel(0);  // store only

            byte[] buffer = new byte[4096];
            foreach (var audioFilePath in audioFilePaths)
            {

                var entry = new ZipEntry(Path.GetFileName(audioFilePath))
                {
                    DateTime = DateTime.UtcNow
                };
                zipStream.PutNextEntry(entry);

                using (var audioStream = System.IO.File.OpenRead(audioFilePath))
                {
                    int sourceBytes;
                    do
                    {
                        sourceBytes = audioStream.Read(buffer, 0, buffer.Length);
                        zipStream.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }
                zipStream.CloseEntry();
            }
            zipStream.Finish();
            zipStream.Close();
        }

        return zipFilePath;
    }

    private string GenerateZipFileDetails(string emotionsSessionId)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var sessionFolderName = "Sessions";
        var emotionsSessionName = $"session-emotions-{emotionsSessionId}";

        var zipFilePath = Path.Combine(baseDirectory, sessionFolderName, emotionsSessionName);

        if (!Directory.Exists(zipFilePath))
        {
            Directory.CreateDirectory(zipFilePath);
        }

        var fileName = $"zip-file-{emotionsSessionId}.zip";
        var outputPath = Path.Combine(zipFilePath, fileName);

        return outputPath;
    }
}
