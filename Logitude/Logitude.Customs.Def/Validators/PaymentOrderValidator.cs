
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.Validators
{
   public class PaymentOrderValidator
    {

       public static ValidationResult IsAccountingCustomFileExist(
       PaymentOrderPM paymentOrderPM,
       System.ComponentModel.DataAnnotations.ValidationContext context)
       {
           bool valid = true;

           ICustomContext customContext = CustomContext.GetContext(paymentOrderPM.Tenant);
           //DeclarationQueryService declarationQuery = new DeclarationQueryService(customContext);
           string declarationId = GetIdByCustomFileNo(paymentOrderPM.AccountingCustomFile, paymentOrderPM.Tenant);

           if (declarationId == null && paymentOrderPM.PaymentOrderSelectedLabel == "AccountingCustomFile")
           {
                Unifreight.Data.AmitalModel.AmitalContext amitalContext = Unifreight.Data.AmitalModel.AmitalContext.GetContext(paymentOrderPM.Tenant);
                var myCCUFILEMQueryService = new Unifreight.BL.EntityQueryServices.CCUFILEMQueryService(amitalContext);
                long lCUSTOMFILENO;
                if (long.TryParse(paymentOrderPM.AccountingCustomFile, out lCUSTOMFILENO))
                {
                    int? FILENO = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);
                    if (!FILENO.HasValue)
                    {
                        valid = false;
                        return new ValidationResult(TranslateTextsClass.Translate("Customs.PaymentOrder.O.FileNumberDoesNotExist", paymentOrderPM.Tenant, true) + " " + paymentOrderPM.AccountingCustomFile);
                    }
                }
           }

           return null;
       }

        public static string GetIdByCustomFileNo(string customFileNo, int tenant)
        {
            if (String.IsNullOrWhiteSpace(customFileNo)) return "";
            DeclarationRepository repository = new DeclarationRepository(tenant);
            return repository.GetIdByCustomFileNo(customFileNo, tenant);
        }

    }
}
