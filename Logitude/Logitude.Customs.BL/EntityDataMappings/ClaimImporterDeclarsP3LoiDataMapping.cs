
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
   
   public partial class ClaimImporterDeclarsP3LoiDataMapping: IMapping<ClaimImporterDeclarsP3LoiPM, ClaimImporterDeclarsP3Loi>
   {

        public void CustomPMToPOCO(ClaimImporterDeclarsP3LoiPM entityPM, ClaimImporterDeclarsP3Loi entityPOCO)
        {
            //throw new NotImplementedException();
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ClaimId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CounterKey);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNo);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {

                entityPOCO.ClaimId = entityPM.ClaimId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CounterKey = entityPM.CounterKey;
                entityPOCO.LineNo = entityPM.LineNo;
            }
        }

        public void CustomPOCOToPM(ClaimImporterDeclarsP3LoiPM entityPM, ClaimImporterDeclarsP3Loi entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   