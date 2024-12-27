using System.Text.Json.Serialization;

namespace DemoChat.Hume.Models;

public class InferenceSourcePredictResult
{
    [JsonPropertyName("source")]
    public Source Source { get; set; } = null!;

    [JsonPropertyName("results")]
    public Results? Results { get; set; }
}

public class Results
{
    [JsonPropertyName("predictions")]
    public ResultsPrediction[] Predictions { get; set; } = null!;

    [JsonPropertyName("errors")]
    public string[] Errors { get; set; } = null!;
}

public class ResultsPrediction
{
    [JsonPropertyName("file")]
    public string File { get; set; } = null!;

    [JsonPropertyName("models")]
    public Models Models { get; set; } = null!;
}

public class Models
{
    [JsonPropertyName("burst")]
    public Metrics? Burst { get; set; }

    [JsonPropertyName("prosody")]
    public Metrics? Prosody { get; set; }

    [JsonPropertyName("language")]
    public Metrics? Language { get; set; }

    [JsonPropertyName("ner")]
    public Metrics? Ner { get; set; }
}

public class Metrics
{
    [JsonPropertyName("grouped_predictions")]
    public GroupedPrediction[] GroupedPredictions { get; set; } = null!;

    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; set; }

}

public class GroupedPrediction
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("predictions")]
    public GroupedPredictionPrediction[] Predictions { get; set; } = null!;
}

public class GroupedPredictionPrediction
{
    [JsonPropertyName("entity")]
    public string? Entity { get; set; }

    [JsonPropertyName("entity_confidence")]
    public double? EntityConfidence { get; set; }

    [JsonPropertyName("support")]
    public double? Support { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("time")]
    public Time? Time { get; set; }

    [JsonPropertyName("confidence")]
    public double? Confidence { get; set; }

    [JsonPropertyName("emotions")]
    public Emotion[]? Emotions { get; set; }

    [JsonPropertyName("sentiment")]
    public Description? Sentiment { get; set; }

    [JsonPropertyName("toxicity")]
    public Description? Toxicity { get; set; }

    [JsonPropertyName("position")]
    public Position? Position { get; set; }

    [JsonPropertyName("speaker_confidence")]
    public double? SpeakerConfidence { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    [JsonPropertyName("link_word")]
    public string? LinkWord { get; set; }

}

public class Description
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("score")]
    public string Score { get; set; } = null!;
}

public class Emotion
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("score")]
    public double Score { get; set; }
}

public class Position
{
    [JsonPropertyName("begin")]
    public int Begin { get; set; }

    [JsonPropertyName("end")]
    public int End { get; set; }
}


public class Time
{
    [JsonPropertyName("begin")]
    public double Begin { get; set; }

    [JsonPropertyName("end")]
    public double End { get; set; }
}

public class Metadata
{
    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }

    [JsonPropertyName("detected_language")]
    public string? DetectedLanguage { get; set; }
}

public class Source
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("filename")]
    public string? Filename { get; set; }

    [JsonPropertyName("content_type")]
    public string? ContentType { get; set; }

    [JsonPropertyName("md5sum")]
    public string? Md5Sum { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

