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
    public class GetAPInvoice
    {

        [TestMethod]
        public async Task GetAPInvoice_Get_Successful()
        {

            HttpResponseMessage response = await RestClientService.GetAsync("APInvoices/GetSingle?id=" + FullAccountingVariables.APInvoice1205Id);
            APInvoicePM APInvoicePM = RestClientService.ParseResponse<APInvoicePM>(response);
            Assert.IsNotNull(APInvoicePM);
            Assert.AreEqual(APInvoicePM.Id, FullAccountingVariables.APInvoice1205Id);
        }
 
    }
}
