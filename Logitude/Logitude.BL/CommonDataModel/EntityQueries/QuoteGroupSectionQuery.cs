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
    public class QuoteGroupSectionQuery
    {
        QuoteGroupSectionRepository repository;





        public QuoteGroupSectionQuery(QuoteGroupSectionRepository repository)
        {
            this.repository = repository;
        }

        public QuoteGroupSectionPM GetSingleQuoteGroupSectionPM(string code)
        {
            return (from a in repository.context.QuoteGroupSections
                    where a.Code == code
                    select new QuoteGroupSectionPM() { Code = a.Code, Name = a.Name, Searchfields=a.Searchfields }).FirstOrDefault();
        }

        public QuoteGroupSectionPM GetSinglePM(string code, int tenant = 0)
        {
            return (from a in repository.context.QuoteGroupSections
                    where a.Code == code
                    select new QuoteGroupSectionPM() { Code = a.Code, Name = a.Name , Searchfields=a.Searchfields }).FirstOrDefault();
        }

        public IQueryable<QuoteGroupSectionPM> GetQuoteGroupSectionPMs()
        {
            return (from a in repository.context.QuoteGroupSections

                    select new QuoteGroupSectionPM() { Code = a.Code, Name = a.Name, Searchfields=a.Searchfields});
        }

        public IQueryable<QuoteGroupSectionList> GetIQueryableEntityList(IQueryable<QuoteGroupSection> iQueryable)
        {
            IQueryable<QuoteGroupSectionList> result = from entity in iQueryable
                                             select new QuoteGroupSectionList()
                                             {
                                                 Name = entity.Name,
                                                 Code = entity.Code,
                                                 Searchfields=entity.Searchfields,
                                             };
            return result;
        }
    }
}