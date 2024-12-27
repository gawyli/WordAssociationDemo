# Getting Started
This project uses .NET 8

##  Download repository on local machine
- Clone git repo to your local station

## Open the solution
- If cloned, navigate to the folder with ```.csproj``` file in the root of the application
  ```.../MePoC/MePoc.csproj```

  ## Set up the environment
  - Navigate to ```appsettings.json``` and set up OpenAI section as follows
    - "ModelId": "gpt-3.5-turbo-0125", 
    - "ChatModelId": "xxx", <- text chat model (optional)
    - "EmbeddingModelId": "text-embedding-3-small",
    - "AudioToTextModelId": "whisper-1",
    - "TextToAudioModelId": "tts-1",
    - "ApiKey": "xxx"

  - Set up the sql server database to your needs
    - "SQLServer": "Server=.;Database={database_name};Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;TrustServerCertificate=True"
  ### Set up user secrets
  - Right click on the MePoC project in the Solution Explorer
  - On the list select ```Manage User Secrets``` and add
    - "OpenAI": {
      "ApiKey": "xxx" <- paste your API key
      }
      
## Run Migrations
- On the top left corner of the Visual Studio navigate to ```View/Other Windows/Package Manager Console```
- In the terminal type ```Update-Database```
  
## Run application
- Once migrations are set up and our appsettings are configured, run the app and have a play with generating and saving recipes.
