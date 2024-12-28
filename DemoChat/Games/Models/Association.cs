using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Common;

namespace DemoChat.Games.Models;
public class Association : BaseEntity
{
    // To think about it
    class Invariants
    {
        public static int MaxLength = 10;
    }

    public string GameSessionId { get; set; } = null!;
    public string Stimulus { get; set; } = null!;
    public string Response { get; set; } = null!;

    public Association(string gameSessionId, string stimulus, string response)
    {
        GameSessionId = gameSessionId;
        Stimulus = stimulus;
        Response = response;
    }
}
