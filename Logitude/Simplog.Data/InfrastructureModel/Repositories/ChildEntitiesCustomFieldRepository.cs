using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ChildEntitiesCustomFieldRepository : IRepository<ChildEntitiesCustomField>
    {
        IWebFreightContext webFreightContext;
        public static int MaxNumberOfCustomFields = 50;

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public ChildEntitiesCustomFieldRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public ChildEntitiesCustomFieldRepository()
        {
        }

        public ChildEntitiesCustomFieldRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(ChildEntitiesCustomField entity)
        {
            context.ChildEntitiesCustomFields.Add(entity);

        }

        public void Remove(ChildEntitiesCustomField entity)
        {
            context.ChildEntitiesCustomFields.Attach(entity);
            context.ChildEntitiesCustomFields.Remove(entity);
        }

        public void Update(ChildEntitiesCustomField entity)
        {
            context.ChildEntitiesCustomFields.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ChildEntitiesCustomField> All()
        {
            return context.ChildEntitiesCustomFields.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ChildEntitiesCustomField> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ChildEntitiesCustomField GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ChildEntitiesCustomField> GetChildEntitiesCustomFields(int tenant)
        {
            return context.ChildEntitiesCustomFields.Where(d => d.Tenant == tenant);
        }
    }
}
