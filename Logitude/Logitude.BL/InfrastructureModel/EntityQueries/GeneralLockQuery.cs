using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class GeneralLockQuery
	{
		GeneralLockRepository repository;
        public GeneralLockQuery()
        {
            repository = new GeneralLockRepository(); 
        }
        public GeneralLockQuery(int tenant)
        {
            repository = new GeneralLockRepository(tenant);
        }
        public GeneralLockQuery(GeneralLockRepository generalLockRepository)
        {
            repository = generalLockRepository;
        }
        

      

        public GeneralLockPM GetSingleGeneralLockNOWAIT(int tenant, string entityId, string objectTableId)
		{
            GeneralLockPM generalLockPM = null;
            GeneralLock generalLock = repository.GetSingleGeneralLockNOWAIT(tenant, entityId, objectTableId);
			UserRepository userRepository = new UserRepository(tenant);
			if (generalLock != null)
            {
                generalLockPM = new GeneralLockPM()
                {

                    CreatedAt = generalLock.CreatedAt,
                    Tenant = generalLock.Tenant,
                    GeneralKey = generalLock.GeneralKey,
                    EntityId1 = generalLock.EntityId1,
                    EntityId2 = generalLock.EntityId2,
                    ObjectTable1 = generalLock.ObjectTable1,
                    ObjectTable2 = generalLock.ObjectTable2,
                    UserId = generalLock.UserId,
                    SessionId = generalLock.SessionId,
                    UserName = userRepository.GetSingleUserById(generalLock.UserId)?.Code,


                };

				return generalLockPM;
            }
            return null;
        }
    }
}
