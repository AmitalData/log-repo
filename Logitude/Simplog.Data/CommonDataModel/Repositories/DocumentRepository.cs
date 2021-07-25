using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentRepository : IRepository<Document>
    {
        ICommonDataContext commonDataContext;

        public DocumentRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Document> GetDocuments(int tenant)
        {
            return (from record in context.Documents where record.Tenant == tenant select record);
        }

        public Document GetSingleDocument(int tenant, string id)
        {
            Document d = (from a in context.Documents
                          where a.Id == id && a.Tenant == tenant
                          select a).FirstOrDefault();
            return d;
        }

        public Document GetSingleDocument(string id)
        {
            Document d = (from a in context.Documents
                          where a.Id == id
                          select a).FirstOrDefault();
            return d;
        }

        public List<Document> GetDocumentsByDocumentIds(List<string>documentIds, int tenant )
        {
            List<Document> result = (from a in context.Documents
                                     where documentIds.Contains(a.Id) && a.Tenant == tenant
                                     select a).ToList();
            return result;
        }
        public List<Document> GetDocumentsByIds(List<string> documentIds)
        {
            List<Document> result = (from a in context.Documents
                                     where documentIds.Contains(a.Id) 
                                     select a).ToList();
            return result;
        }

        public void Add(Document entity)
        {
            if (entity != null)
            {
                entity.IsEncrypted = true;
            }
            
            context.Documents.Add(entity);
        }

        public void Remove(Document entity)
        {
            context.Documents.Attach(entity);
            context.Documents.Remove(entity);
        }

        public void Update(Document entity)
        {

            try
            {

                context.Documents.Attach(entity);

            }
            catch { }

            context.SetAsModified(entity);
        }


        public List<Document> All()
        {
            return context.Documents.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<Document> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Document GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public double? GetUsedSpaceForTenant(int tenant)
        {
            double? usedSpace = (from a in context.Documents
                                 where a.Tenant == tenant
                                 select a).Sum(d => d.FileSize);
            return usedSpace;
        }
        public string GetCalculatedFileNameById(string id,int tenant)
        {
           string calculatedFileName = (from a in context.Documents
                                 where a.Tenant == tenant && a.Id == id
                                 select a.CalculatedFileName).FirstOrDefault();
            return calculatedFileName;
        }

        

        public bool CheckIfDocumentsExistOnTenant(List<string> documentIds, int tenant)
        {
            bool isValid = (from a in context.Documents
                            where documentIds.Contains(a.Id) && a.Tenant != tenant
                            select a).Any();
            return !isValid;
        }

        public string GetFileNameByDocumentId(string id, int tenant)
        {
            string calculatedFileName = (from a in context.Documents
                                         where a.Tenant == tenant && a.Id == id
                                         select a.FileName).FirstOrDefault();
            return calculatedFileName;
        }

    }
}
