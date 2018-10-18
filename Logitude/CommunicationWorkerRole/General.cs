using System;
using System.Linq;
using Logitude.SystemLogs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;





namespace CommunicationWorkerRole
{
    static class General
    {
        public static bool IsUpdating()
        {
            try
            {
                bool isUpgrading = false;
                if (CacheManager.CacheWrapper != null)
                {
                    string cachekey = "isUpgrading_Check";
                    if (CacheManager.CacheWrapper.Get(cachekey) == null)
                    {

                        IGlobalContext globalcontext = GlobalContext.GetContext();
                        isUpgrading = (from a in globalcontext.GlobalDBs
                                       select a).FirstOrDefault().IsUpgrading;

                        if (CacheManager.CacheWrapper.Get(cachekey) == null)
                        {
                            CacheManager.CacheWrapper.Insert(cachekey, isUpgrading, null, DateTime.UtcNow.AddSeconds(30), TimeSpan.Zero);
                        }

                    }
                    else
                    {
                        isUpgrading = (bool)CacheManager.CacheWrapper.Get(cachekey);
                    }
                }
                else
                {
                    IGlobalContext globalcontext = GlobalContext.GetContext();
                    isUpgrading = (from a in globalcontext.GlobalDBs
                                        select a).FirstOrDefault().IsUpgrading;
                }

                return isUpgrading;
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "General : IsUpdating() Method", null);
                return false;
            }
        }
    }
}
