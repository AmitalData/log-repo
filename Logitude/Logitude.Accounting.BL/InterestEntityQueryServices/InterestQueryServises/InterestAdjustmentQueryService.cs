using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.InterestEntityQueryServices.InterestQueryServises
{
    public class InterestAdjustmentQueryService : IInterestEntityQueryService
    {
        public InterestEntityResult GetInterestEntity(InterestTransactionList interestTransactionLists)
        {
            JournalQueryService journalQueryService = new JournalQueryService(interestTransactionLists.Tenant);
            JournalPM journalPM = journalQueryService.GetSinglePMForInterest(interestTransactionLists.EntityId, interestTransactionLists.Tenant);
            InterestEntityResult result = new InterestEntityResult();
            if (journalPM != null)
            {
                result.EntityId = journalPM.Id;
                result.EntityNumber = journalPM.JournalNumber;
                result.JournalId = journalPM.Id;
                result.JournalNumber = journalPM.JournalNumber;
                result.AccountCode = "1";
                result.EntityCode = "5";
                result.EntityType = "Adjustments";
                result.EntityTypeCode = InterestEntityTypeCodes.Adjustments;

                result.OriginalLines = new List<InterestEntityOriginalLineResult>();
                foreach (var item in journalPM.JournalLines)
                {
                    InterestEntityOriginalLineResult line = new InterestEntityOriginalLineResult();
                    line.OriginalLineNumber = item.Line;
                    line.Reference1 = item.Reference1;
                    line.Notes = item.Notes;
                    result.OriginalLines.Add(line);
                }
            }


            return result;
        }

    }

}
