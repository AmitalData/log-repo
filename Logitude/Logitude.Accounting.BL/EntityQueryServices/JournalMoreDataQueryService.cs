using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial class JournalMoreDataQueryService
    {

        public JournalMoreDataPM GetSingleJournalMorData(string journalId, int tenant)
        {
            JournalMoreDataPM entityPM = null;
            JournalMoreData poco = repository.GetSingleJournalMoreData(journalId, tenant);
            entityPM = new JournalMoreDataPM()
            {
                JournalId = poco.JournalId,
                Line = poco.Line,
                Tenant = poco.Tenant,
                GeneralData = poco.GeneralData,
                IsLedgerCreated = poco.IsLedgerCreated


            };
                

            return entityPM;
        }
    }
}
