 
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
using System.Diagnostics;
using Logitude.Server.Tools;
namespace Logitude.Accounting.Data.Repositories
{
   public partial class JournalLineRepository:IRepository<JournalLine>
   {
        
		public List<JournalLine> GetMulti(EntityKeyFields entityKeys)
        {
            JournalKeys journalKeys = entityKeys as JournalKeys;

            return (from a in context.JournalLines
                    where a.JournalId == journalKeys.Id
                    select a).ToList();
        }
        public List<JournalLine> GetJournalLines(string JournalId, int tenant)
        {
            return (from a in context.JournalLines
                    where a.JournalId == JournalId && a.Tenant == tenant
                    select a).ToList();
        }
        public JournalLine GetSingleJournalLine(string journalId, int line, int tenant)
        {
            return (from a in context.JournalLines
                    where a.JournalId == journalId && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
        public List<JournalLine> GetJournalLineByLedgerTransactionIdList(List<String> idList, int tenant)
        {
            var qLedgerTransactions =
               (from a in context.LedgerTransactions
                where idList.Contains(a.Id) && a.Tenant == tenant
                select a);
            var q = (from j in this.GetAll(tenant)
                     join l in qLedgerTransactions
                     on new { j.JournalId, j.Line } equals new { l.JournalId, Line = l.JournalLineNumber }
                     select j);
            var pocos = q.ToList();
            return pocos;
        }


        public IQueryable<JournalLine> GetQueryContainsAccId(IQueryable<string> GLAccountIDList, int tenant
             )
        {
            return (from a in context.JournalLines
                    where a.Tenant == tenant
                    where (GLAccountIDList.Contains(a.CreditAccountId) || GLAccountIDList.Contains(a./*DebitControlAccountId*/ DebitAccountId))
                    select a);
        }


        partial void onUpdate()//Partial Methods
        {
            InsureUsingOnlyByUpdateService();
        }

        partial void onAdd()//Partial Methods
        {
            InsureUsingOnlyByUpdateService();
        }
        private void InsureUsingOnlyByUpdateService()
        {
            var myName = this.NameOf();
            if (myName != "JournalLineRepositoryPriv")
            {
                AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Critical);


                throw new Exception("InsureUsingOnlyByUpdateService");

            }
            return;//mohammad temp fix until itzik is back
            int iFrame = 3;
            var mth = new StackTrace().GetFrame(iFrame).GetMethod();

            var cls = mth.ReflectedType.Name;
            if (cls == "JournalLineUpdateService") //never happen 
            {
                return;
            }
            if (cls == "EntityUpdateService`3" && mth.Name == "PerformUpdate")
            {
                return;
            }
            AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Critical);
            
            var checkInsureUsingOnlyByUpdateService = System.Configuration.ConfigurationManager.AppSettings.Get("InsureUsingOnlyByUpdateService");
            if (!string.IsNullOrWhiteSpace(checkInsureUsingOnlyByUpdateService))
            {
                throw new Exception("InsureUsingOnlyByUpdateService");
            }

        }
   }

}
   