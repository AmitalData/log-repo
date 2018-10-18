using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClientsAddressCommTypeUpdateService : EntityUpdateService<ClientsAddressCommType, ClientsAddressCommTypePM, ClientAddressPM>
    {

        protected override void OnCreating(ClientsAddressCommTypePM entityPM, ClientAddressPM entityParentPM)
        {
            entityPM.ClientId = entityParentPM.ClientId;
            entityPM.AddressId = entityParentPM.AddressId;
            entityParentPM.LastLineNumber += 1;
            entityPM.Line = entityParentPM.LastLineNumber;

        }

    }
}
