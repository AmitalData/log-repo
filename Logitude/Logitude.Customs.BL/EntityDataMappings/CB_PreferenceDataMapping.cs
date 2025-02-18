
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
   
   public partial class CB_PreferenceDataMapping: IMapping<CB_PreferencePM, CB_Preference>
   {

        public void CustomPMToPOCO(CB_PreferencePM entityPM, CB_Preference entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.BackgroundColor = entityPM.BackgroundColor;
            entityPOCO.TextColor = entityPM.TextColor;
            entityPOCO.UserId = entityPM.UserId;
        }

        public void CustomPOCOToPM(CB_PreferencePM entityPM, CB_Preference entityPOCO)
        {
            entityPM.Id = entityPOCO.Id;
            entityPM.Tenant = entityPOCO.Tenant;
            entityPM.BackgroundColor = entityPOCO.BackgroundColor;
            entityPM.TextColor = entityPOCO.TextColor;
            entityPM.UserId = entityPOCO.UserId;
        }
    }


}
   