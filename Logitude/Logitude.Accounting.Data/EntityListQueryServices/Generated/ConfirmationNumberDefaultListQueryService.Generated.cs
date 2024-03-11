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

    public partial class ConfirmationNumberDefaultListQueryService
    {
         private IAccountingContext context;
        public ConfirmationNumberDefaultListQueryService(IAccountingContext context)
        {
            this.context = context;
        }

        public List<ConfirmationNumberDefaultList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<ConfirmationNumberDefaultList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ConfirmationNumberDefault> iQueryable = (from a in context.ConfirmationNumberDefaults
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<ConfirmationNumberDefault>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ConfirmationNumberDefaultList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ConfirmationNumberDefaultList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<ConfirmationNumberDefaultList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ConfirmationNumberDefaultList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ConfirmationNumberDefaultObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ConfirmationNumberDefault",tenant).ToList();

                ObjectField objectField = (from a in ConfirmationNumberDefaultObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ConfirmationNumberDefaultList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ConfirmationNumberDefaultList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ConfirmationNumberDefaultList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ConfirmationNumberDefaultList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ConfirmationNumberDefaultList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ConfirmationNumberDefaultList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ConfirmationNumberDefaultList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.FromDate);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.FromDate);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<ConfirmationNumberDefaultList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ConfirmationNumberDefaultList GetSingle(string id)
        {
            IQueryable<ConfirmationNumberDefault> ConfirmationNumberDefaultQuery = (from a in context.ConfirmationNumberDefaults
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<ConfirmationNumberDefaultList> ConfirmationNumberDefaultListQuery = GetIqueryableList( ConfirmationNumberDefaultQuery);
            ConfirmationNumberDefaultList ConfirmationNumberDefaultList = ConfirmationNumberDefaultListQuery.FirstOrDefault();
            return ConfirmationNumberDefaultList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ConfirmationNumberDefault> iQueryable = (from a in context.ConfirmationNumberDefaults 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<ConfirmationNumberDefault>(nonListQueryOperation, iQueryable);



            IQueryable<ConfirmationNumberDefaultList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ConfirmationNumberDefaultList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<ConfirmationNumberDefaultList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 