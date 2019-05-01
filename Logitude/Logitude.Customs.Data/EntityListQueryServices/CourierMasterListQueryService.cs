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
            IQueryable<CourierMasterList> query = (from a in iQueryable.Include("CustomsAirline").Include("MAWBType").Include("OriginPort").Include("GatewayPort")
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
                                                   });
            return query;
		}

		private IQueryable<CourierMaster> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CourierMaster> iQueryable, int tenant)
        {
            return iQueryable;

        }
	}


}
	