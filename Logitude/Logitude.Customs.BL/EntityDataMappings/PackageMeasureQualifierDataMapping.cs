
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
   
   public partial class PackageMeasureQualifierDataMapping: IMapping<PackageMeasureQualifierPM, PackageMeasureQualifier>
   {

        public void CustomPMToPOCO(PackageMeasureQualifierPM entityPM, PackageMeasureQualifier entityPOCO)
        {
         
        }

        public void CustomPOCOToPM(PackageMeasureQualifierPM entityPM, PackageMeasureQualifier entityPOCO)
        {
      
        }
   }


}
   