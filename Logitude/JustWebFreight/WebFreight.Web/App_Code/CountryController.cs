using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.App_Code
{
    public class CountryController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


 

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        public IQueryable<CountryList> GetCountriesByTenant(bool bycrmTenant,int tenant)
        {
            tenant = bycrmTenant?LogitudeSettings.LogitudeCRMTenantNumber:tenant;
            IQueryable<CountryList> countryList = null;
            CountryRepository countryRepository = new CountryRepository(tenant);
            CountryQuery entityQuery = new CountryQuery(countryRepository);

            countryList = entityQuery.GetIQueryableEntityList(countryRepository.GetCountries(tenant));
           
            return countryList;
        }

    }
}