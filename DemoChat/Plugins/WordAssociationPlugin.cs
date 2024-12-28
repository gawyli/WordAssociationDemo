using DemoChat.Repository;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DemoChat.Games.Models;
using Microsoft.SemanticKernel.ChatCompletion;
using DemoChat.Chat.Models;
using DemoChat.Games.Interfaces;
using DemoChat.Games;
using DemoChat.Hume.Interfaces;

namespace DemoChat.Plugins;
public class WordAssociationPlugin
{
    private readonly ILogger<WordAssociationPlugin> _logger;
    private readonly IGameService _gameService;

    public WordAssociationPlugin(ILogger<WordAssociationPlugin> logger, IGameService gameService)
    {
        _logger = logger;
        _gameService = gameService;
    }

    [KernelFunction("add_associations")]
    [Description("Add an association of a stimulus and a user response word to the database")]
    [return: Description("A string event")]
    public async Task<string> AddAssociations(
        [Description("Enter GameSessionId:")] string gameSessionId,
        [Description("Enter a stimulus word")] string stimulusWord,
        [Description("Enter a user response word")] string responseWord)
    {
        // Preprocess words so it does not include dots, white spaces and starts always with lower case

        _logger.LogInformation($"Adding words {stimulusWord}-{responseWord}");
        try
        {
            await _gameService.AddAssociation(gameSessionId, stimulusWord, responseWord, cancellationToken: default);
            string stimulusWordEvent = $"{stimulusWord}-{responseWord}.CreatedEvent";
            return stimulusWordEvent;
        }
        catch
        {
            string errorEvent = $"{stimulusWord}-{responseWord}.ErrorEvent";
            return errorEvent;
        }
    }
}
