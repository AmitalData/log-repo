
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
   
   public partial class DashboardDataMapping: IMapping<DashboardPM, Dashboard>
   {

        public void CustomPMToPOCO(DashboardPM entityPM, Dashboard entityPOCO)
        {
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
            entityPOCO.Description = entityPM.Description;
            entityPOCO.Id = entityPM.Id;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.SearchFields = entityPM.SearchFields;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.Tenant = entityPM.Tenant;

        }

        public void CustomPOCOToPM(DashboardPM entityPM, Dashboard entityPOCO)
        {
            entityPM.CreateDate = entityPOCO.CreateDate;
            entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            entityPM.Description = entityPOCO.Description;
            entityPM.Id = entityPOCO.Id;
            entityPM.Name = entityPOCO.Name;
            entityPM.SearchFields = entityPOCO.SearchFields;
            entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            entityPM.UpdateDate = entityPOCO.UpdateDate;
            entityPM.Tenant = entityPOCO.Tenant;
        }
   }


}
   