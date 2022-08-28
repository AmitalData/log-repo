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

using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.EntityLists;

namespace Logitude.WarehouseLib.Data.EntityListQueryServices
{ 

    public partial class WarehouseEntryPackageListQueryService
    {
         private IWarehouseContext context;
        public WarehouseEntryPackageListQueryService(IWarehouseContext context)
        {
            this.context = context;
        }

        public List<WarehouseEntryPackageList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<WarehouseEntryPackageList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<WarehouseEntryPackage> iQueryable = (from a in context.WarehouseEntryPackages
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<WarehouseEntryPackage>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<WarehouseEntryPackageList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<WarehouseEntryPackageList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<WarehouseEntryPackageList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(WarehouseEntryPackageList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> WarehouseEntryPackageObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("WarehouseEntryPackage",tenant).ToList();

                ObjectField objectField = (from a in WarehouseEntryPackageObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<WarehouseEntryPackageList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseEntryPackageList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseEntryPackageList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseEntryPackageList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseEntryPackageList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseEntryPackageList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseEntryPackageList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.CreateDate);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.CreateDate);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<WarehouseEntryPackageList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public WarehouseEntryPackageList GetSingle(string id)
        {
            IQueryable<WarehouseEntryPackage> WarehouseEntryPackageQuery = (from a in context.WarehouseEntryPackages
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<WarehouseEntryPackageList> WarehouseEntryPackageListQuery = GetIqueryableList( WarehouseEntryPackageQuery);
            WarehouseEntryPackageList WarehouseEntryPackageList = WarehouseEntryPackageListQuery.FirstOrDefault();
            return WarehouseEntryPackageList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<WarehouseEntryPackage> iQueryable = (from a in context.WarehouseEntryPackages 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<WarehouseEntryPackage>(nonListQueryOperation, iQueryable);



            IQueryable<WarehouseEntryPackageList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<WarehouseEntryPackageList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<WarehouseEntryPackageList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 