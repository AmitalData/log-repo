

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DWObjectFieldCategoriesRepository : IRepository<DWObjectFieldCategories>
    {
        public IWebFreightContext webFreightContext;

        public DWObjectFieldCategoriesRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DWObjectFieldCategoriesRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public DWObjectFieldCategoriesRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(DWObjectFieldCategories entity)
        {
            webFreightContext.DWObjectFieldCategories.Add(entity);
        }

        public void Remove(DWObjectFieldCategories entity)
        {
            webFreightContext.DWObjectFieldCategories.Attach(entity);
            webFreightContext.DWObjectFieldCategories.Remove(entity);
        }

        public void Update(DWObjectFieldCategories entity)
        {
            webFreightContext.DWObjectFieldCategories.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<DWObjectFieldCategories> All()
        {
            return webFreightContext.DWObjectFieldCategories.ToList();
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

        public List<DWObjectFieldCategories> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DWObjectFieldCategories GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        //public IQueryable<DWCategories> GetDWCategories(int tenant)
        //{
        //    return webFreightContext.DWCategories.Where(a => a.Tenant == tenant);
        //}

    }
}
