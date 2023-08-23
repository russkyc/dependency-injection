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
        // Build Container
        var container = new ServicesCollection()
            .AddServices() // Add Services From Entry Assembly
            .AddServicesFromReferenceAssemblies() // Add Services From External Referenced Assemblies
            .Build();
        
        container.Resolve<MainView>().Show();
        container.Resolve<MainView>().Show();
    }
}