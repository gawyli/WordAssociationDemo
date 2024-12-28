using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Common;

namespace DemoChat.Games.Models;
public class GameSession : BaseEntity
{
    public string ChatSessionId { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime? Ended { get; set; }
    public List<Association> Associations { get; set; } = new();

    public GameSession(string chatSessionId, DateTime created)
    {
        this.ChatSessionId = chatSessionId;
        this.Created = created;
    }

    public void AddAssociation(string stimulus, string response)
    {
        // TODO: Check this idea
        var association = new Association(this.Id, stimulus, response);
        this.Associations.Add(association);
    }
    // TODO: vs
    public void AddAssociation(Association association)
    {
        // TODO: this or both?
        this.Associations.Add(association);
    }

    public void SetEndedNow()
    {
        this.Ended = DateTime.UtcNow;
    }
}
