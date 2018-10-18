using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentStatusRepository:IRepository<DocumentStatus>
    {
        ICommonDataContext commonDataContext;

        public DocumentStatusRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentStatusRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentStatusRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<DocumentStatus> GetDocumentStatuss()
        {
            return context.DocumentStatus;
        }

        public DocumentStatus GetSingleDocumentStatus(string code)
        {
            return (from record in context.DocumentStatus where record.Code == code select record).FirstOrDefault();
        }

        public void Add(DocumentStatus entity)
        {
            context.DocumentStatus.Add(entity);
        }

        public void Remove(DocumentStatus entity)
        {
            context.DocumentStatus.Attach(entity);
            context.DocumentStatus.Remove(entity);
        }

        public void Update(DocumentStatus entity)
        {
            context.DocumentStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DocumentStatus> All()
        {
            return context.DocumentStatus.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
