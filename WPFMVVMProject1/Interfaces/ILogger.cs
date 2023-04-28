namespace WPFMVVMProject1.Interfaces;

public interface ILogger
{
    void setName(string name);
    void Log(string message);
}