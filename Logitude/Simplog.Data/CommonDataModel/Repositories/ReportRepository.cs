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
    public class ReportRepository : IRepository<Report>
    {
       ICommonDataContext commonDataContext;

        public ReportRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public ReportRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ReportRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }



        public IQueryable<Report> GetReports(int tenant)
        {
            return (from record in context.Reports where record.Tenant == tenant select record);
        }

        public IQueryable<Report> GetReportsWithLocalNamesTenant0()
        {
            return (from record in context.Reports where record.Tenant == 0 && record.LocalName != null select record);
        }

        public Report GetSingleReport(string id, int tenant)
        {
            var temp = (from record in context.Reports where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
            if (temp == null)
            {
                temp = (from record in context.Reports where record.Id == id && record.Tenant == 0 select record).FirstOrDefault();
            }
            return temp;
        }

        public Report GetSingleReport(string id)
        {
            Report entity = (from record in context.Reports.Include("Feature") where record.Id == id  select record).FirstOrDefault();
            return entity;
        }
       
        public void Add(Report entity)
        {
            this.context.Reports.Add(entity);
        }

        public void Remove(Report entity)
        {
            try
            {
                this.context.Reports.Attach(entity);
            }
            catch { }
            this.context.Reports.Remove(entity);
        }

        public void Update(Report entity)
        {
            try
            {
                this.context.Reports.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<Report> All()
        {
            return this.context.Reports.ToList<Report>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public Report GetReportByName(string name, int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in context.Reports
                         where a.Tenant == tenant && a.Name == name
                         select  a).FirstOrDefault();

            return query;          
        }

        public Report GetReportByCode(string code, int tenant)
        {           
            var query = (from a in context.Reports
                         where a.Tenant == tenant && a.Code == code
                         select a).FirstOrDefault();

            return query;
        }

        public List<Report> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Report GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Report GetSingleReportByCode(string code, int tenant)
        {
            return (from f in context.Reports where f.Code == code && f.Tenant == tenant select f).FirstOrDefault();
        }

        public IQueryable<Report> GetReportsExceptTenant0()
        {
            return (from record in context.Reports
                    where record.Tenant != 0
                    select record);
        }
    }
}
