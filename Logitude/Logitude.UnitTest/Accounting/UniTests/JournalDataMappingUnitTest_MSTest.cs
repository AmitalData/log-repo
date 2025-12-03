using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Test.Infrastructure;

namespace Logitude.UnitTest.Accounting.UniTests
{
    /// <summary>
    /// MSTest-compatible version of JournalDataMappingUnitTest.
    /// Migrated from FakeItEasy to MSTest (no mocking required - pure data mapping test).
    /// </summary>
    [TestClass]
    public class JournalDataMappingUnitTest_MSTest : TestBase_MSTest
    {
        [TestMethod]
        public void CustomPMToPOCO_PMAndPOCOToUpper()
        {
            var ExpectedExternalNo = "2016:01:A003";
            var ExpectedExternalSystem = "AMITAL";
            var JournalDataMapping = new JournalDataMapping();
            JournalPM entityPM = new JournalPM()
            {
                ExternalNo = "2016:01:a003",
                ExternalSystem = "Amital"
            };
            Journal entityPOCO = new Journal();
            JournalDataMapping.CustomPMToPOCO(entityPM, entityPOCO);
            Assert.AreEqual(ExpectedExternalNo, entityPOCO.ExternalNo, "ExternalNo Should be UPPER");
            Assert.AreEqual(ExpectedExternalSystem, entityPOCO.ExternalSystem, "ExternalNo Should be UPPER");

            Assert.AreEqual(ExpectedExternalNo, entityPM.ExternalNo, "ExternalNo Should be UPPER");
            Assert.AreEqual(ExpectedExternalSystem, entityPM.ExternalSystem, "ExternalNo Should be UPPER");
        }
    }
}


