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
    public class HTSCodeQuery
    {
        HTSCodeRepository repository;

        public HTSCodeQuery()
        {
            this.repository = new HTSCodeRepository();
        }

        public HTSCodeQuery(int tenant)
        {
            this.repository = new HTSCodeRepository(tenant);
        }

        public HTSCodeQuery(HTSCodeRepository repository)
        {
            this.repository = repository;
        }

        public HTSCodePM GetSinglePM(string id, int tenant)
        {
            HTSCodePM result = null;
            HTSCode entityPoco = repository.GetSingleHTSCode(id, tenant);

            if (entityPoco != null)
            {
                result = new HTSCodePM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    Code = entityPoco.Code,
                    ItemId = entityPoco.ItemId,
                    DestinationCountryId = entityPoco.DestinationCountryId,
                    ApprovedByCustomer = entityPoco.ApprovedByCustomer,
                    InActive = entityPoco.InActive,
                };
            }

            return result;
        }

        public IQueryable<HTSCodeList> GetIQueryableEntityList(IQueryable<HTSCode> iQueryable)
        {
            IQueryable<HTSCodeList> result = from entity in iQueryable
                                               select new HTSCodeList()
                                               {
                                                   Id = entity.Id,
                                                   Tenant = entity.Tenant,
                                                   Code = entity.Code,
                                                   ItemId = entity.ItemId,
                                                   DestinationCountryId = entity.DestinationCountryId,
                                                   ApprovedByCustomer = entity.ApprovedByCustomer,
                                                   InActive = entity.InActive,
                                               };
            return result;
        }
    }
}
