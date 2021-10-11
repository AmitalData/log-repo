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

        public List<OpportunityCRMDetails> Build()
        {
            List<OpportunityCRMDetails> opportunities = new OpportunityQueryService(tenant).GetLogitudeOpportunities(tenant, logitudeCRMReportFilter);
            List<Card> resellers = GetResellersForIds(opportunities.Select(a => a.ResellerId).ToList());
            List<int> tenantManamgnetNumbers = opportunities.Select(b => (int.TryParse(b.TenantNumber, out int x) ? x : 0)).ToList();
            logitudeCRMReportTenantManagementService = new LogitudeCRMReportTenantManagementService(tenantManamgnetNumbers);

            foreach (OpportunityCRMDetails opportunityCRMDetails in opportunities)
            {
                FillCustomFields(resellers, opportunityCRMDetails);
                FillTenantManagementsFileds(opportunityCRMDetails);
            }
            return opportunities;
        }

        private void FillCustomFields(List<Card> resellers, OpportunityCRMDetails opportunityCRMDetails)
        {
            opportunityCRMDetails.Reseller = resellers.Where(a => a.Id == opportunityCRMDetails.ResellerId).FirstOrDefault()?.EnglishName;
            opportunityCRMDetails.Total = (decimal.TryParse(opportunityCRMDetails.Field4, out decimal x) ? x : 0);
        }

        private void FillTenantManagementsFileds(OpportunityCRMDetails opportunityCRMDetails)
        {
            TenantManagementPM tenantManagement = logitudeCRMReportTenantManagementService.GetByTenantNumber(opportunityCRMDetails.TenantNumber);
            if (tenantManagement != null)
            {
                opportunityCRMDetails.CurrencyCode = tenantManagement.PaymentCurrencyCode;
                opportunityCRMDetails.ResellerCommission = tenantManagement.ResellerCommission;
                opportunityCRMDetails.TenantManagementNumberOfUsers = opportunityCRMDetails.InActive ? 0 : logitudeCRMReportTenantManagementService.GetNumberOfUsers(tenantManagement);
                opportunityCRMDetails.TenantManagementTotalPrice = opportunityCRMDetails.InActive ? 0 : (decimal?)logitudeCRMReportTenantManagementService.GetTotalPrice(tenantManagement);
                opportunityCRMDetails.TenantManagementAveragePrice = opportunityCRMDetails.InActive ? 0 : CalculateAveragePrice(opportunityCRMDetails);
                opportunityCRMDetails.TotalNet = CalculateTotalNet(opportunityCRMDetails, tenantManagement);
            }
        }

        private decimal? CalculateAveragePrice(OpportunityCRMDetails opportunityCRMDetails)
        {
            if (opportunityCRMDetails.TenantManagementNumberOfUsers == null || opportunityCRMDetails.TenantManagementNumberOfUsers == 0)
            {
                return 0;
            }
            return opportunityCRMDetails.TenantManagementTotalPrice / opportunityCRMDetails.TenantManagementNumberOfUsers;
        }

        private decimal? CalculateTotalNet(OpportunityCRMDetails opportunityCRMDetails, TenantManagementPM tenantManagement)
        {
            if (logitudeCRMReportFilter.ShowNet)
            {
                return opportunityCRMDetails.Total * (100 - tenantManagement.ResellerCommission ?? 0) / 100;
            }
            return opportunityCRMDetails.NumberOfUsers * opportunityCRMDetails.Total;
        }

        private List<Card> GetResellersForIds(List<string> cardIds)
        {
            CardRepository cardRepository = new CardRepository(tenant);
            return cardRepository.GetCardsByIds(cardIds, tenant).ToList();
        }
    }
}