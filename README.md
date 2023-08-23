<h2 align="center">Russkyc.DependencyInjection</h2>

<p align="center">
    <a href="https://www.nuget.org/packages/Russkyc.DependencyInjection">
        <img src="https://img.shields.io/nuget/v/Russkyc.DependencyInjection?color=1f72de" alt="Nuget">
    </a>
    <a href="#">
        <img src="https://img.shields.io/badge/-.NET%202.0-blueviolet?color=1f72de&label=NET" alt="">
    </a>
</p>

<p align="center">
<a href="https://www.nuget.org/packages/Russkyc.DependencyInjection">Russkyc.DependencyInjection</a> is a fast and minimal dependency injection container with auto dependency resolving.
</p>

---

## Basic Setup

### 1. Service Registration

Services are automatically registered to the container using the `[Service]` attribute and are injected using constructor injection.

Default
```csharp
[Service]
public class ConsoleLogger : ILogger
{
    private string? _name;

    public void setName(string name)
    {
        _name = name;
    }

    public void Log(string message)
    {
        Console.WriteLine(_name + message);
        Debug.WriteLine(_name + message);
    }
}
```

With defined scope and registration conditions

```csharp
[Service(Scope.Transient, Registration.AsInterfaces)]
public partial class MainViewModel : ViewModelBase, IMainViewModel
{
    private readonly ILogger _logger;

    public MainViewModel( ILogger logger)
    {
        _logger = logger;
        _logger.Log("I am an injected service!");
    }
}
```

> **NOTE:** The scope and registration options are optional, each can be set if needed. By default the scope is _Scope.Tranient_ and the registration option is _Registration.AsSelf_


### 2. Setup and Run in Application Entry
Use the `AddServices()` to resolve the services from the current assembly.
```csharp
// Build Container
var container = new ServicesCollection()
    .AddServices() // Add Services From Entry Assembly
    .AddServicesFromReferenceAssemblies() // Add Services From External Referenced Assemblies (Eg; Project References)
    .Build();

// You are now Ready to Go!
var window = container.Resolve<MainView>();
window.Show();
```

#### Working with External/Referenced Assemblies
If you want to work with specific projects/assemblies you can also use one of these methods before build:
- Specific assembly: `.AddServicesFromAssembly(assembly)`
- External assembly references of specific assembly: `.AddServicesFromReferenceAssemblies(assembly)`

```csharp
// Build Container

var assembly = Assembly.Load("AssemblyName"); // Specific Assembly

var container = new ServicesCollection()
    .AddServicesFromAssembly(assembly) // Add Services From Assembly
    .AddServicesFromAssemblyReferences(assembly) // Add Services From External Referenced Assemblies
    .Build();
```
---

## Manual Setup

You can also register dependencies manually if needed, this also works alongside the default setup.
### 1. Creating the Container

```csharp
var services = new ServicesCollection()
            .AddSingleton<ILogger, ConsoleLogger>()
            .AddSingleton<IMainViewModel, MainViewModel>()
            .AddTransient<MainView>()
            .Build();
```

### 2. Resolving

```csharp
var window = services.Resolve<MainView>();
window.Show();
```

> **NOTE:** No Attributes are required in manual setup. Registration is done manually using the `AddSingleton` and `AddTransient` methods

---

## Auto-resolved dependencies
Dependency auto-wiring is done through constructor injection. Here's what that looks like with a sample ViewModel with injected `ILogger` service
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

---

## Accessing the container in other parts of the application

The container is automatically injected and can be accessed using a constructor injection of an `IServicesContainer`

```csharp
public MainViewModel(IServicesContainer container)
{
    // Using the injected container
    var logger = container.Resolve<ILogger>();
}
```

---

## Sponsors
Special thanks to [JetBrains](https://www.jetbrains.com/) for supporting this project by providing licences to the JetBrains Suite!

<a href="https://www.jetbrains.com/community/opensource/#support">
<img width="200px" src="https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.png" alt="JetBrains Logo (Main) logo.">
</a>