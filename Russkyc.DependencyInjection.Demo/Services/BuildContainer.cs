using DependencyInjectionDemo.Interfaces;
using DependencyInjectionDemo.Models;
using DependencyInjectionDemo.ViewModels;
using DependencyInjectionDemo.Views;
using Russkyc.DependencyInjection.Implementations;
using Russkyc.DependencyInjection.Interfaces;

namespace DependencyInjectionDemo.Services;

public static class BuildContainer
{
    public static IServicesContainer ConfigureServices()
    {
        return new ServicesContainer()
            .AddSingleton<ILogger,ConsoleLogger>()
            .AddSingleton<IMainViewModel,MainViewModel>()
            .AddTransient<MainView>();
    }
}