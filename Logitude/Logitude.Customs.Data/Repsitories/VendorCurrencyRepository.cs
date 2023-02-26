 
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
   public partial class VendorCurrencyRepository:IRepository<VendorCurrency>
   {
        
		public List<VendorCurrency> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
      

        public void FastDelete(string vendorId ,int tenant) {
            (context as DbContextBase)
                   .DeleteWhere<VendorCurrency>(rec => rec.VendorId == vendorId && rec.Tenant == tenant);
        }
        public List<VendorCurrency> GetVendorCurrencyByVendorId(int tenant, string vendorId)
        {

          return (from a in context.VendorCurrencies
                   where a.Tenant == tenant && a.VendorId == vendorId
                   select a).ToList();
 
           
        }
    }

}
   