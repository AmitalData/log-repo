
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
   
   public partial class InterestLastBatchServiceDataMapping: IMapping<InterestLastBatchServicePM, InterestLastBatchService>
   {

        public void CustomPMToPOCO(InterestLastBatchServicePM entityPM, InterestLastBatchService entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
            }
        }

        public void CustomPOCOToPM(InterestLastBatchServicePM entityPM, InterestLastBatchService entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   