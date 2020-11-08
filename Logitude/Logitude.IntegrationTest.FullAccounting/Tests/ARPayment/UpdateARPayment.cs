using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
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
    public class UpdateARPayment
    {
        [TestMethod]
        public async Task UpdateARPayment_Put_Successful()
        {
                ARPaymentPM entityPM = await GetSingle();
                entityPM.ProfitCurrencyExchangeRate = 2;
                HttpResponseMessage response = await RestClientService.PutAsync(entityPM, "ARPayments");
                ARPaymentPM ARPaymentPM = RestClientService.ParseResponse<ARPaymentPM>(response);
                Assert.IsNotNull(ARPaymentPM);
                Assert.AreEqual(entityPM.Id, ARPaymentPM.Id);
        }
        [TestMethod]
      
        private async Task<ARPaymentPM> GetSingle()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ARPayments/GetSingle?id=" + FullAccountingVariables.ARPayment12PMCSId);
            ARPaymentPM ARPaymentPM = RestClientService.ParseResponse<ARPaymentPM>(response);
            return ARPaymentPM;
        }
    }
}
