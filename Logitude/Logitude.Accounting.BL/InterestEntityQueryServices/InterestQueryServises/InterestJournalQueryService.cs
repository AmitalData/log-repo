using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.InterestEntityQueryServices.InterestQueryServises
{
    public class InterestJournalQueryService : IInterestEntityQueryService
    {
        public InterestEntityResult GetInterestEntity(string Id, int Tenant)
        {
            JournalQueryService journalQueryService = new JournalQueryService(Tenant);
            JournalPM journalPM = journalQueryService.GetSinglePM(Id, Tenant);
            InterestEntityResult result = new InterestEntityResult();
            result.EntityId = journalPM.Id;
            result.EntityNumber = journalPM.JournalNumber;
            result.JournalId = journalPM.Id;
            result.JournalNumber = journalPM.JournalNumber;
            result.AccountCode = "1";
            result.EntityCode = "3";
            result.EntityType = "Journal";
            result.EntityTypeCode = "JR";

            return result;
        }
    }
}
