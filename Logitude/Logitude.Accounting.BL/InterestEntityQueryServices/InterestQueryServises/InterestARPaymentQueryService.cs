using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.InterestEntityQueryServices.InterestQueryServises
{
    public class InterestARPaymentQueryService : IInterestEntityQueryService
    {
        public InterestEntityResult GetInterestEntity(string Id, int Tenant)
        {
            ARPaymentQuery aRPaymentQuery = new ARPaymentQuery();
            ARPaymentPM aRPaymentPM = aRPaymentQuery.GetSinglePM(Id, Tenant);
            InterestEntityResult result = new InterestEntityResult();
            result.EntityId = aRPaymentPM.Id;
            result.EntityNumber = aRPaymentPM.PaymentNo;
            result.JournalId = aRPaymentPM.JournalId;
            result.EntityCode = aRPaymentPM.JournalId;
            result.AccountCode = "3";
            result.EntityCode = "2";
            result.EntityType = "ARPayment";
            result.EntityTypeCode = "PY";

            return result;
        }
    }
}
