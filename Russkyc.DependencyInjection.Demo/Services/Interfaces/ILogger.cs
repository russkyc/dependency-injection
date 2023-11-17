namespace DependencyInjectionDemo.Services.Interfaces;

public interface ILogger
{
    void SetName(string name);
    void Log(string message);
}