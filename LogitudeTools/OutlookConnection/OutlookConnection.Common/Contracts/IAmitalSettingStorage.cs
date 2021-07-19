using OutlookConnection.Common.Models;

namespace OutlookConnection.Common.Contracts
{


    public interface IAmitalSettingStorage
    {
        bool StoreCRMStorageSettings(CRMTempSettings value);
        bool StoreSettingsVersion(SettingsVersionM value);
        CRMTempSettings RetrieveCRMStorageSettings(bool forceReload = false);
        CRMTempSettings CRMSettings { get; }
        SettingsVersionM SettingsVersion { get; }
    }
}
