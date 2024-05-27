
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
   
   public partial class CB_CustomsItemComputedDataDataMapping: IMapping<CB_CustomsItemComputedDataPM, CB_CustomsItemComputedData>
   {

        public void CustomPMToPOCO(CB_CustomsItemComputedDataPM entityPM, CB_CustomsItemComputedData entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CB_CustomsItemComputedDataPM entityPM, CB_CustomsItemComputedData entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   