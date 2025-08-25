using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class PostalCodeQuery
    {
        PostalCodeRepository repository;

 

        public PostalCodeQuery(int tenant)
        {
            repository = new PostalCodeRepository(tenant);
        }

        public PostalCodeQuery(PostalCodeRepository repository)
        {
            this.repository = repository;
        }

        public PostalCodePM GetSinglePostalCodePM(string code)
        {
            return (from a in repository.context.PostalCodes
                    where a.Code == code
                    select new PostalCodePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, CountryCode = a.CountryCode, }).FirstOrDefault();
        }

        public PostalCodePM GetSinglePM(string code)
        {
            return (from a in repository.context.PostalCodes
                    where a.Code == code
                    select new PostalCodePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, CountryCode = a.CountryCode, }).FirstOrDefault();
        }

        public PostalCodePM GetSinglePMByCountryCode(string code, string countryCode)
        {
            return (from a in repository.context.PostalCodes
                    where a.Code == code && countryCode == countryCode
                    select new PostalCodePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, CountryCode = a.CountryCode, }).FirstOrDefault();
        }

        public IQueryable<PostalCodePM> GetPostalCodePMs()
        {
            return (from a in repository.context.PostalCodes

                    select new PostalCodePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, CountryCode = a.CountryCode, });
        }

        public IQueryable<PostalCodeList> GetIQueryableEntityList(IQueryable<PostalCode> iQueryable)
        {
            IQueryable<PostalCodeList> result = from entity in iQueryable
                                                select new PostalCodeList()
                                                {
                                                    Name = entity.Name,
                                                    Code = entity.Code,
                                                    SearchFields = entity.SearchFields,
                                                    CountryCode = entity.CountryCode,
                                                };
            return result;
        }
    }
}