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

    public partial class ServiceProviderSubscriptionListQueryService
    {
         private IWorkflowContext context;
        public ServiceProviderSubscriptionListQueryService(IWorkflowContext context)
        {
            this.context = context;
        }

        public List<ServiceProviderSubscriptionList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<ServiceProviderSubscriptionList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ServiceProviderSubscription> iQueryable = (from a in context.ServiceProviderSubscriptions
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<ServiceProviderSubscription>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ServiceProviderSubscriptionList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ServiceProviderSubscriptionList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<ServiceProviderSubscriptionList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ServiceProviderSubscriptionList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ServiceProviderSubscriptionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ServiceProviderSubscription",tenant).ToList();

                ObjectField objectField = (from a in ServiceProviderSubscriptionObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ServiceProviderSubscriptionList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ServiceProviderSubscriptionList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ServiceProviderSubscriptionList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ServiceProviderSubscriptionList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ServiceProviderSubscriptionList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ServiceProviderSubscriptionList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ServiceProviderSubscriptionList, decimal>(queryOperations, query2);
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

         public List<ServiceProviderSubscriptionList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ServiceProviderSubscriptionList GetSingle(string id)
        {
            IQueryable<ServiceProviderSubscription> ServiceProviderSubscriptionQuery = (from a in context.ServiceProviderSubscriptions
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<ServiceProviderSubscriptionList> ServiceProviderSubscriptionListQuery = GetIqueryableList( ServiceProviderSubscriptionQuery);
            ServiceProviderSubscriptionList ServiceProviderSubscriptionList = ServiceProviderSubscriptionListQuery.FirstOrDefault();
            return ServiceProviderSubscriptionList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ServiceProviderSubscription> iQueryable = (from a in context.ServiceProviderSubscriptions 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<ServiceProviderSubscription>(nonListQueryOperation, iQueryable);



            IQueryable<ServiceProviderSubscriptionList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ServiceProviderSubscriptionList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<ServiceProviderSubscriptionList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 