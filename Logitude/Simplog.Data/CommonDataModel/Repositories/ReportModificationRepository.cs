using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ReportModificationRepository: IRepository<ReportModification>
    {
        ICommonDataContext commonDataContext;



        public ReportModificationRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }

        public ReportModificationRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public ReportModification GetSingleReportModification(string reportId, int tenant)
        {
            return (from a in this.context.ReportModifications
                    where a.ReportId == reportId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ReportModification> GetReportModifications(int tenant)
        {
            return (from a in this.context.ReportModifications
                    where a.Tenant == tenant
                    select a);
        }

        public void Add(ReportModification entity)
        {
            this.context.ReportModifications.Add(entity);
        }

        public void Remove(ReportModification entity)
        {

            this.context.ReportModifications.Attach(entity);

            this.context.ReportModifications.Remove(entity);
        }

        public void Update(ReportModification entity)
        {
            this.context.ReportModifications.Attach(entity);

            this.context.SetAsModified(entity);
        }

        public List<ReportModification> All()
        {
            return this.context.ReportModifications.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<ReportModification> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ReportModification GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}