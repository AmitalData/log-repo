using System;
using System.Data.Common;
using Logitude.SystemLogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Server.Infrastructure;

namespace Logitude.Test.SystemLogsModel
{
    [TestClass]
    public class SystemLogTests
    {
        [TestMethod]
        public void SystemLogContextInitialization()
        {
            string dbConnectionInfo = "Logitude2-4_SystemLogs,sa,Saas256,.";
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            SystemLogContext context = new SystemLogContext(connection);
        }
    }
}
