using Logitude.Accounting.Def.EntityPMs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Customs.Tests
{
    [TestClass]
    public class DeclarationIntegrationTests
    {
        [TestMethod]
        public async Task CreateDeclaration_Post_Successful()
        {
                DeclarationPM entityPM = GetNewDeclarationPM();
                HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "Declarations");
                DeclarationPM declarationPM = RestClientService.ParseResponse<DeclarationPM>(response);
                Assert.AreEqual(entityPM.CustomFileNo, declarationPM.CustomFileNo);
        }
        private  DeclarationPM GetNewDeclarationPM()
        {
            DeclarationPM declarationPM = new DeclarationPM();
            declarationPM.CustomFileNo = VariablesGenerater.GetUniqueIdByDate();
            declarationPM.Tenant = IntegrationTestLoginParameters.Tenant;
            declarationPM.CustomerId = CustomsVariables.CusstomerGECUId;
            declarationPM.TransportModeId = CustomsVariables.CustomsTransportModeACode;
            declarationPM.DeclarationOfficeCode = CustomsVariables.CustomsHouseTypesITESTCode;

            return declarationPM;
        }
    }
}
