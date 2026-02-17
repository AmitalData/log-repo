using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
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

            using (var scope = TransactionFactory.GetTransaction())
            {
                repo.FastDelete(GeneralKey, tenant);

                //_logger.AppendLine("add GeneralLock");
                repo.SubmitChanges();
                scope.Complete();
            }

            //poco = AddKeyAndLock(dcaAnalyzeAggregateKey);

        }
    }
}
