using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Helpers
{
#if false
    

    public class SettingUtil
    {
        private const string _CustomsDeploymentStage = "CustomsDeploymentStage";
        public enum CustomsDeploymentStage
        {
            None,
            Test,
            ///PrePilot,//for diffrent SBQueue
            Pilot,
            Production
        }

        public static CustomsDeploymentStage GetCustomsDeploymentStage()
        {
            if (String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get(_CustomsDeploymentStage)))
            {
                return CustomsDeploymentStage.None;
            }

            var deploymentStage = ConfigurationManager.AppSettings.Get(_CustomsDeploymentStage).ToString();
            CustomsDeploymentStage myCustomsDeploymentStage = CustomsDeploymentStage.None;
            Enum.TryParse<CustomsDeploymentStage>(deploymentStage, out myCustomsDeploymentStage);

            return myCustomsDeploymentStage;
        }
        public static bool ForceDownloadXapFromIIS()
        {
            return false; //by jalal : because it doesn't download xap files from storage online. Islam and MOhammad will check with Ihab.
            switch (GetCustomsDeploymentStage())
            {
                     
                case CustomsDeploymentStage.Test:
                case CustomsDeploymentStage.Pilot:
                case CustomsDeploymentStage.Production:
                    return true;
                    break;
                case CustomsDeploymentStage.None:
                default:
                    return true;
                    break;
            }
        }
    }
#endif
}
