using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityQueries
{

    public class BluesnapContractTypeQuery
    {
        BluesnapContractTypeRepository repository;
        public BluesnapContractTypeQuery()
        {
            repository = new BluesnapContractTypeRepository();
        }

        public BluesnapContractTypeQuery(int tenant)
        {
            repository = new BluesnapContractTypeRepository(tenant);
        }

        public BluesnapContractTypeQuery(BluesnapContractTypeRepository quoteQuery)
        {
            repository = quoteQuery;
        }

        public BluesnapContractTypePM GetSingleQuoteTypePM(string code)
        {
            return (from a in repository.context.BluesnapContractTypes
                    where a.Code == code
                    select new BluesnapContractTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public BluesnapContractTypePM GetSinglePM(string code)
        {
            return (from a in repository.context.BluesnapContractTypes
                    where a.Code == code
                    select new BluesnapContractTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public BluesnapContractTypePM GetSinglePMByCode(string code, int tenant)
        {
            return (from a in repository.context.BluesnapContractTypes
                    where a.Code == code
                    select new BluesnapContractTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }


        public BluesnapContractTypePM GetSinglePM(string code, int tenant)
        {
            return (from a in repository.context.BluesnapContractTypes
                    where a.Code == code
                    select new BluesnapContractTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }




        public IQueryable<BluesnapContractTypeList> GetIQueryableEntityList(IQueryable<BluesnapContractType> iQueryable)
        {
            IQueryable<BluesnapContractTypeList> result = from entity in iQueryable
                                               select new BluesnapContractTypeList()
                                               {
                                                   Name = entity.Name,
                                                   Code = entity.Code,
                                                   SearchFields = entity.SearchFields,
                                               };
            return result;
        }
    }

}
