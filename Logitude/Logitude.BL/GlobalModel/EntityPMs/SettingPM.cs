using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.GlobalModel.EntityPMs
{
    public class SettingPM
    {
        [Key]
        public string Id { get; set; }
       // public string LogitudeURL { get; set; }
       // public string ChampURL { get; set; }
        public string DeploymentStage { get; set; }
        public string ChampEnv { get; set; }
        //public string CustomerCareIP { get; set; }
        //public string TotangoServiceId { get; set; }
        public bool UsingAzure { get; set; }
        //public string StorageAccountName { get; set; }
        //public string StorageAccountKey { get; set; }
        public bool IsLogEnabled { get; set; }
        //public string StorageType { get; set; }//azure,azureemulater,
        public int LogitudeCRMTenantNumber { get; set; }
        //public string AutoSignupEmail { get; set; }
        //public string AutoSignupPassword { get; set; }
        public string CheckConnectionURL { get; set; }//a url to call checkconnectionwebservice.asmx;
        public string LogoCode { get; set; }
        public string GLSHKURL { get; set; }
        public string GLSHKEnv { get; set; }
        public bool IsDocumentsArchive { get; set; }
        public string CustomerTenantsURL { get; set; }
        public string ForwarderTenantsURL { get; set; }
        public string QueueServiceMode { get; set; }
        public string StorageServiceMode { get; set; }
        public string WorkEnvironment { get; set; }
        public string HtmlVersion { get; set; }
        public string AndroidAppLink { get; set; }
        public string IOSAppLink { get; set; }
        public bool SameUserLoginEnabled { get; set; }

        public string DocumentFilingEmailDomain { get; set; }
        public bool ReportsRunUsingWR { get; set; }
        
        public int System2RedirectFraction { get; set; }

        public int QBOOAuthDefault { get; set; }
        public string QBOClientID { get; set; }
        public string QBOClientSecret { get; set; }
        public string PrivateKey { get; set; }
        public string AmitalTaxesUrl { get; set; }

    }
}