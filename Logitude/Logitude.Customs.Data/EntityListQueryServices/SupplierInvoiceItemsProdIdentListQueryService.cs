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

    public partial class SupplierInvoiceItemsProdIdentListQueryService
    {
        private IQueryable<SupplierInvoiceItemsProdIdentList> GetIqueryableList(IQueryable<SupplierInvoiceItemsProdIdent> iQueryable)
        {
            IQueryable<SupplierInvoiceItemsProdIdentList> query = (from a in iQueryable
                                                                   select new SupplierInvoiceItemsProdIdentList()
                                                                     {

                                                                         DeclarationId = a.DeclarationId,

                                                                         InvoiceCounterKey = a.InvoiceCounterKey,
                                                                         InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                         LineNumber = a.LineNumber,
                                                                         Identification = a.Identification,
                                                                         Tenant = a.Tenant,
                                                                         TypeCode = a.TypeCode,
                                                                     });
            return query;
		}

        private IQueryable<SupplierInvoiceItemsProdIdent> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvoiceItemsProdIdent> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	