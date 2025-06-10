using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class ARPaymentChequeQueryService
    {


        public List<ARPaymentChequePM> GetARPaymentChequesByPaymentIds(List<string> paymentIds, int tenant)
        {

           

            List<ARPaymentCheque> paymentCheques = (from a in context.ARPaymentCheques
                                                    where paymentIds.Contains(a.PaymentId) && a.Tenant == tenant && a.StatusCode != "6" && a.StatusCode != "5"
                                                    select a).ToList();

            return (from a in paymentCheques
                    select new ARPaymentChequePM()
                    {
                        Id =a.Id,
                        Tenant = a.Tenant,
                        StatusCode = a.StatusCode ,
                        LocalAmount = a.LocalAmount ,
                        ValueDate = a.ValueDate,
                        ChequeNumber = a.ChequeNumber,
                        PaymentId = a.PaymentId,
                        CurrencyId = a.CurrencyId,

                    }).ToList();


        }


        public List<ARPaymentChequePM> GetARPaymentChequesBySinglePaymentId(string paymentId, int tenant)
        {



            List<ARPaymentCheque> paymentCheques = (from a in context.ARPaymentCheques
                                                    where paymentId == a.PaymentId && a.Tenant == tenant
                                                    select a).ToList();

            return (from a in paymentCheques
                    select new ARPaymentChequePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        StatusCode = a.StatusCode,
                        LocalAmount = a.LocalAmount,
                        ValueDate = a.ValueDate,
                        ChequeNumber = a.ChequeNumber,
                        PaymentId = a.PaymentId,
                        BankAccount = a.BankAccount,
                        BankBranch = a.BankBranch 
                    }).ToList();


        }



        public int GetARPaymentChequesCountWithValueDateGreaterThanARPaymentRegisterDate(List<string> paymentIds, int tenant)
        {
            List<ARPaymentCheque> paymentCheques = (from a in context.ARPaymentCheques.Include("Payment")
                                                    where paymentIds.Contains(a.PaymentId) 
                                                    && a.ValueDate > a.Payment.RegisterDate
                                                    && a.Tenant == tenant 
                                                    && a.StatusCode != PaymentChequeStatuses.Redeemed.ToString() && a.StatusCode != PaymentChequeStatuses.ReturnedToCustomer.ToString()
                                                    select a).ToList();
            return paymentCheques.Count();
        }


        public List<ARPaymentChequePM> GetAllARPaymentChequesByPaymentIds(List<string> paymentIds, int tenant)
        {



            List<ARPaymentCheque> paymentCheques = (from a in context.ARPaymentCheques
                                                    where paymentIds.Contains(a.PaymentId) && a.Tenant == tenant
                                                    select a).ToList();

            return paymentCheques.Select(r => this.GetEntityPM(r)).ToList();


        }

      

        public List<ARPaymentChequePM>  GetListByPaymentId(string paymentId, int tenant)
        {
            List<ARPaymentChequePM> paymentCheques = (from a in context.ARPaymentCheques
                                                      where a.PaymentId == paymentId && a.Tenant == tenant
                                                      select new ARPaymentChequePM()
                                                         {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          CurrencyCode = a.Currency.Code,
                                                          SearchFields = a.SearchFields,
                                                          LineNumber = a.LineNumber,
                                                          ChequeNumber = a.ChequeNumber,
                                                          ValueDate = a.ValueDate,
                                                          LocalAmount = a.LocalAmount,
                                                          ForeignAmount = a.ForeignAmount,
                                                          BankId = a.BankId,
                                                          BankBranch = a.BankBranch,
                                                          BankAccount = a.BankAccount,
                                                          StatusName = a.ARPaymentChequeStatus != null ? a.ARPaymentChequeStatus.EnglishName : "",
                                                          PaymentId = paymentId,
                                                          ExchangeRate = a.ExchangeRate,
                                                          StatusCode = a.StatusCode,
                                                          CurrencyId = a.CurrencyId,
                                                         

                                                      }).ToList();
            return paymentCheques;
        }

        public List<ARPaymentChequePM> GetInBankAccountChequesByPaymentId(string paymentId, int tenant)
        {
            List<ARPaymentChequePM> paymentCheques = (from a in context.ARPaymentCheques
                                                      where a.PaymentId == paymentId && a.Tenant == tenant && a.StatusCode =="6"
                                                      select new ARPaymentChequePM()
                                                      {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          CurrencyCode = a.Currency.Code,
                                                          SearchFields = a.SearchFields,
                                                          LineNumber = a.LineNumber,
                                                          ChequeNumber = a.ChequeNumber,
                                                          ValueDate = a.ValueDate,
                                                          LocalAmount = a.LocalAmount,
                                                          ForeignAmount = a.ForeignAmount,
                                                          BankId = a.BankId,
                                                          BankBranch = a.BankBranch,
                                                          BankAccount = a.BankAccount,
                                                          StatusName = a.ARPaymentChequeStatus != null ? a.ARPaymentChequeStatus.EnglishName : "",
                                                          PaymentId = paymentId,
                                                          ExchangeRate = a.ExchangeRate,
                                                          StatusCode = a.StatusCode,
                                                          CurrencyId = a.CurrencyId,


                                                      }).ToList();
            return paymentCheques;
        }
        public ARPaymentChequePM GetSingleByPaymentId(string paymentId, int tenant)
        {
            ARPaymentChequePM paymentCheques = (from a in context.ARPaymentCheques
                                                      where a.PaymentId == paymentId && a.Tenant == tenant
                                                      select new ARPaymentChequePM()
                                                      {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          CurrencyCode = a.Currency.Code,
                                                          SearchFields = a.SearchFields,
                                                          LineNumber = a.LineNumber,
                                                          ChequeNumber = a.ChequeNumber,
                                                          ValueDate = a.ValueDate,
                                                          LocalAmount = a.LocalAmount,
                                                          ForeignAmount = a.ForeignAmount,
                                                          BankId = a.BankId,
                                                          BankBranch = a.BankBranch,
                                                          BankAccount = a.BankAccount,
                                                          StatusName = a.ARPaymentChequeStatus != null ? a.ARPaymentChequeStatus.EnglishName : "",
                                                          StatusCode = a.ARPaymentChequeStatus != null ? a.ARPaymentChequeStatus.Code : "",

                                                      }).FirstOrDefault();
            return paymentCheques;
        }



        public ARPaymentChequePM GetPaymentChequeByChequeNo(string chequeNumber, int tenant)
        {
            ARPaymentCheque poco = (from a in context.ARPaymentCheques
                                  where a.ChequeNumber == chequeNumber && a.Tenant == tenant
                                  select a).FirstOrDefault();

            return this.GetEntityPM(poco);
        }


        public List<ARPaymentChequePM> GetOpenARPaymentCheques( int tenant)
        {
            List<ARPaymentCheque> pocos = (from a in context.ARPaymentCheques
                                    where  a.Tenant == tenant
                                    
                                    select a).ToList();

            return pocos.Select(r => this.GetEntityPM(r)).ToList();
        }

        public List<ARPaymentChequePM> GetChequesByIds(List<string> ids, int tenant)
        {
            List<ARPaymentCheque> cheques = (from a in context.ARPaymentCheques
                                                    where ids.Contains(a.Id) && a.Tenant == tenant
                                                    select a).ToList();

            return cheques.Select(rec => GetEntityPM(rec)).ToList();
        }

        public string GetAccountIdForCheque(int tenant, string paymentId, int lineNumber)
        {
            return repository.GetAccountIdForCheque(tenant, paymentId, lineNumber);
        }


        public  int GetOpenChequesByBankAccount(string bankId, int tenant)
        {

            ARPaymentChequeRepository aRPaymentChequeRepository = new ARPaymentChequeRepository(tenant);
           return aRPaymentChequeRepository.GetOpenChequesByBankAccount(bankId, tenant).Where(a=>a.ValueDate> DateTime.Now).Count();
         
        }
        public string CheckARPaymentChequeAlreadyExists(string chequeOrPaymentRef,  string bank, string bankBranch, string bankAccount, int tenant, bool useLocal)
        {
       
            ARPaymentChequeRepository arPaymentChequeRepository = new ARPaymentChequeRepository(tenant);
            return arPaymentChequeRepository.CheckARPaymentChequeAlreadyExists(chequeOrPaymentRef, bank, bankAccount, bankBranch, tenant, useLocal);

        }


        public List<ARPaymentChequePM> GetOpenChequesByBankAccountInThePast(string bankId, int tenant,DateTime valueDate)
        {
            ARPaymentChequeRepository aRPaymentChequeRepository = new ARPaymentChequeRepository(tenant);
            var cheques = aRPaymentChequeRepository.GetOpenChequesByBankAccount(bankId, tenant).Where(a => a.ValueDate <= valueDate).ToList();
            return cheques.Select(rec => GetEntityPM(rec)).ToList();
         }
    }
}

