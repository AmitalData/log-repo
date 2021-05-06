using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookConnection.Common.LoginWcfServiceReference;

namespace OutlookConnection.Common
{
    public class SettingServiceLocator
    {
        static SettingServiceLocator _SettingServiceLocator;
        public static SettingServiceLocator Instance
        {
            get { return SettingServiceLocator._SettingServiceLocator = SettingServiceLocator._SettingServiceLocator?? new   SettingServiceLocator(); }
            
        }
        

        
        

        SettingServiceLocator()
        {

        }


        //public int Tenant { get; set; }
        //public string WebServiceURL { get; set; }

        public Contracts.ICRMTempSettings CRMSettings { get; set; }
        public List<String> AccountList { get; set; }

        public List<TenantInfo> AvailableTenants { get; set; }

        public object MainWindow { get; set; }
    }
}
