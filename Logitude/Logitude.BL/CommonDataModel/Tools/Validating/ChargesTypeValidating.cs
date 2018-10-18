
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Linq;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class ChargesTypeValidating
    {
        public static void Validate(ChargesTypePM entityPM, ICommonDataContext myCommonContext, bool isNewEntity)
        {
            ValidateMultiVatPercentages(entityPM, myCommonContext, isNewEntity);
        }

        private static void ValidateMultiVatPercentages(ChargesTypePM entityPM, ICommonDataContext myCommonContext, bool isNewEntity)
        {
            //if (isNewEntity)
            //{
                if (!string.IsNullOrEmpty(entityPM.VatTypeId))
                {
                    AccountingSetting accountingSetting = (from a in myCommonContext.AccountingSettings
                                                           where a.Id == entityPM.Tenant
                                                           select a).FirstOrDefault();

                    if (accountingSetting != null)
                    {
                        if (!accountingSetting.EnableMultiPercentageVATTypes)
                        {
                            VatType vatType = (from f in myCommonContext.VatTypes
                                               where f.Id == entityPM.VatTypeId
                                               && f.Tenant == entityPM.Tenant
                                               select f).FirstOrDefault();

                            if (vatType.IsMultiPercentage)
                            {
                                throw new ApplicationException("Your accounting settings doesn't enable Multi-percentage VATs");
                            }
                        }
                    }
                }
            //}
        }
    }
}