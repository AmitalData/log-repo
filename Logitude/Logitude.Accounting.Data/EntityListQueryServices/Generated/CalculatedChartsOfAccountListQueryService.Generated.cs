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

    public partial class CalculatedChartsOfAccountListQueryService
    {
         private IAccountingContext context;
        public CalculatedChartsOfAccountListQueryService(IAccountingContext context)
        {
            this.context = context;
        }

        public List<CalculatedChartsOfAccountList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CalculatedChartsOfAccount> iQueryable = (from a in context.CalculatedChartsOfAccounts
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CalculatedChartsOfAccount>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CalculatedChartsOfAccountList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CalculatedChartsOfAccountList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CalculatedChartsOfAccountList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CalculatedChartsOfAccountObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CalculatedChartsOfAccount",tenant).ToList();

                ObjectField objectField = (from a in CalculatedChartsOfAccountObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsOfAccountList, decimal>(queryOperations, query2);
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

         public List<CalculatedChartsOfAccountList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CalculatedChartsOfAccountList GetSingle(string id)
        {
            IQueryable<CalculatedChartsOfAccount> CalculatedChartsOfAccountQuery = (from a in context.CalculatedChartsOfAccounts
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<CalculatedChartsOfAccountList> CalculatedChartsOfAccountListQuery = GetIqueryableList( CalculatedChartsOfAccountQuery);
            CalculatedChartsOfAccountList CalculatedChartsOfAccountList = CalculatedChartsOfAccountListQuery.FirstOrDefault();
            return CalculatedChartsOfAccountList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CalculatedChartsOfAccount> iQueryable = (from a in context.CalculatedChartsOfAccounts 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CalculatedChartsOfAccount>(nonListQueryOperation, iQueryable);

            IQueryable<CalculatedChartsOfAccountList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CalculatedChartsOfAccountList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 