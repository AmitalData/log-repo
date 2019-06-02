using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.BL.EntityQueryServices;
using Logitude.TimeManagement.BL.EntityUpdateServices;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityListQueryServices;
using Logitude.TimeManagement.Data.EntityLists;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using WebFreight.Web.BookingModel.DomainServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class TimeManagementDomainController : ApiController
    {
        ITimeManagementContext myContext;
        IQueryable<TMOfficeHour> officeHours;
        public HttpResponseMessage GetWeeklyTimeSheetList(string employeeUserId, string locationCode, string periodStartDate)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TMEmployeeTime", "READ", tenant);

                DateTime? myStartDate = periodStartDate == "null" ? null : DateHelper.GetDate(periodStartDate);

                TimeManagementAPIHelper myResult = this.FillWeeklyTimeSheetList(employeeUserId, locationCode, myStartDate, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDataEntryTimeSheetList(string employeeUserId, string locationCode, string startDate, string endDate)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TMEmployeeTime", "READ", tenant);

                DateTime? myStartDate = startDate == "null" ? null : DateHelper.GetDate(startDate);
                DateTime? myEndDate = endDate == "null" ? null : DateHelper.GetDate(endDate);

                TimeManagementAPIHelper myResult = this.FillDataEntryTimeSheetList(employeeUserId, locationCode, myStartDate, myEndDate, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage Put(TimeManagementAPIHelper args)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
                TMEmployeeTimeUpdateService service = new TMEmployeeTimeUpdateService(myContext, new Dictionary<string, IContext>(), tenant);
                TMEmployeeTimeRepository repository = new TMEmployeeTimeRepository(myContext);
                TMEmployeeTimeQueryService queryService = new TMEmployeeTimeQueryService(myContext);

                if (args != null)
                {
                    if (args.ItemsPM.Count > 0)
                    {
                        string loggedUserId = authToken.Email;
                        UserRepository userRepository = new UserRepository(tenant);
                        User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, authToken.Email, tenant, true);
                        if (loggedUser != null)
                        {
                            loggedUserId = loggedUser.Id;
                        }

                        foreach (TMEmployeeTimePM itemChanged in args.ItemsPM)
                        {
                            TMEmployeeTimePM itemPOCO = queryService.GetSingle(itemChanged.Id, true, false);
                            if (itemPOCO != null && !(itemPOCO.LocationCode == itemChanged.LocationCode && itemPOCO.SprintId == itemChanged.SprintId && itemPOCO.ProjectId == itemChanged.ProjectId && itemPOCO.Description == itemChanged.Description && itemPOCO.WINumber == itemChanged.WINumber))
                            {
                                itemPOCO.ProjectId = itemChanged.ProjectId;
                                itemPOCO.SprintId = itemChanged.SprintId;
                                itemPOCO.Description = itemChanged.Description;
                                itemPOCO.WINumber = itemChanged.WINumber;
                                itemPOCO.LocationCode = itemChanged.LocationCode;
                                //repository.Update(itemPOCO);
                                itemPOCO.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                                service.Update(itemPOCO, true);
                            }

                            if (itemChanged.TimeInMinutes != itemChanged.TimeInMinutes_db)
                            {
                                //itemPOCO = repository.GetSingle(itemChanged.Id, itemChanged.Tenant); //dbList.Where(d => d.DateOfWork != null && d.DateOfWork.Date == itemChanged.DateOfWork.Date).FirstOrDefault();
                                if (itemPOCO != null)
                                {
                                    itemPOCO.TimeInMinutes = itemChanged.TimeInMinutes;
                                    itemPOCO.Description = itemChanged.Description;
                                    itemPOCO.WINumber = itemChanged.WINumber;
                                    itemPOCO.EmployeeUserId = itemChanged.EmployeeUserId;
                                    itemPOCO.SprintId = itemChanged.SprintId;
                                    itemPOCO.ProjectId = itemChanged.ProjectId;
                                    itemPOCO.DateOfWork = itemChanged.DateOfWork;
                                    itemPOCO.LocationCode = itemChanged.LocationCode;
                                    //repository.Update(itemPOCO);

                                    itemPOCO.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                                    service.Update(itemPOCO, true);
                                }

                                else
                                {
                                    itemPOCO = new TMEmployeeTimePM()
                                    {
                                        Id = IdCounter.GetNumber("TMEmployeeTime", tenant),
                                        Tenant = tenant,
                                        TimeInMinutes = itemChanged.TimeInMinutes,
                                        DateOfWork = itemChanged.DateOfWork,
                                        CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                        UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                        ProjectId = itemChanged.ProjectId,
                                        Description = itemChanged.Description,
                                        WINumber = itemChanged.WINumber,
                                        EmployeeUserId = itemChanged.EmployeeUserId,
                                        LocationCode = itemChanged.LocationCode,
                                        UpdatedByUserId = loggedUserId,
                                        CreatedByUserId = loggedUserId,
                                        SprintId = itemChanged.SprintId,
                                    };
                                    //repository.Add(itemPOCO);

                                    itemPOCO.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                                    service.Update(itemPOCO, true);
                                }
                            }
                        }
                        //repository.SubmitChanges();
                    }
                }

                TimeManagementAPIHelper myResult = this.FillDataEntryTimeSheetList(args.EmployeeUserId, args.LocationCode, args.StartDate, args.EndDate, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private TimeManagementAPIHelper FillWeeklyTimeSheetList(string employeeUserId, string locationCode, DateTime? myStartDate, int tenant, List<TMEmployeeTime> projects = null)
        {
            TimeManagementAPIHelper myResult = new TimeManagementAPIHelper()
            {
                Id = tenant,
                EmployeeUserId = employeeUserId,
                LocationCode = locationCode,
                StartDate = myStartDate,
            };

            if (myStartDate == null)
            {
                throw new ApplicationException("Please select period start date");
            }

            else
            {
                if (myStartDate.Value.DayOfWeek != DayOfWeek.Sunday)
                {
                    throw new ApplicationException("Period start date should be sunday");
                }
            }

            ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
            IQueryable<TMEmployeeTime> iQueryable;
            DateTime myEndDate = myStartDate.Value.AddDays(6);

            List<TMEmployeeTime> list;

            if (projects != null)
            {
                list = projects;
            }
            else
            {
                iQueryable = (from d in myContext.TMEmployeeTimes
                              where d.Tenant == tenant && d.DateOfWork != null
                              select d);
                if (!string.IsNullOrEmpty(employeeUserId))
                {
                    iQueryable = iQueryable.Where(d => d.EmployeeUserId == employeeUserId);
                }

                if (!string.IsNullOrEmpty(locationCode) && locationCode != "A")
                {
                    iQueryable = iQueryable.Where(d => d.LocationCode == locationCode);
                }

                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(myStartDate));
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(myEndDate));

                list = iQueryable.ToList();
            }

            List<TMProject> allProjects = (from d in myContext.TMProjects where d.Tenant == tenant select d).ToList();

            officeHours = (from a in myContext.TMOfficeHours
                           where a.Tenant == tenant && a.Inactive==false && a.UserId == employeeUserId && a.WorkDate != null &&
                           System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) >= System.Data.Entity.DbFunctions.TruncateTime(myStartDate) &&
                           System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) <= System.Data.Entity.DbFunctions.TruncateTime(myEndDate)
                           select a);

            var groupedItems = (from d in list
                                group d by new { d.WINumber, d.ProjectId, d.Description } into g
                                select new
                                {
                                    ProjectId = g.Key.ProjectId,
                                    Description = g.Key.Description,
                                    WINumber = g.Key.WINumber
                                });

            foreach (var item in groupedItems)
            {
                TimeSheetItem newItem = new TimeSheetItem()
                {
                    ProjectId = item.ProjectId,
                    WINumber = item.WINumber,
                    Description = item.Description,
                    ProjectId_db = item.ProjectId,
                    WINumber_db = item.WINumber,
                    Description_db = item.Description,
                };

                TMProject myProject = allProjects.Where(d => d.Id == item.ProjectId).FirstOrDefault();
                if (myProject != null)
                {
                    newItem.ProjectName = myProject.Name;
                }

                List<TMEmployeeTime> itemGrouplist = list.Where(d => d.ProjectId == item.ProjectId && d.Description == item.Description && d.WINumber == item.WINumber).ToList();

                for (int i = 0; i < 7; i++)
                {
                    var date = myStartDate.Value.AddDays(i);
                    List<TMEmployeeTime> mylist = itemGrouplist.Where(d => d.DateOfWork.Date == date.Date).ToList();

                    TimeSheetItemDay newItemDay = new TimeSheetItemDay();
                    newItemDay.Index = i;
                    newItemDay.Date = date;
                    int? minutsSum = mylist.Sum(s => s.TimeInMinutes);
                    newItemDay.Minuts = minutsSum == 0 ? null : minutsSum;
                    newItemDay.Minuts_db = minutsSum == 0 ? null : minutsSum;
                    //newItemDay.ProjectId = item.ProjectId;
                    newItem.Days.Add(newItemDay);
                }
                newItem.TotalMinutes = newItem.Days.Sum(s => s.Minuts != null ? s.Minuts.Value : 0);
                newItem.EmployeeUserId = employeeUserId;
                newItem.LocationCode = locationCode;
                myResult.Items.Add(newItem);
            }

            for (int i = 0; i < 7; i++)
            {
                var date = myStartDate.Value.AddDays(i);
                TimeSheetItemDay newItemDay = new TimeSheetItemDay();
                newItemDay.Index = i;
                newItemDay.Date = date;
                this.FillOfficeHours(newItemDay, date,false);
                myResult.OfficeClockDays.Add(newItemDay);

            }
            return myResult;
        }
        private TimeManagementAPIHelper FillDataEntryTimeSheetList(string employeeUserId, string locationCode, DateTime? myStartDate, DateTime? myEndDate, int tenant)
        {
            TimeManagementAPIHelper myResult = new TimeManagementAPIHelper()
            {
                Id = tenant,
                EmployeeUserId = employeeUserId,
                LocationCode = locationCode,
                StartDate = myStartDate,
            };

            if (myStartDate != null && myEndDate != null)
            {
                ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);

                List<TMLocation> allLocations = (from d in myContext.TMLocations select d).ToList();
                List<TMProject> allProjects = (from d in myContext.TMProjects where d.Tenant == tenant select d).ToList();
                List<Sprint> allSprints = (from d in myContext.Sprints where d.Tenant == tenant select d).ToList();


                IQueryable<TMEmployeeTime> iQueryable_TMEmployeeTime = (from d in myContext.TMEmployeeTimes where d.Tenant == tenant && d.DateOfWork != null select d);
                IQueryable<TMOfficeHour> iQueryable_TMOfficeHour = (from a in myContext.TMOfficeHours where a.Tenant == tenant && a.WorkDate != null && !a.Inactive select a);

                if (!string.IsNullOrEmpty(employeeUserId))
                {
                    iQueryable_TMEmployeeTime = iQueryable_TMEmployeeTime.Where(d => d.EmployeeUserId == employeeUserId);
                    iQueryable_TMOfficeHour = iQueryable_TMOfficeHour.Where(d => d.UserId == employeeUserId);
                }

                if (!string.IsNullOrEmpty(locationCode) && locationCode != "A")
                {
                    iQueryable_TMEmployeeTime = iQueryable_TMEmployeeTime.Where(d => d.LocationCode == locationCode);
                }

                List<TMOfficeHour> list_TMOfficeHour =
                    (from a in iQueryable_TMOfficeHour
                     where
                     System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) >= System.Data.Entity.DbFunctions.TruncateTime(myStartDate)
                     &&
                     System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) <= System.Data.Entity.DbFunctions.TruncateTime(myEndDate)
                     select a).ToList();

                List<TMEmployeeTime> list_TMEmployeeTime =
                    (from a in iQueryable_TMEmployeeTime
                     where
                     System.Data.Entity.DbFunctions.TruncateTime(a.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(myStartDate)
                     &&
                     System.Data.Entity.DbFunctions.TruncateTime(a.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(myEndDate)
                     select a).ToList();

                myResult.ItemsPM = (from a in list_TMEmployeeTime
                                    select new TMEmployeeTimePM()
                                    {
                                        Id = a.Id,
                                        Tenant = a.Tenant,
                                        CreateDate = a.CreateDate,
                                        CreatedByUserId = a.CreatedByUserId,
                                        UpdateDate = a.UpdateDate,
                                        UpdatedByUserId = a.UpdatedByUserId,
                                        EmployeeUserId = a.EmployeeUserId,
                                        LocationCode = a.LocationCode,
                                        DateOfWork = a.DateOfWork,
                                        Description = a.Description,
                                        Description_db = a.Description,
                                        TimeInMinutes = a.TimeInMinutes,
                                        TimeInMinutes_db = a.TimeInMinutes,
                                        WINumber = a.WINumber,
                                        WINumber_db = a.WINumber,
                                        ProjectId = a.ProjectId,
                                        ProjectId_db = a.ProjectId,
                                        SprintId = a.SprintId,
                                        SprintName = (allSprints.Where(d => d.Id == a.SprintId).FirstOrDefault() != null ? allSprints.Where(d => d.Id == a.SprintId).FirstOrDefault().Name : null),
                                        ProjectName = (allProjects.Where(d => d.Id == a.ProjectId).FirstOrDefault() != null ? allProjects.Where(d => d.Id == a.ProjectId).FirstOrDefault().Name : null),
                                        LocationName = (allLocations.Where(d => d.Code == a.LocationCode).FirstOrDefault() != null ? allLocations.Where(d => d.Code == a.LocationCode).FirstOrDefault().Name : null),
                                    }).ToList();


                DateTime? lastEntryTime = null;
                double totalMinutesFromClock = 0;
                foreach (var item in list_TMOfficeHour)
                {
                    DateTime? entry = item.EntryTime != null ? item.EntryTime : item.RecordedEntryTime;
                    DateTime? exit = item.ExitTime != null ? item.ExitTime : item.RecordedExitTime;

                    if (entry != null && exit != null)
                    {
                        totalMinutesFromClock += (exit.Value - entry.Value).TotalMinutes;
                    }

                    else if (entry != null && exit == null)
                    {
                        if (lastEntryTime == null)
                        {
                            lastEntryTime = entry;
                        }

                        else if (entry > lastEntryTime)
                        {
                            lastEntryTime = entry;
                        }
                    }
                }

                if (lastEntryTime != null)
                {
                    DateTime? iDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    int iDays = (int)(iDate.Value - lastEntryTime.Value).TotalDays;

                    if (iDays == 0)
                    {
                        totalMinutesFromClock += (iDate.Value - lastEntryTime.Value).TotalMinutes;
                    }

                    else
                    {
                        DateTime? lastEntryDayTime = new DateTime(lastEntryTime.Value.Year, lastEntryTime.Value.Month, lastEntryTime.Value.Day, 23, 59, 59);
                        totalMinutesFromClock += (lastEntryDayTime.Value - lastEntryTime.Value).TotalMinutes;
                    }
                }

                myResult.TotalMinutesFromClock = totalMinutesFromClock;
                myResult.TotalFromClock = GetTimeFormatFromMinutes(totalMinutesFromClock);
            }

            return myResult;
        }
        private void FillOfficeHours(TimeSheetItemDay newItem, DateTime date,bool OneDay)
        {
            List<TMOfficeHour> officeDays = officeHours.Where(a => System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) == System.Data.Entity.DbFunctions.TruncateTime(date)).ToList();

            double total = 0;
            double totalMinutesFromClock = 0;
            foreach (var item in officeDays)
            {
                DateTime? entry = item.EntryTime != null ? item.EntryTime : item.RecordedEntryTime;
                DateTime? exit = item.ExitTime != null ? item.ExitTime : item.RecordedExitTime;

                if (OneDay && officeDays.Count==1 && entry!=null && exit==null)
                {
                    exit = TenantServerConfigration.GetCurrentDateTime(item.Tenant);
                    total += Math.Round((exit.Value - entry.Value).TotalHours, 2);
                    totalMinutesFromClock += (exit.Value - entry.Value).TotalMinutes;
                }

                else
                {
                    if (entry != null && exit != null)
                    {
                        total += Math.Round((exit.Value - entry.Value).TotalHours, 2);
                        totalMinutesFromClock += (exit.Value - entry.Value).TotalMinutes;
                    }
                }
            }

            newItem.MinutesFromClock = totalMinutesFromClock;
            newItem.TotalFromClock = total;
            newItem.TotalFromClockString = DateFormat(total);
        }
        private string DateFormat(double time)
        {
            var result = "";
            if (time != 0)
            {
                var ts = TimeSpan.FromHours(time);
                var h = System.Math.Floor(ts.TotalHours);
                var m = (ts.TotalHours - h) * 60;
                result = h + ":" + m.ToString("00");
            }
            return result;
        }
        public HttpResponseMessage GetNewTMProjectConnect(string MainId,string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TMProject", "READ", tenant);

                string myResult = this.updateTimeProjectNumber(MainId, id,tenant);
                if (myResult == null)
                    throw new Exception("The selected project is a child of your project");
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private string updateTimeProjectNumber(string MainId,string id,int tenant) {
            myContext = TimeManagementContext.GetContext(tenant);
            TMProjectRepository repository = new TMProjectRepository(myContext);
            TMProject MainProject=repository.GetSingle(MainId,tenant);
            TMProject ConnectedProject = repository.GetSingle(id, tenant);

            if(ConnectedProject.ProjectNumber.StartsWith(MainProject.ProjectNumber))
            {
                return null;
            }
            else
            {
                List<TMProject> TMProjects = GETTMProjectsStartsWithProjectNumber(MainProject.ProjectNumber, tenant).ToList();
                string BaseProjectNumber = MainProject.ProjectNumber;
                MainProject.ProjectNumber = ConnectedProject.ProjectNumber + "-" + GenerateNewProjectNumber(ConnectedProject);
                MainProject.IsInnerProject = true;
                foreach (TMProject item in TMProjects)
                {
                    string[] ProjectNumberArray = item.ProjectNumber.Split('-');
                    string SubProjectNumber="";
                    bool StartCount = false;
                    foreach(string number in ProjectNumberArray)
                    {
                        if (StartCount == true)
                        {
                            SubProjectNumber += number + "-";
                        }
                        if (number.Equals(BaseProjectNumber))
                            StartCount = true;
                       
                    }
                    SubProjectNumber = SubProjectNumber.Remove(SubProjectNumber.Length - 1);

                    item.ProjectNumber = MainProject.ProjectNumber + "-" + SubProjectNumber;
                    repository.Update(item);
                }
                repository.Update(MainProject);
                repository.SubmitChanges();
                
            }
            return MainProject.ProjectNumber;

        }
        private string GenerateNewProjectNumber(TMProject entity) {
            TMProjectRepository entityRepository = new TMProjectRepository(entity.Tenant);
            List<string> ProjectNumbers = entityRepository.GetInnerTMProjectByNumber(entity.ProjectNumber, entity.Tenant);
            ProjectNumbers= ProjectNumbers.Select(p => p.Split('-').Last()).ToList();
            int ProjectId;
            if (ProjectNumbers.Count == 0)
                ProjectId = 1;
            else
            {
                string max = ProjectNumbers.Max();
                ProjectId = int.Parse(max) + 1;
            }

            return ProjectId + "";            
        }
        private IQueryable<TMProject> GETTMProjectsStartsWithProjectNumber(string ProjectNumber, int tenant)
        {
           
            return (from a in myContext.TMProjects
                    where a.ProjectNumber.StartsWith(ProjectNumber) && a.Tenant == tenant && a.ProjectNumber != ProjectNumber
                    select a);
        }
        public HttpResponseMessage GetProjectsCounts(string loggedUserId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                loggedUserId = this.FixFilter(loggedUserId);

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TMProject", "READ", tenant);

                TMProjectRepository projectRepository = new TMProjectRepository(tenant);
                IQueryable<TMProject> dataSource = projectRepository.GetAll(tenant);

                TMProjectSummary summaryClass = new TMProjectSummary() { Id = tenant };
                summaryClass.MyProjectsCount = dataSource.Where(d => d.OwnerId == loggedUserId).Count();
                summaryClass.AllProjectsCount = dataSource.Count();

                return Request.CreateResponse(HttpStatusCode.OK, summaryClass);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetUpdatedTimeSheetList(string Id, string employeeUserId, string locationCode, string periodStartDate, string exitDate)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
                TMEmployeeTimeUpdateService service = new TMEmployeeTimeUpdateService(myContext);
                TMEmployeeTimeRepository repository = new TMEmployeeTimeRepository(myContext);
                TMEmployeeTimeQueryService tmQueryService = new TMEmployeeTimeQueryService(myContext);

                if (Id == "null")
                {
                    Id = null;
                }
                if (employeeUserId == "null")
                {
                    employeeUserId = null;
                }
                if (locationCode == "null")
                {
                    locationCode = null;
                }
                if (periodStartDate == "null")
                {
                    periodStartDate = null;
                }
                if (exitDate == "null")
                {
                    exitDate = null;
                }

                TMEmployeeTime deletedItem = (from d in myContext.TMEmployeeTimes
                                                    where d.Tenant == tenant
                                                    && d.Id == Id
                                                    select d).FirstOrDefault();

                TimeManagementAPIHelper args = new TimeManagementAPIHelper();
                if (deletedItem != null)
                {
                    deletedItem.TimeInMinutes = 0;
                    service.SendQueueMessage(deletedItem.Id, deletedItem.WINumber, deletedItem.TimeInMinutes, deletedItem.Tenant);
                    repository.Remove(deletedItem);
                    repository.SubmitChanges();

                    DateTime? myStartDate = DateHelper.GetDate(periodStartDate);
                    DateTime? myExitDate = DateHelper.GetDate(exitDate);

                    args = FillDataEntryTimeSheetList(employeeUserId, locationCode, myStartDate, myExitDate, tenant);
                }
                return Request.CreateResponse(HttpStatusCode.OK, args);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private string FixFilter(string filter)
        {
            string myResult = filter;

            if (myResult != null)
            {
                switch (myResult.ToLower())
                {
                    case "all":
                    case "null":
                    case "undefined":
                        {
                            myResult = null;
                            break;
                        }
                }
            }

            return myResult;
        }
        public HttpResponseMessage GetTMProjects(string employeeUserId, string locationCode, string periodStartDate)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                employeeUserId = FixFilter(employeeUserId);

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TMEmployeeTime", "READ", tenant);

                DateTime? myStartDate = periodStartDate == "null" ? null : DateHelper.GetDate(periodStartDate);

                ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
                IQueryable<TMEmployeeTime> iQueryable = (from d in myContext.TMEmployeeTimes
                                                         where d.Tenant == tenant && d.DateOfWork != null
                                                         select d);

                List<TMProject> allProjects = (from d in myContext.TMProjects where d.Tenant == tenant select d).ToList();

                if (!string.IsNullOrEmpty(employeeUserId))
                {
                    iQueryable = iQueryable.Where(d => d.EmployeeUserId == employeeUserId);
                }

                if (!string.IsNullOrEmpty(locationCode) && locationCode != "A")
                {
                    iQueryable = iQueryable.Where(d => d.LocationCode == locationCode);
                }

                DateTime myEndDate = myStartDate.Value.AddDays(6);
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(myStartDate));
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(myEndDate));

                TFSParseWebhook myTFSParseWebhook = new TFSParseWebhook();
                List<TMEmployeeTime> projectsList = myTFSParseWebhook.GetProjects(iQueryable.ToList(), tenant);

                TimeManagementAPIHelper myResult = this.FillWeeklyTimeSheetList(employeeUserId, locationCode, myStartDate, tenant, projectsList);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTMProjectsByBatchTask(string employeeUserId,  string fromDate , string toDate)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TMEmployeeTime", "READ", tenant);

                DateTime? fromDate_ = fromDate == "null" ? null : DateHelper.GetDate(fromDate);
                DateTime? toDate_ = toDate == "null" ? null : DateHelper.GetDate(toDate);

                employeeUserId = FixFilter(employeeUserId);
                
                TMProjectDataArgs args = new TMProjectDataArgs() { EmployeeUserId = employeeUserId, FromDate = fromDate_, ToDate = toDate_, Tenant = tenant };
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(TMProjectDataArgs));
                serializer.Serialize(stringwriter, args);
                string xmlParameters = stringwriter.ToString();

                BatchTaskExecutionPM taskExe = new BatchTaskExecutionPM()
                {
                    Subject = "Get TM Projects",
                    Tenant = tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ClassName = "WebFreight.Web.Helpers.APIHelpers.TMProjectsHelper,WebFreight.Web",
                    CreateDate = DateTime.Now,
                    PrametersXml = xmlParameters,
                    StatusCode = "C",
                };

                IInfrastructureContext MyContext = InfrastructureContext.GetContext(tenant);
                BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                bteUpdateService.Update(taskExe, true);

                // 2- Send to queue
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
                queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", tenant.ToString() }
                });

                return Request.CreateResponse(HttpStatusCode.OK, taskExe);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private string GetTimeFormatFromMinutes(double minutes)
        {
            string iResult = "";

            if (minutes != 0)
            {
                TimeSpan iTimeSpan = TimeSpan.FromMinutes(Math.Abs(minutes));

                iResult = (int)iTimeSpan.TotalHours + ":" + iTimeSpan.Minutes.ToString("00");

                if (minutes < 0)
                {
                    iResult = "- " + iResult;
                }
            }

            return iResult;
        }
        public HttpResponseMessage GetProrate(string EmployeeUserId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TMEmployeeTime", "READ", tenant);

                ITimeManagementContext iContext = TimeManagementContext.GetContext(tenant);
                TMEmployeeTimeRepository iRepository = new TMEmployeeTimeRepository(iContext);

                var dataGroups = (from d in iContext.TMEmployeeTimes
                                  where
                                  d.NeedsProrating == true
                                  && d.EmployeeUserId == EmployeeUserId
                                  group d by new { d.EmployeeUserId, d.SprintId } into g
                                  select new
                                  {
                                      SprintId = g.Key.SprintId,
                                      EmployeeUserId = g.Key.EmployeeUserId,
                                  }).ToList();

                if (dataGroups.Count > 0)
                {
                    foreach (var itemGroup in dataGroups)
                    {
                        IQueryable<TMEmployeeTime> iQueryable = iRepository.GetAllWithoutTenant();
                        iQueryable = iQueryable.Where(d => d.SprintId == itemGroup.SprintId && d.EmployeeUserId == itemGroup.EmployeeUserId);

                        List<TMEmployeeTime> itemsProrated =
                            (from EmployeeTimes in iQueryable
                             join Projects in iContext.TMProjects on EmployeeTimes.ProjectId equals Projects.Id
                             where EmployeeTimes.ProjectId != null && EmployeeTimes.ProjectId != "" && Projects.IsProrated == true
                             select EmployeeTimes).ToList();

                        if (itemsProrated.Count > 0)
                        {
                            double itemsProratedMinutes = itemsProrated.Sum(s => s.TimeInMinutes);

                            if (itemsProratedMinutes > 0)
                            {
                                List<TMEmployeeTime> itemsNotProrated
                                    = (
                                    (from EmployeeTimes in iQueryable
                                     join Projects in iContext.TMProjects on EmployeeTimes.ProjectId equals Projects.Id
                                     where EmployeeTimes.ProjectId != null && EmployeeTimes.ProjectId != ""
                                     && Projects.IsProrated == false
                                     select EmployeeTimes)

                                     .Union

                                     (from EmployeeTimes in iQueryable
                                      where EmployeeTimes.ProjectId == null || EmployeeTimes.ProjectId == ""
                                      select EmployeeTimes)
                                      ).ToList();

                                double itemsNotProratedMinutes = itemsNotProrated.Sum(s => s.TimeInMinutes);

                                foreach (TMEmployeeTime item in itemsNotProrated)
                                {
                                    double iProratedDuration = item.TimeInMinutes / itemsNotProratedMinutes * itemsProratedMinutes;
                                    double iFullDuration = item.TimeInMinutes + iProratedDuration;

                                    item.ProratedDuration = Math.Round(iProratedDuration, 2);
                                    item.FullDuration = Math.Round(iFullDuration, 2);
                                    item.NeedsProrating = false;

                                    iRepository.Update(item);
                                }

                                foreach (TMEmployeeTime item in itemsProrated)
                                {
                                    item.ProratedDuration = 0;
                                    item.FullDuration = item.TimeInMinutes;
                                    item.NeedsProrating = false;
                                    iRepository.Update(item);
                                }

                                iRepository.SubmitChanges();
                            }
                        }                        
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCalculationCompleteWork()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TMEmployeeTime", "READ", tenant);

                TFSParseWebhook myTFSParseWebhook = new TFSParseWebhook();
                myTFSParseWebhook.CalculateCompletedWorkHours(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetVacationsSummary(int Year)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TMEmployeeTime", "READ", tenant);

                string loggedUserId = null;
                UserRepository userRepository = new UserRepository(tenant);
                User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, authToken.Email, tenant, true);
                if (loggedUser != null)
                {
                    loggedUserId = loggedUser.Id;
                }

                List<string> ProjectsNumbers = new List<string>();
                ProjectsNumbers.Add("1014");
                ProjectsNumbers.Add("1014-1");
                ProjectsNumbers.Add("1014-2");
                ProjectsNumbers.Add("1014-3");
                //ProjectsNumbers.Add("1125");
                ProjectsNumbers.Add("1015");
                ProjectsNumbers.Add("1015-1");
                ProjectsNumbers.Add("1015-2");

                ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
                var dataGroups = (from tMEmployeeTime in myContext.TMEmployeeTimes
                                  join tMProject in myContext.TMProjects on tMEmployeeTime.ProjectId equals tMProject.Id
                                  where
                                  tMEmployeeTime.EmployeeUserId == loggedUserId
                                  && tMEmployeeTime.DateOfWork.Year == Year
                                  &&
                                  (ProjectsNumbers.Contains(tMProject.ProjectNumber) || (tMProject.ProjectNumber == "1125" && tMEmployeeTime.TimeInMinutes == 540))
                                  group tMEmployeeTime by new { tMProject.ProjectNumber, tMEmployeeTime.ProjectId } into g
                                  select new
                                  {
                                      ProjectNumber = g.Key.ProjectNumber,
                                      TimeInMinutes = g.Sum(s => s.TimeInMinutes),
                                      Count = g.Count(),
                                  }).ToList();

                TMVacationsSummary iResult = new TMVacationsSummary();
                iResult.Holidays = dataGroups.Where(d => d.ProjectNumber == "1125").Sum(s => s.Count);
                iResult.Vacations = dataGroups.Where(d => d.ProjectNumber == "1014" || d.ProjectNumber == "1014-1").Sum(s => s.Count);
                iResult.HalfVacations = dataGroups.Where(d => d.ProjectNumber == "1014-2").Sum(s => s.Count);
                iResult.UnpaidVacations = dataGroups.Where(d => d.ProjectNumber == "1014-3").Sum(s => s.Count);
                iResult.SicknessVacations = dataGroups.Where(d => d.ProjectNumber == "1015" || d.ProjectNumber == "1015-2").Sum(s => s.Count);
                iResult.SickLeavesMinutes = dataGroups.Where(d => d.ProjectNumber == "1015-1").Sum(s => s.TimeInMinutes);
                iResult.SickLeaves = GetTimeFormatFromMinutes(iResult.SickLeavesMinutes);

                if (string.IsNullOrEmpty(iResult.SickLeaves))
                {
                    iResult.SickLeaves = "0";
                }

                return Request.CreateResponse(HttpStatusCode.OK, iResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetVacationsDetails(int Year, string Type)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TMEmployeeTime", "READ", tenant);

                Type = this.FixFilter(Type);

                List<TMVacationsDetails> myResult = new List<TMVacationsDetails>();

                if (!string.IsNullOrEmpty(Type))
                {
                    List<string> ProjectsNumbers = new List<string>();
                    switch (Type)
                    {
                        case "Holidays": { ProjectsNumbers.Add("1125"); break; }
                        case "Vacations": { ProjectsNumbers.Add("1014"); ProjectsNumbers.Add("1014-1"); break; }
                        case "Half Vacations": { ProjectsNumbers.Add("1014-2"); break; }
                        case "Unpaid Vacations": { ProjectsNumbers.Add("1014-3"); break; }
                        case "Sickness Vacations": { ProjectsNumbers.Add("1015"); ProjectsNumbers.Add("1015-2"); break; }
                        case "Sick Leaves": { ProjectsNumbers.Add("1015-1"); break; }
                    }

                    if (ProjectsNumbers.Count > 0)
                    {
                        string loggedUserId = null;
                        UserRepository userRepository = new UserRepository(tenant);
                        User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, authToken.Email, tenant, true);
                        if (loggedUser != null)
                        {
                            loggedUserId = loggedUser.Id;
                        }

                        ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);

                        myResult = (from tMEmployeeTime in myContext.TMEmployeeTimes
                                    join tMProject in myContext.TMProjects on tMEmployeeTime.ProjectId equals tMProject.Id
                                    where
                                    tMEmployeeTime.EmployeeUserId == loggedUserId
                                    && tMEmployeeTime.DateOfWork.Year == Year
                                    && ProjectsNumbers.Contains(tMProject.ProjectNumber)
                                    select new TMVacationsDetails()
                                    {
                                        DateOfWork = tMEmployeeTime.DateOfWork,
                                        TimeInMinutes = tMEmployeeTime.TimeInMinutes
                                    }).OrderByDescending(o => o.DateOfWork).ToList();

                        if(Type == "Holidays")
                        {
                            myResult = myResult.Where(d => d.TimeInMinutes == 540).ToList();
                        }

                        if (Type == "Sick Leaves")
                        {
                            foreach (TMVacationsDetails item in myResult)
                            {
                                item.SickLeaves = GetTimeFormatFromMinutes(item.TimeInMinutes);

                                if (string.IsNullOrEmpty(item.SickLeaves))
                                {
                                    item.SickLeaves = "0";
                                }
                            }
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    public class TimeManagementAPIHelper
    {
        public TimeManagementAPIHelper()
        {
            this.OfficeClockDays = new List<TimeSheetItemDay>();
            this.Items = new List<TimeSheetItem>();
        }
        public int Id { get; set; }
        public string LocationCode { get; set; }
        public string EmployeeUserId { get; set; }
        public string TotalFromClock { get; set; }
        public double TotalMinutesFromClock { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<TimeSheetItem> Items { get; set; }
        public List<TMEmployeeTimePM> ItemsPM { get; set; }
        public List<TimeSheetItemDay> OfficeClockDays { get; set; }
    }
    public class TimeSheetItem
    {
        public TimeSheetItem()
        {
            this.Days = new List<TimeSheetItemDay>();
        }

        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string WINumber { get; set; }
        public string LocationCode { get; set; }
        public string EmployeeUserId { get; set; }
        public int? TotalMinutes { get; set; }
        public bool IsHeaderUpdated { get; set; }
        public List<TimeSheetItemDay> Days { get; set; }
        public string ProjectId_db { get; set; }
        public string Description_db { get; set; }
        public string WINumber_db { get; set; }
    }
    public class TimeSheetItemDay
    {
        public int Index { get; set; }
        public DateTime Date { get; set; }
        public int? Minuts { get; set; }
        public int? Minuts_db { get; set; }
        public double? TotalFromClock { get; set; }
        public string TotalFromClockString { get; set; }
        public double MinutesFromClock { get; set; }
    }
    public class TMProjectSummary
    {
        public int Id { get; set; }
        public int MyProjectsCount { get; set; }
        public int AllProjectsCount { get; set; }
    }
    public class TMProjectDataArgs
    {
        public string EmployeeUserId { get; set; }
        public int Tenant { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class TMVacationsSummary
    {
        public double Holidays { get; set; }
        public double Vacations { get; set; }
        public double HalfVacations { get; set; }
        public double UnpaidVacations { get; set; }
        public double SicknessVacations { get; set; }
        public double SickLeavesMinutes { get; set; }
        public string SickLeaves { get; set; }

        //-- Sickness Vacations	    1015
        //-- Sick Leave			    1015-1
        //-- Medical Vacation		1015-2

        //-- Holidays				1125

        //-- Vacations			    1014	
        //-- Annual Vacation		1014-1
        //-- Half Vacation		    1014-2
        //-- Unpaid Vacation		1014-3
    }
    public class TMVacationsDetails
    {
        public DateTime? DateOfWork { get; set; }
        public int TimeInMinutes { get; set; }
        public string SickLeaves { get; set; }
    }
}