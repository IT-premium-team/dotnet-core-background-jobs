using BackgroundTest.Data.Context;
using BackgroundTest.Services.ManagerChangeService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace BackgroundTest.Configuration.Services
{
    public static class ServiceExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IManagerChangeService, ManagerChangeService>();

            return services;
        }

        public static IServiceCollection ConfigureDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Connection");
            services.AddDbContext<MainDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            },
            ServiceLifetime.Scoped);

            return services;
        }

        public static IServiceCollection ConfigureJobs(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, CustomJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddSingleton<ManagerChangeJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(ManagerChangeJob),
                cronExpression: "0 0/15 * * * ?" // every 15 min
            ));

            services.AddHostedService<QuartzHostedService>();

            return services;
        }
    }
}
