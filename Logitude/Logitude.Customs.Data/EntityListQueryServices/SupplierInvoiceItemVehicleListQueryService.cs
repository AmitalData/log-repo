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
	    private IQueryable<SupplierInvoiceItemVehicleList> GetIqueryableList(IQueryable<SupplierInvoiceItemVehicle> iQueryable)
        {
            IQueryable<SupplierInvoiceItemVehicleList> query = (from a in iQueryable
                                                                select new SupplierInvoiceItemVehicleList()
                                                                     {
                                                                         DeclarationId = a.DeclarationId,
                                                                         InvoiceCounterKey = a.InvoiceCounterKey,
                                                                         LineNumber = a.LineNumber,
                                                                         Tenant = a.Tenant,
                                                                         InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                         RichbitFileNumber = a.RichbitFileNumber,
                                                                         SequenceNumeric = a.SequenceNumeric,
                                                                         VehicleChassisNumber= a.VehicleChassisNumber,
                                                                         VehicleId = a.VehicleId,
                                                                         VehicleTypeCode = a.VehicleTypeCode

                                                                     });
            return query;
		}

        private IQueryable<SupplierInvoiceItemVehicle> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvoiceItemVehicle> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
	}


}
	