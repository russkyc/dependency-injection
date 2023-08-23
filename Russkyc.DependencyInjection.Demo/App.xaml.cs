using System.Reflection;
using DependencyInjectionDemo.Views;
using Russkyc.DependencyInjection.Helpers;
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
        var assembly2 = Assembly.Load("TestNewAssembly");
        
        // Build Container
        var container = new ServicesCollection()
            // Add Services From Assembly
            .AddServicesFromAssembly(assembly2)
            .AddServicesFromAssembly(assembly)
            .Build();
        
        container.Resolve<MainView>().Show();
        container.Resolve<MainView>().Show();
    }
}