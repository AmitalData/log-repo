using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class GlobalTenantRepository:IRepository<GlobalTenant>
    {

        IGlobalContext globalContext;
        public GlobalTenantRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public GlobalTenantRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public GlobalTenant GetGlobalTenantsByTenant(int tenant)
        {
            GlobalTenant globalTenant = (from a in context.GlobalTenants
                                        where a.Id == tenant
                                        select a).FirstOrDefault();
            return globalTenant;
        }

        public List<GlobalTenant> GetActiveTenants()
        {
          return this.context.GlobalTenants.Where(te => te.Version != -1).ToList();
        }


        public IQueryable<GlobalTenant> GetAllGlobalTenant()
        {
            return this.context.GlobalTenants;
        }

        public List<int> GetActiveGlobalTenantsIds()
        {
            return (from a in context.GlobalTenants
                    where a.IsActive && a.Version != -1
                    select a.Id).ToList();
        }

        public int GetCurrentVersion()
        {
            return (from a in context.GlobalTenants
                    where a.Id == 0
                    select a.Version).FirstOrDefault();
        }

        public static List<GlobalTenant> GetGlobalTenants()
        {
            string entityName = "GlobalTenantsList";
            List<GlobalTenant> theGlobalTenants;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    IGlobalContext context = GlobalContext.GetContext();
                    List<GlobalTenant> globalTenants = (from a in context.GlobalTenants
                                        select a).ToList();


                    CacheManager.CacheWrapper.Insert(entityName, globalTenants, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    theGlobalTenants = CacheManager.CacheWrapper.Get(entityName) as List<GlobalTenant>;



                }
                else
                {
                    theGlobalTenants = CacheManager.CacheWrapper.Get(entityName) as List<GlobalTenant>;
                }

            }
            else
            {
                IGlobalContext context = GlobalContext.GetContext();
                theGlobalTenants = (from a in context.GlobalTenants
                                 select a).ToList();
            }
            return theGlobalTenants;

        }

        public void Add(GlobalTenant entity)
        {
            context.GlobalTenants.Add(entity);
        }

        public void Remove(GlobalTenant entity)
        {
            context.GlobalTenants.Attach(entity);
            context.GlobalTenants.Remove(entity);
        }

        public void Update(GlobalTenant entity)
        {
            context.GlobalTenants.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GlobalTenant> All()
        {
            return context.GlobalTenants.ToList();
        }

        public IGlobalContext context
        {
            get {return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public GlobalTenant GetTenantByTTY(string TTY)
        {
            return (from a in context.GlobalTenants
                    where a.TTY == TTY
                    select a).FirstOrDefault();
        }


        public List<GlobalTenant> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GlobalTenant GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}