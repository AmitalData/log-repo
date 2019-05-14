

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DWCategoriesRepository : IRepository<DWCategories>
    {
        public IWebFreightContext webFreightContext;

        public DWCategoriesRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DWCategoriesRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public DWCategoriesRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(DWCategories entity)
        {
            webFreightContext.DWCategories.Add(entity);
        }

        public void Remove(DWCategories entity)
        {
            webFreightContext.DWCategories.Attach(entity);
            webFreightContext.DWCategories.Remove(entity);
        }

        public void Update(DWCategories entity)
        {
            webFreightContext.DWCategories.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<DWCategories> All()
        {
            return webFreightContext.DWCategories.ToList();
        }

        public List<DWCategories> GetAll()
        {
            return webFreightContext.DWCategories.ToList();
        }


        //public IQueryable<DWCategories> GetObjectsByTenant(int tenant)
        //{
        //    IQueryable<DWCategories> DWCategories = from a in webFreightContext.DWCategories
        //                                            where a.Tenant == tenant || a.Tenant == 0
        //                                              select a;
        //    return DWCategories;
        //}



        //public DWCategories GetSingleDWCategories(string Id, int Tenant)
        //{
        //    return webFreightContext.DWCategories.Where(a => a.Id == Id && a.Tenant == Tenant).FirstOrDefault();
        //}

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<DWCategories> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DWCategories GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        //public IQueryable<DWCategories> GetDWCategories(int tenant)
        //{
        //    return webFreightContext.DWCategories.Where(a => a.Tenant == tenant);
        //}

    }
}
