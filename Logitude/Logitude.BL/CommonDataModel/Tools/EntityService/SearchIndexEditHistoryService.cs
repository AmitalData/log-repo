using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using System;
using Logitude.BL.Security;
using Newtonsoft.Json.Linq;

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
            entityPM.KeyVal = RemoveDollarIdProperty(entityPM.KeyVal);

            Poco = new SearchIndexEditHistory();
            SearchIndexEditHistoryMapping.MapEntity(theEntityPm, Poco, isNewEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(SearchIndexEditHistoryPM theEntityPm)
        {
            if (theEntityPm == null)
                throw new ArgumentNullException("theEntityPm", "theEntityPm cannot be null");
            if (string.IsNullOrEmpty(theEntityPm.Id))
                throw new ArgumentNullException("Id", "theEntityPm.Id cannot be null or empty");

            isNewEntity = false;
            entityPM = theEntityPm;
            Poco = entityRepository.GetSingleSearchIndexEditHistory(theEntityPm.Id, theEntityPm.Tenant);

            if (Poco == null)
                throw new InvalidOperationException($"Poco with Id {entityPM.Id} not found.");

            SearchIndexEditHistoryMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private static string RemoveDollarIdProperty(string json)
        {
            if (string.IsNullOrEmpty(json)) return json;

            try
            {
                JObject jsonObject = JObject.Parse(json);
                jsonObject.Property("$id")?.Remove();
                return jsonObject.ToString(Newtonsoft.Json.Formatting.None);
            }
            catch
            {
                return json;
            }
        }    
    }
}
