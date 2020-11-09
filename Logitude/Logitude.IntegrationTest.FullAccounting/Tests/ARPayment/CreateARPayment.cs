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
    public class CreateARPayment
    {

        [TestMethod]
        public async Task CreateARPayment_Post_Successful()
        {

                ARPaymentPM entityPM = GetNewARPayment();
                HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "ARPayments");
                ARPaymentPM ARPaymentPM = RestClientService.ParseResponse<ARPaymentPM>(response);
                Assert.IsNotNull(ARPaymentPM);
                Assert.IsNotNull(ARPaymentPM.Id);
        }

        private static ARPaymentPM GetNewARPayment()
        {
            ARPaymentPM ARPaymentPM = new ARPaymentPM();
            ARPaymentPM.Tenant = IntegrationTestLoginParameters.Tenant;
            ARPaymentPM.InvoiceNumber = VariablesGenerater.GetUniqueIdByDate();
            ARPaymentPM.PaymentNo = VariablesGenerater.GetUniqueIdByDate();
            ARPaymentPM.IsSecured = false;
            ARPaymentPM.BillToId = FullAccountingVariables.CustomerTestGlCust12PMCSId;
            ARPaymentPM.BillToPartnerTypeId = "CS";
            ARPaymentPM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARPaymentPM.BranchId = FullAccountingVariables.BranchCashBookBK14Id;
            ARPaymentPM.PaymentCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            ARPaymentPM.UpdateDate = DateTime.UtcNow;
            ARPaymentPM.UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARPaymentPM.AccountingPaymentMethodId = FullAccountingVariables.PaymentMethodCashId;
            ARPaymentPM.AccountingPaymentMethodCode = "CA";
            ARPaymentPM.ApprovedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARPaymentPM.LocalCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            ARPaymentPM.AmountInPaymentCurrency = 12000;
            ARPaymentPM.AmountInLocalCurrency = 12000;
            ARPaymentPM.PaymentCurrencyExchangeRate = 1;
            ARPaymentPM.StatusCode = "DR";
            ARPaymentPM.StatusName = "Unpaid";
            ARPaymentPM.CashbookId = FullAccountingVariables.CashBook1421TestId;
            ARPaymentPM.BillToAddressId = FullAccountingVariables.AddressCustomer12PMCS;
            ARPaymentPM.OpenAmount = 12000;
            ARPaymentPM.CreateDate = DateTime.UtcNow;
            ARPaymentPM.SearchFields = "ARP2059,AD,CA,Accounting Customer 2 ,NIS";
            ARPaymentPM.ValueDate = DateTime.UtcNow;
            ARPaymentPM.RegisterDate = DateTime.UtcNow;
            ARPaymentPM.ProfitCurrencyExchangeRate = 2;
            ARPaymentPM.SetApproved = false;
            ARPaymentPM.IsFullAccounting = true;

            return ARPaymentPM;


        }
    }
}
