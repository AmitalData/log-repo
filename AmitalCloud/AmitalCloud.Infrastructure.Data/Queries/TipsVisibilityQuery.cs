using System.Collections.Generic;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Context;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class TipsVisibilityQuery
    {
        private readonly Repository<TipsVisibility> repository;

        public TipsVisibilityQuery(int tenant)
        {
            repository = new Repository<TipsVisibility>(AmitalCloudContext.GetContext(tenant));
        }

        public List<TipsVisibility> GetTipsVisibilities(int tenant, string userId)
        {
            return repository.GetMulti(a => a.Tenant == tenant && a.UserId == userId);
        }
    }
}