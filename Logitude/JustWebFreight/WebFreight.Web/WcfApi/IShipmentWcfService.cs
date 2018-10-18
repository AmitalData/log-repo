using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebFreight.Web.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IShipmentHypredService" in both code and config file together.
    [ServiceContract]
    public interface IShipmentWcfService
    {
        [OperationContract]
        Response Upsert(ShipmentPM entityPM, bool batch);

        [OperationContract]
        Response Cancel(string shipmentNumber, int tenant);

        [OperationContract]
        Response Delete(string shipmentNumber, int tenant);

        [OperationContract]
        Response CreateEvent(int tenant, string externalId, string shipmentNumber, string userId, string eventTypeCode, DateTime logDate, DateTime eventDate, string notes);

        [OperationContract]
        Response BuildEventsList(int tenant, string shipmentNumber, List<TraceEventPM> eventsList);

        [OperationContract]
        List<ShipmentList> GetShipmentList(ShipmentApiFilters filters, int tenant, ref Response response);

        [OperationContract]
        Response DeleteShipmentEvent(string shipmentNumber, string traceEventId, int tenant);
    }
}
