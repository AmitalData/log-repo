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

    public partial class WorkFlowInstanceActivityListQueryService
    {
         private IWorkflowContext context;
        public WorkFlowInstanceActivityListQueryService(IWorkflowContext context)
        {
            this.context = context;
        }

        public List<WorkFlowInstanceActivityList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<WorkFlowInstanceActivityList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<WorkFlowInstanceActivity> iQueryable = (from a in context.WorkFlowInstanceActivities
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<WorkFlowInstanceActivity>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<WorkFlowInstanceActivityList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<WorkFlowInstanceActivityList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<WorkFlowInstanceActivityList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(WorkFlowInstanceActivityList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> WorkFlowInstanceActivityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("WorkFlowInstanceActivity",tenant).ToList();

                ObjectField objectField = (from a in WorkFlowInstanceActivityObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<WorkFlowInstanceActivityList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowInstanceActivityList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowInstanceActivityList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowInstanceActivityList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowInstanceActivityList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowInstanceActivityList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<WorkFlowInstanceActivityList, decimal>(queryOperations, query2);
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

         public List<WorkFlowInstanceActivityList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public WorkFlowInstanceActivityList GetSingle(string id)
        {
            IQueryable<WorkFlowInstanceActivity> WorkFlowInstanceActivityQuery = (from a in context.WorkFlowInstanceActivities
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<WorkFlowInstanceActivityList> WorkFlowInstanceActivityListQuery = GetIqueryableList( WorkFlowInstanceActivityQuery);
            WorkFlowInstanceActivityList WorkFlowInstanceActivityList = WorkFlowInstanceActivityListQuery.FirstOrDefault();
            return WorkFlowInstanceActivityList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<WorkFlowInstanceActivity> iQueryable = (from a in context.WorkFlowInstanceActivities 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<WorkFlowInstanceActivity>(nonListQueryOperation, iQueryable);



            IQueryable<WorkFlowInstanceActivityList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<WorkFlowInstanceActivityList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<WorkFlowInstanceActivityList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 