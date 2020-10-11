
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
   
   public partial class CustomsShipDataMapping: IMapping<CustomsShipPM, CustomsShip>
   {

        public void CustomPMToPOCO(CustomsShipPM entityPM, CustomsShip entityPOCO)
        {
            //throw new NotImplementedException();
            AddPOCOPropertyName(POCOPropertyNames.Code);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Code = entityPM.Code ;
            }
        }

        public void CustomPOCOToPM(CustomsShipPM entityPM, CustomsShip entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   