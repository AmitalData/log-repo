
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class BankChequeDataMapping: IMapping<BankChequePM, BankCheque>
   {

        public void CustomPMToPOCO(BankChequePM entityPM, BankCheque entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(BankChequePM entityPM, BankCheque entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   