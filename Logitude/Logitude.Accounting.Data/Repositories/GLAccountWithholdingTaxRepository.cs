 
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
   public partial class GLAccountWithholdingTaxRepository:IRepository<GLAccountWithholdingTax>
   {
        
		public List<GLAccountWithholdingTax> GetMulti(EntityKeyFields entityKeys)
        {

            GLAccountKeys gLAccountKeys = entityKeys as GLAccountKeys;

            return (from a in context.GLAccountWithholdingTax
                    where a.GLAccountId == gLAccountKeys.Id
                    select a).ToList();
        }

        public int GetMaxLineNumber(string glAccountId, int tenant)
        {
            int maxLine = (from a in context.GLAccountWithholdingTax
                           where a.GLAccountId == glAccountId && a.Tenant == tenant
                           select a).Max(d => (int?)d.LineNumber) ?? 0;
            return maxLine;
        }

    }

}
   