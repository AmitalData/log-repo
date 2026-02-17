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

    public partial class SupplierInvoiceItemsLevyListQueryService
    {
	    private IQueryable<SupplierInvoiceItemsLevyList> GetIqueryableList(IQueryable<SupplierInvoiceItemsLevy> iQueryable)
        {
            IQueryable<SupplierInvoiceItemsLevyList> query = (from a in iQueryable.Include("TradeLevyExamptType")

                                                              select new SupplierInvoiceItemsLevyList()
                                                         {
                                                             DeclarationId = a.DeclarationId,
                                                             InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                             LineNumber = a.LineNumber,
                                                             Tenant = a.Tenant,
                                                             TradeLevyExamptCode = a.TradeLevyExamptCode,
                                                             TradeLevyNumber = a.TradeLevyNumber,
                                                             TradeLevyExamptName = a.TradeLevyExamptType != null ? a.TradeLevyExamptType.LocalName : null,

                                                         });
            return query;
		}

        private IQueryable<SupplierInvoiceItemsLevy> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvoiceItemsLevy> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	