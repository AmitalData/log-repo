using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class AdvancedQueryFilterQuery
    {
        AdvancedQueryFilterRepository repository;
        public AdvancedQueryFilterQuery()
        {
            repository = new AdvancedQueryFilterRepository(); 
        }

        public AdvancedQueryFilterQuery(int tenant)
        {
            repository = new AdvancedQueryFilterRepository(tenant);
        }

        public AdvancedQueryFilterQuery(AdvancedQueryFilterRepository advancedQueryFilterRepository)
        {
            repository = advancedQueryFilterRepository;
        }

        public IQueryable<AdvancedQueryFilterPM> GetAdvancedQueryFilterPMsByTenant(int tenant)
        {
            IQueryable<AdvancedQueryFilterPM> advancedFilters = from a in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                                                                where a.Tenant == tenant
                                                                select new AdvancedQueryFilterPM()
                                                                {
                                                                    DisplayInList = a.ObjectField.DisplayInList,
                                                                    Id = a.Id,
                                                                    IndexOrder = a.IndexOrder,
                                                                    IsCustomFilter = a.ObjectField.IsCustomFilter,
                                                                    IsPredefined = a.IsPredefined,
                                                                    ObjectFieldId = a.ObjectFieldId,
                                                                    ObjectFieldName = a.ObjectField.FieldName,
                                                                    Operator = a.Operator,
                                                                    PredefinedValue = a.PredefinedValue,
                                                                    PredefinedValue2 = a.PredefinedValue2,
                                                                    CustomPredefined = a.CustomPredefined,
                                                                    QueryCode = a.QueryCode,
                                                                    QueryId = a.QueryId,
                                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                                    QueryUserId = a.Query.UserId,
                                                                    Tenant = a.Tenant,
                                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                                    ObjectFieldOperator = a.ObjectField.Operator,
                                                                    UserId = a.UserId,
                                                                    ObjectFieldCode = a.ObjectFieldCode,
                                                                };

 
            return advancedFilters;

        }

        public IQueryable<AdvancedQueryFilterPM> GetAdvancedQueryFilterPMsByTenantAndUser(int tenant, string userid)
        {
            IQueryable<AdvancedQueryFilterPM> advancedFilters = from a in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                                                                where (a.Tenant == tenant && a.UserId == userid) || a.Tenant == 0
                                                                select new AdvancedQueryFilterPM()
                                                                {
                                                                    DisplayInList = a.ObjectField.DisplayInList,
                                                                    Id = a.Id,
                                                                    IndexOrder = a.IndexOrder,
                                                                    IsCustomFilter = a.ObjectField.IsCustomFilter,
                                                                    IsPredefined = a.IsPredefined,
                                                                    ObjectFieldId = a.ObjectFieldId,
                                                                    ObjectFieldName = a.ObjectField.FieldName,
                                                                    Operator = a.Operator,
                                                                    PredefinedValue = a.PredefinedValue,
                                                                    PredefinedValue2 = a.PredefinedValue2,
                                                                    QueryCode = a.QueryCode,
                                                                    QueryId = a.QueryId,
                                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                                    QueryUserId = a.Query.UserId,
                                                                    Tenant = a.Tenant,
                                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                                    ObjectFieldOperator = a.ObjectField.Operator,
                                                                    UserId = a.UserId,
                                                                    ObjectFieldCode = a.ObjectFieldCode,
                                                                };

 
            return advancedFilters;

        }

        public IQueryable<AdvancedQueryFilterPM> GetAdvancedQueryFilterPMsByTenantAndUserAndQuery(int tenant, string userId, string queryCode)
        {
            IQueryable<AdvancedQueryFilterPM> advancedFilters = null;
            Query myQuery = repository.context.Queries.Where(d => d.UniqueCode == queryCode).FirstOrDefault();

            if (myQuery != null)
            {
                if (!string.IsNullOrEmpty(myQuery.SharedByUserId) && myQuery.SharedByUserId != userId)
                {
                    advancedFilters = from a in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                                      where (a.Tenant == tenant && a.Query.UserId == myQuery.SharedByUserId && a.QueryCode == queryCode) || a.Tenant == 0
                                      select new AdvancedQueryFilterPM()
                                      {
                                          DisplayInList = a.ObjectField.DisplayInList,
                                          Id = a.Id,
                                          IndexOrder = a.IndexOrder,
                                          IsCustomFilter = a.ObjectField.IsCustomFilter,
                                          IsPredefined = a.IsPredefined,
                                          ObjectFieldId = a.ObjectFieldId,
                                          ObjectFieldName = a.ObjectField.FieldName,
                                          Operator = a.Operator,
                                          PredefinedValue = a.PredefinedValue,
                                          PredefinedValue2 = a.PredefinedValue2,
                                          QueryCode = a.QueryCode,
                                          QueryId = a.QueryId,
                                          QueryObjectTableName = a.Query.ObjectTable.Name,
                                          QueryUserId = a.Query.UserId,
                                          Tenant = a.Tenant,
                                          DataTypeCode = a.ObjectField.DataTypeCode,
                                          ObjectFieldOperator = a.ObjectField.Operator,
                                          UserId = a.UserId,
                                          ObjectFieldCode = a.ObjectFieldCode,
                                          CustomPredefined = a.CustomPredefined
                                      };

 
                }

                else
                {
                    advancedFilters = from a in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                                      where (a.Tenant == tenant && (a.Query.UserId == userId && a.UserId != null) && a.QueryCode == queryCode) || (a.Tenant == tenant && a.QueryCode == queryCode && a.UserId == userId) || (a.Tenant == 0 && a.QueryCode == queryCode && a.UserId == null)
                                      select new AdvancedQueryFilterPM()
                                      {
                                          DisplayInList = a.ObjectField.DisplayInList,
                                          Id = a.Id,
                                          IndexOrder = a.IndexOrder,
                                          IsCustomFilter = a.ObjectField.IsCustomFilter,
                                          IsPredefined = a.IsPredefined,
                                          ObjectFieldId = a.ObjectFieldId,
                                          ObjectFieldName = a.ObjectField.FieldName,
                                          Operator = a.Operator,
                                          PredefinedValue = a.PredefinedValue,
                                          PredefinedValue2 = a.PredefinedValue2,
                                          QueryCode = a.QueryCode,
                                          QueryId = a.QueryId,
                                          QueryObjectTableName = a.Query.ObjectTable.Name,
                                          QueryUserId = a.Query.UserId,
                                          Tenant = a.Tenant,
                                          DataTypeCode = a.ObjectField.DataTypeCode,
                                          ObjectFieldOperator = a.ObjectField.Operator,
                                          UserId = a.UserId,
                                          ObjectFieldCode = a.ObjectFieldCode,
                                      };

                 }
            }

            return advancedFilters;
        }

        public IQueryable<AdvancedQueryFilterPM> GetAdvancedQueryFiltersByTenantAndNoUser(int tenant)
        {
            IQueryable<AdvancedQueryFilterPM> advancedFilters = from a in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                                                                where a.Tenant == tenant && a.Query.UserId == null
                                                                select new AdvancedQueryFilterPM()
                                                                {
                                                                    DisplayInList = a.ObjectField.DisplayInList,
                                                                    Id = a.Id,
                                                                    IndexOrder = a.IndexOrder,
                                                                    IsCustomFilter = a.ObjectField.IsCustomFilter,
                                                                    IsPredefined = a.IsPredefined,
                                                                    ObjectFieldId = a.ObjectFieldId,
                                                                    ObjectFieldName = a.ObjectField.FieldName,
                                                                    Operator = a.Operator,
                                                                    PredefinedValue = a.PredefinedValue,
                                                                    PredefinedValue2 = a.PredefinedValue2,
                                                                    QueryCode = a.QueryCode,
                                                                    QueryId = a.QueryId,
                                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                                    QueryUserId = a.Query.UserId,
                                                                    Tenant = a.Tenant,
                                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                                    ObjectFieldOperator = a.ObjectField.Operator,
                                                                    UserId = a.UserId,
                                                                    ObjectFieldCode = a.ObjectFieldCode,
                                                                };

 
            return advancedFilters;

        }

        public IQueryable<AdvancedQueryFilterPM> GetAdvancedQueryFiltersByQueryCode(int tenant, string queryCode)
        {
            IQueryable<AdvancedQueryFilterPM> advancedFilters = from a in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                                                                where a.Tenant == tenant && a.QueryCode == queryCode
                                                                select new AdvancedQueryFilterPM()
                                                                {
                                                                    DisplayInList = a.ObjectField.DisplayInList,
                                                                    Id = a.Id,
                                                                    IndexOrder = a.IndexOrder,
                                                                    IsCustomFilter = a.ObjectField.IsCustomFilter,
                                                                    IsPredefined = a.IsPredefined,
                                                                    ObjectFieldId = a.ObjectFieldId,
                                                                    ObjectFieldName = a.ObjectField.FieldName,
                                                                    Operator = a.Operator,
                                                                    PredefinedValue = a.PredefinedValue,
                                                                    PredefinedValue2 = a.PredefinedValue2,
                                                                    QueryCode = a.QueryCode,
                                                                    QueryId = a.QueryId,
                                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                                    QueryUserId = a.Query.UserId,
                                                                    Tenant = a.Tenant,
                                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                                    ObjectFieldOperator = a.ObjectField.Operator,
                                                                    UserId = a.UserId,
                                                                    ObjectFieldCode = a.ObjectFieldCode,
                                                                };

 
            return advancedFilters;

        }
        public IQueryable<AdvancedQueryFilterPM> GetPredefinedQueryFilters(int tenant)
        {
            IQueryable<AdvancedQueryFilterPM> advancedFilters = from a in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                                                                where (a.Tenant == tenant || a.Tenant == 0) && a.IsPredefined == true
                                                                select new AdvancedQueryFilterPM()
                                                                {
                                                                    DisplayInList = a.ObjectField.DisplayInList,
                                                                    Id = a.Id,
                                                                    IndexOrder = a.IndexOrder,
                                                                    IsCustomFilter = a.ObjectField.IsCustomFilter,
                                                                    IsPredefined = a.IsPredefined,
                                                                    ObjectFieldId = a.ObjectFieldId,
                                                                    ObjectFieldName = a.ObjectField.FieldName,
                                                                    Operator = a.Operator,
                                                                    PredefinedValue = a.PredefinedValue,
                                                                    PredefinedValue2 = a.PredefinedValue2,
                                                                    QueryCode = a.QueryCode,
                                                                    QueryId = a.QueryId,
                                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                                    QueryUserId = a.Query.UserId,
                                                                    Tenant = a.Tenant,
                                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                                    ObjectFieldOperator = a.ObjectField.Operator,
                                                                    UserId = a.UserId,
                                                                    ObjectFieldCode = a.ObjectFieldCode,
                                                                };

 
            return advancedFilters;

        }

        public AdvancedQueryFilterPM GetPredefinedQueryFilterByFieldIdTenant(string fieldCode,int tenant)
        {
            AdvancedQueryFilterPM advancedFilters = (from a in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                                                    where a.Tenant == tenant && a.ObjectFieldCode == fieldCode
                                                     select new AdvancedQueryFilterPM()
                                                                {
                                                                    DisplayInList = a.ObjectField.DisplayInList,
                                                                    Id = a.Id,
                                                                    IndexOrder = a.IndexOrder,
                                                                    IsCustomFilter = a.ObjectField.IsCustomFilter,
                                                                    IsPredefined = a.IsPredefined,
                                                                    ObjectFieldId = a.ObjectFieldId,
                                                                    ObjectFieldName = a.ObjectField.FieldName,
                                                                    Operator = a.Operator,
                                                                    PredefinedValue = a.PredefinedValue,
                                                                    PredefinedValue2 = a.PredefinedValue2,
                                                                    QueryCode = a.QueryCode,
                                                                    QueryId = a.QueryId,
                                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                                    QueryUserId = a.Query.UserId,
                                                                    Tenant = a.Tenant,
                                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                                    ObjectFieldOperator = a.ObjectField.Operator,
                                                                    UserId = a.UserId,
                                                                    ObjectFieldCode = a.ObjectFieldCode,
                                                                }).FirstOrDefault();

 
            return advancedFilters;

        }
    }
}