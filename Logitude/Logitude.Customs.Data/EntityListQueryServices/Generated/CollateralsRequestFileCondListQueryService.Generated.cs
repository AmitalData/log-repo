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

    public partial class CollateralsRequestFileCondListQueryService
    {
         private ICustomContext context;
        public CollateralsRequestFileCondListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CollateralsRequestFileCondList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CollateralsRequestFileCond> iQueryable = (from a in context.CollateralsRequestFileConds
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CollateralsRequestFileCond>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CollateralsRequestFileCondList> query2 = GetIqueryableList(iQueryable);
					  query2 = filter.GetFilteredQuery<CollateralsRequestFileCondList>(listQueryOperation, query2);
		
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CollateralsRequestFileCondList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CollateralsRequestFileCondObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CollateralsRequestFileCond",tenant).ToList();

                ObjectField objectField = (from a in CollateralsRequestFileCondObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CollateralsRequestFileCondList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CollateralsRequestFileCondList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CollateralsRequestFileCondList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CollateralsRequestFileCondList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CollateralsRequestFileCondList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CollateralsRequestFileCondList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CollateralsRequestFileCondList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.CustomsCollateralId);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.CustomsCollateralId);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CollateralsRequestFileCondList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CollateralsRequestFileCondList GetSingle(string customscollateralid, string conditioncode, int linenumber)
        {
            IQueryable<CollateralsRequestFileCond> CollateralsRequestFileCondQuery = (from a in context.CollateralsRequestFileConds
                                                       where a.CustomsCollateralId == customscollateralid && a.ConditionCode == conditioncode && a.LineNumber == linenumber
                                                       select a);

             
            IQueryable<CollateralsRequestFileCondList> CollateralsRequestFileCondListQuery = GetIqueryableList( CollateralsRequestFileCondQuery);
            CollateralsRequestFileCondList CollateralsRequestFileCondList = CollateralsRequestFileCondListQuery.FirstOrDefault();
            return CollateralsRequestFileCondList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CollateralsRequestFileCond> iQueryable = (from a in context.CollateralsRequestFileConds 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CollateralsRequestFileCond>(nonListQueryOperation, iQueryable);

            IQueryable<CollateralsRequestFileCondList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<CollateralsRequestFileCondList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 