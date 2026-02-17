
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
   
   public partial class GLAccountWithholdingTaxDataMapping: IMapping<GLAccountWithholdingTaxPM, GLAccountWithholdingTax>
   {

        public void CustomPMToPOCO(GLAccountWithholdingTaxPM entityPM, GLAccountWithholdingTax entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.GLAccountId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.Id);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.GLAccountId = entityPM.GLAccountId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Id = entityPM.Id;

            }
        }


        public void CustomPOCOToPM(GLAccountWithholdingTaxPM entityPM, GLAccountWithholdingTax entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   