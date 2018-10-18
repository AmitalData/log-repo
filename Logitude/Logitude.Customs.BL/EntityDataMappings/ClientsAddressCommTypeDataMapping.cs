
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ClientsAddressCommTypeDataMapping: IMapping<ClientsAddressCommTypePM, ClientsAddressCommType>
   {

        public void CustomPMToPOCO(ClientsAddressCommTypePM entityPM, ClientsAddressCommType entityPOCO)
        {

            CustomMappedPOCOProperties.Add(POCOPropertyNames.ClientId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.AddressId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Line);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
              
                entityPOCO.ClientId = entityPM.ClientId;           
                entityPOCO.AddressId = entityPM.AddressId;
                entityPOCO.Tenant = entityPM.Tenant;          
                entityPOCO.Line = entityPM.Line;



            }
        }

        public void CustomPOCOToPM(ClientsAddressCommTypePM entityPM, ClientsAddressCommType entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CommunicationTypeName);

            if (entityPOCO.CommunicationTypeCode != null)
            {
                CommunicationTypeQueryService communicationTypeQueryService = new CommunicationTypeQueryService(entityPOCO.Tenant);
                CommunicationTypePM communicationType = communicationTypeQueryService.GetSingle(entityPOCO.CommunicationTypeCode, false, true);
                entityPM.CommunicationTypeName = communicationType.LocalName;
            }

        }
   }


}
   