
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
   
   public partial class SharedLogisticsSettingDataMapping: IMapping<SharedLogisticsSettingPM, SharedLogisticsSetting>
   {

        public void CustomPMToPOCO(SharedLogisticsSettingPM entityPM, SharedLogisticsSetting entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.Id == null)
            {
                entityPM.Id = entityPM.Tenant.ToString();
            }

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
        }

        public void CustomPOCOToPM(SharedLogisticsSettingPM entityPM, SharedLogisticsSetting entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   