using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
using System;

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
            return (from a in context.Settings
                    where a.Id == id
                    select a.ReportsRunUsingWR).FirstOrDefault();
        }

        public Setting GetSingleSetting(string id)
        {

            return (from a in context.Settings
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<Setting> GetAllSettings()
        {
            return from a in context.Settings
                   select a;
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