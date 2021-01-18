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
            var queueservice = new DbQueueService();
            var TodayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, task.NextRunTime.Value.Hour, task.NextRunTime.Value.Minute, task.NextRunTime.Value.Second);
            var TodayUTCDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, task.NextRunTimeUTC.Value.Hour, task.NextRunTimeUTC.Value.Minute, task.NextRunTimeUTC.Value.Second);

            if (task.NextRunTimeUTC < DateTime.UtcNow)
            {
                var NewNextRunTime = new DateTime(task.NextRunTime.Value.Year, task.NextRunTime.Value.Month, DateTime.Now.Day, task.NextRunTime.Value.Hour, task.NextRunTime.Value.Minute, task.NextRunTime.Value.Second);
                var NewNextRunTimeUTC = new DateTime(task.NextRunTimeUTC.Value.Year, task.NextRunTimeUTC.Value.Month, DateTime.Now.Day, task.NextRunTimeUTC.Value.Hour, task.NextRunTimeUTC.Value.Minute, task.NextRunTimeUTC.Value.Second);
                task.NextRunTime = NewNextRunTime;
                task.NextRunTimeUTC = NewNextRunTimeUTC; 
            }
            switch (task.TriggerType)
            {
                case "D":
                    {

                        //if (task.RepeatInMinutes != null && task.RepeatInMinutes > 0)
                        //{
                        //    task.NextRunTime = task.NextRunTime.Value.AddMinutes(((int)task.RepeatInMinutes) + 0.0);
                        //    task.NextRunTimeUTC = task.NextRunTimeUTC.Value.AddMinutes(((int)task.RepeatInMinutes) + 0.0);
                        //}
                        //else
                        //{
                        //    task.NextRunTime = task.NextRunTime.Value.AddDays(1);
                        //    task.NextRunTimeUTC = task.NextRunTimeUTC.Value.AddDays(1);
                        //}
                        var tenantDateNow = TenantServerConfigration.GetCurrentDateTime(task.Tenant);
                        var nowDateUTC = DateTime.UtcNow;

                        //TodayDate = new DateTime(tenantDateNow.Year, tenantDateNow.Month, tenantDateNow.Day, task.NextRunTime.Value.Hour, task.NextRunTime.Value.Minute, task.NextRunTime.Value.Second);
                        //var TodayUtcDate = new DateTime(nowDateUTC.Year, nowDateUTC.Month, nowDateUTC.Day, task.NextRunTimeUTC.Value.Hour, task.NextRunTimeUTC.Value.Minute, task.NextRunTimeUTC.Value.Second);

                        //potential fix
                        TodayDate = new DateTime(tenantDateNow.Year, tenantDateNow.Month, tenantDateNow.Day, tenantDateNow.Hour, tenantDateNow.Minute, tenantDateNow.Second);
                        var TodayUtcDate = new DateTime(nowDateUTC.Year, nowDateUTC.Month, nowDateUTC.Day, nowDateUTC.Hour, nowDateUTC.Minute, nowDateUTC.Second);


                        DateTime NewNextRunTime = TodayDate;
                        DateTime NewNextRunTimeUTC = TodayUtcDate;
                        if (task.RepeatInMinutes != null && task.RepeatInMinutes > 0)
                        {
                            double repeatMinutes = ((int)task.RepeatInMinutes) + 0.0;
                            NewNextRunTime = NewNextRunTime.AddMinutes(repeatMinutes);
                            NewNextRunTimeUTC = NewNextRunTimeUTC.AddMinutes(repeatMinutes);
                            if(NewNextRunTime < tenantDateNow) // if it is a past time
                            {
                                var diff = (tenantDateNow - NewNextRunTime).TotalMinutes;
                                NewNextRunTime = NewNextRunTime.AddMinutes(diff).AddMinutes(repeatMinutes);
                                NewNextRunTimeUTC = NewNextRunTimeUTC.AddMinutes(diff).AddMinutes(repeatMinutes);
                            }
                        }
                        else
                        {
                            NewNextRunTime = NewNextRunTime.AddDays(1);
                            NewNextRunTimeUTC = NewNextRunTimeUTC.AddDays(1);
                        }

                        task.NextRunTime = NewNextRunTime;
                        task.NextRunTimeUTC = NewNextRunTimeUTC;
                        break;
                    }
                case "W":
                    {

                         
                        var ToDay = DateTime.Now.DayOfWeek;
                        var ToDayString = DateTime.Now.DayOfWeek.ToString();

                        var NextRunTime = Next(TodayDate, ToDay);
                        var NextRunTimeUTC = Next(TodayUTCDate, ToDay); 

                        task.NextRunTime = NextRunTime;
                        task.NextRunTimeUTC = NextRunTimeUTC;

                        if (task.Sunday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Sunday);
                            NextRunTimeUTC = Next(TodayDate, DayOfWeek.Sunday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                                task.NextRunTimeUTC = NextRunTimeUTC;
                            }
                        }
                        if (task.Monday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Monday);
                            NextRunTimeUTC = Next(TodayDate, DayOfWeek.Monday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                                task.NextRunTimeUTC = NextRunTimeUTC;
                            }
                        }
                        if (task.Tuesday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Tuesday);
                            NextRunTimeUTC = Next(TodayDate, DayOfWeek.Tuesday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                                task.NextRunTimeUTC = NextRunTimeUTC;
                            }
                        }
                        if (task.Wednesday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Wednesday);
                            NextRunTimeUTC = Next(TodayDate, DayOfWeek.Wednesday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                                task.NextRunTimeUTC = NextRunTimeUTC;
                            }
                        }
                        if (task.Thursday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Thursday);
                            NextRunTimeUTC = Next(TodayDate, DayOfWeek.Thursday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                                task.NextRunTimeUTC = NextRunTimeUTC;
                            }
                        }
                        if (task.Friday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Friday);
                            NextRunTimeUTC = Next(TodayDate, DayOfWeek.Friday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                                task.NextRunTimeUTC = NextRunTimeUTC;
                            }
                        }
                        if (task.Satarday)
                        {
                            NextRunTime = Next(TodayDate, DayOfWeek.Saturday);
                            NextRunTimeUTC = Next(TodayDate, DayOfWeek.Saturday);
                            if (NextRunTime < task.NextRunTime)
                            {
                                task.NextRunTime = NextRunTime;
                                task.NextRunTimeUTC = NextRunTimeUTC;
                            }
                        }
                       break;
                    }
                case "M":
                    {
                        //DateTime NextRunTime;
                        task.NextRunTime = task.NextRunTime.Value.AddMonths(1);
                        task.NextRunTimeUTC = task.NextRunTimeUTC.Value.AddMonths(1);
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
                queueservice.Send(new Dictionary<string, string>() { { "TaskId", task.Id }, { "Tenant", task.Tenant.ToString() }, { "Version", task.Version.ToString() } }, task.Tenant, null, null, null, task.NextRunTimeUTC);

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