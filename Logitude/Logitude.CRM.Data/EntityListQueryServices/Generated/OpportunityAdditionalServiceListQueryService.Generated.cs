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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class OpportunityAdditionalServiceListQueryService
    {
         private ICRMContext context;
        public OpportunityAdditionalServiceListQueryService(ICRMContext context)
        {
            this.context = context;
        }

        public List<OpportunityAdditionalServiceList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<OpportunityAdditionalServiceList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<OpportunityAdditionalService> iQueryable = (from a in context.OpportunityAdditionalServices
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<OpportunityAdditionalService>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<OpportunityAdditionalServiceList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<OpportunityAdditionalServiceList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<OpportunityAdditionalServiceList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(OpportunityAdditionalServiceList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> OpportunityAdditionalServiceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("OpportunityAdditionalService",tenant).ToList();

                ObjectField objectField = (from a in OpportunityAdditionalServiceObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<OpportunityAdditionalServiceList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<OpportunityAdditionalServiceList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<OpportunityAdditionalServiceList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<OpportunityAdditionalServiceList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<OpportunityAdditionalServiceList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<OpportunityAdditionalServiceList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<OpportunityAdditionalServiceList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.OpportunityId);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.OpportunityId);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<OpportunityAdditionalServiceList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public OpportunityAdditionalServiceList GetSingle(string opportunityid, string additionalserviceid)
        {
            IQueryable<OpportunityAdditionalService> OpportunityAdditionalServiceQuery = (from a in context.OpportunityAdditionalServices
                                                       where a.OpportunityId == opportunityid && a.AdditionalServiceId == additionalserviceid
                                                       select a);

             
            IQueryable<OpportunityAdditionalServiceList> OpportunityAdditionalServiceListQuery = GetIqueryableList( OpportunityAdditionalServiceQuery);
            OpportunityAdditionalServiceList OpportunityAdditionalServiceList = OpportunityAdditionalServiceListQuery.FirstOrDefault();
            return OpportunityAdditionalServiceList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<OpportunityAdditionalService> iQueryable = (from a in context.OpportunityAdditionalServices 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<OpportunityAdditionalService>(nonListQueryOperation, iQueryable);



            IQueryable<OpportunityAdditionalServiceList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<OpportunityAdditionalServiceList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<OpportunityAdditionalServiceList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 