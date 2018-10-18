 
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
   public partial class CustomsHouseTypeAdditionalRepository:IRepository<CustomsHouseTypeAdditional>
   {
        
		public List<CustomsHouseTypeAdditional> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public CustomsHouseTypeAdditional GetSingleAdditionalByCode(string code, int tenant)
        {
            return (from a in context.CustomsHouseTypeAdditionals
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

   }

}
   