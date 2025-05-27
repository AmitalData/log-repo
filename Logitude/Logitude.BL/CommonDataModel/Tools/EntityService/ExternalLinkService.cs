using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Web;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class ExternalLinkService
    {
        private readonly int tenant;
        private const string CACHE_KEY_FORMAT = "ExternalLink_{0}";
        const string tableName = "ExternalLink";
        private ExternalLinkRepository entityRepository;
        public readonly ICommonDataContext ObjectContext;

        public ExternalLinkService(ICommonDataContext objectContext, int tenant)
        {
            if (objectContext == null)
                throw new ArgumentNullException(nameof(objectContext), "objectContext can not be null");

            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ExternalLinkRepository(objectContext);
        }

        public void Create(ExternalLinkPM entityPM)
        {
            if (entityPM == null)
                throw new ArgumentNullException(nameof(entityPM), "entity can not be null");
            
            entityPM.Id = IdCounter.GetNumber(tableName, tenant).ToString();
            ExternalLink Poco = new ExternalLink();
            Poco.Id = entityPM.Id;

            ExternalLinkMapping.MapEntity(entityPM, Poco, true);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ExternalLinkPM entityPM)
        {            
            if (entityPM == null)
                throw new ArgumentNullException(nameof(entityPM), "entity can not be null");

            if (string.IsNullOrEmpty(entityPM.Id))
                throw new ArgumentNullException(nameof(entityPM), "id can not be empty or null");

            if (NeededCreate(entityPM, out int tenant))
            {
                entityPM.Tenant = tenant;
                Create(entityPM);
                return;
            }

            ExternalLink Poco = entityRepository.GetSingleExternalLink(entityPM.Id, entityPM.Tenant);
            if(Poco == null)
                throw new ArgumentNullException(nameof(Poco), $"external link not found, id: {entityPM.Id}, tenant: {entityPM.Tenant}");

            TraceChangeOfDays(Poco, entityPM.ExpirationDate);
            ExternalLinkMapping.MapEntity(entityPM, Poco, false);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            
            string cacheKey = string.Format(CACHE_KEY_FORMAT, entityPM.Id);
            if (HttpContext.Current != null)
                CacheManager.CacheWrapper.Remove(cacheKey);
        }

        private void TraceChangeOfDays(ExternalLink previousEntity, int nextDays)
        {
            if (previousEntity.ExpirationDate == nextDays) return;

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = previousEntity.Tenant,
                EventTypeCode = "UPEV",
                UserId = new LoggedContactUtil().GetLoggedContact(previousEntity.Tenant)?.Id,
                EntityId = previousEntity.Id,
                ObjectTableName = nameof(ExternalLink),
                Notes = $"Expiration days changed from {previousEntity.ExpirationDate} to {nextDays} days",
            });
        }

        private bool NeededCreate(ExternalLinkPM entityPM, out int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            tenant = 0;

            if (authToken.Tenant == 0 || entityPM.Tenant != 0)
                return false;

            bool notExistsInTenant = ObjectContext.ExternalLinks.All(x => x.Tenant != authToken.Tenant || x.Ref != entityPM.Ref);
            tenant = authToken.Tenant;
            if (notExistsInTenant)
                return true;
            else
                throw new ArgumentException("tenant that save is not tenant of entity", nameof(tenant));
        }
    }
}
