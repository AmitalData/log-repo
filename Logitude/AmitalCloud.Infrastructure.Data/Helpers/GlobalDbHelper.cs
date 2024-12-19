using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web;
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
        public static GlobalDB GetGlobalDBById(string id) => new GlobalDBRepository().GetGlobalDBById(id) ;
    }
}
