
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
   
   public partial class CustomsItemDetailsHistoryDataMapping: IMapping<CustomsItemDetailsHistoryPM, CustomsItemDetailsHistory>
   {
        public void CustomPMToPOCO(CustomsItemDetailsHistoryPM entityPM, CustomsItemDetailsHistory entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.ID = entityPM.ID;
                entityPOCO.CustomsItemID = entityPM.CustomsItemID;
            }
        }

        public void CustomPOCOToPM(CustomsItemDetailsHistoryPM entityPM, CustomsItemDetailsHistory entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   