using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentOutRepository:IRepository<DocumentOut>
    {
        ICommonDataContext commonDataContext;

        public DocumentOutRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentOutRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentOutRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<DocumentOut> GetDocumentOuts(int tenant)
        {
            return (context.DocumentOuts.Include("DocumentsFiling").Include("DocumentsFiling.DocumentType").Include("DocumentsFiling.CreatedByUser.Contact").Where(record => record.Tenant == tenant));
        }

        public DocumentOut GetSingleDocumentOut(string id, int tenant)
        {
            DocumentOut d = (from a in context.DocumentOuts.Include("DocumentsFiling").Include("DocumentsFiling.DocumentType").Include("DocumentsFiling.CreatedByUser.Contact")

                                  where a.Id == id && a.Tenant == tenant
                                  select a).FirstOrDefault();
            return d;
        }

        public DocumentOut GetDocumentOutByEntityAndChildEntity(string entityId, string childEntityId)
        {
            DocumentOut documentOut = (from a in context.DocumentOuts.Include("DocumentsFiling").Include("DocumentsFiling.DocumentType").Include("DocumentsFiling.CreatedByUser.Contact")
                                       where a.DocumentsFiling.EntityId == entityId && a.DocumentsFiling.ChildEntityId == childEntityId
                                       select a).FirstOrDefault();
            return documentOut;
        }

        public DocumentOut GetDocumentOutByDocumentTypeAndEntityAndChildEntity(string entityId, string childEntityId, string documentTypeId, int tenant)
        {
            DocumentOut documentOut = (from a in context.DocumentOuts.Include("DocumentsFiling").Include("DocumentsFiling.DocumentType").Include("DocumentsFiling.CreatedByUser.Contact")
                                       where a.DocumentsFiling.EntityId == entityId && a.DocumentsFiling.ChildEntityId == childEntityId && a.DocumentsFiling.DocumentTypeId == documentTypeId && a.Tenant == tenant
                                       select a).FirstOrDefault();
            return documentOut;
        }

        public DocumentOut GetDocumentOutByDocumentTypeAndEntity(string entityId, string documentTypeId, int tenant)
        {
            DocumentOut documentOut = (from a in context.DocumentOuts.Include("DocumentsFiling").Include("DocumentsFiling.DocumentType").Include("DocumentsFiling.CreatedByUser.Contact")
                                       where a.DocumentsFiling.EntityId == entityId && a.DocumentsFiling.DocumentTypeId == documentTypeId && a.Tenant == tenant
                                       select a).FirstOrDefault();
            return documentOut;
        }



        public string GetDocumentOutIdByDocumentTypeIdAndEntityId(string entityId, string documentTypeId,string objectTableId, int tenant)
        {
            string documentOutId = (from a in context.DocumentOuts.Include("DocumentsFiling")
                                       where a.DocumentsFiling.EntityId == entityId && a.DocumentsFiling.DocumentTypeId == documentTypeId && a.DocumentsFiling.ObjectTableId == objectTableId && a.DocumentsFiling.DirectionCode == "O" && a.Tenant == tenant
                                       select a.Id).FirstOrDefault();
            return documentOutId;
        }



        public List<DocumentOut> GetDocumentOutsByShipmentId(string shipmentId)
        {
            List<DocumentOut> documentOuts;
            documentOuts = (from a in context.DocumentOuts.Include("DocumentsFiling").Include("DocumentsFiling.DocumentType").Include("DocumentsFiling.CreatedByUser.Contact")
                            where a.DocumentsFiling.EntityId == shipmentId
                select a).ToList();
            return documentOuts;
        }

        public void Add(DocumentOut entity)
        {
            context.DocumentOuts.Add(entity);
        }

        public void Remove(DocumentOut entity)
        {
            context.DocumentOuts.Attach(entity);
            context.DocumentOuts.Remove(entity);
        }

        public void Update(DocumentOut entity)
        {
            try
            {
                context.DocumentOuts.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<DocumentOut> All()
        {
            return context.DocumentOuts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();  
        }


        public List<DocumentOut> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentOut GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
