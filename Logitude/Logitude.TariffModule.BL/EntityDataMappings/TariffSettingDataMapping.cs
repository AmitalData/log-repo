
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
   
   public partial class TariffSettingDataMapping: IMapping<TariffSettingPM, TariffSetting>
   {

        public void CustomPMToPOCO(TariffSettingPM entityPM, TariffSetting entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(TariffSettingPM entityPM, TariffSetting entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   