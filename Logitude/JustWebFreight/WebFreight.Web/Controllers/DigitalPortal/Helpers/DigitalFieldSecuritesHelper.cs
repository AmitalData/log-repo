using Logitude.Infrastructure.BL.EntityQueryServices;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure.DataContracts.Models;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.Controllers.DigitalPortal.Helpers
{
    public class DigitalFieldSecuritesHelper
    {
        public List<DigitalFeildSecurityObject> GitDigitalSecuritesFeilds(string objectTableId, string profileId, int tenant, bool singleApi = true)
        {
            var digitalFieldSecurityQuery = new DigitalFieldSecurityQueryService(tenant);
            var digitalFieldSecurity = digitalFieldSecurityQuery.GetDigitalFieldSecurityQuery(0, objectTableId, profileId);
            var defaultDigitalFieldSecurity = JsonConvert.DeserializeObject<List<DigitalFeildSecurityObject>>(digitalFieldSecurity.DefaultSettings);
            var customDigitalFeildSecurityObject = new List<DigitalFeildSecurityObject>();

            var unfoundFeilds = new List<string>
            {
                "Shipment.IsFullInvoiced",
                "Shipment.ContainerNumber",
                "Shipment.CarrierLastStatusName",
                "Shipment.Reference",
                "Shipment.NumberOfContainers",
                "Shipment.Routing",
                "Shipment.FlightDate",
                "Shipment.IssuingCarrierAgentId",
                "Shipment.Notify1Id",
                "Shipment.Notify2Id",
                "Shipment.CustomClearancePointId",
                "Shipment.ReleasingAgentId",
                "Shipment.ColoaderId",
                "Shipment.FreightForwarderId",
                "Shipment.TruckerId",
                "Shipment.Events",
                "Shipment.ConsolidatorName",
                "Shipment.WarehouseLegRemarks",
                "Shipment.IncotermName"
            };

            if (tenant != 0)
            {
                var customDigitalFieldSecurityList = digitalFieldSecurityQuery.GetDigitalFieldSecurityQuery(tenant, objectTableId, profileId);

                if (customDigitalFieldSecurityList != null)
                {
                    customDigitalFeildSecurityObject = JsonConvert.DeserializeObject<List<DigitalFeildSecurityObject>>(customDigitalFieldSecurityList.DefaultSettings);

                    foreach (var item in customDigitalFeildSecurityObject)
                    {
                        var temp = defaultDigitalFieldSecurity.FirstOrDefault(a => a.FieldCode.Equals(item.FieldCode));

                        if (temp != null)
                        {
                            temp.HasPersmission = true;
                        }
                    }
                }
            }

            if (!singleApi)
            {
                var extraFieldsForShipmentListApi  = new List<string>
                {
                    "Id",
                    "MainCarriageETD",
                    "MainCarriageFinalDestinationATA",
                    "MainCarriageFinalDestinationETA",
                    "InlandDomesticFromTypeCode",
                    "MainCarriageFromAddressId",
                    "MainCarriageFromPortCode",
                    "InlandDomesticFromCity",
                    "InlandDomesticToTypeCode",
                    "MainCarriageToAddressId",
                    "MainCarriageToPortCode",
                    "InlandDomesticToCity",
                    "InlandDomesticFromCountryId",
                    "InlandDomesticToCountryId",
                    "MainCarriageFromCity",
                    "FromPortName",
                    "FromCountryCode",
                    "Transshipment1ETA",
                    "Transshipment1ATA",
                    "Transshipment1ETD",
                    "Transshipment1ATD",
                    "Transshipment2ETA",
                    "Transshipment2ATA",
                    "Transshipment2ETD",
                    "Transshipment2ATD",
                    "Transshipment3ETA",
                    "Transshipment3ATA",
                    "Transshipment3ETD",
                    "Transshipment3ATD",
                    "MainCarriageToCity",
                    "ToPortName",
                    "ToCountryCode",
                    "IsOperationalClosed",
                    "IsCustomerArchived",
                    "IsAccountingClosed",
                    "ShipmentLevelCode",
                    "StatusLocation",
                    "StatusCode",
                    "ShipperCountryCode",
                    "ConsigneeCountryCode"
                };

                defaultDigitalFieldSecurity = defaultDigitalFieldSecurity.Where(a => !unfoundFeilds.Contains(a.FieldCode)).ToList();

                foreach (var item in extraFieldsForShipmentListApi)
                {
                    defaultDigitalFieldSecurity.Add(new DigitalFeildSecurityObject
                    {
                        FieldCode = item,
                        HasPersmission = true
                    });
                }
            }

            if (!customDigitalFeildSecurityObject.Any())
            {
                defaultDigitalFieldSecurity.ForEach(a => a.HasPersmission = true);
            }

            return defaultDigitalFieldSecurity;
        }
    }
}