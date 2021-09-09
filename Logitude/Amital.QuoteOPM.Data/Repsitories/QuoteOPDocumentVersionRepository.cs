 
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
   public partial class QuoteOPDocumentVersionRepository:IRepository<QuoteOPDocumentVersion>
   {
        
		public List<QuoteOPDocumentVersion> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public IQueryable<QuoteOPDocumentVersion> GetQuoteDocumentVersionsByQuoteId(string quoteId, int tenant)
        {
            return (from record in context.QuoteOPDocumentVersions where record.Tenant == tenant && record.QuoteOPId == quoteId select record);
        }
    }

}
   