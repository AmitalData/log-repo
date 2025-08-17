using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Data.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentOutCopyRepository:IRepository<DocumentOutCopy>
    {
        ICommonDataContext commonDataContext;



        public DocumentOutCopyRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public DocumentOutCopyRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentOutCopy GetSingleDocumentOutCopy(string id)
        {
            return (from a in context.DocumentOutCopies.Include("DocumentTypeCopy").Include("DocumentOut.DocumentsFiling.DocumentType")
                    where a.Id == id
                    select a).FirstOrDefault();
        }
        public DocumentOutCopy GetDocumentOutCopy(DocumentOutCopyArgs args)
        {
            DocumentOutCopy documentOutCopy = context.DocumentOutCopies.Include("DocumentTypeCopy")
                .Include("DocumentOut.DocumentsFiling")
                .Where(e => e.DocumentTypeCopyId == args.DocumentTypeCopyId
                    && e.DocumentTypeCopy.DocumentTypeId == args.DocumentTypeId
                    && e.DocumentOut.DocumentsFiling.EntityId == args.EntityId
                    && e.DocumentOut.DocumentsFiling.ChildEntityId == args.ChildEntityId
                    && e.DocumentOut.DocumentsFiling.ObjectTableId == args.ObjectTableId
                    && e.Tenant == args.Tenant
                    && e.DocumentOut.DocumentTemplateId == args.DocumentTemplateId)
                .FirstOrDefault();

            return documentOutCopy;
        }

        public DocumentOutCopy GetSingleDocumentOutCopyByTenant(string id,int tenant)
        {
            return (from a in context.DocumentOutCopies.Include("DocumentTypeCopy").Include("DocumentOut.DocumentsFiling.DocumentType")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }


        public DocumentOutCopy GetDocumentOutCopyByDocumentOutAndType(string documentOutId, string documentTypeCopyId, int tenant)
        {
            return (from a in context.DocumentOutCopies.Include("DocumentTypeCopy").Include("DocumentOut.DocumentsFiling.DocumentType")
                    where a.DocumentOutId == documentOutId && a.DocumentTypeCopyId == documentTypeCopyId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public bool DocumentTypeCopyHasDocumentOutCopy(string documentTypeCopyId,string documentOutId)
        {
            bool hasDoc = (from a in context.DocumentOutCopies.Include("DocumentTypeCopy").Include("DocumentOut.DocumentsFiling.DocumentType")
                           where a.DocumentTypeCopyId == documentTypeCopyId && a.DocumentOutId==documentOutId
                           select a).Any();
            return hasDoc;
        }

        public DocumentOutCopy GetThePrintedOnceDocumentOutCopyForDocumentOut(string documentOutId, int tenant)
        {
            return (from a in context.DocumentOutCopies
                    where a.DocumentOutId == documentOutId && a.Tenant == tenant&& !string.IsNullOrEmpty(a.LastPrintedByUserId)
                    select a).FirstOrDefault();
        }

        public void Add(DocumentOutCopy entity)
        {
            context.DocumentOutCopies.Add(entity);
        }

        public List<DocumentOutCopy> GetDocumentOutCopyByDocumentOutId(string documentOutId, int tenant)
        {
            return (from a in context.DocumentOutCopies
                    where a.DocumentOutId == documentOutId && a.Tenant == tenant
                    select a).ToList();
        }

        public void Remove(DocumentOutCopy entity)
        {
            context.DocumentOutCopies.Attach(entity);
            context.DocumentOutCopies.Remove(entity);
        }

        public void Update(DocumentOutCopy entity)
        {
            try
            {
                context.DocumentOutCopies.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<DocumentOutCopy> All()
        {
            return context.DocumentOutCopies.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentOutCopy> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentOutCopy GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}