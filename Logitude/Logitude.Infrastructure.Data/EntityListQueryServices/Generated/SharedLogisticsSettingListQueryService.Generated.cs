using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{ 

    public partial class SharedLogisticsSettingListQueryService
    {
         private IInfrastructureContext context;
        public SharedLogisticsSettingListQueryService(IInfrastructureContext context)
        {
            this.context = context;
        }

        public List<SharedLogisticsSettingList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<SharedLogisticsSettingList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SharedLogisticsSetting> iQueryable = (from a in context.SharedLogisticsSettings
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<SharedLogisticsSetting>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<SharedLogisticsSettingList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<SharedLogisticsSettingList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<SharedLogisticsSettingList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SharedLogisticsSettingList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> SharedLogisticsSettingObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("SharedLogisticsSetting",tenant).ToList();

                ObjectField objectField = (from a in SharedLogisticsSettingObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<SharedLogisticsSettingList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsSettingList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsSettingList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsSettingList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsSettingList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsSettingList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsSettingList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Id);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Id);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<SharedLogisticsSettingList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public SharedLogisticsSettingList GetSingle(string id)
        {
            IQueryable<SharedLogisticsSetting> SharedLogisticsSettingQuery = (from a in context.SharedLogisticsSettings
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<SharedLogisticsSettingList> SharedLogisticsSettingListQuery = GetIqueryableList( SharedLogisticsSettingQuery);
            SharedLogisticsSettingList SharedLogisticsSettingList = SharedLogisticsSettingListQuery.FirstOrDefault();
            return SharedLogisticsSettingList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SharedLogisticsSetting> iQueryable = (from a in context.SharedLogisticsSettings 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<SharedLogisticsSetting>(nonListQueryOperation, iQueryable);



            IQueryable<SharedLogisticsSettingList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<SharedLogisticsSettingList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<SharedLogisticsSettingList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 