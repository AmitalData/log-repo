using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClientAddressUpdateService : EntityUpdateService<ClientAddress, ClientAddressPM, ClientPM>
    {
        protected override void OnCreating(ClientAddressPM entityPM, ClientPM entityParentPM)
        {
            if (entityParentPM != null)
            {
                entityPM.ClientId = entityParentPM.Id;
            }
            entityPM.AddressId = IdCounter.GetNumber("Customs.ClientAddress", entityPM.Tenant);     
        }

        protected override void UpdateComposition(ClientAddressPM entityPM)
        {
            ClientsAddressCommTypeUpdateService clientAddressCommunicationTypeUpdateService = new ClientsAddressCommTypeUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            clientAddressCommunicationTypeUpdateService.UpdateMulti(entityPM.ClientsAddressCommTypes, entityPM.DeletedClientsAddressCommTypes, entityPM, false);
            base.UpdateComposition(entityPM);
        }

    }
}
