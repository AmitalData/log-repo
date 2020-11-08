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
    public class UpdateAPInvoice
    {
        [TestMethod]
        public async Task UpdateAPInvoices_Put_Successful()
        {
                APInvoicePM entityPM = await GetSingle();
                entityPM.VATNumber =  VariablesGenerater.GetNumbersRandomString(5);
                HttpResponseMessage response = await RestClientService.PutAsync(entityPM, "APInvoices");
                ARInvoicePM aRInvoicePM = RestClientService.ParseResponse<ARInvoicePM>(response);
                Assert.IsNotNull(aRInvoicePM);
                Assert.AreEqual(entityPM.Id, aRInvoicePM.Id);
        }
        [TestMethod]
      
        private async Task<APInvoicePM> GetSingle()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("APInvoices/GetSingle?id=" + FullAccountingVariables.APInvoice1205Id);
            APInvoicePM APInvoicePM = RestClientService.ParseResponse<APInvoicePM>(response);
            return APInvoicePM;
        }
    }
}
