using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class SchedulerHelper
    {
        public void AddSchedulerQueue(TasksSchedulerPM task)
        {
            var queueservice = new DbQueueService();
            if (task.NextRunTime < DateTime.Now)
            {
                var NewNextRunTime = new DateTime(task.NextRunTime.Value.Year, task.NextRunTime.Value.Month, DateTime.Now.Day, task.NextRunTime.Value.Hour, task.NextRunTime.Value.Minute, task.NextRunTime.Value.Second);
                var NewNextRunTimeUTC = new DateTime(task.NextRunTimeUTC.Value.Year, task.NextRunTimeUTC.Value.Month, DateTime.Now.Day, task.NextRunTimeUTC.Value.Hour, task.NextRunTimeUTC.Value.Minute, task.NextRunTimeUTC.Value.Second);
                task.NextRunTime = NewNextRunTime;
                task.NextRunTimeUTC = NewNextRunTimeUTC; 
            }
            var TodayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, task.NextRunTime.Value.Hour, task.NextRunTime.Value.Minute, task.NextRunTime.Value.Second);
            switch (task.TriggerType)
            {
                case "D":
                    {
                        if (task.RepeatInMinutes != null && task.RepeatInMinutes > 0)
                        {
                            task.NextRunTime = task.NextRunTime.Value.AddMinutes(((int)task.RepeatInMinutes) + 0.0);
                            task.NextRunTimeUTC = task.NextRunTimeUTC.Value.AddMinutes(((int)task.RepeatInMinutes) + 0.0);
                        }
                        else
                        {
                            task.NextRunTime = task.NextRunTime.Value.AddDays(1);
                            task.NextRunTimeUTC = task.NextRunTimeUTC.Value.AddDays(1);
                        }
                        break;
                    }
                case "W":
                    {
                        DateTime NextRunTime;
                        var ToDay = DateTime.Now.DayOfWeek;
                        var ToDayString = DateTime.Now.DayOfWeek.ToString();
                        NextRunTime = Next(TodayDate, ToDay);
                        task.NextRunTime = NextRunTime;
                        if (task.Sunday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Sunday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        if (task.Monday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Monday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        if (task.Tuesday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Tuesday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        if (task.Wednesday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Wednesday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        if (task.Thursday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Thursday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        if (task.Friday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Friday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                        if (task.Satarday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Saturday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                            }
                        }
                       break;
                    }
                case "M":
                    {
                        //DateTime NextRunTime;
                        task.NextRunTime = task.NextRunTime.Value.AddMonths(1);
                        break;
                    }
                default: // Once
                    {
                        break;
                    }

            }
            if (task.TriggerType.ToUpper() != "O")
            {
                queueservice.InitializeQueue("SchedularQueue", 0);
                queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, null, null, null, task.NextRunTime);

            }

            var objectContext = WebFreightContext.GetContext(task.Tenant);
            TasksSchedulerService service = new TasksSchedulerService(objectContext, task.Tenant);
            service.Update(task);
             
        }
 
        private DateTime Next(DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }
    }
}