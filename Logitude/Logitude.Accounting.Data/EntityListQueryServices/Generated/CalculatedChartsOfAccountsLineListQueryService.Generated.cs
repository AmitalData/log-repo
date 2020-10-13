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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class CalculatedChartsOfAccountsLineListQueryService
    {
         private IAccountingContext context;
        public CalculatedChartsOfAccountsLineListQueryService(IAccountingContext context)
        {
            this.context = context;
        }

        public List<CalculatedChartsOfAccountsLineList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CalculatedChartsOfAccountsLine> iQueryable = (from a in context.CalculatedChartsOfAccountsLines
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CalculatedChartsOfAccountsLine>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CalculatedChartsOfAccountsLineList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CalculatedChartsOfAccountsLineList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CalculatedChartsOfAccountsLineList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CalculatedChartsOfAccountsLineObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CalculatedChartsOfAccountsLine",tenant).ToList();

                ObjectField objectField = (from a in CalculatedChartsOfAccountsLineObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountsLineList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountsLineList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountsLineList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountsLineList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountsLineList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountsLineList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountsLineList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CreateDateTime);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.CreateDateTime);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CalculatedChartsOfAccountsLineList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CalculatedChartsOfAccountsLineList GetSingle(string id)
        {
            IQueryable<CalculatedChartsOfAccountsLine> CalculatedChartsOfAccountsLineQuery = (from a in context.CalculatedChartsOfAccountsLines
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<CalculatedChartsOfAccountsLineList> CalculatedChartsOfAccountsLineListQuery = GetIqueryableList( CalculatedChartsOfAccountsLineQuery);
            CalculatedChartsOfAccountsLineList CalculatedChartsOfAccountsLineList = CalculatedChartsOfAccountsLineListQuery.FirstOrDefault();
            return CalculatedChartsOfAccountsLineList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CalculatedChartsOfAccountsLine> iQueryable = (from a in context.CalculatedChartsOfAccountsLines 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CalculatedChartsOfAccountsLine>(nonListQueryOperation, iQueryable);

            IQueryable<CalculatedChartsOfAccountsLineList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CalculatedChartsOfAccountsLineList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 