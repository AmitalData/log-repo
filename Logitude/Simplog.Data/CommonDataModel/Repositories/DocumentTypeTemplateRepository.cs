using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentTypeTemplateRepository:IRepository<DocumentTypeTemplate>
    {
        ICommonDataContext commonDataContext;

        public DocumentTypeTemplateRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentTypeTemplateRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentTypeTemplateRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<DocumentTypeTemplate> GetDocumentTypeTemplatesByTenant(int tenant)
        {
            return from a in context.DocumentTypeTemplates.Include("LastUpdatedByUser.Contact").Include("DocumentType")
                   where a.Tenant == tenant && (string.IsNullOrEmpty(a.AutomationId) && string.IsNullOrEmpty(a.EntityId))
                   select a;
        }


        public IQueryable<DocumentTypeTemplate> GetDocumentTypeTemplates(int tenant)
        {
            return from a in context.DocumentTypeTemplates.Include("LastUpdatedByUser.Contact").Include("DocumentType")
                   where a.Tenant == tenant && (string.IsNullOrEmpty(a.AutomationId) && string.IsNullOrEmpty(a.EntityId))
                   select a;
        }


        public IQueryable<DocumentTypeTemplate> GetHtmDocumentTypeTemplates(int tenant)
        {
            return from a in context.DocumentTypeTemplates
                   where a.Tenant == tenant && a.EditorTool !="S" && (string.IsNullOrEmpty(a.AutomationId) && string.IsNullOrEmpty(a.EntityId))
                   select a;
        }


    


        public DocumentTypeTemplate GetSingleDocumentTypeTemplate(string id)
        {
            return (from a in context.DocumentTypeTemplates.Include("LastUpdatedByUser.Contact").Include("DocumentType")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public DocumentTypeTemplate GetSingleDocumentTypeTemplateWithOutInClude(string id, int tenant)
        {
            return (from a in context.DocumentTypeTemplates
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }



        public List<DocumentTypeTemplate> GetDocumentTypeTemplatesByDocumentTypeId(int tenant, string documentTypeId)
        {
            return (from a in context.DocumentTypeTemplates
                   where a.Tenant == tenant && a.DocumentTypeId == documentTypeId
                   select a).ToList();
        }



        public DocumentTypeTemplate GetSingleDocumentTypeTemplate(string id,int tenant)
        {
            return (from a in context.DocumentTypeTemplates.Include("LastUpdatedByUser.Contact").Include("DocumentType")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }


        public DocumentTypeTemplate GetSingleDocumentTypeTemplateByTenant(string id , int tenant)
        {
            return (from a in context.DocumentTypeTemplates.Include("LastUpdatedByUser.Contact").Include("DocumentType")
                    where a.Id == id && a.Tenant ==tenant && a.InActive == false 
                    select a).FirstOrDefault();
        }

        public void Add(DocumentTypeTemplate entity)
        {
            context.DocumentTypeTemplates.Add(entity);
        }

        public void Remove(DocumentTypeTemplate entity)
        {
            context.DocumentTypeTemplates.Attach(entity);
            context.DocumentTypeTemplates.Remove(entity);
        }

        public void Update(DocumentTypeTemplate entity)
        {
            try
            {
                context.DocumentTypeTemplates.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<DocumentTypeTemplate> All()
        {
            return context.DocumentTypeTemplates.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentTypeTemplate> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentTypeTemplate GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }


        public IQueryable<DocumentTypeTemplate> GetDocumentTypeTemplatesBydocumentTypeTemplateIds(List<string>documentTypeTemplateIds,int tenant)
        {
            return from a in context.DocumentTypeTemplates
                   where a.Tenant == tenant && documentTypeTemplateIds.Contains(a.Id)
                   select a;
        }
    }
}