using Russkyc.DependencyInjection.Attributes;
using Russkyc.DependencyInjection.Enums;

namespace TestNewAssembly;

[Service(Scope.Singleton, Registration.AsInterfaces)]
public class Class1 : IClass1
{
    public void SayHellow()
    {
        Console.WriteLine("Hellow");
    }
}