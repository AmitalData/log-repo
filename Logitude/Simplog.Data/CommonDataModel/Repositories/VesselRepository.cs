using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class VesselRepository:IRepository<Vessel>
    {
        ICommonDataContext commonDataContext;

        public VesselRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public VesselRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public VesselRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public Vessel GetSingleVessel(string id,int tenant)
        {
            return (from a in context.Vessels.Include("Country") where a.Tenant == tenant && a.Id == id select a).FirstOrDefault();         
        }
        
        public Vessel GetSingleVesselByCode(string code, int tenant)
        {
            return (from a in context.Vessels where a.Tenant == tenant && a.Code == code select a).FirstOrDefault();
        }

        public Vessel GetSingleVesselByName(string name, int tenant)
        {
            return (from a in context.Vessels where a.Tenant == tenant && (a.EnglishName != null && a.EnglishName.ToLower().Trim() == name.ToLower().Trim()) select a).FirstOrDefault();
        }

        public IQueryable<Vessel> GetVesselsByTenant(int tenant)
        {
            return from a in context.Vessels
                   where a.Tenant == tenant
                   select a;
        }

        public IQueryable<Vessel> GetVessels(int tenant)
        {
            return from a in context.Vessels.Include("Country")
                   where a.Tenant == tenant
                   select a;
        }

        public void Add(Vessel entity)
        {
            context.Vessels.Add(entity);
        }

        public void Remove(Vessel entity)
        {
            context.Vessels.Attach(entity);
            context.Vessels.Remove(entity);
        }

        public void Update(Vessel entity)
        {
            context.Vessels.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Vessel> All()
        {
            return context.Vessels.ToList();
        }

        public ICommonDataContext context
        {
            get {return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<Vessel> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Vessel GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
