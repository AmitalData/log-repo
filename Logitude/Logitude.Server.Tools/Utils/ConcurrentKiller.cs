using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Utils
{
    public class ConcurrentKiller
    {

        public void LockOrCrashOnCommitDueUnique(string GeneralKey, int tenant)
        {
            var repo = new GeneralLockRepository(tenant);
            LogMessagingUtil.Instance.AppendLine(" ** try lock, LockOrCrashOnCommitDueUnique, key: " + GeneralKey);

            using (var scope = TransactionFactory.GetTransaction())
            {
                repo.Add(new GeneralLock()
                {
                    Tenant = tenant,
                    GeneralKey = GeneralKey,
                    CreatedAt = TenantServerConfigration.GetCurrentDateTime(tenant)
                });
                //_logger.AppendLine("add GeneralLock");
                repo.SubmitChanges();
                scope.Complete();
            }

            //poco = AddKeyAndLock(dcaAnalyzeAggregateKey);


        }

        public void FreeLock(string GeneralKey, int tenant)
        {
            var repo = new GeneralLockRepository(tenant);
            LogMessagingUtil.Instance.AppendLine(" ** Rlease lock, FreeLock, key: " + GeneralKey);

            using (var scope = TransactionFactory.GetTransaction())
            {
                repo.FastDelete(GeneralKey, tenant);

                //_logger.AppendLine("add GeneralLock");
                repo.SubmitChanges();
                scope.Complete();
            }

            //poco = AddKeyAndLock(dcaAnalyzeAggregateKey);

        }

        public void FreeLockIfCreated15MinOld(string cRSKey, int tenant)
        {
            var repo = new GeneralLockRepository(tenant);
            LogMessagingUtil.Instance.AppendLine(" ** Rlease old lock, FreeLockIfCreated15MinOld, key: " + cRSKey);

            using (var scope = TransactionFactory.GetTransaction())
            {
                repo.FastDeleteIfCreated15MinOld(cRSKey, tenant);

                //_logger.AppendLine("add GeneralLock");
                repo.SubmitChanges();
                scope.Complete();
            }
        }



		public void LockByobjectAndUser(int tenant, string userId, string entityId1, string objectTableId1, string entityId2, string objectTableId2,string sessionId)
		{
			var repo = new GeneralLockRepository(tenant);
          
			using (var scope = TransactionFactory.GetTransaction())
			{
				repo.Add(new GeneralLock()
				{
					Tenant = tenant,
					GeneralKey = Guid.NewGuid().ToString(),
					CreatedAt = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UserId = userId,
                    EntityId1 = entityId1,
					ObjectTableId1 = objectTableId1,
                    EntityId2 = entityId2,
					ObjectTableId2 = objectTableId2,
					SessionId = sessionId

				});
				repo.SubmitChanges();
				scope.Complete();
			}
		}

		public void FreeGeneralLock( int tenant, string entityId1, string objectTableId1, string sessionId)
		{
			var repo = new GeneralLockRepository(tenant);

			using (var scope = TransactionFactory.GetTransaction())
			{
				repo.FastDeleteGeneralLock(tenant, entityId1, objectTableId1, sessionId);

				repo.SubmitChanges();
				scope.Complete();
			}
        }
	}
}
