using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.Helpers.AutomationModel.SendInterface
{
    class ShipmentAPISendInterfaceDataContract : SendInterfaceDataContract,ISendInterfaceDataContract
    {
        public ShipmentPM EntityPM;
        public ShipmentAPISendInterfaceDataContract(byte[] objectData)
        {
            EntityPM = LogitudeXmlSerializer.DeserializeObject<ShipmentPM>(objectData);
        }

        public string GetFileName(SendInterfaceDataContractFileNameArgs sendInterfaceDataContractFileNameArgs)
        {
            int tenant = sendInterfaceDataContractFileNameArgs.Tenant;

            string shipmentNumber = EntityPM.ShipmentNumber;
            string transportModeId = EntityPM.TransportModeId;
            string directionId = EntityPM.DirectionId;
            string calculatedFileName = GetShipmentLevelName(EntityPM, tenant).ToLower() + "_" + transportModeId?.ToLower() + directionId?.ToLower() + "_" + shipmentNumber?.ToLower();
            
            return calculatedFileName;
        }

        public object GetObject(SendInterfaceDataContractObjectArgs sendInterfaceDataContractObjectArgs)
        {
            if (IsShouldBeRemoveEventList(sendInterfaceDataContractObjectArgs.AutomationSendInterface))
            {
                SetPropertyValueToEntity("EventList", EntityPM, null);
            }

            string shipmentLevelName = GetShipmentLevelName(EntityPM, sendInterfaceDataContractObjectArgs.Tenant);
            string instanceTypePath = "Logitude.BL.ShipmentsModel.APIDataContract.ApiV1." + shipmentLevelName + "QueryService";
            Type instanceAssemblyType = GetInstanceAssemblyType("Logitude.BL", instanceTypePath);
            if (instanceAssemblyType == null) { throw new ApplicationException(shipmentLevelName + "QueryService Not Found!"); }

            string computingPartnerCode = GetComputingPartnerCode(sendInterfaceDataContractObjectArgs.Tenant, sendInterfaceDataContractObjectArgs.AutomationSendInterface.ComputingPartnerId);
            object instanceQueryService = Activator.CreateInstance(instanceAssemblyType, new object[] { sendInterfaceDataContractObjectArgs.Tenant });
            object sendInterfaceDataContractObject = GetMethodValue(instanceQueryService, (shipmentLevelName + "DataMapping"), new object[] { EntityPM, sendInterfaceDataContractObjectArgs.Tenant, computingPartnerCode });
            
            return sendInterfaceDataContractObject;
        }

        private bool IsShouldBeRemoveEventList(AutomationSendInterface automationSendInterface)
        {
            if (automationSendInterface.AdvancedAutomationSendInterfaceDetails != null)
                return !automationSendInterface.AdvancedAutomationSendInterfaceDetails.IncludeEvents;
            return true;
        }

        private void SetPropertyValueToEntity(string fieldName, object entity, object fieldValue)
        {
            PropertyInfo propInfo = entity.GetType().GetProperty(fieldName);
            if (propInfo != null) propInfo.SetValue(entity, fieldValue, null);
        }

        public string GetComputingPartnerCode(int tenant, string computingPartnerId)
        {
            ComputingPartnerRepository computingPartnerRepository = new ComputingPartnerRepository(tenant);
            string computingPartnerCode = computingPartnerRepository.GetSingleComputingPartnerCodeById(computingPartnerId);
            return computingPartnerCode;
        }

        private string GetShipmentLevelName(object entityPM, int tenant)
        {
            ShipmentLevelRepository shipmentLevelRepository = new ShipmentLevelRepository(tenant);
            string shipmentLevelCode = GetPropertyValueFromObject("ShipmentLevelCode", entityPM);
            string shipmentLevelName = shipmentLevelRepository.GetSingleShipmentLevelNameByCode(shipmentLevelCode);
            return (shipmentLevelName == "Consol" ? "Master" : shipmentLevelName);
        }
    }
}
