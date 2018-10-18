
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class BatchTaskExecutionStatusDataMapping: IMapping<BatchTaskExecutionStatusPM, BatchTaskExecutionStatus>
   {

        public void CustomPMToPOCO(BatchTaskExecutionStatusPM entityPM, BatchTaskExecutionStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(BatchTaskExecutionStatusPM entityPM, BatchTaskExecutionStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   