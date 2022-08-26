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

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{ 

    public partial class BIReportFolderListQueryService
    {
         private IInfrastructureContext context;
        public BIReportFolderListQueryService(IInfrastructureContext context)
        {
            this.context = context;
        }

        public List<BIReportFolderList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<BIReportFolderList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<BIReportFolder> iQueryable = (from a in context.BIReportFolders
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<BIReportFolder>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<BIReportFolderList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<BIReportFolderList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<BIReportFolderList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BIReportFolderList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> BIReportFolderObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("BIReportFolder",tenant).ToList();

                ObjectField objectField = (from a in BIReportFolderObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<BIReportFolderList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<BIReportFolderList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<BIReportFolderList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<BIReportFolderList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<BIReportFolderList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<BIReportFolderList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<BIReportFolderList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Name);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Name);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<BIReportFolderList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public BIReportFolderList GetSingle(string id)
        {
            IQueryable<BIReportFolder> BIReportFolderQuery = (from a in context.BIReportFolders
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<BIReportFolderList> BIReportFolderListQuery = GetIqueryableList( BIReportFolderQuery);
            BIReportFolderList BIReportFolderList = BIReportFolderListQuery.FirstOrDefault();
            return BIReportFolderList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<BIReportFolder> iQueryable = (from a in context.BIReportFolders 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<BIReportFolder>(nonListQueryOperation, iQueryable);



            IQueryable<BIReportFolderList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<BIReportFolderList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<BIReportFolderList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 