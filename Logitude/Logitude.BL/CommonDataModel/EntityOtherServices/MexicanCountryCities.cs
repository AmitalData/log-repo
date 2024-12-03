using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityOtherServices
{
    public class MexicanCountryCities
    {
        public void AddMexicanCountryCities(int tenant, string countryId = null)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            Country country_MX = commonContext.Countries.Where(a => a.Code == "MX" && a.Tenant == 0).FirstOrDefault();
            var countriesCities = (from countryCity in commonContext.CountryCities.Include("State")
                           where countryCity.CountryId == country_MX.Id && countryCity.Tenant == 0
                           select new
                           {
                               Code = countryCity.Code,
                               EnglishName = countryCity.EnglishName,
                               LocalName = countryCity.LocalName,
                               StateCode = countryCity.State != null ? countryCity.State.Code: null,
                               SearchFields = countryCity.SearchFields,
                           });

            foreach (var item in countriesCities)
            {
                if(countryId == null)
                {
                    var country = commonContext.Countries.Where(a => a.Tenant == tenant && a.Code == "MX").FirstOrDefault();
                    countryId = country != null ? country.Id : null;
                }

                if (countryId != null)
                {
                    CountryCity newCity = commonContext.CountryCities.Where(p => p.Code == item.Code && p.Tenant == tenant && p.CountryId == countryId).FirstOrDefault();
                    if (newCity == null)
                    {
                        var state = commonContext.States.Where(a => a.Code == item.StateCode && a.Tenant == tenant).FirstOrDefault();
                        if (state != null)
                        {
                            newCity = new CountryCity()
                            {
                                Id = IdCounter.GetNumber("CountryCity", 0).ToString(),
                                Tenant = tenant,
                                Code = item.Code,
                                EnglishName = item.EnglishName,
                                LocalName = item.LocalName,
                                StateId = state.Id,
                                CountryId = countryId,
                                SearchFields = item.SearchFields,
                            };

                            commonContext.CountryCities.Add(newCity);
                        }
                    }
                }
            }
            commonContext.SaveChanges();
        }
    }
}
