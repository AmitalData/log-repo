using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TruckerSettingRepository : IRepository<TruckerSetting>
    {
        ICommonDataContext commonDataContext;

 

        public TruckerSettingRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TruckerSettingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

       

        public void Add(TruckerSetting entity)
        {
            context.TruckerSettings.Add(entity);
        }

        public void Remove(TruckerSetting entity)
        {
            context.TruckerSettings.Remove(entity);
        }

        public void Update(TruckerSetting entity)
        {
            context.TruckerSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TruckerSetting> All()
        {
            return context.TruckerSettings.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<TruckerSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TruckerSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public TruckerSetting GetSingleTruckerSetting(string id, int tenant)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TruckerSetting> GetTruckerSettings(int tenant)
        {
            throw new NotImplementedException();
        }
        public TruckerSetting GetSingle(string id, int tenant)
        {
            return (from record in context.TruckerSettings where record.Tenant == tenant && record.Id == id select record).FirstOrDefault();
        }
    }
}