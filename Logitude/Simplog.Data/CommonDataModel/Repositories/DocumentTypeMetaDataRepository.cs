using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentTypeMetaDataRepository:IRepository<DocumentTypeMetaData>
    {
        ICommonDataContext commonDataContext;



        public DocumentTypeMetaDataRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public DocumentTypeMetaDataRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<DocumentTypeMetaData> GetDocumentTypeMetaDatas(int tenant)
        {
            return (from record in context.DocumentTypeMetaDatas.Include("DocumentsMetaDataType")
                    where record.Tenant == tenant select record);
        }

        public DocumentTypeMetaData GetSingleDocumentTypeMetaData(string id, int tenant)
        {
            DocumentTypeMetaData d = (from a in context.DocumentTypeMetaDatas.Include("DocumentsMetaDataType")
                                  where a.Id == id && a.Tenant == tenant
                                  select a).FirstOrDefault();
            return d;
        }


        public void Add(DocumentTypeMetaData entity)
        {
            context.DocumentTypeMetaDatas.Add(entity);
        }

        public void Remove(DocumentTypeMetaData entity)
        {
            context.DocumentTypeMetaDatas.Attach(entity);
            context.DocumentTypeMetaDatas.Remove(entity);
        }

        public void Update(DocumentTypeMetaData entity)
        {
            try
            {
                context.DocumentTypeMetaDatas.Attach(entity);
            }
            catch
            { 
            }
            context.SetAsModified(entity);
            

        }

        public List<DocumentTypeMetaData> All()
        {
            return context.DocumentTypeMetaDatas.ToList();
            
        }

        public ICommonDataContext context
        {
          
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentTypeMetaData> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentTypeMetaData GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
