
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
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class BankAccountDataMapping: IMapping<BankAccountPM, BankAccount>
   {

        public void CustomPMToPOCO(BankAccountPM entityPM, BankAccount entityPOCO)
        {
            BankAccountCustomDataMapping bankAccountCustomDataMapping = new BankAccountCustomDataMapping();
            bankAccountCustomDataMapping.PMToPOCO(entityPM, entityPOCO, this.CustomMappedPOCOProperties);
        }

        public void CustomPOCOToPM(BankAccountPM entityPM, BankAccount entityPOCO)
        {
            BankAccountCustomDataMapping bankAccountCustomDataMapping = new BankAccountCustomDataMapping();
            bankAccountCustomDataMapping.POCOToPM(entityPM, entityPOCO, this.CustomMappedPMProperties);
        }
   }


}
   