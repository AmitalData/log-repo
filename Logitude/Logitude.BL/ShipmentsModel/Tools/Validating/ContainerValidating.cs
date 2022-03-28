using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System.Data.Entity.Core;

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class ContainerValidating
    {
        public static void Validate(ContainerPM entityPM, Container entityPoco, bool isNewEntity)
        {
            if (!isNewEntity)
            {
                ValidateConcurrencyGUID(entityPM, entityPoco);
            }
            ValidateShipmentConcurrencyGUID(entityPM);
        }

        private static void ValidateConcurrencyGUID(ContainerPM entityPM, Container entityPoco)
        {
            if(string.IsNullOrEmpty( entityPM.ConcurrencyGUID) || string.IsNullOrEmpty(entityPM.NewConcurrencyGUID))
            {
                return;
            }
            if (!entityPM.ConcurrencyGUID.Equals(entityPoco.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPoco.ConcurrencyGUID))
            {
                ThrowConcurrencyException(entityPM);
            }
        }
        private static void ValidateShipmentConcurrencyGUID(ContainerPM entityPM)
        {
            var shipmentConcurrencyGUID = GetShipmentConcurrencyGUID(entityPM.ShipmentId, entityPM.Tenant);
            if (!string.IsNullOrEmpty(entityPM.ShipmentConcurrencyGUID) && !string.IsNullOrEmpty(entityPM.ShipmentNewConcurrencyGUID) && !string.IsNullOrEmpty(shipmentConcurrencyGUID))
            {
                if (!entityPM.ShipmentConcurrencyGUID.Equals(shipmentConcurrencyGUID) && !entityPM.ShipmentNewConcurrencyGUID.Equals(shipmentConcurrencyGUID))
                {
                    ThrowConcurrencyException(entityPM);
                }
            }
        }

        private static string GetShipmentConcurrencyGUID(string shipmentId, int tenant)
        {
            var containerRepository = new ContainerRepository(tenant);
            string shipmentConcurrencyGUID = containerRepository.GetConcurrencyGUIDByShipmentId(shipmentId, tenant);
            return shipmentConcurrencyGUID;
        }

        private static void ThrowConcurrencyException(ContainerPM entityPM)
        {
            string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
            throw new OptimisticConcurrencyException(msg);
        }
    }
}
