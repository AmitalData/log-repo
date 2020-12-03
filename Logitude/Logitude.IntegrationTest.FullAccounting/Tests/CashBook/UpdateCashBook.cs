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
    public class UpdateCashBook
    {
        [TestMethod]
        public async Task UpdateCashBook_Put_Successful()
        {
            CashBookPM entityPM = await GetSingle();
            entityPM.LocalName = "CashBook1421:" + VariablesGenerater.GetRandomString(5);
            entityPM.EnglishName = "CashBook1421:" + VariablesGenerater.GetRandomString(5);
            HttpResponseMessage response = await RestClientService.PutAsync(entityPM, "CashBooks");
            CashBookPM CashBooks = RestClientService.ParseResponse<CashBookPM>(response);
            Assert.IsNotNull(CashBooks);
            Assert.AreEqual(CashBooks.Id, entityPM.Id);
        }
       
        private async Task<CashBookPM> GetSingle()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CashBooks/GetSingle?id=" + FullAccountingVariables.CashBook1421TestId);
            CashBookPM CashBookPM = RestClientService.ParseResponse<CashBookPM>(response);
            return CashBookPM;
        }


    }
}
