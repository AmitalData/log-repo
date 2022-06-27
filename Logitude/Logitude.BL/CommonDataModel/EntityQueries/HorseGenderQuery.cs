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
    public class HorseGenderQuery
    {
        HorseGenderRepository repository;

        public HorseGenderQuery(int tenant)
        {
            repository = new HorseGenderRepository(tenant);
        }

        public HorseGenderQuery(HorseGenderRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<HorseGenderList> GetIQueryableEntityList(IQueryable<HorseGender> iQueryable)
        {
            IQueryable<HorseGenderList> result = from entity in iQueryable
                                                    select new HorseGenderList()
                                                    {
                                                        Name = entity.Name,
                                                        Code = entity.Code,
                                                        SearchFields = entity.SearchFields,
                                                    };
            return result;
        }

        public HorseGenderList GetSingleListByCode(string code)
        {
            return (from a in repository.context.HorseGenders
                    where a.Code == code
                    select new HorseGenderList()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public HorseGenderPM GetSinglePM(string code)
        {
            return (from a in repository.context.HorseGenders
                    where a.Code == code
                    select new HorseGenderPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }
    }
}
