 
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

namespace Logitude.Accounting.Data.Repositories
{
   public partial class JournalMoreDataRepository:IRepository<JournalMoreData>
   {
        
		public List<JournalMoreData> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public JournalMoreData GetSingleJournalMoreData(string journalId, int tenant)
        {
            return (from a in context.JournalMoreDatas

                    where a.JournalId == journalId && a.Tenant == tenant

                    
                    select a).FirstOrDefault();
        }


    }

}
   