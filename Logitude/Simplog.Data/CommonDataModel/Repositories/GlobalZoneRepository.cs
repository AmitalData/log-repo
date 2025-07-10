using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class GlobalZoneRepository : IRepository<GlobalZone>
    {
        ICommonDataContext commonDataContext;

        public GlobalZoneRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public GlobalZoneRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public GlobalZoneRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<GlobalZone> GetGlobalZones(int tenant)
        {
            return (from record in context.GlobalZones where record.Tenant == tenant select record);
        }

        public GlobalZone GetSingleGlobalZone(string id, int tenant)
        {
            return (context.GlobalZones.Where(record => record.Id == id && record.Tenant == tenant)).FirstOrDefault();
        }

        public GlobalZone GetSingleGlobalZoneByCode(string code, int tenant)
        {
            return (from record in context.GlobalZones where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(GlobalZone entity)
        {
            context.GlobalZones.Add(entity);
        }

        public void Remove(GlobalZone entity)
        {
            context.GlobalZones.Attach(entity);
            context.GlobalZones.Remove(entity);
        }

        public void Update(GlobalZone entity)
        {
            context.GlobalZones.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GlobalZone> All()
        {
            return context.GlobalZones.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<GlobalZone> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public GlobalZone GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}