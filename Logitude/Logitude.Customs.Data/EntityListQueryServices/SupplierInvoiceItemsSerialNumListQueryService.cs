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

    public partial class SupplierInvoiceItemsSerialNumListQueryService
    {
	    private IQueryable<SupplierInvoiceItemsSerialNumList> GetIqueryableList(IQueryable<SupplierInvoiceItemsSerialNum> iQueryable)
        {
            IQueryable<SupplierInvoiceItemsSerialNumList> query = (from a in iQueryable
                                                                      select new SupplierInvoiceItemsSerialNumList()
                                                                               {

                                                                                   DeclarationId = a.DeclarationId,

                                                                                   InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                   InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                   LineNumber = a.LineNumber,
                                                                                   SerialNumber = a.SerialNumber,
                                                                                   Tenant = a.Tenant,
                                                                                   TypeCode = a.TypeCode,
                                                                               });
            return query;
		}

        private IQueryable<SupplierInvoiceItemsSerialNum> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvoiceItemsSerialNum> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	