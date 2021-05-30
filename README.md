# Quartz jobs with scoped context
When creating some scheduled jobs we need to think properly which type for services and context we should use.

Scheduled jobs should be singletones as their lifetime is the same as the lifetime of the entire application. Even if we configure a service with "IServiceCollection.AddHostedService", we should know the service will be configured as [singleton](https://source.dot.net/#Microsoft.Extensions.Hosting.Abstractions/ServiceCollectionHostedServiceExtensions.cs,44c8ac8d84044702).
But the context used there should not be singleton, but should have scoped lifetime.
In that case we should not use dependency injection mechanism, but should use scoped context inside methods in singleton services, which are injected into singleton jobs.

```c#
public class ManagerChangeService : IManagerChangeService
{
    private IServiceProvider _services { get; }

    public ManagerChangeService(
        IServiceProvider services)
    {
        _services = services;
    }

    public async Task ApplyChangesAsync()
    {
        using (var scope = _services.CreateScope())
        {
            var context = scope.ServiceProvider
                .GetRequiredService<MainDbContext>();

            // doing something
        }
    }
}
```

[reference](https://entityframeworkcore.com/knowledge-base/51939451/how-to-use-a-database-context-in-a-singleton-service-)

In that case we could avoid issues with data reader not being closed.


