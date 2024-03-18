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

    public partial class CB_AdditionRulesDetailsHistoryListQueryService
    {
         private ICustomContext context;
        public CB_AdditionRulesDetailsHistoryListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CB_AdditionRulesDetailsHistoryList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CB_AdditionRulesDetailsHistory> iQueryable = (from a in context.CB_AdditionRulesDetailsHistorys
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CB_AdditionRulesDetailsHistory>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CB_AdditionRulesDetailsHistoryList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CB_AdditionRulesDetailsHistoryList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CB_AdditionRulesDetailsHistoryList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CB_AdditionRulesDetailsHistoryObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CB_AdditionRulesDetailsHistory",tenant).ToList();

                ObjectField objectField = (from a in CB_AdditionRulesDetailsHistoryObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CB_AdditionRulesDetailsHistoryList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CB_AdditionRulesDetailsHistoryList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CB_AdditionRulesDetailsHistoryList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CB_AdditionRulesDetailsHistoryList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CB_AdditionRulesDetailsHistoryList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CB_AdditionRulesDetailsHistoryList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CB_AdditionRulesDetailsHistoryList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.ID);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.ID);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CB_AdditionRulesDetailsHistoryList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CB_AdditionRulesDetailsHistoryList GetSingle(int id)
        {
            IQueryable<CB_AdditionRulesDetailsHistory> CB_AdditionRulesDetailsHistoryQuery = (from a in context.CB_AdditionRulesDetailsHistorys
                                                       where a.ID == id
                                                       select a);

             
            IQueryable<CB_AdditionRulesDetailsHistoryList> CB_AdditionRulesDetailsHistoryListQuery = GetIqueryableList( CB_AdditionRulesDetailsHistoryQuery);
            CB_AdditionRulesDetailsHistoryList CB_AdditionRulesDetailsHistoryList = CB_AdditionRulesDetailsHistoryListQuery.FirstOrDefault();
            return CB_AdditionRulesDetailsHistoryList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CB_AdditionRulesDetailsHistory> iQueryable = (from a in context.CB_AdditionRulesDetailsHistorys  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CB_AdditionRulesDetailsHistory>(nonListQueryOperation, iQueryable);

            IQueryable<CB_AdditionRulesDetailsHistoryList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CB_AdditionRulesDetailsHistoryList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 