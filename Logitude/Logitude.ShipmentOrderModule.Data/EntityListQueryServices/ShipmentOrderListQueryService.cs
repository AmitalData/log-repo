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

using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Data.EntityLists;

namespace Logitude.ShipmentOrderModule.Data.EntityListQueryServices
{

    public partial class ShipmentOrderListQueryService
    {
        private IQueryable<ShipmentOrderList> GetIqueryableList(IQueryable<ShipmentOrder> iQueryable)
        {
            IQueryable<ShipmentOrderList> query = (from a in iQueryable
                                                   select new ShipmentOrderList()
                                                   {

                                                       Id = a.Id,

                                                       Tenant = a.Tenant,

                                                       CreateDate = a.CreateDate,

                                                       CreatedByUserId = a.CreatedByUserId,

                                                       UpdateDate = a.UpdateDate,

                                                       UpdatedByUserId = a.UpdatedByUserId,

                                                       SearchFields = a.SearchFields,

                                                       OrderNumber = a.OrderNumber,

                                                       TransportModeId = a.TransportModeId,

                                                       ConsigneeId = a.ConsigneeId,

                                                       ShipperId = a.ShipperId,

                                                       AgentId = a.AgentId,

                                                       IncotermId = a.IncotermId,

                                                       AccountManagerId = a.AccountManagerId,

                                                       PONumber = a.PONumber,

                                                       DescriptionOfGoods = a.DescriptionOfGoods,

                                                       ShipmentTypeId = a.ShipmentTypeId,

                                                       House = a.House,

                                                       Master = a.Master,

                                                       CustomsAgentId = a.CustomsAgentId,

                                                       SpecialServicesTypeId = a.SpecialServicesTypeId,

                                                       ShipmentTypeName = a.ShipmentType == null ? "" : a.ShipmentType.Name,

                                                       TransportModeName = a.TransportMode == null ? "" : a.TransportMode.Name,

                                                       BookingConfirmationDate = a.BookingConfirmationDate,

                                                       CustomerReferences = a.CustomerReferences,

                                                       IsReadyForPickup = a.IsReadyForPickup,

                                                       PickupEstimatedDateTime = a.PickupEstimatedDateTime,

                                                       PickupActualDateTime = a.PickupActualDateTime,

                                                       ForwarderId = a.ForwarderId,

                                                   });
            return query;
        }

        private IQueryable<ShipmentOrder> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ShipmentOrder> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<ShipmentOrder> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ShipmentOrder> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
