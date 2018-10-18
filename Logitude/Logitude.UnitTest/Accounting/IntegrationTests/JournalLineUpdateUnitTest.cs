using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Logitude.UnitTest.Accounting.IntegrationTests
{
    [TestClass]
    class JournalLineUpdateUnitTest
    {
        [TestMethod]
        public void OnUpdating_ApprovedJournalCanOnlyChangeToVoided_ThrowBLException()
        {
            throw new Exception("_BLException :Approved Journal Can Only Change To Voided");
        }

    }
}
