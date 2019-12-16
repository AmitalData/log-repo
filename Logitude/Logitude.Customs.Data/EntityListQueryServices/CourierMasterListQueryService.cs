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

            var qJoin=
(from p in context.CourierDeclarations
 join dec in context.Declarations
                     on p.DeclarationId equals dec.Id
                     into DecJoin
 from myDeclarations in DecJoin
 join sts1 in context.DeclarationCourierStatuses
                     on myDeclarations.Id equals sts1.DeclarationId
                     into DeclarationCourierStatusesJoin
 from myDeclarationCourierStatuses in DeclarationCourierStatusesJoin
 select new { p.CourierMasterId, myDeclarations , myDeclarationCourierStatuses }



);
            var qMyJoin =
                (
                from rec in qJoin
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
                );


            IQueryable<CourierMasterList> query = (from a in iQueryable.Include("CustomsAirline").Include("MAWBType").Include("OriginPort").Include("GatewayPort").Include("Card")


//#if false


                                                   join recJoin in qMyJoin
                                                              on a.Id equals recJoin.CourierMasterId
                                                              into qrecJoin
                                                   from myJoin in qrecJoin.DefaultIfEmpty()
//#endif
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
                                                       CalcClosedForFollowUp = myJoin != null ? myJoin.IsClosedForFollowUp0 : 0,
                                                       CalcMissingClassification = myJoin != null ? myJoin.IsCourierMissingClassification : 0,
                                                       CalcMissingImporterId = myJoin != null ? myJoin.IsMissingImporterId : 0,
                                                       CalcPending900 = myJoin != null ? myJoin.P900 : 0,
                                                       CalcPendingCustoms = myJoin != null ? myJoin.IsPendingCustoms : 0,
                                                       CalcSuspendedDeclarations = myJoin != null ? myJoin.IsSuspendedDeclarations : 0,
                                                   });
            return query;
		}

		private IQueryable<CourierMaster> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CourierMaster> iQueryable, int tenant)
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

    }


}
	