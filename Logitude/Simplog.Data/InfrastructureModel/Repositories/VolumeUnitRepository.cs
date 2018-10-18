using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class VolumeUnitRepository:IRepository<VolumeUnit>
    {
        IWebFreightContext webFreightContext;
        public VolumeUnitRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public VolumeUnitRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public VolumeUnitRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public VolumeUnit GetSingleVolumeUnit(string code)
        {
            return (from a in context.VolumeUnits
                    where a.Code == code
                    select a).FirstOrDefault();

        }
    
        public IQueryable<VolumeUnit> GetVolumeUnits()
        {
            return context.VolumeUnits;
        }

        public IQueryable<VolumeUnit> GetAll()
        {
            return context.VolumeUnits;
        }
        public void Add(VolumeUnit entity)
        {
            context.VolumeUnits.Add(entity);
        }

        public void Remove(VolumeUnit entity)
        {
            context.VolumeUnits.Attach(entity);
            context.VolumeUnits.Remove(entity);
        }

        public void Update(VolumeUnit entity)
        {
            context.VolumeUnits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VolumeUnit> All()
        {
            return context.VolumeUnits.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<VolumeUnit> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public VolumeUnit GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}