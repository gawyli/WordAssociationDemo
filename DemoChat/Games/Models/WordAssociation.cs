using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Common;

namespace DemoChat.Games.Models;
public class WordAssociation : BaseEntity
{
    public string ChatSessionId { get; set; } = null!;
    public DateTime Created { get; set; }
    public List<Pair> Pairs { get; set; } = new();

    public WordAssociation(string chatSessionId, DateTime created)
    {
        ChatSessionId = chatSessionId;
        Created = created;
    }

    public void AddPairs(Pair wordPair)
    {
        this.Pairs.Add(wordPair);
    }
}

public class Pair : BaseEntity
{
    public string WordAssociationId { get; set; } = null!;
    public string StimulusWord { get; set; } = null!;
    public string ResponseWord { get; set; } = null!;

    public Pair(string wordAssociationId, string stimulusWord, string responseWord)
    {
        WordAssociationId = wordAssociationId;
        StimulusWord = stimulusWord;
        ResponseWord = responseWord;
    }

}

//public class ResponseWord : BaseEntity
//{
//    public string WordsId { get; set; } = null!;
//    public string Word { get; set; } = null!;
//    public string? EmotionsId { get; set; }

//    // EF Core Nav Property
//    public virtual Words? Words { get; set; }

//    public ResponseWord(string wordsId, string word)
//    {
//        WordsId = wordsId;
//        Word = word;
//    }

//    public void SetEmotionsId(string emotionsId)
//    {
//        this.EmotionsId = emotionsId;
//    }
//}
