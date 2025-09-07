using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PortGroupRepository : IRepository<PortGroup>
    {
        ICommonDataContext commonDataContext;



        public PortGroupRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public PortGroupRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<PortGroup> GetPortGroups(int tenant)
        {
            return (from record in context.PortGroups where record.Tenant == tenant select record);
        }

        public PortGroup GetSinglePortGroup(string id, int tenant)
        {
            return (from record in context.PortGroups where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public PortGroup GetSinglePortGroupByCode(string code, int tenant)
        {
            return (from record in context.PortGroups where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(PortGroup entity)
        {
            this.context.PortGroups.Add(entity);
        }

        public void Remove(PortGroup entity)
        {
            try
            {
                this.context.PortGroups.Attach(entity);
            }
            catch { }
            this.context.PortGroups.Remove(entity);

        }

        public void Update(PortGroup entity)
        {
            try
            {
                this.context.PortGroups.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);

        }

        public List<PortGroup> All()
        {
            return this.context.PortGroups.ToList<PortGroup>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<PortGroup> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public PortGroup GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}