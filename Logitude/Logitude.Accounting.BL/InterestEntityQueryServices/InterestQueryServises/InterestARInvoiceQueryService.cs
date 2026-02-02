using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Enums;
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
                result.EntityTypeCode = InterestEntityTypeCodes.ARInvoice;

                int count = 0;
                result.OriginalLines = new List<InterestEntityOriginalLineResult>();

                result.OriginalLines.AddRange(aRInvoice.InvoiceLines.Select(item => new InterestEntityOriginalLineResult
                {
                    OriginalLineNumber = item.LineNumber,
                    Reference1 = aRInvoice.InvoiceNumber,
                    Reference2 = aRInvoice.MainEntityReference,
                    Notes = item.Description
                }).ToList());

                count = aRInvoice.InvoiceLines.Count;

                result.OriginalLines.AddRange(aRInvoice.TotalVATs
                .Where(vt => vt.VATPercent > 0)
                .Select(vt => new InterestEntityOriginalLineResult
                {
                    OriginalLineNumber = ++count,
                    Reference1 = aRInvoice.InvoiceNumber,
                    Reference2 = aRInvoice.MainEntityReference,
                    Notes = $"VAT {vt.VATPercent}%"
                }));
            }
           

            return result;
        }
    }
}
