

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DWObjectFieldRepository : IRepository<DWObjectField>
    {
        public IWebFreightContext webFreightContext;

        public DWObjectFieldRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DWObjectFieldRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public DWObjectFieldRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(DWObjectField entity)
        {
            webFreightContext.DWObjectFields.Add(entity);
        }

        public void Remove(DWObjectField entity)
        {
            webFreightContext.DWObjectFields.Attach(entity);
            webFreightContext.DWObjectFields.Remove(entity);
        }

        public void Update(DWObjectField entity)
        {
            webFreightContext.DWObjectFields.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<DWObjectField> All()
        {
            return webFreightContext.DWObjectFields.ToList();
        }


        public IQueryable<DWObjectField> GetObjectsByTenant(int tenant)
        {
            IQueryable<DWObjectField> dWObjectField = from a in webFreightContext.DWObjectFields
                                                      where a.Tenant == tenant || a.Tenant == 0
                                                      select a;
            return dWObjectField;
        }



        public DWObjectField GetSingleDWObjectField(string Id, int Tenant)
        {
            return webFreightContext.DWObjectFields.Where(a => a.Id == Id && a.Tenant == Tenant).FirstOrDefault();
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<DWObjectField> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DWObjectField GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DWObjectField> GetDWObjectFields(int tenant)
        {
            return webFreightContext.DWObjectFields.Where(a => a.Tenant == tenant);
        }

    }
}
