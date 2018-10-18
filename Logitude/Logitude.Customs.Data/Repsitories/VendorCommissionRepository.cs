 
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
   public partial class VendorCommissionRepository:IRepository<VendorCommission>
   {
        
		public List<VendorCommission> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public VendorCommission GetSingleVendor(string vendorId, string customerId, int tenant)
        {
            return (from a in context.VendorCommissions
                    where a.VendorId == vendorId && a.CustomerId == customerId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

   }

}
   