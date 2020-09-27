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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CurrencyTypeListQueryService
    {
	    private IQueryable<CurrencyTypeList> GetIqueryableList(IQueryable<CurrencyType> iQueryable)
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

            var currencyTypeTenantRepository = new CurrencyTypeTenantRepository(context);
            
            var q_currencyTypeTenant = currencyTypeTenantRepository.GetAll(tenant);


            IQueryable<CurrencyTypeList> query = (from a in iQueryable
                                                  join t in q_currencyTypeTenant
                                                  on a.Code equals t.Code into currencyTypeTenant_Join
                                                  from t in currencyTypeTenant_Join.DefaultIfEmpty()
                                                  select new CurrencyTypeList()
                                                  {
                                                      Code = a.Code,
                                                      EnglishName = a.EnglishName,
                                                      LocalName = a.LocalName,
                                                      SearchFields = a.SearchFields,
                                                      Inactive = a.Inactive || (t != null ? (t.TenantInactive) : false),
                                                      MehesInactive = a.Inactive,
                                                      ///Tenant = tenant,
                                                      TenantInactive = t != null ? (t.TenantInactive) : false,
                                                      CodeSorted = a.Code == "USD" ? "00USD" : (a.Code == "EUR" ? "00EUR" : "99" + a.Code)
                                                  });
            return query;
		}

        private IQueryable<CurrencyType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CurrencyType> iQueryable)
        {
            return iQueryable;
        }
	}


}
	