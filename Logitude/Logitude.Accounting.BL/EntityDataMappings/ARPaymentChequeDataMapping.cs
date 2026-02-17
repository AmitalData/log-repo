
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class ARPaymentChequeDataMapping: IMapping<ARPaymentChequePM, ARPaymentCheque>
   {

        public void CustomPMToPOCO(ARPaymentChequePM entityPM, ARPaymentCheque entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(ARPaymentChequePM entityPM, ARPaymentCheque entityPOCO)
        {

            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.BankName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.PaymentNumber);
          
          
            if (entityPOCO.CurrencyId != null)
            {
                CurrencyQuery currencyQueryService = new CurrencyQuery(entityPOCO.Tenant);
                CurrencyPM currency = currencyQueryService.GetSinglePM(entityPOCO.CurrencyId, entityPOCO.Tenant);
                if (currency != null)
                {
                    entityPM.CurrencyName = currency.EnglishName;
                    entityPM.CurrencyCode = currency.Code;
                }
            }
            if (entityPOCO.PaymentId != null )
            {
                ARPaymentQuery aRPaymentQueryService = new ARPaymentQuery(entityPOCO.Tenant);
                ARPaymentPM aRPayment = aRPaymentQueryService.GetSinglePM(entityPOCO.PaymentId, entityPOCO.Tenant);
                if (aRPayment != null )
                {
                    entityPM.PaymentNumber = aRPayment.PaymentNo;
                }
            }
            if (entityPOCO.BankId != null)
            {
                BankCodeQueryService bankCodeQueryService = new BankCodeQueryService(entityPOCO.Tenant);
                BankCodePM bankCode = bankCodeQueryService.GetSingle(entityPOCO.BankId, false, true);
                if (bankCode != null)
                {
                    entityPM.BankName = bankCode.EnglishName;
                }
            }



        }

        private void BuildSearchFields(ARPaymentChequePM entityPM, ARPaymentCheque entityPOCO, bool p)
        {
            string searchFields = "";

            if (!string.IsNullOrEmpty(entityPM.ChequeNumber))
            {
                MethodHelper.AddToSearchFields(ref searchFields, entityPM.ChequeNumber);
            }

            if (!string.IsNullOrEmpty(entityPM.BankAccount))
            {
                MethodHelper.AddToSearchFields(ref searchFields, entityPM.BankAccount);
            }

            if (searchFields.Length > 1000)
            {
                searchFields = searchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = searchFields;
            entityPOCO.SearchFields = searchFields;
        }
   }

}
   