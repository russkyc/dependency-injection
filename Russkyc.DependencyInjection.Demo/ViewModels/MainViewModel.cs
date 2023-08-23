using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DependencyInjectionDemo.Interfaces;
using DependencyInjectionDemo.Views;
using Russkyc.DependencyInjection.Attributes;
using Russkyc.DependencyInjection.Enums;
using Russkyc.DependencyInjection.Interfaces;

namespace DependencyInjectionDemo.ViewModels;

[Service(Scope.Transient, Registration.AsInterfaces)]
public partial class MainViewModel : ViewModelBase, IMainViewModel
{
    [ObservableProperty]
    private string? _welcomeMessage;
    
    private readonly ILogger _logger;

    public MainViewModel(
        ILogger logger,
        // Inject container
        IServicesContainer container)
    {
        // Use
        container.Resolve<SecondaryView>().Show();
        
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