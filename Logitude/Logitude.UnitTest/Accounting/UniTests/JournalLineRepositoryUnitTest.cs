using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class JournalLineRepositoryUnitTest
    {
        [TestMethod]
        [Ignore]
        public void JournalLineRepositoryAdd_UseWithoutUpdateService_ThrowExcptionInsureUsingOnlyByUpdateService()
        {
           //Debug.Fail
            throw new Exception
               (
               "_BLException :Approved Journal Can Only Change To Voided"); 
        }
    }
}
