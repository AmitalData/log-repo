
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; 
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public partial class UserLastSettingsRepository : IRepository<UserLastSettings>
    {
        ICommonDataContext commonDataContext;

        public UserLastSettingsRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public UserLastSettingsRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public UserLastSettingsRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<UserLastSettings> GetUserLastSettings(int tenant)
        {
            return (from record in context.UserLastSettings where record.Tenant == tenant select record);
        }

        public UserLastSettings GetSingleUserLastSettings(string id, int tenant)
        {
            return (from record in context.UserLastSettings where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
         

        public void Add(UserLastSettings entity)
        {
            context.UserLastSettings.Add(entity);
        }

        public void Remove(UserLastSettings entity)
        {
            try
            {
                context.UserLastSettings.Attach(entity);
            }
            catch { };
            context.UserLastSettings.Remove(entity);
        }

        public void Update(UserLastSettings entity)
        {
            try
            {
                context.UserLastSettings.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<UserLastSettings> All()
        {
            return context.UserLastSettings.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<UserLastSettings> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public UserLastSettings GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
