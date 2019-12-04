using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class QueryColumnQuery
    {
        QueryColumnRepository repository;

        public QueryColumnQuery()
        {
            repository = new QueryColumnRepository(); 
        }

        public QueryColumnQuery(int tenant)
        {
            repository = new QueryColumnRepository(tenant);
        }

        public QueryColumnQuery(QueryColumnRepository queryColumnRepository)
        {
            repository = queryColumnRepository;
        }


        public List<QueryColumnPM> GetQueryColumns(int tenant, string userid)
        {
            List<QueryColumnPM> querycolumns = (from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                                                where (a.Tenant == tenant && a.UserId == userid) || (a.Tenant == 0 && a.UserId == null)
                                                select new QueryColumnPM()
                                                {
                                                    ColumnWidth = a.ColumnWidth,
                                                    Id = a.Id,
                                                    IndexOrder = a.IndexOrder,
                                                    ObjectFieldId = a.ObjectFieldId,
                                                    ObjectFieldName = a.ObjectField.FieldName,
                                                    QueryId = a.QueryId,
                                                    Tenant = a.Tenant,
                                                    QueryCode = a.Query.Code,
                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                   
                                                    ConverterName = a.ObjectField.ConverterName,
                                                    DataTemplateName = a.ObjectField.DataTemplateName,
                                                    ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                                                    ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                                                    ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                                                    DisplayInList = a.ObjectField.DisplayInList,
                                                    UserId = a.UserId,
                                                    ObjectFieldFieldLableTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                                                    ObjectFieldCode = a.ObjectFieldCode,
                                                }).ToList();

            List<QueryColumnPM> selectedQueryColumns = new List<QueryColumnPM>();
            foreach (QueryColumnPM column in querycolumns)
            {
                QueryColumnPM existedColumn = (from a in selectedQueryColumns
                                               where a.QueryId == column.QueryId && a.ObjectFieldId == column.ObjectFieldId && a.UserId == userid
                                               select a).FirstOrDefault();

                if (existedColumn != null)
                {
                    if (existedColumn.Tenant == 0)
                    {
                        selectedQueryColumns.Remove(existedColumn);
                        selectedQueryColumns.Add(column);
                    }
                }

                else
                {
                    selectedQueryColumns.Add(column);
                }
            }

            return querycolumns;
        }

        public IQueryable<QueryColumnPM> GetQueryColumnPMsByTenant(int tenant)
        {
            IQueryable<QueryColumnPM> queries = from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                                                where a.Tenant == tenant
                                                select new QueryColumnPM()
                                                {
                                                    ColumnWidth = a.ColumnWidth,
                                                    Id = a.Id,
                                                    IndexOrder = a.IndexOrder,
                                                    ObjectFieldId = a.ObjectFieldId,
                                                    ObjectFieldName = a.ObjectField.FieldName,
                                                    QueryId = a.QueryId,
                                                    Tenant = a.Tenant,
                                                    QueryCode = a.Query.Code,
                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                 
                                                    ConverterName = a.ObjectField.ConverterName,
                                                    DataTemplateName = a.ObjectField.DataTemplateName,
                                                    ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                                                    ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                                                    ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                                                    DisplayInList = a.ObjectField.DisplayInList,
                                                    UserId = a.UserId,
                                                    ObjectFieldFieldLableTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                                                    ObjectFieldCode = a.ObjectFieldCode,
                                                };

            //string str = TranslateTextsClass.Translate(ObjectFieldListLabelTextCodeCode, tenant);

            return queries;
        }

        public IQueryable<QueryColumnPM> GetQueryColumnsByTenantAndNoUser(int tenant)
        {
            IQueryable<QueryColumnPM> queries = from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                                                where a.Tenant == tenant && a.Query.UserId == null
                                                select new QueryColumnPM()
                                                {
                                                    ColumnWidth = a.ColumnWidth,
                                                    Id = a.Id,
                                                    IndexOrder = a.IndexOrder,
                                                    ObjectFieldId = a.ObjectFieldId,
                                                    ObjectFieldName = a.ObjectField.FieldName,
                                                    QueryId = a.QueryId,
                                                    Tenant = a.Tenant,
                                                    QueryCode = a.Query.Code,
                                                    QueryObjectTableName = a.Query.ObjectTable.Name,
                                                  
                                                    ConverterName = a.ObjectField.ConverterName,
                                                    DataTemplateName = a.ObjectField.DataTemplateName,
                                                    ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                                                    ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                                                    ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                                                    DisplayInList = a.ObjectField.DisplayInList,
                                                    UserId = a.UserId,
                                                    ObjectFieldFieldLableTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                                                    ObjectFieldCode = a.ObjectFieldCode,
                                                };
            return queries;
        }

        public IQueryable<QueryColumnPM> GetQueryColumnsByTenantAndUser(int tenant, string userId)
        {
            IQueryable<QueryColumnPM> queries = null;
            if (!string.IsNullOrEmpty(userId))
            {
                queries = from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                          where a.Tenant == tenant && a.Query.UserId == userId
                          select new QueryColumnPM()
                          {
                              ColumnWidth = a.ColumnWidth,
                              Id = a.Id,
                              IndexOrder = a.IndexOrder,
                              ObjectFieldId = a.ObjectFieldId,
                              ObjectFieldName = a.ObjectField.FieldName,
                              QueryId = a.QueryId,
                              Tenant = a.Tenant,
                              QueryCode = a.Query.Code,
                              QueryObjectTableName = a.Query.ObjectTable.Name,
                           
                              ConverterName = a.ObjectField.ConverterName,
                              DataTemplateName = a.ObjectField.DataTemplateName,
                              ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                              ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                              ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                              DisplayInList = a.ObjectField.DisplayInList,
                              ObjectFieldFieldLableTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                              UserId = a.UserId,
                              ObjectFieldCode = a.ObjectFieldCode,
                          };

                if (queries.Count() == 0)
                {
                    queries = from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                              where a.Tenant == tenant
                              select new QueryColumnPM()
                              {
                                  ColumnWidth = a.ColumnWidth,
                                  Id = a.Id,
                                  IndexOrder = a.IndexOrder,
                                  ObjectFieldId = a.ObjectFieldId,
                                  ObjectFieldName = a.ObjectField.FieldName,
                                  QueryId = a.QueryId,
                                  Tenant = a.Tenant,
                                  QueryCode = a.Query.Code,
                                  QueryObjectTableName = a.Query.ObjectTable.Name,
                               
                                  ConverterName = a.ObjectField.ConverterName,
                                  DataTemplateName = a.ObjectField.DataTemplateName,
                                  ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                                  ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                                  ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                                  DisplayInList = a.ObjectField.DisplayInList,
                                  ObjectFieldFieldLableTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                                  ObjectFieldCode = a.ObjectFieldCode,
                              };
                }
            }
            else
            {
                queries = from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                          where a.Tenant == tenant
                          select new QueryColumnPM()
                          {
                              ColumnWidth = a.ColumnWidth,
                              Id = a.Id,
                              IndexOrder = a.IndexOrder,
                              ObjectFieldId = a.ObjectFieldId,
                              ObjectFieldName = a.ObjectField.FieldName,
                              QueryId = a.QueryId,
                              Tenant = a.Tenant,
                              QueryCode = a.Query.Code,
                              QueryObjectTableName = a.Query.ObjectTable.Name,
                              
                              ConverterName = a.ObjectField.ConverterName,
                              DataTemplateName = a.ObjectField.DataTemplateName,
                              ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                              ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                              ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                              DisplayInList = a.ObjectField.DisplayInList,
                              ObjectFieldFieldLableTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                              ObjectFieldCode = a.ObjectFieldCode,
                          };
            }




            return queries;
        }

        public IQueryable<QueryColumnPM> GetQueryColumnsByQueryIdAndUser(int tenant, string userId, string queryId)
        {
            IQueryable<QueryColumnPM> queries = null;
            Query myQuery = repository.context.Queries.Where(d => d.Id == queryId).FirstOrDefault();
            if (myQuery != null)
            {
                if (!string.IsNullOrEmpty(myQuery.SharedByUserId) && myQuery.SharedByUserId != userId)
                {
                    queries = from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                              where a.Tenant == tenant && (a.UserId == myQuery.SharedByUserId || a.UserId == userId) && a.QueryId == queryId && a.ObjectField.DisplayInList == true
                              select new QueryColumnPM()
                              {
                                  ColumnWidth = a.ColumnWidth,
                                  Id = a.Id,
                                  IndexOrder = a.IndexOrder,
                                  ObjectFieldId = a.ObjectFieldId,
                                  ObjectFieldName = a.ObjectField.FieldName,
                                  QueryId = a.QueryId,
                                  Tenant = a.Tenant,
                                  QueryCode = a.Query.Code,
                                  QueryObjectTableName = a.Query.ObjectTable.Name,
                                  ConverterName = a.ObjectField.ConverterName,
                                  DataTemplateName = a.ObjectField.DataTemplateName,
                                  ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                                  ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                                  ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                                  DisplayInList = a.ObjectField.DisplayInList,
                                  ObjectFieldDataTypeCode = a.ObjectField.DataTypeCode,
                                  UserId = a.UserId,
                                  ObjectFieldFullNameTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                                  ObjectFieldCode = a.ObjectFieldCode,
                              };
                }

                else
                {
                    queries = from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                              where a.Tenant == tenant && a.UserId == userId && a.QueryId == queryId && a.ObjectField.DisplayInList == true
                              select new QueryColumnPM()
                              {
                                  ColumnWidth = a.ColumnWidth,
                                  Id = a.Id,
                                  IndexOrder = a.IndexOrder,
                                  ObjectFieldId = a.ObjectFieldId,
                                  ObjectFieldName = a.ObjectField.FieldName,
                                  QueryId = a.QueryId,
                                  Tenant = a.Tenant,
                                  QueryCode = a.Query.Code,
                                  QueryObjectTableName = a.Query.ObjectTable.Name,
                                  ConverterName = a.ObjectField.ConverterName,
                                  DataTemplateName = a.ObjectField.DataTemplateName,
                                  ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                                  ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                                  ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                                  DisplayInList = a.ObjectField.DisplayInList,
                                  ObjectFieldDataTypeCode = a.ObjectField.DataTypeCode,
                                  UserId = a.UserId,
                                  ObjectFieldFullNameTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                                  ObjectFieldCode = a.ObjectFieldCode,
                              };
                }
            }

            return queries;
        }

        public IQueryable<QueryColumnPM> GetQueryColumnsByQueryIdAndUserAngular(int tenant, string userId, string queryId)
        {
            IQueryable<QueryColumnPM> queries = null;
            Query myQuery = repository.context.Queries.Where(d => d.Id == queryId).FirstOrDefault();

            if (myQuery != null)
            {
                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(myQuery.SharedByUserId) && myQuery.SharedByUserId != userId)
                {
                    queries = from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                              where a.Tenant == tenant && a.UserId == myQuery.SharedByUserId && a.QueryId == queryId && a.ObjectField.DisplayInList == true
                              select new QueryColumnPM()
                              {
                                  ColumnWidth = a.ColumnWidth,
                                  Id = a.Id,
                                  IndexOrder = a.IndexOrder,
                                  ObjectFieldId = a.ObjectFieldId,
                                  ObjectFieldName = a.ObjectField.FieldName,
                                  QueryId = a.QueryId,
                                  Tenant = a.Tenant,
                                  QueryCode = a.Query.Code,
                                  QueryObjectTableName = a.Query.ObjectTable.Name,
                                  ConverterName = a.ObjectField.ConverterName,
                                  DataTemplateName = a.ObjectField.DataTemplateName,
                                  ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                                  ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                                  ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                                  DisplayInList = a.ObjectField.DisplayInList,
                                  ObjectFieldDataTypeCode = a.ObjectField.DataTypeCode,
                                  UserId = a.UserId,
                                  ObjectFieldFullNameTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                                  ObjectFieldCode = a.ObjectFieldCode,
                              };
                }

                else
                {
                    queries = from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                              where a.Tenant == tenant && a.UserId == userId && a.QueryId == queryId && a.ObjectField.DisplayInList == true
                              select new QueryColumnPM()
                              {
                                  ColumnWidth = a.ColumnWidth,
                                  Id = a.Id,
                                  IndexOrder = a.IndexOrder,
                                  ObjectFieldId = a.ObjectFieldId,
                                  ObjectFieldName = a.ObjectField.FieldName,
                                  QueryId = a.QueryId,
                                  Tenant = a.Tenant,
                                  QueryCode = a.Query.Code,
                                  QueryObjectTableName = a.Query.ObjectTable.Name,
                                  ConverterName = a.ObjectField.ConverterName,
                                  DataTemplateName = a.ObjectField.DataTemplateName,
                                  ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                                  ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                                  ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                                  DisplayInList = a.ObjectField.DisplayInList,
                                  ObjectFieldDataTypeCode = a.ObjectField.DataTypeCode,
                                  UserId = a.UserId,
                                  ObjectFieldFullNameTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                                  ObjectFieldCode = a.ObjectFieldCode,
                              };
                }                  
            }

            List<QueryColumnPM> Cols = new List<QueryColumnPM>();

            if (queries != null)
            {
                foreach (var item in queries)
                {
                    if (!Cols.Contains(item))
                    {
                        Cols.Add(item);
                    }
                }
            }

            return Cols.AsQueryable();
        }

        public IQueryable<QueryColumnPM> GetQueryColumnsByQueryId(int tenant, string queryId)
        {
            IQueryable<QueryColumnPM> queries = null;
            //if (!string.IsNullOrEmpty(userId))
            //{
            queries = from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                      where a.Tenant == tenant && a.QueryId == queryId
                      select new QueryColumnPM()
                      {
                          ColumnWidth = a.ColumnWidth,
                          Id = a.Id,
                          IndexOrder = a.IndexOrder,
                          ObjectFieldId = a.ObjectFieldId,
                          ObjectFieldName = a.ObjectField.FieldName,
                          QueryId = a.QueryId,
                          Tenant = a.Tenant,
                          QueryCode = a.Query.Code,
                          QueryObjectTableName = a.Query.ObjectTable.Name,
                          //QueryUserId = a.Query.UserId,
                          ConverterName = a.ObjectField.ConverterName,
                          DataTemplateName = a.ObjectField.DataTemplateName,
                          ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                          ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                          ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                          DisplayInList = a.ObjectField.DisplayInList,
                          ObjectFieldDataTypeCode = a.ObjectField.DataTypeCode,
                          UserId = a.UserId,
                          ObjectFieldFullNameTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                          ObjectFieldCode = a.ObjectFieldCode,
                      };


            return queries;
        }
        
        public IQueryable<QueryColumnPM> GetZeroQueryColumnsByQueryId(int tenant, string queryId)
        {
            IQueryable<QueryColumnPM> queries = null;
            //if (!string.IsNullOrEmpty(userId))
            //{
            queries = from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                      where a.Tenant == tenant && a.QueryId == queryId && a.UserId == null
                      select new QueryColumnPM()
                      {
                          ColumnWidth = a.ColumnWidth,
                          Id = a.Id,
                          IndexOrder = a.IndexOrder,
                          ObjectFieldId = a.ObjectFieldId,
                          ObjectFieldName = a.ObjectField.FieldName,
                          QueryId = a.QueryId,
                          Tenant = a.Tenant,
                          QueryCode = a.Query.Code,
                          QueryObjectTableName = a.Query.ObjectTable.Name,
                          //QueryUserId = a.Query.UserId,
                          ConverterName = a.ObjectField.ConverterName,
                          DataTemplateName = a.ObjectField.DataTemplateName,
                          ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                          ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                          ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                          DisplayInList = a.ObjectField.DisplayInList,
                          ObjectFieldDataTypeCode = a.ObjectField.DataTypeCode,
                          UserId = a.UserId,
                          ObjectFieldFullNameTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                          ObjectFieldCode = a.ObjectFieldCode,
                      };


            return queries;
        }

        public IQueryable<QueryColumnPM> GetQueryColumnsByQueryTenant(int tenant, string queryName)
        {
            IQueryable<QueryColumnPM> query = from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                                              where a.Query.Code == queryName && a.Tenant == tenant
                                              select new QueryColumnPM()
                                              {
                                                  ColumnWidth = a.ColumnWidth,
                                                  Id = a.Id,
                                                  IndexOrder = a.IndexOrder,
                                                  ObjectFieldId = a.ObjectFieldId,
                                                  ObjectFieldName = a.ObjectField.FieldName,
                                                  QueryId = a.QueryId,
                                                  Tenant = a.Tenant,
                                                  QueryCode = a.Query.Code,
                                                  QueryObjectTableName = a.Query.ObjectTable.Name,
                                                 
                                                  ConverterName = a.ObjectField.ConverterName,
                                                  DataTemplateName = a.ObjectField.DataTemplateName,
                                                  ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                                                  ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                                                  ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                                                  DisplayInList = a.ObjectField.DisplayInList,
                                                  UserId = a.UserId,
                                                  ObjectFieldFieldLableTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                                                  ObjectFieldCode = a.ObjectFieldCode,
                                              };
            return query;
        }

        public QueryColumnPM GetQueryColumnsByFieldIdTenant(string FieldId, int Tenant)
        {
            QueryColumnPM query = (from a in repository.context.QueryColumns.Include("Query").Include("Query.ObjectTable").Include("ObjectField").Include("ObjectField.ListTextCode").Include("ObjectField.FullNameTextCode")
                                              where a.ObjectFieldId == FieldId && a.Tenant == Tenant
                                              select new QueryColumnPM()
                                              {
                                                  ColumnWidth = a.ColumnWidth,
                                                  Id = a.Id,
                                                  IndexOrder = a.IndexOrder,
                                                  ObjectFieldId = a.ObjectFieldId,
                                                  ObjectFieldName = a.ObjectField.FieldName,
                                                  QueryId = a.QueryId,
                                                  Tenant = a.Tenant,
                                                  QueryCode = a.Query.Code,
                                                  QueryObjectTableName = a.Query.ObjectTable.Name,

                                                  ConverterName = a.ObjectField.ConverterName,
                                                  DataTemplateName = a.ObjectField.DataTemplateName,
                                                  ObjectFieldListLabelTextCodeCode = a.ObjectField.ListTextCode.Code,
                                                  ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                                                  ObjectFieldFieldLableTextCodeDefaultText = a.ObjectField.FullNameTextCode.DefaultText,
                                                  DisplayInList = a.ObjectField.DisplayInList,
                                                  UserId = a.UserId,
                                                  ObjectFieldFieldLableTextCodeCode = a.ObjectField.FullNameTextCode.Code,
                                                  ObjectFieldCode = a.ObjectFieldCode,
                                              }).FirstOrDefault();
            return query;
        }
    }
}