using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.Repsitories;
using System.Linq;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.DataContracts;
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.BusinessUnitFilters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools;
using Logitude.CRM.BL.EntityDws;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class ActivityQueryService
    {
        public List<ActivityPM> GetNotSyncActivitis(string ownerId, int tenant)
        {
            ICRMContext context = MainContext as ICRMContext;
            ActivityOwnerHistoryRepository activityOwnerHistoryRepository = new ActivityOwnerHistoryRepository(context);

            List<ActivityPM> result = new List<ActivityPM>();

            List<Activity> activitis = repository.GetAll(tenant).Where(a => a.OwnerId == ownerId && a.Tenant == tenant && a.NeedSynchronization == true && a.ActivityTypeCode != "CL" && a.ActivityTypeCode != "EI" && a.ActivityTypeCode != "EO").Take(10).ToList();

            List<ActivityOwnerHistory> activityOwnerHistoris = activityOwnerHistoryRepository.GetAll(tenant).Where(s => s.OwnerId == ownerId && s.NeedSynchronization && s.Tenant == tenant).ToList();

            foreach (Activity poco in activitis)
            {
                EntityPM = new ActivityPM();
                mapping.CustomPOCOToPM(EntityPM, poco);
                mapping.POCOToPM(EntityPM, poco);
                this.GetComposition(new ActivityKeys() { Id = EntityPM.Id }, EntityPM);
                result.Add(EntityPM);
            }
            
            foreach (ActivityOwnerHistory history in activityOwnerHistoris)
            {
                if (!result.Where(a => a.Id == history.ActivityId).Any())
                {
                    Activity poco = repository.GetSingle(history.ActivityId, tenant);

                    EntityPM = new ActivityPM();
                    mapping.CustomPOCOToPM(EntityPM, poco);
                    mapping.POCOToPM(EntityPM, poco);
                    EntityPM.IsOpen = false;
                    result.Add(EntityPM);
                }
            }


          

            return result;

        }

        public ActivityPM GetSingleByOutlookId(string outlookId,int tenant)
        {
            
            Activity entity = repository.GetSingleByOutlookId(outlookId, tenant);
            if (entity != null)
            {
                EntityPM = new ActivityPM();
                mapping.CustomPOCOToPM(EntityPM, entity);
                mapping.POCOToPM(EntityPM, entity);
            }

            return EntityPM;
        }

        public ActivityPM GetSingleByOwnerId(string id,string ownerId, int tenant)
        {

            Activity entity = repository.GetSingleByOwnerId(id,ownerId, tenant);
            if (entity != null)
            {
                EntityPM = new ActivityPM();
                mapping.CustomPOCOToPM(EntityPM, entity);
                mapping.POCOToPM(EntityPM, entity);
            }

            return EntityPM;
        }

        public List<CRMChartingClass> GetActivitiesDashBoard(string ownerId, string businessUnitId, int tenant, string activityTypeCode, string RecordsTypeCode)
        {
            int i = 0;

            List<CRMChartingClass> myResult = new List<CRMChartingClass>();

            ICRMContext context = MainContext as ICRMContext;

            IQueryable<Activity> dataSourceQuery =
                (from d in context.Activities
                 where d.Tenant == tenant
                 && d.IsOpen
                 && d.ActivityStatusCode != "X"
                 select d);

            ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
            dataSourceQuery = filter.RunFilter(dataSourceQuery);

            if (!string.IsNullOrEmpty(activityTypeCode))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.ActivityTypeCode == activityTypeCode);
            }

            if (RecordsTypeCode == "C")
            {
                if (!string.IsNullOrEmpty(ownerId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.CreatedByUserId == ownerId);
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(ownerId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.OwnerId == ownerId);
                }

                if (!string.IsNullOrEmpty(businessUnitId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.BusinessUnitId == businessUnitId);
                }
            }

            DateTime? date1 = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? date2 = date1.Value.AddDays(6).Date;

            IQueryable<Activity> oldEntities = from a in dataSourceQuery
                                               where
                                               (a.StartDateTime != null && System.Data.Entity.DbFunctions.TruncateTime(a.StartDateTime.Value) < date1)
                                               ||
                                               (a.StartDateTime == null && a.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(a.DueDate.Value) < date1)
                                               select a;
            
            IQueryable<Activity> nowEntities = from a in dataSourceQuery
                                               where
                                               (a.StartDateTime != null && System.Data.Entity.DbFunctions.TruncateTime(a.StartDateTime) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(a.StartDateTime) <= date2)
                                               ||
                                               (a.StartDateTime == null && a.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(a.DueDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(a.DueDate) <= date2)
                                               select a;
            
            IQueryable<Activity> noDateEntities = from a in dataSourceQuery 
                                                  where 
                                                  a.StartDateTime == null && a.DueDate == null
                                                  select a;

            var myGroup = nowEntities.GroupBy(g => new
            {
                date = g.StartDateTime != null ? System.Data.Entity.DbFunctions.TruncateTime(g.StartDateTime) : System.Data.Entity.DbFunctions.TruncateTime(g.DueDate),
                typeCode = g.ActivityTypeCode
            });


            #region No Date Entities
            i += 1;
            myResult.Add(new CRMChartingClass()
            {
                Id = i.ToString(),
                DateTimeProperty = date1.Value.AddMonths(-2).Date,
                StringProperty = "TS",
                IntegerProperty = noDateEntities.Where(d => d.ActivityTypeCode == "TS").Count(),
                LabelProperty = "No Date"
            });

            i += 1;
            myResult.Add(new CRMChartingClass()
            {
                Id = i.ToString(),
                DateTimeProperty = date1.Value.AddMonths(-2).Date,
                StringProperty = "CL",
                IntegerProperty = noDateEntities.Where(d => d.ActivityTypeCode == "CL").Count(),
                LabelProperty = "No Date"
            });

            i += 1;
            myResult.Add(new CRMChartingClass()
            {
                Id = i.ToString(),
                DateTimeProperty = date1.Value.AddMonths(-2).Date,
                StringProperty = "AP",
                IntegerProperty = noDateEntities.Where(d => d.ActivityTypeCode == "AP").Count(),
                LabelProperty = "No Date"
            });

            i += 1;
            myResult.Add(new CRMChartingClass()
            {
                Id = i.ToString(),
                DateTimeProperty = date1.Value.AddMonths(-2).Date,
                StringProperty = "EO",
                IntegerProperty = noDateEntities.Where(d => d.ActivityTypeCode == "EO").Count(),
                LabelProperty = "No Date"
            });

            i += 1;
            myResult.Add(new CRMChartingClass()
            {
                Id = i.ToString(),
                DateTimeProperty = date1.Value.AddMonths(-2).Date,
                StringProperty = "EI",
                IntegerProperty = noDateEntities.Where(d => d.ActivityTypeCode == "EI").Count(),
                LabelProperty = "No Date"
            });
            #endregion

            #region Old Entities
            i += 1;
            DateTime oldDate = date1.Value.AddMonths(-1).Date;

            myResult.Add(new CRMChartingClass()
            {
                Id = i.ToString(),
                DateTimeProperty = oldDate,
                StringProperty = "TS",
                IntegerProperty = oldEntities.Where(d => d.ActivityTypeCode == "TS").Count(),
                LabelProperty = "Old"
            });

            i += 1;
            myResult.Add(new CRMChartingClass()
            {
                Id = i.ToString(),
                DateTimeProperty = oldDate,
                StringProperty = "CL",
                IntegerProperty = oldEntities.Where(d => d.ActivityTypeCode == "CL").Count(),
                LabelProperty = "Old"
            });

            i += 1;
            myResult.Add(new CRMChartingClass()
            {
                Id = i.ToString(),
                DateTimeProperty = oldDate,
                StringProperty = "AP",
                IntegerProperty = oldEntities.Where(d => d.ActivityTypeCode == "AP").Count(),
                LabelProperty = "Old"
            });

            i += 1;
            myResult.Add(new CRMChartingClass()
            {
                Id = i.ToString(),
                DateTimeProperty = oldDate,
                StringProperty = "EO",
                IntegerProperty = oldEntities.Where(d => d.ActivityTypeCode == "EO").Count(),
                LabelProperty = "Old"
            });

            i += 1;
            myResult.Add(new CRMChartingClass()
            {
                Id = i.ToString(),
                DateTimeProperty = oldDate,
                StringProperty = "EI",
                IntegerProperty = oldEntities.Where(d => d.ActivityTypeCode == "EI").Count(),
                LabelProperty = "Old"
            });
            #endregion

            #region Due Date Entities

            foreach (var item in myGroup)
            {
                i += 1;

                myResult.Add(new CRMChartingClass()
                {
                    Id = i.ToString(),
                    DateTimeProperty = item.Key.date.Value,
                    StringProperty = item.Key.typeCode,
                    IntegerProperty = item.Count(),
                    LabelProperty = String.Format("{0:ddd}", item.Key.date.Value)
                });
            }

            while (date1 <= date2)
            {
                var iQuery = myGroup.Where(d => d.Key.date == date1);

                var item1 = (from a in iQuery where a.Key.typeCode == "TS" select a).FirstOrDefault();
                var item2 = (from a in iQuery where a.Key.typeCode == "CL" select a).FirstOrDefault();
                var item3 = (from a in iQuery where a.Key.typeCode == "AP" select a).FirstOrDefault();
                var item4 = (from a in iQuery where a.Key.typeCode == "EO" select a).FirstOrDefault();
                var item5 = (from a in iQuery where a.Key.typeCode == "EI" select a).FirstOrDefault();

                if (item1 == null)
                {
                    i += 1;
                    myResult.Add(new CRMChartingClass()
                    {
                        Id = i.ToString(),
                        DateTimeProperty = date1.Value.Date,
                        StringProperty = "TS",
                        IntegerProperty = 0,
                        LabelProperty = String.Format("{0:ddd}", date1.Value.Date)
                    });
                }

                if (item2 == null)
                {
                    i += 1;
                    myResult.Add(new CRMChartingClass()
                    {
                        Id = i.ToString(),
                        DateTimeProperty = date1.Value.Date,
                        StringProperty = "CL",
                        IntegerProperty = 0,
                        LabelProperty = String.Format("{0:ddd}", date1.Value.Date)
                    });
                }

                if (item3 == null)
                {
                    i += 1;
                    myResult.Add(new CRMChartingClass()
                    {
                        Id = i.ToString(),
                        DateTimeProperty = date1.Value.Date,
                        StringProperty = "AP",
                        IntegerProperty = 0,
                        LabelProperty = String.Format("{0:ddd}", date1.Value.Date)
                    });
                }

                if (item4 == null)
                {
                    i += 1;
                    myResult.Add(new CRMChartingClass()
                    {
                        Id = i.ToString(),
                        DateTimeProperty = date1.Value.Date,
                        StringProperty = "EO",
                        IntegerProperty = 0,
                        LabelProperty = String.Format("{0:ddd}", date1.Value.Date)
                    });
                }

                if (item5 == null)
                {
                    i += 1;
                    myResult.Add(new CRMChartingClass()
                    {
                        Id = i.ToString(),
                        DateTimeProperty = date1.Value.Date,
                        StringProperty = "EI",
                        IntegerProperty = 0,
                        LabelProperty = String.Format("{0:ddd}", date1.Value.Date)
                    });
                }

                date1 = date1.Value.AddDays(1);
            }
            #endregion

            return myResult;
        }
        public List<CRMChartingClass> GetActivitiesChartDataCustom(DateTime?FromDate,DateTime?ToDate, string ownerId, string businessUnitId, string chartCode, int tenant)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;          

            IQueryable<Activity> dataSource =
                (from d in context.Activities.Include("Owner")
                 where d.Tenant == tenant
                 && d.IsOpen == false
                 && d.CompleteDate != null
                 && d.ActivityStatusCode == "C"
                 && (d.ActivityTypeCode == "CL" || d.ActivityTypeCode == "AP" || d.ActivityTypeCode == "TS")
                 select d);

            ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
            dataSource = filter.RunFilter(dataSource);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSource = dataSource.Where(d => d.BusinessUnitId == businessUnitId);
            }

          
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CompleteDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.CompleteDate) <= ToDate);
            

            myResult = (from d in dataSource
                        where d.ActivityTypeCode != null
                        group d by new { d.ActivityType } into g
                        select new CRMChartingClass()
                        {
                            Id = g.Key.ActivityType.Code,
                            DataTypeCode = g.Key.ActivityType.Code,
                            StringProperty = g.Key.ActivityType.Name,
                            IntegerProperty = g.Count(),
                            TypeIndex = g.Key.ActivityType.Code == "TS" ? 0 : (g.Key.ActivityType.Code == "CL" ? 1 : 2)
                        }).ToList();


            return myResult;
        }

        public List<CRMChartingClass> GetActivitiesChartData(string code, string ownerId, string businessUnitId, string chartCode, int tenant)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? date1 = helper.Date1;
            DateTime? date2 = helper.Date2;
            int days = helper.Days;

            IQueryable<Activity> dataSource =
                (from d in context.Activities.Include("Owner")
                 where d.Tenant == tenant
                 && d.IsOpen == false
                 && d.CompleteDate != null
                 && d.ActivityStatusCode == "C"
                 && (d.ActivityTypeCode == "CL" || d.ActivityTypeCode == "AP" || d.ActivityTypeCode == "TS")
                 select d);

            ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
            dataSource = filter.RunFilter(dataSource);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSource = dataSource.Where(d => d.BusinessUnitId == businessUnitId);
            }

            if (days >= 0)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CompleteDate) == todayDate);
            }

            else if (days == -1)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CompleteDate) == date1);
            }

            else
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CompleteDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.CompleteDate) <= date2);
            }

            myResult = (from d in dataSource
                        where d.ActivityTypeCode != null
                        group d by new { d.ActivityType } into g
                        select new CRMChartingClass()
                        {
                            Id = g.Key.ActivityType.Code,
                            DataTypeCode = g.Key.ActivityType.Code,
                            StringProperty = g.Key.ActivityType.Name,
                            IntegerProperty = g.Count(),
                            TypeIndex = g.Key.ActivityType.Code == "TS" ? 0 : (g.Key.ActivityType.Code == "CL" ? 1 : 2)
                        }).ToList();


            return myResult;
        }

        public List<CRMChartingClass> GetActivitiesGroupBySalesman(string code, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? date1 = helper.Date1;
            DateTime? date2 = helper.Date2;
            int days = helper.Days;

            IQueryable<Activity> dataSourceQuery =
                (from d in context.Activities.Include("Owner")
                 where d.Tenant == tenant
                 && d.OwnerId != null
                 && d.ActivityStatusCode != "X"
                 select d);

            ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
            dataSourceQuery = filter.RunFilter(dataSourceQuery);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.BusinessUnitId == businessUnitId);
            }

            if (fieldCode == "C")
            {
                if (days >= 0)
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) == todayDate);
                }

                else if (days == -1)
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) == date1);
                }

                else
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= date2);
                }
            }

            else if (fieldCode == "P")
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.IsOpen);
            }

            else if (fieldCode == "S")
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.IsOpen == false && d.CompleteDate != null && d.ActivityStatusCode == "C" && (d.ActivityTypeCode == "CL" || d.ActivityTypeCode == "AP" || d.ActivityTypeCode == "TS"));
                if (days >= 0)
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CompleteDate) == todayDate);
                }

                else if (days == -1)
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CompleteDate) == date1);
                }

                else
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CompleteDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.CompleteDate) <= date2);
                }
            }

            if (dataSourceQuery != null)
            {
                IQueryable<Activity> data_TS = dataSourceQuery.Where(d => d.ActivityTypeCode == "TS");
                IQueryable<Activity> data_CL = dataSourceQuery.Where(d => d.ActivityTypeCode == "CL");
                IQueryable<Activity> data_AP = dataSourceQuery.Where(d => d.ActivityTypeCode == "AP");
                IQueryable<Activity> data_EO = dataSourceQuery.Where(d => d.ActivityTypeCode == "EO");
                IQueryable<Activity> data_EI = dataSourceQuery.Where(d => d.ActivityTypeCode == "EI");

                List<CRMChartingClass> myData = new List<CRMChartingClass>();

                if (isTopTen)
                {
                    myData =
                        (from d in dataSourceQuery
                         group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                         select new CRMChartingClass()
                         {
                             Id = g.Key.OwnerId,
                             StringProperty = g.Key.EnglishName,
                             OwnerId = g.Key.OwnerId,
                             IntegerProperty = g.Count()
                         })
                         .OrderByDescending(o => o.IntegerProperty)
                         .Take(10)
                         .ToList();
                }

                else
                {
                    myData =
                        (from d in dataSourceQuery
                         group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                         select new CRMChartingClass()
                         {
                             Id = g.Key.OwnerId,
                             StringProperty = g.Key.EnglishName,
                             OwnerId = g.Key.OwnerId,
                             IntegerProperty = g.Count()
                         })
                         .ToList();
                }

                foreach (CRMChartingClass item in myData)
                {
                    myResult.Add(new CRMChartingClass()
                    {
                        Id = item.Id + ":TS",
                        DataTypeCode = "TS",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_TS.Where(d => d.OwnerId == item.Id).Count(),
                        OwnerId = item.Id,
                    });

                    myResult.Add(new CRMChartingClass()
                    {
                        Id = item.Id + ":CL",
                        DataTypeCode = "CL",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_CL.Where(d => d.OwnerId == item.Id).Count(),
                        OwnerId = item.Id,
                    });

                    myResult.Add(new CRMChartingClass()
                    {
                        Id = item.Id + ":AP",
                        DataTypeCode = "AP",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_AP.Where(d => d.OwnerId == item.Id).Count(),
                        OwnerId = item.Id,
                    });

                    myResult.Add(new CRMChartingClass()
                    {
                        Id = item.Id + ":EO",
                        DataTypeCode = "EO",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_EO.Where(d => d.OwnerId == item.Id).Count(),
                        OwnerId = item.Id,
                    });

                    myResult.Add(new CRMChartingClass()
                    {
                        Id = item.Id + ":EI",
                        DataTypeCode = "EI",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_EI.Where(d => d.OwnerId == item.Id).Count(),
                        OwnerId = item.Id,
                    });
                }
            }

            return myResult;
        }
        public List<CRMChartingClass> GetActivitiesGroupBySalesmanCustom(DateTime? FromDate,DateTime? ToDate, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;
            
            IQueryable<Activity> dataSourceQuery =
                (from d in context.Activities.Include("Owner")
                 where d.Tenant == tenant
                 && d.OwnerId != null
                 && d.ActivityStatusCode != "X"
                 select d);

            ActivityBusinessUnitFilter filter = new ActivityBusinessUnitFilter(tenant);
            dataSourceQuery = filter.RunFilter(dataSourceQuery);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.BusinessUnitId == businessUnitId);
            }

            if (fieldCode == "C")
            {
              
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= ToDate);
                
            }

            else if (fieldCode == "P")
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.IsOpen);
            }

            else if (fieldCode == "S")
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.IsOpen == false && d.CompleteDate != null && d.ActivityStatusCode == "C" && (d.ActivityTypeCode == "CL" || d.ActivityTypeCode == "AP" || d.ActivityTypeCode == "TS"));
               
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CompleteDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.CompleteDate) <= ToDate);
                
            }

            if (dataSourceQuery != null)
            {
                IQueryable<Activity> data_TS = dataSourceQuery.Where(d => d.ActivityTypeCode == "TS");
                IQueryable<Activity> data_CL = dataSourceQuery.Where(d => d.ActivityTypeCode == "CL");
                IQueryable<Activity> data_AP = dataSourceQuery.Where(d => d.ActivityTypeCode == "AP");
                IQueryable<Activity> data_EO = dataSourceQuery.Where(d => d.ActivityTypeCode == "EO");
                IQueryable<Activity> data_EI = dataSourceQuery.Where(d => d.ActivityTypeCode == "EI");

                List<CRMChartingClass> myData = new List<CRMChartingClass>();

                if (isTopTen)
                {
                    myData =
                        (from d in dataSourceQuery
                         group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                         select new CRMChartingClass()
                         {
                             Id = g.Key.OwnerId,
                             StringProperty = g.Key.EnglishName,
                             OwnerId = g.Key.OwnerId,
                             IntegerProperty = g.Count()
                         })
                         .OrderByDescending(o => o.IntegerProperty)
                         .Take(10)
                         .ToList();
                }

                else
                {
                    myData =
                        (from d in dataSourceQuery
                         group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                         select new CRMChartingClass()
                         {
                             Id = g.Key.OwnerId,
                             StringProperty = g.Key.EnglishName,
                             OwnerId = g.Key.OwnerId,
                             IntegerProperty = g.Count()
                         })
                         .ToList();
                }

                foreach (CRMChartingClass item in myData)
                {
                    myResult.Add(new CRMChartingClass()
                    {
                        Id = item.Id + ":TS",
                        DataTypeCode = "TS",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_TS.Where(d => d.OwnerId == item.Id).Count(),
                        OwnerId = item.Id,
                    });

                    myResult.Add(new CRMChartingClass()
                    {
                        Id = item.Id + ":CL",
                        DataTypeCode = "CL",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_CL.Where(d => d.OwnerId == item.Id).Count(),
                        OwnerId = item.Id,
                    });

                    myResult.Add(new CRMChartingClass()
                    {
                        Id = item.Id + ":AP",
                        DataTypeCode = "AP",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_AP.Where(d => d.OwnerId == item.Id).Count(),
                        OwnerId = item.Id,
                    });

                    myResult.Add(new CRMChartingClass()
                    {
                        Id = item.Id + ":EO",
                        DataTypeCode = "EO",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_EO.Where(d => d.OwnerId == item.Id).Count(),
                        OwnerId = item.Id,
                    });

                    myResult.Add(new CRMChartingClass()
                    {
                        Id = item.Id + ":EI",
                        DataTypeCode = "EI",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_EI.Where(d => d.OwnerId == item.Id).Count(),
                        OwnerId = item.Id,
                    });
                }
            }

            return myResult;
        }

        public override void GetComposition(EntityKeyFields entityKeys, ActivityPM entityPM)
        {
            ICRMContext context = MainContext as ICRMContext;
            ActivityKeys activityKeys = entityKeys as ActivityKeys;

            ActivityInviteeQueryService queryService = new ActivityInviteeQueryService(context);
            entityPM.ActivityInvitees = queryService.GetMulti(activityKeys, true);

            ActivityEmailRecipientQueryService reciepientService = new ActivityEmailRecipientQueryService(context);
            entityPM.ActivityEmailRecipients = reciepientService.GetMulti(activityKeys, true);

            ActivityNoteQueryService notesService = new ActivityNoteQueryService(context);
            entityPM.ActivityNotes = notesService.GetMulti(activityKeys, true);
        }

        //public string GetXmlActivitiyDWByTenantAndFromDateAndToDate(int tenant, DateTime fromdate, DateTime todate)
        //{
        //    return repository.GetXmlActivitiesByTenantandFromDateAndToDate(tenant, fromdate, todate);
        //}

        public List<ActivitiyDW> GetctivitiesDWListsByDates(int tenant, DateTime fromdate, DateTime todate, int skip, int take)
        {

            //ICRMContext context = MainContext as ICRMContext;
            //List<ActivitiyDW> activitiyDWList = (from a in context.Activities.Include("Customer").Include("Contact").Include("Customer.SalesmanUser.Contact").Include("ActivityType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("ActivityStatus")
            //                                                                 where a.Tenant == tenant && a.CreateDate >= fromdate && a.CreateDate <= todate
            //                                                                 select new ActivitiyDW()
            //                                                                 {
            //                                                                     ActivityId = a.Id,
            //                                                                     ActivityTypeCode = a.ActivityTypeCode,
            //                                                                     ActivityTypeName = a.ActivityType != null ? a.ActivityType.Name : "",
            //                                                                     OpportunityId = a.OpportunityId,
            //                                                                     CustomerId = a.CustomerId,
            //                                                                     CustomerName = a.Customer != null ? a.Customer.EnglishName : "",
            //                                                                     CustomerCode = a.Customer != null ? a.Customer.Code : "",
            //                                                                     OwnerId = a.OwnerId,
            //                                                                     OwnerName = a.Owner != null ? a.Owner.Contact != null ? a.Owner.Contact.EnglishName : "" : "",
            //                                                                     CreatedbyUserId = a.CreatedByUserId,
            //                                                                     CreatedbyUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact != null ? a.CreatedByUser.Contact.EnglishName : "" : "",
            //                                                                     Subject = a.Subject,
            //                                                                     Description = a.Description,
            //                                                                     ActivityStatusCode = a.ActivityStatusCode,
            //                                                                     ActivityStatusName = a.ActivityStatus != null ? a.ActivityStatus.Name : "",
            //                                                                     MeetingSummary = a.MeetingSummary,
            //                                                                     IsOpen = a.IsOpen,
            //                                                                     QuoteId = a.QuoteId,
            //                                                                     Notes = a.Notes,
            //                                                                     CreateDate = a.CreateDate,
            //                                                                     UpdateDate = a.UpdateDate,
            //                                                                     DueDate = a.DueDate,
            //                                                                     SalesmanId = a.Customer != null ? a.Customer.SalesmanUserId : "",
            //                                                                     SalesmanName = a.Customer != null ? a.Customer.SalesmanUser != null ? a.Customer.SalesmanUser.Contact != null ? a.Customer.SalesmanUser.Contact.EnglishName : "" : "" : "",
                                                                                
            //                                                                 }).OrderBy(d => d.CreateDate).Skip(skip).Take(take).ToList();

            return new List<ActivitiyDW> ();


        }

        public int GetctivitiesDWListsCountByDates(int tenant, DateTime fromDate, DateTime toDate)
        {
            return 0;//context.Activities.Where(a => a.Tenant == tenant && a.CreateDate >= fromDate && a.CreateDate <= toDate).Count();
        }

        public List<ActivitiyDW> GetActivitiesDWBListsByUpdateDate(int tenant, DateTime updateDate, int skip, int take)
        {

            //ICRMContext context = MainContext as ICRMContext;
            //List<ActivitiyDW> activitiyDWList = (from a in context.Activities.Include("Customer").Include("Contact").Include("Customer.SalesmanUser.Contact").Include("ActivityType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("ActivityStatus")
            //                                     where a.Tenant == tenant && a.UpdateDate > updateDate
            //                                     select new ActivitiyDW()
            //                                     {
            //                                         ActivityId = a.Id,
            //                                         ActivityTypeCode = a.ActivityTypeCode,
            //                                         ActivityTypeName = a.ActivityType != null ? a.ActivityType.Name : "",
            //                                         OpportunityId = a.OpportunityId,
            //                                         CustomerId = a.CustomerId,
            //                                         CustomerName = a.Customer != null ? a.Customer.EnglishName : "",
            //                                         CustomerCode = a.Customer != null ? a.Customer.Code : "",
            //                                         OwnerId = a.OwnerId,
            //                                         OwnerName = a.Owner != null ? a.Owner.Contact != null ? a.Owner.Contact.EnglishName : "" : "",
            //                                         CreatedbyUserId = a.CreatedByUserId,
            //                                         CreatedbyUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact != null ? a.CreatedByUser.Contact.EnglishName : "" : "",
            //                                         Subject = a.Subject,
            //                                         Description = a.Description,
            //                                         ActivityStatusCode = a.ActivityStatusCode,
            //                                         ActivityStatusName = a.ActivityStatus != null ? a.ActivityStatus.Name : "",
            //                                         MeetingSummary = a.MeetingSummary,
            //                                         IsOpen = a.IsOpen,
            //                                         QuoteId = a.QuoteId,
            //                                         CreateDate = a.CreateDate,
            //                                         UpdateDate = a.UpdateDate,
            //                                         Notes = a.Notes,
            //                                         DueDate = a.DueDate,
            //                                         SalesmanId =   a.Customer != null ?  a.Customer.SalesmanUserId:"",
            //                                         SalesmanName = a.Customer != null ? a.Customer.SalesmanUser != null ? a.Customer.SalesmanUser.Contact !=null ? a.Customer.SalesmanUser.Contact.EnglishName: "": "" : "",
            //                                      }).OrderBy(d => d.UpdateDate).Skip(skip).Take(take).ToList();



            return new List<ActivitiyDW>();// activitiyDWList;


        }

        public int GetctivitiesDWCountByUpdateDate(int tenant, DateTime updateDate)
        {
            return 0;//context.Activities.Where(a => a.Tenant == tenant && a.UpdateDate > updateDate).Count();
        }
    }
}
