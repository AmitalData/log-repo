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

    public partial class AdditionalCurrencyRateListQueryService
    {
         private IAccountingContext context;
        public AdditionalCurrencyRateListQueryService(IAccountingContext context)
        {
            this.context = context;
        }

        public List<AdditionalCurrencyRateList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<AdditionalCurrencyRateList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AdditionalCurrencyRate> iQueryable = (from a in context.AdditionalCurrencyRates
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<AdditionalCurrencyRate>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AdditionalCurrencyRateList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<AdditionalCurrencyRateList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<AdditionalCurrencyRateList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AdditionalCurrencyRateList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> AdditionalCurrencyRateObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AdditionalCurrencyRate",tenant).ToList();

                ObjectField objectField = (from a in AdditionalCurrencyRateObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<AdditionalCurrencyRateList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AdditionalCurrencyRateList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AdditionalCurrencyRateList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AdditionalCurrencyRateList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AdditionalCurrencyRateList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AdditionalCurrencyRateList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<AdditionalCurrencyRateList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.UpdateDate);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.UpdateDate);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<AdditionalCurrencyRateList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public AdditionalCurrencyRateList GetSingle(string id)
        {
            IQueryable<AdditionalCurrencyRate> AdditionalCurrencyRateQuery = (from a in context.AdditionalCurrencyRates
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<AdditionalCurrencyRateList> AdditionalCurrencyRateListQuery = GetIqueryableList( AdditionalCurrencyRateQuery);
            AdditionalCurrencyRateList AdditionalCurrencyRateList = AdditionalCurrencyRateListQuery.FirstOrDefault();
            return AdditionalCurrencyRateList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AdditionalCurrencyRate> iQueryable = (from a in context.AdditionalCurrencyRates 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<AdditionalCurrencyRate>(nonListQueryOperation, iQueryable);



            IQueryable<AdditionalCurrencyRateList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<AdditionalCurrencyRateList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<AdditionalCurrencyRateList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 