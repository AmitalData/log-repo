 
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
using System.Runtime.InteropServices;
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

            var q = from j in context.JournalLines
                    join l in context.LedgerTransactions
                        on new { j.JournalId, Line = j.Line }
                        equals new { l.JournalId, Line = l.JournalLineNumber }
                    where idList.Contains(l.Id) && l.Tenant == tenant && j.Tenant == tenant
                    select j;

            return q.ToList();
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
   