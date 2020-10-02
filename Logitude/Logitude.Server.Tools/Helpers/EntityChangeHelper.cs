
using Simplog.Server.Infrastructure;


namespace Logitude.Server.Tools.Helpers
{
    public class EntityChangeHelper
    {
        public static bool IsShowLogBoxAutomationFields()
        {
            bool result = false;
            if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && (LogitudeSettings.DeploymentStage.ToLower() == "logboxpre" || LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1" || LogitudeSettings.DeploymentStage.ToLower() == "test2" || LogitudeSettings.LogitudeURL == "http://localhost:9996"))
            {
                result = true;
            }
            return result;
        }

    }
}