using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DependencyInjectionDemo.Interfaces;
using Russkyc.DependencyInjection.Attributes;
using Russkyc.DependencyInjection.Demo.Services.Interfaces;
using Russkyc.DependencyInjection.Enums;
using Russkyc.DependencyInjection.Interfaces;

namespace DependencyInjectionDemo.ViewModels;

[Service(Scope.Singleton, Registration.AsInterfaces)]
public partial class MainViewModel : ViewModelBase, IMainViewModel
{
    [ObservableProperty]
    private string? _welcomeMessage;
    private readonly ILogger _logger;
    private readonly IServicesContainer _container;

    public MainViewModel(
        ILogger logger,
        IServicesContainer container)
    {
        _logger = logger;
        _container = container;
        WelcomeMessage = "Welcome to your MVVM App!";
    }

    [RelayCommand]
    private void Log()
    {
        _logger.setName($"{GetType().Name}: ");
        _logger.Log(WelcomeMessage!);
    }
}