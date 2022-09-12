using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.Security;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.DataContracts;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.Helpers;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        public List<MoneyStatusClass> GetMoneyStatusForTenant(string type,int lastMonths,int lastDays, int tenant, int selectedIndex, int currencyindex)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            arInvoiceQuery = new ARInvoiceQuery(tenant);
            return arInvoiceQuery.GetMoneyStatusForTenant(type,lastMonths,lastDays, tenant, selectedIndex, currencyindex);
        }

        public List<MoneyStatusClass> GetMoneyOutStatusForTenant(int lastMonths,int lastDays, int tenant, int selectedIndex, int currencyindex)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            arInvoiceQuery = new ARInvoiceQuery(tenant);
            return arInvoiceQuery.GetMoneyOutStatusForTenant(lastMonths, lastDays, tenant, selectedIndex, currencyindex);
        }

        public List<AgingReportDashboardClass> GetAgingReportARInvioceData(int tenant, int index, string customerid)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRInvoiceRepository = new ARInvoiceRepository(tenant);

            IQueryable<AgingReportInvoiceDataView> reports = BranchPermitionsFilter.AddUserBranchRestrictionFilters<AgingReportInvoiceDataView>(new QueryOperations(), aRInvoiceRepository.GetAgingReportInvoiceDataView(tenant, index, customerid), tenant);

            return aRInvoiceRepository.GetAgingReportInvoiceData(index, reports);
        }

        public List<AgingReportDashboardClass> GetAgingReportAPInvioceData(int tenant, int index)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPInvoiceRepository = new APInvoiceRepository(tenant);

            IQueryable<APAgingReportDataView> reports = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APAgingReportDataView>(new QueryOperations(), aPInvoiceRepository.GetAgingReportAPInvoiceDataView(tenant, index).AsQueryable<APAgingReportDataView>(), tenant);



            return aPInvoiceRepository.GetAgingReportAPInvoiceData(tenant, index, reports).ToList();

            
        }

        public List<DebtorsClass> GetDebrotExposure(int tenant, int currencyIndex)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            arInvoiceQuery = new ARInvoiceQuery(tenant);
            return arInvoiceQuery.GetDebtorExposure(tenant, currencyIndex);
        }

        public List<DebtorsClass> GetDebrotExposureForGridControl(int tenant, int index, bool isBranchRestricted)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            arInvoiceQuery = new ARInvoiceQuery(tenant);
            return arInvoiceQuery.GetDebtorsExposureForGridControl(tenant, index, isBranchRestricted);
        }

        public List<CreditorsClass> GetCreditorExposure(int tenant, int index, bool isBranchRestricted)
        {
            apInvoiceQuery = new APInvoiceQuery(tenant);
            return apInvoiceQuery.GetDebtorsExposureForGridControl(tenant, index, isBranchRestricted);
        }

    }
}