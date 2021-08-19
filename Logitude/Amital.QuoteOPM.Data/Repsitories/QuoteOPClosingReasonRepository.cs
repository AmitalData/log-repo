 
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
   public partial class QuoteOPClosingReasonRepository:IRepository<QuoteOPClosingReason>
   {
        
		public List<QuoteOPClosingReason> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public IQueryable<QuoteOPClosingReason> GetQuoteOPClosingReasons(int tenant)
        {
            return (from a in context.QuoteOPClosingReasons.Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                    where a.Tenant == tenant
                    select a);
        }
        public QuoteOPClosingReason GetSingleQuoteOPClosingReason(string id, int tenant)
        {
            return (from a in context.QuoteOPClosingReasons.Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public QuoteOPClosingReason GetSingleQuoteOPClosingReasonByCode(string code, int tenant)
        {
            return (from a in context.QuoteOPClosingReasons
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
    }

}
   