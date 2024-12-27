namespace MePoC.ServiceRegistration.Models;
public class OpenAIConfig
{
    public string WordModelId { get; set; } = null!;
    public string ChatModelId { get; set; } = null!;
    public string EmbeddingModelId { get; set; } = null!;
    public string AudioToTextModelId { get; set; } = null!;
    public string TextToAudioModelId { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
}