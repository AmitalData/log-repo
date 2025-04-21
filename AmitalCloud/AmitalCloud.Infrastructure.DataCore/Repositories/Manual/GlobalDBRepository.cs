using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Domain.Helpers;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class GlobalDBRepository : Repository<GlobalDB>
    {
        IGlobalContext globalContext;
        #region Constructors
        public GlobalDBRepository(IGlobalContext context) : base(context)
        {
            globalContext = context;
        }
        public GlobalDBRepository() : this(GlobalContext.GetContext())
        {
        }
        #endregion Constructors

        public GlobalDB GetGlobalDBById(string id) => GetSingleGlobalDB(id);
        public GlobalDB GetSingleGlobalDB(string id) => ConfigurationManager.AppSettings.Get("ENVIROMENT") == "azure app service" ? GetGlobalDbFromEnviroment() : GetAll(0, true).Where(a => a.Id == id).FirstOrDefault();
        public int GetDataBasesCount() => GetAll(0, true).Count();
        public List<GlobalDB> GetActiveDataBases() => GetAll(0, true).Where(a => a.IsActive == true).ToList();
        public IQueryable<GlobalDB> GetGlobalDBs() => context.GlobalDBs;
        public IGlobalContext context
        {
            get { return globalContext == null ? GlobalContext.GetContext() : globalContext; }
        }
        public static GlobalDB GetGlobalDBByTenant(int tenant) => ConfigurationManager.AppSettings.Get("ENVIROMENT") == "azure app service" ? GetGlobalDbFromEnviroment() : GetByGlobalTenant(tenant);
        private static GlobalDB GetByGlobalTenant(int tenant)
        {
            tenant = SettingUtil.GetCurrentTenant();
            IGlobalContext context = GlobalContext.GetContext();
            GlobalTenant globaltenant = new Repository<GlobalTenant>(context).GetAll(0, true).Where(a => a.Id == tenant).FirstOrDefault();
            return new Repository<GlobalDB>(context).GetAll(0, true).Where(a => a.Id == globaltenant.GlobalDBId).FirstOrDefault();
        }
        private static GlobalDB GetGlobalDbFromEnviroment()
        {

            return new GlobalDB()
            {
                Id = SettingUtil.GetCurrentTenant().ToString(),
                DBConnection = ConfigurationManager.ConnectionStrings["SystemMainStr"].ConnectionString,
                IsUpgrading = false,
                IsActive = true,
                SharedDWConnection = null,
                SecondaryAzureDBConnection = null,
                IsBlocking = false
            };
        }
    }
}