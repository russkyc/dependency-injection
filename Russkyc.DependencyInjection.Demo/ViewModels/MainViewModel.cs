using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DependencyInjectionDemo.Interfaces;
using DependencyInjectionDemo.Views;
using Russkyc.DependencyInjection.Interfaces;

namespace DependencyInjectionDemo.ViewModels;

public partial class MainViewModel : ViewModelBase, IMainViewModel
{
    [ObservableProperty]
    private string? _welcomeMessage;
    
    private readonly ILogger _logger;

    public MainViewModel(
        ILogger logger,
        IServicesContainer container)
    {
        _logger = logger;
        container.Resolve<SecondaryView>()
            .Show();
        WelcomeMessage = "Welcome to your MVVM App!";
    }

    [RelayCommand]
    private void Log()
    {
        _logger.setName($"{GetType().Name}: ");
        _logger.Log(WelcomeMessage!);
    }
}