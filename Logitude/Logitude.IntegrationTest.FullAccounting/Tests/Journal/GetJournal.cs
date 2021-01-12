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

namespace Logitude.IntegrationTest.FullAccounting.Tests.Journal
{
    [TestClass]
    public class GetJournal
    {

        [TestMethod]
        public async Task GetJournal_Get_Successful()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("Journals/GetSingle?id=" + FullAccountingVariables.JournalPreprationId);
            JournalPM JournalPM = RestClientService.ParseResponse<JournalPM>(response);
            Assert.IsNotNull(JournalPM);
            Assert.AreEqual(JournalPM.Id, FullAccountingVariables.JournalPreprationId);

        } 

    }
}
