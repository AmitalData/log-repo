using Logitude.DeploymentUtilities.Models;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System.Configuration;

namespace Logitude.DeploymentUtilities
{
    public class Program
    {
        static void Main(string[] args)
        {
            ToolArguments.Arguments = args;

            CacheManager.CacheWrapper = new MockCacheWrapper();

            string dbms = ConfigurationManager.AppSettings.Get("DBMS");
            LogitudeSettings.DatabaseManagementSystem = dbms;

            DeploymentUtilitiesTool deploymentUtilitiesTool = new DeploymentUtilitiesTool();
            deploymentUtilitiesTool.RunTool();
        }
    }
}