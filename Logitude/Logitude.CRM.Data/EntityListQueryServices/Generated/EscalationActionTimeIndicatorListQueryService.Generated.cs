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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class EscalationActionTimeIndicatorListQueryService
    {
         private ICRMContext context;
        public EscalationActionTimeIndicatorListQueryService(ICRMContext context)
        {
            this.context = context;
        }

        public List<EscalationActionTimeIndicatorList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<EscalationActionTimeIndicatorList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<EscalationActionTimeIndicator> iQueryable = (from a in context.EscalationActionTimeIndicators
                                               select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<EscalationActionTimeIndicator>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<EscalationActionTimeIndicatorList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<EscalationActionTimeIndicatorList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<EscalationActionTimeIndicatorList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(EscalationActionTimeIndicatorList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> EscalationActionTimeIndicatorObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("EscalationActionTimeIndicator",tenant).ToList();

                ObjectField objectField = (from a in EscalationActionTimeIndicatorObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<EscalationActionTimeIndicatorList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<EscalationActionTimeIndicatorList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<EscalationActionTimeIndicatorList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<EscalationActionTimeIndicatorList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<EscalationActionTimeIndicatorList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<EscalationActionTimeIndicatorList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<EscalationActionTimeIndicatorList, decimal>(queryOperations, query2);
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

         public List<EscalationActionTimeIndicatorList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public EscalationActionTimeIndicatorList GetSingle(string code)
        {
            IQueryable<EscalationActionTimeIndicator> EscalationActionTimeIndicatorQuery = (from a in context.EscalationActionTimeIndicators
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<EscalationActionTimeIndicatorList> EscalationActionTimeIndicatorListQuery = GetIqueryableList( EscalationActionTimeIndicatorQuery);
            EscalationActionTimeIndicatorList EscalationActionTimeIndicatorList = EscalationActionTimeIndicatorListQuery.FirstOrDefault();
            return EscalationActionTimeIndicatorList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations ){
		 		  return GetListCount(queryOperations, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<EscalationActionTimeIndicator> iQueryable = (from a in context.EscalationActionTimeIndicators  select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<EscalationActionTimeIndicator>(nonListQueryOperation, iQueryable);



            IQueryable<EscalationActionTimeIndicatorList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<EscalationActionTimeIndicatorList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<EscalationActionTimeIndicatorList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 