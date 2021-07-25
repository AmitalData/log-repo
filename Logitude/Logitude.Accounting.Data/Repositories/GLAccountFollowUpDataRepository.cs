 
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
   public partial class GLAccountFollowUpDataRepository:IRepository<GLAccountFollowUpData>
   {
        
		public List<GLAccountFollowUpData> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public GLAccountFollowUpData GetSingleByAccountId(string accountid, int tenant)
        {
            return (from a in context.GLAccountFollowUpDatas
                    where a.GlAccountId == accountid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
    }

}
   