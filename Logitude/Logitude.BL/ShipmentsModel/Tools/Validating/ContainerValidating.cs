using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
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

        private static void ThrowConcurrencyException(ContainerPM entityPM)
        {
            string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
            throw new OptimisticConcurrencyException(msg);
        }
    }
}
