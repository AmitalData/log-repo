
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class ExternalPageAdditionalDataDataMapping: IMapping<ExternalPageAdditionalDataPM, ExternalPageAdditionalData>
   {

        public void CustomPMToPOCO(ExternalPageAdditionalDataPM entityPM, ExternalPageAdditionalData entityPOCO)
        {
            entityPOCO.ObjectTableId = entityPM.ObjectTableId;
            entityPOCO.EntityId = entityPM.EntityId;


        }

        public void CustomPOCOToPM(ExternalPageAdditionalDataPM entityPM, ExternalPageAdditionalData entityPOCO)
        {

        }
   }


}
   