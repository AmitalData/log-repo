using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class ARPaymentBankTranferQueryService : EntityQueryService<ARPaymentBankTranfer, ARPaymentBankTranferKeys, ARPaymentBankTranferPM, object, ARPaymentBankTranferKeys>
    {

        public ARPaymentBankTranferPM GetSingleARPaymentBankTranferByPaymentId(string paymentId, int tenant)
        {
            ARPaymentBankTranfer  arPaymentBankTranfer = this.repository.GetSingleARPaymentBankTranferByPaymentId(paymentId, tenant);


            if (arPaymentBankTranfer != null)
            {
                EntityPM = new ARPaymentBankTranferPM();
                mapping.CustomPOCOToPM(EntityPM, arPaymentBankTranfer);
                mapping.POCOToPM(EntityPM, arPaymentBankTranfer);
            }

            return EntityPM;
        }

    }
}
