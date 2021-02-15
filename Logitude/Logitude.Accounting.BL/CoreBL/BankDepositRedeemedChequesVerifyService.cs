using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Accounting.BL.CoreBL
{
    public class BankDepositRedeemedChequesVerifyService
    {
        public string LoggingText { get; set; } = "";
        public int DoneTenants = 0;
        public int WorkingTenant;
        public List<ARPaymentCheque> DoneCheques = new List<ARPaymentCheque>();

        public BankDepositRedeemedChequesVerifyService()
        {
        }

        public void UpdateCheqesForTenantList(List<string> Tenants)
        {
            DoneCheques = new List<ARPaymentCheque>();
            foreach (var tenant in Tenants)
            {
                WorkingTenant = Convert.ToInt32(tenant);
                GetAndUpdateChequesForTenant(WorkingTenant);
                DoneTenants++;
            }
            writeChequesOnFile();
        }
        public void GetAndUpdateChequesForTenant(int tenant)
        {
            var cheques = GetNotRedeemedReconciledCheques(tenant);
            SetChequesAsRedeemed(tenant, cheques);
        }

        public void RecalculateChequesTotals(List<string> Tenants)
        {
            Log(">>> fixing cheques totals");

            foreach (var tenantStr in Tenants)
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    try
                    {
                        Log("-----------------------------------------------------------------");
                        Log("   >>> fixing for tenant " + tenantStr);

                        billToAccounts = new List<string>();

                        var tenant = Convert.ToInt32(tenantStr);
                        var tenantCheques = DoneCheques.Where(d => d.Tenant == tenant);

                        Log("[Tenant " + tenantStr + "] get bill to accounts");

                        foreach (var cheque in tenantCheques)
                        {
                            GetTenantBillToAccounts(tenant, cheque.PaymentId);
                        }

                        List<string> billtos = billToAccounts.Distinct().ToList();

                        Log("[Tenant " + tenantStr + "] bill to accounts got, (" + billtos.Count() + ")");


                        foreach (var billTo in billtos)
                        {
                            GLAccountChequesTotalCalculator chequesTotalCalculator = new GLAccountChequesTotalCalculator(tenant);
                            chequesTotalCalculator.RecalculateChequesTotalForBillToAccount(billTo);
                        }

                        scope.Complete();

                        Log("   >>>>> tenant (" + tenantStr + ") cheques recalculated successfully");
                    }
                    catch (Exception ex)
                    {
                        Log("*FAILED* Tenant(" + tenantStr + ")" + ex.Message);

                        throw;
                    }


                }
            }


            Log(".........................");
            Log(".........................");
            Log(">>> cheques recalculated successfully for giver tenants");

        }
        public void RecalculateAllBilltoChequesTotals(List<string> Tenants)
        {
            Log(">>> fixing cheques totals");

            foreach (var tenantStr in Tenants)
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    try
                    {
                        Log("-----------------------------------------------------------------");
                        Log("   >>> fixing for tenant " + tenantStr);

                        if (string.IsNullOrWhiteSpace(tenantStr))
                            return;

                        var tenant = Convert.ToInt32(tenantStr);

                        List<string> billToAccountsIds = GetTenantBillToAccountsThatHaveCheques(tenant);

                        foreach (var billToAccountId in billToAccountsIds)
                        {
                            GLAccountChequesTotalCalculator chequesTotalCalculator = new GLAccountChequesTotalCalculator(tenant);
                            chequesTotalCalculator.RecalculateChequesTotalForBillToAccount(billToAccountId);
                        }

                        scope.Complete();
                        Log("   >>>>> tenant (" + tenantStr + ") cheques recalculated successfully");
                    }
                    catch (Exception ex)
                    {
                        Log("*FAILED* Tenant(" + tenantStr + ")" + ex.Message);

                        throw;
                    }


                }
            }


            Log(".........................");
            Log(".........................");
            Log(">>> cheques recalculated successfully for giver tenants");

        }

        private static List<string> GetTenantBillToAccountsThatHaveCheques(int tenant)
        {
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);

            return (from cheque in invoiceContext.ARPaymentChequeReplicas
                    join payment in invoiceContext.ARPayments on cheque.PaymentId equals payment.Id
                    where cheque.Tenant == tenant
                    select payment.BillToId).Distinct().ToList();
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
                                   where ledger.IsExternalReconcile == true
                                          && journal.AccountingEntityCode == "6"
                                          && ledger.Tenant == tenant
                                   select journal.AccountingEntityId).ToList();

            List<string> paymentChequeIds = (from a in context.BankDepositLines
                                             where depositIds.Contains(a.DepositId) && a.Tenant == tenant
                                             select a.ARPaymentChequeId).ToList();

            List<ARPaymentChequePM> paymentChequesNotRedeemed = (from cheque in context.ARPaymentCheques
                                                                 where paymentChequeIds.Contains(cheque.Id) && cheque.Tenant == tenant
                                                                       && cheque.StatusCode == "3"
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
                                                                 }).OrderBy(d => d.ChequeNumber).ToList();


            Log("[Tenant " + tenant + "] cheques got, count: " + paymentChequesNotRedeemed.Count());

            return paymentChequesNotRedeemed;

        }

        public void RedeemCheques(List<ARPaymentChequePM> cheques, int tenant)
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

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    List<string> chequesIds = chequesPMs.Select(d => d.Id).ToList();
                    ARPaymentChequeRepository chequeRepository = new ARPaymentChequeRepository(tenant);
                    List<ARPaymentCheque> cheques = chequeRepository.GetByIds(tenant, chequesIds);

                    foreach (var cheque in cheques)
                    {
                        cheque.StatusCode = ARPaymentChequeStatusValues.Redeemed;
                        chequeRepository.Update(cheque);
                    }


                    chequeRepository.SubmitChanges();

                    DoneCheques.AddRange(cheques);

                    scope.Complete();
                }


                Log("[Tenant " + tenant + "] update cheques finished");
                Log("......................................");

            }
            catch (Exception ex)
            {
                Log("*failed* [Tenant " + tenant + "] " + ex.Message);

                throw;
            }

        }

        private void Log(string text)
        {
            LoggingText += text + Environment.NewLine;
        }

        private void writeChequesOnFile()
        {
            var fileName = @"D:\Abdullah\cheques.csv";
            // Check if file already exists. If yes, delete it.     
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var all = "Tenant,Cheques Number,Cheque Id" + Environment.NewLine;
            var list = DoneCheques.Select(d => d.Tenant + "," + d.ChequeNumber + "," + d.Id).ToList();
            var text = string.Join(Environment.NewLine, list);
            all += text;
            // Create a new file     
            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.WriteLine(all);
            }
        }


        List<string> billToAccounts;
        private void GetTenantBillToAccounts(int tenant, string paymentId)
        {

            ARPaymentRepository repo = new ARPaymentRepository(tenant);

            ARPayment payment = repo.GetSingleNotCancelledARPayment(paymentId, tenant);
            if (payment != null)
            {
                billToAccounts.Add(payment.BillToId);
            }
        }

        private void CalculateBilltoFutureCheques(int tenant, string billTo)
        {
            ARPaymentRepository repo = new ARPaymentRepository(tenant);

            List<ARPayment> payments = repo.GetARPaymentsByBillTo(billTo, tenant);
            List<string> paymentIds = new List<string>();
            foreach (var item in payments)
            {

                paymentIds.Add(item.Id);

            }


            ARPaymentChequeQueryService queryService = new ARPaymentChequeQueryService(tenant);
            List<ARPaymentChequePM> aRPaymentChequePMs = queryService.GetARPaymentChequesByPaymentIds(paymentIds, tenant);
            CardRepository cardRepo = new CardRepository(tenant);


            //if (card != null)
            //{
            string GLAccountId = cardRepo.GetGLAccountIdByCardId(billTo, tenant);
            GLAccountMoreDataQueryService moreDataQueryService = new GLAccountMoreDataQueryService(tenant);
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            GLAccountMoreDataPM moreDataPM = moreDataQueryService.GetSingle(GLAccountId, false, false);
            if (moreDataPM == null)
                return;
            GLAccountMoreDataUpdateService updateService = new GLAccountMoreDataUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            moreDataPM.TotFutureOpenChequesInLocalCur = 0;
            moreDataPM.TotalOpenChequesInLocalCur = 0;
            foreach (ARPaymentChequePM item in aRPaymentChequePMs)
            {
                if (item.StatusCode != "6" && item.StatusCode != "5")
                {
                    if (item.ValueDate > DateTime.Today)
                    {
                        moreDataPM.TotFutureOpenChequesInLocalCur += item.LocalAmount;

                    }
                    else
                    {
                        moreDataPM.TotalOpenChequesInLocalCur += item.LocalAmount;

                    }
                }
            }

            moreDataPM.ChangeSetOp = ChangeSetOperation.Update;
            updateService.Update(moreDataPM, true);

            Log("[Tenant " + tenant + "] account updated (" + billTo + ")");

        }
    }
}
