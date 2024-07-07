using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class HelpResourceRepository:IRepository<HelpResource>
    {
        IGlobalContext globalContext;

        public HelpResourceRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public HelpResourceRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public HelpResource GetSingleHelpResource(string code, int tenant)
        {
            HelpResource item = (from a in context.HelpResources where a.Code == code select a).FirstOrDefault();
            string entityName = "HelpResource" + code + tenant;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && item != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, item, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    item = (HelpResource)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return item;
        }

        public IQueryable<HelpResource> GetAllHelpResources()
        {
            IQueryable<HelpResource> items = from a in context.HelpResources select a;

            string entityName = "AllHelpResources";
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && items != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, items, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    items = (IQueryable<HelpResource>)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return items;
        }
        public IQueryable<HelpResource> GetReleaseHelpResources()
        {
            string type = "REL";
            IQueryable<HelpResource> items = (from a in context.HelpResources where a.Type == type select a).OrderByDescending(a => a.CreateDate).Take(5);

            string entityName = "ReleaseHelpResources";
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && items != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, items, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    items = (IQueryable<HelpResource>)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return items;
        }

        public IQueryable<HelpResource> GetAllActiveHelpResources()
        {
            return from a in context.HelpResources where a.Inactive == false select a;
        }

        public IQueryable<HelpResource> GetHelpResources(int tenant)
        {
            return from a in context.HelpResources select a;
        }

        public void Add(HelpResource entity)
        {
            context.HelpResources.Add(entity);
        }

        public void Remove(HelpResource entity)
        {
            context.HelpResources.Attach(entity);
            context.HelpResources.Remove(entity);
        }

        public void Update(HelpResource entity)
        {
            context.HelpResources.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<HelpResource> All()
        {
            return context.HelpResources.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<HelpResource> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public HelpResource GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
