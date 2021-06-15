using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class CourierMasterListQueryService
    {
        private IQueryable<CourierMasterList> GetIqueryableList(IQueryable<CourierMaster> iQueryable)
        {
            var today = DateTime.Now.Date;
            var qJoin =
(from p in context.CourierDeclarations
 join dec in context.Declarations
                     on p.DeclarationId equals dec.Id
                     into DecJoin
 from myDeclarations in DecJoin
 join sts1 in context.DeclarationCourierStatuses
                     on myDeclarations.Id equals sts1.DeclarationId
                     into DeclarationCourierStatusesJoin
 from myDeclarationCourierStatuses in DeclarationCourierStatusesJoin
 select new { p.CourierMasterId, myDeclarations, myDeclarationCourierStatuses }



);



            IQueryable<CourierMasterList> query = (from a in iQueryable.Include("CustomsAirline").Include("MAWBType").Include("OriginPort").Include("GatewayPort").Include("Card")

                                                   select new CourierMasterList()
                                                   {
                                                       // comments made because of cannot convert nclob to char exception ---mohammad
                                                       SearchFields = a.SearchFields,
                                                       AirlinePrefix = a.CustomsAirline == null ? null : a.CustomsAirline.AirlinePrefix,
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       MAWB = a.MAWB,
                                                       MAWBTypeCode = a.MAWBTypeCode,
                                                       MAWBTypeName = a.MAWBType == null ? null : a.MAWBType.Name,
                                                       HAWB = a.HAWB,
                                                       GatewayPortCode = a.GatewayPortCode,
                                                       OriginPortCode = a.OriginPortCode,
                                                       IsOpen = a.IsOpen,
                                                       IsCancelled = a.IsCancelled,
                                                       EstimatedArrivalDate = a.EstimatedArrivalDate,
                                                       //EstimatedArrivalDateOnly = a.EstimatedArrivalDate != null ? a.EstimatedArrivalDate.Value.Date : a.EstimatedArrivalDate,
                                                       GatewayPortName = a.GatewayPort != null ? a.GatewayPort.LocalName : null,
                                                       OriginPortName = a.OriginPort != null ? a.OriginPort.LocalName : null,
                                                       CreateDateTime = a.CreateDateTime,
                                                       CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.LocalName : null,
                                                       AirlineId = a.AirlineId,
                                                       AirlineName = a.CustomsAirline != null ? a.CustomsAirline.LocalName : null,
                                                       UpdateDateTime = a.UpdateDateTime,
                                                       UpdatedByUserName = a.User != null ? a.User.Contact.LocalName : null,
                                                       ManifestNumber = a.ManifestNumber,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       DepartureDate = a.DepartureDate,
                                                       FlightNumber = a.FlightNumber,
                                                       GrossMassMeasure = a.GrossMassMeasure,
                                                       PackageQuantity = a.PackageQuantity,
                                                       ShortHAWB = a.ShortHAWB,
                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                       WeightValueCode = a.WeightValueCode,
                                                       WeightValueName = a.FreightPaymentMethod != null ? a.FreightPaymentMethod.LocalName : null,
                                                       StorageSiteCode = a.StorageSiteCode,
                                                       StorageSiteName = a.DeliverySiteType != null ? a.DeliverySiteType.LocalName : null,
                                                       TruckerId = a.TruckerId,
                                                       IntegratorCode = a.IntegratorCode,
                                                       IntegratorName = a.Card != null ? a.Card.LocalName : null,
                                                       EstimatedArrivalColor =
                                                       a.EstimatedArrivalDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(a.EstimatedArrivalDate.Value) == today ? "Blue" :
                                                       (System.Data.Entity.DbFunctions.TruncateTime(a.EstimatedArrivalDate.Value) < today ? "Red" : "Black")) : "Black",
                                                       OpenDeclarations = a.OpenDeclarations,
                                                       CourierMasterRemarks =a.CourierMasterRemarks
                                                   });

            return query;
        }


        public string SetEstimatedArrivalColor(DateTime? EstimatedArrivalDate)
        {
            string sColor = "Black";
            var today = DateTime.Now.Date;

            if (EstimatedArrivalDate != null)
            {
                if (EstimatedArrivalDate == today)
                {
                    sColor = "Blue";
                }
                if (EstimatedArrivalDate < today)
                {
                    sColor = "Red";
                }
            }

            return sColor;
        }

        private IQueryable<CourierMaster> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CourierMaster> iQueryable, int tenant)
        {
            return iQueryable;

        }

        class MyJoin
        {
            public string CourierMasterId { get; set; }
            public int IsClosedForFollowUp0 { get; set; }
            public int P900 { get; set; }
            public int IsCourierMissingClassification { get; set; }
            public int IsMissingImporterId { get; set; }
            public int IsPendingCustoms { get; set; }
            public int IsSuspendedDeclarations { get; set; }
        }

        public List<CourierMasterList> AddCalcFields(List<CourierMasterList> entityLists)
        {
            var today = DateTime.Now.Date;
            var ids = entityLists.Select(x => x.Id).ToList();
            var qJoin =
(from p in context.CourierDeclarations
 join dec in context.Declarations
                     on p.DeclarationId equals dec.Id
                     into DecJoin
 from myDeclarations in DecJoin
 join sts1 in context.DeclarationCourierStatuses
                     on myDeclarations.Id equals sts1.DeclarationId
                     into DeclarationCourierStatusesJoin
 from myDeclarationCourierStatuses in DeclarationCourierStatusesJoin
 where ids.Contains(p.CourierMasterId)
 select new { p.CourierMasterId, myDeclarations, myDeclarationCourierStatuses });//.ToList();

            var qJoinList = qJoin.ToList();

            List<MyJoin> qMyJoin =
                  (
                  from rec in qJoinList
                  group rec by rec.CourierMasterId into g
                  select new MyJoin
                  {
                      CourierMasterId = g.Key,
                      IsClosedForFollowUp0 = g.Count(r => r.myDeclarationCourierStatuses.IsClosedForFollowUp == false),
                      P900 = g.Count(
                          r => r.myDeclarationCourierStatuses.CourierPendingReasonList != null && r.myDeclarationCourierStatuses.CourierPendingReasonList.Contains("900")),
                      IsCourierMissingClassification = g.Count(r => r.myDeclarationCourierStatuses.IsCourierMissingClassification == true),
                      IsMissingImporterId = g.Count(
                          r => (r.myDeclarationCourierStatuses.CourierPendingReasonList != null && r.myDeclarationCourierStatuses.CourierPendingReasonList.Contains("902"))
                          && r.myDeclarationCourierStatuses.IsClosedForFollowUp == false),
                      IsPendingCustoms = g.Count(r => r.myDeclarations.CourierCustomStatusCode == "2" && r.myDeclarationCourierStatuses.IsClosedForFollowUp == false),
                      IsSuspendedDeclarations = g.Count(r => r.myDeclarationCourierStatuses.IsClosedForFollowUp == false && r.myDeclarations.CourierCustomStatusCode == "2")
                  }
                  ).ToList();



            entityLists = entityLists.Select(a => new CourierMasterList()
            {
                SearchFields = a.SearchFields,
                AirlinePrefix = a.AirlinePrefix,
                Id = a.Id,
                Tenant = a.Tenant,
                MAWB = a.MAWB,
                MAWBTypeCode = a.MAWBTypeCode,
                MAWBTypeName = a.MAWBTypeName,
                HAWB = a.HAWB,
                GatewayPortCode = a.GatewayPortCode,
                OriginPortCode = a.OriginPortCode,
                IsOpen = a.IsOpen,
                IsCancelled = a.IsCancelled,
                EstimatedArrivalDate = a.EstimatedArrivalDate,
                GatewayPortName = a.GatewayPortName,
                OriginPortName = a.OriginPortName,
                CreateDateTime = a.CreateDateTime,
                CreatedByUserName = a.CreatedByUserName,
                AirlineId = a.AirlineId,
                AirlineName = a.AirlineName,
                UpdateDateTime = a.UpdateDateTime,
                UpdatedByUserName = a.UpdatedByUserName,
                ManifestNumber = a.ManifestNumber,
                CreatedByUserId = a.CreatedByUserId,
                DepartureDate = a.DepartureDate,
                FlightNumber = a.FlightNumber,
                GrossMassMeasure = a.GrossMassMeasure,
                PackageQuantity = a.PackageQuantity,
                ShortHAWB = a.ShortHAWB,
                UpdatedByUserId = a.UpdatedByUserId,
                WeightValueCode = a.WeightValueCode,
                WeightValueName = a.WeightValueName,
                StorageSiteCode = a.StorageSiteCode,
                StorageSiteName = a.StorageSiteName,
                TruckerId = a.TruckerId,
                IntegratorCode = a.IntegratorCode,
                IntegratorName = a.IntegratorName,
                EstimatedArrivalColor = a.EstimatedArrivalColor,
                CalcClosedForFollowUp = qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id) != null ? qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).IsClosedForFollowUp0 : 0,
                CalcMissingClassification = qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id) != null ? qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).IsCourierMissingClassification : 0,
                CalcMissingImporterId = qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id) != null ? qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).IsMissingImporterId : 0,
                CalcPending900 = qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id) != null ? qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).P900 : 0,
                CalcPendingCustoms = qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id) != null ? qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).IsPendingCustoms : 0,
                CalcSuspendedDeclarations = qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id) != null ? qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).IsSuspendedDeclarations : 0,
            }).ToList();

            return entityLists;
        }
    }


}
