namespace Russkyc.DependencyInjection.Demo.Services.Interfaces;

public interface ILogger
{
    void setName(string name);
    void Log(string message);
}