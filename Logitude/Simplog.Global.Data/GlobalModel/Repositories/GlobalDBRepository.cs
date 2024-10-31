using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System.Configuration;
namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class GlobalDBRepository:IRepository<GlobalDB>
    {

        IGlobalContext globalContext;
        public GlobalDBRepository(IGlobalContext context)
        {
            globalContext = context;

        }

        public GlobalDBRepository()
        {
            //string _GlobalContextCache = "Global_Context";
            //if (HttpContext.Current != null)
            //{
            //    if (HttpContext.Current.Cache.Get(_GlobalContextCache) == null)
            //    {
            //        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //        {
            //            Context = GlobalContext.GetContext();
            //            HttpContext.Current.Cache.Insert(_GlobalContextCache, Context, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            //        }

            //    }
            //    else
            //    {
            //        Context = (GlobalContext)HttpContext.Current.Cache.Get(_GlobalContextCache);
            //    }
            //}

            //else
            //{
            //    Context = GlobalContext.GetContext();
            //}
            globalContext = GlobalContext.GetContext();
           
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
                  where a.Id == globaltenant.GlobalDBId|| a.Id == "0"
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

        public void Add(GlobalDB entity)
        {
            context.GlobalDBs.Add(entity);
        }

        public void Remove(GlobalDB entity)
        {
            context.GlobalDBs.Attach(entity);
            context.GlobalDBs.Remove(entity);
        }

        public void Update(GlobalDB entity)
        {
            context.GlobalDBs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GlobalDB> All()
        {
            return context.GlobalDBs.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<GlobalDB> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GlobalDB GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
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