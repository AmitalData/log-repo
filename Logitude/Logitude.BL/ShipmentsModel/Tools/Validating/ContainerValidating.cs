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
            ValidateShipmentConcurrencyGUID(entityPM, entityPoco);
        }

        private static void ValidateConcurrencyGUID(ContainerPM entityPM, Container entityPoco)
        {
            if (entityPM.IsUpdatedOceanInsightsAnalyzer == true)
                return;
            if (entityPM.IsUpdatedVizionAnalyzer == true)
                return;
            if (entityPM.IsShipmentBatchUpdate == true)
                return;

            if (string.IsNullOrEmpty( entityPM.ConcurrencyGUID) || string.IsNullOrEmpty(entityPM.NewConcurrencyGUID))
            {
                return;
            }
            if (!entityPM.ConcurrencyGUID.Equals(entityPoco.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPoco.ConcurrencyGUID))
            {
                ThrowConcurrencyException(entityPM, entityPoco.UpdatedByPartner);
            }
        }
        private static void ValidateShipmentConcurrencyGUID(ContainerPM entityPM, Container entityPoco)
        {
            if (entityPM.IsUpdatedOceanInsightsAnalyzer == true)
                return;
            if (entityPM.IsUpdatedVizionAnalyzer == true)
                return;

            var shipmentConcurrencyGUID = GetShipmentConcurrencyGUID(entityPM.ShipmentId, entityPM.Tenant);
            if (string.IsNullOrEmpty(entityPM.ShipmentConcurrencyGUID) || string.IsNullOrEmpty(entityPM.ShipmentNewConcurrencyGUID) || string.IsNullOrEmpty(shipmentConcurrencyGUID))
            {
                return;
            }
            if (!entityPM.ShipmentConcurrencyGUID.Equals(shipmentConcurrencyGUID) && !entityPM.ShipmentNewConcurrencyGUID.Equals(shipmentConcurrencyGUID))
            {
                ThrowConcurrencyException(entityPM, entityPoco.UpdatedByPartner);
            }
        }

        private static string GetShipmentConcurrencyGUID(string shipmentId, int tenant)
        {
            var containerRepository = new ContainerRepository(tenant);
            string shipmentConcurrencyGUID = containerRepository.GetConcurrencyGUIDByShipmentId(shipmentId, tenant);
            return shipmentConcurrencyGUID;
        }

        private static void ThrowConcurrencyException(ContainerPM entityPM, string updatedByPartner)
        {
            string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
            if (updatedByPartner != null)
            {
                msg = msg.Replace("another user", updatedByPartner);
            }
            throw new OptimisticConcurrencyException(msg);
        }
    }
}
