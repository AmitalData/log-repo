using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class CategoryTypeRepository:IRepository<CategoryType>
    {

        IWebFreightContext webFreightContext;
        public CategoryTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public CategoryTypeRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public CategoryTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<CategoryType> GetCategoryTypes()
        {
            return context.CategoryTypes;
        }

        public CategoryType GetSingleCategoryType(string code)
        {
            return (from a in context.CategoryTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(CategoryType entity)
        {
            context.CategoryTypes.Add(entity);
        }

        public void Remove(CategoryType entity)
        {
            context.CategoryTypes.Attach(entity);
            context.CategoryTypes.Remove(entity);
        }

        public void Update(CategoryType entity)
        {
            context.CategoryTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CategoryType> All()
        {
            return context.CategoryTypes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CategoryType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CategoryType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}