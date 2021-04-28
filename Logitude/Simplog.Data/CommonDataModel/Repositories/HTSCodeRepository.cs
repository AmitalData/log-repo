using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class HTSCodeRepository
    {

        ICommonDataContext commonDataContext;

        public HTSCodeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public HTSCodeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public HTSCodeRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<HTSCode> GetHTSCodes()
        {
            return context.HTSCodes;
        }
        public IQueryable<HTSCode> GetAll()
        {
            return context.HTSCodes;
        }

        public HTSCode GetSingleHTSCode(string id,int tenant)
        {
            return (from a in context.HTSCodes.Include("Card").Include("Country")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(HTSCode entity)
        {
            commonDataContext.HTSCodes.Add(entity);
        }

        public void Remove(HTSCode entity)
        {
            commonDataContext.HTSCodes.Remove(entity);
        }

        public void Update(HTSCode entity)
        {
            commonDataContext.HTSCodes.Attach(entity);
            commonDataContext.SetAsModified(entity);
        }

        public List<HTSCode> All()
        {
            return context.HTSCodes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            commonDataContext.SaveChanges();
        }

        public List<HTSCode> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public HTSCode GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
