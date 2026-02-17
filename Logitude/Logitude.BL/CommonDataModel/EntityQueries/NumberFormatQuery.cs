using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class NumberFormatQuery
    {
        NumberFormatRepository repository;

        public NumberFormatQuery()
        {
            repository = new NumberFormatRepository();
        }

        public NumberFormatQuery(int tenant)
        {
            repository = new NumberFormatRepository(tenant);
        }

        public NumberFormatQuery(NumberFormatRepository communicationLogTypeRepository)
        {
            repository = communicationLogTypeRepository;
        }
        public NumberFormatPM GetSinglePM(string code)
        {
            return (from a in repository.context.NumberFormats
                    where a.Code == code
                    select new NumberFormatPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }
        public NumberFormatPM GetSingleNumberFormatPM(string code)
        {
            return (from a in repository.context.NumberFormats
                    where a.Code == code
                    select new NumberFormatPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<NumberFormatList> GetIQueryableEntityList(IQueryable<NumberFormat> iQueryable)
        {
            IQueryable<NumberFormatList> result = from entity in iQueryable
                                                          select new NumberFormatList()
                                                          {
                                                              Name = entity.Name,
                                                              Code = entity.Code,
                                                              SearchFields = entity.SearchFields,
                                                          };
            return result;
        }
    }
}
