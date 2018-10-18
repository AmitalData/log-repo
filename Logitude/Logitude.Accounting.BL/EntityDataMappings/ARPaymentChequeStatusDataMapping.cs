
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
   
   public partial class ARPaymentChequeStatusDataMapping: IMapping<ARPaymentChequeStatusPM, ARPaymentChequeStatus>
   {

        public void CustomPMToPOCO(ARPaymentChequeStatusPM entityPM, ARPaymentChequeStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ARPaymentChequeStatusPM entityPM, ARPaymentChequeStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   