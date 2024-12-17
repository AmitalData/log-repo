using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentTypeCopyRepository:IRepository<DocumentTypeCopy>
    {
        ICommonDataContext commonDataContext;

        public DocumentTypeCopyRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentTypeCopyRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentTypeCopyRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public DocumentTypeCopy GetSingleDocumentTypeCopy(string id)
        {
            DocumentTypeCopy copy = (from a in context.DocumentTypeCopies.Include("DocumentType")
                                     where a.Id == id
                                     select a).FirstOrDefault();
            return copy;
        }

        public DocumentTypeCopy GetSingleDocumentTypeCopyByTenant(string id,int tenant)
        {
            DocumentTypeCopy copy = (from a in context.DocumentTypeCopies
                                     where a.Id == id && a.Tenant == tenant
                                     select a).FirstOrDefault();
            return copy;
        }

        public DocumentTypeCopy GetSingleDocumentTypeCopyByDocumentTypeId(string docTypeId, int tenant)
        {
            DocumentTypeCopy copy = (from a in context.DocumentTypeCopies
                                     where a.DocumentTypeId == docTypeId && a.Tenant == tenant
                                     select a).FirstOrDefault();
            return copy;
        }

        public List<DocumentTypeCopy> GetDocumentTypeCopiesByDocumentTypeIdTenant(string docTypeId, int tenant)
        {
            List<DocumentTypeCopy> copies = (from a in context.DocumentTypeCopies
                                     where a.DocumentTypeId == docTypeId && a.Tenant == tenant
                                     select a).ToList();
            return copies;
        }

        public void Add(DocumentTypeCopy entity)
        {
            context.DocumentTypeCopies.Add(entity);
        }

        public void Remove(DocumentTypeCopy entity)
        {
            context.DocumentTypeCopies.Attach(entity);
            context.DocumentTypeCopies.Remove(entity);
        }

        public void Update(DocumentTypeCopy entity)
        {
            context.DocumentTypeCopies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DocumentTypeCopy> All()
        {
            return context.DocumentTypeCopies.ToList();
        }

        public ICommonDataContext context
        {
            get {return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentTypeCopy> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentTypeCopy GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}