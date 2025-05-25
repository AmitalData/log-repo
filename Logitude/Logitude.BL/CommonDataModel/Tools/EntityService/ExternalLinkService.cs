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

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class ExternalLinkService
    {
        const string tableName = "ExternalLink";        
        private int tenant;        
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

            ExternalLink Poco = entityRepository.GetSingleExternalLink(entityPM.Id, tenant);
            if(Poco == null)
                throw new ArgumentNullException(nameof(Poco), $"external link not found, id: {entityPM.Id}, tenant: {tenant}");

            TraceChangeOfDays(entityPM, Poco.ExpirationDate);
            ExternalLinkMapping.MapEntity(entityPM, Poco, false);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            string cacheKey = tableName + "_" + entityPM.Id;
            if (HttpContext.Current != null)
                CacheManager.CacheWrapper.Remove(cacheKey);
        }

        private void TraceChangeOfDays(ExternalLinkPM previousEntity, int nextDays)
        {
            if (previousEntity.ExpirationDate == nextDays) return;

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = previousEntity.Tenant,
                EventTypeCode = "UPEV",
                UserId = new LoggedContactUtil().GetLoggedContact(tenant)?.Id,
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
