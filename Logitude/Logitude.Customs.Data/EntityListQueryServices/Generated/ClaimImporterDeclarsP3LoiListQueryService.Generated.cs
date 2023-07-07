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

    public partial class ClaimImporterDeclarsP3LoiListQueryService
    {
         private ICustomContext context;
        public ClaimImporterDeclarsP3LoiListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<ClaimImporterDeclarsP3LoiList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ClaimImporterDeclarsP3Loi> iQueryable = (from a in context.ClaimImporterDeclarsP3Lois
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ClaimImporterDeclarsP3Loi>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ClaimImporterDeclarsP3LoiList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ClaimImporterDeclarsP3LoiList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ClaimImporterDeclarsP3LoiList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ClaimImporterDeclarsP3LoiObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.ClaimImporterDeclarsP3Loi",tenant).ToList();

                ObjectField objectField = (from a in ClaimImporterDeclarsP3LoiObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsP3LoiList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsP3LoiList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsP3LoiList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsP3LoiList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsP3LoiList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsP3LoiList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsP3LoiList, decimal>(queryOperations, query2);
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

         public List<ClaimImporterDeclarsP3LoiList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ClaimImporterDeclarsP3LoiList GetSingle(string claimid, int counterkey, int lineno)
        {
            IQueryable<ClaimImporterDeclarsP3Loi> ClaimImporterDeclarsP3LoiQuery = (from a in context.ClaimImporterDeclarsP3Lois
                                                       where a.ClaimId == claimid && a.CounterKey == counterkey && a.LineNo == lineno
                                                       select a);

             
            IQueryable<ClaimImporterDeclarsP3LoiList> ClaimImporterDeclarsP3LoiListQuery = GetIqueryableList( ClaimImporterDeclarsP3LoiQuery);
            ClaimImporterDeclarsP3LoiList ClaimImporterDeclarsP3LoiList = ClaimImporterDeclarsP3LoiListQuery.FirstOrDefault();
            return ClaimImporterDeclarsP3LoiList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ClaimImporterDeclarsP3Loi> iQueryable = (from a in context.ClaimImporterDeclarsP3Lois 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ClaimImporterDeclarsP3Loi>(nonListQueryOperation, iQueryable);

            IQueryable<ClaimImporterDeclarsP3LoiList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ClaimImporterDeclarsP3LoiList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 