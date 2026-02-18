using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class BusinessUnitRepository : IRepository<BusinessUnit>
    {
        ICommonDataContext Context;
        public ICommonDataContext context
        {
            get { return Context; }
        }



        public BusinessUnitRepository(int tenant)
        {
            this.Context = CommonDataContext.GetContext(tenant);
        }

        public BusinessUnitRepository(ICommonDataContext context)
        {
            this.Context = context;
        }

        public BusinessUnit GetSingleBusinessUnit(string id, int tenant)
        {
            return (from a in context.BusinessUnits where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<BusinessUnit> GetBusinessUnits(int tenant)
        {
            return context.BusinessUnits.Where(d => d.Tenant == tenant);
        }

        public List<string> GetAllIds(int tenant)
        {
            List<string> myResult = context.BusinessUnits.Where(d => d.Tenant == tenant).Select(s => s.Id).ToList();
            return myResult;
        }

        public bool IsBusinessUnitExists(int tenant)
        {
            return context.BusinessUnits.Where(d => d.Tenant == tenant && d.Name == "Organization").Any();
        }

        public void Add(BusinessUnit entity)
        {
            context.BusinessUnits.Add(entity);
        }

        public void Remove(BusinessUnit entity)
        {
            context.BusinessUnits.Remove(entity);
        }

        public void Update(BusinessUnit entity)
        {
            context.BusinessUnits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BusinessUnit> All()
        {
            return context.BusinessUnits.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<BusinessUnit> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public BusinessUnit GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}
