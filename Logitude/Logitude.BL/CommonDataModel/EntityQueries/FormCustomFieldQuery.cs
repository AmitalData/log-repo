using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class FormCustomFieldQuery
    {
        FormCustomFieldRepository repository;

        public FormCustomFieldQuery()
        {
            repository = new FormCustomFieldRepository(); 
        }

        public FormCustomFieldQuery(int tenant)
        {
            repository = new FormCustomFieldRepository(tenant);
        }

        public FormCustomFieldQuery(FormCustomFieldRepository repository)
        {
            this.repository = repository;
        }

        public FormCustomFieldPM GetSinglePM(string id, int tenant)
        {
            var formCustomfield = (from a in repository.context.FormCustomFields
                                   where a.Id == id && a.Tenant == tenant
                                   select new FormCustomFieldPM()
                                   {
                                       Value = a.Value,
                                       DocumentTypeId = a.DocumentTypeId,
                                       FieldCode = a.FieldCode,
                                       EntityId = a.EntityId,
                                       Id = a.Id,
                                       ObjectTableId = a.ObjectTableId,
                                       Tenant = a.Tenant,
                                   }).FirstOrDefault();
            return formCustomfield;
        }

        public FormCustomFieldPM GetSingleFormCustomFieldsPMByDocument(int tenant, string documentTypeId, string fieldCode, string entityId, string objectTableId)
        {
            var formCustom = (from a in repository.context.FormCustomFields
                              where a.Tenant == tenant && a.DocumentTypeId == documentTypeId && a.FieldCode == fieldCode && a.EntityId == entityId && a.ObjectTableId == objectTableId
                              select new FormCustomFieldPM()
                              {
                                  Value = a.Value,
                                  DocumentTypeId = a.DocumentTypeId,
                                  FieldCode = a.FieldCode,
                                  ObjectTableId = a.ObjectTableId,
                                  EntityId = a.EntityId,
                                  Id = a.Id,
                                  Tenant = a.Tenant,
                              }).FirstOrDefault();
            return formCustom;
        }

        public IQueryable<FormCustomFieldPM> GetFormCustomFieldPMsByTenant(int tenant)
        {
            var formCustom = from a in repository.context.FormCustomFields
                             where a.Tenant == tenant
                             select new FormCustomFieldPM()
                             {
                                 Value = a.Value,
                                 DocumentTypeId = a.DocumentTypeId,
                                 FieldCode = a.FieldCode,
                                 ObjectTableId = a.ObjectTableId,
                                 EntityId = a.EntityId,
                                 Id = a.Id,
                                 Tenant = a.Tenant,
                             };
            return formCustom;
        }

        public IQueryable<FormCustomFieldPM> GetFormCusotmFieldPMsByDocumentTypeId(string documentTypeId, int tenant)
        {
            var formCustom = from a in repository.context.FormCustomFields
                             where a.Tenant == tenant && a.DocumentTypeId == documentTypeId
                             select new FormCustomFieldPM()
                             {
                                 EntityId = a.EntityId,
                                 DocumentTypeId = a.DocumentTypeId,
                                 FieldCode = a.FieldCode,
                                 ObjectTableId = a.ObjectTableId,
                                 Id = a.Id,
                                 Value = a.Value,
                                 Tenant = a.Tenant,
                             };
            return formCustom;
        }
    }
}
