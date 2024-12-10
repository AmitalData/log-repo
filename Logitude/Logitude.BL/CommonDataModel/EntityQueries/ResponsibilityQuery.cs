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
    public class ResponsibilityQuery
    {
        ResponsibilityRepository repository;

        public ResponsibilityQuery(int tenant)
        {
            repository = new ResponsibilityRepository(tenant);
        }

        public ResponsibilityQuery(ResponsibilityRepository repository)
        {
            this.repository = repository;
        }

        public ResponsibilityList GetSingle(string code)
        {
            return (from a in repository.context.Responsibilities
                    where a.Code == code
                    select new ResponsibilityList() { Code = a.Code, EnglishName = a.EnglishName, SearchFields = a.SearchFields }).FirstOrDefault();
        }

        public IQueryable<ResponsibilityList> GetIQueryableEntityList(IQueryable<Responsibility> iQueryable)
        {
            IQueryable<ResponsibilityList> result = from entity in iQueryable
                                                    select new ResponsibilityList()
                                                    {
                                                        EnglishName = entity.EnglishName,
                                                        LocalName = entity.LocalName,
                                                        Code = entity.Code,
                                                        SearchFields = entity.SearchFields,
                                                    };
            return result;
        }
    }
}