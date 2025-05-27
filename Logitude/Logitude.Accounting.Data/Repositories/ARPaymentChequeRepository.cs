 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Runtime.Remoting.Contexts;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class ARPaymentChequeRepository:IRepository<ARPaymentCheque>
   {
        private const string DateFormat = "yyyy-MM-dd";
        private const string AmountFormat = "F2";


        public List<ARPaymentCheque> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public IQueryable<ARPaymentCheque> GetQueryValueDateAndNumber(int tenant, string chequeNumber, DateTime @from, DateTime to,
    string currencyId,
    string searchByFilter)
        {
            var q = (from rec in context.ARPaymentCheques
                     where rec.Tenant == tenant
                     select rec
                     );
            if (context.ToString().StartsWith("Fake"))
            {
                q = (from rec in q
                     where rec.ValueDate.Date >= @from
                     where rec.ValueDate.Date <= to
                     select rec
                    );
            }
            else
            {
                q = (from rec in q
                     where EntityFunctions.TruncateTime(rec.ValueDate) >= @from
                     where EntityFunctions.TruncateTime(rec.ValueDate) <= to
                     select rec
                     );
            }

            if (!string.IsNullOrWhiteSpace(currencyId))
            {
                q = q.Where(rec => rec.CurrencyId == currencyId);
            }

            if (!string.IsNullOrWhiteSpace(chequeNumber))
            {
                q = q.Where(rec => rec.ChequeNumber == chequeNumber);
            }

            if (!string.IsNullOrWhiteSpace(searchByFilter))
            {
                q = q.Where(rec => rec.SearchFields.Contains(searchByFilter));
            }

            q = (from rec in q
                 orderby rec.ValueDate, rec.Id
                 select rec);


            return q;



        }


        public List<ARPaymentCheque> GetByIds(int tenant, List<string> ids)
        {
            return (from a in context.ARPaymentCheques
                   where a.Tenant == tenant && ids.Contains(a.Id)
                   select a).ToList();
        }

        public void RemoveARPaymentsCheques(string paymentId, List<string> arPaymentChequesIds, int tenant)
        {
            var aRPaymentCheques = (from a in context.ARPaymentCheques where a.PaymentId == paymentId && arPaymentChequesIds.Contains(a.Id) && a.Tenant == tenant select a).ToList();

            foreach (var entity in aRPaymentCheques)
            {
                context.ARPaymentCheques.Attach(entity);
                context.ARPaymentCheques.Remove(entity);
            }
        }
        public List<ARPaymentCheque> GetOpenChequesByBankAccount(string bankId, int tenant)
        {
            return context.BankDeposits
                .Join(context.BankDepositLines,
                    deposit => deposit.Id,
                    line => line.DepositId,
                    (deposit, line) => new { deposit, line })
                .Join(context.ARPaymentCheques,
                    combined => combined.line.ARPaymentChequeId,
                    cheque => cheque.Id,
                    (combined, cheque) => new { combined.deposit, combined.line, cheque })
                .Where(a => a.deposit.DepositBankAccountId == bankId && a.deposit.Tenant == tenant && (a.cheque.StatusCode == "2" || a.cheque.StatusCode == "3"))
                .Select(b => b.cheque)
                .ToList();
        }

        public string CheckARPaymentChequeAlreadyExists(string ChequeNumber, string BankId, string BankAccount, string BankBranch, int tenant ,bool useLocal, string arPaymentId = null)
        {
            var existingCheques = new StringBuilder();

            // temporary canceled until spec will be fixed
            return existingCheques.ToString();

            var existingCheque = context.ARPaymentCheques
                .FirstOrDefault(a => a.Tenant == tenant &&
                                     a.ChequeNumber == ChequeNumber &&
                                     a.BankId == BankId &&
                                     a.BankAccount == BankAccount &&
                                     a.BankBranch == BankBranch   &&
                                     (arPaymentId == null || a.PaymentId != arPaymentId));

            if (existingCheque != null)
            {
                var invoiceContext = InvoiceContext.GetContext(tenant);

                 var paymentNumber = invoiceContext.ARPayments
                   .Where(a => a.Tenant == tenant && a.Id == existingCheque.PaymentId && a.StatusCode != "VD" )
                   .Select(a => a.PaymentNo)
                   .FirstOrDefault();

                if(!string.IsNullOrWhiteSpace(paymentNumber))
                {
                    string msg = TranslateTextsClass.Translate("ARPaymentCheque.O.ChequeAlreadyexists", tenant, useLocal);
                    string textCodeAlreadyExist = msg.Replace("ChequeNumber", existingCheque.ChequeNumber)
                        .Replace("PaymentNumber", paymentNumber)
                        .Replace("LocalAmount", existingCheque.LocalAmount.ToString(AmountFormat));

                    if (existingCheque.ValueDate != null)
                    {
                        textCodeAlreadyExist = textCodeAlreadyExist.Replace("ValueDate", existingCheque.ValueDate.ToString(DateFormat) ?? " ");
                    }

                    existingCheques.AppendLine(textCodeAlreadyExist);
                }
               
            }

            return existingCheques.ToString();
        }

        public bool IsChequeExists(string paymentId, string arPaymentChequesId, int tenant)
        {
            return context.ARPaymentCheques.Any(a => a.PaymentId == paymentId && a.Id == arPaymentChequesId && a.Tenant == tenant);
        }

        public string GetAccountIdForCheque(int tenant, string paymentId, int lineNumber)
        {
            return (from a in context.Journals
                    join l in context.LedgerTransactions
                    on a.Id equals l.JournalId
                    where a.AccountingEntityId == paymentId && a.Tenant == tenant && l.JournalLineNumber == lineNumber 
                    select l.AccountId).FirstOrDefault();
        }


    }

}
   