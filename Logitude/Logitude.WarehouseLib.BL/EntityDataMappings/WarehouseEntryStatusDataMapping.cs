
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.EntityPMs; 
using Logitude.WarehouseLib.Data;

namespace Logitude.WarehouseLib.BL.EntityDataMappings
{
   
   public partial class WarehouseEntryStatusDataMapping: IMapping<WarehouseEntryStatusPM, WarehouseEntryStatus>
   {

        public void CustomPMToPOCO(WarehouseEntryStatusPM entityPM, WarehouseEntryStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(WarehouseEntryStatusPM entityPM, WarehouseEntryStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   