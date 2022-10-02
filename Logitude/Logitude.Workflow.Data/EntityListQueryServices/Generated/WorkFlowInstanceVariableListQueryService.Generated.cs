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

    public partial class WorkFlowInstanceVariableListQueryService
    {
         private IWorkflowContext context;
        public WorkFlowInstanceVariableListQueryService(IWorkflowContext context)
        {
            this.context = context;
        }

        public List<WorkFlowInstanceVariableList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<WorkFlowInstanceVariableList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<WorkFlowInstanceVariable> iQueryable = (from a in context.WorkFlowInstanceVariables
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<WorkFlowInstanceVariable>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<WorkFlowInstanceVariableList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<WorkFlowInstanceVariableList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<WorkFlowInstanceVariableList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(WorkFlowInstanceVariableList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> WorkFlowInstanceVariableObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("WorkFlowInstanceVariable",tenant).ToList();

                ObjectField objectField = (from a in WorkFlowInstanceVariableObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<WorkFlowInstanceVariableList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowInstanceVariableList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowInstanceVariableList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowInstanceVariableList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowInstanceVariableList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowInstanceVariableList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowInstanceVariableList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CreateDate);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.CreateDate);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<WorkFlowInstanceVariableList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public WorkFlowInstanceVariableList GetSingle(string id)
        {
            IQueryable<WorkFlowInstanceVariable> WorkFlowInstanceVariableQuery = (from a in context.WorkFlowInstanceVariables
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<WorkFlowInstanceVariableList> WorkFlowInstanceVariableListQuery = GetIqueryableList( WorkFlowInstanceVariableQuery);
            WorkFlowInstanceVariableList WorkFlowInstanceVariableList = WorkFlowInstanceVariableListQuery.FirstOrDefault();
            return WorkFlowInstanceVariableList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<WorkFlowInstanceVariable> iQueryable = (from a in context.WorkFlowInstanceVariables 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<WorkFlowInstanceVariable>(nonListQueryOperation, iQueryable);



            IQueryable<WorkFlowInstanceVariableList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<WorkFlowInstanceVariableList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<WorkFlowInstanceVariableList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 