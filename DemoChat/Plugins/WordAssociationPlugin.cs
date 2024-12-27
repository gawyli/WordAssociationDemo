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

namespace DemoChat.Plugins;
public class WordAssociationPlugin
{
    private readonly ILogger<WordAssociationPlugin> _logger;
    private readonly IRepository _repository;

    public WordAssociationPlugin(ILogger<WordAssociationPlugin> logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [KernelFunction("add_words")]
    [Description("Add a stimulus and a user response words to database")]
    [return: Description("A string event")]
    public async Task<string> AddWords(
        [Description("Enter GameSessionId:")] string gameSessionId,
        [Description("Enter a stimulus word")] string stimulusWord,
        [Description("Enter a user response word")] string responseWord)
    {
        // Preprocess words so it does not include dots, white spaces and starts always with lower case

        _logger.LogInformation($"Adding words {stimulusWord}-{responseWord}");

        var pair = new Pair(gameSessionId, stimulusWord, responseWord);

        await _repository.AddAsync(pair, cancellationToken: default);

        string stimulusWordEvent = $"{stimulusWord}{responseWord}CreatedEvent";
        return stimulusWordEvent;
    }

    //[KernelFunction("add_stimulus_word")]
    //[Description("Add your stimulus word to database")]
    //[return: Description("A string event")]
    //public async Task<string> AddStimulusWord(
    //    [Description("Enter GameSessionId:")] string gameSessionId, 
    //    [Description("Enter your stimulus word")]string stimulusWord)
    //{
    //    // Preprocess word so it does not include dots, white spaces and starts always with lower case

    //    _logger.LogInformation($"Adding stimulus word {stimulusWord}");

    //    var wordAssociation = new StimulusWord()

    //    //wordAssociation = await _repository.AddAsync(wordAssociation, cancellationToken: default);

    //    string stimulusWordEvent = $"{stimulusWord}CreatedEvent";

    //    return stimulusWordEvent;
    //}

    //[KernelFunction("add_response_word")]
    //[Description("Add a user response word to database")]
    //[return: Description("A string event")]
    //public async Task<string> AddResponseWord(
    //    [Description("Enter GameSessionId:")] string gameSessionId, 
    //    [Description("Enter user response word")] string responseWord)
    //{
    //    // Preprocess word so it does not include dots, white spaces and starts always with lower case

    //    _logger.LogInformation($"Adding response word {responseWord}");

    //    //var wordAssociation = new WordAssociation(chatSessionId, gameChatHistoryJson, DateTime.UtcNow);

    //    //wordAssociation = await _repository.AddAsync(wordAssociation, cancellationToken: default);

    //    string responsesWordEvent = $"{responseWord}CreatedEvent";

    //    return responsesWordEvent;
    //}
}
