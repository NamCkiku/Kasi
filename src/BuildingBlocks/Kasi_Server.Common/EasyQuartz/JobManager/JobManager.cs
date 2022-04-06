using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasi_Server.Common.EasyQuartz
{
    public class JobManager : IJobManager
    {
        private readonly IJobFactory _jobFactory;
        private readonly ISchedulerFactory _schedulerFactory;
        public JobManager(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }
        public IScheduler Scheduler
        {
            get
            {
                return _schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            }
        }
        public async Task AddAsync<TJob>(string cron, string name, IDictionary<string, object> map = null) where TJob : IJob
            => await AddAsync(typeof(TJob), cron, name, map);
        public async Task AddAsync(Type jobType, string cron, string name, IDictionary<string, object> map = null)
        {
            var group = $"{jobType.FullName}.Group";

            var scheduler = Scheduler;
            scheduler.JobFactory = _jobFactory;

            var exist = await scheduler.CheckExists(new JobKey(name, group));

            if (exist) return;

            var job = JobBuilder.Create(jobType).WithIdentity(name, group)
                .WithDescription(jobType.Name)
                .UsingJobData("JobId", name)
                .Build();

            var trigger = TriggerBuilder
                .Create()
                .WithIdentity($"{name}.trigger", group)
                .WithCronSchedule(cron)
                .WithDescription(jobType.Name)
                .Build();

            if (map != null)
                foreach (var item in map)
                    job.JobDataMap.Add(item);

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
        public async Task<List<JobKey>> GetJobKeysAsync<TJob>() where TJob : IJob
            => await GetJobKeysAsync(typeof(TJob));
        public async Task<List<JobKey>> GetJobKeysAsync(Type jobType)
        {
            var group = $"{jobType.FullName}.Group";

            var jobKeys = await Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));

            return jobKeys.ToList();
        }
        public async Task<bool> ExistAsync<TJob>(string name) where TJob : IJob
            => await ExistAsync(typeof(TJob), name);
        public async Task<bool> ExistAsync(Type jobType, string name)
        {
            var group = $"{jobType.FullName}.Group";

            var exist = await Scheduler.CheckExists(new JobKey(name, group));

            return exist;
        }
        public async Task<bool> RemoveAllAsync<TJob>() where TJob : IJob
            => await RemoveAllAsync(typeof(TJob));
        public async Task<bool> RemoveAllAsync(Type jobType)
        {
            var group = $"{jobType.FullName}.Group";

            var jobKeys = await Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));

            return await Scheduler.DeleteJobs(jobKeys);
        }
        public async Task<bool> RemoveAsync<TJob>(string name) where TJob : IJob
            => await RemoveAsync(typeof(TJob), name);
        public async Task<bool> RemoveAsync(Type jobType, string name)
        {
            var group = $"{jobType.FullName}.Group";

            return await Scheduler.DeleteJob(new JobKey(name, group));
        }
        public async Task Pause<TJob>(string name) where TJob : IJob
            => await Pause(typeof(TJob), name);
        public async Task Pause(Type jobType, string name)
        {
            var group = $"{jobType.FullName}.Group";

            await Scheduler.PauseJob(new JobKey(name, group));
        }
        public async Task Pause<TJob>() where TJob : IJob
            => await Pause(typeof(TJob));
        public async Task Pause(Type jobType)
        {
            var group = $"{jobType.FullName}.Group";

            await Scheduler.PauseJobs(GroupMatcher<JobKey>.GroupEquals(group));
        }
        public async Task Resume<TJob>(string name) where TJob : IJob
            => await Resume(typeof(TJob), name);
        public async Task Resume(Type jobType, string name)
        {
            var group = $"{jobType.FullName}.Group";

            await Scheduler.ResumeJob(new JobKey(name, group));
        }
        public async Task Resume<TJob>() where TJob : IJob
            => await Resume(typeof(TJob));
        public async Task Resume(Type jobType)
        {
            var group = $"{jobType.FullName}.Group";

            await Scheduler.ResumeJobs(GroupMatcher<JobKey>.GroupEquals(group));
        }
        public async Task Clear()
        {
            await Scheduler.Clear();
        }
        public async Task Operate<TJob>(OperateEnum operate, string name) where TJob : IJob
            => await Operate(typeof(TJob), operate, name);
        public async Task Operate(Type jobType, OperateEnum operate, string name)
        {
            var group = $"{jobType.FullName}.Group";

            switch (operate)
            {
                case OperateEnum.Delete:
                    await Scheduler.DeleteJob(new JobKey(name, group));
                    break;
                case OperateEnum.Pause:
                    await Scheduler.PauseJob(new JobKey(name, group));
                    break;
                case OperateEnum.Resume:
                    await Scheduler.ResumeJob(new JobKey(name, group));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operate), operate, null);
            }
        }
    }
}
