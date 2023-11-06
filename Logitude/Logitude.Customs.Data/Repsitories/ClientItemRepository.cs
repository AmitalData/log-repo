 
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
   public partial class ClientItemRepository:IRepository<ClientItem>
   {
        
		public List<ClientItem> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public ClientItem GetSingleWithTenant(string itemcode, string clientcode, int tenant)
        {
            return (from a in context.ClientItems
                    where a.ItemCode == itemcode && a.ClientCode == clientcode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

    }

}
   