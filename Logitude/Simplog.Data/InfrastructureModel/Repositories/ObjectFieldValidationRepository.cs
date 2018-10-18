using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ObjectFieldValidationRepository:IRepository<ObjectFieldValidation>
    {

        IWebFreightContext webFreightContext;
        public ObjectFieldValidationRepository()
        {
           // Context = new WebFreightContext();

        }
        public ObjectFieldValidationRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public ObjectFieldValidationRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<ObjectFieldValidation> GetObjectFieldValidations(int tenant)
        {
            return (from record in context.ObjectFieldValidations where record.Tenant == tenant select record);
        }

        public ObjectFieldValidation GetSingleObjectFieldValidation(string id, int tenant)
        {
            return (from record in context.ObjectFieldValidations where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

     

        public void Add(ObjectFieldValidation entity)
        {
            context.ObjectFieldValidations.Add(entity);
        }

        public void Remove(ObjectFieldValidation entity)
        {
            try
            {
                context.ObjectFieldValidations.Attach(entity);
            }
            catch { }
            context.ObjectFieldValidations.Remove(entity);



        }

        public void Update(ObjectFieldValidation entity)
        {
            try
            {
                context.ObjectFieldValidations.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);



        }

        public List<ObjectFieldValidation> All()
        {
            return context.ObjectFieldValidations.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectFieldValidation> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ObjectFieldValidation GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}