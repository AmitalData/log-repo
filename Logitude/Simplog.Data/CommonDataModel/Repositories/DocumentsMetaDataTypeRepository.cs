using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentsMetaDataTypeRepository:IRepository<DocumentsMetaDataType>
    {
        ICommonDataContext commonDataContext;



        public DocumentsMetaDataTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public DocumentsMetaDataTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<DocumentsMetaDataType> GetDocumentsMetaDataTypes(int tenant)
        {
            return (from record in context.DocumentsMetaDataTypes
                    where record.Tenant == tenant select record);
        }

        public DocumentsMetaDataType GetSingleDocumentsMetaDataType(string id, int tenant)
        {
            DocumentsMetaDataType d = (from a in context.DocumentsMetaDataTypes
                                  where a.Id == id && a.Tenant == tenant
                                  select a).FirstOrDefault();
            return d;
        }

        public DocumentsMetaDataType GetSingleDocumentsMetaDataTypeByCode(string code, int tenant, bool fromCache = false)
        {

            if (fromCache)
            {

                string entityKeyString = $"GetSingleDocumentsMetaDataTypeByCode({code},{tenant})";
                DocumentsMetaDataType myres = CacheManager.GetOrInsertNewObject<DocumentsMetaDataType>(entityKeyString, () =>
                {


                    return (from a in context.DocumentsMetaDataTypes
                            where a.Code == code && a.Tenant == tenant
                            select a).FirstOrDefault();

                }
                );
                return myres;
            }

            DocumentsMetaDataType d = (from a in context.DocumentsMetaDataTypes
                                       where a.Code == code && a.Tenant == tenant
                                       select a).FirstOrDefault();
            return d;
        }

        public DocumentsMetaDataType GetSingleDocumentsMetaDataTypeByCustomsMetaDataCode(string code, int tenant)
        {

            string entityKeyString = $"GetSingleDocumentsMetaDataTypeByCustomsMetaDataCode({code},{tenant})";
            var res = CacheManager.GetOrInsertNewObject<DocumentsMetaDataType>(entityKeyString, () =>
            {
                DocumentsMetaDataType d = (from a in context.DocumentsMetaDataTypes
                                           where a.CustomsMetaDataCode == code && a.Tenant == tenant
                                           select a).FirstOrDefault();
                return d;
            });
            return res;
           
        }

        public void Add(DocumentsMetaDataType entity)
        {
            context.DocumentsMetaDataTypes.Add(entity);
        }

        public void Remove(DocumentsMetaDataType entity)
        {
            context.DocumentsMetaDataTypes.Attach(entity);
            context.DocumentsMetaDataTypes.Remove(entity);
        }

        public void Update(DocumentsMetaDataType entity)
        {
            try
            {
                context.DocumentsMetaDataTypes.Attach(entity);
            }
            catch
            { 
            }
            context.SetAsModified(entity);
            

        }

        public List<DocumentsMetaDataType> All()
        {
            return context.DocumentsMetaDataTypes.ToList();
            
        }

        public ICommonDataContext context
        {
          
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentsMetaDataType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentsMetaDataType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
