
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
using Logitude.Server.Tools.Helpers;

namespace Logitude.Accounting.BL.EntityDataMappings
{

    public partial class ARPaymentBankTranferDataMapping : IMapping<ARPaymentBankTranferPM, ARPaymentBankTranfer>
    {

        public void CustomPMToPOCO(ARPaymentBankTranferPM entityPM, ARPaymentBankTranfer entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(ARPaymentBankTranferPM entityPM, ARPaymentBankTranfer entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private static void BuildSearchFields(ARPaymentBankTranferPM entityPM, ARPaymentBankTranfer poco, bool isNewEntity)
        {
            string searchFields = "";

            if (!string.IsNullOrEmpty(entityPM.PaymentRef))
            {
                MethodHelper.AddToSearchFields(ref searchFields, entityPM.PaymentRef);
            }

            if (!string.IsNullOrEmpty(entityPM.LocalAmount.ToString()))
            {
                MethodHelper.AddToSearchFields(ref searchFields, entityPM.LocalAmount.ToString());
            }

            if (searchFields.Length > 1000)
            {
                searchFields = searchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = searchFields;
            poco.SearchFields = searchFields;
        }
    }

}
   