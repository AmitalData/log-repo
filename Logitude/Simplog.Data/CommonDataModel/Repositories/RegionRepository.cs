using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class RegionRepository : IRepository<Region>
    {
        ICommonDataContext Context;


        public RegionRepository(ICommonDataContext context)
        {
            Context = context;
        }

        public RegionRepository(int tenant)
        {
            Context = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Region> GetRegions(int tenant)
        {
            return context.Regions.Where(d => d.Tenant == tenant);
        }

        public Region GetSingleRegion(string id, int tenant)
        {
            return (from record in context.Regions where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
      
        
        #region IRepository<Region> Members

        public void Add(Region entity)
        {
            context.Regions.Add(entity);
        }

        public void Remove(Region entity)
        {
            context.Regions.Remove(entity);
        }

        public void Update(Region entity)
        {
            context.Regions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Region> All()
        {
            return context.Regions.ToList();
        }

        public ICommonDataContext context
        {
            get { return Context; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        #endregion
        
        public List<Region> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Region GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }

}
