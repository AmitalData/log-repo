using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Linq;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentsMetaDataTypeQuery
    {
        DocumentsMetaDataTypeRepository repository;
        private DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository;

        public DocumentsMetaDataTypeQuery()
        {
            repository = new DocumentsMetaDataTypeRepository();
        }

        public DocumentsMetaDataTypeQuery(int tenant)
        {
            repository = new DocumentsMetaDataTypeRepository(tenant);
        }

        public DocumentsMetaDataTypeQuery(DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository)
        {
            this.documentsMetaDataTypeRepository = documentsMetaDataTypeRepository;
        }

        public IQueryable<DocumentsMetaDataTypeList> GetIQueryableEntityList(IQueryable<DocumentsMetaDataType> iQueryable)
        {
            IQueryable<DocumentsMetaDataTypeList> result = from entity in iQueryable
                                                           select new DocumentsMetaDataTypeList()
                                                           {
                                                               Id = entity.Id,
                                                               Tenant = entity.Tenant,
                                                               Code = entity.Code,
                                                               EnglishName = entity.EnglishName,
                                                               LocalName = entity.LocalName,
                                                               InActive = entity.InActive,
                                                               Format = entity.Format,
                                                               CustomsMetaDataCode = entity.CustomsMetaDataCode
                                                           };
            return result;
        }

        public DocumentsMetaDataTypePM GetSinglePM(string id, int tenant) =>
            (from a in repository.context.DocumentsMetaDataTypes
             where a.Id == id && a.Tenant == tenant
             select new DocumentsMetaDataTypePM()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 Code = a.Code,
                 EnglishName = a.EnglishName,
                 LocalName = a.LocalName,
                 InActive = a.InActive,
                 Format = a.Format,
                 CustomsMetaDataCode = a.CustomsMetaDataCode
             }).FirstOrDefault();
    }
}