using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
     public partial class MeasurmentUnitQueryService
    {
         public MeasurmentUnitPM GetMeasurmentUnitByMalamId
            //(int? malamId)
            (int malamId)
        {
             MeasurmentUnitPM pm = null;
             var poco = repository.GetMeasurementUnitByMalamId(malamId);

             if (poco != null)
             {

                 pm = this.GetEntityPM(poco);
             }
             return pm;
         }


    }
}
