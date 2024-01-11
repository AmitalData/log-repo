
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
   
   public partial class EntryExitTypeDataMapping: IMapping<EntryExitTypePM, EntryExitType>
   {

        public void CustomPMToPOCO(EntryExitTypePM entityPM, EntryExitType entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                entityPOCO.Code = entityPM.Code;
        }

        public void CustomPOCOToPM(EntryExitTypePM entityPM, EntryExitType entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   