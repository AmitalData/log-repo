	using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

    public partial class VendorCurrencyListQueryService
    {
	    private IQueryable<VendorCurrencyList> GetIqueryableList(IQueryable<VendorCurrency> iQueryable)
        {
		IQueryable<VendorCurrencyList> query = (from a in iQueryable
                                            select new VendorCurrencyList()
											{
                     
					                            Tenant = a.Tenant,
												LineNumber=a.LineNumber,
												VendorId=a.VendorId,
												Currency=a.Currency,
												CurrencyTypeName= a.CurrencyType.LocalName
											});
            return query;
		}

		private IQueryable<VendorCurrency> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<VendorCurrency> iQueryable, int tenant)
        {
			return iQueryable;
		}
    }


}
	