using Azure;
using MePoC.Chat;
using MePoC.DataBus;
using MePoC.Models;
using MePoC.Repository.Interfaces;
using MePoC.WordAssociation.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;

namespace MePoC.WordAssociation;

/// <summary>
/// Implementation class for Word Association Test
/// Session can only be modified within "Session" scope.
/// </summary>

public class WordAssociationTestService : IWordAssociationService
{
    private const char Space = (char)32;

    private readonly ILogger<WordAssociationTestService> _logger;
    private readonly IRepository _repository;
    private readonly IChatService _chatService;
    private readonly Kernel _kernel;

    public Session Session { get; set; } = null!;

    private Word? stimulusWord;
    private ChatHistory chatHistory;

    public WordAssociationTestService(ILogger<WordAssociationTestService> logger, [FromKeyedServices("G4T")] Kernel kernel,
        IRepository repository, IChatService chatService)
    {
        _logger = logger;
        _repository = repository;
        _kernel = kernel;
        _chatService = chatService;

        this.chatHistory = new ChatHistory();

    }

    private async Task TestInitialize(CancellationToken cancellationToken)
    {
        var sysPrompt = @"
You are AI Word Association Master and your role is to lead the word association test.
During the test you are allowed to write ONLY one word without any other information.

Your message ALWAYS should look like this example:
//Start example
apple
//End example

Apple is a stimulus word in the example.- Stimulus word is the word that you write.

Here are the rules you MUST follow:
1. You can ONLY write one word.
2. You ALWAYS start the test with the ONLY one stimulus word.
3. You ALWAYS select a stimulus word from the Stimulus word list provided to you.


Stimulus word list:
1. Apple
2. Banana
3. Carrot
4. Dog
5. Elephant
6. Frog
7. Night
8. Ocean
9. Ghost
10. House
";

        this.chatHistory.AddSystemMessage(sysPrompt);


        var id = await StartSession(cancellationToken);
        _logger.LogInformation($"Session started with id: {id}");
    }


    public async Task InitializeChat(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Chat");

        bool isSessionActive = true;
        string testId = string.Empty;

        TestDetails? test = null;

        this.chatHistory.AddSystemMessage("Nice short welcoming to the user");
        
        while (isSessionActive)
        {
            if (test == null || !test.IsTestInitialized || !test.IsTestRunning || test.IsTestCompleted)
            {
                await _chatService.GetAIMessageAsync(chatHistory, _kernel, cancellationToken).ConfigureAwait(true);

                if (String.IsNullOrEmpty(testId))
                {
                    var messages = chatHistory.ToList();
                    var toolMessages = messages.Where(m => m.Role == AuthorRole.Tool).ToList();

                    if (toolMessages.Count > 0)
                    {
                        var toolMessagesContent = toolMessages.FirstOrDefault()?.Content?.ToString();
                        var keyValuePair = JsonSerializer.Deserialize<KeyValuePair<string, string>>(toolMessagesContent!);
                        testId = !String.IsNullOrEmpty(keyValuePair!.Key) ? keyValuePair!.Value : string.Empty;

                    }
                }
            }

            if (!String.IsNullOrEmpty(testId))
            {
                test = _repository.GetByIdAsync<TestDetails>(testId, cancellationToken: default);
                if (test != null && test.IsTestInitialized && !test.IsTestRunning)
                {
                    test = await InitializeTestIfRequired(test, cancellationToken);
                }  
            }

            
            if (test != null && test.IsTestRunning && !test.IsTestCompleted)
            {
                var sWord = await _chatService.GetAIMessageAsync(chatHistory, _kernel, cancellationToken).ConfigureAwait(true);
                if (!sWord.Contains(Space)) 
                {
                    await AddStimulusWord(sWord, cancellationToken);
                }
                
            }

            var userMessage = await _chatService.GetUserMessageAsync(chatHistory, cancellationToken).ConfigureAwait(true);

            if (test != null && test.IsTestRunning && !test.IsTestCompleted)
            {
                if (!userMessage.Contains(Space))
                { 
                    await AddResponseWord(userMessage, cancellationToken); 
                }
            }
            else if (test != null && test.IsTestCompleted)
            {
                await StopSession(cancellationToken);
                testId = string.Empty;
                test = null;

            }

            if (userMessage.Contains("stop", StringComparison.OrdinalIgnoreCase))
            {
                isSessionActive = false;
            }
        }
    }

    private async Task<TestDetails> InitializeTestIfRequired(TestDetails? test, CancellationToken cancellationToken)
    {
        if (test != null && test.IsTestInitialized && !test.IsTestRunning)
        {
            await TestInitialize(cancellationToken);
            _logger.LogInformation("Test initialized");

            test.IsTestRunning = true;
            await _repository.UpdateAsync(test, cancellationToken: default);

            _logger.LogInformation("Test running");
        }

        return test!;
    }

    public async Task AddStimulusWord(string word, CancellationToken cancellationToken)
    {
        stimulusWord = new Word(word, WordType.Stimulus);

        var id = await _repository.AddAsync(stimulusWord, cancellationToken);
        _logger.LogInformation($"Stimulus word added: {id}");

        chatHistory.AddAssistantMessage($"{stimulusWord}");
    }

    public async Task AddResponseWord(string word, CancellationToken cancellationToken)
    {
        var responseWord = new Word(word, WordType.Response);

        var id = await _repository.AddAsync(responseWord, cancellationToken);
        _logger.LogInformation($"Response word added: {id}");

        await AddWordsPair(responseWord);

        chatHistory.AddUserMessage($"{responseWord}");
    }

    public async Task StopSession(CancellationToken cancellationToken)
    {
        if (this.Session == null)
        {
            _logger.LogError("Session is not started");
            throw new ArgumentNullException("Session is not started - DO NOT TRY THIS METHOD AGAIN!");
        }

        this.Session.SetStopTime();

        await _repository.UpdateAsync(this.Session, cancellationToken);
        _logger.LogInformation($"Session stopped with at {this.Session.StopTime.ToString("hh:ss tt")} Session Id: {this.Session.Id}");
    }

    private async Task<string> StartSession(CancellationToken cancellationToken)
    {
        var session = new Session(DateTime.UtcNow);

        this.Session = session;
        this.Session.SetStartTime();

        var id = await _repository.AddAsync(this.Session, cancellationToken);
        _logger.LogInformation($"Session started with id: {id}");

        return id;
    }

    private async Task AddWordsPair(Word responseWord)
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

    private bool GenereateTestVault(string testName)
    {

        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string subfolderName = "Responses";
        string sessionFolderName = $"test{testName}";

        // Build the full path to the subfolder
        string fullPath = Path.Combine(baseDirectory, subfolderName, sessionFolderName);

        if (!Directory.Exists(fullPath))
        {
            _logger.LogInformation($"Creating directory {fullPath}");
            Directory.CreateDirectory(fullPath);
        }

        return true;
    }

}


/*
 * 
 * 
 * 
 * public async void Planner()
    {
        var plannerOptions = new HandlebarsPlannerOptions()
        {
            // When using OpenAI models, we recommend using low values for temperature and top_p to minimize planner hallucinations.
            ExecutionSettings = new OpenAIPromptExecutionSettings()
            {
                Temperature = 0.0,
                TopP = 0.1,
            },
            // Use gpt-4 or newer models if you want to test with loops.
            // Older models like gpt-35-turbo are less recommended. They do handle loops but are more prone to syntax errors.
            AllowLoops = true
        };

        var initialArguments = new KernelArguments()
        {

        };

        var template = LoadTemplate();

        var plan = JsonSerializer.Deserialize<HandlebarsPlan>(template);
        if (plan == null)
        {
            _logger.LogError("Failed to deserialize plan");
            throw new ArgumentNullException("Failed to deserialize plan");
        }
        var planResult = await plan.InvokeAsync(kernel, initialArguments);


        var memory = new Memories(planResult);
        await _repository.AddAsync(memory, cancellationToken: default);
    }


    public Task TestFinalize(string sessionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private string LoadTemplate()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string subfolderName = "Templates";
        string fileName = $"wordAssociationPlanTemplate.txt";

        // Build the full path to the subfolder
        string fullPath = Path.Combine(baseDirectory, subfolderName, fileName);

        if (!File.Exists(fullPath))
        {
            _logger.LogError($"Template file not found at {fullPath}");
            return string.Empty;
        }

        var testTemplate = File.ReadAllText(fullPath);
        _logger.LogInformation($"Template loaded from {fullPath}");
        return testTemplate;
    }
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */