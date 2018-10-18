
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ConsignmentInternalTransitionDataMapping: IMapping<ConsignmentInternalTransitionPM, ConsignmentInternalTransition>
   {

        public void CustomPMToPOCO(ConsignmentInternalTransitionPM entityPM, ConsignmentInternalTransition entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.ConsignmentNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                CustomMappedPOCOProperties.Add(POCOPropertyNames.SiteCode);
                entityPOCO.DeclarationId = entityPM.DeclarationId;              
                entityPOCO.ConsignmentNumber = entityPM.ConsignmentNumber;             
                entityPOCO.SiteCode = entityPM.SiteCode;              
                entityPOCO.Tenant = entityPM.Tenant;              
                entityPOCO.LineNumber = entityPM.LineNumber;

            }
        }

        public void CustomPOCOToPM(ConsignmentInternalTransitionPM entityPM, ConsignmentInternalTransition entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   