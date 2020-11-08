using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
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

namespace Logitude.IntegrationTest.FullAccounting.Tests.CashBook
{
    [TestClass]
    public class GetCashBook
    {
         
        [TestMethod]
        public async Task GetCashBook_Get_Successful()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CashBooks/GetSingle?id=" + FullAccountingVariables.CashBook1421TestId);
            CashBookPM CashBookPM = RestClientService.ParseResponse<CashBookPM>(response);
            Assert.IsNotNull(CashBookPM);
            Assert.AreEqual(CashBookPM.Id, FullAccountingVariables.CashBook1421TestId);
        }
        

    }
}
