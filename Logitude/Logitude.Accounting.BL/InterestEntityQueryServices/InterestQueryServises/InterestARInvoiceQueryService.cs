using Logitude.Accounting.Data.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.InterestEntityQueryServices.InterestQueryServises
{
    public class InterestARInvoiceQueryService : IInterestEntityQueryService
    {
        public InterestEntityResult GetInterestEntity(InterestTransactionList interestTransactionLists)
        {
            ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(interestTransactionLists.Tenant);
            ARInvoicePM aRInvoice = aRInvoiceQuery.GetSinglePMForInterest(interestTransactionLists.EntityId, interestTransactionLists.Tenant);
            InterestEntityResult result = new InterestEntityResult();

            if (aRInvoice!=null)
            {
                result.EntityId = aRInvoice.Id;
                result.EntityNumber = aRInvoice.InvoiceNumber;
                result.JournalId = aRInvoice.JournalId;
                result.JournalNumber = aRInvoice.JournalNumber;
                result.AccountCode = "2";
                result.EntityCode = "1";
                result.EntityType = "ARInvoice";
                result.EntityTypeCode = "IN";
            }
           

            return result;
        }
    }
}
