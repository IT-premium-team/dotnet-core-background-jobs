using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

public class CustomJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public CustomJobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return _serviceProvider
            .GetRequiredService(bundle.JobDetail.JobType) as IJob;
    }

    public void ReturnJob(IJob job) {}
}