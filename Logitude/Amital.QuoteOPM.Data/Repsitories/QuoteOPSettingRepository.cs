 
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
   public partial class QuoteOPSettingRepository:IRepository<QuoteOPSetting>
   {
        
		public List<QuoteOPSetting> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public QuoteOPSetting GetSingleQuoteSetting(string id)
        {
            return (from a in context.QuoteOPSettings where a.Id == id select a).FirstOrDefault();
        }

        public QuoteOPSetting GetSingleQuoteSetting(int tenant)
        {
            return (from a in context.QuoteOPSettings where a.Tenant == tenant select a).FirstOrDefault();
        }


    }

}
   