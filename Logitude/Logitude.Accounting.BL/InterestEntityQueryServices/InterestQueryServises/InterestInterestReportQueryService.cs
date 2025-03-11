using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Def.EntityPMs;
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
    public class InterestInterestReportQueryService : IInterestEntityQueryService
    {
        public InterestEntityResult GetInterestEntity(InterestTransactionList interestTransactionLists)
        {
            InterestReportQueryService reportQuery = new InterestReportQueryService(interestTransactionLists.Tenant);
            InterestReportPM  reportPM = reportQuery.GetSinglePMForInterest(interestTransactionLists.EntityId, interestTransactionLists.Tenant);
            InterestEntityResult result = new InterestEntityResult();

            if (reportPM != null)
            {
                result.EntityId = reportPM.Id;
                result.EntityNumber = reportPM.ReportNumber;
                result.JournalId = null;
                result.JournalNumber = null;
                result.AccountCode = "11";
                result.EntityCode = "4";
                result.EntityType = "Interest Report";
                result.EntityTypeCode = InterestEntityTypeCodes.InterestReport;
                result.OriginalLines = new List<InterestEntityOriginalLineResult>();
                InterestEntityOriginalLineResult line = new InterestEntityOriginalLineResult();
                line.OriginalLineNumber = 1;
                line.Reference1 = reportPM.ReportNumber;
                line.Notes = null;
                result.OriginalLines.Add(line);            }

            return result;
        }
    }
}
