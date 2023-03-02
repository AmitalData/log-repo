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

    public partial class AddressCurrencyListQueryService
    {
	    private IQueryable<AddressCurrencyList> GetIqueryableList(IQueryable<AddressCurrency> iQueryable)
        {
		IQueryable<AddressCurrencyList> query = (from a in iQueryable
                                            select new AddressCurrencyList()
											{

												Tenant = a.Tenant,
												LineNumber = a.LineNumber,
												AddressId = a.AddressId,
												Currency = a.Currency,
												CurrencyTypeName = a.CurrencyType.LocalName

											});
            return query;
		}

		private IQueryable<AddressCurrency> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AddressCurrency> iQueryable, int tenant)
        {
			return iQueryable;
		}
	}


}
	