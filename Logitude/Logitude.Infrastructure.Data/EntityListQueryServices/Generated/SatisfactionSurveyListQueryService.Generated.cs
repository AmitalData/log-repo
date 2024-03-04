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

    public partial class SatisfactionSurveyListQueryService
    {
         private IInfrastructureContext context;
        public SatisfactionSurveyListQueryService(IInfrastructureContext context)
        {
            this.context = context;
        }

        public List<SatisfactionSurveyList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<SatisfactionSurveyList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SatisfactionSurvey> iQueryable = (from a in context.SatisfactionSurveys
                                               select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<SatisfactionSurvey>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<SatisfactionSurveyList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<SatisfactionSurveyList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<SatisfactionSurveyList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SatisfactionSurveyList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> SatisfactionSurveyObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("SatisfactionSurvey",tenant).ToList();

                ObjectField objectField = (from a in SatisfactionSurveyObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<SatisfactionSurveyList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<SatisfactionSurveyList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<SatisfactionSurveyList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<SatisfactionSurveyList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<SatisfactionSurveyList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<SatisfactionSurveyList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<SatisfactionSurveyList, decimal>(queryOperations, query2);
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

         public List<SatisfactionSurveyList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public SatisfactionSurveyList GetSingle(string id)
        {
            IQueryable<SatisfactionSurvey> SatisfactionSurveyQuery = (from a in context.SatisfactionSurveys
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<SatisfactionSurveyList> SatisfactionSurveyListQuery = GetIqueryableList( SatisfactionSurveyQuery);
            SatisfactionSurveyList SatisfactionSurveyList = SatisfactionSurveyListQuery.FirstOrDefault();
            return SatisfactionSurveyList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations ){
		 		  return GetListCount(queryOperations, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SatisfactionSurvey> iQueryable = (from a in context.SatisfactionSurveys  select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<SatisfactionSurvey>(nonListQueryOperation, iQueryable);



            IQueryable<SatisfactionSurveyList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<SatisfactionSurveyList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<SatisfactionSurveyList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 