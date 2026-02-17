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

    public partial class SupplierInvoiceItemsModListQueryService
    {
	    private IQueryable<SupplierInvoiceItemsModList> GetIqueryableList(IQueryable<SupplierInvoiceItemsMod> iQueryable)
        {
			throw new NotImplementedException();
		}
        private IQueryable<SupplierInvoiceItemsMod> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SupplierInvoiceItemsMod> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	