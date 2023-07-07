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

    public partial class ClaimImporterDeclarsPage3BListQueryService
    {
         private ICustomContext context;
        public ClaimImporterDeclarsPage3BListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<ClaimImporterDeclarsPage3BList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ClaimImporterDeclarsPage3B> iQueryable = (from a in context.ClaimImporterDeclarsPage3Bs
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ClaimImporterDeclarsPage3B>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ClaimImporterDeclarsPage3BList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ClaimImporterDeclarsPage3BList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ClaimImporterDeclarsPage3BList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ClaimImporterDeclarsPage3BObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.ClaimImporterDeclarsPage3B",tenant).ToList();

                ObjectField objectField = (from a in ClaimImporterDeclarsPage3BObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsPage3BList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsPage3BList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsPage3BList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsPage3BList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsPage3BList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsPage3BList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ClaimImporterDeclarsPage3BList, decimal>(queryOperations, query2);
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

         public List<ClaimImporterDeclarsPage3BList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ClaimImporterDeclarsPage3BList GetSingle(string claimid, int lineno)
        {
            IQueryable<ClaimImporterDeclarsPage3B> ClaimImporterDeclarsPage3BQuery = (from a in context.ClaimImporterDeclarsPage3Bs
                                                       where a.ClaimId == claimid && a.LineNo == lineno
                                                       select a);

             
            IQueryable<ClaimImporterDeclarsPage3BList> ClaimImporterDeclarsPage3BListQuery = GetIqueryableList( ClaimImporterDeclarsPage3BQuery);
            ClaimImporterDeclarsPage3BList ClaimImporterDeclarsPage3BList = ClaimImporterDeclarsPage3BListQuery.FirstOrDefault();
            return ClaimImporterDeclarsPage3BList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ClaimImporterDeclarsPage3B> iQueryable = (from a in context.ClaimImporterDeclarsPage3Bs 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ClaimImporterDeclarsPage3B>(nonListQueryOperation, iQueryable);

            IQueryable<ClaimImporterDeclarsPage3BList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ClaimImporterDeclarsPage3BList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 