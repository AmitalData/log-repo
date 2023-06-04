
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
   
   public partial class StatusCodeDataMapping: IMapping<StatusCodePM, StatusCode>
   {

        public void CustomPMToPOCO(StatusCodePM entityPM, StatusCode entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
            }
        }

        public void CustomPOCOToPM(StatusCodePM entityPM, StatusCode entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   