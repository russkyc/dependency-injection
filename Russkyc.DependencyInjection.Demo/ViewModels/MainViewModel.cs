using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DependencyInjectionDemo.Interfaces;

namespace DependencyInjectionDemo.ViewModels;

public partial class MainViewModel : ViewModelBase, IMainViewModel
{
    [ObservableProperty]
    private string? _welcomeMessage;
    
    private readonly ILogger _logger;

    public MainViewModel(ILogger logger)
    {
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