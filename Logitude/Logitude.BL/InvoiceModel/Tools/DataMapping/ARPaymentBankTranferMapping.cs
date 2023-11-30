using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
   public class ARPaymentBankTranferMapping
    {
        public static void MapEntity(ARPaymentBankTranferPM entityPM, ARPaymentBankTranfer poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Tenant = entityPM.Tenant;
                poco.Id = entityPM.Id;
                poco.LineNumber = entityPM.LineNumber;
            }
            poco.BankAccountId = entityPM.BankAccountId;
            poco.CurrencyId = entityPM.CurrencyId;
            poco.ExchageRate  = entityPM.ExchageRate;            
            poco.ForeignAmount = entityPM.ForeignAmount;        
            poco.LocalAmount = entityPM.LocalAmount;
            poco.PaymentId = entityPM.PaymentId;
            poco.ValueDate = entityPM.ValueDate;
            poco.PaymentRef = entityPM.PaymentRef;
            BuildSearchFields(entityPM, poco, isNewEntity);
        }

        private static void  BuildSearchFields(ARPaymentBankTranferPM entityPM, ARPaymentBankTranfer poco, bool isNewEntity)
        {
            string searchFields = "";

            if (!string.IsNullOrEmpty(entityPM.PaymentRef))
            {
                MethodHelper.AddToSearchFields(ref searchFields, entityPM.PaymentRef);
            }

            if (!string.IsNullOrEmpty(entityPM.LocalAmount.ToString()))
            {
                MethodHelper.AddToSearchFields(ref searchFields, entityPM.LocalAmount.ToString());
            }

            if (searchFields.Length > 1000)
            {
                searchFields = searchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = searchFields;
            poco.SearchFields = searchFields;
        }
    }
}
