using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class TariffCarrierTranslationValidating
    {
        public static void Validate(TariffCarrierTranslationPM entityPM, ICommonDataContext iContext, bool isNewEntity)
        {
                bool exist = false;

                if (isNewEntity)
                {
                    exist = (from a in iContext.TariffCarrierTranslations
                             where a.PartnerCode.ToLower() == entityPM.PartnerCode.ToLower() 
                             && a.Tenant == entityPM.Tenant 
                             && a.CarrierId == entityPM.CarrierId
                             select a).Any();
                }

                else
                {
                    exist = (from a in iContext.TariffCarrierTranslations
                             where a.PartnerCode.ToLower() == entityPM.PartnerCode.ToLower()
                             && a.Id != entityPM.Id
                             && a.Tenant == entityPM.Tenant
                             && a.CarrierId == entityPM.CarrierId
                             select a).Any();
                }

                if (exist)
                {
                    throw new ApplicationException("Partner Code already exists for this carrier");
                }
        }
    }
}
