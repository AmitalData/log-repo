
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Linq;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class VatTypeValidating
    {
        public static void Validate(VatTypePM entityPM, ICommonDataContext myCommonContext, bool isNewEntity)
        {
            ValidateMultiVatPercentages(entityPM, myCommonContext, isNewEntity);
        }

        private static void ValidateMultiVatPercentages(VatTypePM entityPM, ICommonDataContext myCommonContext, bool isNewEntity)
        {
            if (isNewEntity)
            {
                if (entityPM.IsMultiPercentage)
                {
                    AccountingSetting accountingSetting = (from a in myCommonContext.AccountingSettings
                                                           where a.Id == entityPM.Tenant
                                                           select a).FirstOrDefault();

                    if (accountingSetting != null)
                    {
                        if (!accountingSetting.EnableMultiPercentageVATTypes)
                        {
                            throw new ApplicationException("Your accounting settings doesn't enable Multi-percentage VATs");
                        }
                    }

                    if (entityPM.VatTypeGroups.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).Count() == 0)
                    {
                        throw new ApplicationException("Multi-percentage VAT must have atleast 1 VAT percentage");
                    }
                }
            }
        }
    }
}