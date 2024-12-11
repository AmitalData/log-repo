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
using Logitude.Server.Tools.Helpers;
namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerFieldsUpdateSettingQuery
    {
        CustomerFieldsUpdateSettingRepository repository;
        public CustomerFieldsUpdateSettingQuery()
        {
            repository = new CustomerFieldsUpdateSettingRepository();
        }

        public CustomerFieldsUpdateSettingQuery(int tenant)
        {
            repository = new CustomerFieldsUpdateSettingRepository(tenant);
        }

        public CustomerFieldsUpdateSettingQuery(CustomerFieldsUpdateSettingRepository CustomerFieldsUpdateSettingRepository)
        {
            repository = CustomerFieldsUpdateSettingRepository;
        }

        public CustomerFieldsUpdateSettingPM GetSinglePM(string id, int tenant)
        {
            var query = (from a in repository.context.CustomerFieldsUpdateSettings.Include("ObjectField").Include("ObjectField.FullNameTextCode")
                         where a.Tenant == tenant && a.Id == id
                         select new CustomerFieldsUpdateSettingPM()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             ObjectFieldId = a.ObjectFieldId,
                             ObjectFieldCode = a.ObjectFieldCode,
                             UpdateDirection = a.UpdateDirection,
                             ObjectFieldName = a.ObjectField != null ? a.ObjectField.FullNameTextCode != null ? a.ObjectField.FullNameTextCode.Code : "" : "",
                             SearchFields = a.SearchFields

                         }).FirstOrDefault();

            if (query != null)
            {
                query.ObjectFieldName = !string.IsNullOrEmpty(query.ObjectFieldName) ? TranslateTextsClass.Translate(query.ObjectFieldName, tenant) : "";
            }

            return query;
        }

        public IQueryable<CustomerFieldsUpdateSettingList> GetIQueryableEntityList(IQueryable<CustomerFieldsUpdateSetting> iQueryable)
        {
            IQueryable<CustomerFieldsUpdateSettingList> result = from a in iQueryable.Include("ObjectField")
                                                                 select new CustomerFieldsUpdateSettingList()
                                                                 {
                                                                     Id = a.Id,
                                                                     Tenant = a.Tenant,
                                                                     ObjectFieldId = a.ObjectFieldId,
                                                                     ObjectFieldCode = a.ObjectFieldCode,
                                                                     UpdateDirection = a.UpdateDirection,
                                                                     ObjectFieldName = a.ObjectField != null ? a.ObjectField.FieldName : "",
                                                                     SearchFields = a.SearchFields
                                                                 };

            return result;
        }

        public IQueryable<CustomerFieldsUpdateSettingPM> GetCustomerFieldsUpdateSettingPMsByTenant(int tenant)
        {
            IQueryable<CustomerFieldsUpdateSettingPM> result = from a in repository.context.CustomerFieldsUpdateSettings.Include("ObjectField")
                                                               where a.Tenant == tenant
                                                               select new CustomerFieldsUpdateSettingPM()
                                                               {
                                                                   Id = a.Id,
                                                                   Tenant = a.Tenant,
                                                                   ObjectFieldId = a.ObjectFieldId,
                                                                   ObjectFieldCode = a.ObjectFieldCode,
                                                                   UpdateDirection = a.UpdateDirection,
                                                                   ObjectFieldName = a.ObjectField != null ? a.ObjectField.FieldName : "",
                                                                   SearchFields = a.SearchFields
                                                               };
            return result;
        }

        public IQueryable<CustomerFieldsUpdateSettingList> GetCustomerFieldsUpdateSettinges(int tenant)
        {
            IQueryable<CustomerFieldsUpdateSettingList> result = from a in repository.context.CustomerFieldsUpdateSettings.Include("ObjectField")
                                                                 where a.Tenant == tenant
                                                                 select new CustomerFieldsUpdateSettingList()
                                                                 {
                                                                     Id = a.Id,
                                                                     Tenant = a.Tenant,
                                                                     ObjectFieldId = a.ObjectFieldId,
                                                                     ObjectFieldCode = a.ObjectFieldCode,
                                                                     UpdateDirection = a.UpdateDirection,
                                                                     ObjectFieldName = a.ObjectField != null ? a.ObjectField.FieldName : "",
                                                                     SearchFields = a.SearchFields
                                                                 };
            return result;
        }

        public bool CheckIfExistCustomerFieldsUpdateSetting(string objectFieldCode, int tenant)
        {
            bool result = false;
            var query = (from a in repository.context.CustomerFieldsUpdateSettings
                         where a.Tenant == tenant && a.ObjectFieldCode == objectFieldCode
                         select new CustomerFieldsUpdateSettingPM()
                         {
                             Id = a.Id,

                         }).FirstOrDefault();

            if (query != null) result = true;

            return result;
        }

        public CustomerFieldsUpdateSettingList GetSingleList(string id, int tenant)
        {
            var query = (from a in repository.context.CustomerFieldsUpdateSettings.Include("ObjectField")
                         where a.Tenant == tenant && a.Id == id
                         select new CustomerFieldsUpdateSettingList()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             ObjectFieldId = a.ObjectFieldId,
                             ObjectFieldCode = a.ObjectFieldCode,
                             UpdateDirection = a.UpdateDirection,
                             ObjectFieldName = a.ObjectField != null ? a.ObjectField.FieldName : "",
                             SearchFields = a.SearchFields
                         }).FirstOrDefault();

            return query;
        }
    }
}
