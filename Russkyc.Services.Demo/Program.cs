using Russkyc.Services.Demo.Entities;
using Russkyc.Services.Demo.Interfaces;
using Russkyc.Services.Entities;
using Russkyc.Services.Services;

public static class Program
{
    public static void Main(string[] args)
    {
        var injector = Injector.GetInstance();
        injector.Use(new DependencyContainer());
        injector.Register<ILogger,ConsoleLogger>();
        var container = injector.GetContainer();
        var logger = injector.Resolve<ILogger>();
        logger.Log("Hello");
    }
}