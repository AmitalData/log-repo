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

using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.EntityLists;

namespace Logitude.Workflow.Data.EntityListQueryServices
{ 

    public partial class WorkFlowVersionStatusListQueryService
    {
         private IWorkflowContext context;
        public WorkFlowVersionStatusListQueryService(IWorkflowContext context)
        {
            this.context = context;
        }

        public List<WorkFlowVersionStatusList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<WorkFlowVersionStatusList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<WorkFlowVersionStatus> iQueryable = (from a in context.WorkFlowVersionStatuses
                                               select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<WorkFlowVersionStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<WorkFlowVersionStatusList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<WorkFlowVersionStatusList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<WorkFlowVersionStatusList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(WorkFlowVersionStatusList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> WorkFlowVersionStatusObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("WorkFlowVersionStatus",tenant).ToList();

                ObjectField objectField = (from a in WorkFlowVersionStatusObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<WorkFlowVersionStatusList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowVersionStatusList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowVersionStatusList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowVersionStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowVersionStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowVersionStatusList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowVersionStatusList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Name);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Name);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<WorkFlowVersionStatusList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public WorkFlowVersionStatusList GetSingle(string code)
        {
            IQueryable<WorkFlowVersionStatus> WorkFlowVersionStatusQuery = (from a in context.WorkFlowVersionStatuses
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<WorkFlowVersionStatusList> WorkFlowVersionStatusListQuery = GetIqueryableList( WorkFlowVersionStatusQuery);
            WorkFlowVersionStatusList WorkFlowVersionStatusList = WorkFlowVersionStatusListQuery.FirstOrDefault();
            return WorkFlowVersionStatusList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations ){
		 		  return GetListCount(queryOperations, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<WorkFlowVersionStatus> iQueryable = (from a in context.WorkFlowVersionStatuses  select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<WorkFlowVersionStatus>(nonListQueryOperation, iQueryable);



            IQueryable<WorkFlowVersionStatusList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<WorkFlowVersionStatusList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<WorkFlowVersionStatusList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 