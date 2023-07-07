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

    public partial class CustomsAutonomyKeywordListQueryService
    {
         private ICustomContext context;
        public CustomsAutonomyKeywordListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CustomsAutonomyKeywordList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomsAutonomyKeyword> iQueryable = (from a in context.CustomsAutonomyKeywords
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomsAutonomyKeyword>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomsAutonomyKeywordList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CustomsAutonomyKeywordList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomsAutonomyKeywordList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CustomsAutonomyKeywordObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CustomsAutonomyKeyword",tenant).ToList();

                ObjectField objectField = (from a in CustomsAutonomyKeywordObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CustomsAutonomyKeywordList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsAutonomyKeywordList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsAutonomyKeywordList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsAutonomyKeywordList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsAutonomyKeywordList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsAutonomyKeywordList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsAutonomyKeywordList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Id);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Id);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CustomsAutonomyKeywordList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CustomsAutonomyKeywordList GetSingle(string id)
        {
            IQueryable<CustomsAutonomyKeyword> CustomsAutonomyKeywordQuery = (from a in context.CustomsAutonomyKeywords
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<CustomsAutonomyKeywordList> CustomsAutonomyKeywordListQuery = GetIqueryableList( CustomsAutonomyKeywordQuery);
            CustomsAutonomyKeywordList CustomsAutonomyKeywordList = CustomsAutonomyKeywordListQuery.FirstOrDefault();
            return CustomsAutonomyKeywordList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomsAutonomyKeyword> iQueryable = (from a in context.CustomsAutonomyKeywords 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CustomsAutonomyKeyword>(nonListQueryOperation, iQueryable);

            IQueryable<CustomsAutonomyKeywordList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CustomsAutonomyKeywordList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 