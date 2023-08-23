using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DependencyInjectionDemo.Interfaces;
using Russkyc.DependencyInjection.Attributes;
using Russkyc.DependencyInjection.Enums;
using TestNewAssembly;

namespace DependencyInjectionDemo.ViewModels;

[Service(Scope.Singleton, Registration.AsInterfaces)]
public partial class MainViewModel : ViewModelBase, IMainViewModel
{
    [ObservableProperty]
    private string? _welcomeMessage;
    
    private readonly ILogger _logger;

    public MainViewModel(
        ILogger logger,
        IClass1 class1)
    {
        var t = class1;
        _logger = logger;
        WelcomeMessage = "Welcome to your MVVM App!";
    }

    [RelayCommand]
    private void Log()
    {
        _logger.setName($"{GetType().Name}: ");
        _logger.Log(WelcomeMessage!);
    }
}