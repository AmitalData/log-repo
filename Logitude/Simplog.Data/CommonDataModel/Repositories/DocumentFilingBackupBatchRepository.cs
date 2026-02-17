using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentFilingBackupBatchRepository : IRepository<DocumentFilingBackupBatch>
    {
          ICommonDataContext commonDataContext;

        public DocumentFilingBackupBatchRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentFilingBackupBatchRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<DocumentFilingBackupBatch> GetCustomerTenantAccessCradsBatchByTenant(int tenant)
        {
            return from a in context.DocumentFilingBackupBatches where a.Tenant == tenant select a;
        } 

        public DocumentFilingBackupBatch GetSingleDocumentFilingBackupBatch(string Id,int Tenant)
        {
            return (from record in context.DocumentFilingBackupBatches where record.Id == Id && record.Tenant == Tenant  select record).FirstOrDefault();
        }

        public IQueryable<DocumentFilingBackupBatch> GetDocumentFilingBackupBatches(int tenant)
        {
            return from a in context.DocumentFilingBackupBatches where a.Tenant == tenant select a;
        }

        


        public DocumentFilingBackupBatch GetSingleCustomerTenantAccessCard1sBatch( string BatchNumber, int Tenant)
        {
            return (from record in context.DocumentFilingBackupBatches where record.BatchNumber == BatchNumber && record.Tenant == Tenant select record).FirstOrDefault();
        }
        public DocumentFilingBackupBatchRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void Add(DocumentFilingBackupBatch entity)
        {
            context.DocumentFilingBackupBatches.Add(entity);
        }

        public void Remove(DocumentFilingBackupBatch entity)
        {
            context.DocumentFilingBackupBatches.Attach(entity);
            context.DocumentFilingBackupBatches.Remove(entity);
        }

        public void Update(DocumentFilingBackupBatch entity)
        {
            context.DocumentFilingBackupBatches.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DocumentFilingBackupBatch> All()
        {
            return context.DocumentFilingBackupBatches.ToList();

        }

        public List<DocumentFilingBackupBatch> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentFilingBackupBatch GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
    }
}
