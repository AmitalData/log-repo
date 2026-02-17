using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class INTTRASettingModeRepository : IRepository<INTTRASettingMode>
    {
        ICommonDataContext context;
        public ICommonDataContext Context
        {
            get { return this.context; }
        }
        public INTTRASettingModeRepository(int tenant)
        {
            context = CommonDataContext.GetContext(tenant);
        }
        public INTTRASettingModeRepository(ICommonDataContext context)
        {
            this.context = context;
        }

        public INTTRASettingMode GetSingleINTTRASettingMode(string code)
        {
            return (from a in context.INTTRASettingModes where a.Code == code select a).FirstOrDefault();
        }
        public IQueryable<INTTRASettingMode> GetINTTRASettingModes()
        {
            return (from a in context.INTTRASettingModes select a);
        }

        public IQueryable<INTTRASettingMode> GetAll()
        {
            return (from a in context.INTTRASettingModes select a);
        }

        public void Add(INTTRASettingMode entity)
        {
            context.INTTRASettingModes.Add(entity);
        }
        public void Remove(INTTRASettingMode entity)
        {
            context.INTTRASettingModes.Attach(entity);
            context.INTTRASettingModes.Remove(entity);
        }
        public void Update(INTTRASettingMode entity)
        {
            context.INTTRASettingModes.Attach(entity);
            context.SetAsModified(entity);
        }
        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<INTTRASettingMode> All()
        {
            return context.INTTRASettingModes.ToList();
        }
        public INTTRASettingMode GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
        public List<INTTRASettingMode> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
