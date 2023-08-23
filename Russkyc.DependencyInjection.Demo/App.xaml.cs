using System.Reflection;
using DependencyInjectionDemo.Views;
using Russkyc.DependencyInjection.Implementations;

namespace DependencyInjectionDemo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    /// <summary>
    /// Application Entry for WPFMVVMProject1
    /// </summary>
    public App()
    {
        // Get Current Assembly
        var assembly = Assembly.GetExecutingAssembly();
        
        // Build Container
        var container = new ServicesContainer()
            // Add Services From Assembly
            .AddServicesFromAssembly(assembly);
        
        container.Resolve<MainView>().Show();
    }
}