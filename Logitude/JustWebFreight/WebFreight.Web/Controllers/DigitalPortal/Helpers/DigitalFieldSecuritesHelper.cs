using Logitude.Infrastructure.BL.EntityQueryServices;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure.DataContracts.Models;
using System;
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
                "Shipment.IncotermName",
                "Shipment.WarehouseLegExpectedEntryDate",
                "Shipment.WarehouseLegExpectedReleaseDate",
                "Shipment.WarehouseLegActualReleaseDate",
                "Shipment.WarehouseLegCutOffDate",
                "Shipment.PreCarriageFromPortId",
                "Shipment.PreCarriageFromPortName",
                "Shipment.PreCarriageATD",
                "Shipment.PreCarriageETD",
                "Shipment.PreCarriageFromPortCountryCode",
                "Shipment.PreCarriageATA",
                "Shipment.PreCarriageETA",
                "Shipment.PreCarriageTransportModeId",
                "Shipment.PreCarriageCarrierName",
                "Shipment.PreCarriageCarrierNumber",
                "Shipment.PreCarriageVesselName",
                "Shipment.MainCarriageFromPortName",
                "Shipment.FromPartnerCountryCode",
                "Shipment.MainCarriageFromPortCountryCode",
                "Shipment.MainCarriageATA",
                "Shipment.AirlinePrefix",
                "Shipment.MainCarriageCarrierName",
                "Shipment.MainCarriageCarrierNumber",
                "Shipment.MainCarriageVesselName",
                "Shipment.Transshipment1FromPortId",
                "Shipment.Transshipment1FromPortName",
                "Shipment.Transshipment1FromPortCountryCode",
                "Shipment.Transshipment2FromPortName",
                "Shipment.Transshipment2FromPortCountryCode",
                "Shipment.Transshipment3FromPortName",
                "Shipment.Transshipment3FromPortCountryCode",
                "Shipment.Transshipment1AdditionalMAWBOBLBL",
                "Shipment.Transshipment1CarrierNumber",
                "Shipment.Transshipment1VesselName",
                "Shipment.Transshipment2AdditionalMAWBOBLBL",
                "Shipment.Transshipment2CarrierName",
                "Shipment.Transshipment2CarrierNumber",
                "Shipment.Transshipment2VesselName",
                "Shipment.Transshipment3AdditionalMAWBOBLBL",
                "Shipment.Transshipment3CarrierName",
                "Shipment.Transshipment3CarrierNumber",
                "Shipment.Transshipment3VesselName",
                "Shipment.MainCarriageFinalDestinationPortName",
                "Shipment.ToPartnerCountryCode",
                "Shipment.MainCarriageFinalDestinationPortCountryCode",
                "Shipment.Transshipment3ToPortId",
                "Shipment.Transshipment1ToPortId",
                "Shipment.Transshipment2ToPortId",
                "Shipment.MainCarriageToPortId",
                "Shipment.AirlinePrefix",
                "Shipment.Master",
                "Shipment.OnCarriageFromPortName",
                "Shipment.OnCarriageFromPortCountryCode",
                "Shipment.OnCarriageTransportModeId",
                "Shipment.OnCarriageCarrierName",
                "Shipment.OnCarriageCarrierNumber",
                "Shipment.OnCarriageVesselName",
            };

            if (tenant != 0)
            {
                var customDigitalFieldSecurityList = digitalFieldSecurityQuery.GetDigitalFieldSecurityQuery(tenant, objectTableId, profileId);

                if (customDigitalFieldSecurityList != null)
                {
                    customDigitalFeildSecurityObject = JsonConvert.DeserializeObject<List<DigitalFeildSecurityObject>>(customDigitalFieldSecurityList.DefaultSettings);

                    foreach (var item in defaultDigitalFieldSecurity)
                    {
                        var temp = customDigitalFeildSecurityObject.FirstOrDefault(a => a.FieldCode.Equals(item.FieldCode));

                        if (temp != null)
                        {
                            continue;
                        }
                        else
                        {
                            customDigitalFeildSecurityObject.Add(item);
                        }
                    }
                }
            }

            if (!singleApi)
            {
                var extraFieldsForShipmentListApi  = new List<string>
                {
                    "Field1",
                    "Field2",
                    "Field3",
                    "Field4",
                    "Field5",
                    "Field6",
                    "Field7",
                    "Field8"
                };

                defaultDigitalFieldSecurity = defaultDigitalFieldSecurity.Where(a => !unfoundFeilds.Contains(a.FieldCode)).ToList();

                foreach (var item in extraFieldsForShipmentListApi)
                {
                    defaultDigitalFieldSecurity.Add(new DigitalFeildSecurityObject
                    {
                        FieldCode = item,
                        HasPermission = true
                    });
                }
            }

            if (!customDigitalFeildSecurityObject.Any())
            {
                return defaultDigitalFieldSecurity;
            }
            
            return customDigitalFeildSecurityObject;
        }
    
        public bool CheckIfFieldInuse(CheckObjectFieldExistenceRequest checkObjectFieldExistenceRequest, int tenant)
        {
            var helper = new DigitalFieldSecuritesHelper();
            var defaultDigitalFieldSecurity = helper.GitDigitalSecuritesFeilds(checkObjectFieldExistenceRequest.ObjectTableId,
                                                                               checkObjectFieldExistenceRequest.ProfileId, tenant);

            var defaultDigitalFieldTenant0 = helper.GitDigitalSecuritesFeilds(checkObjectFieldExistenceRequest.ObjectTableId,
                                                                               checkObjectFieldExistenceRequest.ProfileId, 0);

            var res = false;

            if (defaultDigitalFieldSecurity.Any(a => a.FieldCode.Equals(checkObjectFieldExistenceRequest.FieldCode, StringComparison.InvariantCultureIgnoreCase))
                || defaultDigitalFieldTenant0.Any(a => a.FieldCode.Equals(checkObjectFieldExistenceRequest.FieldCode, StringComparison.InvariantCultureIgnoreCase)))
            {
                res = true;
            }

            return res;
        }
    }
}