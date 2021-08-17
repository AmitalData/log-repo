 
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
   public partial class QuoteOPStageRepository:IRepository<QuoteOPStage>
   {
        
		public List<QuoteOPStage> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public QuoteOPStage GetSingleQuoteOPStage(string id, int tenant)
        {
            return (from a in context.QuoteOPStages.Include("UpdatedByUser").Include("UpdatedByUser.Contact") where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public QuoteOPStage GetSingleQuoteOPStageByCode(string code, int tenant)
        {
            return (from a in context.QuoteOPStages.Include("UpdatedByUser").Include("UpdatedByUser.Contact") where a.Code == code && a.Tenant == tenant select a).FirstOrDefault();
        }

        public string GetQuoteOPStageIdByCode(string code, int tenant)
        {
            string myResult = null;

            QuoteOPStage myQuoteOPStage = (from a in context.QuoteOPStages where a.Code == code && a.Tenant == tenant select a).FirstOrDefault();
            if (myQuoteOPStage != null)
            {
                myResult = myQuoteOPStage.Id;
            }

            return myResult;
        }

        public IQueryable<QuoteOPStage> GetQuoteOPStages(int tenant)
        {
            return (from a in context.QuoteOPStages.Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                    where a.Tenant == tenant
                    select a);
        }

        public IQueryable<QuoteOPStage> GetQuoteOPStagesForSignup(int tenant)
        {
            IQueryable<QuoteOPStage> iQueryable = (from a in context.QuoteOPStages
                                                 where a.Tenant == tenant
                                                 select a);

            return iQueryable;
        }

    }

}
   