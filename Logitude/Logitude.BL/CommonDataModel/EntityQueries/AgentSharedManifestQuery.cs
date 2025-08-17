using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AgentSharedManifestQuery
    {
        AgentSharedManifestRepository repository;

 

        public AgentSharedManifestQuery(int tenant)
        {
            repository = new AgentSharedManifestRepository(tenant);
        }

        public AgentSharedManifestQuery(AgentSharedManifestRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<AgentSharedManifestList> GetIQueryableEntityList(IQueryable<AgentSharedManifest> iQueryable)
        {
            IQueryable<AgentSharedManifestList> result = from a in iQueryable
                                                         select new AgentSharedManifestList()
                                                         {

                                                             Tenant = a.Tenant,
                                                             Id = a.Id,
                                                             Master = a.Master,
                                                             AgentReference = a.AgentReference,
                                                             CreateDate = a.CreateDate,
                                                             UpdateDate = a.UpdateDate,
                                                             UpdatedByUserId = a.UpdatedByUserId,
                                                             SearchFields = a.SearchFields,
                                                             TransportModeId = a.TransportModeId,
                                                             GrossWeight = a.GrossWeight,
                                                             ChargeableWeight = a.ChargeableWeight,
                                                             TEU = a.TEU,
                                                             PackagesQuantity = a.PackagesQuantity,
                                                             AgentId = a.AgentId,
                                                             DirectionId = a.DirectionId,
                                                             FromPortId = a.FromPortId,
                                                             ToPortId = a.ToPortId,
                                                             StatusCode = a.StatusCode,
                                                             TransportModeName = a.TransportModeId == "A" ? "Air" : a.TransportModeId == "I" ? "Inland" : a.TransportModeId == "I" ? "Ocean" : "",
                                                             StatusName = a.SharedManifestsStatus != null ? a.SharedManifestsStatus.StatusName : "",
                                                             Routing = a.FromPort.Code + " > " + a.ToPort.Code,
                                                             ShipmentLevelCode = a.ShipmentLevelCode,
                                                             ShipmentLevelName =(a.ShipmentType != null ? a.ShipmentType.Name : "" )+ " " + (a.ShipmentLevel != null ? a.ShipmentLevel.Name : ""),
                                                             AgentName = a.Agent != null ? a.Agent.Card!=null ? a.Agent.Card .EnglishName : "" : "",
                                                             CancelledBySenderAgent = a.CancelledBySenderAgent,
                                                         };
       

            return result;
        }

        private string BuildRouting(Port fromPort, Port toPort)
        {
            string routing = "";
            if (fromPort != null)
            {
                routing += fromPort.Code;
            }
            if (toPort != null)
            {
                routing += " > ";
                routing += toPort.Code;
            }

            return routing;
        }

        //        a.FromPort!=null? a.FromPort.Code + " > " : "" + a.ToPort != null ?C:"",
        public AgentSharedManifestPM GetSinglePM(string id, int tenant)
        {
            AgentSharedManifestPM entity = (from a in repository.context.AgentSharedManifests
                                            where a.Tenant == tenant
                                            && a.Id == id
                                            select new AgentSharedManifestPM()
                                            {
                                                Tenant = a.Tenant,
                                                Id = a.Id,
                                                Master = a.Master,
                                                AgentReference = a.AgentReference,
                                                CreateDate = a.CreateDate,
                                                UpdateDate  = a.UpdateDate,
                                                UpdatedByUserId = a.UpdatedByUserId,
                                                ManifestXML = a.ManifestXML,
                                                SearchFields = a.SearchFields,
                                                TransportModeId = a.TransportModeId,
                                                GrossWeight = a.GrossWeight,
                                                ChargeableWeight = a.ChargeableWeight,
                                                TEU = a.TEU,
                                                PackagesQuantity = a.PackagesQuantity,
                                                AgentId = a.AgentId,
                                                DirectionId = a.DirectionId,
                                                FromPortId = a.FromPortId,
                                                ToPortId = a.ToPortId,
                                                StatusCode = a.StatusCode,
                                                TransportModeName = a.TransportModeId == "A" ? "Air" : a.TransportModeId == "I" ? "Inland" : a.TransportModeId == "I" ? "Ocean" : "",
                                                ShipmentLevelCode = a.ShipmentLevelCode,
                                                CancelledBySenderAgent = a.CancelledBySenderAgent,
                                            }).FirstOrDefault();
            return entity;
        }

        public IQueryable<AgentSharedManifestPM> GetAgentSharedManifestPMsByTenant(int tenant)
        {
            IQueryable<AgentSharedManifestPM> agentSharedManifestPMs = from a in repository.context.AgentSharedManifests
                                                                       where a.Tenant == tenant
                                                                       select new AgentSharedManifestPM()
                                                                       {
                                                                           Tenant = a.Tenant,
                                                                           Id = a.Id,
                                                                           Master = a.Master,
                                                                           AgentReference = a.AgentReference,
                                                                           CreateDate = a.CreateDate,
                                                                           UpdateDate = a.UpdateDate,
                                                                           UpdatedByUserId = a.UpdatedByUserId,
                                                                           ManifestXML = a.ManifestXML,
                                                                           SearchFields = a.SearchFields,

                                                                           TransportModeId = a.TransportModeId,
                                                                           GrossWeight = a.GrossWeight,
                                                                           ChargeableWeight = a.ChargeableWeight,
                                                                           TEU = a.TEU,
                                                                           PackagesQuantity = a.PackagesQuantity,
                                                                           AgentId = a.AgentId,
                                                                           DirectionId = a.DirectionId,
                                                                           FromPortId = a.FromPortId,
                                                                           ToPortId = a.ToPortId,
                                                                           StatusCode = a.StatusCode,
                                                                           TransportModeName = a.TransportModeId == "A" ? "Air" : a.TransportModeId == "I" ? "Inland" : a.TransportModeId == "I" ? "Ocean" : "",
                                                                           ShipmentLevelCode = a.ShipmentLevelCode,
                                                                           ShipmentLevelName =(a.ShipmentType != null ? a.ShipmentType.Name : "") + " " + (a.ShipmentLevel != null ? a.ShipmentLevel.Name : ""),
                                                                           CancelledBySenderAgent = a.CancelledBySenderAgent,
                                                                       };
            return agentSharedManifestPMs;
        }

        public IQueryable<AgentSharedManifestList> GetAgentSharedManifestListsByTenant(int tenant)
        {
            IQueryable<AgentSharedManifestList> agentSharedManifestLists = from a in repository.context.AgentSharedManifests
                                                                           where a.Tenant == tenant
                                                                           select new AgentSharedManifestList()
                                                                           {
                                                                               Tenant = a.Tenant,
                                                                               Id = a.Id,
                                                                               Master = a.Master,
                                                                               AgentReference = a.AgentReference,
                                                                               CreateDate = a.CreateDate,
                                                                               UpdateDate = a.UpdateDate,
                                                                               UpdatedByUserId = a.UpdatedByUserId,
                                                                               ManifestXML = a.ManifestXML,
                                                                               SearchFields = a.SearchFields,
                                                                               TransportModeId = a.TransportModeId,
                                                                               GrossWeight = a.GrossWeight,
                                                                               ChargeableWeight = a.ChargeableWeight,
                                                                               TEU = a.TEU,
                                                                               PackagesQuantity = a.PackagesQuantity,
                                                                               AgentId = a.AgentId,
                                                                               DirectionId = a.DirectionId,
                                                                               FromPortId = a.FromPortId,
                                                                               ToPortId = a.ToPortId,
                                                                               StatusCode = a.StatusCode,
                                                                               TransportModeName = a.TransportModeId == "A" ? "Air" : a.TransportModeId == "I" ? "Inland" : a.TransportModeId == "I" ? "Ocean" : "",
                                                                               ShipmentLevelCode = a.ShipmentLevelCode,
                                                                               ShipmentLevelName = (a.ShipmentType != null ? a.ShipmentType.Name : "") + " " + (a.ShipmentLevel != null ? a.ShipmentLevel.Name : ""),
                                                                               CancelledBySenderAgent = a.CancelledBySenderAgent,

                                                                           };
            return agentSharedManifestLists;
        }


        public AgentSharedManifestPM GetAgentSharedManifestByAgentReference(string agentReference, int tenant)
        {
            AgentSharedManifestPM entity = (from a in repository.context.AgentSharedManifests
                                            where a.Tenant == tenant
                                            && a.AgentReference == agentReference
                                            select new AgentSharedManifestPM()
                                            {
                                                Tenant = a.Tenant,
                                                Id = a.Id,
                                                Master = a.Master,
                                                AgentReference = a.AgentReference,
                                                CreateDate = a.CreateDate,
                                                UpdateDate = a.UpdateDate,
                                                UpdatedByUserId = a.UpdatedByUserId,
                                                ManifestXML = a.ManifestXML,
                                                SearchFields = a.SearchFields,
                                                TransportModeId = a.TransportModeId,
                                                GrossWeight = a.GrossWeight,
                                                ChargeableWeight = a.ChargeableWeight,
                                                TEU = a.TEU,
                                                PackagesQuantity = a.PackagesQuantity,
                                                AgentId = a.AgentId,
                                                DirectionId = a.DirectionId,
                                                FromPortId = a.FromPortId,
                                                ToPortId = a.ToPortId,
                                                StatusCode = a.StatusCode,
                                                TransportModeName = a.TransportModeId == "A" ? "Air" : a.TransportModeId == "I" ? "Inland" : a.TransportModeId == "I" ? "Ocean" : "",
                                                ShipmentLevelCode = a.ShipmentLevelCode,
                                                CancelledBySenderAgent = a.CancelledBySenderAgent,
                                            }).FirstOrDefault();
            return entity;
        }



        public List<SharedManifestsStatusClass> GetAgentSharedManifestsForDashBoard( int lastMonths, int lastDays, int selectedIndex,  int currentTenant)
        {
            int months = 0;
            DateTime lastDate;
            int days;

            days = lastDays + 1;
            lastDate = DateTime.Today.Date.AddDays(days);

            if (lastMonths != 0)
            {
                months = lastMonths + 1;
                lastDate = DateTime.Today.Date.AddMonths(months);
            }

            IQueryable<AgentSharedManifest> agentSharedManifests = repository.GetAgentSharedManifestsForDashBoard(currentTenant);
            agentSharedManifests = agentSharedManifests.Where(d => d.CreateDate >= lastDate);
            List<SharedManifestsStatusClass> datalistAirManifests = (from s in agentSharedManifests
                                                                     where s.TransportModeId == "A"
                                                                     group s by new
                                                                     {
                                                                         s.CreateDate.Day,
                                                                         s.CreateDate.Month,
                                                                         s.CreateDate.Year,
                                                                     } into m
                                                                     orderby m.Key.Year, m.Key.Month, m.Key.Day
                                                                     select new SharedManifestsStatusClass()
                                                                     {
                                                                         day = m.Key.Day,
                                                                         month = m.Key.Month,
                                                                         year = m.Key.Year,
                                                                         DataType = "A",
                                                                         TotalAmount = m.Count(),
                                                                     }
                                              ).ToList();

            List<SharedManifestsStatusClass> datalistOceanManifests = (from s in agentSharedManifests
                                                                       where s.TransportModeId == "O"
                                                                       group s by new
                                                                       {
                                                                           s.CreateDate.Day,
                                                                           s.CreateDate.Month,
                                                                           s.CreateDate.Year,
                                                                       } into m
                                                                       orderby m.Key.Year, m.Key.Month, m.Key.Day
                                                                       select new SharedManifestsStatusClass()
                                                                       {
                                                                           day = m.Key.Day,
                                                                           month = m.Key.Month,
                                                                           year = m.Key.Year,
                                                                           DataType = "O",
                                                                           TotalAmount = m.Count(),
                                                                       }
                                  ).ToList();


            List<SharedManifestsStatusClass> datalistInlandManifests = (from s in agentSharedManifests
                                                                       where s.TransportModeId == "I"
                                                                       group s by new
                                                                       {
                                                                           s.CreateDate.Day,
                                                                           s.CreateDate.Month,
                                                                           s.CreateDate.Year,
                                                                       } into m
                                                                       orderby m.Key.Year, m.Key.Month, m.Key.Day
                                                                       select new SharedManifestsStatusClass()
                                                                       {
                                                                           day = m.Key.Day,
                                                                           month = m.Key.Month,
                                                                           year = m.Key.Year,
                                                                           DataType = "I",
                                                                           TotalAmount = m.Count(),
                                                                       }
                                  ).ToList();



            List<SharedManifestsStatusClass> datalist = datalistAirManifests.Concat(datalistOceanManifests).Concat(datalistInlandManifests).ToList();

            List<SharedManifestsStatusClass> datalist2 = null;
            switch (selectedIndex)
            {
                case 1:
                case 2:
                    {
                        #region by week
                        foreach (SharedManifestsStatusClass m in datalist)
                        {
                            DateTime todayDate = new DateTime(m.year, m.month, m.day);

                            int day = Convert.ToInt32(todayDate.DayOfWeek);
                            DateTime startOfWeek = todayDate.AddDays((-1 * day));
                            DateTime endOfWeek = todayDate.AddDays((6 - day));

                            m.StartDate = startOfWeek;
                            m.EndDate = endOfWeek;
                            m.DateRange = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;
                        }

                        if (selectedIndex == 1)
                        {
                            datalist = FillEmptyDates(datalist, -1);
                        }
                        else
                        {
                            datalist = FillEmptyDates(datalist, -3);
                        }

                        datalist2 = (from a in datalist
                                     group a by new
                                     {
                                         a.StartDate,
                                         a.EndDate,
                                         a.DateRange,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.StartDate, inv.Key.EndDate
                                     select new SharedManifestsStatusClass()
                                     {
                                         DateRange = inv.Key.DateRange,
                                         StartDate = inv.Key.StartDate,
                                         EndDate = inv.Key.EndDate,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        break;
                        #endregion
                    }
                case 0:
                    {
                        #region by 7 days
                        datalist2 = (from a in datalist
                                     group a by new
                                     {
                                         a.day,
                                         a.month,
                                         a.year,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.year, inv.Key.month, inv.Key.day
                                     select new SharedManifestsStatusClass()
                                     {
                                         DateRange = inv.Key.day.ToString() + "/" + inv.Key.month.ToString(),
                                         day = inv.Key.day,
                                         month = inv.Key.month,
                                         year = inv.Key.year,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        //if (datalist2.Count < 7)
                        //{
                            datalist2 = FillEmptyDates(datalist2, -6);
                            datalist2 = (from a in datalist2
                                         orderby a.year, a.month, a.day
                                         select a).ToList();
                        //}
                        break;

                        #endregion

                        #region by 6 months
                        //datalist2 = (from a in datalist
                        //             group a by new
                        //             {

                        //                 a.month,
                        //                 a.year,
                        //                 a.DataType,
                        //             } into inv
                        //             orderby inv.Key.year, inv.Key.month
                        //             select new SharedManifestsStatusClass()
                        //             {
                        //                 DateRange = inv.Key.month.ToString() + "/" + inv.Key.year.ToString(),

                        //                 month = inv.Key.month,
                        //                 year = inv.Key.year,
                        //                 TotalAmount = inv.Sum(d => d.TotalAmount),
                        //                 DataType = inv.Key.DataType,
                        //             }).ToList();
                        //if (datalist2.Count < 6)
                        //{
                        //    datalist2 = FillEmptyDates(datalist2, -5);
                        //    datalist2 = (from a in datalist2
                        //                 orderby a.year, a.month
                        //                 select a).ToList();
                        //}
                        //break;
                        #endregion
                    }
                case 3:
                    {
                        #region by 12 months
                        datalist2 = (from a in datalist
                                     group a by new
                                     {

                                         a.month,
                                         a.year,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.year, inv.Key.month
                                     select new SharedManifestsStatusClass()
                                     {
                                         DateRange = inv.Key.month.ToString() + "/" + inv.Key.year.ToString(),

                                         month = inv.Key.month,
                                         year = inv.Key.year,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        if (datalist2.Count < 12)
                        {
                            datalist2 = FillEmptyDates(datalist2, -12);
                            datalist2 = (from a in datalist2
                                         orderby a.year, a.month
                                         select a).ToList();
                        }
                        break;
                        #endregion
                    }
                case 5:
                    {
                        #region year by quarter
                        datalist2 = (from a in datalist
                                     group a by new
                                     {
                                         Quarter = ((a.month - 1) / 3) + 1,
                                         a.year,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.year, inv.Key.Quarter
                                     select new SharedManifestsStatusClass()
                                     {
                                         DateRange = "Q" + inv.Key.Quarter.ToString() + "." + inv.Key.year.ToString().Substring(2, 2),
                                         Quarter = inv.Key.Quarter,
                                         year = inv.Key.year,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        if (datalist2.Count < 4)
                        {
                            datalist2 = getFilledListByQuarters(datalist2, -11);
                            datalist2 = (from a in datalist2
                                         orderby a.year, a.Quarter
                                         select a).ToList();
                        }

                        break;
                        #endregion
                    }
                case 4:
                    {
                        #region 3 years by querter
                        datalist2 = (from a in datalist
                                     group a by new
                                     {
                                         Quarter = ((a.month - 1) / 3) + 1,
                                         a.year,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.year, inv.Key.Quarter
                                     select new SharedManifestsStatusClass()
                                     {
                                         DateRange = "Q" + inv.Key.Quarter.ToString() + "." + inv.Key.year.ToString().Substring(2, 2),
                                         Quarter = inv.Key.Quarter,
                                         year = inv.Key.year,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        if (datalist2.Count < 12)
                        {
                            datalist2 = getFilledListByQuarters(datalist2, -35);
                            datalist2 = (from a in datalist2
                                         orderby a.year, a.Quarter
                                         select a).ToList();
                        }
                        break;
                        #endregion
                    }
            }

            foreach (SharedManifestsStatusClass m in datalist2)
            {
                m.TotalAmountLabel = string.Format((string)"{0:#,0.00}", (object)m.TotalAmount);
            }


            return datalist2;
        }
        public List<SharedManifestsStatusClass> FillEmptyDates(List<SharedManifestsStatusClass> datalist, int months)
        {
            if (months == -1 || months == -3)
            {
                #region Fill the empty dates
                DateTime startDate = DateTime.Today.Date.AddMonths(months);

                while (startDate <= DateTime.Today.Date)
                {
                    DateTime date = startDate;
                    var monthManinfest = from s in datalist
                                         where s.month == date.Month && s.year == date.Year && s.day == date.Day
                                         select s;
                    if (monthManinfest.Count() == 0)
                    {
                        SharedManifestsStatusClass newEntry = new SharedManifestsStatusClass()
                        {
                            day = startDate.Day,
                            month = startDate.Month,
                            year = startDate.Year,
                            DataType = "A",
                            TotalAmount = 0,
                        };
                        SharedManifestsStatusClass newEntry2 = new SharedManifestsStatusClass()
                        {
                            day = startDate.Day,
                            month = startDate.Month,
                            year = startDate.Year,
                            DataType = "O",
                            TotalAmount = 0,
                        };

                        SharedManifestsStatusClass newEntry3 = new SharedManifestsStatusClass()
                        {
                            day = startDate.Day,
                            month = startDate.Month,
                            year = startDate.Year,
                            DataType = "I",
                            TotalAmount = 0,
                        };


                        int day = Convert.ToInt32(startDate.DayOfWeek);
                        DateTime startOfWeek = startDate.AddDays((-1 * day));
                        DateTime endOfWeek = startDate.AddDays((6 - day));

                        newEntry.StartDate = startOfWeek;
                        newEntry.EndDate = endOfWeek;
                        newEntry.DateRange = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;

                        newEntry2.StartDate = startOfWeek;
                        newEntry2.EndDate = endOfWeek;
                        newEntry2.DateRange = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;

                        newEntry3.StartDate = startOfWeek;
                        newEntry3.EndDate = endOfWeek;
                        newEntry3.DateRange = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;



                        datalist.Add(newEntry);
                        datalist.Add(newEntry2);
                        datalist.Add(newEntry3);
                    }

                    startDate = startDate.AddDays(1);
                }
                #endregion
            }
            else if (months == -6)
            {

                DateTime startDate = DateTime.Today.Date.AddDays(months);

                while (startDate <= DateTime.Today.Date)
                {
                    DateTime date = startDate;
                    var monthManinfest = from s in datalist
                                         where s.month == date.Month && s.year == date.Year && s.day == date.Day
                                         select s;

                    FillDatalist(datalist, startDate, monthManinfest, "A");

                    FillDatalist(datalist, startDate, monthManinfest, "O");

                    FillDatalist(datalist, startDate, monthManinfest, "I");

                    //if (monthManinfest.Count() == 0)
                    //{
                    //    SharedManifestsStatusClass newEntry = new SharedManifestsStatusClass()
                    //    {
                    //        day = startDate.Day,
                    //        month = startDate.Month,
                    //        year = startDate.Year,
                    //        DataType = "A",
                    //        DateRange = startDate.Day.ToString() + "/" + startDate.Month,
                    //        TotalAmount = 0,
                    //    };
                    //    SharedManifestsStatusClass newEntry2 = new SharedManifestsStatusClass()
                    //    {
                    //        day = startDate.Day,
                    //        month = startDate.Month,
                    //        year = startDate.Year,
                    //        DataType = "O",
                    //        DateRange = startDate.Day.ToString() + "/" + startDate.Month,
                    //        TotalAmount = 0,
                    //    };

                    //    SharedManifestsStatusClass newEntry3 = new SharedManifestsStatusClass()
                    //    {
                    //        day = startDate.Day,
                    //        month = startDate.Month,
                    //        year = startDate.Year,
                    //        DataType = "I",
                    //        DateRange = startDate.Day.ToString() + "/" + startDate.Month,
                    //        TotalAmount = 0,
                    //    };


                    //    datalist.Add(newEntry);
                    //    datalist.Add(newEntry2);
                    //    datalist.Add(newEntry3);
                    //}

                    startDate = startDate.AddDays(1);
                }
            }

            else
            {
                DateTime startDate = DateTime.Today.Date.AddMonths(months);

                while (startDate <= DateTime.Today.Date)
                {
                    DateTime date = startDate;
                    var monthManinfest = from s in datalist
                                         where s.month == date.Month && s.year == date.Year && s.day == date.Day
                                         select s;

                    //FillDatalist(datalist, startDate, monthManinfest, "A");

                    //FillDatalist(datalist, startDate, monthManinfest, "O");

                    //FillDatalist(datalist, startDate, monthManinfest, "I");

                    if (monthManinfest.Count() == 0)
                    {
                        SharedManifestsStatusClass newEntry = new SharedManifestsStatusClass()
                        {
                            day = startDate.Day,
                            month = startDate.Month,
                            year = startDate.Year,
                            DataType = "A",
                            DateRange = startDate.Month.ToString() + "/" + startDate.Year,
                            TotalAmount = 0,
                        };
                        SharedManifestsStatusClass newEntry2 = new SharedManifestsStatusClass()
                        {
                            day = startDate.Day,
                            month = startDate.Month,
                            year = startDate.Year,
                            DataType = "O",
                            DateRange = startDate.Month.ToString() + "/" + startDate.Year,
                            TotalAmount = 0,
                        };

                        SharedManifestsStatusClass newEntry3 = new SharedManifestsStatusClass()
                        {
                            day = startDate.Day,
                            month = startDate.Month,
                            year = startDate.Year,
                            DataType = "I",
                            DateRange = startDate.Month.ToString() + "/" + startDate.Year,
                            TotalAmount = 0,
                        };


                        datalist.Add(newEntry);
                        datalist.Add(newEntry2);
                        datalist.Add(newEntry3);
                    }

                    startDate = startDate.AddMonths(1);
                }
            }
            return datalist;
        }

        private  void FillDatalist(List<SharedManifestsStatusClass> datalist, DateTime startDate, IEnumerable<SharedManifestsStatusClass> monthManinfest , string dataType)
        {
            if (monthManinfest.Where(d => d.DataType == dataType).FirstOrDefault() == null)
            {
                SharedManifestsStatusClass newEntry = new SharedManifestsStatusClass()
                {
                    day = startDate.Day,
                    month = startDate.Month,
                    year = startDate.Year,
                    DataType = dataType,
                    DateRange = startDate.Day.ToString() + "/" + startDate.Month,
                    TotalAmount = 0,
                };
                datalist.Add(newEntry);
            }
        }

        private List<SharedManifestsStatusClass> getFilledListByQuarters(List<SharedManifestsStatusClass> list, int months)
        {
            DateTime startDate = DateTime.Today.Date.AddMonths(months);
            int startYear = startDate.Year;
            int startQuarter = Convert.ToInt32(((startDate.Month - 1) / 3) + 1);
            int endQuarter = Convert.ToInt32(((DateTime.Today.Date.Month - 1) / 3) + 1);

            while (startYear <= DateTime.Today.Date.Year)
            {
                while (startQuarter != 0)
                {
                    int year = startYear;
                    int quarter = startQuarter;
                    var quarterShipments = from s in list
                                           where s.year == year
                                           && s.Quarter == quarter
                                           select s;
                    if (quarterShipments.Count() == 0)
                    {
                        SharedManifestsStatusClass newEntry = new SharedManifestsStatusClass()
                        {
                            year = startYear,
                            Quarter = startQuarter,
                            DateRange = "Q" + startQuarter.ToString() + "." + startYear.ToString().Substring(2, 2),
                            TotalAmount = 0,
                            DataType = "A",
                        };
                        SharedManifestsStatusClass newEntry2 = new SharedManifestsStatusClass()
                        {
                            year = startYear,
                            Quarter = startQuarter,
                            DateRange = "Q" + startQuarter.ToString() + "." + startYear.ToString().Substring(2, 2),
                            TotalAmount = 0,
                            DataType = "O",
                        };

                        SharedManifestsStatusClass newEntry3 = new SharedManifestsStatusClass()
                        {
                            year = startYear,
                            Quarter = startQuarter,
                            DateRange = "Q" + startQuarter.ToString() + "." + startYear.ToString().Substring(2, 2),
                            TotalAmount = 0,
                            DataType = "I",
                        };

                        list.Add(newEntry);
                        list.Add(newEntry2);
                        list.Add(newEntry3);
                    }

                    startQuarter++;
                    if (startQuarter > 4)
                    {
                        startQuarter = 0;
                    }

                    else if (startYear == DateTime.Today.Date.Year && startQuarter > endQuarter)
                    {
                        startQuarter = 0;
                    }
                }

                startYear++;
                startQuarter++;
            }

            return list;
        }



        public bool IsAgentSharedManifests(string agentId , string agentReference)
        {
            string result = (from a in repository.context.AgentSharedManifests
                             where a.AgentId == agentId
                             && a.AgentReference == agentReference
                             select a.AgentId).FirstOrDefault();
    
            return !string.IsNullOrEmpty(result) ? true:false;
        }










    }
}