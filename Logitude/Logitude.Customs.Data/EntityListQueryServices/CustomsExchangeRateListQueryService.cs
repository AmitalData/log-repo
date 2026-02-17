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

    public partial class CustomsExchangeRateListQueryService
    {
	    private IQueryable<CustomsExchangeRateList> GetIqueryableList(IQueryable<CustomsExchangeRate> iQueryable)
        {
            IQueryable<CustomsExchangeRateList> query = (from a in iQueryable
                                                         select new CustomsExchangeRateList()
                                                    {
                                                       Id = a.Id,
                                                       CurrencyTypeCode = a.CurrencyTypeCode,
                                                       CurrencyTypeName = a.CurrencyType != null? a.CurrencyType.LocalName: null,
                                                       RateDate = a.RateDate,
                                                       ExchangeRate = a.ExchangeRate,
                                                       Tenant = a.Tenant,
                                                       UpdateDateTime = a.UpdateDateTime

                                                    });
            return query;
		}


        private IQueryable<CustomsExchangeRate> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsExchangeRate> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	