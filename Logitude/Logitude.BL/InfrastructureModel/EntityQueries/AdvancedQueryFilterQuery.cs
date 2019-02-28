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
                                                                    QueryCode = a.Query.Code,
                                                                    QueryId = a.QueryId,
                                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                                    QueryUserId = a.Query.UserId,
                                                                    Tenant = a.Tenant,
                                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                                    ObjectFieldOperator = a.ObjectField.Operator,
                                                                    UserId = a.UserId,
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
                                                                    QueryCode = a.Query.Code,
                                                                    QueryId = a.QueryId,
                                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                                    QueryUserId = a.Query.UserId,
                                                                    Tenant = a.Tenant,
                                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                                    ObjectFieldOperator = a.ObjectField.Operator,
                                                                    UserId = a.UserId,
                                                                };
            return advancedFilters;

        }

        public IQueryable<AdvancedQueryFilterPM> GetAdvancedQueryFilterPMsByTenantAndUserAndQuery(int tenant, string userId, string queryId)
        {
            IQueryable<AdvancedQueryFilterPM> advancedFilters = null;
            Query myQuery = repository.context.Queries.Where(d => d.Id == queryId).FirstOrDefault();

            if (myQuery != null)
            {
                if (!string.IsNullOrEmpty(myQuery.SharedByUserId) && myQuery.SharedByUserId != userId)
                {
                    advancedFilters = from a in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                                      where (a.Tenant == tenant && a.Query.UserId == myQuery.SharedByUserId && a.QueryId == queryId) || a.Tenant == 0
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
                                          QueryCode = a.Query.Code,
                                          QueryId = a.QueryId,
                                          QueryObjectTableName = a.Query.ObjectTable.Name,
                                          QueryUserId = a.Query.UserId,
                                          Tenant = a.Tenant,
                                          DataTypeCode = a.ObjectField.DataTypeCode,
                                          ObjectFieldOperator = a.ObjectField.Operator,
                                          UserId = a.UserId,
                                      };
                }

                else
                {
                    advancedFilters = from a in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                                      where (a.Tenant == tenant && (a.Query.UserId == userId && a.UserId != null) && a.QueryId == queryId) || (a.Tenant == tenant && a.QueryId == queryId && a.UserId == userId) || (a.Tenant == 0 && a.QueryId == queryId && a.UserId == null)
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
                                          QueryCode = a.Query.Code,
                                          QueryId = a.QueryId,
                                          QueryObjectTableName = a.Query.ObjectTable.Name,
                                          QueryUserId = a.Query.UserId,
                                          Tenant = a.Tenant,
                                          DataTypeCode = a.ObjectField.DataTypeCode,
                                          ObjectFieldOperator = a.ObjectField.Operator,
                                          UserId = a.UserId,
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
                                                                    QueryCode = a.Query.Code,
                                                                    QueryId = a.QueryId,
                                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                                    QueryUserId = a.Query.UserId,
                                                                    Tenant = a.Tenant,
                                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                                    ObjectFieldOperator = a.ObjectField.Operator,
                                                                    UserId = a.UserId,
                                                                };
            return advancedFilters;

        }

        public IQueryable<AdvancedQueryFilterPM> GetAdvancedQueryFiltersByQueryId(int tenant, string queryId)
        {
            IQueryable<AdvancedQueryFilterPM> advancedFilters = from a in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                                                                where a.Tenant == tenant && a.QueryId == queryId
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
                                                                    QueryCode = a.Query.Code,
                                                                    QueryId = a.QueryId,
                                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                                    QueryUserId = a.Query.UserId,
                                                                    Tenant = a.Tenant,
                                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                                    ObjectFieldOperator = a.ObjectField.Operator,
                                                                    UserId = a.UserId,
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
                                                                    QueryCode = a.Query.Code,
                                                                    QueryId = a.QueryId,
                                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                                    QueryUserId = a.Query.UserId,
                                                                    Tenant = a.Tenant,
                                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                                    ObjectFieldOperator = a.ObjectField.Operator,
                                                                    UserId = a.UserId,
                                                                };
            return advancedFilters;

        }

        public AdvancedQueryFilterPM GetPredefinedQueryFilterByFieldIdTenant(string FieldId,int tenant)
        {
            AdvancedQueryFilterPM advancedFilters = (from a in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                                                    where a.Tenant == tenant && a.ObjectFieldId == FieldId
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
                                                                    QueryCode = a.Query.Code,
                                                                    QueryId = a.QueryId,
                                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                                    QueryUserId = a.Query.UserId,
                                                                    Tenant = a.Tenant,
                                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                                    ObjectFieldOperator = a.ObjectField.Operator,
                                                                    UserId = a.UserId,
                                                                }).FirstOrDefault();
            return advancedFilters;

        }
    }
}