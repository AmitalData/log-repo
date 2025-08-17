using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentTypeRepository:IRepository<DocumentType>
    {
        ICommonDataContext commonDataContext;



        public DocumentTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
            
        }

        public bool IsQuotationDocumentType(string id , int tenant)
        {
            return (from record in context.DocumentTypes where record.Id == id && record.Tenant == tenant && record.Code == "QUOTE" select record).Any();
        }

        public DocumentTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<DocumentType> GetDocumentTypes(int tenant)
        {
            return (from record in context.DocumentTypes.Include("ObjectTable").Include("TemplateFormat").Include("CustomerRole").Include("AgentRole").Include("DocumentTypeCategory") where record.Tenant == tenant select record);
        }


        public IQueryable<DocumentType> GetDocumentTypesByObjectTableId(int tenant, string objecttableid)
        {
            return (from record in context.DocumentTypes.Include("ObjectTable").Include("TemplateFormat").Include("CustomerRole").Include("AgentRole").Include("DocumentTypeCategory") where record.Tenant == tenant && record.ObjectTableId == objecttableid select record);
        }

        public DocumentType GetSingleDocumentTypes(string id, int tenant)
        {
            return (from record in context.DocumentTypes.Include("ObjectTable").Include("TemplateFormat").Include("CustomerRole").Include("AgentRole").Include("DocumentTypeCategory") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        public DocumentType GetSingleDocumentTypeByCodeByObjectTableId(
            string code, string objectTableId  ,int tenant)
        {
            return (from record in 
                        context.DocumentTypes.Include("ObjectTable").Include("TemplateFormat").Include("CustomerRole").Include("AgentRole").Include("DocumentTypeCategory")
                    where record.Code == code && record.Tenant == tenant && record.ObjectTableId == objectTableId
                    select record).FirstOrDefault();
        }

        public DocumentType GetSingleDocumentType(string id, int tenant)
        {
            return (from record in context.DocumentTypes.Include("ObjectTable").Include("TemplateFormat").Include("CustomerRole").Include("AgentRole").Include("DocumentTypeCategory") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }


        public IQueryable<DocumentType> GetDocumentTypeListsByListids(List<string> documentTypeIds, int tenant)
        {
            return (from record in context.DocumentTypes where record.Tenant == tenant && documentTypeIds.Contains(record.Id)  select record);
           
        }


        public DocumentType GetSingleDocumentTypeByCode(string code, int tenant)
        {
            var cacheKey = "DocumentTypeByCode,code," + code + ",tenant," + tenant.ToString();
            var dt=CacheManager.GetOrInsertNewObject(cacheKey, () =>
            {
                return (from record in context.DocumentTypes.Include("ObjectTable").Include("TemplateFormat").Include("CustomerRole").Include("AgentRole").Include("DocumentTypeCategory") where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
            });
            return dt;
        }


        public List<DocumentType> GetDocumentTypesByCodeLists( List<string>documentTypeCodeLists ,int tenant)
        {
            return (from record in context.DocumentTypes where record.Tenant == tenant && documentTypeCodeLists.Contains(record.Code) select record).ToList();
        }
        

        public void Add(DocumentType entity)
        {
            context.DocumentTypes.Add(entity);
        }

        public void Remove(DocumentType entity)
        {
            context.DocumentTypes.Attach(entity);
            context.DocumentTypes.Remove(entity);
        }

        public void Update(DocumentType entity)
        {
            try
            {
                context.DocumentTypes.Attach(entity);
            }
            catch
            { }
            context.SetAsModified(entity);
        }

        public List<DocumentType> All()
        {
            return context.DocumentTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public string GetDocumentTypeIdByCode(string code,int tenant)
        {
            return (from record in context.DocumentTypes where record.Code == code && record.Tenant == tenant select record.Id).FirstOrDefault();
        }
        public string GetDocumentTypeCodeById(string id, int tenant)
        {
            return (from record in context.DocumentTypes where record.Id == id && record.Tenant == tenant select record.Code).FirstOrDefault();
        }
        public string GetDocumentTypeIdByCodeAndObjectTable(string code, string objectTableId ,int tenant)
        {
            return (from record in context.DocumentTypes where record.Code == code && record.Tenant == tenant && record.ObjectTableId == objectTableId select record.Id).FirstOrDefault();
        }

        public DocumentType GetDocumentTypeByCode(string code, int tenant)
        {
            return (from record in context.DocumentTypes where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public DocumentType GetByCodeAndObjectTable(string code, string objectTableId, int tenant)
        {
            return (from record in context.DocumentTypes where record.Code == code && record.Tenant == tenant && record.ObjectTableId == objectTableId select record).FirstOrDefault();
        }


        public string GetObjectTableIdByDocumentCode(string code, int tenant)
        {
            return (from record in context.DocumentTypes where record.Code == code && record.Tenant == tenant select record.ObjectTableId).FirstOrDefault();
        }

    }
}
