
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
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class GLAccountRecocileDataDataMapping: IMapping<GLAccountRecocileDataPM, GLAccountRecocileData>
   {

        public void CustomPMToPOCO(GLAccountRecocileDataPM entityPM, GLAccountRecocileData entityPOCO)
        {
            //throw new NotImplementedException();
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.AccountId = entityPM.AccountId;
                entityPOCO.Tenant = entityPM.Tenant;

            }
        }

        public void CustomPOCOToPM(GLAccountRecocileDataPM entityPM, GLAccountRecocileData entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   