using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Logitude.IntegrationTest.FullAccounting;
using Logitude.Server.Tools.Counters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.FullAccounting.Tests.ARInvoice
{
    [TestClass]
    public class CreateAPInvoice
    {

        [TestMethod]
        public async Task CreateAPInvoice_Post_Successful()
        {

                APInvoicePM entityPM = GetNewAPInvoice();
                HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "APInvoices");
                APInvoicePM APInvoicePM = RestClientService.ParseResponse<APInvoicePM>(response);
                Assert.IsNotNull(APInvoicePM);
                Assert.IsNotNull(APInvoicePM.Id);
        }

        private APInvoicePM GetNewAPInvoice()
        {
            APInvoicePM APInvoicePM = new APInvoicePM();
            APInvoicePM.Tenant = IntegrationTestLoginParameters.Tenant;
            APInvoicePM.InvoiceNumber = VariablesGenerater.GetUniqueIdByDate();
            APInvoicePM.IsSecured = false;
            APInvoicePM.InternalNumber = VariablesGenerater.GetRandomString(5);
            APInvoicePM.VendorId = FullAccountingVariables.VendorTestGlVendor1s5PMV2Id;
            APInvoicePM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            APInvoicePM.InvoiceDate = DateTime.UtcNow;
            APInvoicePM.PaymentTermId = FullAccountingVariables.PaymentTermCashId;
            APInvoicePM.DueDate = DateTime.UtcNow;
            APInvoicePM.InvoiceCurrencyExchangeRate = 1;
            APInvoicePM.UpdateDate = DateTime.UtcNow;
            APInvoicePM.UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            APInvoicePM.InvoiceCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            APInvoicePM.ApprovedByUserId = IntegrationTestLoginParameters.LoginUserId;
            APInvoicePM.LocalCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            APInvoicePM.SubTotalInLocalCurrency = 200;
            APInvoicePM.SubTotalInInvoiceCurrency = 200;
            APInvoicePM.AmountInLocalCurrency = 200;
            APInvoicePM.AmountInInvoiceCurrency = 200;
            APInvoicePM.StatusCode = "AD";
            APInvoicePM.StatusName = "Unpaid";
            APInvoicePM.VendorPartnerTypeId = "VD";
            APInvoicePM.AmountInProfitCurrency = 100;
            APInvoicePM.InvoiceExpectedAmount = 400;
            APInvoicePM.IsClosed = false;
            APInvoicePM.JournalNumber = "1230";
            APInvoicePM.CreateDate = DateTime.UtcNow;
            APInvoicePM.SearchFields = "1205,test";
            APInvoicePM.IsGeneralInvoice = true;
            if (!String.IsNullOrEmpty(FullAccountingVariables.AccountingCurrencyTenantId)) APInvoicePM.ProfitCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            APInvoicePM.ProfitCurrencyExchangeRate = 2;
            APInvoicePM.AmountDueInLocalCurrency = 1200;
            APInvoicePM.AmountDueInProfitCurrency = 600;
            APInvoicePM.BranchId = FullAccountingVariables.BranchMainOfficeId;
            APInvoicePM.SetApproved = true;
            APInvoicePM.VATNumber = "5145599";
            APInvoicePM.AccountingDate = DateTime.UtcNow;
            APInvoicePM.IsExternalEntity = false;
            APInvoicePM.IsGeneralInvoice = true;
            APInvoicePM.AmountDue = 200;
            APInvoicePM.InvoiceLines = new List<APInvoiceLinePM>();
            APInvoicePM.InvoiceLines.Add(GetNewAPInvoiceLine());
            APInvoicePM.TotalVATs = new List<APInvoiceTotalVATPM>();
            APInvoicePM.TotalVATs.Add(GetNewAPInvoiceTotalVATs());
            return APInvoicePM;
        }

        private APInvoiceLinePM GetNewAPInvoiceLine()
        {
            APInvoiceLinePM Line = new APInvoiceLinePM
            {
                Tenant = IntegrationTestLoginParameters.Tenant,
                ChargesTypeId = FullAccountingVariables.ChargeTypesAirFreightId,
                ForiegnCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId,
                ForiegnCurrencyAmount = 400,
                LocalCurrencyAmount = 400,
                InvoiceCurrencyAmount = 400,
                VatTypeId = FullAccountingVariables.VatEXEMPTId,
                VatPercentage = 0.0,
                VatTypeName = "Exempt",
                ForiegnExchangeRate = 1,
                LineNumber = 1,
                Quantity = 20,
                AmountTypeCode = "NEXP",
                ProfitCurrencyAmount = 200,
                ChargeTypeGLAccountId = FullAccountingVariables.GLAccountCustomer54l4CSPMId,
                Description = "Air Freight",
                LocalDescription = "Air Freight",

            };

            return Line;
        }

        private APInvoiceTotalVATPM GetNewAPInvoiceTotalVATs()
        {
            APInvoiceTotalVATPM Line = new APInvoiceTotalVATPM
            {
                Tenant = IntegrationTestLoginParameters.Tenant,
                VatTypeId = FullAccountingVariables.VatEXEMPTId,
                VatPercent = 0.0,
                VatTypeName = "Exempt",
                VatTypeCell = "Exempt (0%)",
                InvoiceCurrencyVatableAmount = 200,
                LocalVatableAmount = 200,
                ProfitVatableAmount = 100,
                LocalVATAmount = 0,
                ProfitCurrencyVATAmount = 0,
           

            };

            return Line;
        }
    }
}
