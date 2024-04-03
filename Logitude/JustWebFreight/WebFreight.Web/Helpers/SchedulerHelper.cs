using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.Helpers;
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
            CalculateNewNextRunTime(task);
            
            if (task.TriggerType.ToUpper() != "O")
            {
                SendNewSchedularQueue(task);
            }

            UpdateTaskService(task);
        }

        private void CalculateNewNextRunTime(TasksSchedulerPM task)
        {
            if (task.NextRunTimeUTC < DateTime.UtcNow)
            {
                task.NextRunTime = GetPastNextRunTime(task.NextRunTime, task.TriggerType, task.RepeatInMinutes);
                task.NextRunTimeUTC = GetPastNextRunTime(task.NextRunTimeUTC, task.TriggerType, task.RepeatInMinutes);
                return;
            }

            switch (task.TriggerType)
            {
                case "D":
                    {
                        GetDailyNextRunTime(task);
                        break;
                    }
                case "W":
                    {
                        GetWeeklyNextRunTime(task);
                        break;
                    }
                case "M":
                    {
                        GetMonthlyNextRunTime(task);
                        break;
                    }
                default: // Once
                    {
                        break;
                    }
            }
        }

        private static void GetDailyNextRunTime(TasksSchedulerPM task)
        {
            if (task.RepeatInMinutes != null && task.RepeatInMinutes > 0)
            {
                task.NextRunTime = task.NextRunTime.Value.AddMinutes(((int)task.RepeatInMinutes) + 0.0);
                task.NextRunTimeUTC = task.NextRunTimeUTC.Value.AddMinutes(((int)task.RepeatInMinutes) + 0.0);
                return;
            }

            task.NextRunTime = task.NextRunTime.Value.AddDays(1);
            task.NextRunTimeUTC = task.NextRunTimeUTC.Value.AddDays(1);
        }

        private void GetWeeklyNextRunTime(TasksSchedulerPM task)
        {
            DateTime NewNextRunTime = task.NextRunTime.Value;
            DateTime NewNextRunTimeUTC = task.NextRunTimeUTC.Value;
            DayOfWeek ToDay = DateTime.Now.DayOfWeek;
            DateTime NextRunTime = NextDayOfWeek(NewNextRunTime, ToDay);
            DateTime NextRunTimeUTC = NextDayOfWeek(NewNextRunTimeUTC, ToDay);

            task.NextRunTime = NextRunTime;
            task.NextRunTimeUTC = NextRunTimeUTC;

            if (task.Sunday)
            {
                NextRunTime = NextDayOfWeek(NewNextRunTime, DayOfWeek.Sunday);
                NextRunTimeUTC = NextDayOfWeek(NewNextRunTimeUTC, DayOfWeek.Sunday);
                if (NextRunTime < task.NextRunTime)
                {
                    task.NextRunTime = NextRunTime;
                    task.NextRunTimeUTC = NextRunTimeUTC;
                }
            }

            if (task.Monday)
            {
                NextRunTime = NextDayOfWeek(NewNextRunTime, DayOfWeek.Monday);
                NextRunTimeUTC = NextDayOfWeek(NewNextRunTimeUTC, DayOfWeek.Monday);
                if (NextRunTime < task.NextRunTime)
                {
                    task.NextRunTime = NextRunTime;
                    task.NextRunTimeUTC = NextRunTimeUTC;
                }
            }

            if (task.Tuesday)
            {
                NextRunTime = NextDayOfWeek(NewNextRunTime, DayOfWeek.Tuesday);
                NextRunTimeUTC = NextDayOfWeek(NewNextRunTimeUTC, DayOfWeek.Tuesday);
                if (NextRunTime < task.NextRunTime)
                {
                    task.NextRunTime = NextRunTime;
                    task.NextRunTimeUTC = NextRunTimeUTC;
                }
            }

            if (task.Wednesday)
            {
                NextRunTime = NextDayOfWeek(NewNextRunTime, DayOfWeek.Wednesday);
                NextRunTimeUTC = NextDayOfWeek(NewNextRunTimeUTC, DayOfWeek.Wednesday);
                if (NextRunTime < task.NextRunTime)
                {
                    task.NextRunTime = NextRunTime;
                    task.NextRunTimeUTC = NextRunTimeUTC;
                }
            }

            if (task.Thursday)
            {
                NextRunTime = NextDayOfWeek(NewNextRunTime, DayOfWeek.Thursday);
                NextRunTimeUTC = NextDayOfWeek(NewNextRunTimeUTC, DayOfWeek.Thursday);
                if (NextRunTime < task.NextRunTime)
                {
                    task.NextRunTime = NextRunTime;
                    task.NextRunTimeUTC = NextRunTimeUTC;
                }
            }

            if (task.Friday)
            {
                NextRunTime = NextDayOfWeek(NewNextRunTime, DayOfWeek.Friday);
                NextRunTimeUTC = NextDayOfWeek(NewNextRunTimeUTC, DayOfWeek.Friday);
                if (NextRunTime < task.NextRunTime)
                {
                    task.NextRunTime = NextRunTime;
                    task.NextRunTimeUTC = NextRunTimeUTC;
                }
            }

            if (task.Satarday)
            {
                NextRunTime = NextDayOfWeek(NewNextRunTime, DayOfWeek.Saturday);
                NextRunTimeUTC = NextDayOfWeek(NewNextRunTimeUTC, DayOfWeek.Saturday);
                if (NextRunTime < task.NextRunTime)
                {
                    task.NextRunTime = NextRunTime;
                    task.NextRunTimeUTC = NextRunTimeUTC;
                }
            }
        }

        private static void GetMonthlyNextRunTime(TasksSchedulerPM task)
        {
            task.NextRunTime = task.NextRunTime.Value.AddMonths(1);
            task.NextRunTimeUTC = task.NextRunTimeUTC.Value.AddMonths(1);
        }

        private DateTime GetPastNextRunTime(DateTime? taskNextRunTime, string taskTriggerType, int? taskRepeatInMinutes)
        {
            DateTime nextRunTime = taskNextRunTime.Value;
            switch (taskTriggerType)
            {
                case "D":
                    {
                        nextRunTime = GetDailyPastNextRunTime(taskRepeatInMinutes, nextRunTime);
                        break;
                    }
                case "W":
                    {
                        nextRunTime = GetWeeklyPastNextRunTime(nextRunTime);
                        break;
                    }
                case "M":
                    {
                        nextRunTime = GetMontlyPastNextRunTime(nextRunTime);
                        break;
                    }
            }

            return nextRunTime;
        }

        private DateTime GetDailyPastNextRunTime(int? taskRepeatInMinutes, DateTime nextRunTime)
        {
            if (taskRepeatInMinutes != null && taskRepeatInMinutes > 0)
            {
                while (nextRunTime < DateTime.UtcNow) nextRunTime = nextRunTime.AddMinutes(((int)taskRepeatInMinutes) + 0.0);
                return nextRunTime;
            }

            while (nextRunTime < DateTime.UtcNow)
            {
                nextRunTime = nextRunTime.AddDays(1);
            }

            return nextRunTime;
        }

        private DateTime GetWeeklyPastNextRunTime(DateTime nextRunTime)
        {
            while (nextRunTime < DateTime.UtcNow)
            {
                nextRunTime = nextRunTime.AddDays(7);
            }

            return nextRunTime;
        }

        private DateTime GetMontlyPastNextRunTime(DateTime nextRunTime)
        {
            while (nextRunTime < DateTime.UtcNow)
            {
                nextRunTime = nextRunTime.AddMonths(1);
            }

            return nextRunTime;
        }

        private DateTime NextDayOfWeek(DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }

        private void SendNewSchedularQueue(TasksSchedulerPM task)
        {
            var queueservice = new DbQueueService();
            queueservice.InitializeQueue("SchedularQueue", 0);
            queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, task.Tenant, null, null, null, task.NextRunTimeUTC);
        }

        private static void UpdateTaskService(TasksSchedulerPM task)
        {
            var objectContext = WebFreightContext.GetContext(task.Tenant);
            TasksSchedulerService service = new TasksSchedulerService(objectContext, task.Tenant);
            service.Update(task);
        }
    }
}