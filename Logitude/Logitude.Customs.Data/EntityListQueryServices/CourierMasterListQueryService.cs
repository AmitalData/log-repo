using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using Devart.Common;
using Devart.Data.Linq;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class CourierMasterListQueryService
    {

        IQueryable<HawbQuantityNoDocumentsClass> qHawbQuantityNoDocuments;
        IQueryable<IsCourierMissingClassificationClass> qIsCourierMissingClassification;
        IQueryable<HawbQuantityNoTransManifestClass> qHawbQuantityNoTransManifest;
        IQueryable<HawbQuantityNoTransDeclarationClass> qHawbQuantityNoTransDeclaration;
        IQueryable<HawbQuantityNoTransPaymentClass> qHawbQuantityNoTransPayment;
        private IQueryable<CourierMasterList> GetIqueryableList(IQueryable<CourierMaster> iQueryable)
        {

            //SetQuantity();

            var today = DateTime.Now.Date;

            IQueryable<CourierMasterList> query = (from a in iQueryable.Include("CustomsAirline").Include("MAWBType").Include("OriginPort").Include("GatewayPort").Include("Card")

                                                   join qCourierDeclarationStatuses in context.DecCourierStatusesViews
                                                   on a.Id equals qCourierDeclarationStatuses.CourierMasterId
                                                   into qCourierDeclarationStatusesJoin
                                                   from myJoinCourierDeclarationStatuses in qCourierDeclarationStatusesJoin.DefaultIfEmpty()
                                                       //    join recHawbQuantityNoDocuments in qHawbQuantityNoDocuments
                                                       //on a.Id equals recHawbQuantityNoDocuments.CourierMasterId
                                                       // into joingHawbQuantityNoDocuments
                                                       //    from recHawbQuantityNoDocuments in joingHawbQuantityNoDocuments.DefaultIfEmpty()

                                                       //    join recIsCourierMissingClassification in qIsCourierMissingClassification
                                                       // on a.Id equals recIsCourierMissingClassification.CourierMasterId
                                                       // into joingIsCourierMissingClassification
                                                       //    from recIsCourierMissingClassification in joingIsCourierMissingClassification.DefaultIfEmpty()

                                                       //    join recHawbQuantityNoTransManifest in qHawbQuantityNoTransManifest
                                                       // on a.Id equals recHawbQuantityNoTransManifest.CourierMasterId
                                                       // into joingHawbQuantityNoTransManifest
                                                       //    from recHawbQuantityNoTransManifest in joingHawbQuantityNoTransManifest.DefaultIfEmpty()


                                                       //    join recHawbQuantityNoTransDeclaration in qHawbQuantityNoTransDeclaration
                                                       //                                                     on a.Id equals recHawbQuantityNoTransDeclaration.CourierMasterId
                                                       //                                                     into joingHawbQuantityNoTransDeclaration
                                                       //    from recHawbQuantityNoTransDeclaration in joingHawbQuantityNoTransDeclaration.DefaultIfEmpty()

                                                       //    join recHawbQuantityNoTransPayment in qHawbQuantityNoTransPayment
                                                       // on a.Id equals recHawbQuantityNoTransPayment.CourierMasterId
                                                       //  into joingHawbQuantityNoTransPayment
                                                       //    from recHawbQuantityNoTransPayment in joingHawbQuantityNoTransPayment.DefaultIfEmpty()

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
                                                       CourierMasterRemarks = a.CourierMasterRemarks,
                                                       EstimatedArrivalTimeOnly = a.EstimatedArrivalDate,
                                                       EstimatedArrivalDateOnly = a.EstimatedArrivalDate,
                                                       NoOfCourierHawbWithoutDelivery = a.NoOfCourierHawbWithoutDelivery,
                                                       NoOfCourierHawbwWithoutHatara = a.NoOfCourierHawbwWithoutHatara,
                                                       LandingDateDateOnly = a.LandingDate,
                                                       LandingDateTimeOnly = (DateTime)a.LandingDate,
                                                       HawbQuantityNoDocuments = myJoinCourierDeclarationStatuses.QuantityNoDocuments,
                                                       HawbQuantityNoClassification = myJoinCourierDeclarationStatuses.QuantityNoClassification,
                                                       HawbQuantityNoTransManifest = myJoinCourierDeclarationStatuses.QuantityNoManifest,
                                                       HawbQuantityNoTransDeclaration = myJoinCourierDeclarationStatuses.QuantityNoDeclaration,

                                                       DocumentStatusCode = myJoinCourierDeclarationStatuses.DocumentStatusCode,
                                                       CourierPaymentStatusCode = myJoinCourierDeclarationStatuses.CourierPaymentStatusCode,
                                                       CourierDeclarationStatusCode = myJoinCourierDeclarationStatuses.CourierDeclarationStatusCode,
                                                       CourierManifestStatusCode = myJoinCourierDeclarationStatuses.CourierManifestStatusCode,
                                                       IsCourierMissingClassification = myJoinCourierDeclarationStatuses.IsCourierMissingClassification,
                                                       IsReadyForInvoice = a.IsReadyForInvoice
                                                   }); ;
            return query;
        }
        public class DecCourierStatuses
        {

            public string CourierMasterId { get; set; }

            public int Tenant { get; set; }
            public int QuantityNoDocuments { get; set; }
            public int QuantityNoClassification { get; set; }
            public int QuantityNoManifest { get; set; }
            public int QuantityNoDeclaration { get; set; }

            public string DocumentStatusCode { get; set; }
            public string CourierPaymentStatusCode { get; set; }
            public string CourierDeclarationStatusCode { get; set; }
            public string CourierManifestStatusCode { get; set; }
            public string IsCourierMissingClassification { get; set; }


        }
        private void SetQuantity()
        {
            var qHawbQuantity =
                     (from cd in context.CourierDeclarations
                      join cds in context.DeclarationCourierStatuses
                          .Where(r => r.IsClosedForFollowUp == false)

                          on cd.DeclarationId equals cds.DeclarationId  //into DeclarationCourierStatusesJoin

                      select new
                      {
                          cds.IsCourierMissingClassification,
                          cds.CourierManifestStatusCode,
                          cds.CourierDeclarationStatusCode,
                          cds.CourierPaymentStatusCode,
                          cds.DocumentStatusCode,
                          cd.CourierMasterId,
                          cds.DeclarationId,
                      }
                );
            var gHawbQuantityNoDocumentsClass = (
                 from flightStatisticRows in qHawbQuantity
                 group flightStatisticRows by flightStatisticRows.CourierMasterId
                 into g
                 select new HawbQuantityNoDocumentsClass
                 {
                     Status = g.Any(s => s.DocumentStatusCode == "M" || s.DocumentStatusCode == "X") ? -1 :
                     g.Any(s => s.DocumentStatusCode == "I") ? -2 : g.All(s => s.DocumentStatusCode == "V") ? -3 : 0,
                     CourierMasterId = g.Key,
                     HawbQuantityNoDocuments = g.Count(s => s.DocumentStatusCode == "M" || s.DocumentStatusCode == "X")
                 }
              );
            qHawbQuantityNoDocuments = gHawbQuantityNoDocumentsClass;

            var gHawbQuantityNoTransPayment = (
                from flightStatisticRows in qHawbQuantity
                group flightStatisticRows by flightStatisticRows.CourierMasterId
                into g
                select new HawbQuantityNoTransPaymentClass
                {
                    Status = g.Any(s => s.CourierPaymentStatusCode == "I") ? -1 :
                                g.Any(s => s.CourierPaymentStatusCode == "R") ? -2 :
                                g.Any(s => s.CourierPaymentStatusCode == "O") ? -3 :
                                g.Any(s => s.CourierPaymentStatusCode == "P") ? -4 : 0,
                    CourierMasterId = g.Key,
                    HawbQuantityNoTransPayment = 0,
                }

                );
            qHawbQuantityNoTransPayment = gHawbQuantityNoTransPayment;

            var gHawbQuantityNoTransDeclaration = (
           from flightStatisticRows in qHawbQuantity
           group flightStatisticRows by flightStatisticRows.CourierMasterId
           into g
           select new HawbQuantityNoTransDeclarationClass
           {
               Status = g.Any(s => s.CourierDeclarationStatusCode == "X" || s.CourierDeclarationStatusCode == "M") ? -1 :
               g.All(s => s.CourierDeclarationStatusCode == "R") ? -2 : g.Any(s => s.CourierDeclarationStatusCode == "I") ? -3 : g.All(s => s.CourierDeclarationStatusCode == "V") ? -4 : 0,
               CourierMasterId = g.Key,
               HawbQuantityNoTransDeclaration = g.Count(s => s.CourierDeclarationStatusCode == "X" || s.CourierDeclarationStatusCode == "M"),
           }

           );
            qHawbQuantityNoTransDeclaration = gHawbQuantityNoTransDeclaration;


            var gHawbQuantityNoTransManifest = (
                 from flightStatisticRows in qHawbQuantity
                 group flightStatisticRows by flightStatisticRows.CourierMasterId
                 into g
                 select new HawbQuantityNoTransManifestClass
                 {
                     Status = g.Any(s => s.CourierManifestStatusCode == "X" || s.CourierManifestStatusCode == "M") ? -1 :
                                     g.All(s => s.CourierManifestStatusCode == "R") ? -2 :
                                     g.Any(s => s.CourierManifestStatusCode == "I") ? -3 :
                                     g.All(s => s.CourierManifestStatusCode == "V") ? -4 : 0,
                     CourierMasterId = g.Key,
                     HawbQuantityNoTransManifest = g.Count(s => s.CourierManifestStatusCode == "X" || s.CourierManifestStatusCode == "M"),
                 }

                );
            qHawbQuantityNoTransManifest = gHawbQuantityNoTransManifest;

            var gHawbQuantityNoTransClassification = (
                from flightStatisticRows in qHawbQuantity
                group flightStatisticRows by flightStatisticRows.CourierMasterId
                 into g
                select new IsCourierMissingClassificationClass
                {
                    Status = g.Any(s => s.IsCourierMissingClassification) ? 1 : g.All(s => s.IsCourierMissingClassification == false) ? 0 : -1,
                    CourierMasterId = g.Key,
                    HawbQuantityNoClassification = g.Count(s => s.IsCourierMissingClassification),
                }

              ); ;

            qIsCourierMissingClassification = gHawbQuantityNoTransClassification;

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
            public int HawbQuantityNoDocuments { get; set; }
            public int NoDocumentsStatusI { get; set; }
            public int NoDocumentsStatusR { get; set; }
            public int NoDocumentsStatusV { get; set; }
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
 select new
 {
     p.CourierMasterId,
     myDeclarations.CourierCustomStatusCode,
     myDeclarationCourierStatuses.IsClosedForFollowUp,
     myDeclarationCourierStatuses.CourierDeclarationStatusCode,
     myDeclarationCourierStatuses.CourierPendingReasonList,
     myDeclarationCourierStatuses.DocumentStatusCode,
     myDeclarationCourierStatuses.IsCourierMissingClassification
 });

            var qJoinList = qJoin.ToList();

            List<MyJoin> qMyJoin =
                  (
                  from rec in qJoinList
                  group rec by rec.CourierMasterId into g
                  select new MyJoin
                  {
                      CourierMasterId = g.Key,
                      IsClosedForFollowUp0 = g.Count(r => r.IsClosedForFollowUp == false),
                      P900 = g.Count(
                          r => r.CourierPendingReasonList != null && r.CourierPendingReasonList.Contains("900")),
                      IsCourierMissingClassification = g.Count(r => r.IsCourierMissingClassification == true),
                      IsMissingImporterId = g.Count(
                          r => (r.CourierPendingReasonList != null && r.CourierPendingReasonList.Contains("902"))
                          && r.IsClosedForFollowUp == false),
                      IsPendingCustoms = g.Count(r => r.CourierCustomStatusCode == "2" && r.IsClosedForFollowUp == false),
                      IsSuspendedDeclarations = g.Count(r => r.IsClosedForFollowUp == false && r.CourierCustomStatusCode == "2"),

                      HawbQuantityNoDocuments = g.Count(s => s.IsClosedForFollowUp == false && s.DocumentStatusCode == "M"
                      || s.IsClosedForFollowUp == false && s.DocumentStatusCode == "X"),

                      NoDocumentsStatusR = g.Count(s => s.IsClosedForFollowUp == false && s.CourierDeclarationStatusCode == "R"),
                      NoDocumentsStatusI = g.Count(s => s.IsClosedForFollowUp == false && s.CourierDeclarationStatusCode == "I"),
                      NoDocumentsStatusV = g.Count(s => s.IsClosedForFollowUp == false && s.CourierDeclarationStatusCode == "V"),

                      //NoDocumentsStatus = g.All(s => s.IsClosedForFollowUp == false && s.CourierDeclarationStatusCode == "R") ? -2 :
                      //                    g.Any(s => s.IsClosedForFollowUp == false && s.CourierDeclarationStatusCode == "I") ? -3 :
                      //                    g.All(s => s.IsClosedForFollowUp == false && s.CourierDeclarationStatusCode == "V") ? -4 : 0,

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
                NoOfCourierHawbWithoutDelivery = string.IsNullOrWhiteSpace(a.NoOfCourierHawbWithoutDelivery) ? "0" : a.NoOfCourierHawbWithoutDelivery,
                NoOfCourierHawbwWithoutHatara = string.IsNullOrWhiteSpace(a.NoOfCourierHawbwWithoutHatara) ? "0" : a.NoOfCourierHawbwWithoutHatara,

                HawbQuantityNoDocuments = qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id) == null ? 0 :
                                            qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).HawbQuantityNoDocuments > 0 ? qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).HawbQuantityNoDocuments
                                            : qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).NoDocumentsStatusR == qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).IsClosedForFollowUp0 ? -2
                                            : qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).NoDocumentsStatusI > 0 ? -3
                                            : qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).NoDocumentsStatusV == qMyJoin.FirstOrDefault(c => c.CourierMasterId == a.Id).IsClosedForFollowUp0 ? -4 : 0,


            }).ToList();

            return entityLists;
        }
    }
    internal class HawbQuantityNoDocumentsClass
    {
        public int Status { get; internal set; }
        public string CourierMasterId { get; internal set; }
        public int HawbQuantityNoDocuments { get; internal set; }
    }

    internal class IsCourierMissingClassificationClass
    {

        public int Status { get; internal set; }
        public string CourierMasterId { get; internal set; }
        public int HawbQuantityNoClassification { get; internal set; }
    }

    internal class HawbQuantityNoTransManifestClass
    {
        public int Status { get; internal set; }
        public string CourierMasterId { get; internal set; }
        public int HawbQuantityNoTransManifest { get; internal set; }
    }

    internal class HawbQuantityNoTransDeclarationClass
    {
        public int Status { get; internal set; }
        public string CourierMasterId { get; internal set; }
        public int HawbQuantityNoTransDeclaration { get; internal set; }
    }

    internal class HawbQuantityNoTransPaymentClass
    {
        public int Status { get; internal set; }
        public string CourierMasterId { get; internal set; }
        public int HawbQuantityNoTransPayment { get; internal set; }
    }
}
