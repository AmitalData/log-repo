using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityLists;
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
            InterestReportQueryService ReportQuery = new InterestReportQueryService(interestTransactionLists.Tenant);
            InterestReportPM  ReportPM = ReportQuery.GetSinglePMForInterest(interestTransactionLists.EntityId, interestTransactionLists.Tenant);
            InterestEntityResult result = new InterestEntityResult();

            if (ReportPM != null)
            {
                result.EntityId = ReportPM.Id;
                result.EntityNumber = ReportPM.ReportNumber;
                result.JournalId = null;
                result.JournalNumber = null;
                result.AccountCode = "11";
                result.EntityCode = "4";
                result.EntityType = "Interest Report";
                result.EntityTypeCode = "IR";
            }
           

            return result;
        }
    }
}
