using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class OwnerUnifreightUserService
    {
        public string GetOwnerUnifreightUserCode(DeclarationPM declaration)
        {

            var userRepository = new UserRepository(declaration.Tenant);
            var storageSiteCode = declaration.Consignments.FirstOrDefault()?.StorageSiteCode;
            if (!String.IsNullOrWhiteSpace(storageSiteCode))
            {
                var user = userRepository.GetSingleUserByCode(storageSiteCode, declaration.Tenant, true);
                if (user != null)
                {
                    return storageSiteCode;
                }

            }
            return FUOwnerUnifreightUserCode.MEHES;
        }
    }
}
