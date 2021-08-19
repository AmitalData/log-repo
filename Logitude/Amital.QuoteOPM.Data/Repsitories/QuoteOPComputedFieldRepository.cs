 
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
   public partial class QuoteOPComputedFieldRepository:IRepository<QuoteOPComputedField>
   {
        
		public List<QuoteOPComputedField> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public IQueryable<QuoteOPComputedField> GetQuoteComputedField(int tenant)
        {
            return context.QuoteOPComputedFields.Where(s => s.Tenant == tenant);
        }
        public IQueryable<QuoteOPComputedField> GetQuoteComputedField()
        {
            return context.QuoteOPComputedFields;
        }
        public QuoteOPComputedField GetSingleQuoteComputedField(string Id, int tenant)
        {
            if (!string.IsNullOrEmpty(Id))
            {

                var quoteComputedFieldEntity = (from quoteComputedField in context.QuoteOPComputedFields
                                                               where quoteComputedField.Id == Id && quoteComputedField.Tenant == tenant
                                                               select quoteComputedField).FirstOrDefault();
                return quoteComputedFieldEntity;
            }
            return null;
        }

        public QuoteOPComputedField GetSingleQuoteComputedField(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var quoteComputedFieldEntity = (from quoteComputedField in context.QuoteOPComputedFields
                                                               where quoteComputedField.Id == id
                                                               select quoteComputedField).FirstOrDefault();
                return quoteComputedFieldEntity;
            }
            return null;
        }
    }

}
   