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

    public partial class CustomDocumentTypeMetaDataListQueryService
    {
         private ICustomContext context;
        public CustomDocumentTypeMetaDataListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CustomDocumentTypeMetaDataList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomDocumentTypeMetaData> iQueryable = (from a in context.CustomDocumentTypeMetaData
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomDocumentTypeMetaData>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomDocumentTypeMetaDataList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CustomDocumentTypeMetaDataList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomDocumentTypeMetaDataList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CustomDocumentTypeMetaDataObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CustomDocumentTypeMetaData",tenant).ToList();

                ObjectField objectField = (from a in CustomDocumentTypeMetaDataObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CustomDocumentTypeMetaDataList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomDocumentTypeMetaDataList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomDocumentTypeMetaDataList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomDocumentTypeMetaDataList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomDocumentTypeMetaDataList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomDocumentTypeMetaDataList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CustomDocumentTypeMetaDataList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Format);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Format);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CustomDocumentTypeMetaDataList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CustomDocumentTypeMetaDataList GetSingle(string metadatatypecode, string documenttypecode)
        {
            IQueryable<CustomDocumentTypeMetaData> CustomDocumentTypeMetaDataQuery = (from a in context.CustomDocumentTypeMetaData
                                                       where a.MetaDataTypeCode == metadatatypecode && a.DocumentTypeCode == documenttypecode
                                                       select a);

             
            IQueryable<CustomDocumentTypeMetaDataList> CustomDocumentTypeMetaDataListQuery = GetIqueryableList( CustomDocumentTypeMetaDataQuery);
            CustomDocumentTypeMetaDataList CustomDocumentTypeMetaDataList = CustomDocumentTypeMetaDataListQuery.FirstOrDefault();
            return CustomDocumentTypeMetaDataList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomDocumentTypeMetaData> iQueryable = (from a in context.CustomDocumentTypeMetaData  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CustomDocumentTypeMetaData>(nonListQueryOperation, iQueryable);

            IQueryable<CustomDocumentTypeMetaDataList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CustomDocumentTypeMetaDataList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 