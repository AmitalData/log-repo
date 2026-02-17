using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ValidationTypeRepository:IRepository<ValidationType>
    {
        IWebFreightContext webFreightContext;
        public ValidationTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public ValidationTypeRepository()
        {
            webFreightContext=new WebFreightContext();
        }
        public ValidationTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<ValidationType> GetValidationTypes()
        {
            return context.ValidationTypes;
        }

        public IQueryable<ValidationType> GetValidationTypesByTenant(int tenant)
        {
            IQueryable<ValidationType> validationTypes = from a in context.ValidationTypes
                                                         where a.Tenant == tenant
                                                         select a;
            return validationTypes;
        }

        public void Add(ValidationType entity)
        {
            context.ValidationTypes.Add(entity);
        }

        public void Remove(ValidationType entity)
        {
            context.ValidationTypes.Attach(entity);
            context.ValidationTypes.Remove(entity);
        }

        public void Update(ValidationType entity)
        {
            context.ValidationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ValidationType> All()
        {
            return context.ValidationTypes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ValidationType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ValidationType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}