# Russkyc.DependencyInjection

A bare-bones dependency injection container with auto dependency resolving.

## Usage

### 1. Inline Container Creation

```csharp
// Create container
var services = new ServicesContainer()
            .AddSingleton<ILogger, ConsoleLogger>()
            .AddSingleton<IMainViewModel, MainViewModel>()
            .AddTransient<MainView>();

// Build container
BuilderServices.BuildWithContainer(services);
// Resolve service (auto resolves constructor dependencies
BuilderServices.Resolve<MainView>().Show();
```

### 2. Separate container builder (Cleaner code)

1. Setup
```csharp
// Container builder
public static class BuildContainer
{
    // Returns container with registered dependencies
    public static IServicesContainer ConfigureServices()
    {
        return new ServicesCollection()
            .AddSingleton<ILogger,ConsoleLogger>()
            .AddSingleton<IMainViewModel,MainViewModel>()
            .AddTransient<MainView>()
            .Build();
    }
}
```
2. Usage

```csharp
BuilderServices.BuildWithContainer(BuildContainer.ConfigureServices());
BuilderServices.Resolve<MainView>().Show();
```

## Autowire dependencies
Dependency autowiring is done through constructor injection. Here's what that looks like:

1. Setup

```csharp
public class MainViewModel : IMainViewModel
{
    private readonly ILogger _logger;
    
    // Constructor injection of logger
    public MainViewModel(ILogger logger)
    {
        _logger = logger;
    }
}
```

2. Container Setup

```csharp
public static class BuildContainer
{
    // Returns container with registered dependencies
    public static IServicesContainer ConfigureServices()
    {
        // Order is important.
        // dependencies to be injected should be registered first
        return new ServicesContainer()
            // Register Logger
            .AddSingleton<ILogger,ConsoleLogger>()
            // Register ViewModel
            .AddSingleton<IMainViewModel,MainViewModel>()
            .AddTransient<MainView>();
    }
}
```
