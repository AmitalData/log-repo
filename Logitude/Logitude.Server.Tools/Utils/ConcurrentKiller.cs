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
    }
}
