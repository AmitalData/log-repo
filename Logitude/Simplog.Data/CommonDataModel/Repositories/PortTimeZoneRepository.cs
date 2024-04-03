using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PortTimeZoneRepository : IRepository<PortTimeZone>
    {
        ICommonDataContext commonDataContext;

        public PortTimeZoneRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public PortTimeZoneRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public PortTimeZoneRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<PortTimeZone> GetPortTimeZones()
        {
            return context.PortTimeZones;
        }

        public IQueryable<PortTimeZone> GetAll()
        {
            return context.PortTimeZones;
        }

        public PortTimeZone GetSinglePortTimeZone(string code)
        {
            return (from a in context.PortTimeZones
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(PortTimeZone entity)
        {
            commonDataContext.PortTimeZones.Add(entity);
        }

        public void Remove(PortTimeZone entity)
        {
            commonDataContext.PortTimeZones.Remove(entity);
        }

        public void Update(PortTimeZone entity)
        {
            commonDataContext.PortTimeZones.Attach(entity);
            commonDataContext.SetAsModified(entity);
        }

        public List<PortTimeZone> All()
        {
            return context.PortTimeZones.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            commonDataContext.SaveChanges();
        }

        public List<PortTimeZone> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PortTimeZone GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public bool CheckIfTimeZoneExists(string code)
        {
            return (from a in context.PortTimeZones
                    where a.Code == code
                    select a).Any();
        }
    }
}
