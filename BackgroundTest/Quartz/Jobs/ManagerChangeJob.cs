using BackgroundTest.Services.ManagerChangeService;
using Quartz;
using System.Threading.Tasks;

[DisallowConcurrentExecution]
public class ManagerChangeJob : IJob {
    private readonly IManagerChangeService _managerChangeService;

    public ManagerChangeJob(IManagerChangeService managerChangeService) {
        _managerChangeService = managerChangeService;
    }

    public Task Execute(IJobExecutionContext context) {
        return _managerChangeService.ApplyChangesAsync();
    }
}