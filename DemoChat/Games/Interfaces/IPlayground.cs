using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChat.Games.Interfaces;
public interface IPlayground
{
    Task StartChat(CancellationToken stoppingToken);
}
