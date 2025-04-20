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

    public partial class SupplierInvoiceItemsReqListListQueryService
    {
	    private IQueryable<SupplierInvoiceItemsReqListList> GetIqueryableList(IQueryable<SupplierInvoiceItemsReqList> iQueryable)
        {
		IQueryable<SupplierInvoiceItemsReqListList> query = (from a in iQueryable
                                            select new SupplierInvoiceItemsReqListList()
											{
                     
					                          SearchFields = a.SearchFields,
					
					                          DeclarationId = a.DeclarationId,
					
					                          RequestType = a.RequestType,
					
					                          ProductFileNumber = a.ProductFileNumber,
					
					                          ManufactureCountryCode = a.ManufactureCountryCode,
					
					                          ManufacturerName = a.ManufacturerName,
					
					                          Remarks = a.Remarks,
					
					                          DutchRequested = a.DutchRequested,
					
		                    	            });
            return query;
		}

		private IQueryable<SupplierInvoiceItemsReqList> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<SupplierInvoiceItemsReqList> iQueryable, int tenant)
        {
			return iQueryable;
		}
			}


}
	