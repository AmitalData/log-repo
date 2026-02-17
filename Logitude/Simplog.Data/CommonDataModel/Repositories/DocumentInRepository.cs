//using System.Collections.Generic;
//using System.Linq;

//using Simplog.Data.CommonDataModel.EntityPOCOs;
//using Simplog.Server.Infrastructure.Helpers;
//using Simplog.Server.Infrastructure;

//namespace Simplog.Data.CommonDataModel.Repositories
//{
//    public class DocumentInRepository:IRepository<DocumentIn>
//    {
//        ICommonDataContext commonDataContext;

//        public DocumentInRepository()
//        {
//            commonDataContext = new CommonDataContext();
//        }

//        public DocumentInRepository(int tenant)
//        {
//            commonDataContext = CommonDataContext.GetContext(tenant);
//        }

//        public DocumentInRepository(ICommonDataContext context)
//        {
//            commonDataContext = context;
//        }

//        public IQueryable<DocumentIn> GetDocumentIns(int tenant)
//        {
//            return (from record in context.DocumentIns.Include("ReceivedByUser.Contact").Include("Document") where record.Tenant == tenant select record);
//        }

//        public DocumentIn GetSingleDocumentIn(string id, int tenant)
//        {
//            DocumentIn d = (from a in context.DocumentIns.Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType")
//                                  where a.Id == id && a.Tenant == tenant
//                                  select a).FirstOrDefault();
//            return d;
//        }

//        public List<DocumentIn> GetDocumentInsByEntityId(string entityId, int tenant)
//        {
//            List<DocumentIn> externalDocuments = (from a in context.DocumentIns.Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType")
//                                                  where (a.EntityId == entityId || a.ChildEntityId == entityId)  && a.Tenant == tenant
//                                                  select a).ToList();
//            return externalDocuments;
//        }

//        public List<DocumentIn> GetDocumentInsForEntityTableId(string entityId, string objecttableId, int tenant)
//        {
//            List<DocumentIn> externalDocuments = (from a in context.DocumentIns.Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType")
//                                                  where (a.EntityId == entityId || a.ChildEntityId == entityId) && a.ObjectTableId == objecttableId && a.Tenant == tenant
//                                                  select a).ToList();
//            return externalDocuments;
//        }

//        public DocumentIn GetSingleDocumentInByExternalId(string externalId, int tenant)
//        {
//            var externalDocument = (from a in context.DocumentIns.Include("ReceivedByUser.Contact").Include("Document").Include("ReceivedByUser.Contact").Include("ObjectTable").Include("DocumentType")
//                                                  where a.ExternalId == externalId && a.Tenant == tenant
//                                                  select a).FirstOrDefault();
//           return externalDocument;
//        }

//        public void Add(DocumentIn entity)
//        {
//            context.DocumentIns.Add(entity);
//        }

//        public void Remove(DocumentIn entity)
//        {
//            context.DocumentIns.Attach(entity);
//            context.DocumentIns.Remove(entity);
//        }

//        public void Update(DocumentIn entity)
//        {
//            try
//            {
//                context.DocumentIns.Attach(entity);
//            }
//            catch
//            { 
//            }
//            context.SetAsModified(entity);
            

//        }

//        public List<DocumentIn> All()
//        {
//            return context.DocumentIns.ToList();
            
//        }

//        public ICommonDataContext context
//        {
          
//            get { return commonDataContext; }
//        }

//        public void SubmitChanges()
//        {
//            context.SaveChanges();
//        }


//        public List<DocumentIn> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
//        {
//            throw new System.NotImplementedException();
//        }

//        public DocumentIn GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}
