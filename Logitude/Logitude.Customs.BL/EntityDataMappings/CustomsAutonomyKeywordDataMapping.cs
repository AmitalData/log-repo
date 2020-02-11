
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
   
   public partial class CustomsAutonomyKeywordDataMapping: IMapping<CustomsAutonomyKeywordPM, CustomsAutonomyKeyword>
   {

        public void CustomPMToPOCO(CustomsAutonomyKeywordPM entityPM, CustomsAutonomyKeyword entityPOCO)
        {
            //throw new NotImplementedException();
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            

            }

        }

        public void CustomPOCOToPM(CustomsAutonomyKeywordPM entityPM, CustomsAutonomyKeyword entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   