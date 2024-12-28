using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChat.Emotions.Interfaces;
public interface IEmotionsService
{
    Task<string> CreateEmotionsSession(string gameSessionId, CancellationToken cancellationToken);
    Task<string> CreateEmotionsJobInference(string emotionsSessionId, CancellationToken cancellationToken);
    Task<string> GetEmotionsJobStatus(string jobId, CancellationToken cancellationToken);
}
