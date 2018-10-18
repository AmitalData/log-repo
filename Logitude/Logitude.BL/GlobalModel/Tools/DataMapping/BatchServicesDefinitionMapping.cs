using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.Tools.DataMapping
{
   public  class BatchServicesDefinitionMapping
    {

       public static void MapEntity(BatchServicesDefinitionPM entityPM, BatchServicesDefinition poco, bool isNewState)
       {
           if (isNewState)
           {
               poco.Code = entityPM.Code;
           }

           //poco.NumberOfThreads = entityPM.NumberOfThreads;
           //poco.InActive = entityPM.InActive;
           poco.ClassName = entityPM.ClassName;
           poco.Parameter1 = entityPM.Parameter1;
           poco.Parameter2 = entityPM.Parameter2;
       }
    }
}
