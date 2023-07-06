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

    public partial class SupplierInvoiceItemsConDeclarListQueryService
    {
         private ICustomContext context;
        public SupplierInvoiceItemsConDeclarListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<SupplierInvoiceItemsConDeclarList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SupplierInvoiceItemsConDeclar> iQueryable = (from a in context.SupplierInvoiceItemsConDeclars
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SupplierInvoiceItemsConDeclar>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<SupplierInvoiceItemsConDeclarList> query2 = GetIqueryableList(iQueryable);
					  query2 = filter.GetFilteredQuery<SupplierInvoiceItemsConDeclarList>(listQueryOperation, query2);
		
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SupplierInvoiceItemsConDeclarList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> SupplierInvoiceItemsConDeclarObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.SupplierInvoiceItemsConDeclar",tenant).ToList();

                ObjectField objectField = (from a in SupplierInvoiceItemsConDeclarObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsConDeclarList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsConDeclarList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsConDeclarList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsConDeclarList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsConDeclarList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsConDeclarList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemsConDeclarList, decimal>(queryOperations, query2);
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

         public List<SupplierInvoiceItemsConDeclarList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public SupplierInvoiceItemsConDeclarList GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber)
        {
            IQueryable<SupplierInvoiceItemsConDeclar> SupplierInvoiceItemsConDeclarQuery = (from a in context.SupplierInvoiceItemsConDeclars
                                                       where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber
                                                       select a);
          
		  
		  			IQueryable<SupplierInvoiceItemsConDeclarList> SupplierInvoiceItemsConDeclarListQuery = GetIqueryableList( SupplierInvoiceItemsConDeclarQuery);
			            SupplierInvoiceItemsConDeclarList SupplierInvoiceItemsConDeclarList = SupplierInvoiceItemsConDeclarListQuery.FirstOrDefault();
            return SupplierInvoiceItemsConDeclarList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SupplierInvoiceItemsConDeclar> iQueryable = (from a in context.SupplierInvoiceItemsConDeclars 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<SupplierInvoiceItemsConDeclar>(nonListQueryOperation, iQueryable);

            IQueryable<SupplierInvoiceItemsConDeclarList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<SupplierInvoiceItemsConDeclarList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 