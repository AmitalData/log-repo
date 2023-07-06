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

    public partial class InterfaceTenantDefinitionListQueryService
    {
         private ICustomContext context;
        public InterfaceTenantDefinitionListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<InterfaceTenantDefinitionList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<InterfaceTenantDefinition> iQueryable = (from a in context.InterfaceTenantDefinitions
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<InterfaceTenantDefinition>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<InterfaceTenantDefinitionList> query2 = GetIqueryableList(iQueryable);
<<<<<<< HEAD
					  query2 = filter.GetFilteredQuery<InterfaceTenantDefinitionList>(listQueryOperation, query2);
		
=======
           
            query2 = filter.GetFilteredQuery<InterfaceTenantDefinitionList>(listQueryOperation, query2);

>>>>>>> parent of 094118ae335 (#181601)
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(InterfaceTenantDefinitionList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> InterfaceTenantDefinitionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.InterfaceTenantDefinition",tenant).ToList();

                ObjectField objectField = (from a in InterfaceTenantDefinitionObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<InterfaceTenantDefinitionList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<InterfaceTenantDefinitionList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<InterfaceTenantDefinitionList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<InterfaceTenantDefinitionList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<InterfaceTenantDefinitionList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<InterfaceTenantDefinitionList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<InterfaceTenantDefinitionList, decimal>(queryOperations, query2);
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

         public List<InterfaceTenantDefinitionList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public InterfaceTenantDefinitionList GetSingle(string id)
        {
            IQueryable<InterfaceTenantDefinition> InterfaceTenantDefinitionQuery = (from a in context.InterfaceTenantDefinitions
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<InterfaceTenantDefinitionList> InterfaceTenantDefinitionListQuery = GetIqueryableList( InterfaceTenantDefinitionQuery);
            InterfaceTenantDefinitionList InterfaceTenantDefinitionList = InterfaceTenantDefinitionListQuery.FirstOrDefault();
            return InterfaceTenantDefinitionList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<InterfaceTenantDefinition> iQueryable = (from a in context.InterfaceTenantDefinitions 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<InterfaceTenantDefinition>(nonListQueryOperation, iQueryable);

            IQueryable<InterfaceTenantDefinitionList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<InterfaceTenantDefinitionList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 