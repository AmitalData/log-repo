 
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
   public partial class ClientsAddressCommTypeRepository:IRepository<ClientsAddressCommType>
   {
        
		public List<ClientsAddressCommType> GetMulti(EntityKeyFields entityKeys)
        {

            ClientAddressKeys clientAddressKeys = entityKeys as ClientAddressKeys;

            return (from a in context.ClientsAddressCommTypes
                    where a.ClientId == clientAddressKeys.ClientId && a.AddressId == clientAddressKeys.AddressId
                    select a).ToList();
        }

   }

}
   