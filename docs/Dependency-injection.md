# Dependency injection

To dependency injection is used [IServiceCollection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-usage) from Microsoft.

Example of use

```c#

IFionaHostBuilder serviceBuilder = FionaHostBuilder.CreateHostBuilder();

serviceBuilder.Service.AddSingleton(CallMock);
serviceBuilder.Service.AddTransient<IInterface, Impl>();
```