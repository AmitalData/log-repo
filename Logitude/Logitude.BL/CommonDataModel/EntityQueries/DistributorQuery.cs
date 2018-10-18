using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DistributorQuery
    {
        DistributorRepository repository;

        public DistributorQuery()
        {
            repository = new DistributorRepository();
        }

        public DistributorQuery(int tenant)
        {
            repository = new DistributorRepository(tenant);
        }

        public DistributorQuery(DistributorRepository repository)
        {
            this.repository = repository;
        }

        public DistributorPM GetSingleDistributorPM(string code)
        {
            return (from a in repository.context.Distributors
                    where a.Code == code
                    select new DistributorPM() { Code = a.Code, EnglishName = a.EnglishName, LocalName=a.LocalName,SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public DistributorPM GetSinglePM(string code, int tenant = 0)
        {
            return (from a in repository.context.Distributors
                    where a.Code == code
                    select new DistributorPM() { Code = a.Code, EnglishName = a.EnglishName,LocalName=a.LocalName, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<DistributorPM> GetDistributorPMs()
        {
            return (from a in repository.context.Distributors

                    select new DistributorPM() { Code = a.Code, EnglishName = a.EnglishName, LocalName = a.LocalName,SearchFields = a.SearchFields, });
        }

        public IQueryable<DistributorList> GetIQueryableEntityList(IQueryable<Distributor> iQueryable)
        {
            IQueryable<DistributorList> result = from entity in iQueryable
                                             select new DistributorList()
                                             {
                                                 EnglishName = entity.EnglishName,
                                                 Code = entity.Code,
                                                 LocalName=entity.LocalName,
                                                 SearchFields = entity.SearchFields,
                                             };
            return result;
        }
    }
}