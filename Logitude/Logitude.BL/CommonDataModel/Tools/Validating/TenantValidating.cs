
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using System;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class TenantValidating
    {
        public static void Validate(TenantPM entityPM)
        {
            ValidateRatioFields(entityPM);
        }

        private static void ValidateRatioFields(TenantPM entityPM)
        {
            ValidateRatio(entityPM.AirRatio, "AirRatio", entityPM.Id);
            ValidateRatio(entityPM.FCLRatio, "FCLRatio", entityPM.Id);
            ValidateRatio(entityPM.LCLRatio, "LCLRatio", entityPM.Id);
            ValidateRatio(entityPM.FTLRatio, "FTLRatio", entityPM.Id);
            ValidateRatio(entityPM.LTLRatio, "LTLRatio", entityPM.Id);
        }

        private static void ValidateRatio(double? value, string fieldName, int tenant)
        {
            if (value == null)
            {
                string field = TranslateTextsClass.Translate("Tenant.F." + fieldName, tenant);
                throw new ApplicationException(field + " field is required");
            }

            if (value < 1 || value > 10)
            {
                string field = TranslateTextsClass.Translate("Tenant.F." + fieldName, tenant);
                throw new ApplicationException(field + " must be between 1-10");
            }
        }
    }
}