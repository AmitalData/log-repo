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

    public partial class RegularityRequirementWarningListQueryService
    {
         private ICustomContext context;
        public RegularityRequirementWarningListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<RegularityRequirementWarningList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<RegularityRequirementWarningList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<RegularityRequirementWarning> iQueryable = (from a in context.RegularityRequirementWarnings
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<RegularityRequirementWarning>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<RegularityRequirementWarningList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<RegularityRequirementWarningList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<RegularityRequirementWarningList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(RegularityRequirementWarningList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> RegularityRequirementWarningObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.RegularityRequirementWarning",tenant).ToList();

                ObjectField objectField = (from a in RegularityRequirementWarningObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<RegularityRequirementWarningList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<RegularityRequirementWarningList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<RegularityRequirementWarningList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<RegularityRequirementWarningList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<RegularityRequirementWarningList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<RegularityRequirementWarningList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<RegularityRequirementWarningList, decimal>(queryOperations, query2);
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

         public List<RegularityRequirementWarningList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public RegularityRequirementWarningList GetSingle(string code)
        {
            IQueryable<RegularityRequirementWarning> RegularityRequirementWarningQuery = (from a in context.RegularityRequirementWarnings
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<RegularityRequirementWarningList> RegularityRequirementWarningListQuery = GetIqueryableList( RegularityRequirementWarningQuery);
            RegularityRequirementWarningList RegularityRequirementWarningList = RegularityRequirementWarningListQuery.FirstOrDefault();
            return RegularityRequirementWarningList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations ){
		 		  return GetListCount(queryOperations, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<RegularityRequirementWarning> iQueryable = (from a in context.RegularityRequirementWarnings  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<RegularityRequirementWarning>(nonListQueryOperation, iQueryable);



            IQueryable<RegularityRequirementWarningList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<RegularityRequirementWarningList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<RegularityRequirementWarningList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 