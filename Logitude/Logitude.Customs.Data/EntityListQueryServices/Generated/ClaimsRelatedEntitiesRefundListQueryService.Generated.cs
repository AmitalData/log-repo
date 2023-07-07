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

    public partial class ClaimsRelatedEntitiesRefundListQueryService
    {
         private ICustomContext context;
        public ClaimsRelatedEntitiesRefundListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<ClaimsRelatedEntitiesRefundList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ClaimsRelatedEntitiesRefund> iQueryable = (from a in context.ClaimsRelatedEntitiesRefunds
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ClaimsRelatedEntitiesRefund>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ClaimsRelatedEntitiesRefundList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ClaimsRelatedEntitiesRefundList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ClaimsRelatedEntitiesRefundList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ClaimsRelatedEntitiesRefundObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.ClaimsRelatedEntitiesRefund",tenant).ToList();

                ObjectField objectField = (from a in ClaimsRelatedEntitiesRefundObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesRefundList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesRefundList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesRefundList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesRefundList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesRefundList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesRefundList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntitiesRefundList, decimal>(queryOperations, query2);
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

         public List<ClaimsRelatedEntitiesRefundList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ClaimsRelatedEntitiesRefundList GetSingle(string claimid, int counterkey, int refundquntitylineno)
        {
            IQueryable<ClaimsRelatedEntitiesRefund> ClaimsRelatedEntitiesRefundQuery = (from a in context.ClaimsRelatedEntitiesRefunds
                                                       where a.ClaimId == claimid && a.CounterKey == counterkey && a.RefundQuntityLineNo == refundquntitylineno
                                                       select a);

             
            IQueryable<ClaimsRelatedEntitiesRefundList> ClaimsRelatedEntitiesRefundListQuery = GetIqueryableList( ClaimsRelatedEntitiesRefundQuery);
            ClaimsRelatedEntitiesRefundList ClaimsRelatedEntitiesRefundList = ClaimsRelatedEntitiesRefundListQuery.FirstOrDefault();
            return ClaimsRelatedEntitiesRefundList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ClaimsRelatedEntitiesRefund> iQueryable = (from a in context.ClaimsRelatedEntitiesRefunds 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ClaimsRelatedEntitiesRefund>(nonListQueryOperation, iQueryable);

            IQueryable<ClaimsRelatedEntitiesRefundList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ClaimsRelatedEntitiesRefundList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 