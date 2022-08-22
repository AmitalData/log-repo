
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class WidgetDataMapping: IMapping<WidgetPM, Widget>
   {

        public void CustomPMToPOCO(WidgetPM entityPM, Widget entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert && entityPOCO != null)
            {
                entityPOCO.Id = entityPM.Id;
            }
        }

        public void CustomPOCOToPM(WidgetPM entityPM, Widget entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   