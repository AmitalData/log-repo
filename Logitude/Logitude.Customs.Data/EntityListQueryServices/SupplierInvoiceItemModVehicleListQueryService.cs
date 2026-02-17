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

    public partial class SupplierInvoiceItemModVehicleListQueryService
    {
	    private IQueryable<SupplierInvoiceItemModVehicleList> GetIqueryableList(IQueryable<SupplierInvoiceItemModVehicle> iQueryable)
        {
		IQueryable<SupplierInvoiceItemModVehicleList> query = (from a in iQueryable
                                            select new SupplierInvoiceItemModVehicleList()
											{
                     
					                          DeclarationId = a.DeclarationId,
					
					                          InvoiceCounterKey = a.InvoiceCounterKey,
					
					                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
					
					                          AdjustmentTypeCode = a.AdjustmentTypeCode,
					
					                          DeductAmount = a.DeductAmount,
					
		                    	            });
            return query;
		}

        private IQueryable<SupplierInvoiceItemModVehicle> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvoiceItemModVehicle> iQueryable, int tenant)
        {
            return iQueryable;
		}
			}


}
	