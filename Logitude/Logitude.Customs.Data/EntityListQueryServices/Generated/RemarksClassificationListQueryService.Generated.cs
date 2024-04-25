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

    public partial class RemarksClassificationListQueryService
    {
         private ICustomContext context;
        public RemarksClassificationListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<RemarksClassificationList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<RemarksClassification> iQueryable = (from a in context.RemarksClassifications
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<RemarksClassification>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<RemarksClassificationList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<RemarksClassificationList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(RemarksClassificationList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> RemarksClassificationObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.RemarksClassification",tenant).ToList();

                ObjectField objectField = (from a in RemarksClassificationObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<RemarksClassificationList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<RemarksClassificationList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<RemarksClassificationList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<RemarksClassificationList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<RemarksClassificationList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<RemarksClassificationList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<RemarksClassificationList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<RemarksClassificationList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public RemarksClassificationList GetSingle(string id)
        {
            IQueryable<RemarksClassification> RemarksClassificationQuery = (from a in context.RemarksClassifications
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<RemarksClassificationList> RemarksClassificationListQuery = GetIqueryableList( RemarksClassificationQuery);
            RemarksClassificationList RemarksClassificationList = RemarksClassificationListQuery.FirstOrDefault();
            return RemarksClassificationList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<RemarksClassification> iQueryable = (from a in context.RemarksClassifications 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<RemarksClassification>(nonListQueryOperation, iQueryable);

            IQueryable<RemarksClassificationList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<RemarksClassificationList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 