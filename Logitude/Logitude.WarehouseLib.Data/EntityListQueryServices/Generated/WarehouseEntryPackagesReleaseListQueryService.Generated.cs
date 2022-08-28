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

    public partial class WarehouseEntryPackagesReleaseListQueryService
    {
         private IWarehouseContext context;
        public WarehouseEntryPackagesReleaseListQueryService(IWarehouseContext context)
        {
            this.context = context;
        }

        public List<WarehouseEntryPackagesReleaseList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<WarehouseEntryPackagesReleaseList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<WarehouseEntryPackagesRelease> iQueryable = (from a in context.WarehouseEntryPackagesReleases
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<WarehouseEntryPackagesRelease>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<WarehouseEntryPackagesReleaseList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<WarehouseEntryPackagesReleaseList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<WarehouseEntryPackagesReleaseList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(WarehouseEntryPackagesReleaseList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> WarehouseEntryPackagesReleaseObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("WarehouseEntryPackagesRelease",tenant).ToList();

                ObjectField objectField = (from a in WarehouseEntryPackagesReleaseObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<WarehouseEntryPackagesReleaseList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseEntryPackagesReleaseList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseEntryPackagesReleaseList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseEntryPackagesReleaseList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseEntryPackagesReleaseList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseEntryPackagesReleaseList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseEntryPackagesReleaseList, decimal>(queryOperations, query2);
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

         public List<WarehouseEntryPackagesReleaseList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public WarehouseEntryPackagesReleaseList GetSingle(string entrypackageid, string releasepackageid)
        {
            IQueryable<WarehouseEntryPackagesRelease> WarehouseEntryPackagesReleaseQuery = (from a in context.WarehouseEntryPackagesReleases
                                                       where a.EntryPackageId == entrypackageid && a.ReleasePackageId == releasepackageid
                                                       select a);

             
            IQueryable<WarehouseEntryPackagesReleaseList> WarehouseEntryPackagesReleaseListQuery = GetIqueryableList( WarehouseEntryPackagesReleaseQuery);
            WarehouseEntryPackagesReleaseList WarehouseEntryPackagesReleaseList = WarehouseEntryPackagesReleaseListQuery.FirstOrDefault();
            return WarehouseEntryPackagesReleaseList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<WarehouseEntryPackagesRelease> iQueryable = (from a in context.WarehouseEntryPackagesReleases 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<WarehouseEntryPackagesRelease>(nonListQueryOperation, iQueryable);



            IQueryable<WarehouseEntryPackagesReleaseList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<WarehouseEntryPackagesReleaseList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<WarehouseEntryPackagesReleaseList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 