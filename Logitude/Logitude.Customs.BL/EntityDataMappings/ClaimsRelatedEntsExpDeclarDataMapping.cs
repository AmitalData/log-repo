
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
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ClaimsRelatedEntsExpDeclarDataMapping: IMapping<ClaimsRelatedEntsExpDeclarPM, ClaimsRelatedEntsExpDeclar>
   {

        public void CustomPMToPOCO(ClaimsRelatedEntsExpDeclarPM entityPM, ClaimsRelatedEntsExpDeclar entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ClaimId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CounterKey);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ExportDeclarationNumber);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.ClaimId = entityPM.ClaimId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CounterKey = entityPM.CounterKey;
                entityPOCO.ExportDeclarationNumber = entityPM.ExportDeclarationNumber;
            }
        }

        public void CustomPOCOToPM(ClaimsRelatedEntsExpDeclarPM entityPM, ClaimsRelatedEntsExpDeclar entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   