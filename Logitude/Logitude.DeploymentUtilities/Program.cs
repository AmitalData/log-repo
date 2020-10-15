using Logitude.DeploymentUtilities.Models;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.DeploymentUtilities
{
    public class Program
    {
        static void Main(string[] args)
        {
            ToolArguments.Arguments = args;
            CacheManager.CacheWrapper = new MockCacheWrapper();
            DeploymentUtilitiesTool deploymentUtilitiesTool = new DeploymentUtilitiesTool();
            deploymentUtilitiesTool.RunTool();
        }
    }
}