
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
   
   public partial class RevenueExpenseTypeDataMapping: IMapping<RevenueExpenseTypePM, RevenueExpenseType>
   {

        public void CustomPMToPOCO(RevenueExpenseTypePM entityPM, RevenueExpenseType entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(RevenueExpenseTypePM entityPM, RevenueExpenseType entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   