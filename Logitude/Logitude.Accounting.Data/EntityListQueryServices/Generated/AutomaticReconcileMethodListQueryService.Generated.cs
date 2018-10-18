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

    public partial class AutomaticReconcileMethodListQueryService
    {
         private IAccountingContext context;
        public AutomaticReconcileMethodListQueryService(IAccountingContext context)
        {
            this.context = context;
        }

        public List<AutomaticReconcileMethodList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AutomaticReconcileMethod> iQueryable = (from a in context.AutomaticReconcileMethods
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AutomaticReconcileMethod>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AutomaticReconcileMethodList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<AutomaticReconcileMethodList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AutomaticReconcileMethodList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> AutomaticReconcileMethodObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AutomaticReconcileMethod",tenant).ToList();

                ObjectField objectField = (from a in AutomaticReconcileMethodObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<AutomaticReconcileMethodList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AutomaticReconcileMethodList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AutomaticReconcileMethodList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AutomaticReconcileMethodList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AutomaticReconcileMethodList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AutomaticReconcileMethodList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<AutomaticReconcileMethodList, decimal>(queryOperations, query2);
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

         public List<AutomaticReconcileMethodList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public AutomaticReconcileMethodList GetSingle(string id)
        {
            IQueryable<AutomaticReconcileMethod> AutomaticReconcileMethodQuery = (from a in context.AutomaticReconcileMethods
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<AutomaticReconcileMethodList> AutomaticReconcileMethodListQuery = GetIqueryableList( AutomaticReconcileMethodQuery);
            AutomaticReconcileMethodList AutomaticReconcileMethodList = AutomaticReconcileMethodListQuery.FirstOrDefault();
            return AutomaticReconcileMethodList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AutomaticReconcileMethod> iQueryable = (from a in context.AutomaticReconcileMethods 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<AutomaticReconcileMethod>(nonListQueryOperation, iQueryable);

            IQueryable<AutomaticReconcileMethodList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<AutomaticReconcileMethodList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 