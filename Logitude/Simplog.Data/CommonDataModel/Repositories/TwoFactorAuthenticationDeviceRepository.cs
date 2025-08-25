using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TwoFactorAuthenticationDeviceRepository : IRepository<TwoFactorAuthenticationDevice>
    {
        
           ICommonDataContext commonDataContext;



        public TwoFactorAuthenticationDeviceRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TwoFactorAuthenticationDeviceRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<TwoFactorAuthenticationDevice> GetTwoFactorAuthenticationDevices(int tenant)
        {
            return (from record in context.TwoFactorAuthenticationDevices where record.Tenant == tenant select record);
        }

        public TwoFactorAuthenticationDevice GetSingleTwoFactorAuthenticationDeviceByUser(string twoFactorkey,string userId, int tenant)
        {
            return (from record in context.TwoFactorAuthenticationDevices where record.TwoFactorkey == twoFactorkey && record.UserId == userId && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<TwoFactorAuthenticationDevice> GetAllTwoFactorAuthenticationDevicesByUser(string userId, int tenant)
        {
            return (from record in context.TwoFactorAuthenticationDevices where record.InActive == false && record.UserId == userId && record.Tenant == tenant select record);
        }

        public TwoFactorAuthenticationDevice GetSingleTwoFactorAuthenticationDeviceByKey(string key, int tenant)
        {
            return (from record in context.TwoFactorAuthenticationDevices where  record.TwoFactorkey == key && record.Tenant == tenant select record).FirstOrDefault();
        }

        public TwoFactorAuthenticationDevice GetSingleTwoFactorAuthenticationDevice(string id, int tenant)
        {
            return (from record in context.TwoFactorAuthenticationDevices where record.Id == id  && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<TwoFactorAuthenticationDevice> GetUsersWorkspaceLastLogins(int tenant)
        {
            return (from d in context.TwoFactorAuthenticationDevices
                    where d.Tenant == tenant
                    select d);
        }

        public void Add(TwoFactorAuthenticationDevice entity)
        {
            context.TwoFactorAuthenticationDevices.Add(entity);
        }

        public void Remove(TwoFactorAuthenticationDevice entity)
        {
            try
            {
                context.TwoFactorAuthenticationDevices.Attach(entity);
            }
            catch { };
            context.TwoFactorAuthenticationDevices.Remove(entity);
        }

        public void Update(TwoFactorAuthenticationDevice entity)
        {
            try
            {
                context.TwoFactorAuthenticationDevices.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<TwoFactorAuthenticationDevice> All()
        {
            return context.TwoFactorAuthenticationDevices.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<TwoFactorAuthenticationDevice> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TwoFactorAuthenticationDevice GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}