using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentsFilingMetaDataValueRepository:IRepository<DocumentsFilingMetaDataValue>
    {
        ICommonDataContext commonDataContext;

        public DocumentsFilingMetaDataValueRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentsFilingMetaDataValueRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public DocumentsFilingMetaDataValueRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<DocumentsFilingMetaDataValue> GetDocumentsFilingMetaDataValues(int tenant)
        {
            return (from record in context.DocumentsFilingMetaDataValues
                    where record.Tenant == tenant select record);
        }

        public DocumentsFilingMetaDataValue GetSingleDocumentsFilingMetaDataValue(string id, int tenant)
        {
            DocumentsFilingMetaDataValue d = (from a in context.DocumentsFilingMetaDataValues
                                  where a.Id == id && a.Tenant == tenant
                                  select a).FirstOrDefault();
            return d;
        }


        public void Add(DocumentsFilingMetaDataValue entity)
        {
            context.DocumentsFilingMetaDataValues.Add(entity);
        }

        public void Remove(DocumentsFilingMetaDataValue entity)
        {
            context.DocumentsFilingMetaDataValues.Attach(entity);
            context.DocumentsFilingMetaDataValues.Remove(entity);
        }

        public void Update(DocumentsFilingMetaDataValue entity)
        {
            try
            {
                context.DocumentsFilingMetaDataValues.Attach(entity);
            }
            catch
            { 
            }
            context.SetAsModified(entity);
            

        }

        public List<DocumentsFilingMetaDataValue> All()
        {
            return context.DocumentsFilingMetaDataValues.ToList();
            
        }

        public ICommonDataContext context
        {
          
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentsFilingMetaDataValue> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentsFilingMetaDataValue GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<DocumentsFilingMetaDataValue> GetDocumentsFilingMetaDataValuesByTenantDocFilingId(int tenant,string DocumentFilingId)
        {
            return (from record in context.DocumentsFilingMetaDataValues
                    where record.Tenant == tenant && record.DocumentsFilingId == DocumentFilingId
                    select record);
        }
    }
}
