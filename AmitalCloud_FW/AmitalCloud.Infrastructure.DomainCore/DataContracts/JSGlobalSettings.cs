using AmitalCloud.Infrastructure.Domain.EntityClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.DataContracts
{
    public class JSGlobalSettings
    {
        public JSGlobalSettings(Setting mySetting) 
        {
            Id = mySetting.Id;
            LogitudeURL = mySetting.LogitudeURL;
            LogoCode = mySetting.LogoCode;
            WorkEnvironment = mySetting.WorkEnvironment;
            SameUserLoginEnabled = (bool)mySetting.SameUserLoginEnabled;
            LayoutDirection = mySetting.LayoutDirection;
            ReportsRunUsingWR = (bool)mySetting.ReportsRunUsingWR;
            DocumentFilingEmailDomain = mySetting.DocumentFilingEmailDomain;
            DeploymentStage = mySetting.DeploymentStage;
            ReleaseNotesURL = mySetting.ReleaseNotesURL;
            LogitudeDemoTenants = mySetting.LogitudeDemoTenants;
            TMPersonalAccessExpirationDate = mySetting.TMPersonalAccessExpirationDate;
            ReleaseDateString = mySetting.ReleaseDateString;
            DNSZone = mySetting.DNSZone;

        }

        [Key]
        public string Id { get; set; }
        public string LogitudeURL { get; set; }
        public string LogoCode { get; set; }
        public string WorkEnvironment { get; set; } //customs,main....
        public bool SameUserLoginEnabled { get; set; }
        public string LayoutDirection { get; set; }
        public string ProductInfo { get;  set; }
        public string ProductMessage { get;  set; }
        public bool ReportsRunUsingWR { get; set; }
        public string DocumentFilingEmailDomain { get; set; }
        public string DeploymentStage { get; set; }
        public string ReleaseNotesURL { get; set; }
        public string LogitudeDemoTenants { get; set; }
        public DateTime? TMPersonalAccessExpirationDate { get; set; }
        public string ReleaseDateString { get; set; }
        public string DNSZone { get; set; }
    }

}
