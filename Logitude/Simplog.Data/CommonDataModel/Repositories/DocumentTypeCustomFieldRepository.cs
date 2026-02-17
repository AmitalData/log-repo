using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentTypeCustomFieldRepository:IRepository<DocumentTypeCustomField>
    {
        ICommonDataContext commonDataContext;

        public DocumentTypeCustomFieldRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentTypeCustomFieldRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentTypeCustomFieldRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<DocumentTypeCustomField> GetDocumentTypeCustomFields(int tenant)
        {
            return (from record in context.DocumentTypeCustomFields where record.Tenant == tenant select record);
        }

        public DocumentTypeCustomField GetSingleDocumentTypeCusotmField(string id, int tenant)
        {
            return (from record in context.DocumentTypeCustomFields where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<DocumentTypeCustomField> GetDocumentTypeCusotmFieldsByDocumentTypeId(string documentTypeId, int tenant)
        {
            var documentTypeCustom = from a in context.DocumentTypeCustomFields
                                     where a.Tenant == tenant && a.DocumentTypeId==documentTypeId
                                     select a;
            return documentTypeCustom;
        }

        public void Add(DocumentTypeCustomField entity)
        {
            context.DocumentTypeCustomFields.Add(entity);
        }

        public void Remove(DocumentTypeCustomField entity)
        {
            context.DocumentTypeCustomFields.Attach(entity);
            context.DocumentTypeCustomFields.Remove(entity);
        }

        public void Update(DocumentTypeCustomField entity)
        {
            context.DocumentTypeCustomFields.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DocumentTypeCustomField> All()
        {
            return context.DocumentTypeCustomFields.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentTypeCustomField> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentTypeCustomField GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}