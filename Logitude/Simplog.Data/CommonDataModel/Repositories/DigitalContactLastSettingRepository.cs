using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DigitalContactLastSettingRepository : IRepository<DigitalContactLastSetting>
    {
        ICommonDataContext commonDataContext;

        public DigitalContactLastSettingRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DigitalContactLastSettingRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DigitalContactLastSettingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<DigitalContactLastSetting> GetDigitalContactLastSetting(int tenant)
        {
            return context.DigitalContactLastSettings.Where(a => a.Tenant == tenant).Select(a => a);
        }

        public DigitalContactLastSetting GetSingleDigitalContactLastSetting(string id, int tenant)
        {
            return context.DigitalContactLastSettings.Where(a => a.Id == id && a.Tenant == tenant).FirstOrDefault();
        }

        public void Add(DigitalContactLastSetting entity)
        {
            context.DigitalContactLastSettings.Add(entity);
        }

        public void Remove(DigitalContactLastSetting entity)
        {
            try
            {
                context.DigitalContactLastSettings.Attach(entity);
            }
            catch { };
            context.DigitalContactLastSettings.Remove(entity);
        }

        public List<DigitalContactLastSetting> GetDigitalContactLastSettings(string contactId, string objectTableId, int tenant)
        {
            var result = context.DigitalContactLastSettings.Where(a => a.ContactId == contactId && a.Tenant == tenant && a.ObjectTableId == objectTableId)
                                                           .ToList();

            return result;
        }

        public void Update(DigitalContactLastSetting entity)
        {
            try
            {
                context.DigitalContactLastSettings.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<DigitalContactLastSetting> All()
        {
            return context.DigitalContactLastSettings.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<DigitalContactLastSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DigitalContactLastSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<DigitalContactLastSetting> GetSingleByEntityAndContact(string contactId, int tenant, string objectTableId)
        {
            return context.DigitalContactLastSettings
                                 .Where(a => a.ContactId == contactId && a.Tenant == tenant && a.ObjectTableId == objectTableId)
                                 .ToList();

        }
    }
}
