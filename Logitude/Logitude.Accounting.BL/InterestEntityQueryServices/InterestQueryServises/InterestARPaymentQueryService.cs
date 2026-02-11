using Logitude.Accounting.BL.EntityQueryServiceExt;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Enums;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
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
        public InterestEntityResult GetInterestEntity(InterestTransactionList interestTransactionLists)
        {
            ARPaymentQuery aRPaymentQuery = new ARPaymentQuery(interestTransactionLists.Tenant);
            ARPaymentPM aRPaymentPM = aRPaymentQuery.GetSinglePMForInterest(interestTransactionLists);
            InterestEntityResult result = new InterestEntityResult();

            if (aRPaymentPM!=null)
            {
                ARPaymentChequeQueryServiceExt aRPaymentChequeQueryServiceExt = new ARPaymentChequeQueryServiceExt();
                var aRPaymentChequePMList = aRPaymentChequeQueryServiceExt.GetListByPaymentId(aRPaymentPM.Id, aRPaymentPM.Tenant);
                string chequesCSV = String.Empty;
                if (aRPaymentChequePMList != null && aRPaymentChequePMList.Any())
                {
                    var distinctCheques = aRPaymentChequePMList
                        .Select(x => x.ChequeNumber)
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Distinct()
                        .ToList();

                    if (distinctCheques.Count == 1)
                    {
                        chequesCSV = distinctCheques[0];
                    }
                    else if (distinctCheques.Count > 1)
                    {
                        chequesCSV = $"{distinctCheques[0]}, ...";
                    }
                }
                result.EntityId = aRPaymentPM.Id;
                result.EntityNumber = aRPaymentPM.PaymentNo;
                result.JournalId = aRPaymentPM.JournalId;
                result.JournalNumber = aRPaymentPM.JournalNumber;
                result.AccountCode = "3";
                result.EntityCode = "2";
                result.EntityType = "ARPayment";
                result.EntityTypeCode = InterestEntityTypeCodes.ARPayment;

                result.OriginalLines = new List<InterestEntityOriginalLineResult>();
                InterestEntityOriginalLineResult line = new InterestEntityOriginalLineResult();
                line.OriginalLineNumber = 1;
                line.Reference1 = aRPaymentPM.PaymentNo;
                line.Reference2 = chequesCSV;
                line.Notes = aRPaymentPM.PrintNotes;
                result.OriginalLines.Add(line);
            }
  
            return result;
        }
    }
}
