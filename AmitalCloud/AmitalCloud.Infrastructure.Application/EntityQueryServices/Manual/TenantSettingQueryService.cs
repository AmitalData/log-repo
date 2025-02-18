using AmitalCloud.Infrastructure.Application.BaseClasses;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityDataMappings;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityKeys;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Application.EntityQueryServices
{
    public partial class TenantSettingQueryService : BaseEntityQueryService<IAmitalCloudContext, TenantSetting, TenantSettingKeys<string>, TenantSettingPM, TenantSettingList, string>
    {
        public TenantSettingQueryService(int tenant) : this(AmitalCloudContext.GetContext(tenant)) { }
        public TenantSettingQueryService(IAmitalCloudContext context) : base(new Repository<TenantSetting>(context), new TenantSettingDataMapping()) { }
        public TenantSettingPM GetSingle(string id, bool getComposition, bool getFromCache) => base.GetSingle(new TenantSettingKeys<string>() { Id = id }, getComposition, getFromCache);
        protected override IEntityKeyFields<TenantSetting, string> GetKeys(TenantSetting entityPOCO) => new TenantSettingKeys<string>() { Id = entityPOCO.Id, };
    }
}
