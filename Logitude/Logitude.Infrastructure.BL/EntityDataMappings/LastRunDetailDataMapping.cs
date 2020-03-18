
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
   
   public partial class LastRunDetailDataMapping: IMapping<LastRunDetailPM, LastRunDetail>
   {

        public void CustomPMToPOCO(LastRunDetailPM entityPM, LastRunDetail entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.LastRunByUserId = entityPM.LastRunByUserId;
            entityPOCO.LastRunDate = entityPM.LastRunDate;
        }

        public void CustomPOCOToPM(LastRunDetailPM entityPM, LastRunDetail entityPOCO)
        {
            entityPM.Id = entityPOCO.Id;
            entityPM.Tenant = entityPOCO.Tenant;
            entityPM.LastRunByUserId = entityPOCO.LastRunByUserId;
            entityPM.LastRunDate = entityPOCO.LastRunDate;
        }
   }


}
   