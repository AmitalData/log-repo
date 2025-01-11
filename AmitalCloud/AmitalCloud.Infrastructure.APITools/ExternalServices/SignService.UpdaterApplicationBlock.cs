using AmitalCloud.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.APITools.ExternalServices
{

    public partial class SignService
    {

         public enum FeaturesEnum
        {
            MehesDorBMust, PdfMust, IconMust, ShaarOlamiEnableMust,Feature1Optional, UpdaterApplicationBlockMust
             //,TestMust
        }

        public void UpdaterApplicationBlock(List<string> signServerFeatures, string state, out string updaterApplicationBlockLink,
            out bool isMust,
            ref string queryStringMoreParams)
        {
            isMust = false;
            updaterApplicationBlockLink = "";
            var mySignServerFeaturesEnum = Enum.GetNames(typeof(FeaturesEnum));
            var exceptList = mySignServerFeaturesEnum.Except(signServerFeatures);
            if (exceptList.Count() < 1)
            {
                return;
            }
            var stateList = state.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var ShaarOlamiLine=stateList.FirstOrDefault(l => l.ToLower().Contains("ShaarOlami=".ToLower())) ?? "";
            var SuppressCreateHostLine=stateList.FirstOrDefault(l => l.ToLower().Contains("SuppressCreateHost=".ToLower()))??"";

            if (!ShaarOlamiLine.ToLower().Contains("true") && SuppressCreateHostLine.ToLower().Contains("true"))
            {
                isMust = exceptList.Any(f => f.EndsWith("Must", StringComparison.OrdinalIgnoreCase));
            }
            updaterApplicationBlockLink=AmitalCloudSettings.LogitudeURL; 
        }
    }
}