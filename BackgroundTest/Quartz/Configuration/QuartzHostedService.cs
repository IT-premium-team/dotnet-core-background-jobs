using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

public class QuartzHostedService : IHostedService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IJobFactory _jobFactory;
    private readonly IEnumerable<JobSchedule> _jobSchedules;

    public IScheduler Scheduler { get; set; }

    public QuartzHostedService(
        ISchedulerFactory schedulerFactory,
        IJobFactory jobFactory,
        IEnumerable<JobSchedule> jobSchedules)
    {
        _schedulerFactory = schedulerFactory;
        _jobFactory = jobFactory;
        _jobSchedules = jobSchedules;
    }

    public async Task StartAsync(CancellationToken token)
    {
        Scheduler = await _schedulerFactory.GetScheduler(token);
        Scheduler.JobFactory = _jobFactory;

        foreach (var schedule in _jobSchedules)
        {
            var job = CreateJob(schedule);
            var trigger = CreateTrigger(schedule);

            await Scheduler.ScheduleJob(job, trigger, token);
        }

        await Scheduler.Start(token);
    }

    public async Task StopAsync(CancellationToken token)
    {
        if (Scheduler != default) {
            await Scheduler.Shutdown(token);
        }
    }

    private static IJobDetail CreateJob(JobSchedule schedule)
    {
        var jobType = schedule.JobType;
        return JobBuilder
            .Create(jobType)
            .WithIdentity(jobType.FullName)
            .WithDescription(jobType.Name)
            .Build();
    }

    private static ITrigger CreateTrigger(JobSchedule schedule)
    {
        var jobType = schedule.JobType;
        return TriggerBuilder
            .Create()
            .WithIdentity($"{jobType.FullName}.trigger")
            .WithDescription(schedule.CronExpression)
            .WithCronSchedule(schedule.CronExpression)
            .Build();
    }
}