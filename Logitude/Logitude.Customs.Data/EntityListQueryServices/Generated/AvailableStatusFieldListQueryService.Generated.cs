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

    public partial class AvailableStatusFieldListQueryService
    {
         private ICustomContext context;
        public AvailableStatusFieldListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<AvailableStatusFieldList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AvailableStatusField> iQueryable = (from a in context.AvailableStatusFields
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AvailableStatusField>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AvailableStatusFieldList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<AvailableStatusFieldList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AvailableStatusFieldList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> AvailableStatusFieldObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.AvailableStatusField",tenant).ToList();

                ObjectField objectField = (from a in AvailableStatusFieldObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<AvailableStatusFieldList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AvailableStatusFieldList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AvailableStatusFieldList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AvailableStatusFieldList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AvailableStatusFieldList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AvailableStatusFieldList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<AvailableStatusFieldList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.FieldCode);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.FieldCode);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<AvailableStatusFieldList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public AvailableStatusFieldList GetSingle(string fieldcode)
        {
            IQueryable<AvailableStatusField> AvailableStatusFieldQuery = (from a in context.AvailableStatusFields
                                                       where a.FieldCode == fieldcode
                                                       select a);

             
            IQueryable<AvailableStatusFieldList> AvailableStatusFieldListQuery = GetIqueryableList( AvailableStatusFieldQuery);
            AvailableStatusFieldList AvailableStatusFieldList = AvailableStatusFieldListQuery.FirstOrDefault();
            return AvailableStatusFieldList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AvailableStatusField> iQueryable = (from a in context.AvailableStatusFields 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<AvailableStatusField>(nonListQueryOperation, iQueryable);

            IQueryable<AvailableStatusFieldList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<AvailableStatusFieldList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 