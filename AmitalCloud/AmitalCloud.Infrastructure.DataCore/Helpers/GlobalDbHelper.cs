using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityClasses;
using System.Configuration;
namespace AmitalCloud.Infrastructure.Data.Helpers
{
    public class GlobalDbHelper
    {
        public static GlobalDB GetGlobalDB(int tenant) => GlobalDBRepository.GetGlobalDBByTenant(tenant);
        private static GlobalDB GetGlobalDbFromEnviroment()
        {
            return new GlobalDB()
            {
                Id = "0",
                DBConnection = ConfigurationManager.ConnectionStrings["SystemMainStr"].ConnectionString,
                IsUpgrading = false,
                IsActive = true,
                SharedDWConnection = null,
                SecondaryAzureDBConnection = null,
                IsBlocking = false
            };
        }
        private static GlobalDB GetGlobalDbWithoutProxy(GlobalDB currentDb)
        {
            GlobalDB currentDbCacheWithoutProxy = new GlobalDB()
            {
                DBConnection = currentDb.DBConnection,
                Id = currentDb.Id,
                IsActive = currentDb.IsActive,
                IsUpgrading = currentDb.IsUpgrading,
                SecondaryAzureDBConnection = currentDb.SecondaryAzureDBConnection
            };
            return currentDbCacheWithoutProxy;
        }
        public static GlobalDB GetGlobalDBById(string id) => new GlobalDBRepository().GetGlobalDBById(id);
    }
}
