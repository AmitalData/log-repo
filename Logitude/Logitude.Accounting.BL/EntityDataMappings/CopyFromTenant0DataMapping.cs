
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
   
   public partial class CopyFromTenant0DataMapping: IMapping<CopyFromTenant0PM, CopyFromTenant0>
   {

        public void CustomPMToPOCO(CopyFromTenant0PM entityPM, CopyFromTenant0 entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CopyFromTenant0PM entityPM, CopyFromTenant0 entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   