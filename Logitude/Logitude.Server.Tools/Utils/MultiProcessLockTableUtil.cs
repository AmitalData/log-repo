using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Server.Tools.Utils
{
    public class MultiProcessLockTableUtil
    {
       


        public IDisposable LockItAndGetReleaseToken(int tenant, string key2Upsert, string requestLog)
        {
            if (Transaction.Current == null)
            {
                throw new Exception("MultiProcessLockTableUtil:4 use must be under Transaction");
            }
                ProcessLockReleaseToken processLockToken = null;
            var repo = new GeneralLockRepository(tenant);
            LogMessagingUtil.Instance.AppendLine("MultiProcessLockTableUtil: trylock<<<" + key2Upsert.ToString());
            var lockPoco = repo.GetSingleGeneralLockNOWAIT(key2Upsert, tenant);
            if (lockPoco == null)
            {
                using (var scope = TransactionFactory.GetNewTransaction())
                {
                    repo.Add(new GeneralLock()
                    {
                        Tenant = tenant,
                        GeneralKey = key2Upsert,
                        CreatedAt = TenantServerConfigration.GetCurrentDateTime(tenant)
                    });
                    LogMessagingUtil.Instance.AppendLine("MultiProcessLockTableUtil: LockItAndGetReleaseToken:ADD<<<" + key2Upsert.ToString());
                    repo.SubmitChanges();
                    scope.Complete();
                }
                LogMessagingUtil.Instance.AppendLine("MultiProcessLockTableUtil: trylock<<<" + key2Upsert.ToString());
                lockPoco = repo.GetSingleGeneralLockNOWAIT(key2Upsert, tenant);

            }

            if (lockPoco == null)
            {
                throw new Exception("MultiProcessLockTableUtil:lockPoco ==null");
            }





            var newLock = new ProccesLockData()
            {
                Tenant = tenant,
                MyKey = key2Upsert,
                MyLog = requestLog,
                InsertTime = DateTime.Now

            };
            processLockToken = new ProcessLockReleaseToken()
            {

                ProccesLockData = newLock
            };



            return processLockToken as IDisposable;
        }
        private void RealseKey(ProcessLockReleaseToken disposeProcessLockToken)
        {
            

                var repo = new GeneralLockRepository(disposeProcessLockToken.ProccesLockData.Tenant);

                //var repo = new GeneralLockRepository(_Tenant);
                repo.FastDelete(disposeProcessLockToken.ProccesLockData.MyKey, disposeProcessLockToken.ProccesLockData.Tenant);

                LogMessagingUtil.Instance.AppendLine("MultiProcessLockTableUtil:RealseKey:Removed>>>:" + disposeProcessLockToken.ToString());

            
        }
    
        public class ProcessLockReleaseToken : IDisposable
        {
            public ProccesLockData ProccesLockData { get; internal set; }

            public void Dispose()
            {
                var multiProcessLockTableUtil = new MultiProcessLockTableUtil();
                multiProcessLockTableUtil.RealseKey(this);
            }
        }
    }
}
