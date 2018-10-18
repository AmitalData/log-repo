
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class AutomaticExternalRconcilMthodDataMapping: IMapping<AutomaticExternalRconcilMthodPM, AutomaticExternalRconcilMthod>
   {

        public void CustomPMToPOCO(AutomaticExternalRconcilMthodPM entityPM, AutomaticExternalRconcilMthod entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(AutomaticExternalRconcilMthodPM entityPM, AutomaticExternalRconcilMthod entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   