
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
   
   public partial class UIMessageAdditionalDataMapping: IMapping<UIMessageAdditionalPM, UIMessageAdditional>
   {

        public void CustomPMToPOCO(UIMessageAdditionalPM entityPM, UIMessageAdditional entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(UIMessageAdditionalPM entityPM, UIMessageAdditional entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   