
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
   
   public partial class ClaimImporterDeclarsPage3BDataMapping: IMapping<ClaimImporterDeclarsPage3BPM, ClaimImporterDeclarsPage3B>
   {

        public void CustomPMToPOCO(ClaimImporterDeclarsPage3BPM entityPM, ClaimImporterDeclarsPage3B entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ClaimId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNo);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.ClaimId = entityPM.ClaimId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.LineNo = entityPM.LineNo;
            }
        }

        public void CustomPOCOToPM(ClaimImporterDeclarsPage3BPM entityPM, ClaimImporterDeclarsPage3B entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   