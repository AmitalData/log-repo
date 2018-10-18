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

    public partial class ClaimsRelatedEntsExpDeclarListQueryService
    {
         private ICustomContext context;
        public ClaimsRelatedEntsExpDeclarListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<ClaimsRelatedEntsExpDeclarList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ClaimsRelatedEntsExpDeclar> iQueryable = (from a in context.ClaimsRelatedEntsExpDeclars
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ClaimsRelatedEntsExpDeclar>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ClaimsRelatedEntsExpDeclarList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ClaimsRelatedEntsExpDeclarList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ClaimsRelatedEntsExpDeclarList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ClaimsRelatedEntsExpDeclarObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.ClaimsRelatedEntsExpDeclar",tenant).ToList();

                ObjectField objectField = (from a in ClaimsRelatedEntsExpDeclarObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ClaimsRelatedEntsExpDeclarList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntsExpDeclarList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntsExpDeclarList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntsExpDeclarList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntsExpDeclarList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntsExpDeclarList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimsRelatedEntsExpDeclarList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.ClaimId);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.ClaimId);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<ClaimsRelatedEntsExpDeclarList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ClaimsRelatedEntsExpDeclarList GetSingle(string claimid, int counterkey, string exportdeclarationnumber)
        {
            IQueryable<ClaimsRelatedEntsExpDeclar> ClaimsRelatedEntsExpDeclarQuery = (from a in context.ClaimsRelatedEntsExpDeclars
                                                       where a.ClaimId == claimid && a.CounterKey == counterkey && a.ExportDeclarationNumber == exportdeclarationnumber
                                                       select a);

             
            IQueryable<ClaimsRelatedEntsExpDeclarList> ClaimsRelatedEntsExpDeclarListQuery = GetIqueryableList( ClaimsRelatedEntsExpDeclarQuery);
            ClaimsRelatedEntsExpDeclarList ClaimsRelatedEntsExpDeclarList = ClaimsRelatedEntsExpDeclarListQuery.FirstOrDefault();
            return ClaimsRelatedEntsExpDeclarList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ClaimsRelatedEntsExpDeclar> iQueryable = (from a in context.ClaimsRelatedEntsExpDeclars 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ClaimsRelatedEntsExpDeclar>(nonListQueryOperation, iQueryable);

            IQueryable<ClaimsRelatedEntsExpDeclarList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ClaimsRelatedEntsExpDeclarList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 