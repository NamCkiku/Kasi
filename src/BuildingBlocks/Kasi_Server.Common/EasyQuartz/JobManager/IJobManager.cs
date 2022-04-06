using Quartz;

namespace Kasi_Server.Common.EasyQuartz
{
    public interface IJobManager
    {
        IScheduler Scheduler { get; }

        Task AddAsync<TJob>(string cron, string name, IDictionary<string, object> map = null) where TJob : IJob;

        Task AddAsync(Type jobType, string cron, string name, IDictionary<string, object> map = null);

        Task<List<JobKey>> GetJobKeysAsync<TJob>() where TJob : IJob;

        Task<List<JobKey>> GetJobKeysAsync(Type jobType);

        Task<bool> ExistAsync<TJob>(string name) where TJob : IJob;

        Task<bool> ExistAsync(Type jobType, string name);

        Task<bool> RemoveAllAsync<TJob>() where TJob : IJob;

        Task<bool> RemoveAllAsync(Type jobType);

        Task<bool> RemoveAsync<TJob>(string name) where TJob : IJob;

        Task<bool> RemoveAsync(Type jobType, string name);

        Task Pause<TJob>(string name) where TJob : IJob;

        Task Pause(Type jobType, string name);

        Task Pause<TJob>() where TJob : IJob;

        Task Pause(Type jobType);

        Task Resume<TJob>(string name) where TJob : IJob;

        Task Resume(Type jobType, string name);

        Task Resume<TJob>() where TJob : IJob;

        Task Resume(Type jobType);

        Task Clear();

        Task Operate<TJob>(OperateEnum operate, string name) where TJob : IJob;

        Task Operate(Type jobType, OperateEnum operate, string name);
    }
}