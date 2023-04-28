
using DependencyInjectionDemo.Interfaces;

namespace DependencyInjectionDemo.Views;

public partial class MainView
{
    
    // Gets Resolved Automatically
    public MainView(IMainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}