using DependencyInjectionDemo.Services;
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
        BuilderServices.BuildWithContainer(BuildContainer.ConfigureServices());
        BuilderServices.Resolve<MainView>().Show();
    }
}