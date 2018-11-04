 
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
        public IQueryable<JournalLine> GetQueryContainsAccId(IQueryable<string> GLAccountIDList, int tenant
             )
        {
            return (from a in context.JournalLines
                    where a.Tenant == tenant
                    where (GLAccountIDList.Contains(a.CreditAccountId) || GLAccountIDList.Contains(a.DebitControlAccountId))
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
        private static void InsureUsingOnlyByUpdateService()
        {
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
   