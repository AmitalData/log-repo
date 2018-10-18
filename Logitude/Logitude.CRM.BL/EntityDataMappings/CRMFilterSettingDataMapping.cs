
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class CRMFilterSettingDataMapping: IMapping<CRMFilterSettingPM, CRMFilterSetting>
   {

        public void CustomPMToPOCO(CRMFilterSettingPM entityPM, CRMFilterSetting entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CRMFilterSettingPM entityPM, CRMFilterSetting entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   