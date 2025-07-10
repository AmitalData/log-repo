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

        public DocumentsMetaDataTypeQuery() : this(new DocumentsMetaDataTypeRepository()) { }

        public DocumentsMetaDataTypeQuery(int tenant) : this(new DocumentsMetaDataTypeRepository(tenant)) { }

        public DocumentsMetaDataTypeQuery(DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository)
        {
            repository = documentsMetaDataTypeRepository ?? throw new ArgumentNullException(nameof(documentsMetaDataTypeRepository));
        }

        public IQueryable<DocumentsMetaDataTypeList> GetIQueryableEntityList(IQueryable<DocumentsMetaDataType> iQueryable)
        {
            return iQueryable.Select(entity => new DocumentsMetaDataTypeList
            {
                Id = entity.Id,
                Tenant = entity.Tenant,
                Code = entity.Code,
                EnglishName = entity.EnglishName,
                LocalName = entity.LocalName,
                InActive = entity.InActive,
                Format = entity.Format,
                CustomsMetaDataCode = entity.CustomsMetaDataCode
            });
        }

        public DocumentsMetaDataTypePM GetSinglePM(string id, int tenant)
        {
            return repository.context.DocumentsMetaDataTypes
                .Where(a => a.Id == id && a.Tenant == tenant)
                .Select(a => new DocumentsMetaDataTypePM
                {
                    Id = a.Id,
                    Tenant = a.Tenant,
                    Code = a.Code,
                    EnglishName = a.EnglishName,
                    LocalName = a.LocalName,
                    InActive = a.InActive,
                    Format = a.Format,
                    CustomsMetaDataCode = a.CustomsMetaDataCode
                })
                .FirstOrDefault();
        }
    }
}