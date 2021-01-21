using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class BankDepositRedeemedChequesVerifyService
    {
        public string LoggingText { get; set; } = "";
        public BankDepositRedeemedChequesVerifyService()
        {
        }


        public void GetAndUpdateChequesForTenant(int tenant)
        {
            var cheques = GetNotRedeemedReconciledCheques(tenant);
            SetChequesAsRedeemed(tenant, cheques);
        }
        public List<ARPaymentChequePM> GetNotRedeemedReconciledCheques(int tenant)
        {
            Log("[Tenant " + tenant + "] getting cheques ...");

            var context = AccountingContext.GetContext(tenant);

            //var externallyReconciledChequeDepositTransactions
            //    = from ledger in context.LedgerTransactions
            //      join journal in context.Journals on ledger.JournalId equals journal.Id
            //      join cheque in context.ARPaymentCheques on journal.AccountingEntityId equals cheque.Id
            //      where ledger.IsExternalReconcile == true
            //             && journal.AccountingEntityCode == "6"
            //      select ledger;

            List<string> depositIds
                                = (from ledger in context.LedgerTransactions
                                  join journal in context.Journals on ledger.JournalId equals journal.Id
                                  join cheque in context.ARPaymentCheques on journal.AccountingEntityId equals cheque.Id
                                  where ledger.IsExternalReconcile == true
                                         && journal.AccountingEntityCode == "6"
                                  select journal.AccountingEntityId).ToList();

            List<string> paymentChequeIds = (from a in context.BankDepositLines
                                             where depositIds.Contains(a.DepositId)  && a.Tenant == tenant
                                             select a.ARPaymentChequeId).ToList();

            List<ARPaymentChequePM> paymentChequesNotRedeemed = (from cheque in context.ARPaymentCheques
                                                      where paymentChequeIds.Contains(cheque.Id) && cheque.Tenant == tenant
                                                            && cheque.StatusCode != "6" 
                                                      select new ARPaymentChequePM()
                                                      {
                                                          Id = cheque.Id,
                                                          Tenant = cheque.Tenant,
                                                          CurrencyCode = cheque.Currency.Code,
                                                          SearchFields = cheque.SearchFields,
                                                          LineNumber = cheque.LineNumber,
                                                          ChequeNumber = cheque.ChequeNumber,
                                                          ValueDate = cheque.ValueDate,
                                                          LocalAmount = cheque.LocalAmount,
                                                          ForeignAmount = cheque.ForeignAmount,
                                                          BankId = cheque.BankId,
                                                          BankBranch = cheque.BankBranch,
                                                          BankAccount = cheque.BankAccount,
                                                          StatusName = cheque.ARPaymentChequeStatus != null ? cheque.ARPaymentChequeStatus.EnglishName : "",
                                                          PaymentId = cheque.PaymentId,
                                                          ExchangeRate = cheque.ExchangeRate,
                                                          StatusCode = cheque.StatusCode,
                                                          CurrencyId = cheque.CurrencyId,
                                                      }).ToList();


            Log("[Tenant " + tenant + "] cheques got, count: " + paymentChequesNotRedeemed.Count());

            return paymentChequesNotRedeemed;

        }

        public void RedeemCheques(List<ARPaymentChequePM> cheques)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            ARPaymentChequeUpdateService service = new ARPaymentChequeUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);

            foreach (var cheque in cheques)
            {
                cheque.StatusCode = ARPaymentChequeStatusValues.Redeemed;
                cheque.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                service.Update(cheque, false);
            }

            service.Save();
        }

        public void UpdateChequeAsRedeemed(int tenant, string chequeId)
        {
            try
            {
                IAccountingContext MyContext = AccountingContext.GetContext(tenant);

                ARPaymentChequePM arpcheque = (from cheque in MyContext.ARPaymentCheques
                                               where cheque.Id == chequeId && cheque.Tenant == tenant
                                               select new ARPaymentChequePM()
                                               {
                                                   Id = cheque.Id,
                                                   Tenant = cheque.Tenant,
                                                   CurrencyCode = cheque.Currency.Code,
                                                   SearchFields = cheque.SearchFields,
                                                   LineNumber = cheque.LineNumber,
                                                   ChequeNumber = cheque.ChequeNumber,
                                                   ValueDate = cheque.ValueDate,
                                                   LocalAmount = cheque.LocalAmount,
                                                   ForeignAmount = cheque.ForeignAmount,
                                                   BankId = cheque.BankId,
                                                   BankBranch = cheque.BankBranch,
                                                   BankAccount = cheque.BankAccount,
                                                   StatusName = cheque.ARPaymentChequeStatus != null ? cheque.ARPaymentChequeStatus.EnglishName : "",
                                                   PaymentId = cheque.PaymentId,
                                                   ExchangeRate = cheque.ExchangeRate,
                                                   StatusCode = cheque.StatusCode,
                                                   CurrencyId = cheque.CurrencyId,
                                               }).FirstOrDefault();

                ARPaymentChequeUpdateService service = new ARPaymentChequeUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);

                arpcheque.StatusCode = ARPaymentChequeStatusValues.Redeemed;
                arpcheque.ChangeSetOp = ChangeSetOperation.Update;

                service.Update(arpcheque, false);

                service.Save();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public void SetChequesAsRedeemed(int tenant, List<ARPaymentChequePM> chequesPMs)
        {
            try
            {
                Log("[Tenant " + tenant + "] update cheques started");

                List<string> chequesIds = chequesPMs.Select(d => d.Id).ToList();
                ARPaymentChequeRepository chequeRepository = new ARPaymentChequeRepository(tenant);
                var cheques = chequeRepository.GetByIds(tenant, chequesIds);

                foreach (var cheque in cheques)
                {
                    cheque.StatusCode = ARPaymentChequeStatusValues.Redeemed;
                    //chequeRepository.Update(cheque);
                }

                //chequeRepository.SubmitChanges();

                Log("[Tenant " + tenant + "] update cheques finished");

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private void Log(string text)
        {
            LoggingText += text +Environment.NewLine;
        }


    }
}
