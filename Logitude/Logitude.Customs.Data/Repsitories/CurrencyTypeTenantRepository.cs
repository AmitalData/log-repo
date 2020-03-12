 
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
   public partial class CurrencyTypeTenantRepository:IRepository<CurrencyTypeTenant>
   {
        
		public List<CurrencyTypeTenant> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public CurrencyTypeTenant GetPMByCode(int tenant, string code)
        {
            return this.GetAll(tenant).FirstOrDefault(r => r.Code == code);
        }
    }

}
   