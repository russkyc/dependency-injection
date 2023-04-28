namespace DependencyInjectionDemo.Interfaces;

public interface ILogger
{
    void setName(string name);
    void Log(string message);
}