using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DemoChat.Hume.Models.Dtos;
public class JobDto
{
    [JsonPropertyName("job_id")]
    public string JobId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }
}
