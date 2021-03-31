
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
using Logitude.Server.Tools.Helpers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class BankAccountDataMapping: IMapping<BankAccountPM, BankAccount>
   {

        public void CustomPMToPOCO(BankAccountPM entityPM, BankAccount entityPOCO)
        {
            BankAccountCustomDataMapping bankAccountCustomDataMapping = new BankAccountCustomDataMapping();
            bankAccountCustomDataMapping.PMToPOCO(entityPM, entityPOCO, this.CustomMappedPOCOProperties);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(BankAccountPM entityPM, BankAccount entityPOCO)
        {
            BankAccountCustomDataMapping bankAccountCustomDataMapping = new BankAccountCustomDataMapping();
            bankAccountCustomDataMapping.POCOToPM(entityPM, entityPOCO, this.CustomMappedPMProperties);
        }

        private void BuildSearchFields(BankAccountPM entityPM, BankAccount entityPOCO, bool p)
        {
            string searchFields = "";

            if (!string.IsNullOrEmpty(entityPM.GLAccountId))
            {
                MethodHelper.AddToSearchFields(ref searchFields, entityPM.GLAccountNumber);
            }

            if (!string.IsNullOrEmpty(entityPM.TransferGLAcccountId))
            {
                MethodHelper.AddToSearchFields(ref searchFields, entityPM.TransferGLAcccountNumber);
            }

            if (!string.IsNullOrEmpty(entityPM.DeferredGLAccountId))
            {
                MethodHelper.AddToSearchFields(ref searchFields, entityPM.DeferedGLAccountNumber);
            }

            if (searchFields.Length > 1000)
            {
                searchFields = searchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = searchFields;
            entityPOCO.SearchFields = searchFields;
        }
    }


}
   