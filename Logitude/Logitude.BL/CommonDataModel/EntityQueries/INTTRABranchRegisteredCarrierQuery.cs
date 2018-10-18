using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class INTTRABranchRegisteredCarrierQuery
    {
        INTTRABranchRegisteredCarrierRepository repository;
        public INTTRABranchRegisteredCarrierQuery(int tenant)
        {
            repository = new INTTRABranchRegisteredCarrierRepository(tenant);
        }
        public INTTRABranchRegisteredCarrierQuery(INTTRABranchRegisteredCarrierRepository myRepository)
        {
            repository = myRepository;
        }

        public IQueryable<INTTRABranchRegisteredCarrierPM> GetAllByTenant(int tenant)
        {
            IQueryable<INTTRABranchRegisteredCarrierPM> myResult = from a in repository.Context.INTTRABranchRegisteredCarriers
                                                                   where a.Tenant == tenant
                                                                   select new INTTRABranchRegisteredCarrierPM()
                                                                   {
                                                                       Id = a.Id,
                                                                       Tenant = a.Tenant,
                                                                       UpdateDate = a.UpdateDate,
                                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                                       BranchId = a.BranchId,
                                                                       ShippingLineId = a.ShippingLineId,
                                                                   };
            return myResult;
        }
    }
}
