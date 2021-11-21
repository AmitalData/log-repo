using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
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
                    entity.AccountingPaymentMethodCode = myAccountingPaymentMethodPM.Code;
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
            
            if(entity.BankAccountNumber != null)
            {
              entity.BankAccountId=  GetBankAccountIdByNumber(entity.BankAccountNumber, entity.Tenant);
            }
             if( entity.AccountingPaymentMethodCode =="BT" && entity.ARPaymentBankTranfers.Count > 0)
            {
                entity.BankAccountId = entity.ARPaymentBankTranfers.Where(d=> d.LineNumber ==1).Select(d => d.BankAccountId).First();
            }


                return entity;

        }


        public void CheckARPaymentNumber(string paymentNumber,string id, int tenant)
        {

            if(paymentNumber == null)
            {
                throw new ApplicationException("Payment number is null");
            }
           bool exist =  query.CheckARPaymentNumber(paymentNumber,id, tenant);
            if(exist)
            {
                throw new ApplicationException("Payment with the same number already exist");
            }
           



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
        private string GetBankAccountIdByNumber(string number, int tenant)
        {
            IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
            BankAccountPM bankAccount = bankAccountQuery.GetBankAccountByNumber(number, tenant);
            if (bankAccount != null)
            {
                return bankAccount.Id;
            }
            else
            {
                throw new ApplicationException("Bank account with number " + number + " doesn't exist");

            }
        }

        public ARPaymentPM MapAPPaymentChequeFieldsToARPayment(ARPayment payment , ARPaymentPM paymentPM)
        {

            if(payment.ARPaymentCheques.Count == 1)
            {
                ARPaymentCheque cheque = payment.ARPaymentCheques.First();

                paymentPM.Bank = cheque.Bank;
                paymentPM.Account = cheque.BankAccount;
                paymentPM.BankBranch = cheque.BankBranch;
                paymentPM.ValueDate = cheque.ValueDate;
                paymentPM.ChequeOrPaymentRef = cheque.ChequeNumber;


            }
            return paymentPM;

        }

        //public ARPaymentPM SetARPaymentInvoicesIds(ARPayment payment, ARPaymentPM paymentPM)
        //{
        //    if(payment.PaymentInvoices.Count >0)
        //    {
        //        foreach(ARPaymentInvoicePM paymentInvoice in payment.PaymentInvoices)
        //        {

        //            paymentPM.PaymentInvoices

        //        }
        //    }
        //}

    }
}
