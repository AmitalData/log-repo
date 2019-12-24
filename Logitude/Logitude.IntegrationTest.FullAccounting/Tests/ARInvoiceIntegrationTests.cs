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

namespace Logitude.IntegrationTest.FullAccounting.Tests
{
    [TestClass]
    public class ARInvoiceIntegrationTests
    {

        [TestMethod]
        public async Task Post()
        {

                ARInvoicePM entityPM = GetNewARInvoice();
                HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "ARInvoices");
                ARInvoicePM ARInvoicePM = RestClientService.ParseResponse<ARInvoicePM>(response);
                Assert.AreEqual(entityPM.Id, ARInvoicePM.Id);
        }

        private ARInvoicePM GetNewARInvoice()
        {
            ARInvoicePM ARInvoicePM = new ARInvoicePM();
            ARInvoicePM.Tenant = IntegrationTestLoginParameters.Tenant;
            ARInvoicePM.InvoiceNumber = VariablesGenerater.GetUniqueIdByDate() ;
            ARInvoicePM.ARInvoiceTypeCode = "IN";
            ARInvoicePM.BillToId = "1-53459";
            ARInvoicePM.BillToPartnerTypeId = "CS";
            ARInvoicePM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARInvoicePM.InvoiceDate = DateTime.UtcNow;
            ARInvoicePM.DueDate = DateTime.UtcNow;
            ARInvoicePM.UpdateDate = DateTime.UtcNow;
            ARInvoicePM.UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARInvoicePM.IssuedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARInvoicePM.InvoiceCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            ARInvoicePM.ApprovedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARInvoicePM.LocalCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            ARInvoicePM.SubTotalInLocalCurrency = 400;
            ARInvoicePM.SubTotalInInvoiceCurrency = 400;
            ARInvoicePM.AmountInLocalCurrency = 400;
            ARInvoicePM.AmountInInvoiceCurrency = 400;
            ARInvoicePM.StatusCode = "AD";
            ARInvoicePM.StatusName = "Unpaid";
            ARInvoicePM.InvoiceCurrencyExchangeRate = 1;
            ARInvoicePM.PaymentTermId = FullAccountingVariables.PaymentTermCashId;
            ARInvoicePM.CreateDate = DateTime.UtcNow;
            ARInvoicePM.SearchFields = "12PMCS,AccountingCustomer2";
            ARInvoicePM.IsGeneralInvoice = true;
            ARInvoicePM.ProfitCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            ARInvoicePM.ProfitCurrencyExchangeRate =2;
            ARInvoicePM.AmountDueInLocalCurrency = 40000;
            ARInvoicePM.AmountDueInProfitCurrency = 2000;
            ARInvoicePM.BranchId = FullAccountingVariables.BranchMainOfficeId;
            ARInvoicePM.SetApproved = true;
            ARInvoicePM.TotalAmountForTaxReport = 4000;
            ARInvoicePM.TotaVatableAmountForTaxReport = 0;
            ARInvoicePM.TotalVAT = 0;
            ARInvoicePM.IsFullAccounting = true;
            ARInvoicePM.AmountDue = 4000;
            ARInvoicePM.InvoiceLines = new List<ARInvoiceLinePM>();
            ARInvoicePM.InvoiceLines.Add(new ARInvoiceLinePM {
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
                LineNumber=1,
                Quantity=20,
                UnitPrice=200,
                ViewOrder=0,
                ProfitCurrencyAmount=200,
                CreditAccount = FullAccountingVariables.GLAccountCustomer54l4CSPMId,
                Description = "Air Freight",
                LocalDescription = "Air Freight",
                GLAccountId = FullAccountingVariables.GLAccountCustomer54l4CSPMId,
                LineActionCode="1"
            });
            return ARInvoicePM;
        }
    }
}
