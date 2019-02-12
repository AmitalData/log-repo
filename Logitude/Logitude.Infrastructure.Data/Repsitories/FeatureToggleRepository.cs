 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class FeatureToggleRepository:IRepository<FeatureToggle>
   {
        public IQueryable<FeatureToggle> GetAllByToggleCodeList(List<string> toggleCodes, int tenant)
        {
            return from a in context.FeatureToggles
                   where a.Tenant == tenant && toggleCodes.Contains(a.ToggleCode)
                   select a;
        }

        public List<FeatureToggle> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

   }

}
   