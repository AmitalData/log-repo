using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportLinesCreationService
    {
        public void CreateInterestReportLines(List<InterestTransactionPM> interestTransactionPMs, string interestReportId, int tenant)
        {
            InterestReportLineRepository interestReportLineRepository = new InterestReportLineRepository(tenant);
            IEnumerable<List<InterestTransactionPM>> listOfMultiEntityUpdateDataEntities = SplitListIntoNList(interestTransactionPMs, 5000);
            listOfMultiEntityUpdateDataEntities.ToList().ForEach(entities =>
            {
                foreach (var entityPM in entities)
                {
                    InterestReportLine interestReportLineEntity = new InterestReportLine()
                    {
                        InterestReportId = interestReportId,
                        InterestTransactionId = entityPM.Id,
                        Tenant = tenant,
                    };
                    interestReportLineRepository.Add(interestReportLineEntity);
                };
                interestReportLineRepository.SubmitChanges();
            });
        }

        public IEnumerable<List<T>> SplitListIntoNList<T>(List<T> fullList, int nSize)

        {
            for (int i = 0; i < fullList.Count; i += nSize)
            {
                yield return fullList.GetRange(i, Math.Min(nSize, fullList.Count - i)).ToList();
            }
        }
    }
}
