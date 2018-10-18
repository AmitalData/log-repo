
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL.EntityPMs;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.Accounting.BL.EntityDataMappings
{

    public partial class ARPChequeLineDataMapping : IMapping<ARPChequeLinePM, ARPChequeLine>
    {

        public void CustomPMToPOCO(ARPChequeLinePM entityPM, ARPChequeLine entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(ARPChequeLinePM entityPM, ARPChequeLine entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.BankName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.BankNumber);
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

            if (entityPOCO.BankId != null)
            {
                BankCodeQueryService bankQueryService = new BankCodeQueryService(entityPOCO.Tenant);
                BankCodePM bank = bankQueryService.GetSingle(entityPOCO.BankId, false, true);
                if (bank != null)
                {
                    entityPM.BankNumber = bank.Code;
                    entityPM.BankName = bank.EnglishName;
                }
            }

            if (entityPOCO.PaymentId != null)
            {
                ARPaymentQuery paymentQueryService = new ARPaymentQuery(entityPOCO.Tenant);
                ARPaymentPM payment = paymentQueryService.GetSinglePM(entityPOCO.PaymentId, entityPOCO.Tenant);
                if (payment != null) entityPM.PaymentNumber = payment.PaymentNo;
            }

        }


    }

}
   