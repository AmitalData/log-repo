
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
   
   public partial class CB_AdditionRulesDetailsHistoryDataMapping: IMapping<CB_AdditionRulesDetailsHistoryPM, CB_AdditionRulesDetailsHistory>
   {

        public void CustomPMToPOCO(CB_AdditionRulesDetailsHistoryPM entityPM, CB_AdditionRulesDetailsHistory entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CB_AdditionRulesDetailsHistoryPM entityPM, CB_AdditionRulesDetailsHistory entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   