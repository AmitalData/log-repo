using Logitude.CRM.BL.DataContracts;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.CRM.Data.BusinessUnitFilters;
using Logitude.Server.Tools.Helpers;
using System.Globalization;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.CRM.BL.WorkRoles;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class TicketQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, TicketPM entityPM)
        {
            ICRMContext context = MainContext as ICRMContext;
            TicketKeys ticketKeys = entityKeys as TicketKeys;
        }

        public List<CRMChartingClass> GetOpenTicketsGroupByClassification(string ownerId, string employeeGroupId, int tenant, bool isTopTen)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;
            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            TicketStageRepository repository = new TicketStageRepository(tenant);
            TicketStage resolveStage = repository.GetTicketStageByCode("RE", tenant);
            TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

            IQueryable<Ticket> dataSourceQuery =
                (from d in context.Tickets.Include("MainClassification")
                 where d.Tenant == tenant
                 && d.MainClassificationId != null
                 && (d.MainClassification.ParentId == tenant.ToString() || d.MainClassificationId == tenant.ToString())
                 && d.IsCancelled == false
                 && d.IsClosed == false
                 && d.StageId != resolveStage.Id
                 && d.StageId != closedStage.Id
                 select d);

            //TicketBusinessUnitFilter filter = new TicketBusinessUnitFilter(tenant);
            //dataSourceQuery = filter.RunFilter(dataSourceQuery);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            if (dataSourceQuery != null)
            {
                if (isTopTen)
                {
                    myResult =
                        (from d in dataSourceQuery
                         group d by new { d.MainClassificationId, d.MainClassification.Name } into g
                         select new CRMChartingClass()
                         {
                             Id = g.Key.MainClassificationId,
                             StringProperty = g.Key.Name,
                             IntegerProperty = g.Count(),
                             OwnerId = ownerId,
                             ClassificationId = g.Key.MainClassificationId,
                         })
                         .OrderByDescending(o => o.IntegerProperty)
                         .Take(10)
                         .ToList();
                }

                else
                {
                    myResult =
                        (from d in dataSourceQuery
                         group d by new { d.MainClassificationId, d.MainClassification.Name } into g
                         select new CRMChartingClass()
                         {
                             Id = g.Key.MainClassificationId,
                             StringProperty = g.Key.Name,
                             IntegerProperty = g.Count(),
                             OwnerId = ownerId,
                             ClassificationId = g.Key.MainClassificationId,
                         })
                         .ToList();
                }
            }

            return myResult;
        }

        public List<CRMChartingClass> GetOpenTicketsByDueTime(string ownerId, string employeeGroupId, int tenant, bool isTopTen)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            TicketStageRepository repository = new TicketStageRepository(tenant);
            TicketStage resolveStage = repository.GetTicketStageByCode("RE", tenant);
            TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

            IQueryable<Ticket> dataSourceQuery =
                (from d in context.Tickets
                 where d.Tenant == tenant
                 && d.IsClosed == false
                 && d.IsCancelled == false
                 && (d.StageId != resolveStage.Id && d.StageId != closedStage.Id)
                 select d);

            //TicketBusinessUnitFilter filter = new TicketBusinessUnitFilter(tenant);
            //dataSourceQuery = filter.RunFilter(dataSourceQuery);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            List<Ticket> itemsSource = dataSourceQuery.ToList();
            List<Ticket> noResolveTimeList = itemsSource.Where(d => d.FullResolvedTime == null && d.ResolveWithinDue != null).ToList();
            List<Ticket> noFirstResponseList = itemsSource.Where(d => d.FirstResponseTime == null && d.FirstResponseDue != null).ToList();

            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            int i = 0;

            #region  Resolve  Data

            List<Ticket> noResolveTimeList_Overdue = new List<Ticket>(), noResolveTimeList_Due1H = new List<Ticket>(), noResolveTimeList_Due2H = new List<Ticket>(), noResolveTimeList_Due4H = new List<Ticket>(), noResolveTimeList_Due8H = new List<Ticket>();

            if (noResolveTimeList != null && noResolveTimeList.Count() > 0)
            {
                noResolveTimeList_Overdue = noResolveTimeList.Where(d => d.ResolveWithinDue < todayDateTime).ToList();
                i += 1;
                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    DataTypeCode = "RE",
                    LabelProperty = "Overdue",
                    LabelColor = "Red",
                    OwnerId = ownerId,
                    IntegerProperty = noResolveTimeList_Overdue.Count(),
                });

                noResolveTimeList_Due1H = noResolveTimeList.Where(d => d.ResolveWithinDue != null && d.FirstResponseTime != null && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes > 0 && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes <= 60).ToList();
                i += 1;
                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    DataTypeCode = "RE",
                    LabelProperty = "due < 1h",
                    LabelColor = "Black",
                    OwnerId = ownerId,
                    IntegerProperty = noResolveTimeList_Due1H.Count(),
                });

                noResolveTimeList_Due2H = noResolveTimeList.Where(d => d.ResolveWithinDue != null && d.FirstResponseTime != null && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes > 60 && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes <= 120).ToList();
                i += 1;
                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    DataTypeCode = "RE",
                    LabelProperty = "due < 2h",
                    LabelColor = "Black",
                    OwnerId = ownerId,
                    IntegerProperty = noResolveTimeList_Due2H.Count(),
                });

                noResolveTimeList_Due4H = noResolveTimeList.Where(d => d.ResolveWithinDue != null && d.FirstResponseTime != null && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes > 120 && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes <= 240).ToList();
                i += 1;
                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    DataTypeCode = "RE",
                    LabelProperty = "due < 4h",
                    LabelColor = "Black",
                    OwnerId = ownerId,
                    IntegerProperty = noResolveTimeList_Due4H.Count(),
                });

                noResolveTimeList_Due8H = noResolveTimeList.Where(d => d.ResolveWithinDue != null && d.FirstResponseTime != null && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes > 240 && (d.ResolveWithinDue.Value - todayDateTime).TotalMinutes <= 480).ToList();
                i += 1;
                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    DataTypeCode = "RE",
                    LabelProperty = "due < 8h",
                    LabelColor = "Black",
                    OwnerId = ownerId,
                    IntegerProperty = noResolveTimeList_Due8H.Count(),
                });
            }
            #endregion

            #region First Respone Data
            if (noFirstResponseList != null && noFirstResponseList.Count() > 0)
            {
                i += 1;
                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    DataTypeCode = "FR",
                    LabelProperty = "Overdue",
                    LabelColor = "Red",
                    OwnerId = ownerId,
                    IntegerProperty = noFirstResponseList.Where(d => d.FirstResponseDue < todayDateTime && d.ResolveWithinDue > todayDateTime && d.FullResolvedTime == null).Where(a => !noResolveTimeList_Overdue.Contains(a)).Count(),
                });

                i += 1;
                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    DataTypeCode = "FR",
                    LabelProperty = "due < 1h",
                    LabelColor = "Black",
                    OwnerId = ownerId,
                    IntegerProperty = noFirstResponseList.Where(d => d.FirstResponseDue != null && (d.FirstResponseDue.Value - todayDateTime).TotalMinutes > 0 && (d.FirstResponseDue.Value - todayDateTime).TotalMinutes <= 60).Where(a => !noResolveTimeList_Due1H.Contains(a)).Count(),
                });

                i += 1;
                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    DataTypeCode = "FR",
                    LabelProperty = "due < 2h",
                    LabelColor = "Black",
                    OwnerId = ownerId,
                    IntegerProperty = noFirstResponseList.Where(d => d.FirstResponseDue != null && (d.FirstResponseDue.Value - todayDateTime).TotalMinutes > 60 && (d.FirstResponseDue.Value - todayDateTime).TotalMinutes <= 120).Where(a => !noResolveTimeList_Due2H.Contains(a)).Count(),
                });

                i += 1;
                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    DataTypeCode = "FR",
                    LabelProperty = "due < 4h",
                    LabelColor = "Black",
                    OwnerId = ownerId,
                    IntegerProperty = noFirstResponseList.Where(d => d.FirstResponseDue != null && (d.FirstResponseDue.Value - todayDateTime).TotalMinutes > 120 && (d.FirstResponseDue.Value - todayDateTime).TotalMinutes <= 240).Where(a => !noResolveTimeList_Due4H.Contains(a)).Count(),
                });

                i += 1;
                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    DataTypeCode = "FR",
                    LabelProperty = "due < 8h",
                    LabelColor = "Black",
                    OwnerId = ownerId,
                    IntegerProperty = noFirstResponseList.Where(d => d.FirstResponseDue != null && (d.FirstResponseDue.Value - todayDateTime).TotalMinutes > 240 && (d.FirstResponseDue.Value - todayDateTime).TotalMinutes <= 480).Where(a => !noResolveTimeList_Due8H.Contains(a)).Count(),
                });
            }

            #endregion

            return myResult;
        }

        #region Closed and Resolved Tickets

        // Closed Ticket By Classifications 
        public List<CRMChartingClass> GetClosedTicketsGroupByClassification(string code, string ownerId, string employeeGroupId, int tenant)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

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

            TicketStageRepository repository = new TicketStageRepository(tenant);
            TicketStage resolveStage = repository.GetTicketStageByCode("RE", tenant);
            TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

            IQueryable<Ticket> dataSource =
                  (from d in context.Tickets.Include("MainClassification")
                   where d.Tenant == tenant
                   && d.MainClassificationId != null
                   && (d.MainClassification.ParentId == tenant.ToString())
                   && (d.StageId == resolveStage.Id || d.StageId == closedStage.Id)
                   && d.FirstResolveDate != null
                   select d);


            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                dataSource = dataSource.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (days >= 0)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate.Value) == todayDate);
            }

            else if (days == -1)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate.Value) == date1);
            }

            else
            {
                dataSource = dataSource.Where(d => d.FirstResolveDate.Value >= date1 && d.FirstResolveDate.Value <= date2);
            }

            myResult = (from d in dataSource
                        group d by new { d.MainClassificationId, d.MainClassification.Name } into g
                        select new CRMChartingClass()
                        {
                            Id = g.Key.MainClassificationId,
                            StringProperty = g.Key.Name,
                            IntegerProperty = g.Count(),
                            OwnerId = ownerId,
                            ClassificationId = g.Key.MainClassificationId,

                        }).ToList();

            return myResult;
        }

        // Closed Ticket By Severity
        public List<CRMChartingClass> GetClosedTicketsGroupBySeverity(string code, string ownerId, string employeeGroupId, int tenant)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

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

            TicketStageRepository repository = new TicketStageRepository(tenant);
            TicketStage resolveStage = repository.GetTicketStageByCode("RE", tenant);
            TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

            IQueryable<Ticket> dataSource =
                dataSource =
                  (from d in context.Tickets
                   where d.Tenant == tenant
                   && (d.StageId == resolveStage.Id || d.StageId == closedStage.Id)
                   && d.FirstResolveDate != null
                   select d);


            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                dataSource = dataSource.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (days >= 0)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate.Value) == todayDate);
            }

            else if (days == -1)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate.Value) == date1);
            }

            else
            {
                dataSource = dataSource.Where(d => d.FirstResolveDate.Value >= date1 && d.FirstResolveDate.Value <= date2);
            }

            myResult = (from d in dataSource
                        group d by new { d.SeverityId, d.Severity.Name } into g
                        select new CRMChartingClass()
                        {
                            Id = g.Key.SeverityId,
                            StringProperty = g.Key.Name,
                            IntegerProperty = g.Count(),
                            OwnerId = ownerId,
                            SeverityId = g.Key.SeverityId,
                        }).ToList();

            return myResult;
        }

        // Closed Ticket By Type
        public List<CRMChartingClass> GetClosedTicketsGroupByType(string code, string ownerId, string employeeGroupId, int tenant)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? date1 = helper.Date1;
            DateTime? date2 = helper.Date2;

            if (code == "-1")
            {
                date2 = date1;
            }

            if (code == "-7")
            {
                date1 = date1.Value.AddDays(1);
            }

            int days = helper.Days;

            TicketStageRepository repository = new TicketStageRepository(tenant);
            TicketStage resolveStage = repository.GetTicketStageByCode("RE", tenant);
            TicketStage closedStage = repository.GetTicketStageByCode("CS", tenant);

            IQueryable<Ticket> dataSource =
                dataSource =
                  (from d in context.Tickets
                   where d.Tenant == tenant
                   && (d.StageId == resolveStage.Id || d.StageId == closedStage.Id)
                   && d.FirstResolveDate != null
                   select d);

            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                dataSource = dataSource.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (days >= 0)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate.Value) == todayDate);
            }

            else if (days == -1)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate.Value) == date1);
            }

            else
            {
                dataSource = dataSource.Where(d => d.FirstResolveDate.Value >= date1 && d.FirstResolveDate.Value <= date2);
            }

            myResult = (from d in dataSource.Where(a=>a.TicketTypeId != null)
                        group d by new { d.TicketTypeId, d.TicketType.Name } into g
                        select new CRMChartingClass()
                        {
                            Id = g.Key.TicketTypeId,
                            StringProperty = g.Key.Name,
                            IntegerProperty = g.Count(),
                            OwnerId = ownerId,
                            TicketTypeId = g.Key.TicketTypeId,
                        }).ToList();

            #region No Types
            if (dataSource.Where(a => a.TicketTypeId == null).Count() > 0)
            {
                int i = myResult.Count();
                myResult.Add(new CRMChartingClass()
                {
                    Id = i++ + "",
                    StringProperty = "No Type",
                    IntegerProperty = dataSource.Where(a => a.TicketTypeId == null).Count(),
                });
            }
            #endregion 

            return myResult;
        }

        // Closed Ticket By SLA Violation 
        public List<CRMChartingClass> GetClosedTicketsBySLAViolation(int selectedIndex, string code, string ownerId, string employeeGroupId, int tenant)
        {
            ICRMContext myContext = CRMContext.GetContext(tenant);

            TicketStageRepository repository = new TicketStageRepository(myContext);
            TicketStage stage1 = repository.GetTicketStageByCode("RE", tenant);
            TicketStage stage2 = repository.GetTicketStageByCode("CS", tenant);

            DatesHelper helper = MethodHelper.GetDates(code, tenant);
            DateTime? date1 = helper.Date1 == null ? helper.Date1 : helper.Date1.Value.Date;
            DateTime? date2 = helper.Date2 == null ? helper.Date2 : helper.Date2.Value.Date;

            if (code == "-1")
            {
                date2 = date1;
            }

            if (code == "-7")
            {
                date1 = date1.Value.AddDays(1);
            }

            IQueryable<Ticket> iQueryableTickets = (from d in myContext.Tickets
                                                    where d.Tenant == tenant
                                                    && (d.StageId == stage1.Id || d.StageId == stage2.Id)
                                                    &&
                                                    (
                                                    (d.FirstResponseTime != null && d.FirstResponseDue >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.FirstResponseDue) <= date2)
                                                    ||
                                                    (d.FirstResolveDate != null && d.ResolveWithinDue >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.ResolveWithinDue) <= date2)
                                                    )
                                                    select d);

            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                iQueryableTickets = iQueryableTickets.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                iQueryableTickets = iQueryableTickets.Where(d => d.OwnerId == ownerId);
            }

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

            #region myData
            List<Ticket> allTickets = iQueryableTickets.ToList();
            List<SLAJoinedData> allData = new List<SLAJoinedData>();
            foreach (Ticket item in allTickets)
            {
                if ((item.FirstResolveDate > item.ResolveWithinDue) || (item.FirstResolveDate > item.ResolveWithinDue && item.FirstResponseTime > item.FirstResponseDue))
                {
                    allData.Add(new SLAJoinedData
                    {
                        Id = item.Id,
                        TicketId = item.Id,
                        Date = item.ResolveWithinDue.Value,
                        EscalationFor = "RW",
                        CreateDate = item.CreateDate.Value,
                    });
                }

                else if (item.FirstResponseTime > item.FirstResponseDue)
                {
                    allData.Add(new SLAJoinedData
                    {
                        Id = item.Id,
                        TicketId = item.Id,
                        Date = item.FirstResponseDue.Value,
                        EscalationFor = "FR",
                        CreateDate = item.CreateDate.Value,
                    });
                }
            }

            List<SLAJoinedData> myData = (from d in allData
                                          group d by new { d.Date, d.EscalationFor, d.CreateDate } into g
                                          select new SLAJoinedData()
                                          {
                                              Date = g.Key.Date,
                                              EscalationFor = g.Key.EscalationFor,
                                          }).ToList();
            #endregion

            string groupByCode = "D";
            if (code == "-30" || code == "-30_1" || code == "0_1")
            {
                groupByCode = "W";
            }
            else if (code == "-90" || code == "-365" || code == "-365_1" || code == "-90_1")
            {
                groupByCode = "M";
            }

            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            if (myData.Count > 0)
            {
                int i = 0;
                List<CRMChartingClass> myGroupedData = this.GetSLAGroupedData(myData, groupByCode);

                int indexOrder = 0;
                i = myGroupedData.Count();
                while (date1 <= date2)
                {
                    CRMChartingClass myItem_FR = null;
                    CRMChartingClass myItem_RW = null;

                    if (groupByCode == "D")
                    {
                        //theCustomFilterValue = date1.ToString() + ":" + date1.ToString();
                        myItem_FR = myGroupedData.Where(d => d.DateTimeProperty.Day == date1.Value.Day && d.DataTypeCode == "FR").FirstOrDefault();
                        myItem_RW = myGroupedData.Where(d => d.DateTimeProperty.Day == date1.Value.Day && d.DataTypeCode == "RW").FirstOrDefault();
                    }

                    else if (groupByCode == "W")
                    {
                        //DateTime nextWeek = date1.Value.AddDays(6);
                        int day = Convert.ToInt32(date1.Value.DayOfWeek);
                        DateTime startOfWeek = date1.Value.AddDays((-1 * day));
                        DateTime endOfWeek = date1.Value.AddDays((6 - day));

                        myItem_FR = myGroupedData.Where(d => d.DateTimeProperty >= startOfWeek && d.DateTimeProperty <= endOfWeek && d.DataTypeCode == "FR").FirstOrDefault();
                        myItem_RW = myGroupedData.Where(d => d.DateTimeProperty >= startOfWeek && d.DateTimeProperty <= endOfWeek && d.DataTypeCode == "RW").FirstOrDefault();
                    }

                    else
                    {
                        DateTime filterDate = new DateTime(date1.Value.Year, date1.Value.Month, 1);
                        myItem_FR = myGroupedData.Where(d => d.DateTimeProperty.Year == filterDate.Year && d.DateTimeProperty.Month == filterDate.Month && d.DataTypeCode == "FR").FirstOrDefault();
                        myItem_RW = myGroupedData.Where(d => d.DateTimeProperty.Year == filterDate.Year && d.DateTimeProperty.Month == filterDate.Month && d.DataTypeCode == "RW").FirstOrDefault();
                    }

                    if (myItem_FR == null)
                    {
                        myItem_FR = new CRMChartingClass()
                        {
                            Id = i++ + "",
                            DataTypeCode = "FR",
                            IntegerProperty = 0,
                            IndexOrder = indexOrder,
                        };
                    }

                    else
                    {
                        myItem_FR.IndexOrder = indexOrder;
                    }

                    indexOrder++;

                    if (myItem_RW == null)
                    {
                        myItem_RW = new CRMChartingClass()
                        {
                            Id = i++ + "",
                            DataTypeCode = "RW",
                            IntegerProperty = 0,
                            IndexOrder = indexOrder,
                        };
                    }

                    else
                    {
                        myItem_RW.IndexOrder = indexOrder;
                    }

                    indexOrder++;

                    myResult.Add(myItem_FR);
                    myResult.Add(myItem_RW);

                    if (groupByCode == "D")
                    {
                        myItem_FR.LabelProperty = code == "-7" ? string.Format("{0:ddd}", date1) : date1.Value.DayOfWeek.ToString();
                        myItem_RW.LabelProperty = code == "-7" ? string.Format("{0:ddd}", date1) : date1.Value.DayOfWeek.ToString();
                        date1 = date1.Value.AddDays(1);
                        if (date1.Value.Day == date2.Value.Day)
                        {
                            //date1 = date2.Value.AddDays(1);
                        }
                    }

                    else if (groupByCode == "W")
                    {
                        int day = Convert.ToInt32(date1.Value.DayOfWeek);
                        DateTime startOfWeek = date1.Value.AddDays((-1 * day));
                        DateTime endOfWeek = date1.Value.AddDays((6 - day));

                        myItem_FR.StartDate = startOfWeek;
                        myItem_FR.EndDate = endOfWeek;
                        myItem_RW.StartDate = startOfWeek;
                        myItem_RW.EndDate = endOfWeek;

                        myItem_FR.LabelProperty = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;
                        myItem_RW.LabelProperty = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;

                        date1 = date1.Value.AddDays(7);
                    }

                    else
                    {
                        myItem_FR.LabelProperty = string.Format("{0:MMM}", date1) + " " + string.Format("{0:yy}", date1) + "’";
                        myItem_RW.LabelProperty = string.Format("{0:MMM}", date1) + " " + string.Format("{0:yy}", date1) + "’";
                        date1 = date1.Value.AddMonths(1);

                        if (date1.Value.Month == date2.Value.Month)
                        {
                            date1 = date2;
                        }
                    }
                }
            }

            return myResult;
        }

        // Closed Ticket By Solved Stage 
        public List<CRMChartingClass> GetClosedTicketsBySolvedStage(int selectedIndex, string code, string ownerId, string employeeGroupId, int tenant)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

            DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

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
            TicketStage resolveStage = repository.GetTicketStageByCode("RE", tenant);
            TicketStage closedstage = repository.GetTicketStageByCode("CS", tenant);

            IQueryable<Ticket> dataSourceQuery =
                  (from d in context.Tickets
                   where d.Tenant == tenant
                   && (d.StageId == resolveStage.Id || d.StageId == closedstage.Id)
                   && System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate) >= date1
                   && System.Data.Entity.DbFunctions.TruncateTime(d.FirstResolveDate) <= date2
                   select d);

            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.OwnerId == ownerId);
            }

            List<Ticket> myData = dataSourceQuery.ToList();

            string groupByCode = "D";

            if (code == "-30" || code == "-30_1" || code == "0_1")
            {
                groupByCode = "W";
            }
            else if (code == "-90" || code == "-365" || code == "-365_1" || code == "-90_1")
            {
                groupByCode = "M";
            }

            if (myData.Count > 0)
            {
                int i = 0;
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                List<CRMChartingClass> myGroupedData = this.GetSolvedTicketGroupedData(myData, groupByCode);

                int indexOrder = 0;
                i = myGroupedData.Count();
                while (date1 <= date2)
                {
                    CRMChartingClass myItem = null;

                    if (groupByCode == "D")
                    {
                        myItem = myGroupedData.Where(d => d.DateTimeProperty.Day == date1.Value.Day).FirstOrDefault();
                    }

                    else if (groupByCode == "W")
                    {
                        //DateTime nextWeek = date1.Value.AddDays(6);
                        int day = Convert.ToInt32(date1.Value.DayOfWeek);
                        DateTime startOfWeek = date1.Value.AddDays((-1 * day));
                        DateTime endOfWeek = date1.Value.AddDays((6 - day));
                        myItem = myGroupedData.Where(d => d.DateTimeProperty >= startOfWeek && d.DateTimeProperty <= endOfWeek).FirstOrDefault();
                    }

                    else
                    {
                        DateTime filterDate = new DateTime(date1.Value.Year, date1.Value.Month, 1);
                        myItem = myGroupedData.Where(d => d.DateTimeProperty.Year == filterDate.Year && d.DateTimeProperty.Month == filterDate.Month).FirstOrDefault();
                    }

                    // Add zero point when date filter = today or yesterday
                    if (code == "0" || code == "-1")
                    {
                        myResult.Add(new CRMChartingClass()
                        {
                            Id = i++ + "s",
                            IntegerProperty = 0,
                            IndexOrder = indexOrder,
                        });

                        indexOrder++;
                    }

                    if (myItem == null)
                    {
                        myItem = new CRMChartingClass()
                        {
                            Id = i++ + "s",
                            IntegerProperty = 0,
                            IndexOrder = indexOrder,
                            //LabelProperty = " ",
                        };
                    }

                    else
                    {
                        myItem.IndexOrder = indexOrder;
                    }

                    indexOrder++;
                    myResult.Add(myItem);

                    if (groupByCode == "D")
                    {
                        myItem.LabelProperty = code == "-7" ? string.Format("{0:ddd}", date1) : date1.Value.DayOfWeek.ToString();

                        date1 = date1.Value.AddDays(1);
                        if (date1.Value.Day == date2.Value.Day)
                        {
                            //date1 = date2.Value.AddDays(1);
                        }
                    }

                    else if (groupByCode == "W")
                    {
                        int day = Convert.ToInt32(date1.Value.DayOfWeek);
                        DateTime startOfWeek = date1.Value.AddDays((-1 * day));
                        DateTime endOfWeek = date1.Value.AddDays((6 - day));

                        myItem.StartDate = startOfWeek;
                        myItem.EndDate = endOfWeek;
                        myItem.LabelProperty = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;
                        date1 = date1.Value.AddDays(7);
                    }

                    else
                    {
                        myItem.LabelProperty = string.Format("{0:MMM}", date1) + " " + string.Format("{0:yy}", date1) + "’";
                        date1 = date1.Value.AddMonths(1);

                        if (date1.Value.Month == date2.Value.Month)
                        {
                            date1 = date2;
                        }
                    }
                }
            }

            return myResult;
        }

        private List<CRMChartingClass> GetSLAGroupedData(List<SLAJoinedData> myData, string groupByCode)
        {
            int i = 0;
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();

            if (groupByCode == "D")
            {
                myResult = (from a in myData.ToList()
                            group a by new { a.Date.Year, a.Date.Month, a.Date.Day, a.EscalationFor } into g
                            select new CRMChartingClass()
                            {
                                Id = i++ + "",
                                DataTypeCode = g.Key.EscalationFor,
                                IntegerProperty = g.Count(),
                                DateTimeProperty = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                                GroupByCode = groupByCode,
                            }).ToList();
            }

            else if (groupByCode == "W")
            {
                foreach (SLAJoinedData m in myData)
                {
                    DateTime todayDate2 = new DateTime(m.Date.Year, m.Date.Month, m.Date.Day);

                    int day = Convert.ToInt32(todayDate2.DayOfWeek);
                    DateTime startOfWeek = todayDate2.AddDays((-1 * day));
                    DateTime endOfWeek = todayDate2.AddDays((6 - day));

                    m.StartDate = startOfWeek;
                    m.EndDate = endOfWeek;
                }

                myResult = (from a in myData.ToList()
                            group a by new { a.StartDate, a.EndDate, a.EscalationFor } into g
                            select new CRMChartingClass()
                            {
                                Id = i++ + "",
                                DataTypeCode = g.Key.EscalationFor,
                                IntegerProperty = g.Count(),
                                StartDate = g.Key.StartDate,
                                EndDate = g.Key.EndDate,
                                DateTimeProperty = g.Key.StartDate,
                                GroupByCode = groupByCode,
                            }).ToList();
            }

            else
            {
                myResult = (from a in myData.ToList()
                            group a by new { a.Date.Month, a.Date.Year, a.EscalationFor } into g
                            select new CRMChartingClass()
                            {
                                Id = i++ + "",
                                DataTypeCode = g.Key.EscalationFor,
                                IntegerProperty = g.Count(),
                                DateTimeProperty = new DateTime(g.Key.Year, g.Key.Month, 1),
                                GroupByCode = groupByCode,
                            }).ToList();
            }

            return myResult;
        }

        private List<CRMChartingClass> GetSolvedTicketGroupedData(List<Ticket> myData, string groupByCode)
        {
            int i = 0;
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();

            if (groupByCode == "D")
            {
                myResult = (from a in myData.ToList()
                            group a by new { a.FirstResolveDate.Value.Year, a.FirstResolveDate.Value.Month, a.FirstResolveDate.Value.Day } into g
                            select new CRMChartingClass()
                            {
                                Id = i++ + "s",
                                IntegerProperty = g.Count(),
                                DateTimeProperty = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                                GroupByCode = groupByCode,
                            }).ToList();
            }

            else if (groupByCode == "W")
            {
                List<CRMChartingClass> myList = (from a in myData.ToList()
                                                 group a by new { a.FirstResolveDate } into g
                                                 select new CRMChartingClass()
                                                 {
                                                     DateTimeProperty = g.Key.FirstResolveDate.Value,

                                                 }).ToList();

                foreach (CRMChartingClass m in myList)
                {
                    DateTime todayDate2 = new DateTime(m.DateTimeProperty.Year, m.DateTimeProperty.Month, m.DateTimeProperty.Day);

                    int day = Convert.ToInt32(todayDate2.DayOfWeek);
                    DateTime startOfWeek = todayDate2.AddDays((-1 * day));
                    DateTime endOfWeek = todayDate2.AddDays((6 - day));

                    m.StartDate = startOfWeek;
                    m.EndDate = endOfWeek;
                }

                myResult = (from a in myList
                            group a by new { a.StartDate, a.EndDate } into g
                            select new CRMChartingClass()
                            {
                                Id = i++ + "s",
                                IntegerProperty = g.Count(),
                                StartDate = g.Key.StartDate,
                                EndDate = g.Key.EndDate,
                                GroupByCode = groupByCode,
                                DateTimeProperty = g.Key.StartDate,

                            }).ToList();
            }

            else
            {
                myResult = (from a in myData.ToList()
                            group a by new { a.FirstResolveDate.Value.Month, a.FirstResolveDate.Value.Year } into g
                            select new CRMChartingClass()
                            {
                                Id = i++ + "s",
                                IntegerProperty = g.Count(),
                                DateTimeProperty = new DateTime(g.Key.Year, g.Key.Month, 1),
                                GroupByCode = groupByCode,
                            }).ToList();
            }

            return myResult;
        }

        #endregion

        #region Opened Tickets

        // Opened Ticket By Classifications 
        public List<CRMChartingClass> GetOpenedTicketsGroupByClassification(string code, string ownerId, string employeeGroupId, int tenant)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

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

            TicketStageRepository repository = new TicketStageRepository(tenant);
            TicketStage resolveStage = repository.GetTicketStageByCode("RE", tenant);
            TicketStage openedStage = repository.GetTicketStageByCode("CS", tenant);

            IQueryable<Ticket> dataSource =
                  (from d in context.Tickets.Include("MainClassification")
                   where d.Tenant == tenant
                   && d.MainClassificationId != null
                   && (d.StageId != resolveStage.Id && d.StageId != openedStage.Id)
                   && d.CreateDate != null
                   select d);


            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                dataSource = dataSource.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (days >= 0)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate.Value) == todayDate);
            }

            else if (days == -1)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate.Value) == date1);
            }

            else
            {
                dataSource = dataSource.Where(d => d.CreateDate.Value >= date1 && d.CreateDate.Value <= date2);
            }

            myResult = (from d in dataSource
                        group d by new { d.MainClassificationId, d.MainClassification.Name } into g
                        select new CRMChartingClass()
                        {
                            Id = g.Key.MainClassificationId,
                            StringProperty = g.Key.Name,
                            IntegerProperty = g.Count(),
                            OwnerId = ownerId,
                            ClassificationId = g.Key.MainClassificationId,

                        }).ToList();

            return myResult;
        }

        // Opened Ticket By Severity
        public List<CRMChartingClass> GetOpenedTicketsGroupBySeverity(string code, string ownerId, string employeeGroupId, int tenant)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

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

            TicketStageRepository repository = new TicketStageRepository(tenant);
            TicketStage resolveStage = repository.GetTicketStageByCode("RE", tenant);
            TicketStage openedStage = repository.GetTicketStageByCode("CS", tenant);

            IQueryable<Ticket> dataSource =
                dataSource =
                  (from d in context.Tickets
                   where d.Tenant == tenant
                   && (d.StageId != resolveStage.Id && d.StageId != openedStage.Id)
                   && d.CreateDate != null
                   select d);


            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                dataSource = dataSource.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (days >= 0)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate.Value) == todayDate);
            }

            else if (days == -1)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate.Value) == date1);
            }

            else
            {
                dataSource = dataSource.Where(d => d.CreateDate.Value >= date1 && d.CreateDate.Value <= date2);
            }

            myResult = (from d in dataSource
                        group d by new { d.SeverityId, d.Severity.Name } into g
                        select new CRMChartingClass()
                        {
                            Id = g.Key.SeverityId,
                            StringProperty = g.Key.Name,
                            IntegerProperty = g.Count(),
                            OwnerId = ownerId,
                            SeverityId = g.Key.SeverityId,
                        }).ToList();

            return myResult;
        }

        // Opened Ticket By Owner
        public List<CRMChartingClass> GetOpenedTicketsGroupByOwner(string code, string ownerId, string employeeGroupId, int tenant)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

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

            TicketStageRepository repository = new TicketStageRepository(tenant);
            TicketStage resolveStage = repository.GetTicketStageByCode("RE", tenant);
            TicketStage openedStage = repository.GetTicketStageByCode("CS", tenant);

            IQueryable<Ticket> dataSource =
                dataSource =
                  (from d in context.Tickets
                   where d.Tenant == tenant
                   && (d.StageId != resolveStage.Id && d.StageId != openedStage.Id)
                   && d.OwnerId != null
                   && d.CreateDate != null
                   select d);

            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                dataSource = dataSource.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (days >= 0)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate.Value) == todayDate);
            }

            else if (days == -1)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate.Value) == date1);
            }

            else
            {
                dataSource = dataSource.Where(d => d.CreateDate.Value >= date1 && d.CreateDate.Value <= date2);
            }

            myResult = (from d in dataSource
                        group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                        select new CRMChartingClass()
                        {
                            Id = g.Key.OwnerId,
                            StringProperty = g.Key.EnglishName,
                            OwnerId = g.Key.OwnerId,
                            IntegerProperty = g.Count(),
                        }).ToList();

            return myResult;
        }

        // Opened Ticket By SLA Violation 
        public List<CRMChartingClass> GetOpenedTicketsBySLAViolation(int selectedIndex, string code, string ownerId, string employeeGroupId, int tenant)
        {
            ICRMContext myContext = CRMContext.GetContext(tenant);

            TicketStageRepository repository = new TicketStageRepository(myContext);
            TicketStage stage1 = repository.GetTicketStageByCode("RE", tenant);
            TicketStage stage2 = repository.GetTicketStageByCode("CS", tenant);

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DatesHelper helper = MethodHelper.GetDates(code, tenant);
            DateTime? date1 = helper.Date1 == null ? helper.Date1 : helper.Date1.Value.Date;
            DateTime? date2 = helper.Date2 == null ? helper.Date2 : helper.Date2.Value.Date;

            if (code == "-1")
            {
                date2 = date1;
            }

            if (code == "-7")
            {
                date1 = date1.Value.AddDays(1);
            }

            IQueryable<Ticket> iQueryableTickets = (from d in myContext.Tickets
                                                    where d.Tenant == tenant
                                                    && (d.StageId != stage1.Id && d.StageId != stage2.Id)
                                                    && (d.CreateDate != null && d.CreateDate >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= date2)
                                                    &&
                                                    (
                                                    (d.FirstResponseDue >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.FirstResponseDue) <= date2)
                                                    ||
                                                    (d.ResolveWithinDue >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.ResolveWithinDue) <= date2)
                                                    )
                                                    select d);

            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                iQueryableTickets = iQueryableTickets.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                iQueryableTickets = iQueryableTickets.Where(d => d.OwnerId == ownerId);
            }

            #region myData
            List<Ticket> allTickets = iQueryableTickets.ToList();
            List<SLAJoinedData> allData = new List<SLAJoinedData>();
            foreach (Ticket item in allTickets)
            {
                if ((item.FirstResolveDate != null && ((item.FirstResolveDate > item.ResolveWithinDue)))
                 || (item.FirstResolveDate == null && item.ResolveWithinDue < todayDate))
                {
                    allData.Add(new SLAJoinedData
                    {
                        Id = item.Id,
                        TicketId = item.Id,
                        Date = item.ResolveWithinDue.Value,
                        EscalationFor = "RW",
                        CreateDate = item.CreateDate.Value,
                    });
                }

                else if ((item.FirstResponseTime != null && (item.FirstResponseTime > item.FirstResponseDue))
                       || (item.FirstResponseTime == null && item.FirstResponseDue < todayDate))
                {
                    allData.Add(new SLAJoinedData
                    {
                        Id = item.Id,
                        TicketId = item.Id,
                        Date = item.FirstResponseDue.Value,
                        EscalationFor = "FR",
                        CreateDate = item.CreateDate.Value,
                    });
                }
            }

            List<SLAJoinedData> myData = (from d in allData
                                          group d by new { d.Date, d.EscalationFor, d.CreateDate } into g
                                          select new SLAJoinedData()
                                          {
                                              Date = g.Key.Date,
                                              EscalationFor = g.Key.EscalationFor,

                                          }).ToList();
            #endregion

            string groupByCode = "D";
            if (code == "-30" || code == "-30_1" || code == "0_1")
            {
                groupByCode = "W";
            }
            else if (code == "-90" || code == "-365" || code == "-365_1" || code == "-90_1")
            {
                groupByCode = "M";
            }

            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            if (myData.Count > 0)
            {
                int i = 0;
                List<CRMChartingClass> myGroupedData = this.GetSLAGroupedData(myData, groupByCode);

                int indexOrder = 0;
                i = myGroupedData.Count();
                while (date1 <= date2)
                {
                    CRMChartingClass myItem_FR = null;
                    CRMChartingClass myItem_RW = null;

                    if (groupByCode == "D")
                    {
                        myItem_FR = myGroupedData.Where(d => d.DateTimeProperty.Day == date1.Value.Day && d.DataTypeCode == "FR").FirstOrDefault();
                        myItem_RW = myGroupedData.Where(d => d.DateTimeProperty.Day == date1.Value.Day && d.DataTypeCode == "RW").FirstOrDefault();
                    }

                    else if (groupByCode == "W")
                    {
                        //DateTime nextWeek = date1.Value.AddDays(6);
                        int day = Convert.ToInt32(date1.Value.DayOfWeek);
                        DateTime startOfWeek = date1.Value.AddDays((-1 * day));
                        DateTime endOfWeek = date1.Value.AddDays((6 - day));
                        myItem_FR = myGroupedData.Where(d => d.DateTimeProperty >= startOfWeek && d.DateTimeProperty <= endOfWeek && d.DataTypeCode == "FR").FirstOrDefault();
                        myItem_RW = myGroupedData.Where(d => d.DateTimeProperty >= startOfWeek && d.DateTimeProperty <= endOfWeek && d.DataTypeCode == "RW").FirstOrDefault();
                    }

                    else
                    {
                        DateTime filterDate = new DateTime(date1.Value.Year, date1.Value.Month, 1);
                        myItem_FR = myGroupedData.Where(d => d.DateTimeProperty.Year == filterDate.Year && d.DateTimeProperty.Month == filterDate.Month && d.DataTypeCode == "FR").FirstOrDefault();
                        myItem_RW = myGroupedData.Where(d => d.DateTimeProperty.Year == filterDate.Year && d.DateTimeProperty.Month == filterDate.Month && d.DataTypeCode == "RW").FirstOrDefault();
                    }

                    if (myItem_FR == null)
                    {
                        myItem_FR = new CRMChartingClass()
                        {
                            Id = i++ + "",
                            DataTypeCode = "FR",
                            IntegerProperty = 0,
                            IndexOrder = indexOrder,
                        };
                    }

                    else
                    {
                        myItem_FR.IndexOrder = indexOrder;
                    }

                    indexOrder++;

                    if (myItem_RW == null)
                    {
                        myItem_RW = new CRMChartingClass()
                        {
                            Id = i++ + "",
                            DataTypeCode = "RW",
                            IntegerProperty = 0,
                            IndexOrder = indexOrder,
                        };
                    }

                    else
                    {
                        myItem_RW.IndexOrder = indexOrder;
                    }

                    indexOrder++;

                    myResult.Add(myItem_FR);
                    myResult.Add(myItem_RW);

                    if (groupByCode == "D")
                    {
                        myItem_FR.LabelProperty = code == "-7" ? string.Format("{0:ddd}", date1) : date1.Value.DayOfWeek.ToString();
                        myItem_RW.LabelProperty = code == "-7" ? string.Format("{0:ddd}", date1) : date1.Value.DayOfWeek.ToString();
                        date1 = date1.Value.AddDays(1);
                        if (date1.Value.Day == date2.Value.Day)
                        {
                            //date1 = date2.Value.AddDays(1);
                        }
                    }

                    else if (groupByCode == "W")
                    {
                        int day = Convert.ToInt32(date1.Value.DayOfWeek);
                        DateTime startOfWeek = date1.Value.AddDays((-1 * day));
                        DateTime endOfWeek = date1.Value.AddDays((6 - day));

                        myItem_FR.StartDate = startOfWeek;
                        myItem_FR.EndDate = endOfWeek;
                        myItem_RW.StartDate = startOfWeek;
                        myItem_RW.EndDate = endOfWeek;

                        myItem_FR.LabelProperty = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;
                        myItem_RW.LabelProperty = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;

                        date1 = date1.Value.AddDays(7);
                    }

                    else
                    {
                        myItem_FR.LabelProperty = string.Format("{0:MMM}", date1) + " " + string.Format("{0:yy}", date1) + "’";
                        myItem_RW.LabelProperty = string.Format("{0:MMM}", date1) + " " + string.Format("{0:yy}", date1) + "’";
                        date1 = date1.Value.AddMonths(1);

                        if (date1.Value.Month == date2.Value.Month)
                        {
                            date1 = date2;
                        }
                    }
                }
            }

            return myResult;
        }

        // Opened Ticket By Open Stage 
        public List<CRMChartingClass> GetOpenedTicketsByOpenedStage(int selectedIndex, string code, string ownerId, string employeeGroupId, int tenant)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

            DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

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
            TicketStage resolveStage = repository.GetTicketStageByCode("RE", tenant);
            TicketStage closedstage = repository.GetTicketStageByCode("CS", tenant);

            IQueryable<Ticket> dataSourceQuery =
                  (from d in context.Tickets
                   where d.Tenant == tenant
                   && (d.StageId != resolveStage.Id && d.StageId != closedstage.Id)
                   && d.CreateDate != null
                   && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= date1
                   && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= date2
                   select d);

            if (!string.IsNullOrEmpty(employeeGroupId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.EmployeeGroupId == employeeGroupId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.OwnerId == ownerId);
            }

            List<Ticket> myData = dataSourceQuery.ToList();

            string groupByCode = "D";

            if (code == "-30" || code == "-30_1" || code == "0_1")
            {
                groupByCode = "W";
            }
            else if (code == "-90" || code == "-365" || code == "-365_1" || code == "-90_1")
            {
                groupByCode = "M";
            }

            if (myData.Count > 0)
            {
                int i = 0;
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                List<CRMChartingClass> myGroupedData = this.GetOpenedTicketGroupedData(myData, groupByCode);

                int indexOrder = 0;
                i = myGroupedData.Count();


                while (date1 <= date2)
                {
                    CRMChartingClass myItem = null;

                    if (groupByCode == "D")
                    {
                        myItem = myGroupedData.Where(d => d.DateTimeProperty.Day == date1.Value.Day).FirstOrDefault();
                    }

                    else if (groupByCode == "W")
                    {
                        //DateTime nextWeek = date1.Value.AddDays(6);
                        int day = Convert.ToInt32(date1.Value.DayOfWeek);
                        DateTime startOfWeek = date1.Value.AddDays((-1 * day));
                        DateTime endOfWeek = date1.Value.AddDays((6 - day));
                        myItem = myGroupedData.Where(d => d.DateTimeProperty >= startOfWeek && d.DateTimeProperty <= endOfWeek).FirstOrDefault();
                    }

                    else
                    {
                        DateTime filterDate = new DateTime(date1.Value.Year, date1.Value.Month, 1);
                        myItem = myGroupedData.Where(d => d.DateTimeProperty.Year == filterDate.Year && d.DateTimeProperty.Month == filterDate.Month).FirstOrDefault();
                    }

                    // Add zero point when date filter = today or yesterday
                    if (code == "0" || code == "-1")
                    {
                        myResult.Add(new CRMChartingClass()
                        {
                            Id = i++ + "s",
                            IntegerProperty = 0,
                            IndexOrder = indexOrder,
                        });

                        indexOrder++;
                    }

                    if (myItem == null)
                    {
                        myItem = new CRMChartingClass()
                        {
                            Id = i++ + "s",
                            IntegerProperty = 0,
                            IndexOrder = indexOrder,
                            //LabelProperty = " ",
                        };
                    }

                    else
                    {
                        myItem.IndexOrder = indexOrder;
                    }

                    indexOrder++;
                    myResult.Add(myItem);

                    if (groupByCode == "D")
                    {
                        myItem.LabelProperty = code == "-7" ? string.Format("{0:ddd}", date1) : date1.Value.DayOfWeek.ToString();

                        date1 = date1.Value.AddDays(1);
                        if (date1.Value.Day == date2.Value.Day)
                        {
                            // date1 = date2.Value.AddDays(1);
                        }
                    }

                    else if (groupByCode == "W")
                    {
                        int day = Convert.ToInt32(date1.Value.DayOfWeek);
                        DateTime startOfWeek = date1.Value.AddDays((-1 * day));
                        DateTime endOfWeek = date1.Value.AddDays((6 - day));

                        myItem.StartDate = startOfWeek;
                        myItem.EndDate = endOfWeek;
                        myItem.LabelProperty = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;
                        date1 = date1.Value.AddDays(7);
                    }

                    else
                    {
                        myItem.LabelProperty = string.Format("{0:MMM}", date1) + " " + string.Format("{0:yy}", date1) + "’";
                        date1 = date1.Value.AddMonths(1);

                        if (date1.Value.Month == date2.Value.Month)
                        {
                            date1 = date2;
                        }
                    }
                }
            }

            return myResult;
        }

        private List<CRMChartingClass> GetOpenedTicketGroupedData(List<Ticket> myData, string groupByCode)
        {
            int i = 0;
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();

            if (groupByCode == "D")
            {
                myResult = (from a in myData.ToList()
                            group a by new { a.CreateDate.Value.Year, a.CreateDate.Value.Month, a.CreateDate.Value.Day } into g
                            select new CRMChartingClass()
                            {
                                Id = i++ + "s",
                                IntegerProperty = g.Count(),
                                DateTimeProperty = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                                GroupByCode = groupByCode,
                            }).ToList();
            }

            else if (groupByCode == "W")
            {
                List<CRMChartingClass> myList = (from a in myData.ToList()
                                                 group a by new { a.CreateDate } into g
                                                 select new CRMChartingClass()
                                                 {
                                                     DateTimeProperty = g.Key.CreateDate.Value,

                                                 }).ToList();

                foreach (CRMChartingClass m in myList)
                {
                    DateTime todayDate2 = new DateTime(m.DateTimeProperty.Year, m.DateTimeProperty.Month, m.DateTimeProperty.Day);

                    int day = Convert.ToInt32(todayDate2.DayOfWeek);
                    DateTime startOfWeek = todayDate2.AddDays((-1 * day));
                    DateTime endOfWeek = todayDate2.AddDays((6 - day));

                    m.StartDate = startOfWeek;
                    m.EndDate = endOfWeek;
                }

                myResult = (from a in myList
                            group a by new { a.StartDate, a.EndDate } into g
                            select new CRMChartingClass()
                            {
                                Id = i++ + "s",
                                IntegerProperty = g.Count(),
                                StartDate = g.Key.StartDate,
                                EndDate = g.Key.EndDate,
                                GroupByCode = groupByCode,
                                DateTimeProperty = g.Key.StartDate,

                            }).ToList();
            }

            else
            {
                myResult = (from a in myData.ToList()
                            group a by new { a.CreateDate.Value.Month, a.CreateDate.Value.Year } into g
                            select new CRMChartingClass()
                            {
                                Id = i++ + "s",
                                IntegerProperty = g.Count(),
                                DateTimeProperty = new DateTime(g.Key.Year, g.Key.Month, 1),
                                GroupByCode = groupByCode,
                            }).ToList();
            }

            return myResult;
        }

        #endregion

        #region Overview Performance Tab
        public List<CRMChartingClass> GetTicketOverviewPerformance(string ticketId, int tenant)
        {
            int i = 0;
            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            Ticket dataSourceQuery =
                  (from d in context.Tickets
                   where d.Tenant == tenant && d.Id == ticketId
                   select d).FirstOrDefault();

            SLALineRepository slaRep = new SLALineRepository(tenant);
            BusinessHourRepository businessHourRep = new BusinessHourRepository(tenant);
            SLALine slaLine = slaRep.GetSLALineBySeverityId(tenant, dataSourceQuery.SeverityId , dataSourceQuery.SLAId);
            BusinessHour businessHour = new BusinessHour();
            businessHour = businessHourRep.GetSingleBusinessHours(slaLine.BusinessHoursId, slaLine.Tenant);
            BusinessHourCalcualtions businessCalculation = new BusinessHourCalcualtions(businessHour);

            int firstResponseSLA = slaLine.FirstResponseTimeInMinute.Value;
            int resolveTimeSLA = slaLine.ResolveWithinTimeInMinute.Value;

            #region First Response
            i += 1;
            int count = (firstResponseSLA - firstResponseSLA % 60) / 60;
            int var = Math.Abs(firstResponseSLA - count * 60);
            string temp = count + "." + (var == 10 || var == 20 | var == 30 || var == 40 || var == 50 ? Convert.ToString((firstResponseSLA - count * 60)).PadLeft(2, '0') : Convert.ToString((firstResponseSLA - count * 60)));
            double frSlaCount = Convert.ToDouble(temp);
            TimeSpan time = TimeSpan.FromMinutes(firstResponseSLA);

            myResult.Add(new CRMChartingClass()
            {
                Id = i.ToString(),
                StringProperty = count + " Hours " + (firstResponseSLA - count * 60) + " Min",
                DataTypeCode = "SLA",
                DoubleProperty = time.TotalHours,
                IntegerProperty = firstResponseSLA,
                TimeProperty = time,
                LabelProperty = "First Response",

            });

            int actualFRMinutes = dataSourceQuery.FirstResponseTime != null ? businessCalculation.CalculateBusinessHours(dataSourceQuery.CreateDate, dataSourceQuery.FirstResponseTime.Value, businessHour.Is247) : 0;
            time = TimeSpan.FromMinutes(actualFRMinutes);
            var = Math.Abs(time.Minutes);
            var actualFRTime = time.Hours + "." + (var == 10 || var == 20 | var == 30 || var == 40 || var == 50 ? Convert.ToString(var).PadLeft(2, '0') : Convert.ToString(var));
            double actualFR = Double.Parse(actualFRTime);
            if (dataSourceQuery.FirstResponseTime != null)
            {
                i += 1;
                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    StringProperty = time.Hours + " Hours " + var + " Min",
                    DataTypeCode = "Actual",
                    DoubleProperty = time.TotalHours,
                    IntegerProperty = actualFRMinutes,
                    TimeProperty = time,
                    LabelProperty = "First Response"
                });
            }
            #endregion

            #region First Resolve
            i += 1;

            count = (resolveTimeSLA - resolveTimeSLA % 60) / 60;

            var = resolveTimeSLA - count * 60;
            temp = count + "." + (var == 10 || var == 20 | var == 30 || var == 40 || var == 50 ? Convert.ToString((resolveTimeSLA - count * 60)).PadLeft(2, '0') : Convert.ToString((resolveTimeSLA - count * 60)));
            double reSlaCount = Convert.ToDouble(temp);
            time = TimeSpan.FromMinutes(resolveTimeSLA);
            myResult.Add(new CRMChartingClass()
            {
                Id = i.ToString(),
                StringProperty = count + " Hours " + (resolveTimeSLA - count * 60) + " Min",
                DataTypeCode = "SLA",
                DoubleProperty = time.TotalHours,
                IntegerProperty = resolveTimeSLA,
                TimeProperty = time,
                LabelProperty = "First Resolve"
            });

            int actualREMinutes = dataSourceQuery.FirstResolveDate != null ? businessCalculation.CalculateBusinessHours(dataSourceQuery.CreateDate, dataSourceQuery.FirstResolveDate.Value, businessHour.Is247) : 0;
            time = TimeSpan.FromMinutes(actualREMinutes);
            var = Math.Abs(time.Minutes);
            double actualRE = Convert.ToDouble(time.Hours + "." + (var == 10 || var == 20 | var == 30 || var == 40 || var == 50 ? Convert.ToString(var).PadLeft(2,'0') : Convert.ToString(var)));
            if (dataSourceQuery.FirstResolveDate != null)
            {
                i += 1;
                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    StringProperty = time.Hours + " Hours " + var + " Min",
                    DataTypeCode = "Actual",
                    DoubleProperty = time.TotalHours,
                    IntegerProperty = actualREMinutes,
                    TimeProperty  = time,
                    LabelProperty = "First Resolve"
                });
            }
            #endregion

            return myResult;
        }

        #endregion
    }

    public class SLAJoinedData
    {
        public string Id { get; set; }
        public string TicketId { get; set; }
        public string EscelationId { get; set; }
        public DateTime Date { get; set; }
        public string EscalationFor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}



