using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookConnection.Common.LoginWcfServiceReference;

namespace OutlookConnection.Common.Contracts
{
    public interface ICRMTempSettings
    {
        string AccountID { get; set; }
        bool EnableAddin { get; set; }
        int EnableAutoSync { get; set; }
        int EnableNotification { get; set; }
        string Password { get; set; }
        event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        string ServerURL { get; set; }
        int SyncNumberOfDays { get; set; }

        string Token { get; set; }
        string User { get; set; }
        TenantInfo MyTenantInfo { get; set; }
    }
}
