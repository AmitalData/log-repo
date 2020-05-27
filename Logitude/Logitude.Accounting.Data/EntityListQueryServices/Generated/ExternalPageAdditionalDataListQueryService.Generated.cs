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

    public partial class ExternalPageAdditionalDataListQueryService
    {
         private IAccountingContext context;
        public ExternalPageAdditionalDataListQueryService(IAccountingContext context)
        {
            this.context = context;
        }

        public List<ExternalPageAdditionalDataList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ExternalPageAdditionalData> iQueryable = (from a in context.ExternalPageAdditionalDatas
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ExternalPageAdditionalData>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ExternalPageAdditionalDataList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ExternalPageAdditionalDataList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ExternalPageAdditionalDataList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ExternalPageAdditionalDataObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ExternalPageAdditionalData",tenant).ToList();

                ObjectField objectField = (from a in ExternalPageAdditionalDataObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ExternalPageAdditionalDataList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalPageAdditionalDataList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalPageAdditionalDataList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalPageAdditionalDataList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalPageAdditionalDataList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalPageAdditionalDataList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalPageAdditionalDataList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.ObjectTableId);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.ObjectTableId);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<ExternalPageAdditionalDataList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ExternalPageAdditionalDataList GetSingle(string objecttableid, string entityid)
        {
            IQueryable<ExternalPageAdditionalData> ExternalPageAdditionalDataQuery = (from a in context.ExternalPageAdditionalDatas
                                                       where a.ObjectTableId == objecttableid && a.EntityId == entityid
                                                       select a);

             
            IQueryable<ExternalPageAdditionalDataList> ExternalPageAdditionalDataListQuery = GetIqueryableList( ExternalPageAdditionalDataQuery);
            ExternalPageAdditionalDataList ExternalPageAdditionalDataList = ExternalPageAdditionalDataListQuery.FirstOrDefault();
            return ExternalPageAdditionalDataList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ExternalPageAdditionalData> iQueryable = (from a in context.ExternalPageAdditionalDatas 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ExternalPageAdditionalData>(nonListQueryOperation, iQueryable);

            IQueryable<ExternalPageAdditionalDataList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ExternalPageAdditionalDataList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 