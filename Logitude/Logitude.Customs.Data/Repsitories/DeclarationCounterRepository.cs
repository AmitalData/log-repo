 
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
   public partial class DeclarationCounterRepository:IRepository<DeclarationCounter>
   {
        
		public List<DeclarationCounter> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public DeclarationCounter GetSingleByCustomFileNo(string customFileNo, int tenant)
        {
            return (from a in context.DeclarationCounters
                    where a.CustomFileNo == customFileNo && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

    }

}
   