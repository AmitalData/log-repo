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

    public partial class SupplierInvoiceItemVehicleModListQueryService
    {
	    private IQueryable<SupplierInvoiceItemVehicleModList> GetIqueryableList(IQueryable<SupplierInvoiceItemVehicleMod> iQueryable)
        {
            IQueryable<SupplierInvoiceItemVehicleModList> query = (from a in iQueryable
                                                                   select new SupplierInvoiceItemVehicleModList()
                                                          {
                                                              DeclarationId = a.DeclarationId,
                                                              DeductAmount = a.DeductAmount,
                                                              AdjustmentTypeCode = a.AdjustmentTypeCode,
                                                              InvoiceCounterKey = a.InvoiceCounterKey,
                                                              InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                              LineNumber = a.LineNumber,
                                                              VehicleLineNumber = a.VehicleLineNumber,
                                                             

                                                          });
            return query;
		}

        private IQueryable<SupplierInvoiceItemVehicleMod> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvoiceItemVehicleMod> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	