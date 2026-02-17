
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;

namespace Logitude.TariffModule.BL.EntityDataMappings
{
   
   public partial class TariffVersionAllInChargeDataMapping: IMapping<TariffVersionAllInChargePM, TariffVersionAllInCharge>
   {

        public void CustomPMToPOCO(TariffVersionAllInChargePM entityPM, TariffVersionAllInCharge entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.TariffId);

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.TariffId = entityPM.TariffId;
        }

        public void CustomPOCOToPM(TariffVersionAllInChargePM entityPM, TariffVersionAllInCharge entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   