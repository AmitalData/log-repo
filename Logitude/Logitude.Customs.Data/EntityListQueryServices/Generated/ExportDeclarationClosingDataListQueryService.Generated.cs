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

    public partial class ExportDeclarationClosingDataListQueryService
    {
         private ICustomContext context;
        public ExportDeclarationClosingDataListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<ExportDeclarationClosingDataList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ExportDeclarationClosingData> iQueryable = (from a in context.ExportDeclarationClosingDatas
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ExportDeclarationClosingData>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ExportDeclarationClosingDataList> query2 = GetIqueryableList(iQueryable);
<<<<<<< HEAD
					  query2 = filter.GetFilteredQuery<ExportDeclarationClosingDataList>(listQueryOperation, query2);
		
=======
           
            query2 = filter.GetFilteredQuery<ExportDeclarationClosingDataList>(listQueryOperation, query2);

>>>>>>> parent of 094118ae335 (#181601)
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ExportDeclarationClosingDataList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ExportDeclarationClosingDataObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.ExportDeclarationClosingData",tenant).ToList();

                ObjectField objectField = (from a in ExportDeclarationClosingDataObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ExportDeclarationClosingDataList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ExportDeclarationClosingDataList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ExportDeclarationClosingDataList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ExportDeclarationClosingDataList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ExportDeclarationClosingDataList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ExportDeclarationClosingDataList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ExportDeclarationClosingDataList, decimal>(queryOperations, query2);
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

         public List<ExportDeclarationClosingDataList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ExportDeclarationClosingDataList GetSingle(string declarationid)
        {
            IQueryable<ExportDeclarationClosingData> ExportDeclarationClosingDataQuery = (from a in context.ExportDeclarationClosingDatas
                                                       where a.DeclarationId == declarationid
                                                       select a);

             
            IQueryable<ExportDeclarationClosingDataList> ExportDeclarationClosingDataListQuery = GetIqueryableList( ExportDeclarationClosingDataQuery);
            ExportDeclarationClosingDataList ExportDeclarationClosingDataList = ExportDeclarationClosingDataListQuery.FirstOrDefault();
            return ExportDeclarationClosingDataList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ExportDeclarationClosingData> iQueryable = (from a in context.ExportDeclarationClosingDatas 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ExportDeclarationClosingData>(nonListQueryOperation, iQueryable);

            IQueryable<ExportDeclarationClosingDataList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<ExportDeclarationClosingDataList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 