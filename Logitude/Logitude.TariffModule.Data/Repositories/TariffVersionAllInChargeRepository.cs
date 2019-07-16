 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.TariffModule.Data.Repositories
{
   public partial class TariffVersionAllInChargeRepository:IRepository<TariffVersionAllInCharge>
   {
        
		public List<TariffVersionAllInCharge> GetMulti(EntityKeyFields entityKeys)
        {
            TariffVersionKeys myEntityKeys = entityKeys as TariffVersionKeys;
            return (from a in context.TariffVersionAllInCharges where a.TariffId == myEntityKeys.TariffId && a.Version == myEntityKeys.Version select a).ToList();
        }

   }

}
   