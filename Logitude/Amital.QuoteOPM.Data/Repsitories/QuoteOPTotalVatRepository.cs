 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Amital.QuoteOPM.Data.Repsitories
{
   public partial class QuoteOPTotalVATRepository:IRepository<QuoteOPTotalVAT>
   {
        
		public List<QuoteOPTotalVAT> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public IQueryable<QuoteOPTotalVAT> GetTotalVATs(string quoteId, int tenant)
        {
            return (from a in context.QuoteOPTotalVATs where a.Tenant == tenant && a.QuoteOPId == quoteId select a);
        }
    }

}
   