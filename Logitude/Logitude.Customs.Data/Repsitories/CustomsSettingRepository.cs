
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomsSettingRepository:IRepository<CustomsSetting>
   {
        
		public List<CustomsSetting> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public CustomsSetting GetSettingByTenant(int tenant)
        {
            return (from a in context.CustomsSettings
                    select a).FirstOrDefault();
        }

        public CustomsSetting GetTenantByCustomsAgentId(string customsAgentId)
        {
            return (from a in context.CustomsSettings
                    where a.CustomsAgentId == customsAgentId
                    select a).FirstOrDefault();
        }
        public IQueryable<CustomsSetting> GetRealAll()
        {
            return from a in context.CustomsSettings
                   select a;
        }

        public bool AnyCourierTenant()
        {
            return this.context.CustomsSettings.Any(r => r.CompanyType == "B");
        }
        public AmitalRestrictOwnerModel GetMyAmitalRestrictOwnerModel(bool getFromCache, int tenant = 1, string UnifreightUserId = null)
        {
            var res = InjectionUtil.Instance.GetAmitalRestrictOwnerModel(getFromCache, tenant, UnifreightUserId);
            return res;
        }

        public CustomsSetting GetSettingByTenantCache(int tenant)
        {
            string key = $"RepoGetSettingByTenant{tenant}";
            var res = CacheManager.GetOrInsertNewObject<CustomsSetting>(key,
                () =>
            {
                var repo = new CustomsSettingRepository(tenant);
                return repo.GetSettingByTenant(tenant);

            });
            return res;
            
        }

    }

}
   