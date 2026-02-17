using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentsExecutionLogRepository : IRepository<DocumentsExecutionLog>
    {
        ICommonDataContext commonDataContext;

        public DocumentsExecutionLogRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentsExecutionLogRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentsExecutionLogRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<DocumentsExecutionLog> GetDocumentsExecutionLogs(int tenant)
        {
            return (from record in context.DocumentsExecutionLogs where record.Tenant == tenant select record);
        }

        public DocumentsExecutionLog GetSingleDocumentsExecutionLog(string id, int tenant)
        {
            return (from record in context.DocumentsExecutionLogs where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }


        public void Add(DocumentsExecutionLog entity)
        {
            context.DocumentsExecutionLogs.Add(entity);
        }

        public void Remove(DocumentsExecutionLog entity)
        {
            try
            {
                context.DocumentsExecutionLogs.Attach(entity);
            }
            catch { };
            context.DocumentsExecutionLogs.Remove(entity);
        }

        public void Update(DocumentsExecutionLog entity)
        {
            try
            {
                context.DocumentsExecutionLogs.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<DocumentsExecutionLog> All()
        {
            return context.DocumentsExecutionLogs.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentsExecutionLog> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentsExecutionLog GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}