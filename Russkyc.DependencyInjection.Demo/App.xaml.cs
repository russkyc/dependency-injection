using DependencyInjectionDemo.Views;
using Russkyc.DependencyInjection.Implementations;

namespace DependencyInjectionDemo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    /// <summary>
    /// Application Entry
    /// </summary>
    public App()
    {
        // Hosting setup
        var host = ApplicationHost<MainView>.CreateDefault();
        host.Root.Show();
    }
}