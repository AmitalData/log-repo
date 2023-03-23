 
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
   public partial class AddressCurrencyRepository:IRepository<AddressCurrency>
   {
        
		public List<AddressCurrency> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public void FastDelete(string addressId, int tenant)
        {
            (context as DbContextBase)
                   .DeleteWhere<AddressCurrency>(rec => rec.AddressId == addressId && rec.Tenant == tenant);
        }

    }

}
   