using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ComputingPartnerTableRepository : IRepository<ComputingPartnerTable>
    {
        ICommonDataContext context;
        public ICommonDataContext Context
        {
            get { return context; }
        }



        public ComputingPartnerTableRepository(int tenant)
        {
            this.context = CommonDataContext.GetContext(tenant);
        }

        public ComputingPartnerTableRepository(ICommonDataContext context)
        {
            this.context = context;
        }

        public IQueryable<ComputingPartnerTable> GetComputingPartnerTables(int tenant)
        {
            return (from d in Context.ComputingPartnerTables  where d.Tenant == tenant select d);
        }

        public IQueryable<ComputingPartnerTable> GetComputingPartnerTablesByPartnerId(int tenant, string computingPartnerId)
        {
            return (from d in Context.ComputingPartnerTables where d.Tenant == tenant && d.ComputingPartnerId == computingPartnerId select d);
        }

        public ComputingPartnerTable GetSingleComputingPartnerTable(int tenant, string objectTableId, string computingPartnerId)
        {
            return (from d in Context.ComputingPartnerTables where d.Tenant == tenant && d.ObjectTableId == objectTableId && d.ComputingPartnerId == computingPartnerId select d).FirstOrDefault();
        }

        public List<ComputingPartnerTable> All()
        {
            return Context.ComputingPartnerTables.ToList();
        }

        public List<ComputingPartnerTable> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ComputingPartnerTable GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void Add(ComputingPartnerTable entity)
        {
            Context.ComputingPartnerTables.Add(entity);
        }

        public void Remove(ComputingPartnerTable entity)
        {
            Context.ComputingPartnerTables.Attach(entity);
            Context.ComputingPartnerTables.Remove(entity);
        }

        public void Update(ComputingPartnerTable entity)
        {
            Context.ComputingPartnerTables.Attach(entity);
            Context.SetAsModified(entity);
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }
    }
}
