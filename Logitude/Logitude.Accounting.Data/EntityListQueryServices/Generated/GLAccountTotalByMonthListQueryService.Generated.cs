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

    public partial class GLAccountTotalByMonthListQueryService
    {
         private IAccountingContext context;
        public GLAccountTotalByMonthListQueryService(IAccountingContext context)
        {
            this.context = context;
        }

        public List<GLAccountTotalByMonthList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<GLAccountTotalByMonthList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<GLAccountTotalByMonth> iQueryable = (from a in context.GLAccountTotalByMonths
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<GLAccountTotalByMonth>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<GLAccountTotalByMonthList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<GLAccountTotalByMonthList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<GLAccountTotalByMonthList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(GLAccountTotalByMonthList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> GLAccountTotalByMonthObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("GLAccountTotalByMonth",tenant).ToList();

                ObjectField objectField = (from a in GLAccountTotalByMonthObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<GLAccountTotalByMonthList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<GLAccountTotalByMonthList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<GLAccountTotalByMonthList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<GLAccountTotalByMonthList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<GLAccountTotalByMonthList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<GLAccountTotalByMonthList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<GLAccountTotalByMonthList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.AccountId);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.AccountId);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<GLAccountTotalByMonthList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public GLAccountTotalByMonthList GetSingle(string accountid, string datetypecode, int year, int month, string currencyid)
        {
            IQueryable<GLAccountTotalByMonth> GLAccountTotalByMonthQuery = (from a in context.GLAccountTotalByMonths
                                                       where a.AccountId == accountid && a.DateTypeCode == datetypecode && a.Year == year && a.Month == month && a.CurrencyId == currencyid
                                                       select a);

             
            IQueryable<GLAccountTotalByMonthList> GLAccountTotalByMonthListQuery = GetIqueryableList( GLAccountTotalByMonthQuery);
            GLAccountTotalByMonthList GLAccountTotalByMonthList = GLAccountTotalByMonthListQuery.FirstOrDefault();
            return GLAccountTotalByMonthList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<GLAccountTotalByMonth> iQueryable = (from a in context.GLAccountTotalByMonths 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<GLAccountTotalByMonth>(nonListQueryOperation, iQueryable);



            IQueryable<GLAccountTotalByMonthList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<GLAccountTotalByMonthList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<GLAccountTotalByMonthList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 