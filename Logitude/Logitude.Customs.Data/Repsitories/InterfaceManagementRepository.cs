using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class InterfaceManagementRepository : IRepository<InterfaceManagement>
    {

        public List<InterfaceManagement> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public InterfaceManagement GetSingleInterfaceManagement(EntityKeyFields entityKeys)
        {
            InterfaceManagementKeys keys = entityKeys as InterfaceManagementKeys;
            return (from a in context.InterfaceManagements.Include("InterfaceSendOption")
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }

        public InterfaceManagement GetSingleFromCache(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;
            string cacheKey = $"InterfaceManagement_{code}";
            InterfaceManagement interfaceManagement = (InterfaceManagement)CacheManager.CacheWrapper.Get(cacheKey, 0);
            if (interfaceManagement == null)
            {
                interfaceManagement = context.InterfaceManagements.FirstOrDefault(x => x.Code == code);
                if (interfaceManagement != null)
                {
                    CacheManager.CacheWrapper.Insert(cacheKey, interfaceManagement, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero, 0);
                }
            }
            return interfaceManagement;
        }
    }
}
