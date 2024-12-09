using Quartz.Impl;
using Quartz.Spi;
using Quartz;

namespace BillingApplication.Server.Quartz
{
    public static class DataScheduler
    {

        public static async Task Start(IServiceProvider serviceProvider)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = serviceProvider.GetService<JobFactory>()!;
            await scheduler.Start();

            IJobDetail jobDetailBilling = JobBuilder.Create<BillingJob>().Build();
            ITrigger triggerBilling = TriggerBuilder.Create()
                .WithIdentity("BillingTrigger", "default")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInHours(24)
                //.WithIntervalInMinutes(1) для тестирования
                .RepeatForever())
                .Build();

            IJobDetail jobDetailUserActions = JobBuilder.Create<UserActionsJob>().Build();
            ITrigger triggerUserActions = TriggerBuilder.Create()
                .WithIdentity("UserActionsTrigger", "default")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInHours(24)
                //.WithIntervalInMinutes(1) для тестирования
                .RepeatForever())
                .Build();


            await scheduler.ScheduleJob(jobDetailBilling, triggerBilling);
            await scheduler.ScheduleJob(jobDetailUserActions, triggerUserActions);
        }
    }
}
