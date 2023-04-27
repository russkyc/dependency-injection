using Russkyc.Services.Demo.Interfaces;

namespace Russkyc.Services.Demo.Entities;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}