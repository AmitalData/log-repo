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
    public class DueTypeQuery
    {
        DueTypeRepository repository;

   

        public DueTypeQuery(int tenant)
        {
            repository = new DueTypeRepository(tenant);
        }

        public DueTypeQuery(DueTypeRepository repository)
        {
            this.repository = repository;
        }

        public DueTypePM GetSingleDueTypePM(string code)
        {
            return (from a in repository.context.DueTypes
                    where a.Code == code
                    select new DueTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public DueTypePM GetSinglePM(string code, int tenant = 0)
        {
            return (from a in repository.context.DueTypes
                    where a.Code == code
                    select new DueTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<DueTypePM> GetDueTypePMs()
        {
            return (from a in repository.context.DueTypes

                    select new DueTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, });
        }

        public IQueryable<DueTypeList> GetIQueryableEntityList(IQueryable<DueType> iQueryable)
        {
            IQueryable<DueTypeList> result = from entity in iQueryable
                                             select new DueTypeList()
                                             {
                                                 Name = entity.Name,
                                                 Code = entity.Code,
                                                 SearchFields = entity.SearchFields,
                                             };
            return result;
        }
    }
}