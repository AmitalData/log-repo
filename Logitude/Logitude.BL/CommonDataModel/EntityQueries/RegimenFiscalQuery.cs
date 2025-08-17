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
    public class RegimenFiscalQuery
    {
        RegimenFiscalRepository repository;



        public RegimenFiscalQuery(int tenant)
        {
            repository = new RegimenFiscalRepository(tenant);
        }

        public RegimenFiscalQuery(RegimenFiscalRepository repository)
        {
            this.repository = repository;
        }

        public RegimenFiscalPM GetSingleRegimenFiscalPM(string code)
        {
            return (from a in repository.context.RegimenFiscals
                    where a.Code == code
                    select new RegimenFiscalPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public RegimenFiscalPM GetSinglePM(string code, int tenant = 0)
        {
            return (from a in repository.context.RegimenFiscals
                    where a.Code == code
                    select new RegimenFiscalPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<RegimenFiscalPM> GetRegimenFiscalPMs()
        {
            return (from a in repository.context.RegimenFiscals

                    select new RegimenFiscalPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, });
        }

        public IQueryable<RegimenFiscalList> GetIQueryableEntityList(IQueryable<RegimenFiscal> iQueryable)
        {
            IQueryable<RegimenFiscalList> result = from entity in iQueryable
                                                select new RegimenFiscalList()
                                                {
                                                    Name = entity.Name,
                                                    Code = entity.Code,
                                                    SearchFields = entity.SearchFields,
                                                };
            return result;
        }
    }
}