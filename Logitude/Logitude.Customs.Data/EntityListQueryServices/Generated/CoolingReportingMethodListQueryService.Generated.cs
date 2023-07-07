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

    public partial class CoolingReportingMethodListQueryService
    {
         private ICustomContext context;
        public CoolingReportingMethodListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CoolingReportingMethodList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CoolingReportingMethod> iQueryable = (from a in context.CoolingReportingMethods
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CoolingReportingMethod>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CoolingReportingMethodList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CoolingReportingMethodList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CoolingReportingMethodList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CoolingReportingMethodObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CoolingReportingMethod",tenant).ToList();

                ObjectField objectField = (from a in CoolingReportingMethodObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CoolingReportingMethodList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CoolingReportingMethodList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CoolingReportingMethodList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CoolingReportingMethodList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CoolingReportingMethodList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CoolingReportingMethodList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CoolingReportingMethodList, decimal>(queryOperations, query2);
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

         public List<CoolingReportingMethodList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CoolingReportingMethodList GetSingle(string code)
        {
            IQueryable<CoolingReportingMethod> CoolingReportingMethodQuery = (from a in context.CoolingReportingMethods
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<CoolingReportingMethodList> CoolingReportingMethodListQuery = GetIqueryableList( CoolingReportingMethodQuery);
            CoolingReportingMethodList CoolingReportingMethodList = CoolingReportingMethodListQuery.FirstOrDefault();
            return CoolingReportingMethodList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CoolingReportingMethod> iQueryable = (from a in context.CoolingReportingMethods  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CoolingReportingMethod>(nonListQueryOperation, iQueryable);

            IQueryable<CoolingReportingMethodList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CoolingReportingMethodList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 