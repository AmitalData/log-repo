using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityQueryServices
{
   public partial class ClientAddressQueryService: EntityQueryService<ClientAddress,ClientAddressKeys,ClientAddressPM,ClientPM,ClientKeys>
    {

       public override void GetComposition(EntityKeyFields entityKeys, ClientAddressPM entityPM)
       {
           ICustomContext context = MainContext as CustomContext;
           ClientAddressKeys clientAddressKeys = entityKeys as ClientAddressKeys;
           ClientsAddressCommTypeQueryService communicationQueryService = new ClientsAddressCommTypeQueryService(context);
           entityPM.ClientsAddressCommTypes = communicationQueryService.GetMulti(clientAddressKeys, true);

           if (entityPM.ClientsAddressCommTypes.Count > 0)
           {
               entityPM.LastLineNumber = entityPM.ClientsAddressCommTypes.Max(m => m.Line);
           }

           base.GetComposition(entityKeys, entityPM);
       }

        public ClientAddressPM GetCityOfDeclarationByImporterID(string importerID, string addressTypeCode, int tenant)
        {
            var clientAddressPOCO = repository.GetCityOfDeclarationByImporterID(importerID, addressTypeCode, tenant);
            ClientAddressPM clientAddressPM = this.GetEntityPM(clientAddressPOCO);
            return clientAddressPM;
        }


    }
}

