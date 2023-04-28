using Russkyc.DependencyInjection.Implementations;
using Russkyc.DependencyInjection.Interfaces;
using WPFMVVMProject1.Interfaces;
using WPFMVVMProject1.Models;
using WPFMVVMProject1.ViewModels;
using WPFMVVMProject1.Views;

namespace WPFMVVMProject1.Services;

public static class BuildContainer
{
    public static IServicesContainer ConfigureServices()
    {
        return new ServicesCollection()
            .AddSingleton<ILogger,ConsoleLogger>()
            .AddSingleton<IMainViewModel,MainViewModel>()
            .AddTransient<MainView>()
            .Build();
    }
}