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
        private IQueryable<SupplierInvoiceItemsConDeclarList> GetIqueryableList(IQueryable<SupplierInvoiceItemsConDeclar> iQueryable)
        {
            IQueryable<SupplierInvoiceItemsConDeclarList> query = (from a in iQueryable
                                                                              select new SupplierInvoiceItemsConDeclarList()
                                                    {
                                                      InvoiceNumber = a.InvoiceNumber,
                                                      DeclarationId = a.DeclarationId,
                                                      DeclarationNumber = a.DeclarationNumber,
                                                      DeclarationTypeCode = a.DeclarationTypeCode,
                                                      InvoiceCounterKey = a.InvoiceCounterKey,
                                                      InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                      LineNumber = a.LineNumber,
                                                      Quantity = a.Quantity,
                                                      ItemSequence = a.ItemSequence,
                                                      Tenant = a.Tenant,

                                                    });
            return query;
		}

        private IQueryable<SupplierInvoiceItemsConDeclar> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvoiceItemsConDeclar> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	