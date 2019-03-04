using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
    public partial class ARPaymentQueryService
    {
        public ARPaymentPM SetARPaymentPMFields(ARPaymentPM entity)
        {
            CurrencyQuery currencyQuery = new CurrencyQuery(entity.Tenant);
            if (entity.LocalCurrencyCode != null)
            {
                CurrencyPM currency = currencyQuery.GetSingleCurrencyByCode(entity.LocalCurrencyCode, entity.Tenant);
                if (currency != null)
                    entity.LocalCurrencyId = currency.Id;

            }
            AddressQuery addressQuery = new AddressQuery(entity.Tenant);
            List<AddressPM> addresses = addressQuery.GetAddressesByCardId(entity.BillToId, entity.Tenant);

            var address = addresses.Where(d => d.AddressTypeId == "B").FirstOrDefault();

            if(address != null)
            {
                entity.BillToAddressId = address.Id;
            }
            else
            {
                address = addresses.Where(d => d.AddressTypeId == "M").FirstOrDefault();
                entity.BillToAddressId = address.Id;
            }

            CardQuery cardQuery = new CardQuery(entity.Tenant);
            CardPM card = cardQuery.GetSinglePM(entity.BillToId, entity.Tenant);
            if(card != null)
            {
                entity.GLAccountId = card.GLAccountId;
            }
            else
            {
                throw new ApplicationException("Bill to field  is required");
            }

            AccountingPaymentMethodQueryService AccountingPaymentMethodAccountingPaymentMethodService = new AccountingPaymentMethodQueryService(entity.Tenant);
            if (entity.AccountingPaymentMethodId != null)
            {
                var myAccountingPaymentMethodPM = AccountingPaymentMethodAccountingPaymentMethodService.GetAccountingPaymentMethodById(entity.AccountingPaymentMethodId, entity.Tenant);
                if (myAccountingPaymentMethodPM != null)
                {
                    entity.AccountingPaymentMethodCode = myAccountingPaymentMethodPM.LogitudeCode;
                }

            }
            else
            {
                throw new ApplicationException("Accounting Payment Method field  is required");
            }

            if (entity.PaymentCurrencyId == null)
            {
                throw new ApplicationException("Payment currency field  is required");
            }

                return entity;

        }


        public ARPayment GetARPaymentByNumber(string number, int Tenant)
        {
            try
            {


                var temp = query.GetSinglePaymentByPaymentNumber_00(number, Tenant);
                if (temp == null)
                    throw new ApplicationException("ARPayment with number " + number + " doesn't exist");

                return ARPaymentDataMapping(temp, Tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
