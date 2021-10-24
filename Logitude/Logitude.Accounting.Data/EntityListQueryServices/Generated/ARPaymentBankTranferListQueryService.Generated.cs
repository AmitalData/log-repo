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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class ARPaymentBankTranferListQueryService
    {
         private IAccountingContext context;
        public ARPaymentBankTranferListQueryService(IAccountingContext context)
        {
            this.context = context;
        }

        public List<ARPaymentBankTranferList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARPaymentBankTranfer> iQueryable = (from a in context.ARPaymentBankTranfers
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARPaymentBankTranfer>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ARPaymentBankTranferList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ARPaymentBankTranferList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARPaymentBankTranferList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ARPaymentBankTranferObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ARPaymentBankTranfer",tenant).ToList();

                ObjectField objectField = (from a in ARPaymentBankTranferObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ARPaymentBankTranferList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentBankTranferList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentBankTranferList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentBankTranferList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentBankTranferList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentBankTranferList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ARPaymentBankTranferList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<ARPaymentBankTranferList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ARPaymentBankTranferList GetSingle(string id)
        {
            IQueryable<ARPaymentBankTranfer> ARPaymentBankTranferQuery = (from a in context.ARPaymentBankTranfers
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<ARPaymentBankTranferList> ARPaymentBankTranferListQuery = GetIqueryableList( ARPaymentBankTranferQuery);
            ARPaymentBankTranferList ARPaymentBankTranferList = ARPaymentBankTranferListQuery.FirstOrDefault();
            return ARPaymentBankTranferList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARPaymentBankTranfer> iQueryable = (from a in context.ARPaymentBankTranfers 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ARPaymentBankTranfer>(nonListQueryOperation, iQueryable);

            IQueryable<ARPaymentBankTranferList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ARPaymentBankTranferList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 