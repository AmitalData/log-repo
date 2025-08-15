using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentTypeMetaDataQuery
    {
        DocumentTypeMetaDataRepository repository;



        public DocumentTypeMetaDataQuery(int tenant)
        {
            repository = new DocumentTypeMetaDataRepository(tenant);
        }

        public DocumentTypeMetaDataQuery(DocumentTypeMetaDataRepository DocumentTypeMetaDataRepository)
        {
            repository = DocumentTypeMetaDataRepository;
        }

        public DocumentTypeMetaDataPM GetSinglePM(string id, int tenant)
        {
            DocumentTypeMetaDataPM DocMetaData = (from a in repository.context.DocumentTypeMetaDatas.Include("DocumentsMetaDataType")
                                                  where a.Id == id && a.Tenant == tenant
                                                  select new DocumentTypeMetaDataPM()
                                            {
                                                Id = a.Id,
                                                DocumentTypeId = a.DocumentTypeId,
                                                DocumentsMetaDataTypeId = a.DocumentsMetaDataTypeId,
                                                Tenant = a.Tenant,
                                                DocumentsMetaDataTypeCode = a.DocumentsMetaDataType.Code,
                                                DocumentsMetaDataTypeEnglishName = a.DocumentsMetaDataType.EnglishName,
                                                DocumentsMetaDataTypeLocalName = a.DocumentsMetaDataType.LocalName,
                                                DocumentsMetaDataTypeFormat = a.DocumentsMetaDataType.Format,
                                                Mandatory = a.Mandatory,



                                            }).FirstOrDefault();

            return DocMetaData;
        }

        public IQueryable<DocumentTypeMetaDataPM> GetDocumentTypeMetaDataPMsByDocumentIdTenant(string documentTypeId, int tenant)
        {
            IQueryable<DocumentTypeMetaDataPM> documents = from a in repository.context.DocumentTypeMetaDatas.Include("DocumentsMetaDataType")
                                                           where a.Tenant == tenant && a.DocumentTypeId == documentTypeId
                                                           select new DocumentTypeMetaDataPM()
                                     {

                                         Id = a.Id,
                                         DocumentTypeId = a.DocumentTypeId,
                                         DocumentsMetaDataTypeId = a.DocumentsMetaDataTypeId,
                                         Tenant = a.Tenant,
                                         DocumentsMetaDataTypeCode = a.DocumentsMetaDataType.Code,
                                         DocumentsMetaDataTypeEnglishName = a.DocumentsMetaDataType.EnglishName,
                                         DocumentsMetaDataTypeLocalName = a.DocumentsMetaDataType.LocalName,
                                         DocumentsMetaDataTypeFormat = a.DocumentsMetaDataType.Format,
                                         Mandatory=a.Mandatory,
                                     };
            return documents;
        }

        public IQueryable<DocumentTypeMetaDataPM> GetDocumentTypeMetaDataPMsByTenant(int tenant)
        {
            IQueryable<DocumentTypeMetaDataPM> documents = from a in repository.context.DocumentTypeMetaDatas.Include("DocumentsMetaDataType")
                                                           where a.Tenant == tenant
                                                           select new DocumentTypeMetaDataPM()
                                                           {
                                                               Id = a.Id,
                                                               DocumentTypeId = a.DocumentTypeId,
                                                               DocumentsMetaDataTypeId = a.DocumentsMetaDataTypeId,
                                                               Tenant = a.Tenant,
                                                               DocumentsMetaDataTypeCode = a.DocumentsMetaDataType.Code,
                                                               DocumentsMetaDataTypeEnglishName = a.DocumentsMetaDataType.EnglishName,
                                                               DocumentsMetaDataTypeLocalName = a.DocumentsMetaDataType.LocalName,
                                                               DocumentsMetaDataTypeFormat = a.DocumentsMetaDataType.Format,
                                                               Mandatory = a.Mandatory,
                                                           };
            return documents;
        }

        public IQueryable<DocumentTypeMetaDataList> GetIQueryableEntityList(IQueryable<DocumentTypeMetaData> iQueryable)
        {
            IQueryable<DocumentTypeMetaDataList> result = from a in iQueryable.Include("DocumentsMetaDataType")
                                                          select new DocumentTypeMetaDataList()
                                                       {
                                                           Id = a.Id,
                                                           DocumentTypeId = a.DocumentTypeId,
                                                           DocumentsMetaDataTypeId = a.DocumentsMetaDataTypeId,
                                                           Tenant = a.Tenant,
                                                           DocumentsMetaDataTypeCode = a.DocumentsMetaDataType.Code,
                                                           DocumentsMetaDataTypeEnglishName = a.DocumentsMetaDataType.EnglishName,
                                                           DocumentsMetaDataTypeLocalName = a.DocumentsMetaDataType.LocalName,
                                                           DocumentsMetaDataTypeFormat = a.DocumentsMetaDataType.Format,
                                                           Mandatory = a.Mandatory,

                                                       };
            return result;

        }


        public DocumentTypeMetaDataList GetSingle(string metadatatypeid, string documenttypeid)
        {
            IQueryable<DocumentTypeMetaData> DocumentTypeMetaDataQuery = (from a in repository.context.DocumentTypeMetaDatas
                                                                          where a.DocumentsMetaDataTypeId == metadatatypeid && a.DocumentTypeId == documenttypeid
                                                                          select a);


            IQueryable<DocumentTypeMetaDataList> DocumentTypeMetaDataListQuery = GetIqueryableList(DocumentTypeMetaDataQuery);
            DocumentTypeMetaDataList DocumentTypeMetaDataList = DocumentTypeMetaDataListQuery.FirstOrDefault();
            return DocumentTypeMetaDataList;


        }

        private IQueryable<DocumentTypeMetaDataList> GetIqueryableList(IQueryable<DocumentTypeMetaData> iQueryable)
        {
            IQueryable<DocumentTypeMetaDataList> query = (from a in iQueryable.Include("DocumentsMetaDataType")
                                                                select new DocumentTypeMetaDataList()
                                                                {
                                                                    Id = a.Id,
                                                                    DocumentTypeId = a.DocumentTypeId,
                                                                    DocumentsMetaDataTypeId = a.DocumentsMetaDataTypeId,
                                                                    Tenant = a.Tenant,
                                                                    DocumentsMetaDataTypeCode = a.DocumentsMetaDataType.Code,
                                                                    DocumentsMetaDataTypeEnglishName = a.DocumentsMetaDataType.EnglishName,
                                                                    DocumentsMetaDataTypeLocalName = a.DocumentsMetaDataType.LocalName,
                                                                    DocumentsMetaDataTypeFormat = a.DocumentsMetaDataType.Format,
                                                                    Mandatory = a.Mandatory,
                                                                   
                                                                });
            return query;
        }
        

    }


}
