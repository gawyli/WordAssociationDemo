# Getting Started
This project uses .NET 8

##  Download repository on local machine
- Clone git repo to your local station

## Open the solution
- If cloned, navigate to the folder with ```.sln``` solution ```WordsEmotions.sln```

  ## Set up the environment
  - Navigate to ```appsettings.json``` and set up OpenAI section as follows
    - "ChatModelId": "gpt-4o-mini", <- chat completion
    - "EmbeddingModelId": "text-embedding-3-small",
    - "AudioToTextModelId": "whisper-1",
    - "TextToAudioModelId": "tts-1",
    - "ApiKey": "xxx"

  - SQLite is already set up in ```appsettings.json```
    - "DemoRepository": "Data Source=demo.db"
  ### Set up user secrets
  - Right click on the ```DemoChat``` project in the Solution Explorer
  - On the options list select ```Manage User Secrets``` and add
    - "OpenAI": {
      "ApiKey": "xxx" <- paste your API key
      }
      
## Run Migrations
- On the top left corner of the Visual Studio navigate to ```View/Other Windows/Package Manager Console```
- In the terminal type ```Add-Migration InitialCreate``` (creates Migrations folder) then,
- Type ```Update-Database```
- This should create a ```demo.db``` file and apply migrations from ```Migrations``` folder
  
## Run application
- Once migrations are set up and our appsettings are configured, run the app and have a play word associaton with AI

## Controles
- When finish talk during the game, press spacebar to continue, send message to Assistatnt and wait for Assistant response
- To stop conversation simply say stop and press spacebar
