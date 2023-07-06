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

    public partial class SupplierInvoiceItemVehicleListQueryService
    {
         private ICustomContext context;
        public SupplierInvoiceItemVehicleListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<SupplierInvoiceItemVehicleList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SupplierInvoiceItemVehicle> iQueryable = (from a in context.SupplierInvoiceItemVehicles
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SupplierInvoiceItemVehicle>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<SupplierInvoiceItemVehicleList> query2 = GetIqueryableList(iQueryable);
					  query2 = filter.GetFilteredQuery<SupplierInvoiceItemVehicleList>(listQueryOperation, query2);
		
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SupplierInvoiceItemVehicleList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> SupplierInvoiceItemVehicleObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.SupplierInvoiceItemVehicle",tenant).ToList();

                ObjectField objectField = (from a in SupplierInvoiceItemVehicleObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<SupplierInvoiceItemVehicleList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemVehicleList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemVehicleList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemVehicleList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemVehicleList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemVehicleList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<SupplierInvoiceItemVehicleList, decimal>(queryOperations, query2);
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

         public List<SupplierInvoiceItemVehicleList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public SupplierInvoiceItemVehicleList GetSingle(string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int linenumber)
        {
            IQueryable<SupplierInvoiceItemVehicle> SupplierInvoiceItemVehicleQuery = (from a in context.SupplierInvoiceItemVehicles
                                                       where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.InvoiceItemLineNumber == invoiceitemlinenumber && a.LineNumber == linenumber
                                                       select a);
          
		  
		  			IQueryable<SupplierInvoiceItemVehicleList> SupplierInvoiceItemVehicleListQuery = GetIqueryableList( SupplierInvoiceItemVehicleQuery);
			            SupplierInvoiceItemVehicleList SupplierInvoiceItemVehicleList = SupplierInvoiceItemVehicleListQuery.FirstOrDefault();
            return SupplierInvoiceItemVehicleList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SupplierInvoiceItemVehicle> iQueryable = (from a in context.SupplierInvoiceItemVehicles 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<SupplierInvoiceItemVehicle>(nonListQueryOperation, iQueryable);

            IQueryable<SupplierInvoiceItemVehicleList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<SupplierInvoiceItemVehicleList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 