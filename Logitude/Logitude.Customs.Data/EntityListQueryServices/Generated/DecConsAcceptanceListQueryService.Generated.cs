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

    public partial class DecConsAcceptanceListQueryService
    {
         private ICustomContext context;
        public DecConsAcceptanceListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<DecConsAcceptanceList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DecConsAcceptance> iQueryable = (from a in context.DecConsAcceptances
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DecConsAcceptance>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<DecConsAcceptanceList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<DecConsAcceptanceList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DecConsAcceptanceList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> DecConsAcceptanceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.DecConsAcceptance",tenant).ToList();

                ObjectField objectField = (from a in DecConsAcceptanceObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<DecConsAcceptanceList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DecConsAcceptanceList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DecConsAcceptanceList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DecConsAcceptanceList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DecConsAcceptanceList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DecConsAcceptanceList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<DecConsAcceptanceList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
							 			 query2 = query2.OrderBy(d => d.DeclarationId);
			                                 
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
			  			 query2 = query2.OrderBy(d => d.DeclarationId);
			                 
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<DecConsAcceptanceList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public DecConsAcceptanceList GetSingle(string declarationid, int consignmentnumber, int linenumber)
        {
            IQueryable<DecConsAcceptance> DecConsAcceptanceQuery = (from a in context.DecConsAcceptances
                                                       where a.DeclarationId == declarationid && a.ConsignmentNumber == consignmentnumber && a.LineNumber == linenumber
                                                       select a);

             
            IQueryable<DecConsAcceptanceList> DecConsAcceptanceListQuery = GetIqueryableList( DecConsAcceptanceQuery);
            DecConsAcceptanceList DecConsAcceptanceList = DecConsAcceptanceListQuery.FirstOrDefault();
            return DecConsAcceptanceList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DecConsAcceptance> iQueryable = (from a in context.DecConsAcceptances 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<DecConsAcceptance>(nonListQueryOperation, iQueryable);

            IQueryable<DecConsAcceptanceList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<DecConsAcceptanceList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 