 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class InternationalSiteRepository:IRepository<InternationalSite>
   {
        
		public List<InternationalSite> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public InternationalSite GetSingleInternationalSiteByCode(string code)
        {
            return (from a in context.InternationalSites
                    where a.Code == code
                    select a).FirstOrDefault();
        }
    }

}
   