using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.UnitTest.Accounting.UniTests.EntityMapping
{
    [TestClass]
    public class ChartOfAccountDataMappingUnderTest
    {
        [TestMethod]
        [Ignore]
        public void MyTestMethod()
        {

            ChartOfAccountPM  myChartOfAccount= JsonConvert.DeserializeObject<ChartOfAccountPM >(
                @"{""Id"":null,""Tenant"":989,""Code"":""191"",""EncodeBase64NVARCHARFieldsBy"":""windows-1255"",""LocalName"":""Muwy4VdB5OdHQeHwV0Lj"",""EnglishName"":null,""ParentCode"":""19"",""TypeCode"":""3"",""Inactive"":null,""TypeName"":null,""ParentName"":null,""SearchFields"":null}");
            var ChartOfAccountDataMapping = new ChartOfAccountDataMapping();
            //myChartOfAccount.
            myChartOfAccount.EncodeBase64NVARCHARFieldsBy = "windows-1255";
            myChartOfAccount.LocalName = "Muwy4VdB5OdHQeHwV0Lj";
            ChartOfAccountDataMapping.EncodeBase64NVARCHARFields(myChartOfAccount);
            
            Assert.AreEqual("", myChartOfAccount.LocalName);
        }

        [TestMethod]
        
        public void MyTestMethod1()
        {

            ChartOfAccountPM myChartOfAccount = JsonConvert.DeserializeObject<ChartOfAccountPM>(
                @"{""Id"":null,""Tenant"":989,""Code"":""191"",""EncodeBase64NVARCHARFieldsBy"":""windows-1255"",""LocalName"":""7Pfl5+X6IODp9un3IODo6eDx"",""EnglishName"":null,""ParentCode"":""19"",""TypeCode"":""3"",""Inactive"":null,""TypeName"":null,""ParentName"":null,""SearchFields"":null}"
                );
            var ChartOfAccountDataMapping = new ChartOfAccountDataMapping();
            //myChartOfAccount.
            myChartOfAccount.EncodeBase64NVARCHARFieldsBy = "windows-1255";
            
            ChartOfAccountDataMapping.EncodeBase64NVARCHARFields(myChartOfAccount);

            Assert.AreEqual("לקוחות איציק אטיאס", myChartOfAccount.LocalName);
        }
    }
}
