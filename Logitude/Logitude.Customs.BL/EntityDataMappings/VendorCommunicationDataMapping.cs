
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
   
   public partial class VendorCommunicationDataMapping: IMapping<VendorCommunicationPM, VendorCommunication>
   {

        public void CustomPMToPOCO(VendorCommunicationPM entityPM, VendorCommunication entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.VendorId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
               
                entityPOCO.VendorId = entityPM.VendorId;             
                entityPOCO.LineNumber = entityPM.LineNumber;               
                entityPOCO.Tenant = entityPM.Tenant;

            }
        }

        public void CustomPOCOToPM(VendorCommunicationPM entityPM, VendorCommunication entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.CommunicationTypeName);
            if (entityPOCO.CommunicationTypeCode != null)
            {
                CommunicationTypeQueryService CommunicationTypeQueryService = new CommunicationTypeQueryService(entityPOCO.Tenant);
                CommunicationTypePM communicationType = CommunicationTypeQueryService.GetSingle(entityPOCO.CommunicationTypeCode, false, true);
                entityPM.CommunicationTypeName = communicationType.LocalName;
            }
        }
   }


}
   