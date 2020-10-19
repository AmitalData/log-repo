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

    public partial class CalculatedChartsLineTypeListQueryService
    {
         private IAccountingContext context;
        public CalculatedChartsLineTypeListQueryService(IAccountingContext context)
        {
            this.context = context;
        }

        public List<CalculatedChartsLineTypeList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CalculatedChartsLineType> iQueryable = (from a in context.CalculatedChartsLineTypes
                                               select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CalculatedChartsLineType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CalculatedChartsLineTypeList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CalculatedChartsLineTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CalculatedChartsLineTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CalculatedChartsLineTypeObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CalculatedChartsLineType",tenant).ToList();

                ObjectField objectField = (from a in CalculatedChartsLineTypeObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CalculatedChartsLineTypeList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsLineTypeList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsLineTypeList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsLineTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsLineTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsLineTypeList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CalculatedChartsLineTypeList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CalculatedChartsLineTypeList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CalculatedChartsLineTypeList GetSingle(string code)
        {
            IQueryable<CalculatedChartsLineType> CalculatedChartsLineTypeQuery = (from a in context.CalculatedChartsLineTypes
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<CalculatedChartsLineTypeList> CalculatedChartsLineTypeListQuery = GetIqueryableList( CalculatedChartsLineTypeQuery);
            CalculatedChartsLineTypeList CalculatedChartsLineTypeList = CalculatedChartsLineTypeListQuery.FirstOrDefault();
            return CalculatedChartsLineTypeList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CalculatedChartsLineType> iQueryable = (from a in context.CalculatedChartsLineTypes  select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CalculatedChartsLineType>(nonListQueryOperation, iQueryable);

            IQueryable<CalculatedChartsLineTypeList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CalculatedChartsLineTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 