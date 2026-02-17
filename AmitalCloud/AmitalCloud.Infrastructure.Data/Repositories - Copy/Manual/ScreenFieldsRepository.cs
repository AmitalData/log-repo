using System.Collections.Generic;
using System.Linq;
using AmitalCloud.Infrastructure.Data.Interfaces;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Context;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class ScreenFieldsRepository : IRepository<ScreenField, string>
    {

         IAmitalCloudContext amitalCloudContext;
        public ScreenFieldsRepository()
        {
            amitalCloudContext = new AmitalCloudContext();
        }
        public ScreenFieldsRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;
        }

        public ScreenFieldsRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
        }
        public IQueryable<ScreenField> GetScreenFields()
        {
            return this.context.ScreenFields;
        }

        public IQueryable<ScreenField> GetScreenFieldsByTenant(int tenant)
        {
            IQueryable<ScreenField> screenfields = from a in context.ScreenFields
                                                   where a.Tenant == tenant
                                                   select a;
            return screenfields;
        }

      

        public ScreenField GetSingleScreenField(string id)
        {
            return (from a in context.ScreenFields
                    where a.Id == id
                    select a).FirstOrDefault();
        }


        public void Add(ScreenField entity)
        {
            context.ScreenFields.Add(entity);
        }

        public void Remove(ScreenField entity)
        {
            this.context.ScreenFields.Attach(entity);
            this.context.ScreenFields.Remove(entity);
        }

        public void Update(ScreenField entity)
        {
            try
            {
                this.context.ScreenFields.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<ScreenField> All()
        {
            return this.context.ScreenFields.ToList<ScreenField>();
        }

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<ScreenField> GetMulti(IEntityKeyFields<ScreenField,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ScreenField GetSingle(IEntityKeyFields<ScreenField,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}