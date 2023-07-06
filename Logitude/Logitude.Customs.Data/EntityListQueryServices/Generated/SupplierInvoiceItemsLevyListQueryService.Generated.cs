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

    public partial class SupplierInvoiceItemsLevyListQueryService
    {
         private ICustomContext context;
        public SupplierInvoiceItemsLevyListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<SupplierInvoiceItemsLevyList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SupplierInvoiceItemsLevy> iQueryable = (from a in context.SupplierInvoiceItemsLevies
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SupplierInvoiceItemsLevy>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<SupplierInvoiceItemsLevyList> query2 = GetIqueryableList(iQueryable);
<<<<<<< HEAD
					  query2 = filter.GetFilteredQuery<SupplierInvoiceItemsLevyList>(listQueryOperation, query2);
		
=======
           
            query2 = filter.GetFilteredQuery<SupplierInvoiceItemsLevyList>(listQueryOperation, query2);

>>>>>>> parent of 094118ae335 (#181601)
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SupplierInvoiceItemsLevyList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> SupplierInvoiceItemsLevyObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.SupplierInvoiceItemsLevy",tenant).ToList();

                ObjectField objectField = (from a in SupplierInvoiceItemsLevyObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsLevyList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsLevyList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsLevyList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsLevyList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsLevyList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsLevyList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsLevyList, decimal>(queryOperations, query2);
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

         public List<SupplierInvoiceItemsLevyList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public SupplierInvoiceItemsLevyList GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber)
        {
            IQueryable<SupplierInvoiceItemsLevy> SupplierInvoiceItemsLevyQuery = (from a in context.SupplierInvoiceItemsLevies
                                                       where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber
                                                       select a);

             
            IQueryable<SupplierInvoiceItemsLevyList> SupplierInvoiceItemsLevyListQuery = GetIqueryableList( SupplierInvoiceItemsLevyQuery);
            SupplierInvoiceItemsLevyList SupplierInvoiceItemsLevyList = SupplierInvoiceItemsLevyListQuery.FirstOrDefault();
            return SupplierInvoiceItemsLevyList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SupplierInvoiceItemsLevy> iQueryable = (from a in context.SupplierInvoiceItemsLevies 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<SupplierInvoiceItemsLevy>(nonListQueryOperation, iQueryable);

            IQueryable<SupplierInvoiceItemsLevyList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<SupplierInvoiceItemsLevyList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 