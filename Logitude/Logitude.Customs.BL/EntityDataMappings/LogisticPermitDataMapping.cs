
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
   
   public partial class LogisticPermitDataMapping: IMapping<LogisticPermitPM, LogisticPermit>
   {

        public void CustomPMToPOCO(LogisticPermitPM entityPM, LogisticPermit entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
        }

        public void CustomPOCOToPM(LogisticPermitPM entityPM, LogisticPermit entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   