using MePoC.DataBus;
using MePoC.Models;
using MePoC.Repository.Interfaces;
using MePoC.WordAssociation.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.ComponentModel;

namespace MePoC.Plugins.WordAssociationPlugin;

// Base Test?
public class WordAssociationTestPlugin
{
    private readonly ILogger<WordAssociationTestPlugin> _logger;
    private readonly IRepository _repository;
    private readonly ChatHistory _thread;

    public Session? Session { get; set; }

    public WordAssociationTestPlugin(ILogger<WordAssociationTestPlugin> logger, IRepository repository, ChatHistory thread)
    {
        _logger = logger;
        _repository = repository;

        _thread = thread;
    }

    [KernelFunction]
    [Description("Starts the Word Association Test")]
    public async Task<KeyValuePair<string, string>> StartWordAssociationTest()
    {
        Console.WriteLine("Starting Word Association Test");
        _logger.LogInformation("Starting Word Association Test");

        var test = new TestDetails(true, false, false);

        test.Id = await _repository.AddAsync(test, cancellationToken: default);
        _repository.DetachModel(test);

        return new KeyValuePair<string, string>("test", test.Id);
    }


    [KernelFunction]
    [Description("Completes the Word Association Test")]
    public async Task CompleteWordAssociationTest([Description("Id of the test to complete")] string testId)
    {
        var messages = _thread.ToList();
        var toolMessages = messages.Where(m => m.Role == AuthorRole.System).ToList();

        if (toolMessages.Count > 0)
            testId = toolMessages.FirstOrDefault()!.Content!.ToString()!;

        var test = _repository.GetByIdAsync<TestDetails>(testId, cancellationToken: default);
        if (test != null && test.IsTestInitialized && test.IsTestRunning)
        {
            test.IsTestCompleted = true;
            await _repository.UpdateAsync(test, cancellationToken: default);
            _repository.DetachModel(test);


            Console.WriteLine("Completed Word Association Test");
            _logger.LogInformation("Completed Word Association Test");
        } 
    }

}


/*
 * 
 * [KernelFunction]
    [Description("Add Stimulus Word")]
    public async Task AddStimulusWord(string word, CancellationToken cancellationToken)
    {
        stimulusWord = new Word(word, WordType.Stimulus);

        var id = await _repository.AddAsync(stimulusWord, cancellationToken);
        _logger.LogInformation($"Stimulus word added: {id}");

        _chats.AddAssistantMessage($"{stimulusWord}");
    }

    [KernelFunction]
    [Description("Add Response Word")]
    public async Task AddResponseWord(string word, CancellationToken cancellationToken)
    { 
        responseWord = new Word(word, WordType.Response);

        var id = await _repository.AddAsync(responseWord, cancellationToken);
        _logger.LogInformation($"Response word added: {id}");

        await AddWordsPair();

        _chats.AddUserMessage($"{responseWord}");
    }

    [KernelFunction]
    [Description("Stops session for Word Association Test")]
    public async Task StopSession()
    {
        if (this.Session == null)
        {
            _logger.LogError("Session is not started");
            throw new ArgumentNullException("Session is not started - DO NOT TRY THIS METHOD AGAIN!");
        }

        this.Session.SetStopTime();

        await _repository.UpdateAsync(this.Session, cancellationToken: default);
        _logger.LogInformation($"Session stopped with at {this.Session.StopTime.ToString("hh:ss tt")} Session Id: {this.Session.Id}");

        //await _wordService.TestFinalize(cancellationToken: default);
    }

    //[KernelFunction]
    //[Description("Starts Session for Word Association Test")]
    private async Task<string> StartSession()
    {
        var session = new Session(DateTime.UtcNow);

        this.Session = session;
        this.Session.SetStartTime();

        var id = await _repository.AddAsync(this.Session, cancellationToken: default);
        _logger.LogInformation($"Session started with id: {id}");

        return id;
    }

    //[KernelFunction]
    //[Description("Add Words pair to the Session")]
    private async Task AddWordsPair()
    {
        if (stimulusWord != null && responseWord != null && this.Session != null)
        {
            var wordsPair = new WordsPair(this.Session.Id, stimulusWord, responseWord, DateTime.UtcNow);
            var id = await _repository.AddAsync(wordsPair, cancellationToken: default);
            _logger.LogInformation($"Words pair added {id}");


            this.Session!.AddWordsPair(wordsPair);
            await _repository.UpdateAsync(this.Session, cancellationToken: default);
            _logger.LogInformation($"Words pair added to session {this.Session.Id}");
        }

    }
 * 
 * 
 */