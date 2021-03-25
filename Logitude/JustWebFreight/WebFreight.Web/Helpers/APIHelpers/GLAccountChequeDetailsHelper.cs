using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class GLAccountChequeDetailsHelper
    {
        CurrencyQueryService CurrencyQuery;
        List<ARPaymentChequeReplicaPM> paymentCheques;
        List<Cheque> cheques;
        int tenant;
        public GLAccountChequeDetailsHelper(int Tenant)
        {
           tenant = Tenant;
            CurrencyQuery = new CurrencyQueryService(tenant);
            cheques = new List<Cheque>();
        }
      
        public GLAccountChequeDetails GetLAccountChequeDetails(string number)
        {
            GLAccountPM gLAccount = GetGLAccountByNumber(number, tenant);
            List<CardList> cards = GetGLaccountConnectedCards(gLAccount);
            List<LedgerTransaction>  externalTransactions = GetGLaccountConnectedExternalTransactions(gLAccount);
            foreach (CardList card in cards)
            {
                paymentCheques= GetCardPaymentCheques(card);
                FillARPaymentChequesList(paymentCheques);
            }
            GLAccountChequeDetails gLAccountChequeDetails = new GLAccountChequeDetails()
            {
                DisplayNumber = gLAccount.DisplayNumber,
                Id = gLAccount.Id,
                TotalOpenChequesInLocalCur = gLAccount.TotalOpenChequesInLocalCur,
                TotFutureOpenChequesInLocalCur = gLAccount.TotFutureOpenChequesInLocalCur,
                LocalName = gLAccount.LocalName,
                Tenant = gLAccount.Tenant,
                GLaccountCheques = cheques,
                ExternalTransactions = externalTransactions
            };
            return gLAccountChequeDetails;
        }
     
        private GLAccountPM GetGLAccountByNumber(string internalNumber, int tenant)
        {
            Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService Service = new Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService(tenant);
            GLAccountPM account = Service.GetByInternalNumber(internalNumber, tenant);
            if (account == null)
            {
                throw new Exception("There is no GLAccount with number" + internalNumber);
            }
            else { return account; }
        }

        private List<CardList> GetGLaccountConnectedCards(GLAccountPM account)
        {
            CardQuery cardQuery = new CardQuery(account.Tenant);
            return cardQuery.GetAllCardsByGLAccount(account.Id, account.Tenant);

        }

        private List<LedgerTransaction> GetGLaccountConnectedExternalTransactions(GLAccountPM gLAccount)
        {
            List<LedgerTransactionList> transactions = GetTransactionsForGLaccount(gLAccount);
            List<LedgerTransaction> externalTransactions = FillExternalTransactionsList(transactions);
            return externalTransactions;
        }

        private List<LedgerTransactionList> GetTransactionsForGLaccount(GLAccountPM gLAccount)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService service = new LedgerTransactionListQueryService(accountingContext);
            var transactions = service.GetExternalTransactionsForAccount(gLAccount.Id, tenant).ToList();
            return transactions;
        }

        private List<LedgerTransaction> FillExternalTransactionsList(List<LedgerTransactionList> transactions)
        {
            List<LedgerTransaction> externalTransactions = new List<LedgerTransaction>();
            foreach (LedgerTransactionList transaction in transactions)
            {
                LedgerTransaction ledgerTransaction = new LedgerTransaction()
                {
                    Currency = CurrencyQuery.GetCurrencyById(transaction.CurrencyId, transaction.Tenant),
                    DueDate = transaction.DueDate,
                    LocalAmount = transaction.LocalAmountCredit,
                    ForeignAmount = transaction.ForeignAmountCredit,
                    Reference1 = transaction.Reference1,
                    Reference2 = transaction.Reference2,
                    Notes = transaction.Notes
                };
                externalTransactions.Add(ledgerTransaction);
            }

            return externalTransactions;
        }

        private List<ARPaymentChequeReplicaPM> GetCardPaymentCheques(CardList card)
        {
          
            paymentCheques = new List<ARPaymentChequeReplicaPM>();
            List<ARPaymentPM> payments = GetARPayments(card);           
           foreach(ARPaymentPM payment in payments)
            {
                paymentCheques.AddRange(payment.ARPaymentChequeReplicas);
            }
            return paymentCheques;
     
        }
        private List<ARPaymentPM> GetARPayments(CardList card)
        {
            ARPaymentQuery aRPaymentQuery = new ARPaymentQuery(card.Tenant);
           return aRPaymentQuery.GetARpaymentsForCard(card.Id, card.Tenant);
        }
        private  void FillARPaymentChequesList(List<ARPaymentChequeReplicaPM> paymentCheques)
        {
          
              foreach(ARPaymentChequeReplicaPM paymentCheque in paymentCheques)
                {
                    Cheque cheque = new Cheque()
                    {
                        ChequeNumber = paymentCheque.ChequeNumber,
                        BankAccount = paymentCheque.BankAccount,
                        ForeignAmount = paymentCheque.ForeignAmount,
                        LocalAmount = paymentCheque.LocalAmount,
                        BankBranch = paymentCheque.BankBranch,
                        Id = paymentCheque.Id,
                        Tenant = paymentCheque.Tenant,
                        BankId = paymentCheque.BankId,
                        Currency = CurrencyQuery.GetCurrencyById(paymentCheque.CurrencyId, paymentCheque.Tenant),
                        StatusCode = paymentCheque.StatusCode,
                        ValueDate = paymentCheque.ValueDate,
                    };
                    cheques.Add(cheque);
                }
           
        }

   
    }
}
