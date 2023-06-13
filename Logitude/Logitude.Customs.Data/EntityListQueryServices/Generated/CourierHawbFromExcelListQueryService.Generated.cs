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

    public partial class CourierHawbFromExcelListQueryService
    {
         private ICustomContext context;
        public CourierHawbFromExcelListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CourierHawbFromExcelList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CourierHawbFromExcel> iQueryable = (from a in context.CourierHawbFromExcels
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CourierHawbFromExcel>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CourierHawbFromExcelList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CourierHawbFromExcelList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CourierHawbFromExcelList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CourierHawbFromExcelObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CourierHawbFromExcel",tenant).ToList();

                ObjectField objectField = (from a in CourierHawbFromExcelObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CourierHawbFromExcelList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CourierHawbFromExcelList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CourierHawbFromExcelList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CourierHawbFromExcelList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CourierHawbFromExcelList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CourierHawbFromExcelList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CourierHawbFromExcelList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.DeclarationId);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.DeclarationId);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CourierHawbFromExcelList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CourierHawbFromExcelList GetSingle(string declarationid)
        {
            IQueryable<CourierHawbFromExcel> CourierHawbFromExcelQuery = (from a in context.CourierHawbFromExcels
                                                       where a.DeclarationId == declarationid
                                                       select a);

             
            IQueryable<CourierHawbFromExcelList> CourierHawbFromExcelListQuery = GetIqueryableList( CourierHawbFromExcelQuery);
            CourierHawbFromExcelList CourierHawbFromExcelList = CourierHawbFromExcelListQuery.FirstOrDefault();
            return CourierHawbFromExcelList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CourierHawbFromExcel> iQueryable = (from a in context.CourierHawbFromExcels 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CourierHawbFromExcel>(nonListQueryOperation, iQueryable);

            IQueryable<CourierHawbFromExcelList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CourierHawbFromExcelList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 