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
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CustomsCountryListQueryService
    {
	    private IQueryable<CustomsCountryList> GetIqueryableList(IQueryable<CustomsCountry> iQueryable)
        {

			int tenant = 1;
			try
			{

				string token = HttpContext.Current.Request.Headers["Token"];
				AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
				tenant = authToken.Tenant;
			}
			catch (Exception)
			{

				// throw;
			}

			var customsCountryTenantRepository = new CustomsCountryTenantRepository(context);

			var q_customsCountryTenant = customsCountryTenantRepository.GetAll(tenant);

			IQueryable<CustomsCountryList> query = (from a in iQueryable
													join cc in q_customsCountryTenant.Include("TradeAgreement")
														on a.Code equals cc.Code into xy
													from s in xy.DefaultIfEmpty()
													select new CustomsCountryList()
                                             {
                                                 Code = a.Code,
                                                 EnglishName = a.EnglishName,
                                                 LocalName = a.LocalName,
                                                 SearchFields = a.SearchFields,
                                                 Inactive = a.Inactive,
                                                 MalamId = a.MalamId,
                                                 TarriffCode = s != null ? s.TarriffCode : a.TarriffCode,
                                                 TarriffName = s != null ? (s.TradeAgreement.LocalName != null ? s.TradeAgreement.LocalName : null) : (a.TradeAgreement.LocalName != null ? a.TradeAgreement.LocalName : null),

                                             });
            return query;
		}

		private IQueryable<CustomsCountry> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsCountry> iQueryable)
        {
            return iQueryable;
		}
	}


}
	