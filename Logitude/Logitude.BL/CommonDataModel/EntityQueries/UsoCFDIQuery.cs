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
    public class UsoCFDIQuery
    {
        UsoCFDIRepository repository;

        public UsoCFDIQuery()
        {
            repository = new UsoCFDIRepository();
        }

        public UsoCFDIQuery(int tenant)
        {
            repository = new UsoCFDIRepository(tenant);
        }

        public UsoCFDIQuery(UsoCFDIRepository repository)
        {
            this.repository = repository;
        }

        public UsoCFDIPM GetSingleUsoCFDIPM(string code)
        {
            return (from a in repository.context.UsoCFDIs
                    where a.Code == code
                    select new UsoCFDIPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public UsoCFDIPM GetSinglePM(string code, int tenant = 0)
        {
            return (from a in repository.context.UsoCFDIs
                    where a.Code == code
                    select new UsoCFDIPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<UsoCFDIPM> GetUsoCFDIPMs()
        {
            return (from a in repository.context.UsoCFDIs

                    select new UsoCFDIPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, });
        }

        public IQueryable<UsoCFDIList> GetIQueryableEntityList(IQueryable<UsoCFDI> iQueryable)
        {
            IQueryable<UsoCFDIList> result = from entity in iQueryable
                                                select new UsoCFDIList()
                                                {
                                                    Name = entity.Name,
                                                    Code = entity.Code,
                                                    SearchFields = entity.SearchFields,
                                                };
            return result;
        }
    }
}