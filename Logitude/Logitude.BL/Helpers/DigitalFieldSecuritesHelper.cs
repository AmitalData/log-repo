using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
using Newtonsoft.Json;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.Helpers
{
    public class DigitalFieldSecuritesHelper
    {
        public List<DigitalFeildSecurityObject> GitDigitalSecuritesFeilds(string objectTableId, string profileCode, int tenant, bool singleApi = true)
        {
            var digitalFieldSecurity = GetDigitalFieldSecurityQuery(0, objectTableId, profileCode);

            if (digitalFieldSecurity == null)
            {
                return new List<DigitalFeildSecurityObject>();
            }

            var defaultDigitalFieldSecurity = JsonConvert.DeserializeObject<List<DigitalFeildSecurityObject>>(digitalFieldSecurity?.DefaultSettings);
            var customDigitalFeildSecurityObject = new List<DigitalFeildSecurityObject>();
            var objectTableName = ObjectTableRepository.GetNameById(objectTableId, tenant);

            if (tenant != 0)
            {
                var customDigitalFieldSecurityList = GetDigitalFieldSecurityQuery(tenant, objectTableId, profileCode);

                if (customDigitalFieldSecurityList != null)
                {
                    customDigitalFeildSecurityObject = JsonConvert.DeserializeObject<List<DigitalFeildSecurityObject>>(customDigitalFieldSecurityList?.DefaultSettings);

                    foreach (var item in customDigitalFeildSecurityObject)
                    {
                        var temp = defaultDigitalFieldSecurity.FirstOrDefault(a => a.FieldCode.Equals(item.FieldCode, StringComparison.InvariantCultureIgnoreCase));

                        if (temp != null)
                        {
                            temp.HasPermission = item.HasPermission;
                            temp.CreatedBy = item.CreatedBy;
                            temp.CreatedOn = item.CreatedOn;
                            temp.ModifiedOn = item.ModifiedOn;
                            temp.ModifiedBy = item.ModifiedBy;
                            continue;
                        }
                        else
                        {
                            defaultDigitalFieldSecurity.Add(item);
                        }
                    }
                }
                else
                {
                    if (!singleApi)
                    {
                        defaultDigitalFieldSecurity = DiscardUnfoundFeildsFromList(defaultDigitalFieldSecurity);
                    }

                    return defaultDigitalFieldSecurity;
                }
            }

            if (!singleApi && objectTableName.Equals("Shipment", StringComparison.InvariantCultureIgnoreCase))
            {
                defaultDigitalFieldSecurity = DiscardUnfoundFeildsFromList(defaultDigitalFieldSecurity);
            }

            if (!customDigitalFeildSecurityObject.Any())
            {
                return defaultDigitalFieldSecurity;
            }

            return defaultDigitalFieldSecurity;
        }

        private List<DigitalFeildSecurityObject> DiscardUnfoundFeildsFromList(List<DigitalFeildSecurityObject> defaultDigitalFieldSecurity)
        {
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
                     "Shipment.Transshipment1CarrierName",
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

             return defaultDigitalFieldSecurity.Where(a => !unfoundFeilds.Contains(a.FieldCode)).ToList();
        }

        public DigitalFieldSecurityList GetDigitalFieldSecurityQuery(int tenant, string objectTableId, string profileCode)
        {
            DigitalFieldSecurityRepository digitalFieldSecurityRepository = new DigitalFieldSecurityRepository(tenant);
            var digitalFieldSecurity = digitalFieldSecurityRepository.GetDigitalFieldSecurity(tenant, objectTableId, profileCode)
                                                                     .Select(x => new DigitalFieldSecurityList
                                                                     {
                                                                         Id = x.Id,
                                                                         ObjectTableId = x.ObjectTableId,
                                                                         Tenant = x.Tenant,
                                                                         DefaultSettings = x.DefaultSettings,
                                                                         CreateDate = x.CreateDate,
                                                                         UpdateDate = x.UpdateDate,
                                                                         ProfileId = x.ProfileId
                                                                     })
                                                                     .FirstOrDefault();
            return digitalFieldSecurity;
        }


        public bool CheckIfFieldInuse(CheckObjectFieldExistenceRequest checkObjectFieldExistenceRequest, int tenant)
        {
            var helper = new DigitalFieldSecuritesHelper();
            var defaultDigitalFieldSecurity = helper.GitDigitalSecuritesFeilds(checkObjectFieldExistenceRequest.ObjectTableId,
                                                                               checkObjectFieldExistenceRequest.ProfileCode, tenant);

            var defaultDigitalFieldTenant0 = helper.GitDigitalSecuritesFeilds(checkObjectFieldExistenceRequest.ObjectTableId,
                                                                               checkObjectFieldExistenceRequest.ProfileCode, 0);

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
