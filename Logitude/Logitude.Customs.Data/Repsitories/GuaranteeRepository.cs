 
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
   public partial class GuaranteeRepository:IRepository<Guarantee>
   {
        
		public List<Guarantee> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public Guarantee GetGuaranteeByTapagId(string tapagId, int tenant)
        {
           return (from a in context.Guarantees 
                  where a.TapagID == tapagId && a.Tenant == tenant select a).FirstOrDefault();
        }


   }

}
   