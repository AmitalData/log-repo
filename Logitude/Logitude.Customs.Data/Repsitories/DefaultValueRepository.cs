 
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
   public partial class DefaultValueRepository:IRepository<DefaultValue>
   {
        
		public List<DefaultValue> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public DefaultValue GetSingleByDefaultTypeId(string defTypeId, int tenant)
        {
            return (from a in context.DefaultValues
                    where a.DefaultTypeId == defTypeId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

    }

}
   