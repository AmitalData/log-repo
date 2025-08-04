using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
            var openDeclarationsGrouped = from dcs in context.DeclarationCourierStatuses
                                          where !dcs.IsClosedForFollowUp
                                          join cd in context.CourierDeclarations
                                          on dcs.DeclarationId equals cd.DeclarationId
                                          group dcs by cd.CourierMasterId into g
                                          select new
                                          {
                                              CourierMasterId = g.Key,
                                              OpenDeclarations = g.Count()
                                          };
            var today = DateTime.Now.Date;

            IQueryable<CourierMasterList> query = (from a in iQueryable.Include("CustomsAirline").Include("MAWBType").Include("OriginPort").Include("GatewayPort").Include("Card")
                                                   join openDecl in openDeclarationsGrouped
                                                   on a.Id equals openDecl.CourierMasterId into openDeclJoin
                                                   from openDecl in openDeclJoin.DefaultIfEmpty()

                                                   join qCourierDeclarationStatuses in context.DecCourierStatusesViews
                                                   on a.Id equals qCourierDeclarationStatuses.CourierMasterId
                                                   into qCourierDeclarationStatusesJoin
                                                   from myJoinCourierDeclarationStatuses in qCourierDeclarationStatusesJoin.DefaultIfEmpty()

                                                   select new CourierMasterList()
                                                   {
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
                                                       OpenDeclarations = openDecl != null ? openDecl.OpenDeclarations : 0,
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
            var ids = entityLists.Select(x => x.Id).ToHashSet();

            var qJoin = (
                from p in context.CourierDeclarations
                where ids.Contains(p.CourierMasterId)
                join dec in context.Declarations on p.DeclarationId equals dec.Id into DecJoin
                from myDeclarations in DecJoin.DefaultIfEmpty()
                join sts in context.DeclarationCourierStatuses on myDeclarations.Id equals sts.DeclarationId into StsJoin
                from myStatus in StsJoin.DefaultIfEmpty()
                select new
                {
                    p.CourierMasterId,
                    myDeclarations.CourierCustomStatusCode,
                    myStatus.IsClosedForFollowUp,
                    myStatus.CourierDeclarationStatusCode,
                    myStatus.CourierPendingReasonList,
                    myStatus.DocumentStatusCode,
                    myStatus.IsCourierMissingClassification
                }
            ).ToList();

            var joinMap = qJoin
                .GroupBy(r => r.CourierMasterId)
                .ToDictionary(g => g.Key, g =>
                {
                    var list = g.ToList();
                    int closed = list.Count(r => r.IsClosedForFollowUp == false);
                    int hawbQtyNoDocs = list.Count(s => !s.IsClosedForFollowUp && (s.DocumentStatusCode == "M" || s.DocumentStatusCode == "X"));

                    return new MyJoin
                    {
                        CourierMasterId = g.Key,
                        IsClosedForFollowUp0 = closed,
                        P900 = list.Count(r => r.CourierPendingReasonList?.Contains("900") == true),
                        IsCourierMissingClassification = list.Count(r => r.IsCourierMissingClassification == true),
                        IsMissingImporterId = list.Count(r => r.CourierPendingReasonList?.Contains("902") == true && !r.IsClosedForFollowUp),
                        IsPendingCustoms = list.Count(r => r.CourierCustomStatusCode == "2" && !r.IsClosedForFollowUp),
                        IsSuspendedDeclarations = list.Count(r => !r.IsClosedForFollowUp && r.CourierCustomStatusCode == "2"),
                        HawbQuantityNoDocuments = hawbQtyNoDocs,
                        NoDocumentsStatusR = list.Count(r => !r.IsClosedForFollowUp && r.CourierDeclarationStatusCode == "R"),
                        NoDocumentsStatusI = list.Count(r => !r.IsClosedForFollowUp && r.CourierDeclarationStatusCode == "I"),
                        NoDocumentsStatusV = list.Count(r => !r.IsClosedForFollowUp && r.CourierDeclarationStatusCode == "V"),
                    };
                });

            return entityLists.Select(a =>
            {
                joinMap.TryGetValue(a.Id, out var calc);

                int noDocs = 0;
                if (calc != null)
                {
                    if (calc.HawbQuantityNoDocuments > 0)
                        noDocs = calc.HawbQuantityNoDocuments;
                    else if (calc.NoDocumentsStatusR == calc.IsClosedForFollowUp0)
                        noDocs = -2;
                    else if (calc.NoDocumentsStatusI > 0)
                        noDocs = -3;
                    else if (calc.NoDocumentsStatusV == calc.IsClosedForFollowUp0)
                        noDocs = -4;
                }

                return new CourierMasterList
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
                    NoOfCourierHawbWithoutDelivery = string.IsNullOrWhiteSpace(a.NoOfCourierHawbWithoutDelivery) ? "0" : a.NoOfCourierHawbWithoutDelivery,
                    NoOfCourierHawbwWithoutHatara = string.IsNullOrWhiteSpace(a.NoOfCourierHawbwWithoutHatara) ? "0" : a.NoOfCourierHawbwWithoutHatara,

                    CalcClosedForFollowUp = calc?.IsClosedForFollowUp0 ?? 0,
                    CalcMissingClassification = calc?.IsCourierMissingClassification ?? 0,
                    CalcMissingImporterId = calc?.IsMissingImporterId ?? 0,
                    CalcPending900 = calc?.P900 ?? 0,
                    CalcPendingCustoms = calc?.IsPendingCustoms ?? 0,
                    CalcSuspendedDeclarations = calc?.IsSuspendedDeclarations ?? 0,
                    HawbQuantityNoDocuments = noDocs
                };
            }).ToList();
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

    internal  class HawbQuantityNoTransDeclarationClass
    {
        public  int Status { get; internal set; }
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
