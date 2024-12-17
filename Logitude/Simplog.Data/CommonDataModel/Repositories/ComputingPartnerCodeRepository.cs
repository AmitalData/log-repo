using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ComputingPartnerCodeRepository : IRepository<ComputingPartnerCode>
    {
        ICommonDataContext context;
        public ICommonDataContext Context
        {
            get { return context; }
        }

        public ComputingPartnerCodeRepository()
        {
            this.context = new CommonDataContext();
        }

        public ComputingPartnerCodeRepository(int tenant)
        {
            this.context = CommonDataContext.GetContext(tenant);
        }

        public ComputingPartnerCodeRepository(ICommonDataContext context)
        {
            this.context = context;
        }

        public IQueryable<ComputingPartnerCode> GetComputingPartnerCodes(int tenant)
        {
            return (from d in Context.ComputingPartnerCodes where d.Tenant == tenant select d);
        }

        public ComputingPartnerCode GetSingleComputingPartnerCode(string id, int tenant)
        {
            return (from d in Context.ComputingPartnerCodes where d.Tenant == tenant && d.Id == id select d).FirstOrDefault();
        }

        public List<ComputingPartnerCode> All()
        {
            return Context.ComputingPartnerCodes.ToList();
        }

        public List<ComputingPartnerCode> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ComputingPartnerCode GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void Add(ComputingPartnerCode entity)
        {
            Context.ComputingPartnerCodes.Add(entity);
        }

        public void Remove(ComputingPartnerCode entity)
        {
            Context.ComputingPartnerCodes.Attach(entity);
            Context.ComputingPartnerCodes.Remove(entity);
        }

        public void Update(ComputingPartnerCode entity)
        {
            Context.ComputingPartnerCodes.Attach(entity);
            Context.SetAsModified(entity);
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }
    }
}
