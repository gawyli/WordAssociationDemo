using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DemoChat.Hume.Models;
public class InferenceJobConfiguration
{
    [JsonPropertyName("models")]
    public ModelsConfiguration Models { get; set; }

    [JsonPropertyName("transcription")]
    public TranscriptionConfiguration Transcription { get; set; }

}

public class ModelsConfiguration
{
    [JsonPropertyName("burst")]
    public object Burst { get; set; } = new { };

    [JsonPropertyName("prosody")]
    public ProsodyConfiguration Prosody { get; set; }

    [JsonPropertyName("language")]
    public LanguageConfiguration Language { get; set; }

    [JsonPropertyName("ner")]
    public NerConfiguration Ner { get; set; }

    [JsonPropertyName("facemesh")]
    public object Facemesh { get; set; } = new { };

}

public class ProsodyConfiguration
{
    [JsonPropertyName("granularity")]
    public string Granularity { get; set; } // Allowed values: word, sentence, utterance, conversational_turn

    [JsonPropertyName("window")]
    public WindowConfiguration Window { get; set; }

    [JsonPropertyName("identify_speakers")]
    public bool? IdentifySpeakers { get; set; }
}

public class WindowConfiguration
{
    [JsonPropertyName("length")]
    public double? Length { get; set; } // Minimum 0.5, defaults to 4

    [JsonPropertyName("step")]
    public double? Step { get; set; }   // Minimum 0.5, defaults to 1
}

public class LanguageConfiguration
{
    [JsonPropertyName("granularity")]
    public string Granularity { get; set; } // Allowed values: word, sentence, utterance, conversational_turn

    [JsonPropertyName("identify_speakers")]
    public bool? IdentifySpeakers { get; set; }

    [JsonPropertyName("sentiment")]
    public object Sentiment { get; set; } = null;

    [JsonPropertyName("toxicity")]
    public object Toxicity { get; set; } = null;

}

public class NerConfiguration
{
    [JsonPropertyName("identify_speakers")]
    public bool? IdentifySpeakers { get; set; }
}

public class TranscriptionConfiguration
{
    [JsonPropertyName("identify_speakers")]
    public bool? IdentifySpeakers { get; set; }

    [JsonPropertyName("confidence_threshold")]
    public double? ConfidenceThreshold { get; set; } // 0 to 1, defaults to 0.5

    [JsonPropertyName("language")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // Ensure enum is serialized as a string
    public Language? Language { get; set; }
}

public enum Language
{
    [JsonPropertyName("zh")]
    Chinese,

    [JsonPropertyName("da")]
    Danish,

    [JsonPropertyName("nl")]
    Dutch,

    [JsonPropertyName("en")]
    en
}