using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
using System;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class SettingRepository : IRepository<Setting>
    {
        IGlobalContext globalContext;
        public SettingRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public SettingRepository(IGlobalContext context)
        {
            globalContext = context;
        }



        public bool GetCheckIfReportsRunUsingWR(string id)
        {
            string entityName = "Setting" + id;
         
            Setting entity = (from a in context.Settings
                              where a.Id == id
                              select a).FirstOrDefault();

            if (CacheManager.CacheWrapper != null)
            { 
                if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                {     
                    CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    entity = (Setting)CacheManager.CacheWrapper.Get(entityName);
                }

            }
            return entity.ReportsRunUsingWR;

            //return (from a in context.Settings
            //        where a.Id == id
            //        select a.ReportsRunUsingWR).FirstOrDefault();
        }

        public Setting GetSingleSetting(string id)
        {
            string entityName = "Setting" + id;
            Setting entity = (from a in context.Settings
                              where a.Id == id
                              select a).FirstOrDefault();

            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    entity = (Setting)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return entity;

            //return (from a in context.Settings
            //        where a.Id == id
            //        select a).FirstOrDefault();
        }

        public IQueryable<Setting> GetAllSettings()
        {
            string entityName = "Settings";
            IQueryable<Setting> Settings = from a in context.Settings
                                              select a;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && Settings != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, Settings, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
            }
            else
            {
                Settings = (IQueryable<Setting>)CacheManager.CacheWrapper.Get(entityName);
            }
            return Settings;
            //return from a in context.Settings
            //       select a;
        }

        public void Add(Setting entity)
        {
            context.Settings.Add(entity);
        }

        public void Remove(Setting entity)
        {
            context.Settings.Attach(entity);
            context.Settings.Remove(entity);
        }

        public void Update(Setting entity)
        {
            context.Settings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Setting> All()
        {
            return context.Settings.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<Setting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Setting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }


        public bool IsDemoTenant(string tenant)
        {
            var id = "1";
            var sitting =  (from a in context.Settings
                    where a.Id == id
                    select a).FirstOrDefault();


            return sitting.LogitudeDemoTenants!=null? Array.IndexOf(sitting.LogitudeDemoTenants.Split(','), tenant) != -1:false;
        }
    }
}