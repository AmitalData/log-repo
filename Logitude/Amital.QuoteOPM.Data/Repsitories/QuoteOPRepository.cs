 
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
using Simplog.Server.Infrastructure.Helpers;

namespace Amital.QuoteOPM.Data.Repsitories
{
   public partial class QuoteOPRepository:IRepository<QuoteOP>
   {
        
		public List<QuoteOP> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public IQueryable<QuoteOP> GetQuotes(int tenant)
        {
            return (from d in context.QuoteOPs.Include("Stage").Include("Rating") where d.Tenant == tenant select d);
        }
    }

}
   