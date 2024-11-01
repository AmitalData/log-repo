using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class GlobalDBRepository: Repository<GlobalDB>
    {

        IGlobalContext globalContext;
        public GlobalDBRepository(IGlobalContext context):base(context)
        {
            globalContext = context;
        }

        public GlobalDBRepository():this(GlobalContext.GetContext())
        {
        }
        public GlobalDB GetGlobalDBById(string id)
        {
            string enviroment = ConfigurationManager.AppSettings.Get("ENVIROMENT");
            if (enviroment == "azure app service")
                return GetGlobalDbFromEnviroment();

            return (from a in context.GlobalDBs
                    where a.Id == id
                    select a).FirstOrDefault();
        }
        
        public GlobalDB GetSingleGlobalDB(string id)
        {

            string name = "TenantDB" + id;
            GlobalDB db = null;
            string enviroment = ConfigurationManager.AppSettings.Get("ENVIROMENT");

            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(name) == null)
                {
                    if (enviroment == "azure app service")
                        db = GetGlobalDbFromEnviroment();

                    else
                    {
                        IGlobalContext context = GlobalContext.GetContext();


                        db = (from a in context.GlobalDBs
                              where a.Id == id
                              select a).FirstOrDefault();
                    }

                    CacheManager.CacheWrapper.Insert(name, db, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    //}
                }
                else
                {
                    db = (GlobalDB)CacheManager.CacheWrapper.Get(name);
                }
            }

            else
            {
                if (enviroment == "azure app service")
                    db = GetGlobalDbFromEnviroment();

                else
                {
                    IGlobalContext context = GlobalContext.GetContext();

                    db = (from a in context.GlobalDBs
                          where a.Id == id
                          select a).FirstOrDefault();
                }
            }



            return db;
        }

        public static GlobalDB GetGlobalDBByTenant(int tenant)
        {
            string name = "TenantDB" + tenant;
            GlobalDB db = null;
            string enviroment = ConfigurationManager.AppSettings.Get("ENVIROMENT");

            if (true) //HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(name) == null)
                {
                    db = enviroment == "azure app service" ? GetGlobalDbFromEnviroment() : GetByGlobalTenant(tenant);                    

                    CacheManager.CacheWrapper.Insert(name, db, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    //}
                }
                else
                {
                    db = (GlobalDB)CacheManager.CacheWrapper.Get(name);
                }
            }

            else
            {
                db = enviroment == "azure app service" ? GetGlobalDbFromEnviroment() : GetByGlobalTenant(tenant);
            }

            return db;
        }

        private static GlobalDB GetByGlobalTenant(int tenant)
        {
            GlobalDB db;
            IGlobalContext context = GlobalContext.GetContext();

            GlobalTenant globaltenant = (from a in context.GlobalTenants
                                         where a.Id == tenant
                                         select a).FirstOrDefault();

            db = (from a in context.GlobalDBs
                  where a.Id == globaltenant.GlobalDBId
                  select a).FirstOrDefault();
            return db;
        }

        public int GetDataBasesCount()
        {
            return context.GlobalDBs.Count();
        }

        public List<GlobalDB> GetActiveDataBases()
        {
            return (from a in context.GlobalDBs
                   where a.IsActive == true
                   select a).ToList();
        }

        public IQueryable<GlobalDB> GetGlobalDBs()
        {
            return context.GlobalDBs;
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

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
    }
}