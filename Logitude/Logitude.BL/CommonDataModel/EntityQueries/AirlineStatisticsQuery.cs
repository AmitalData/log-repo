using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.Helpers;
using Simplog.Data.Helpers;
using Logitude.BL.DataContracts;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AirlineStatisticsQuery
    {
        AirlineStatisticsRepository repository;

  

        public AirlineStatisticsQuery(int tenant)
        {
            repository = new AirlineStatisticsRepository(tenant);
        }

        public AirlineStatisticsQuery(AirlineStatisticsRepository airlineStatisticsRepository)
        {
            repository = airlineStatisticsRepository;
        }

        public AirlineStatisticsPM GetSinglePM(string id, int tenant)
        {
            AirlineStatisticsPM entityPM = (from a in repository.context.AirlineStatistics.Include("ShipmentLevel")
                                            where a.Id == id
                                            select new AirlineStatisticsPM()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                SourceTenant = a.SourceTenant,
                                                SourceTenantName = a.SourceTenantName,
                                                ShipmentId = a.ShipmentId,
                                                ShipmentLevelCode = a.ShipmentLevelCode,
                                                BookingId = a.BookingId,
                                                EntityReference = a.EntityReference,
                                                AWBNumber = a.AWBNumber,
                                                HWBNumber = a.HWBNumber,
                                                AirlineCode = a.AirlineCode,
                                                CreateDate = a.CreateDate,
                                                UpdateDate = a.UpdateDate,
                                                EntitiyCreateDate = a.EntitiyCreateDate,
                                                EntitiyUpdateDate = a.EntitiyUpdateDate,
                                                EntityCreatedByUserName = a.EntityCreatedByUserName,
                                                MessageType = a.MessageType,
                                                LastSentDate = a.LastSentDate,
                                                EntityStatus = a.EntityStatus,
                                                NumberOfPackages = a.NumberOfPackages,
                                                ChargeableWeight = a.ChargeableWeight,
                                                ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                                                GrossWeight = a.GrossWeight,
                                                GrossWeightUnitCode = a.GrossWeightUnitCode,
                                                Volume = a.Volume,
                                                VolumeUnitCode = a.VolumeUnitCode,
                                                OriginCode = a.OriginCode,
                                                DestinationCode = a.DestinationCode,
                                                DescriptionOfGoods = a.DescriptionOfGoods,
                                                ShipperName = a.ShipperName,
                                                ConsigneeName = a.ConsigneeName,
                                                Flight1 = a.Flight1,
                                                Flight1Date = a.Flight1Date,
                                                Flight2 = a.Flight2,
                                                Flight2Date = a.Flight2Date,
                                                Flight3 = a.Flight3,
                                                Flight3Date = a.Flight3Date,
                                                OnCarriageTo = a.OnCarriageTo,
                                                OnCarriageDate = a.OnCarriageDate,
                                                PreCarriageFrom = a.PreCarriageFrom,
                                                PreCarriageDate = a.PreCarriageDate,
                                                Allotment = a.Allotment,
                                                SearchFields = a.SearchFields,
                                            }).FirstOrDefault();

            return entityPM;
        }

        public IQueryable<AirlineStatisticsPM> GetAirlineStatisticsPMsByTenant(int tenant)
        {
            return (from a in repository.context.AirlineStatistics.Include("ShipmentLevel")
                    where a.Tenant == tenant
                    select new AirlineStatisticsPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        SourceTenant = a.SourceTenant,
                        SourceTenantName = a.SourceTenantName,
                        ShipmentId = a.ShipmentId,
                        ShipmentLevelCode = a.ShipmentLevelCode,
                        BookingId = a.BookingId,
                        EntityReference = a.EntityReference,
                        AWBNumber = a.AWBNumber,
                        HWBNumber = a.HWBNumber,
                        AirlineCode = a.AirlineCode,
                        CreateDate = a.CreateDate,
                        UpdateDate = a.UpdateDate,
                        EntitiyCreateDate = a.EntitiyCreateDate,
                        EntitiyUpdateDate = a.EntitiyUpdateDate,
                        EntityCreatedByUserName = a.EntityCreatedByUserName,
                        MessageType = a.MessageType,
                        LastSentDate = a.LastSentDate,
                        EntityStatus = a.EntityStatus,
                        NumberOfPackages = a.NumberOfPackages,
                        ChargeableWeight = a.ChargeableWeight,
                        ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                        GrossWeight = a.GrossWeight,
                        GrossWeightUnitCode = a.GrossWeightUnitCode,
                        Volume = a.Volume,
                        VolumeUnitCode = a.VolumeUnitCode,
                        OriginCode = a.OriginCode,
                        DestinationCode = a.DestinationCode,
                        DescriptionOfGoods = a.DescriptionOfGoods,
                        ShipperName = a.ShipperName,
                        ConsigneeName = a.ConsigneeName,
                        Flight1 = a.Flight1,
                        Flight1Date = a.Flight1Date,
                        Flight2 = a.Flight2,
                        Flight2Date = a.Flight2Date,
                        Flight3 = a.Flight3,
                        Flight3Date = a.Flight3Date,
                        OnCarriageTo = a.OnCarriageTo,
                        OnCarriageDate = a.OnCarriageDate,
                        PreCarriageFrom = a.PreCarriageFrom,
                        PreCarriageDate = a.PreCarriageDate,
                        Allotment = a.Allotment,
                        SearchFields = a.SearchFields,
                    });
        }

        public IQueryable<AirlineStatisticsList> GetIQueryableEntityList(IQueryable<AirlineStatistics> iQueryable)
        {
            IQueryable<AirlineStatisticsList> result = from a in iQueryable.Include("ShipmentLevel")
                                                       select new AirlineStatisticsList()
                                                      {

                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          SourceTenant = a.SourceTenant,
                                                          SourceTenantName = a.SourceTenantName,
                                                          ShipmentId = a.ShipmentId,
                                                          ShipmentLevelCode = a.ShipmentLevelCode,
                                                          BookingId = a.BookingId,
                                                          EntityReference = a.EntityReference,
                                                          AWBNumber = a.AWBNumber,
                                                          HWBNumber = a.HWBNumber,
                                                          AirlineCode = a.AirlineCode,
                                                          CreateDate = a.CreateDate,
                                                          UpdateDate = a.UpdateDate,
                                                          EntitiyCreateDate = a.EntitiyCreateDate,
                                                          EntitiyUpdateDate = a.EntitiyUpdateDate,
                                                          EntityCreatedByUserName = a.EntityCreatedByUserName,
                                                          MessageType = a.MessageType,
                                                          LastSentDate = a.LastSentDate,
                                                          EntityStatus = a.EntityStatus,
                                                          NumberOfPackages = a.NumberOfPackages,
                                                          ChargeableWeight = a.ChargeableWeight,
                                                          ChargeableWeightUnitCode = a.ChargeableWeightUnitCode,
                                                          GrossWeight = a.GrossWeight,
                                                          GrossWeightUnitCode = a.GrossWeightUnitCode,
                                                          Volume = a.Volume,
                                                          VolumeUnitCode = a.VolumeUnitCode,
                                                          OriginCode = a.OriginCode,
                                                          DestinationCode = a.DestinationCode,
                                                          DescriptionOfGoods = a.DescriptionOfGoods,
                                                          ShipperName = a.ShipperName,
                                                          ConsigneeName = a.ConsigneeName,
                                                          Flight1 = a.Flight1,
                                                          Flight1Date = a.Flight1Date,
                                                          Flight2 = a.Flight2,
                                                          Flight2Date = a.Flight2Date,
                                                          Flight3 = a.Flight3,
                                                          Flight3Date = a.Flight3Date,
                                                          OnCarriageTo = a.OnCarriageTo,
                                                          OnCarriageDate = a.OnCarriageDate,
                                                          PreCarriageFrom = a.PreCarriageFrom,
                                                          PreCarriageDate = a.PreCarriageDate,
                                                          Allotment = a.Allotment,
                                                          SearchFields = a.SearchFields,
                                                      };
            return result;
        }

        public List<DashBoardBookingClass> GetDashBoardBookings(int tenant)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime date1 = todayDate.AddDays(-100);
            DateTime date2 = todayDate.AddHours(23).AddMinutes(59).AddSeconds(59);

            IQueryable<AirlineStatistics> entityList = repository.context.AirlineStatistics.Where( a => a.Tenant == tenant && !string.IsNullOrEmpty(a.BookingId));
            entityList = entityList.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.EntitiyCreateDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.EntitiyCreateDate) <= date2);

            List<DashBoardBookingClass> datalist = (from a in entityList
                                                    where a.Tenant == tenant
                                                    group a by new
                                                    {
                                                        a.MessagingStatus,
                                                    } into gr
                                                    orderby gr.Key.MessagingStatus
                                                    select new DashBoardBookingClass()
                                                    {
                                                        Count = gr.Count(),
                                                        FFRStatusName = gr.Key.MessagingStatus,
                                                    }).ToList();

            datalist = datalist.OrderByDescending(d => d.Count).ToList();
            return datalist;
        }

        public List<ChartingDataClass> GetTopParticipantsDashBoard(int lastDays, int tenant)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            DateTime lastDate;
            int days;

            days = lastDays + 1;
            lastDate = DateTime.Today.Date.AddDays(days);
            
            IQueryable<AirlineStatistics> dataSourceQuery =
                (from d in repository.context.AirlineStatistics
                 where d.Tenant == tenant
                 select d);

            if (dataSourceQuery != null)
            {
                dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.EntitiyCreateDate) >= lastDate);

                IQueryable<AirlineStatistics> data_FWB = dataSourceQuery.Where(d => d.MessageType == "FWB");
                IQueryable<AirlineStatistics> data_FHL = dataSourceQuery.Where(d => d.MessageType == "FHL");
                IQueryable<AirlineStatistics> data_FFR = dataSourceQuery.Where(d => d.MessageType == "FFR");

                List<ChartingDataClass> myData = new List<ChartingDataClass>();

                myData =
                    (from d in dataSourceQuery
                     group d by new { d.SourceTenant, d.SourceTenantName } into g
                     select new ChartingDataClass()
                     {
                         Id = g.Key.SourceTenant.ToString(),
                         StringProperty = g.Key.SourceTenantName,
                         ParticipantId = g.Key.SourceTenant.ToString(),
                         IntegerProperty = g.Count()
                     })
                     .OrderByDescending(o => o.IntegerProperty)
                     .Take(5)
                     .ToList();


                foreach (ChartingDataClass item in myData)
                {
                    myResult.Add(new ChartingDataClass()
                    {
                        Id = item.Id + ":FWB",
                        DataTypeCode = "FWB",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_FWB.Where(d => d.SourceTenant.ToString() == item.Id).Count(),
                        ParticipantId = item.Id,
                    });

                    myResult.Add(new ChartingDataClass()
                    {
                        Id = item.Id + ":FHL",
                        DataTypeCode = "FHL",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_FHL.Where(d => d.SourceTenant.ToString() == item.Id).Count(),
                        ParticipantId = item.Id,
                    });

                    myResult.Add(new ChartingDataClass()
                    {
                        Id = item.Id + ":FFR",
                        DataTypeCode = "FFR",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_FFR.Where(d => d.SourceTenant.ToString() == item.Id).Count(),
                        ParticipantId = item.Id,
                    });
                }
            }

            return myResult;
        }
    }
}
