 
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
   public partial class ClientsPoaRepository:IRepository<ClientsPoa>
   {
        
		public List<ClientsPoa> GetMulti(EntityKeyFields entityKeys)
        {

            ClientKeys clientKeys = entityKeys as ClientKeys;

            return (from a in context.ClientsPoas
                    where a.ClientId == clientKeys.Id
                    select a).ToList();
        }

        public List<ClientsPoa> GetPoas(string authorizerExternalId, string authorizerPassportNumber, string poaID, int tenant)
        {
            return (from a in context.ClientsPoas
                    where (a.AuthorizerExternalId == authorizerExternalId || a.AuthorizerPassportNumber == authorizerPassportNumber) && a.PoaID == poaID
                    select a).ToList();
        }

    }

}
   