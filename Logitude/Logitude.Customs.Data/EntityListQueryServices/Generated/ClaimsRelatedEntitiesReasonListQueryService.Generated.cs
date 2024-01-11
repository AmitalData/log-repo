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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class ClaimsRelatedEntitiesReasonListQueryService
    {
         private ICustomContext context;
        public ClaimsRelatedEntitiesReasonListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<ClaimsRelatedEntitiesReasonList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<ClaimsRelatedEntitiesReasonList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ClaimsRelatedEntitiesReason> iQueryable = (from a in context.ClaimsRelatedEntitiesReasons
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<ClaimsRelatedEntitiesReason>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ClaimsRelatedEntitiesReasonList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ClaimsRelatedEntitiesReasonList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<ClaimsRelatedEntitiesReasonList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ClaimsRelatedEntitiesReasonList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ClaimsRelatedEntitiesReasonObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.ClaimsRelatedEntitiesReason",tenant).ToList();

                ObjectField objectField = (from a in ClaimsRelatedEntitiesReasonObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesReasonList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesReasonList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesReasonList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesReasonList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesReasonList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesReasonList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesReasonList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.ClaimId);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.ClaimId);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<ClaimsRelatedEntitiesReasonList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ClaimsRelatedEntitiesReasonList GetSingle(string claimid, int counterkey, int lineno)
        {
            IQueryable<ClaimsRelatedEntitiesReason> ClaimsRelatedEntitiesReasonQuery = (from a in context.ClaimsRelatedEntitiesReasons
                                                       where a.ClaimId == claimid && a.CounterKey == counterkey && a.LineNo == lineno
                                                       select a);

             
            IQueryable<ClaimsRelatedEntitiesReasonList> ClaimsRelatedEntitiesReasonListQuery = GetIqueryableList( ClaimsRelatedEntitiesReasonQuery);
            ClaimsRelatedEntitiesReasonList ClaimsRelatedEntitiesReasonList = ClaimsRelatedEntitiesReasonListQuery.FirstOrDefault();
            return ClaimsRelatedEntitiesReasonList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ClaimsRelatedEntitiesReason> iQueryable = (from a in context.ClaimsRelatedEntitiesReasons 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<ClaimsRelatedEntitiesReason>(nonListQueryOperation, iQueryable);



            IQueryable<ClaimsRelatedEntitiesReasonList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ClaimsRelatedEntitiesReasonList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<ClaimsRelatedEntitiesReasonList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 