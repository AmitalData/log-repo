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
    public class AdvancedQueryFilterQueryService :  BaseEntityQueryService<IAmitalCloudContext, AdvancedQueryFilter, AdvancedQueryFilterKeys<string>, AdvancedQueryFilterPM, AdvancedQueryFilterList, string>
    {
        public AdvancedQueryFilterQueryService(int tenant) : this(AmitalCloudContext.GetContext(tenant)) { }
        public AdvancedQueryFilterQueryService(IAmitalCloudContext context) : base(new Repository<AdvancedQueryFilter>(context), new AdvancedQueryFilterDataMapping()) { }
        public AdvancedQueryFilterPM GetSingle(string id, bool getComposition, bool getFromCache) => base.GetSingle(new AdvancedQueryFilterKeys<string>() { Id = id }, getComposition, getFromCache);
        protected override IEntityKeyFields<AdvancedQueryFilter, string> GetKeys(AdvancedQueryFilter entityPOCO) => new AdvancedQueryFilterKeys<string>() { Id = entityPOCO.Id, };

    }
}
