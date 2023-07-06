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

    public partial class ConsignmentInternalTransitionListQueryService
    {
         private ICustomContext context;
        public ConsignmentInternalTransitionListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<ConsignmentInternalTransitionList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ConsignmentInternalTransition> iQueryable = (from a in context.ConsignmentInternalTransitions
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ConsignmentInternalTransition>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ConsignmentInternalTransitionList> query2 = GetIqueryableList(iQueryable);
<<<<<<< HEAD
					  query2 = filter.GetFilteredQuery<ConsignmentInternalTransitionList>(listQueryOperation, query2);
		
=======
           
            query2 = filter.GetFilteredQuery<ConsignmentInternalTransitionList>(listQueryOperation, query2);

>>>>>>> parent of 094118ae335 (#181601)
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ConsignmentInternalTransitionList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ConsignmentInternalTransitionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.ConsignmentInternalTransition",tenant).ToList();

                ObjectField objectField = (from a in ConsignmentInternalTransitionObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ConsignmentInternalTransitionList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ConsignmentInternalTransitionList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ConsignmentInternalTransitionList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ConsignmentInternalTransitionList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ConsignmentInternalTransitionList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ConsignmentInternalTransitionList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ConsignmentInternalTransitionList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.SiteCode);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.SiteCode);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<ConsignmentInternalTransitionList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ConsignmentInternalTransitionList GetSingle(string declarationid, int? consignmentnumber, int linenumber)
        {
            IQueryable<ConsignmentInternalTransition> ConsignmentInternalTransitionQuery = (from a in context.ConsignmentInternalTransitions
                                                       where a.DeclarationId == declarationid && a.ConsignmentNumber == consignmentnumber && a.LineNumber == linenumber
                                                       select a);

             
            IQueryable<ConsignmentInternalTransitionList> ConsignmentInternalTransitionListQuery = GetIqueryableList( ConsignmentInternalTransitionQuery);
            ConsignmentInternalTransitionList ConsignmentInternalTransitionList = ConsignmentInternalTransitionListQuery.FirstOrDefault();
            return ConsignmentInternalTransitionList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ConsignmentInternalTransition> iQueryable = (from a in context.ConsignmentInternalTransitions 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ConsignmentInternalTransition>(nonListQueryOperation, iQueryable);

            IQueryable<ConsignmentInternalTransitionList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<ConsignmentInternalTransitionList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 