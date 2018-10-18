 
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
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomsPartnersItemRepository:IRepository<CustomsPartnersItem>
   {
        
		public List<CustomsPartnersItem> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<CustomsPartnersItem> GetItemsForCustomerOrVendor(string vendorId, string CustomerId, int Tenant)
        {
          
            return (from a in context.CustomsPartnersItems.Include("Client").Include("CustomsVendor")
                    where a.VendorId == vendorId && a.Tenant == Tenant
                        select a).ToList();
        }

        public CustomsPartnersItem GetSingleItemByCode(string itemCode, string vendorId, string CustomerId, int Tenant)
        {

            return (from a in context.CustomsPartnersItems.Include("Client").Include("CustomsVendor")
                    where a.VendorId == vendorId && a.ItemCode == itemCode && a.Tenant == Tenant
                    select a).FirstOrDefault();
        }

   }

}
   