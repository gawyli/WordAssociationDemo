using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Chat.Interfaces;
using DemoChat.Emotions.Interfaces;
using DemoChat.Games.Interfaces;
using DemoChat.Games.Models;
using DemoChat.Repository;
using Microsoft.SemanticKernel;

namespace DemoChat.Plugins;
public class PlaygroundPlugin
{
    private readonly IEmotionsService _emotionsService;
    //private readonly IChatService _chatService;
    private readonly IGameService _gameService;

    public PlaygroundPlugin(ILogger<PlaygroundPlugin> logger, IEmotionsService emotionsService, IGameService gameService)
    {
        _emotionsService = emotionsService;
        //_chatService = chatService;
        _gameService = gameService;
    }

    // TODO: We can't use chat service as ChatService register after Plugin registration
    //[KernelFunction("get_chat_sessions")]
    //[Description("Get a list of key value pair of Chat Session Ids with its Created date time")]
    //[return: Description("List<KeyValuePair<string, string>>")]
    //public async Task<List<KeyValuePair<string, string>>> GetChatSessions()
    //{
    //    var chatSessions = await _chatService.GetChatSession(cancellationToken: default);
    //    return chatSessions;
    //}

    [KernelFunction("get_game_sessions")]
    [Description("Get a list of key value pair of Game Session Ids with its Created date time")]
    [return: Description("List<KeyValuePair<string, string>>")]
    public async Task<List<KeyValuePair<string, string>>> GetGameSessions()
    {
        var gameSessions = await _gameService.GetGameSession(cancellationToken: default);

        return gameSessions;
    }

    [KernelFunction("create_emotions_session")]
    [Description("Create an emotions session in the database")]
    [return: Description("string")]
    public async Task<string> CreateEmotionsSession([Description("Enter GameSessionId:")]string gameSessionId)
    {
        var emotionsSessionId = await _emotionsService.CreateEmotionsSession(gameSessionId, cancellationToken: default);

        await _gameService.SetEmotionsSessionId(gameSessionId, emotionsSessionId, cancellationToken: default);

        return emotionsSessionId;
    }

    [KernelFunction("create_emotions_job_inference")]
    [Description("Create an emotions job inference in the database")]
    [return: Description("string")]
    public async Task<string> CreateEmotionsJobInference([Description("Enter EmotionsSessionId:")] string emotionsSessionId)
    {
        var jobInferenceId = await _emotionsService.CreateEmotionsJobInference(emotionsSessionId, cancellationToken: default);

        return jobInferenceId;
    }

    [KernelFunction("get_emotions_job_status")]
    [Description("Get a emotions job inference status")]
    [return: Description("string")]
    public async Task<string> GetEmotionsJobStatus([Description("Enter EmotionsSessionId:")] string emotionsSessionId)
    {
        var jobStatus = await _emotionsService.GetEmotionsJobStatus(emotionsSessionId, cancellationToken: default);

        return jobStatus;
    }

}
