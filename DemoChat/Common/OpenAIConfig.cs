using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChat.Common;
public class OpenAIConfig
{
    public string ChatModelId { get; set; } = null!;
    public string EmbeddingModelId { get; set; } = null!;
    public string AudioToTextModelId { get; set; } = null!;
    public string TextToAudioModelId { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
}
