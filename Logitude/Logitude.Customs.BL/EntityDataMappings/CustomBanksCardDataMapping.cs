
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
   
   public partial class CustomBanksCardDataMapping: IMapping<CustomBanksCardPM, CustomBanksCard>
   {

        public void CustomPMToPOCO(CustomBanksCardPM entityPM, CustomBanksCard entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.CustomBankId);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CustomBankId = entityPM.CustomBankId;

            }
        }

        public void CustomPOCOToPM(CustomBanksCardPM entityPM, CustomBanksCard entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   