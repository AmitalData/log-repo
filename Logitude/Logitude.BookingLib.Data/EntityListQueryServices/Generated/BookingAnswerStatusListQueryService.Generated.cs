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

using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.EntityLists;

namespace Logitude.BookingLib.Data.EntityListQueryServices
{ 

    public partial class BookingAnswerStatusListQueryService
    {
         private IBookingContext context;
        public BookingAnswerStatusListQueryService(IBookingContext context)
        {
            this.context = context;
        }

        public List<BookingAnswerStatusList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<BookingAnswerStatusList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<BookingAnswerStatus> iQueryable = (from a in context.BookingAnswerStatus
                                               select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<BookingAnswerStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<BookingAnswerStatusList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<BookingAnswerStatusList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<BookingAnswerStatusList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BookingAnswerStatusList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> BookingAnswerStatusObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("BookingAnswerStatus",tenant).ToList();

                ObjectField objectField = (from a in BookingAnswerStatusObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<BookingAnswerStatusList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<BookingAnswerStatusList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<BookingAnswerStatusList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<BookingAnswerStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<BookingAnswerStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<BookingAnswerStatusList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<BookingAnswerStatusList, decimal>(queryOperations, query2);
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

         public List<BookingAnswerStatusList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public BookingAnswerStatusList GetSingle(string code)
        {
            IQueryable<BookingAnswerStatus> BookingAnswerStatusQuery = (from a in context.BookingAnswerStatus
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<BookingAnswerStatusList> BookingAnswerStatusListQuery = GetIqueryableList( BookingAnswerStatusQuery);
            BookingAnswerStatusList BookingAnswerStatusList = BookingAnswerStatusListQuery.FirstOrDefault();
            return BookingAnswerStatusList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations ){
		 		  return GetListCount(queryOperations, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<BookingAnswerStatus> iQueryable = (from a in context.BookingAnswerStatus  select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<BookingAnswerStatus>(nonListQueryOperation, iQueryable);



            IQueryable<BookingAnswerStatusList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<BookingAnswerStatusList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<BookingAnswerStatusList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 