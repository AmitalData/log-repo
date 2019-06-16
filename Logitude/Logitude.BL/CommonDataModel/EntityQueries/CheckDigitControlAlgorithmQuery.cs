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
    public class CheckDigitControlAlgorithmQuery
    {
        CheckDigitControlAlgorithmRepository repository;

        public CheckDigitControlAlgorithmQuery()
        {
            repository = new CheckDigitControlAlgorithmRepository();
        }

        public CheckDigitControlAlgorithmQuery(int tenant)
        {
            repository = new CheckDigitControlAlgorithmRepository(tenant);
        }

        public CheckDigitControlAlgorithmQuery(CheckDigitControlAlgorithmRepository repository)
        {
            this.repository = repository;
        }

        public CheckDigitControlAlgorithmPM GetSinglePM(string code, int tenant = 0)
        {
            CheckDigitControlAlgorithm entityPoco = repository.GetSingleCheckDigitControlAlgorithm(code);
            CheckDigitControlAlgorithmPM entityPM = new CheckDigitControlAlgorithmPM()
            {
                Code = entityPoco.Code,
                Name = entityPoco.Name,
                SearchFields = entityPoco.SearchFields,
            };

            return entityPM;
        }


        public IQueryable<CheckDigitControlAlgorithmList> GetIQueryableEntityList(IQueryable<CheckDigitControlAlgorithm> iQueryable)
        {
            IQueryable<CheckDigitControlAlgorithmList> result
                = (from entity in iQueryable
                   select new CheckDigitControlAlgorithmList()
                   {
                       Name = entity.Name,
                       Code = entity.Code,
                       SearchFields = entity.SearchFields,
                   });


            return result;
        }
    }
}
