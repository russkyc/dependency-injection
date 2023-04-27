
namespace Russkyc.Services.Tests;

public class ServiceTests
{
    private IContainer _container;
    void Init()
    {
        _container = new DependencyContainer();
    }

    [Fact]
    void CREATE_CONTAINER_RETURNS_CONTAINER_INSTANCE()
    {
        Init();
    }
}