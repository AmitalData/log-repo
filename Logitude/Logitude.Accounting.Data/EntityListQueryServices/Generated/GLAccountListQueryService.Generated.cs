using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class GLAccountListQueryService
    {
         private IAccountingContext context;
        public GLAccountListQueryService(IAccountingContext context)
        {
            this.context = context;
        }

        public List<GLAccountList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            
            IQueryable<GLAccount> iQueryable = (from a in context.GLAccounts
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<GLAccount>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            User loggedUser = GetLoggedUser(tenant);

            IQueryable<GLAccountList> query2 = GetIqueryableList(iQueryable, loggedUser);
            
            //query2 = MapListFields(query2, loggedUser);

            FullAccountingSettingListQueryService fullAccountingSettingListQueryService = new FullAccountingSettingListQueryService(AccountingContext.GetContext(tenant));
            var settings = fullAccountingSettingListQueryService.GetSingle(tenant.ToString());

            query2 = filter.GetFilteredQuery<GLAccountList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(GLAccountList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> GLAccountObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("GLAccount",tenant).ToList();

                ObjectField objectField = (from a in GLAccountObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<GLAccountList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<GLAccountList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<GLAccountList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<GLAccountList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<GLAccountList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<GLAccountList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<GLAccountList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.DisplayNumber);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.DisplayNumber);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<GLAccountList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public GLAccountList GetSingle(string id)
        {
            IQueryable<GLAccount> GLAccountQuery = (from a in context.GLAccounts
                                                       where a.Id == id
                                                       select a);

            var tempGlAccountData = GLAccountQuery.FirstOrDefault();
            if (tempGlAccountData == null)
            {
                return null;
            }
            IQueryable<GLAccountList> GLAccountListQuery = GetIqueryableList(GLAccountQuery,GetLoggedUser(tempGlAccountData.Tenant));
            GLAccountList GLAccountList = GLAccountListQuery.FirstOrDefault();
            return GLAccountList;
           
        }

        public GLAccountList GetSingle(string id, int tenant)
        {
            IQueryable<GLAccount> GLAccountQuery = (from a in context.GLAccounts
                                                    where a.Id == id
                                                    select a);


            IQueryable<GLAccountList> GLAccountListQuery = GetIqueryableList(GLAccountQuery,GetLoggedUser(tenant));


            User loggedUser = GetLoggedUser(tenant);
            //GLAccountListQuery = MapListFields(GLAccountListQuery, loggedUser);

            GLAccountList GLAccountList = GLAccountListQuery.FirstOrDefault();
            return GLAccountList;

        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<GLAccount> iQueryable = (from a in context.GLAccounts 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();


            iQueryable = filter.GetFilteredQuery<GLAccount>(nonListQueryOperation, iQueryable);

            User loggedUser = GetLoggedUser(tenant);

            IQueryable<GLAccountList> query2 = GetIqueryableList(iQueryable, loggedUser);
            
            
           //query2 = MapListFields(query2, loggedUser);

            query2 = filter.GetFilteredQuery<GLAccountList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 