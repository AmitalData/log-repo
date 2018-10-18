using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ScreenFieldsRepository : IRepository<ScreenField>
    {

         IWebFreightContext webFreightContext;
        public ScreenFieldsRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public ScreenFieldsRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public ScreenFieldsRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
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
            webFreightContext.ScreenFields.Add(entity);
        }

        public void Remove(ScreenField entity)
        {
            this.webFreightContext.ScreenFields.Attach(entity);
            this.webFreightContext.ScreenFields.Remove(entity);
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

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<ScreenField> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ScreenField GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}