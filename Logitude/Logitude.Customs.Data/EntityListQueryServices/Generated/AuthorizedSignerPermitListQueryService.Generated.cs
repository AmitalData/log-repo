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

    public partial class AuthorizedSignerPermitListQueryService
    {
         private ICustomContext context;
        public AuthorizedSignerPermitListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<AuthorizedSignerPermitList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AuthorizedSignerPermit> iQueryable = (from a in context.AuthorizedSignerPermits
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AuthorizedSignerPermit>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AuthorizedSignerPermitList> query2 = GetIqueryableList(iQueryable);
					  query2 = filter.GetFilteredQuery<AuthorizedSignerPermitList>(listQueryOperation, query2);
		
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AuthorizedSignerPermitList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> AuthorizedSignerPermitObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.AuthorizedSignerPermit",tenant).ToList();

                ObjectField objectField = (from a in AuthorizedSignerPermitObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<AuthorizedSignerPermitList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AuthorizedSignerPermitList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AuthorizedSignerPermitList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AuthorizedSignerPermitList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AuthorizedSignerPermitList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AuthorizedSignerPermitList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<AuthorizedSignerPermitList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Code);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Code);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<AuthorizedSignerPermitList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public AuthorizedSignerPermitList GetSingle(string code)
        {
            IQueryable<AuthorizedSignerPermit> AuthorizedSignerPermitQuery = (from a in context.AuthorizedSignerPermits
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<AuthorizedSignerPermitList> AuthorizedSignerPermitListQuery = GetIqueryableList( AuthorizedSignerPermitQuery);
            AuthorizedSignerPermitList AuthorizedSignerPermitList = AuthorizedSignerPermitListQuery.FirstOrDefault();
            return AuthorizedSignerPermitList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AuthorizedSignerPermit> iQueryable = (from a in context.AuthorizedSignerPermits  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<AuthorizedSignerPermit>(nonListQueryOperation, iQueryable);

            IQueryable<AuthorizedSignerPermitList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<AuthorizedSignerPermitList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 