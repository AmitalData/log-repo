using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using System;
using Logitude.BL.Security;
using System.Collections.Generic;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class SearchIndexEditHistoryService
    {
        bool isNewEntity;
        private readonly int tenant;
        public SearchIndexEditHistory Poco { get; set; }
        public ICommonDataContext ObjectContext => objectContext;
        private SearchIndexEditHistoryPM entityPM;
        private readonly ICommonDataContext objectContext;
        private readonly SearchIndexEditHistoryRepository entityRepository;

        public SearchIndexEditHistoryService(int tenant)
        {
            this.tenant = tenant;
            objectContext = CommonDataContext.GetContext(tenant);
            entityRepository = new SearchIndexEditHistoryRepository(objectContext);
        }

        public SearchIndexEditHistoryService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            entityRepository = new SearchIndexEditHistoryRepository(objectContext);
        }

        public void Create(SearchIndexEditHistoryPM theEntityPm)
        {
            if (theEntityPm == null)
                throw new ArgumentNullException("theEntityPm", "theEntityPm cannot be null");

            isNewEntity = true;
            entityPM = theEntityPm;
            entityPM.Id = IdCounter.GetNumber("SearchIndexEditHistory", tenant).ToString();
            entityPM.CreateDate = DateTime.Now;
            entityPM.CreatedByUserId = new LoggedContactUtil().GetLoggedContact(tenant)?.Id;
            entityPM.Tenant = tenant;
            
            Poco = new SearchIndexEditHistory();
            Poco.Id = this.entityPM.Id;

            SearchIndexEditHistoryMapping.MapEntity(theEntityPm, Poco, isNewEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(SearchIndexEditHistoryPM theEntityPm)
        {
            if (theEntityPm == null)
                throw new ArgumentNullException("theEntityPm", "theEntityPm cannot be null");

            isNewEntity = false;
            entityPM = theEntityPm;
            Poco = entityRepository.GetSingleSearchIndexEditHistory(theEntityPm.Tenant, theEntityPm.Id);

            if (Poco == null)
                throw new InvalidOperationException($"Poco with Id {entityPM.Id} not found.");

            SearchIndexEditHistoryMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        public List<string> GetRecent(int tenant, string screen, string entname, string userId, int size = 50)
        {
            if (tenant == null)
                throw new ArgumentNullException("tenant", "tenant cannot be null");
            if (string.IsNullOrEmpty(screen))
                throw new ArgumentNullException("screen", "screen cannot be null or empty");
            if (string.IsNullOrEmpty(entname))
                throw new ArgumentNullException("entname", "entname cannot be null or empty");
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException("userId", "userId cannot be null or empty");

            return entityRepository.GetRecent(tenant, screen, entname, userId, size);
        }
    }
}
