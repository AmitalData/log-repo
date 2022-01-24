using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;                        
using Simplog.Data.CommonDataModel.EntityPOCOs;            
using Simplog.Data.CommonDataModel.Repositories;           
using System;                                              
using System.Collections.Generic;                          
using System.Linq;                                         
using System.Web;                                          

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class AirlineValidating
    {
        public static void Validate(EntityPMs.AirlinePM entityPM,Card Card, ICommonDataContext myContext, bool isNewEntity)
        {
            ValidateVatNumber(entityPM);

            TenantRepository tenantRepository = new TenantRepository(myContext);
            Tenant myTenant = tenantRepository.GetSingleTenantOnly(entityPM.Tenant);
            CardValidating.ValidateCode_Unique(Card, myContext);

            if (myTenant.ApplyVATForAllPartners)
            {
                if (Card != null)
                {
                    VATValidating.ValidateVAT_Required(Card, myTenant);
                    VATValidating.ValidateVAT_Unique(Card, myContext, myTenant, isNewEntity);
                    VATValidating.ValidateVAT_Format(Card, myContext, myTenant);
                }
            }

        }

        private static void ValidateVatNumber(AirlinePM entityPM)
        {
            bool isAccountingActivated = CheckFullAccountingActivated(entityPM.Tenant);

            if (isAccountingActivated && !string.IsNullOrEmpty(entityPM.VatNumber))
            {
                var isValid = LuhnAlgorithm.IsVatNumberValid(entityPM.VatNumber);
                if (!isValid)
                    throw new ApplicationException(TranslateTextsClass.Translate("General.O.WrongVatNumber", entityPM.Tenant));
            }
            else if (isAccountingActivated && string.IsNullOrEmpty(entityPM.VatNumber))
            {
                var message = TranslateTextsClass.Translate("General.M.FieldIsRequired", entityPM.Tenant);
                message = message.Replace("%FieldName", TranslateTextsClass.Translate("ARInvoice.F.VatNumber", entityPM.Tenant));
                throw new ApplicationException(message);
            }
        }

        private static bool CheckFullAccountingActivated(int tenantNumber)
        {
            var tenant = TenantQuery.GetSingleTenantPM(tenantNumber);
            var isAccountingActivated = tenant.AccountingActivated;
            return isAccountingActivated;
        }
    }
}