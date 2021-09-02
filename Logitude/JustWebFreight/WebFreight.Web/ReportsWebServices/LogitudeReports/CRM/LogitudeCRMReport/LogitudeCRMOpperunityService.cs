using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.CRM.BL.DataContracts;
using Logitude.CRM.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.CRM.LogitudeCRMReport
{
    public class LogitudeCRMOpperunityService
    {
        private readonly int tenant;
        private readonly LogitudeCRMReportFilter logitudeCRMReportFilter;
        private LogitudeCRMReportTenantManagementService logitudeCRMReportTenantManagementService;

        public LogitudeCRMOpperunityService(int tenant, LogitudeCRMReportFilter logitudeCRMReportFilter)
        {
            this.tenant = tenant;
            this.logitudeCRMReportFilter = logitudeCRMReportFilter;
        }

        public List<OpportunityDetails> Build()
        {
            List<OpportunityDetails> opportunities = new OpportunityQueryService(tenant).GetLogitudeOpportunities(tenant, logitudeCRMReportFilter);
            List<Card> resellers = GetResellersForIds(opportunities.Select(a => a.ResellerId).ToList());
            List<int> tenantManamgnetNumbers = opportunities.Select(b => (int.TryParse(b.TenantNumber, out int x) ? x : 0)).ToList();
            logitudeCRMReportTenantManagementService = new LogitudeCRMReportTenantManagementService(tenantManamgnetNumbers);

            foreach (OpportunityDetails opportunity in opportunities)
            {
                FillCustomFields(resellers, opportunity);
                FillTenantManagementsFileds(opportunity);
            }
            return opportunities;
        }

        private void FillCustomFields(List<Card> resellers, OpportunityDetails opportunity)
        {
            opportunity.Reseller = resellers.Where(a => a.Id == opportunity.ResellerId).FirstOrDefault()?.EnglishName;
            opportunity.Total = (decimal.TryParse(opportunity.Field4, out decimal x) ? x : 0);
        }

        private void FillTenantManagementsFileds(OpportunityDetails opportunity)
        {
            TenantManagementPM tenantManagement = logitudeCRMReportTenantManagementService.GetByTenantNumber(opportunity.TenantNumber);
            if (tenantManagement != null)
            {
                opportunity.CurrencyCode = tenantManagement.PaymentCurrencyCode;
                opportunity.ResellerCommission = tenantManagement.ResellerCommission;
                opportunity.TenantManagementNumberOfUsers = logitudeCRMReportTenantManagementService.GetNumberOfUsers(tenantManagement);
                opportunity.TenantManagementTotalPrice = (decimal?)logitudeCRMReportTenantManagementService.GetTotalPrice(tenantManagement);
                opportunity.TenantManagementAveragePrice = (opportunity.TenantManagementNumberOfUsers == null || opportunity.TenantManagementNumberOfUsers == 0) ? 0 : opportunity.TenantManagementTotalPrice / opportunity.TenantManagementNumberOfUsers;
                opportunity.TotalNet = opportunity.Total * (100 - tenantManagement.ResellerCommission ?? 0) / 100;
            }
        }

        private List<Card> GetResellersForIds(List<string> cardIds)
        {
            CardRepository cardRepository = new CardRepository(tenant);
            return cardRepository.GetCardsByIds(cardIds, tenant).ToList();
        }
    }
}