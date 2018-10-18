
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
   
   public partial class AuthorizedSignerPermitDataMapping: IMapping<AuthorizedSignerPermitPM, AuthorizedSignerPermit>
   {

        public void CustomPMToPOCO(AuthorizedSignerPermitPM entityPM, AuthorizedSignerPermit entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(AuthorizedSignerPermitPM entityPM, AuthorizedSignerPermit entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   