
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
   
   public partial class WithholdingTaxDeductionTypeDataMapping: IMapping<WithholdingTaxDeductionTypePM, WithholdingTaxDeductionType>
   {

        public void CustomPMToPOCO(WithholdingTaxDeductionTypePM entityPM, WithholdingTaxDeductionType entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(WithholdingTaxDeductionTypePM entityPM, WithholdingTaxDeductionType entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   