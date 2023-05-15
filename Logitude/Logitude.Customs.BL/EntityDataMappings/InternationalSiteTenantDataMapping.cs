
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
   
   public partial class InternationalSiteTenantDataMapping: IMapping<InternationalSiteTenantPM, InternationalSiteTenant>
   {

        public void CustomPMToPOCO(InternationalSiteTenantPM entityPM, InternationalSiteTenant entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(InternationalSiteTenantPM entityPM, InternationalSiteTenant entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   