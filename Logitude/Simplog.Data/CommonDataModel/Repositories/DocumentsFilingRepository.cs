using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentsFilingRepository:IRepository<DocumentsFiling>
    {
        ICommonDataContext commonDataContext;

        public DocumentsFilingRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentsFilingRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public DocumentsFilingRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<DocumentsFiling> GetDocumentsFilings(int tenant)
        {
            return (from record in context.DocumentsFilings.Include("CreatedByUser.Contact").Include("Document").Include("Owner.Contact").Include("ObjectTable").Include("DocumentType")
                    where record.Tenant == tenant select record);
        }

        public DocumentsFiling GetSingleDocumentsFiling(string id, int tenant)
        {
            DocumentsFiling d = (from a in context.DocumentsFilings.Include("CreatedByUser.Contact").Include("Document").Include("Owner.Contact").Include("ObjectTable").Include("DocumentType")
                                  where a.Id == id && a.Tenant == tenant
                                  select a).FirstOrDefault();
            return d;
        }


        public List<DocumentsFiling> GetDocumentsFilingsByEntityId(string entityId, int tenant)
        {
            List<DocumentsFiling> externalDocuments = (from a in context.DocumentsFilings.Include("CreatedByUser.Contact").Include("Document").Include("Owner.Contact").Include("ObjectTable").Include("DocumentType")
                                                  where (a.EntityId == entityId || a.ChildEntityId == entityId)  && a.Tenant == tenant
                                                  select a).ToList();
            return externalDocuments;
        }

        public List<DocumentsFiling> GetRequestedDocumentsFilingPMsByEntityId(string entityId, int tenant)
        {
            List<DocumentsFiling> externalDocuments = (from a in context.DocumentsFilings.Include("CreatedByUser.Contact").Include("Document").Include("Owner.Contact").Include("ObjectTable").Include("DocumentType")
                                                       where (a.EntityId == entityId) && a.Tenant == tenant && (a.IsDigitalSignRequired == true || a.IsRequested == true)
                                                       select a).ToList();
            return externalDocuments;
        }

        public List<DocumentsFiling> GetDocumentsFilingsForEntityTableId(string entityId, string objecttableId, int tenant)
        {
            List<DocumentsFiling> externalDocuments = (from a in context.DocumentsFilings.Include("CreatedByUser.Contact").Include("Document").Include("Owner.Contact").Include("ObjectTable").Include("DocumentType")
                                                       where (a.EntityId == entityId || a.ChildEntityId == entityId) && (a.ObjectTableId == objecttableId || a.ChildObjectTableId == objecttableId) && a.Tenant == tenant
                                                  select a).ToList();
            return externalDocuments;
        }

        public DocumentsFiling GetSingleDocumentsFilingByCode(string code, int tenant)
        {
            var externalDocument = (from a in context.DocumentsFilings.Include("CreatedByUser.Contact").Include("Document").Include("Owner.Contact").Include("ObjectTable").Include("DocumentType")
                                                  where a.Code == code && a.Tenant == tenant
                                                  select a).FirstOrDefault();
           return externalDocument;
        }
        public DocumentsFiling GetSingleDocumentsFilingByDocumentId(string DocumentId, int tenant, bool fromCache = true)
        {

            string name = "SingleDocumentsFilingByDocumentId" + DocumentId + "," + tenant;

            DocumentsFiling externalDocument = null;
            externalDocument = CacheManager.GetOrInsertNewObject<DocumentsFiling>(name, () =>
        {
            var poco = (from a in context.DocumentsFilings
                        ///.Include("CreatedByUser.Contact").Include("Document").Include("Owner.Contact").Include("ObjectTable").Include("DocumentType")
                        where a.DocumentId == DocumentId && a.Tenant == tenant
                        select a).FirstOrDefault();
            return poco;

        }, fromCache, true);


            return externalDocument;

        }
        public bool CheckIfDocumentTypeHasDocumentFilling(string documentTypeId, string objectTableId, string entityId, int tenant)
        {

            return (
                GetByDocType(documentTypeId, objectTableId, entityId, tenant)
                    ).Any();

        }

        private IQueryable<DocumentsFiling> GetByDocType(string documentTypeId, string objectTableId, string entityId, int tenant)
        {
            
            var q = (from a in context.DocumentsFilings
                     where a.DocumentTypeId == documentTypeId && a.Tenant == tenant && a.ObjectTableId == objectTableId && a.EntityId == entityId
                     select a);
            var isItFaster = false;
            if (isItFaster)
            {
                if (!String.IsNullOrWhiteSpace(documentTypeId) && !String.IsNullOrWhiteSpace(objectTableId) && !String.IsNullOrWhiteSpace(entityId))
                {

                    q = q.Where(r => !(r.ObjectTableId == null || r.ObjectTableId.Trim() == String.Empty));
                    q = q.Where(r => !(r.EntityId == null || r.EntityId.Trim() == String.Empty));

                }
            }
            return q;
        }

        public string GetDocumentIdByDocumentType(string documentTypeId, string objectTableId, string entityId, int tenant ,out string DocumentsFilingId)
        {
            DocumentsFilingId = null;
            bool ihabIsSureItsBetter = true;
            if (ihabIsSureItsBetter)
            {
                var q = (from a in context.DocumentsFilings
                         where
                         //a.DocumentTypeId == documentTypeId && 
                         a.Tenant == tenant && a.ObjectTableId == objectTableId && a.EntityId == entityId
                         select new
                         {
                             a.Id,
                             a.DocumentTypeId,
                             a.DocumentId
                         });
                var allForEntity = q.ToList();
                var rec4DocumentTypeId = allForEntity.FirstOrDefault(a => a.DocumentTypeId == documentTypeId);
                if (rec4DocumentTypeId == null) return null;
                DocumentsFilingId = rec4DocumentTypeId.Id;
                return rec4DocumentTypeId.DocumentId;
            }
            

            string resDocumentId = GetByDocType(documentTypeId, objectTableId, entityId, tenant).Select(r => r.DocumentId).FirstOrDefault();
            if (String.IsNullOrWhiteSpace(resDocumentId))
            {
                return null;
            }
            return resDocumentId;
            var poco = GetByDocType(documentTypeId, objectTableId, entityId, tenant).FirstOrDefault();
            
            if (poco == null) return null;
            return poco.DocumentId;
        }

        public List<DocumentsFiling> GetDocumentsFilingsByIds(string ids, int tenant)
        {
            string[] docFilingIds = ids.Split(',');

            List<DocumentsFiling> d = (from a in context.DocumentsFilings.Include("DocumentType").Include("ObjectTable")
                                       where docFilingIds.Contains(a.Id) && a.Tenant == tenant
                                 select a).ToList();
            return d;
        }

        public void Add(DocumentsFiling entity)
        {

            if (string.IsNullOrEmpty(entity.ForwarderDocumentId))
            {
                entity.ComputedForwarderDocumentId = entity.Id;
            }
            else
            {
                entity.ComputedForwarderDocumentId = entity.ForwarderDocumentId;
            }

            context.DocumentsFilings.Add(entity);
        }

        public void Remove(DocumentsFiling entity)
        {
            context.DocumentsFilings.Attach(entity);
            context.DocumentsFilings.Remove(entity);
        }

        public void Update(DocumentsFiling entity)
        {
            try
            {
                context.DocumentsFilings.Attach(entity);
            }
            catch
            { 
            }
            context.SetAsModified(entity);
            

        }

        public IQueryable<DocumentsFiling> GetAll(int tenant)
        {
            return context.DocumentsFilings.Where( r=>r .Tenant == tenant)  
                ;

        }

        public List<DocumentsFiling> All()
        {
            return context.DocumentsFilings.ToList();
            
        }

        public ICommonDataContext context
        {
          
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentsFiling> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentsFiling GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public string GetEntityDocumentsSearchFields(string EntityId, string ObjectTableId, int tenant)
        {
            if (!string.IsNullOrEmpty(EntityId))
            {

                var entity = (from a in context.DocumentsFilings
                                          where a.EntityId == EntityId && a.Tenant == tenant && a.ObjectTableId == ObjectTableId
                                 select  a.SearchFields).ToArray();
                var SField = String.Join(",", entity);
                return SField;
            }
            return null;
        }
        public DocumentsFiling GetSingleDocumentsFiling(string id)
        {
            DocumentsFiling d = (from a in context.DocumentsFilings.Include("CreatedByUser.Contact").Include("Document").Include("Owner.Contact").Include("ObjectTable").Include("DocumentType")
                                 where a.Id == id 
                                 select a).FirstOrDefault();
            return d;
        }

        public DocumentsFiling GetSingleDocumentFilingBySecurityId(string id, int tenant)
        {
            DocumentsFiling d = (from a in context.DocumentsFilings
                                 where a.SecurityId == id && a.Tenant == tenant
                                 select a).FirstOrDefault();
            return d;
        }

        public DocumentsFiling GetSingleDocumentFilingBySecurityId(string id)
        {
            DocumentsFiling d = (from a in context.DocumentsFilings
                                 where a.SecurityId == id 
                                 select a).FirstOrDefault();
            return d;
        }


        public string GetDocumentIdByChild(string documentTypeId, string ChildEntityReference, int tenant)
        {
            DocumentsFiling d = (from a in context.DocumentsFilings
                                 where a.DocumentTypeId == documentTypeId && a.Tenant == tenant && a.ChildEntityReference == ChildEntityReference
                                 select a).FirstOrDefault();
            if (d == null) return null;
            return d.DocumentId ;
        }

        public IQueryable<DocumentsFilingsView> GetDocumentsFilingsViews(int tenant)
        {
            IDocumentsFilingsViewContext viewContext = DocumentsFilingsViewContext.GetContext(tenant);
            return (from record in viewContext.DocumentsFilingsViews
                    where record.Tenant == tenant
                    select record);
        }


        public List<DocumentsFiling> GetDocumentsFilingsBySecurityIdsAndObjectTable(List<string> securityIds,string objecttableId , int tenant)
        {
            List<DocumentsFiling> externalDocuments = (from a in context.DocumentsFilings
                                                       where (securityIds.Contains(a.SecurityId)  && (a.ObjectTableId == objecttableId || a.ChildObjectTableId == objecttableId) && a.Tenant == tenant)
                                                       select a).ToList();
            return externalDocuments;
        }



    }
}
