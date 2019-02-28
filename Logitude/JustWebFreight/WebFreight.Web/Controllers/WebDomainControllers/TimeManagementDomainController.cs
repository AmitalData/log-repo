using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools.Counters;
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
                            if (itemPOCO != null && !(itemPOCO.ProjectId == itemChanged.ProjectId && itemPOCO.Description == itemChanged.Description && itemPOCO.WINumber == itemChanged.WINumber))
                            {
                                itemPOCO.ProjectId = itemChanged.ProjectId;
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
            int count = 1;
            TimeManagementAPIHelper myResult = new TimeManagementAPIHelper()
            {
                Id = tenant,
                EmployeeUserId = employeeUserId,
                LocationCode = locationCode,
                StartDate = myStartDate,
            };

            ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
            IQueryable<TMEmployeeTime> iQueryable = (from d in myContext.TMEmployeeTimes
                                                     where d.Tenant == tenant && d.DateOfWork != null
                                                     select d);
            List<TMProject> allProjects = (from d in myContext.TMProjects where d.Tenant == tenant select d).ToList();
            List<TMLocation> allLocations = (from d in myContext.TMLocations select d).ToList();

            if (!string.IsNullOrEmpty(employeeUserId))
            {
                iQueryable = iQueryable.Where(d => d.EmployeeUserId == employeeUserId);
            }
            if (!string.IsNullOrEmpty(locationCode) && locationCode != "A")
            {
                iQueryable = iQueryable.Where(d => d.LocationCode == locationCode);
            }

            if (myEndDate != null)
            {
                count = (int)(myEndDate.Value - myStartDate.Value).TotalDays;
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(myStartDate));
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(myEndDate));
                officeHours = (from a in myContext.TMOfficeHours
                               where a.Tenant == tenant && !a.Inactive && a.UserId == employeeUserId && a.WorkDate != null &&
                               System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) >= System.Data.Entity.DbFunctions.TruncateTime(myStartDate) &&
                               System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) <= System.Data.Entity.DbFunctions.TruncateTime(myEndDate)
                               select a);
            }
            else
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) == System.Data.Entity.DbFunctions.TruncateTime(myStartDate));
                officeHours = (from a in myContext.TMOfficeHours
                               where a.Tenant == tenant && !a.Inactive && a.UserId == employeeUserId && a.WorkDate != null &&
                               System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) == System.Data.Entity.DbFunctions.TruncateTime(myStartDate)
                               select a);
            }

            //List<DateTime> workDays = iQueryable.Select(a => a.DateOfWork).ToList();
            //officeHours = officeHours.Where(a => workDays.Contains(System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate)));

            double? totalOfficeHours = 0;
            for (int i = 0; i <= count; i++)
            {
                var date = myStartDate.Value.AddDays(i);
                TimeSheetItemDay newItemDay = new TimeSheetItemDay();
                newItemDay.Index = i;
                newItemDay.Date = date;
                if (count == 0)
                    this.FillOfficeHours(newItemDay, date,true);
                else
                {
                    this.FillOfficeHours(newItemDay, date,false);
                }
                totalOfficeHours += newItemDay.TotalFromClock;
            }

            myResult.TotalFromClock = DateFormat(totalOfficeHours.Value);
            List<TMEmployeeTime> list = iQueryable.ToList();
            List<TMEmployeeTimePM> listPM = (from a in list
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
                                                 SprintId=a.SprintId,
                                                 ProjectName = (allProjects.Where(d => d.Id == a.ProjectId).FirstOrDefault() != null ? allProjects.Where(d => d.Id == a.ProjectId).FirstOrDefault().Name : null),
                                                 LocationName = (allLocations.Where(d => d.Code == a.LocationCode).FirstOrDefault() != null ? allLocations.Where(d => d.Code == a.LocationCode).FirstOrDefault().Name : null),
                                             }).ToList();

            myResult.ItemsPM = listPM;
            return myResult;
        }


        private void FillOfficeHours(TimeSheetItemDay newItem, DateTime date,bool OneDay)
        {
            List<TMOfficeHour> officeDays = officeHours.Where(a => System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) == System.Data.Entity.DbFunctions.TruncateTime(date)).ToList();
            double total = 0;
            foreach (var item in officeDays)
            {
                DateTime? entry = item.EntryTime != null ? item.EntryTime : item.RecordedEntryTime;
                DateTime? exit = item.ExitTime != null ? item.ExitTime : item.RecordedExitTime;
                if (OneDay && officeDays.Count==1 && entry!=null && exit==null)
                {
                    exit = TenantServerConfigration.GetCurrentDateTime(item.Tenant);
                    total += Math.Round((exit.Value - entry.Value).TotalHours, 2);

                }
                else
                {
                    if (entry != null && exit != null)
                    {
                        total += Math.Round((exit.Value - entry.Value).TotalHours, 2);
                    }
                }
            }

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
        //public HttpResponseMessage GetTimeOfficeClock(string employeeUserId, string FromDate, string ToDate)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        int tenant = authToken.Tenant;
        //        string loggedUserEmail = authToken.Email;

        //        SecurityUtility.AuthenticationOnTenant(tenant);
        //        SecurityUtility.CheckContactFeature("TMOfficeHour", "READ", tenant);

        //        List<TMOfficeHour> myResult = this.GetTimeOffice(employeeUserId, FromDate, ToDate, tenant);
        //        return Request.CreateResponse(HttpStatusCode.OK, myResult);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}
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
        //private List<TMOfficeHour> GetTimeOffice(string employeeUserId, string FromDate, string ToDate, int tenant)
        //{

        //    DateTime? myStartDate = DateHelper.GetDate(FromDate);

        //    if (myStartDate == null)
        //    {
        //        throw new ApplicationException("Please select from date");
        //    }

        //    DateTime? myEndDate = DateHelper.GetDate(ToDate);

        //    if (myEndDate == null)
        //    {
        //        throw new ApplicationException("Please select to date");
        //    }

        //    ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
        //    IQueryable<TMOfficeHour> iQueryable = (from d in myContext.TMOfficeHours
        //                                           where d.Tenant == tenant && d.UserId == employeeUserId && d.WorkDate >= myStartDate && d.WorkDate <= myEndDate
        //                                           select d);

        //    return iQueryable.ToList();

        //}
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
        //public bool IsUpdated { get; set; }
        //public string ProjectId { get; set; }
    }
    public class TMProjectSummary
    {
        public int Id { get; set; }
        public int MyProjectsCount { get; set; }
        public int AllProjectsCount { get; set; }
    }
}