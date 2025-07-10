using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentTypeCustomFieldQuery
    {
        DocumentTypeCustomFieldRepository repository;

        public DocumentTypeCustomFieldQuery()
        {
            repository = new DocumentTypeCustomFieldRepository(); 
        }

        public DocumentTypeCustomFieldQuery(int tenant)
        {
            repository = new DocumentTypeCustomFieldRepository(tenant);
        }

        public DocumentTypeCustomFieldQuery(DocumentTypeCustomFieldRepository repository)
        {
            this.repository = repository;
        }

        public DocumentTypeCustomFieldPM GetSinglePM(string id, int tenant)
        {
            var docTypeCustomfield = (from a in repository.context.DocumentTypeCustomFields
                                      where a.Id == id && a.Tenant == tenant
                                      select new DocumentTypeCustomFieldPM()
                                      {
                                          DefaultValue = a.DefaultValue,
                                          DocumentTypeId = a.DocumentTypeId,
                                          FieldCode = a.FieldCode,
                                          FieldDataTypeCode = a.FieldDataTypeCode,
                                          Id = a.Id,
                                          InActive = a.InActive,
                                          IsRequired = a.IsRequired,
                                          Tenant = a.Tenant,
                                          MultiLine = a.MultiLine,
                                          Name = a.Name,
                                          IndexOrder = a.IndexOrder,
                                      }).FirstOrDefault();
            return docTypeCustomfield;
        }

        public IQueryable<DocumentTypeCustomFieldPM> GetDocumentTypeCustomFieldPMsByTenant(int tenant)
        {
            var documentTypeCustom = from a in repository.context.DocumentTypeCustomFields
                                     where a.Tenant == tenant //&& a.InActive == false
                                     select new DocumentTypeCustomFieldPM()
                                     {
                                         DefaultValue = a.DefaultValue,
                                         DocumentTypeId = a.DocumentTypeId,
                                         FieldCode = a.FieldCode,
                                         FieldDataTypeCode = a.FieldDataTypeCode,
                                         Id = a.Id,
                                         InActive = a.InActive,
                                         IsRequired = a.IsRequired,
                                         Tenant = a.Tenant,
                                         MultiLine = a.MultiLine,
                                         Name = a.Name,
                                         IndexOrder = a.IndexOrder,
                                     };
            return documentTypeCustom;
        }

        public IQueryable<DocumentTypeCustomFieldPM> GetDocumentTypeCusotmFieldPMsByDocumentTypeId(string documentTypeId, int tenant)
        {
            var documentTypeCustom = from a in repository.context.DocumentTypeCustomFields.Include("FieldDataType")
                                     where a.Tenant == tenant && a.DocumentTypeId == documentTypeId && a.InActive == false
                                     select new DocumentTypeCustomFieldPM()
                                     {
                                         DefaultValue = a.DefaultValue,
                                         DocumentTypeId = a.DocumentTypeId,
                                         FieldCode = a.FieldCode,
                                         FieldDataTypeCode = a.FieldDataTypeCode,
                                         FieldDataTypeName = a.FieldDataType.Name,
                                         Id = a.Id,
                                         InActive = a.InActive,
                                         IsRequired = a.IsRequired,
                                         Tenant = a.Tenant,
                                         MultiLine = a.MultiLine,
                                         Name = a.Name,
                                         IndexOrder = a.IndexOrder,
                                     };
            return documentTypeCustom;
        }
    }
}
