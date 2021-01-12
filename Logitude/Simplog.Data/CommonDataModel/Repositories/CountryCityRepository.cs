using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CountryCityRepository : IRepository<CountryCity>
    {
        ICommonDataContext commonDataContext;

        public CountryCityRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CountryCityRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CountryCityRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CountryCity> GetCountryCities(int tenant)
        {
            return (from record in context.CountryCities.Include("Country") where record.Tenant == tenant select record);
        }

        public CountryCity GetSingleCountryCity(string id, int tenant)
        {
            return (from record in context.CountryCities.Include("Country") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public CountryCity GetSingleCountryCityByCodeAndCountry(string code, string countryId, int tenant)
        {
            return (from record in context.CountryCities.Include("Country") where record.Code == code && record.CountryId == countryId && record.Tenant == tenant select record).FirstOrDefault();
        }

        public bool CheckCountryCityAlreadyExists(string name, string id, int tenant)
        {
            return (from a in context.CountryCities where a.Tenant == tenant && a.EnglishName == name && a.Id != id select a).Any();
        }

        public bool IsCountryHasCities(string countryId, int tenant)
        {
            return (from a in context.CountryCities where a.CountryId == countryId && a.Tenant == tenant select a).Any();
        }

        public IQueryable<CountryCity> GetCountryCitiesByCountry(string countryId, int tenant)
        {
            return (from d in context.CountryCities.Include("Country") 
                    where d.Tenant == tenant
                    && d.CountryId == countryId
                    select d);
        }

        public void Add(CountryCity entity)
        {
            context.CountryCities.Add(entity);
        }

        public void Remove(CountryCity entity)
        {
            context.CountryCities.Attach(entity);
            context.CountryCities.Remove(entity);
        }

        public void Update(CountryCity entity)
        {
            context.CountryCities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CountryCity> All()
        {
            return context.CountryCities.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CountryCity> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CountryCity GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CountryCity GetSingleCountryCityByNameAndCountry(string cityName, string countryId, int tenant)
        {
            return (from record in context.CountryCities where record.EnglishName == cityName && record.CountryId == countryId && record.Tenant == tenant select record).FirstOrDefault();
        }
    }
}
