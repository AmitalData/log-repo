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

    public partial class LogisticActionResponseReqSListQueryService
    {
         private ICustomContext context;
        public LogisticActionResponseReqSListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<LogisticActionResponseReqSList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<LogisticActionResponseReqS> iQueryable = (from a in context.LogisticActionResponseReqSes
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<LogisticActionResponseReqS>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<LogisticActionResponseReqSList> query2 = GetIqueryableList(iQueryable);
					  query2 = filter.GetFilteredQuery<LogisticActionResponseReqSList>(listQueryOperation, query2);
		
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(LogisticActionResponseReqSList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> LogisticActionResponseReqSObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.LogisticActionResponseReqS",tenant).ToList();

                ObjectField objectField = (from a in LogisticActionResponseReqSObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<LogisticActionResponseReqSList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<LogisticActionResponseReqSList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<LogisticActionResponseReqSList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<LogisticActionResponseReqSList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<LogisticActionResponseReqSList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<LogisticActionResponseReqSList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<LogisticActionResponseReqSList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Code);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Code);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<LogisticActionResponseReqSList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public LogisticActionResponseReqSList GetSingle(string code)
        {
            IQueryable<LogisticActionResponseReqS> LogisticActionResponseReqSQuery = (from a in context.LogisticActionResponseReqSes
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<LogisticActionResponseReqSList> LogisticActionResponseReqSListQuery = GetIqueryableList( LogisticActionResponseReqSQuery);
            LogisticActionResponseReqSList LogisticActionResponseReqSList = LogisticActionResponseReqSListQuery.FirstOrDefault();
            return LogisticActionResponseReqSList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<LogisticActionResponseReqS> iQueryable = (from a in context.LogisticActionResponseReqSes  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<LogisticActionResponseReqS>(nonListQueryOperation, iQueryable);

            IQueryable<LogisticActionResponseReqSList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<LogisticActionResponseReqSList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 