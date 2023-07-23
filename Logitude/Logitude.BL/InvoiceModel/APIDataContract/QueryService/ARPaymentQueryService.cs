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
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Server.Tools.Helpers;
using User = Logitude.BL.CommonDataModel.APIDataContract.ApiV1.User;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
    public partial class ARPaymentQueryService
    {

        public ARPayment SetARPaymentSystemUser(ARPayment entity)
        {
            string systemUserId;
            UserPM myCreatedByUserPM = null;
            if (entity.CreatedByUser != null)
            {
                UserQuery query = new UserQuery(entity.Tenant);
                myCreatedByUserPM = query.UserCustomDataMappingAndValidatin(entity.CreatedByUser, entity.Tenant);
            }


            
            if (entity.CreatedByUser == null || myCreatedByUserPM == null)
            {
                systemUserId = AuthenticationUtil.ResolveSystemUserId(entity.Tenant);
                if (!String.IsNullOrEmpty(systemUserId))
                {
                    entity.CreatedByUser = null;

                    UserQueryService userQueryService = new UserQueryService(entity.Tenant);
                    User systemUser = userQueryService.GetUserById(systemUserId, entity.Tenant);
                    if (systemUser != null)
                    {
                        entity.CreatedByUser = new User();
                        entity.CreatedByUser.Id = systemUser.Id;
                        entity.CreatedByUser.EnglishName = systemUser.EnglishName;
                        entity.CreatedByUser.ExternalCode = systemUser.ExternalCode;
                        entity.CreatedByUser.LocalName = systemUser.LocalName;

                    }
                }
            }


            return entity;
        }


        public JournalPM GetARPaymentExistingJournalPM(ARPayment entity)
        {
            JournalPM journalPM = null;
            if (String.IsNullOrWhiteSpace(entity.ExistingExternalJournal) && entity.DoNotCreateJournal.HasValue && entity.DoNotCreateJournal.Value == true)
            {
                throw new ApplicationException("ExistingExternalJournal is required when DoNotCreateJournal==true");
            }

            if (!String.IsNullOrWhiteSpace(entity.ExistingExternalJournal) && !(entity.DoNotCreateJournal.HasValue && entity.DoNotCreateJournal.Value == true))
            {
                throw new ApplicationException("DoNotCreateJournal required to be true when ExistingExternalJournal is present");
            }

            if (!String.IsNullOrWhiteSpace(entity.ExistingExternalJournal) && entity.DoNotCreateJournal.HasValue && entity.DoNotCreateJournal.Value == true)
            {
                IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
                journalPM = GetSingleJournalByExternalNoAndExternalSystem(entity.ExistingExternalJournal, "AMITAL", entity.Tenant);

                if (journalPM == null)
                {
                    throw new ApplicationException("ExistingExternalJournal " + entity.ExistingExternalJournal + " from AMITAL is not found");
                }
            }
            return journalPM;
        }


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
                if (entity.AccountingPaymentMethodCode == "BT")
                {
                    entity.BankAccountId = GetBankAccountIdByDisplay(entity.BankAccountNumber, entity.Tenant);
                }
                else
                {
                    entity.BankAccountId = GetBankAccountIdByNumber(entity.BankAccountNumber, entity.Tenant);
                }
            }
             if( entity.AccountingPaymentMethodCode =="BT" && entity.ARPaymentBankTranfers?.Count > 0)
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

        public ARPaymentLite GetARPaymentLiteByNumber(string number, int Tenant)
        {
            try
            {
                string aRPaymentId = query.GetSinglePaymentIdByPaymentNumber(number, Tenant);

                return new ARPaymentLite() { Id = aRPaymentId };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public ARPaymentLite GetARPaymentLiteById(string paymentId, int tenant, string ComputingPartnerName = "")
        {
            try
            {
                string aRPaymentId = query.GetCheckPaymentId(paymentId, tenant);

                return new ARPaymentLite() { Id = aRPaymentId };
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        private JournalPM GetSingleJournalByExternalNoAndExternalSystem(string externalNo, string externalSystem, int tenant)
        {
            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            return journalQuery.GetSingleJournalByExternalNoAndExternalSystem(externalNo, externalSystem, tenant);


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

        private string GetBankAccountIdByDisplay(string number, int tenant)
        {
            IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
            BankAccountPM bankAccount = bankAccountQuery.GetBankAccountByDisplay(number, tenant);
            if (bankAccount != null)
            {
                return bankAccount.Id;
            }
            else
            {
                throw new ApplicationException("Bank account with display number " + number + " doesn't exist");

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
