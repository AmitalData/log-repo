
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.BL.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class DecConsAcceptanceDataMapping: IMapping<DecConsAcceptancePM, DecConsAcceptance>
   {

        public void CustomPMToPOCO(DecConsAcceptancePM entityPM, DecConsAcceptance entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            entityPOCO.DeclarationId = entityPM.DeclarationId;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConsignmentNumber);
            entityPOCO.ConsignmentNumber = entityPM.ConsignmentNumber;
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            entityPOCO.LineNumber = entityPM.LineNumber;
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            entityPOCO.Tenant = entityPM.Tenant;
        }

        public void CustomPOCOToPM(DecConsAcceptancePM entityPM, DecConsAcceptance entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   