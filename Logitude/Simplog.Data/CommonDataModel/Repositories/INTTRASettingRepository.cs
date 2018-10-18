using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class INTTRASettingRepository : IRepository<INTTRASetting>
    {
        ICommonDataContext context;
        public ICommonDataContext Context
        {
            get { return this.context; }
        }
        public INTTRASettingRepository(int tenant)
        {
            context = CommonDataContext.GetContext(tenant);
        }
        public INTTRASettingRepository(ICommonDataContext context)
        {
            this.context = context;
        }

        public INTTRASetting GetSingleById(string id)
        {
            return (from d in this.context.INTTRASettings where d.Id == id select d).FirstOrDefault();
        }
        public INTTRASetting GetSingleByTenant(int tenant)
        {
            return (from d in this.context.INTTRASettings.Include("InFTPDetail").Include("OutFTPDetail")
                    where d.Tenant == tenant
                    select d).FirstOrDefault();
        }

        public void Add(INTTRASetting entity)
        {
            context.INTTRASettings.Add(entity);
        }
        public void Remove(INTTRASetting entity)
        {
            context.INTTRASettings.Attach(entity);
            context.INTTRASettings.Remove(entity);
        }
        public void Update(INTTRASetting entity)
        {
            context.INTTRASettings.Attach(entity);
            context.SetAsModified(entity);
        }
        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<INTTRASetting> All()
        {
            return context.INTTRASettings.ToList();
        }
        public INTTRASetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
        public List<INTTRASetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
