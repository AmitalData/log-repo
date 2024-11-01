using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using AmitalCloud.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;  
using System.Reflection;
using AmitalCloud.Infrastructure.Domain.Helpers;

namespace AmitalCloud.Infrastructure.Application.BaseClasses
{

    public abstract class BaseEntityListQueryService<TEntityList, TEntity, TEntityKeys, TKeyType> : IEntityListQueryService<TEntityList> where TEntityList : class, new()
            where TEntity : BaseEntity
            where TEntityKeys : IEntityKeyFields<TEntity, TKeyType>, new()
    {
        protected IContext context;
        protected BaseEntityListQueryService(IContext context) => this.context = context;
        public List<TEntityList> GetList(int tenant) => GetList(new QueryOperations() { QueryFilterItems = new List<QueryFilterItem>(), PageIndex = 0, GetAll = true }, tenant);
        public List<TEntityList> GetList(QueryOperations queryOperations, int tenant) => GetList(queryOperations, tenant, new TreeFilterQueryArgs());
        public List<TEntityList> GetList(QueryOperations queryOperations, int tenant, TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericSort sortClass;
            int skippedPorts;
            IQueryable<TEntityList> query = GetQuery(queryOperations, treeFilterQueryArgs, out sortClass, out skippedPorts);
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TEntityList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> AccountingInformationIdentifierObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AccountingInformationIdentifier", tenant).ToList();
                ObjectField objectField = (from a in AccountingInformationIdentifierObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query = sortClass.GetSorterQuery<TEntityList, string>(queryOperations, query);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    query = sortClass.GetSorterQuery<TEntityList, string>(queryOperations, query);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    query = sortClass.GetSorterQuery<TEntityList, double>(queryOperations, query);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    query = sortClass.GetSorterQuery<TEntityList, DateTime>(queryOperations, query);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    query = sortClass.GetSorterQuery<TEntityList, int>(queryOperations, query);
                                    break;
                                }
                            case "boolean":
                                {
                                    query = sortClass.GetSorterQuery<TEntityList, bool>(queryOperations, query);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    query = sortClass.GetSorterQuery<TEntityList, decimal>(queryOperations, query);
                                    break;
                                }
                            default:
                                {
                                    //query2 = query2.OrderBy(d => d.Code);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                //query2 = query2.OrderBy(a => a.)
            }
            if (!queryOperations.GetAll)
            {
                query = query.Skip(skippedPorts).Take(queryOperations.PageSize);
            }
            return query.ToList();
        }
        private IQueryable<TEntityList> GetQuery(QueryOperations queryOperations, TreeFilterQueryArgs treeFilterQueryArgs, out GenericSort sortClass, out int skippedPorts)
        {
            GenericFilter filter = new GenericFilter();
            sortClass = new GenericSort();
            IQueryable<TEntity> iQueryable = Query();
            iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
            iQueryable = ApplyCustomFilters(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            iQueryable = filter.GetFilteredQuery<TEntity>(nonListQueryOperation, iQueryable);
            skippedPorts = queryOperations.PageIndex;
            IQueryable<TEntityList> query = filter.GetFilteredQuery<TEntityList>(listQueryOperation, GetIqueryableList(iQueryable));
            //if (Convert.ToBoolean(typeof(TEntity).GetField("HasTenant").GetValue(null)))
            //{
                //query = query.Where<TEntityList>(Predicate);
                treeFilterQueryArgs.Tenant = context.Tenant;
            //}
            return InjectionUtil.Instance.ApplyTreeFilter<TEntityList>(query, treeFilterQueryArgs);
        }
        public int GetListCount(QueryOperations queryOperations) => GetListCount(queryOperations, 0, new TreeFilterQueryArgs());
        public int GetListCount(QueryOperations queryOperations, TreeFilterQueryArgs treeFilterQueryArgs) => GetListCount(queryOperations, 0, treeFilterQueryArgs);
        public int GetListCount(QueryOperations queryOperations, int tenant, TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericSort sortClass;
            int skippedPorts;
            var query = GetQuery(queryOperations, treeFilterQueryArgs, out sortClass, out skippedPorts);
            //if (Convert.ToBoolean(typeof(TEntity).GetField("HasTenant").GetValue(null)))
            //{
            //    query = query.Where<TEntityList>(Predicate);
            //}

            return query.Count();
        }
        public TEntityList GetSingle(IEnumerable<KeyValuePair<string, string>> paramList)
        {
            var key = new TEntityKeys();
            key.Initialize(paramList);
            return GetIqueryableList(Query().Where(key.Predicate)).FirstOrDefault();
        }
        protected IQueryable<TEntity> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<TEntity> iQueryable) => iQueryable;
        protected IQueryable<TEntity> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<TEntity> iQueryable) => throw new NotImplementedException();
        private IQueryable<TEntity> Query() => (from a in contextEntity select a);
        protected abstract IDbSet<TEntity> contextEntity { get; }
        protected abstract IQueryable<TEntityList> GetIqueryableList(IQueryable<TEntity> iQueryable);
        //protected abstract Expression<Func<TEntityList, bool>> Predicate { get; }
    }
}