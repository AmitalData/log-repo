 
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
   public partial class ClientAddressRepository:IRepository<ClientAddress>
   {
        
		public List<ClientAddress> GetMulti(EntityKeyFields entityKeys)
        {

            ClientKeys clientKeys = entityKeys as ClientKeys;

            return (from a in context.ClientAddresses
                    where a.ClientId == clientKeys.Id 
                    select a).ToList();
        }

        public bool CheckIfClientHasAddresses(string clientId, int tenant)
        {
            
            return (from a in context.ClientAddresses
                    where a.ClientId ==clientId && a.Tenant == tenant
                    select a).Any();
        }

    }

}
   