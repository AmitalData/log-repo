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
    public class TemplateFormatQuery
    {
        TemplateFormatRepository repository;

        public TemplateFormatQuery()
        {
            repository = new TemplateFormatRepository(); 
        }

        public TemplateFormatQuery(int tenant)
        {
            repository = new TemplateFormatRepository(tenant);
        }

        public TemplateFormatQuery(TemplateFormatRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<TemplateFormatPM> GetTemplateFormatPMs()
        {
            return from a in repository.context.TemplateFormats
                   select new TemplateFormatPM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                   };
        }

        public TemplateFormatPM GetSigleTemplateFormatPM(string code)
        {
            return (from a in repository.context.TemplateFormats
                    where a.Code == code
                    select new TemplateFormatPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                    }).FirstOrDefault();
        }

        public IQueryable<TemplateFormatList> GetIQueryableEntityList(IQueryable<TemplateFormat> iQueryable)
        {
            IQueryable<TemplateFormatList> result = from entity in iQueryable
                                                    select new TemplateFormatList()
                                                    {
                                                        Name = entity.Name,
                                                        Code = entity.Code,
                                                        SearchFields = entity.SearchFields,
                                                    };
            return result;
        }
    }
}