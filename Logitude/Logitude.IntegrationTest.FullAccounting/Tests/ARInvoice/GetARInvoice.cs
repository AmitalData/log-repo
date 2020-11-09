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
    public class GetARInvoice
    {

        [TestMethod]
        public async Task GetARInvoice_Get_Successful()
        {

            HttpResponseMessage response = await RestClientService.GetAsync("ARInvoices/GetSingle?id=" + FullAccountingVariables.InvoiceNumber12PMCSId);
            ARInvoicePM ARInvoicePM = RestClientService.ParseResponse<ARInvoicePM>(response);
            Assert.IsNotNull(ARInvoicePM);
            Assert.AreEqual(ARInvoicePM.Id, FullAccountingVariables.InvoiceNumber12PMCSId);
        }
 
    }
}
