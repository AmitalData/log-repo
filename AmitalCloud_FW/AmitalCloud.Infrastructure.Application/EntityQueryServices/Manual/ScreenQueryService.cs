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
    public partial class ScreenQueryService : BaseEntityQueryService<IAmitalCloudContext, Screen, ScreenKeys<string>, ScreenPM, ScreenList, string>
    {
        public ScreenQueryService(int tenant) : this(AmitalCloudContext.GetContext(tenant)) { }
        public ScreenQueryService(IAmitalCloudContext context) : base(new Repository<Screen>(context), new ScreenDataMapping()) { }
        public ScreenPM GetSingle(string id, bool getComposition, bool getFromCache) => base.GetSingle(new ScreenKeys<string>() { Id = id }, getComposition, getFromCache);
        protected override IEntityKeyFields<Screen, string> GetKeys(Screen entityPOCO) => new ScreenKeys<string>() { Id = entityPOCO.Id, };
    }
}
