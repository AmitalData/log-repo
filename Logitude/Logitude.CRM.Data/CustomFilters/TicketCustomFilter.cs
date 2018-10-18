using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Helpers;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.Data.CustomFilters
{
    public class TicketCustomFilter
    {
        public static IQueryable<Ticket> GetFilteredQuery(QueryOperations operations, IQueryable<Ticket> queryableData, int tenant)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            bool showIsCancelled = false;
            ActivityRepository myRepository = new ActivityRepository(tenant);
            CorrespondenceRepository correspondenceRepository = new CorrespondenceRepository(tenant);

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "IsCancelled")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        queryableData = queryableData.Where(d => d.IsCancelled == value);
                        if (value)
                        {
                            showIsCancelled = true;
                        }
                    }

                    if (item.FieldName == "ClassificationAdvanced")
                    {
                        if (item.FieldValue != null)
                        {
                            var strItem = item.FieldValue.ToString();
                            queryableData = queryableData.Where(d => d.MainClassificationId != null);
                            queryableData = queryableData.Where(d => d.MainClassificationId.StartsWith(strItem));                           
                        }
                    }

                    if (item.FieldName == "HasOpenedActivities")
                    {
                        IQueryable<Activity> allActivities = myRepository.GetAll(tenant);

                        List<string> allActivitiesTickitsIds = allActivities.Where(d => d.IsOpen).Select(s => s.TicketId).ToList();

                        queryableData = from d in queryableData
                                        where allActivitiesTickitsIds.Contains(d.Id)
                                        select d;
                    }

                    if (item.FieldName == "HasOpenedAppointments")
                    {
                        IQueryable<Activity> allActivities = myRepository.GetAll(tenant);

                        List<string> allActivitiesTickitsIds = allActivities.Where(d => d.IsOpen && d.ActivityTypeCode=="AP").Select(s => s.TicketId).ToList();

                        queryableData = from d in queryableData
                                        where allActivitiesTickitsIds.Contains(d.Id)
                                        select d;
                    }

                    if (item.FieldName == "HasOpenedTasks")
                    {
                        IQueryable<Activity> allActivities = myRepository.GetAll(tenant);

                        List<string> allActivitiesTickitsIds = allActivities.Where(d => d.IsOpen && d.ActivityTypeCode == "TS").Select(s => s.TicketId).ToList();

                        queryableData = from d in queryableData
                                        where allActivitiesTickitsIds.Contains(d.Id)
                                        select d;
                    }

                    if (item.FieldName == "HasOpenedPhonecalls")
                    {
                        IQueryable<Activity> allActivities = myRepository.GetAll(tenant);

                        List<string> allActivitiesTickitsIds = allActivities.Where(d => d.IsOpen && d.ActivityTypeCode == "CL").Select(s => s.TicketId).ToList();

                        queryableData = from d in queryableData
                                        where allActivitiesTickitsIds.Contains(d.Id)
                                        select d;
                    }

                    if (item.FieldName == "ClosedTicketsWithOpenActivity")
                    {
                        IQueryable<Activity> allActivities = myRepository.GetAll(tenant);

                        List<string> allActivitiesTickitsIds = allActivities.Where(d => d.IsOpen).Select(s => s.TicketId).ToList();

                        queryableData = from d in queryableData
                                        where allActivitiesTickitsIds.Contains(d.Id) && d.IsClosed == true
                                        select d;
                    }

                    if (item.FieldName == "Description")
                    {
                        string searchText = item.FieldValue.ToString();
                        IQueryable<Correspondence> allCorrespondences = correspondenceRepository.GetAll(tenant);
                        List<string> allTickitsIds = allCorrespondences.Where(d => d.Description.Contains(searchText)).Select(s => s.EntityId).ToList();
                        queryableData = queryableData.Where(d => d.Subject.Contains(searchText) || allTickitsIds.Contains(d.Id));                           
                    }

                    if (item.FieldName == "TicketsBusinessUnitFilter")
                    {
                        string myOwnerId = null;
                        string myOwnerBusinessUnitId = null;

                        if (item.FieldValue != null)
                        {
                            myOwnerId = item.FieldValue.ToString();
                        }

                        if (item.FieldValue2 != null)
                        {
                            myOwnerBusinessUnitId = item.FieldValue2.ToString();
                        }

                        if (!string.IsNullOrEmpty(myOwnerId))
                        {
                            queryableData = queryableData.Where(d => d.OwnerId == null || d.OwnerId == myOwnerId);
                        }

                        if (!string.IsNullOrEmpty(myOwnerBusinessUnitId))
                        {
                            queryableData = queryableData.Where(d => d.BusinessUnitId == null || d.BusinessUnitId == myOwnerBusinessUnitId);
                        }
                    }

                    if (item.FieldName == "FirstResponseViolated")
                    {
                        DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        bool myFlag = (bool)item.FieldValue;
                        if (myFlag)
                        {
                            queryableData = queryableData.Where(d => (d.FirstResponseTime == null && d.FirstResponseDue != null && d.FirstResponseDue < todayDate)
                                                                   || (d.FirstResponseTime != null && d.FirstResponseDue != null && d.FirstResponseDue < d.FirstResponseTime));      
                        }
                        else
                        {
                            queryableData = queryableData.Where(d => (d.FirstResponseTime == null && d.FirstResponseDue != null && d.FirstResponseDue > todayDate)
                                                                   || (d.FirstResponseTime != null && d.FirstResponseDue != null && d.FirstResponseDue > d.FirstResponseTime));   
                        }
                    }

                    if (item.FieldName == "ResolvedViolated")
                    {
                        DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        bool myFlag = (bool)item.FieldValue;
                        if (myFlag)
                        {            
                            queryableData = queryableData.Where(d => (d.FirstResolveDate == null && d.ResolveWithinDue != null && d.ResolveWithinDue < todayDate)
                                                                  || (d.FirstResolveDate != null && d.ResolveWithinDue != null && d.ResolveWithinDue < d.FirstResolveDate));
                        }
                        else
                        {
                            queryableData = queryableData.Where(d => (d.FirstResolveDate == null && d.ResolveWithinDue != null && d.ResolveWithinDue > todayDate)
                                                                 || (d.FirstResolveDate != null && d.ResolveWithinDue != null && d.ResolveWithinDue > d.FirstResolveDate));
                        }
                    }

                    #region Ticket Queries 

                    if (item.FieldName == "MyUnassignedTickets")
                    {
                        TicketStageRepository repository = new TicketStageRepository(tenant);
                        TicketStage resolvedStage = repository.GetTicketStageByCode("RE", tenant);
                        TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);
                        queryableData = queryableData.Where(d => d.OwnerId == null && d.StageId != resolvedStage.Id && d.StageId != closedStage.Id);
                    }

                    if (item.FieldName == "MyAllOpenTickets")
                    {
                        TicketStageRepository repository = new TicketStageRepository(tenant);
                        TicketStage resolvedStage = repository.GetTicketStageByCode("RE", tenant);
                        TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);
                        queryableData = queryableData.Where(d => d.IsClosed == false && d.IsCancelled == false && d.StageId != resolvedStage.Id &&  d.StageId != closedStage.Id);
                    }

                    if (item.FieldName == "MySolvedTickets")
                    {
                        TicketStageRepository repository = new TicketStageRepository(tenant);
                        TicketStage resolvedStage = repository.GetTicketStageByCode("RE", tenant);
                        TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);
                        queryableData = queryableData.Where(d => d.StageId == resolvedStage.Id || d.StageId == closedStage.Id);
                    }

                    if (item.FieldName == "MySolvedSLATickets")
                    {
                        DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        TicketStageRepository repository = new TicketStageRepository(tenant);
                        TicketStage resolvedStage = repository.GetTicketStageByCode("RE", tenant);
                        TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

                        queryableData = queryableData.Where(d => d.IsCancelled == false
                                                            && (d.StageId == resolvedStage.Id || d.StageId == closedStage.Id)
                                                            && ((d.FirstResponseTime == null && d.FirstResponseDue != null && d.FirstResponseDue < todayDate)
                                                            || (d.FirstResponseTime != null && d.FirstResponseDue != null && d.FirstResponseDue < d.FirstResponseTime)
                                                            || (d.FirstResolveDate == null && d.ResolveWithinDue != null && d.ResolveWithinDue < todayDate)
                                                            || (d.FirstResolveDate != null && d.ResolveWithinDue != null && d.ResolveWithinDue < d.FirstResolveDate)));
                    }

                    if (item.FieldName == "MySLAFailures")
                    {
                        DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        TicketStageRepository repository = new TicketStageRepository(tenant);
                        TicketStage resolvedStage = repository.GetTicketStageByCode("RE", tenant);
                        TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

                        queryableData = queryableData.Where(d => d.IsCancelled == false && d.IsClosed == false
                                                            && (d.StageId != resolvedStage.Id  &&  d.StageId != closedStage.Id)
                                                            && ((d.FirstResponseTime == null && d.FirstResponseDue != null && d.FirstResponseDue < todayDate)
                                                            || (d.FirstResponseTime != null && d.FirstResponseDue != null && d.FirstResponseDue < d.FirstResponseTime)
                                                            || (d.FirstResolveDate == null && d.ResolveWithinDue != null && d.ResolveWithinDue < todayDate)
                                                            || (d.FirstResolveDate != null && d.ResolveWithinDue != null && d.ResolveWithinDue < d.FirstResolveDate)));                                                           
                    }

                    if (item.FieldName == "MyRecentlyUpdatedTickets")
                    {
                        TicketStageRepository repository = new TicketStageRepository(tenant);
                        TicketStage resolvedStage = repository.GetTicketStageByCode("RE", tenant);
                        TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

                        string loggedUserEmail = Tools.GetAuthenticatedUser();
                        UserRepository userRepository = new UserRepository(tenant);
                        User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);
                        queryableData = queryableData.Where(d => d.IsClosed == false && d.IsCancelled == false && d.StageId != resolvedStage.Id &&  d.StageId != closedStage.Id &&  d.UpdatedByUserId != loggedUser.Id);
                    }

                    #endregion 

                    if (item.FieldName == "OpenTicketByDueTimeCustomFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            IQueryable<Ticket> noResolveTimeIQueryable = queryableData.Where(d => d.FullResolvedTime == null && d.ResolveWithinDue != null);
                            IQueryable<Ticket> noFirstResponseIQueryable = queryableData.Where(d => d.FirstResponseTime == null && d.FirstResponseDue != null);

                            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

                            List<Ticket> noResolveTimeList = noResolveTimeIQueryable.ToList();
                            List<Ticket> noResolveTimeList_Overdue = new List<Ticket>(), noResolveTimeList_Due1H = new List<Ticket>(), noResolveTimeList_Due2H = new List<Ticket>(), noResolveTimeList_Due4H = new List<Ticket>(), noResolveTimeList_Due8H = new List<Ticket>();
                            noResolveTimeList_Overdue = noResolveTimeList.Where(d => d.ResolveWithinDue < todayDateTime).ToList();
                            noResolveTimeList_Due1H = noResolveTimeList.Where(d => d.ResolveWithinDue != null && d.FirstResponseTime != null && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes > 0 && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes <= 60).ToList();
                            noResolveTimeList_Due2H = noResolveTimeList.Where(d => d.ResolveWithinDue != null && d.FirstResponseTime != null && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes > 60 && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes <= 120).ToList();
                            noResolveTimeList_Due4H = noResolveTimeList.Where(d => d.ResolveWithinDue != null && d.FirstResponseTime != null && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes > 120 && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes <= 240).ToList();
                            noResolveTimeList_Due8H = noResolveTimeList.Where(d => d.ResolveWithinDue != null && d.FirstResponseTime != null && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes > 240 && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes <= 480).ToList();

                            if (item.FieldValue.ToString() == "overduefr")
                            {
                                queryableData = noFirstResponseIQueryable.Where(d => d.FirstResponseDue < todayDateTime && d.ResolveWithinDue > todayDateTime && d.FullResolvedTime == null).ToList().Where(a => !noResolveTimeList_Overdue.Contains(a)).AsQueryable();
                            }

                            else if (item.FieldValue.ToString() == "overduere")
                            {
                                queryableData = noResolveTimeIQueryable.Where(d => d.ResolveWithinDue < todayDateTime);
                            }

                            else if (item.FieldValue.ToString() == "due < 1hfr")
                            {
                                queryableData = noFirstResponseIQueryable.Where(d => SqlFunctions.DateDiff("minute", todayDateTime, d.FirstResponseDue.Value) > 0 && SqlFunctions.DateDiff("minute", todayDateTime, d.FirstResponseDue.Value) <= 60).Where(a => !noResolveTimeList_Due1H.Contains(a));
                            }

                            else if (item.FieldValue.ToString() == "due < 1hre")
                            {
                                queryableData = noResolveTimeIQueryable.Where(d => SqlFunctions.DateDiff("minute", todayDateTime, d.ResolveWithinDue.Value) > 0 && SqlFunctions.DateDiff("minute", todayDateTime, d.ResolveWithinDue.Value) <= 60);
                            }

                            else if (item.FieldValue.ToString() == "due < 2hfr")
                            {
                                queryableData = noFirstResponseIQueryable.Where(d => SqlFunctions.DateDiff("minute", todayDateTime, d.FirstResponseDue.Value) > 60 && SqlFunctions.DateDiff("n", todayDateTime, d.FirstResponseDue.Value) <= 120).Where(a => !noResolveTimeList_Due2H.Contains(a));
                            }

                            else if (item.FieldValue.ToString() == "due < 2hre")
                            {
                                queryableData = noResolveTimeIQueryable.Where(d => SqlFunctions.DateDiff("minute", todayDateTime, d.ResolveWithinDue.Value) > 60 && SqlFunctions.DateDiff("minute", todayDateTime, d.ResolveWithinDue.Value) <= 120);
                            }

                            else if (item.FieldValue.ToString() == "due < 4hfr")
                            {
                                queryableData = noFirstResponseIQueryable.Where(d => SqlFunctions.DateDiff("minute", todayDateTime, d.FirstResponseDue.Value) > 120 && SqlFunctions.DateDiff("minute", todayDateTime, d.FirstResponseDue.Value) <= 240).Where(a => !noResolveTimeList_Due4H.Contains(a));
                            }

                            else if (item.FieldValue.ToString() == "due < 4hre")
                            {
                                queryableData = noResolveTimeIQueryable.Where(d => SqlFunctions.DateDiff("minute", todayDateTime, d.ResolveWithinDue.Value) > 120 && SqlFunctions.DateDiff("minute", todayDateTime, d.ResolveWithinDue.Value) <= 240);
                            }

                            else if (item.FieldValue.ToString() == "due < 8hfr")
                            {
                                queryableData = noFirstResponseIQueryable.Where(d => SqlFunctions.DateDiff("minute", todayDateTime, d.FirstResponseDue.Value) > 240 && SqlFunctions.DateDiff("minute", todayDateTime, d.FirstResponseDue.Value) <= 480).Where(a => !noResolveTimeList_Due8H.Contains(a));
                            }

                            else if (item.FieldValue.ToString() == "due < 8hre")
                            {
                                queryableData = noResolveTimeIQueryable.Where(d => SqlFunctions.DateDiff("minute", todayDateTime, d.ResolveWithinDue.Value) > 240 && SqlFunctions.DateDiff("minute", todayDateTime, d.ResolveWithinDue.Value) <= 480);
                            }
                        }
                    }

                    #region Ticket Create Date Filter 
                    if (item.FieldName == "ChartCreateDateTicketFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            TicketStageRepository repository = new TicketStageRepository(tenant);
                            TicketStage resolvedStage = repository.GetTicketStageByCode("RE", tenant);
                            TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

                            TicketClassificationRepository classificationRepository = new TicketClassificationRepository(tenant);
                            TicketClassification generalClassification = classificationRepository.GetMainTicketClassificationByTenant(tenant);

                            queryableData = queryableData.Where(d => (d.StageId != resolvedStage.Id && d.StageId != closedStage.Id)
                                                                && d.CreateDate != null);

                            string code = item.FieldValue != null ? item.FieldValue.ToString() : "";

                            DatesHelper helper = Tools.GetDates(code, tenant);
                            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                            DateTime? date1 = helper.Date1;
                            DateTime? date2 = helper.Date2;
                            int days = helper.Days;

                            if (code == "-1")
                            {
                                date2 = date1;
                            }
                            if (code == "-7")
                            {
                                date1 = date1.Value.AddDays(1);
                            }

                            if (days >= 0)
                            {
                                queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate.Value) == todayDate);
                            }

                            else if (days == -1)
                            {
                                queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate.Value) == date1);
                            }

                            else
                            {
                                queryableData = queryableData.Where(d => d.CreateDate.Value >= date1 && d.CreateDate.Value <= date2);
                            }
                        }
                    }

                    if (item.FieldName == "SLAEscalationsOpenedTicketsFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string values = item.FieldValue != null ? item.FieldValue.ToString() : "";

                            if (!string.IsNullOrEmpty(values))
                            {
                                string splitDate = values.Split('?')[0];
                                string groupcode = values.Split('?')[1];
                                string escalationtype = values.Split('?')[2];
                                string code = values.Split('?')[3];

                                //DateTime date = DateTime.Parse(splitDate);
                                DateTime date = Convert.ToDateTime(item.FieldValue2);

                                DatesHelper helper = Tools.GetDates(code, tenant);
                                DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                DateTime? date1 = helper.Date1;
                                DateTime? date2 = helper.Date2;
                                int days = helper.Days;

                                if (code == "-1")
                                {
                                    date2 = date1;
                                }
                                if (code == "-7")
                                {
                                    date1 = date1.Value.AddDays(1);
                                }

                                TicketStageRepository repository = new TicketStageRepository(tenant);
                                TicketStage stage1 = repository.GetTicketStageByCode("RE", tenant);
                                TicketStage stage2 = repository.GetTicketStageByCode("CS", tenant);

                                queryableData = queryableData.Where(d => d.Tenant == tenant
                                                    && (d.StageId != stage1.Id && d.StageId != stage2.Id)
                                                    && (d.CreateDate != null && d.CreateDate >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= date2)
                                                    &&
                                                    (
                                                    (d.FirstResponseDue >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.FirstResponseDue) <= date2)
                                                    ||
                                                    (d.ResolveWithinDue >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.ResolveWithinDue) <= date2)
                                                    ));

                                IQueryable<Ticket> queryableData_RE = queryableData.Where(d => (d.FirstResolveDate != null && ((d.FirstResolveDate > d.ResolveWithinDue)))
                                                                                            || (d.FirstResolveDate == null && d.ResolveWithinDue < todayDate));
                                IQueryable<Ticket> queryableData_FR = queryableData.Where(d => (d.FirstResponseTime != null && (d.FirstResponseTime > d.FirstResponseDue)) 
                                                                                            || (d.FirstResponseTime == null && d.FirstResponseDue < todayDate));

                                if (groupcode == "D")
                                {
                                    if (escalationtype == "RW")
                                    {
                                        queryableData = queryableData_RE.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ResolveWithinDue.Value) == System.Data.Entity.DbFunctions.TruncateTime(date));

                                    }
                                    else
                                    {
                                        queryableData = queryableData_FR.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResponseDue.Value) == System.Data.Entity.DbFunctions.TruncateTime(date)).Where(a => !queryableData_RE.Contains(a));
                                    }
                                }

                                else if (groupcode == "M")
                                {
                                    if (escalationtype == "RW")
                                    {
                                        queryableData = queryableData_RE.Where(d => d.ResolveWithinDue.Value.Month == date.Month && d.ResolveWithinDue.Value.Year == date.Year);
                                    }

                                    else
                                    {
                                        queryableData = queryableData_FR.Where(d => d.FirstResponseDue.Value.Month == date.Month && d.FirstResponseDue.Value.Year == date.Year).Where(a => !queryableData_RE.Contains(a));

                                    }
                                }

                                else
                                {
                                    //DateTime endWeek = date.AddDays(6).Date;
                                    int day = Convert.ToInt32(date.DayOfWeek);
                                    DateTime startOfWeek = date.AddDays((-1 * day));
                                    DateTime endOfWeek = date.AddDays((6 - day));

                                    if (escalationtype == "RW")
                                    {
                                        queryableData = queryableData_RE.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ResolveWithinDue.Value) >= System.Data.Entity.DbFunctions.TruncateTime(date) && System.Data.Entity.DbFunctions.TruncateTime(d.ResolveWithinDue.Value) <= endOfWeek);
                                    }

                                    else
                                    {
                                        queryableData = queryableData_FR.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResponseDue.Value) >= System.Data.Entity.DbFunctions.TruncateTime(date) && System.Data.Entity.DbFunctions.TruncateTime(d.FirstResponseDue.Value) <= endOfWeek).Where(a => !queryableData_RE.Contains(a));
                                    }
                                }
                            }
                        }
                    }

                    if (item.FieldName == "OpenedTicketsCreateDateFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string values = item.FieldValue != null ? item.FieldValue.ToString() : "";

                            if (!string.IsNullOrEmpty(values))
                            {
                                string splitDate = values.Split('?')[0];
                                string dateCode = values.Split('?')[1];
                                string code = values.Split('?')[2];

                                //DateTime date = DateTime.Parse(splitDate);
                                DateTime date = Convert.ToDateTime(item.FieldValue2);

                                TicketStageRepository repository = new TicketStageRepository(tenant);
                                TicketStage resolveStage = repository.GetTicketStageByCode("RE", tenant);
                                TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

                                DatesHelper helper = Tools.GetDates(code, tenant);

                                DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                                DateTime? date1 = helper.Date1;
                                DateTime? date2 = helper.Date2;
                                int days = helper.Days;

                                if (code == "-1")
                                {
                                    date2 = date1;
                                }

                                if (code == "-7")
                                {
                                    date1 = date1.Value.AddDays(1);
                                }

                                queryableData = queryableData.Where(d => d.Tenant == tenant && (d.StageId != resolveStage.Id && d.StageId != closedStage.Id)
                                       && d.CreateDate != null
                                       && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= date1
                                       && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= date2);

                                if (dateCode == "D")
                                {
                                    queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate.Value) == System.Data.Entity.DbFunctions.TruncateTime(date));
                                }

                                else if (dateCode == "M")
                                {
                                    queryableData = queryableData.Where(d => d.CreateDate.Value.Month == date.Month && d.CreateDate.Value.Year == date.Year);
                                }
                                else
                                {
                                    //DateTime endWeek = date.AddDays(6).Date;
                                        int day = Convert.ToInt32(date.DayOfWeek);
                                        DateTime startOfWeek = date.AddDays((-1 * day));
                                        DateTime endOfWeek = date.AddDays((6 - day));
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate.Value) >= System.Data.Entity.DbFunctions.TruncateTime(date) && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate.Value) <= endOfWeek);
                                  

                                    }
                            }
                        }
                    }

                    #endregion 

                    #region Ticket First Resolve Date Filter

                    if (item.FieldName == "ChartFirstResolveTicketFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            TicketStageRepository repository = new TicketStageRepository(tenant);
                            TicketStage resolvedStage = repository.GetTicketStageByCode("RE", tenant);
                            TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

                            TicketClassificationRepository classificationRepository = new TicketClassificationRepository(tenant);
                            TicketClassification generalClassification = classificationRepository.GetMainTicketClassificationByTenant(tenant);

                            queryableData = queryableData.Where(d => (d.StageId == resolvedStage.Id || d.StageId == closedStage.Id)
                                                                && d.FirstResolveDate != null);

                            string code = item.FieldValue != null ? item.FieldValue.ToString() : "";
                            DatesHelper helper = Tools.GetDates(code, tenant);

                            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                            DateTime? date1 = helper.Date1;
                            DateTime? date2 = helper.Date2;
                            int days = helper.Days;

                            if (code == "-1")
                            {
                                date2 = date1;
                            }
                            if (code == "-7")
                            {
                                date1 = date1.Value.AddDays(1);
                            }

                            if (days >= 0)
                            {
                                queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate.Value) == todayDate);
                            }

                            else if (days == -1)
                            {
                                queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate.Value) == date1);
                                }

                            else
                            {
                                queryableData = queryableData.Where(d => d.FirstResolveDate.Value >= date1 && d.FirstResolveDate.Value <= date2);
                            }
                        }
                    }

                    if (item.FieldName == "SLAEscalationsClosedTicketsFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string values = item.FieldValue != null ? item.FieldValue.ToString() : "";

                            if (!string.IsNullOrEmpty(values))
                            {
                                string splitDate = values.Split('?')[0];
                                string groupcode = values.Split('?')[1];
                                string escalationtype = values.Split('?')[2];
                                string code = values.Split('?')[3];

                                //DateTime date =  DateTime.Parse(splitDate);
                                
                                DateTime date = Convert.ToDateTime(item.FieldValue2);

                                DatesHelper helper = Tools.GetDates(code, tenant);
                                DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                                DateTime? date1 = helper.Date1;
                                DateTime? date2 = helper.Date2;
                                int days = helper.Days;
                                
                                // Maheera: This is not exactly true, I add this case without update on general class (time) one
                                if (code == "-1")
                                {
                                    date2 = date1;
                                }
                                if (code == "-7")
                                {
                                    date1 = date1.Value.AddDays(1);
                                }

                                TicketStageRepository repository = new TicketStageRepository(tenant);
                                TicketStage stage1 = repository.GetTicketStageByCode("RE", tenant);
                                TicketStage stage2 = repository.GetTicketStageByCode("CS", tenant);

                                queryableData = queryableData.Where(d => d.Tenant == tenant
                                                    && (d.StageId == stage1.Id || d.StageId == stage2.Id)
                                                    &&
                                                    (
                                                    (d.FirstResponseTime != null &&  d.FirstResponseDue >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.FirstResponseDue) <= date2)
                                                    ||
                                                    (d.FirstResolveDate != null && d.ResolveWithinDue >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.ResolveWithinDue) <= date2)
                                                    ));

                                IQueryable<Ticket> queryableData_RE = queryableData.Where(d => (d.FirstResolveDate > d.ResolveWithinDue) || (d.FirstResolveDate > d.ResolveWithinDue && d.FirstResponseTime > d.FirstResponseDue));
                                IQueryable<Ticket> queryableData_FR = queryableData.Where(d => (d.FirstResponseTime > d.FirstResponseDue));

                                if (groupcode == "D")
                                {
                                    if (escalationtype == "RW")
                                    {
                                        queryableData = queryableData_RE.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ResolveWithinDue.Value) == System.Data.Entity.DbFunctions.TruncateTime(date));

                                    }
                                    else
                                    {
                                        queryableData = queryableData_FR.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResponseDue.Value) == System.Data.Entity.DbFunctions.TruncateTime(date)).Where(a => !queryableData_RE.Contains(a));
                                    }
                                }

                                else if (groupcode == "M")
                                {
                                    if (escalationtype == "RW")
                                    {
                                        queryableData = queryableData_RE.Where(d => d.ResolveWithinDue.Value.Month == date.Month && d.ResolveWithinDue.Value.Year == date.Year);
                                    }

                                    else
                                    {
                                        queryableData = queryableData_FR.Where(d => d.FirstResponseDue.Value.Month == date.Month && d.FirstResponseDue.Value.Year == date.Year).Where(a => !queryableData_RE.Contains(a));
                                    }
                                }

                                else
                                {
                                    //DateTime endWeek = date.AddDays(6).Date;
                                    int day = Convert.ToInt32(date.DayOfWeek);
                                    DateTime startOfWeek = date.AddDays((-1 * day));
                                    DateTime endOfWeek = date.AddDays((6 - day));
                                    if (escalationtype == "RW")
                                    {
                                        queryableData = queryableData_RE.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ResolveWithinDue.Value) >= System.Data.Entity.DbFunctions.TruncateTime(date) && System.Data.Entity.DbFunctions.TruncateTime(d.ResolveWithinDue.Value) <= endOfWeek);
                                    }

                                    else
                                    {
                                        queryableData = queryableData_FR.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResponseDue.Value) >= System.Data.Entity.DbFunctions.TruncateTime(date) && System.Data.Entity.DbFunctions.TruncateTime(d.FirstResponseDue.Value) <= endOfWeek).Where(a => !queryableData_RE.Contains(a));
                                    }
                                }
                            }
                        }
                    }

                    if (item.FieldName == "SolvedClosedTicketsFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string values = item.FieldValue != null ? item.FieldValue.ToString() : "";

                            if (!string.IsNullOrEmpty(values))
                            {
                                string splitDate = values.Split('?')[0];
                                string dateCode = values.Split('?')[1];
                                string code = values.Split('?')[2];

                               // DateTime date = DateTime.Parse(splitDate);
                                DateTime date = Convert.ToDateTime(item.FieldValue2);
                                TicketStageRepository repository = new TicketStageRepository(tenant);
                                TicketStage resolveStage = repository.GetTicketStageByCode("RE", tenant);
                                TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

                                DatesHelper helper = Tools.GetDates(code, tenant);

                                DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                                DateTime? date1 = helper.Date1;
                                DateTime? date2 = helper.Date2;
                                int days = helper.Days;

                                if (code == "-1")
                                {
                                    date2 = date1;
                                }
                                if (code == "-7")
                                {
                                    date1 = date1.Value.AddDays(1);
                                }

                                queryableData = queryableData.Where(d => d.Tenant == tenant && (d.StageId == resolveStage.Id || d.StageId == closedStage.Id)
                                       && System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate) >= date1
                                       && System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate) <= date2);

                                if (dateCode == "D")
                                {
                                    queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate.Value) == System.Data.Entity.DbFunctions.TruncateTime(date));
                                }

                                else if (dateCode == "M")
                                {
                                    queryableData = queryableData.Where(d => d.FirstResolveDate.Value.Month == date.Month && d.FirstResolveDate.Value.Year == date.Year);
                                }
                                else
                                {
                                    //DateTime endWeek = date.AddDays(6).Date;
                                    int day = Convert.ToInt32(date.DayOfWeek);
                                    DateTime startOfWeek = date.AddDays((-1 * day));
                                    DateTime endOfWeek = date.AddDays((6 - day));
                                    queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate.Value) >= System.Data.Entity.DbFunctions.TruncateTime(date) && System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate.Value) <= endOfWeek);
                                }
                            }
                        }
                    }

                    #endregion 
                }
            }

            //if (showIsCancelled)
            //{
            //    queryableData = queryableData.Where(d => d.IsCancelled == true);
            //}
            //else
            //{
            //    queryableData = queryableData.Where(d => d.IsCancelled == false);
            //}

            return queryableData;
        }
    }
}
