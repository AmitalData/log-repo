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

    public partial class PropertiesDetailsHistoryListQueryService
    {
         private ICustomContext context;
        public PropertiesDetailsHistoryListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<PropertiesDetailsHistoryList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PropertiesDetailsHistory> iQueryable = (from a in context.PropertiesDetailsHistorys
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PropertiesDetailsHistory>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<PropertiesDetailsHistoryList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<PropertiesDetailsHistoryList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PropertiesDetailsHistoryList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> PropertiesDetailsHistoryObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.PropertiesDetailsHistory",tenant).ToList();

                ObjectField objectField = (from a in PropertiesDetailsHistoryObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<PropertiesDetailsHistoryList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PropertiesDetailsHistoryList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PropertiesDetailsHistoryList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PropertiesDetailsHistoryList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PropertiesDetailsHistoryList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PropertiesDetailsHistoryList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<PropertiesDetailsHistoryList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.ID);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.ID);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<PropertiesDetailsHistoryList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public PropertiesDetailsHistoryList GetSingle(string id)
        {
            IQueryable<PropertiesDetailsHistory> PropertiesDetailsHistoryQuery = (from a in context.PropertiesDetailsHistorys
                                                       where a.ID == id
                                                       select a);

             
            IQueryable<PropertiesDetailsHistoryList> PropertiesDetailsHistoryListQuery = GetIqueryableList( PropertiesDetailsHistoryQuery);
            PropertiesDetailsHistoryList PropertiesDetailsHistoryList = PropertiesDetailsHistoryListQuery.FirstOrDefault();
            return PropertiesDetailsHistoryList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PropertiesDetailsHistory> iQueryable = (from a in context.PropertiesDetailsHistorys  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<PropertiesDetailsHistory>(nonListQueryOperation, iQueryable);

            IQueryable<PropertiesDetailsHistoryList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<PropertiesDetailsHistoryList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 