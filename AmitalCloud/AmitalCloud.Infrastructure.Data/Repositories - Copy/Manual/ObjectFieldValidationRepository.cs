using System.Collections.Generic;
using System.Linq;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class ObjectFieldValidationRepository:IRepository<ObjectFieldValidation, string>
    {

        IAmitalCloudContext amitalCloudContext;
        public ObjectFieldValidationRepository() : this(0)
        {
        }
        public ObjectFieldValidationRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;
        }
        public ObjectFieldValidationRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
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

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectFieldValidation> GetMulti(IEntityKeyFields<ObjectFieldValidation,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ObjectFieldValidation GetSingle(IEntityKeyFields<ObjectFieldValidation,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}