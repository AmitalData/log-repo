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

    public partial class SupplierInvoiceItemProcesTypeListQueryService
    {
         private ICustomContext context;
        public SupplierInvoiceItemProcesTypeListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<SupplierInvoiceItemProcesTypeList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SupplierInvoiceItemProcesType> iQueryable = (from a in context.SupplierInvoiceItemProcesTypes
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SupplierInvoiceItemProcesType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<SupplierInvoiceItemProcesTypeList> query2 = GetIqueryableList(iQueryable);
<<<<<<< HEAD
					  query2 = filter.GetFilteredQuery<SupplierInvoiceItemProcesTypeList>(listQueryOperation, query2);
		
=======
           
            query2 = filter.GetFilteredQuery<SupplierInvoiceItemProcesTypeList>(listQueryOperation, query2);

>>>>>>> parent of 094118ae335 (#181601)
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SupplierInvoiceItemProcesTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> SupplierInvoiceItemProcesTypeObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.SupplierInvoiceItemProcesType",tenant).ToList();

                ObjectField objectField = (from a in SupplierInvoiceItemProcesTypeObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<SupplierInvoiceItemProcesTypeList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemProcesTypeList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemProcesTypeList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemProcesTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemProcesTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemProcesTypeList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemProcesTypeList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.LineNumber);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.LineNumber);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<SupplierInvoiceItemProcesTypeList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public SupplierInvoiceItemProcesTypeList GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber)
        {
            IQueryable<SupplierInvoiceItemProcesType> SupplierInvoiceItemProcesTypeQuery = (from a in context.SupplierInvoiceItemProcesTypes
                                                       where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber
                                                       select a);

             
            IQueryable<SupplierInvoiceItemProcesTypeList> SupplierInvoiceItemProcesTypeListQuery = GetIqueryableList( SupplierInvoiceItemProcesTypeQuery);
            SupplierInvoiceItemProcesTypeList SupplierInvoiceItemProcesTypeList = SupplierInvoiceItemProcesTypeListQuery.FirstOrDefault();
            return SupplierInvoiceItemProcesTypeList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SupplierInvoiceItemProcesType> iQueryable = (from a in context.SupplierInvoiceItemProcesTypes 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<SupplierInvoiceItemProcesType>(nonListQueryOperation, iQueryable);

            IQueryable<SupplierInvoiceItemProcesTypeList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<SupplierInvoiceItemProcesTypeList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 