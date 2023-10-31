 
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
using Logitude.Accounting.Data.EntityLists;
using Simplog.Data.Helpers;
using Logitude.Accounting.Data.Enums;
using System.Data.Entity.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class GLAccountMoreDataRepository:IRepository<GLAccountMoreData>
   {
        
		public List<GLAccountMoreData> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public List<LedgerTransactionList> GetAllChecks(string billToId, int tenant, bool isFuture, bool showLocal = true, bool withoutDate = false)
        {

           DateTime today = GetCurrentDate(tenant);
           (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            var query
                = (from a in context.AllARPaymentChequesViews
                   where a.BillToId == billToId && a.Tenant == tenant
                  && (withoutDate || (isFuture && a.ValueDate > today) || (!isFuture && a.ValueDate <= today))
                   select new LedgerTransactionList()
                   {
                       PaymentValueDate = a.ValueDate,
                       PaymentChequeStatus = showLocal ? a.LocalName : a.EnglishName,
                       Source = a.AccountingEntityReference,
                       SourceType = a.AccountingEntityCode,
                       SourceNumber = a.AccountingEntityReference,
                       LocalAmountCredit = a.LocalAmountCredit,
                       ForeignAmountCredit = a.ForeignAmountCredit,
                       Reference1 = a.Reference1,
                       Reference2 = a.Reference2,
                       Reference3 = a.Reference3,
                       JournalNumber = a.Journalnumber,
                       Notes = a.Notes,
                       AccountId = a.GLAccountId,
                       // InternalNote = transaction.InternalNote,
                       //UpdateDateTime = transaction.UpdateDateTime,
                       //UpdatedByUserName = transaction.UpdatedByUserName,
                       SourceId = a.AccountingEntityId,
                       SourceTypeCode = a.Type == "C" ? AccountingEntityValues.ARPayment : AccountingEntityValues.Journal,
                       JournalId = a.journalId,
                       IconCode = a.Type == "C" ? "PY" : "JR",
                       IsForeignAmountCreditPos = a.ForeignAmountCredit != 0,
                       IsLocalAmountCreditPos = a.LocalAmountCredit != 0,
                       CalculatedForeignAmount = a.ForeignAmountCredit != 0 ? a.ForeignAmountCredit : a.ForeignAmountDebit,
                       CalculatedLocalAmount = a.LocalAmountCredit != 0 ? a.LocalAmountCredit : a.LocalAmountDebit,
                       //CurrencySign = transaction.Currency.Sign,
                       Tenant = a.Tenant
                   });



            return query.ToList();


        }
         private static DateTime GetCurrentDate(int tenant)
        {
            DateTime today = TenantServerConfigration.GetCurrentDateTime(tenant);
            today = new DateTime(today.Year, today.Month, today.Day, 11, 59, 59);
            return today;
        }

    }

}
   