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

    public partial class SupplierInvoiceModificationListQueryService
    {
	    private IQueryable<SupplierInvoiceModificationList> GetIqueryableList(IQueryable<SupplierInvoiceModification> iQueryable)
        {
            IQueryable<SupplierInvoiceModificationList> query = (from a in iQueryable
                                                                 select new SupplierInvoiceModificationList()
                                                    {
                                                   DeclarationId = a.DeclarationId,
                                                   Amount = a.Amount,
                                                   CurrencyTypeCode = a.CurrencyTypeCode,
                                                   InvoiceCounterKey = a.InvoiceCounterKey,
                                                   Tenant = a.Tenant,
                                                   TypeCode = a.TypeCode,



                                                    });
            return query;
		}

        private IQueryable<SupplierInvoiceModification> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvoiceModification> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	