
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
   
   public partial class TradeLevyStatusDataMapping: IMapping<TradeLevyStatusPM, TradeLevyStatus>
   {

        public void CustomPMToPOCO(TradeLevyStatusPM entityPM, TradeLevyStatus entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                entityPOCO.Code = entityPM.Code;
        }

        public void CustomPOCOToPM(TradeLevyStatusPM entityPM, TradeLevyStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   