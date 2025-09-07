using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class LeadSourceRepository : IRepository<LeadSource>
    {
        ICommonDataContext commonDataContext;



        public LeadSourceRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public LeadSourceRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<LeadSource> GetLeadSources(int tenant)
        {
            return (from record in context.LeadSources where record.Tenant == tenant select record);
        }
        
        public LeadSource GetSingleLeadSource(string id, int tenant)
        {
            return (from record in context.LeadSources where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public LeadSource GetSingleLeadSourceByCode(string code, int tenant)
        {
            return (from record in context.LeadSources where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();            
        }
        
        public void Add(LeadSource entity)
        {
            this.context.LeadSources.Add(entity);
        }

        public void Remove(LeadSource entity)
        {
            try
            {
                this.context.LeadSources.Attach(entity);
            }
            catch { }
            this.context.LeadSources.Remove(entity);

        }

        public void Update(LeadSource entity)
        {
            try
            {
                this.context.LeadSources.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);

        }

        public List<LeadSource> All()
        {
            return this.context.LeadSources.ToList<LeadSource>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<LeadSource> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public LeadSource GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
