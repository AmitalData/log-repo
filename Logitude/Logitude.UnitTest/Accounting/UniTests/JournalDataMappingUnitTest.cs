using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class JournalDataMappingUnitTest
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
