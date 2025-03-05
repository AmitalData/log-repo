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
    public class ScreenFieldQueryService : BaseEntityQueryService<IAmitalCloudContext, ScreenField, ScreenFieldKeys<string>, ScreenFieldPM, ScreenFieldList, string>
    {
        public ScreenFieldQueryService(int tenant) : this(AmitalCloudContext.GetContext(tenant)) { }
        public ScreenFieldQueryService(IAmitalCloudContext context) : base(new Repository<ScreenField>(context), new ScreenFieldDataMapping()) { }
        public ScreenFieldPM GetSingle(string id, bool getComposition, bool getFromCache) => base.GetSingle(new ScreenFieldKeys<string>() { Id = id }, getComposition, getFromCache);
        protected override IEntityKeyFields<ScreenField, string> GetKeys(ScreenField entityPOCO) => new ScreenFieldKeys<string>() { Id = entityPOCO.Id, };

    }

}
