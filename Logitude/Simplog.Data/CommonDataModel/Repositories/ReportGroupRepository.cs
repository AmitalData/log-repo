using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
   public  class ReportGroupRepository: IRepository<ReportGroup>
    {
        ICommonDataContext commonDataContext;
        public ReportGroupRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public ReportGroupRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ReportGroupRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<ReportGroup> GetReportGroups(int tenant)
        {
            return (from record in context.ReportGroups where record.Tenant == tenant select record);
        }

        public ReportGroup GetSingleReportGroup(string id, int tenant)
        {
            return (from record in context.ReportGroups where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }        
       
        public void Add(ReportGroup entity)
        {
            this.context.ReportGroups.Add(entity);
        }

        public void Remove(ReportGroup entity)
        {
            try
            {
                this.context.ReportGroups.Attach(entity);
            }
            catch { }
            this.context.ReportGroups.Remove(entity);
        }

        public void Update(ReportGroup entity)
        {
            try
            {
                this.context.ReportGroups.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<ReportGroup> All()
        {
            return this.context.ReportGroups.ToList<ReportGroup>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }     

        public ReportGroup GetReportGroupByName(string name, int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in context.ReportGroups
                         where a.Tenant == tenant && a.EnglishName == name
                         select  a).FirstOrDefault();

            return query;          
        }

        public List<ReportGroup> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ReportGroup GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ReportGroup GetReportGroupByCode(string code, int tenant)
        {
            var query = (from a in context.ReportGroups
                         where a.Tenant == tenant && a.Code == code
                         select a).FirstOrDefault();

            return query;
        }
    }
}
