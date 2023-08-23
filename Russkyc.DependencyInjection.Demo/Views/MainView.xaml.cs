
using DependencyInjectionDemo.Interfaces;
using Russkyc.DependencyInjection.Attributes;

namespace DependencyInjectionDemo.Views;

// Transient service by default, register as self by default
[Service]
public partial class MainView
{
    // Gets Resolved Automatically
    public MainView(IMainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}