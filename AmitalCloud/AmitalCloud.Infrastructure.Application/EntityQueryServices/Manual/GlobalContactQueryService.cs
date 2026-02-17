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

    public class GlobalContactQueryService : BaseEntityQueryService<IAmitalCloudContext, GlobalContact, GlobalContactKeys<string>, GlobalContactPM, GlobalContactList, string>
    {
        public GlobalContactQueryService(int tenant) : this(GlobalContext.GetContext(tenant)) { }
        public GlobalContactQueryService(IGlobalContext context) : base(new Repository<GlobalContact>(context), new GlobalContactDataMapping()) { }
        public GlobalContactPM GetSingle(string id, bool getComposition, bool getFromCache) => base.GetSingle(new GlobalContactKeys<string>() { Id = id }, getComposition, getFromCache);
        protected override IEntityKeyFields<GlobalContact, string> GetKeys(GlobalContact entityPOCO) => new GlobalContactKeys<string>() { Id = entityPOCO.Id, };

    }

}
