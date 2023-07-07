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

    public partial class CustomsDocumentsDefinitionListQueryService
    {
         private ICustomContext context;
        public CustomsDocumentsDefinitionListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CustomsDocumentsDefinitionList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomsDocumentsDefinition> iQueryable = (from a in context.CustomsDocumentsDefinitions
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomsDocumentsDefinition>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomsDocumentsDefinitionList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CustomsDocumentsDefinitionList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomsDocumentsDefinitionList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CustomsDocumentsDefinitionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CustomsDocumentsDefinition",tenant).ToList();

                ObjectField objectField = (from a in CustomsDocumentsDefinitionObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CustomsDocumentsDefinitionList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsDocumentsDefinitionList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsDocumentsDefinitionList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsDocumentsDefinitionList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsDocumentsDefinitionList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsDocumentsDefinitionList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CustomsDocumentsDefinitionList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.DocumentTypeCode);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.DocumentTypeCode);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CustomsDocumentsDefinitionList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CustomsDocumentsDefinitionList GetSingle(string id)
        {
            IQueryable<CustomsDocumentsDefinition> CustomsDocumentsDefinitionQuery = (from a in context.CustomsDocumentsDefinitions
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<CustomsDocumentsDefinitionList> CustomsDocumentsDefinitionListQuery = GetIqueryableList( CustomsDocumentsDefinitionQuery);
            CustomsDocumentsDefinitionList CustomsDocumentsDefinitionList = CustomsDocumentsDefinitionListQuery.FirstOrDefault();
            return CustomsDocumentsDefinitionList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomsDocumentsDefinition> iQueryable = (from a in context.CustomsDocumentsDefinitions 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CustomsDocumentsDefinition>(nonListQueryOperation, iQueryable);

            IQueryable<CustomsDocumentsDefinitionList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CustomsDocumentsDefinitionList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 