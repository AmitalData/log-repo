using Confluent.Kafka;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
   public class ExternalLinkService
    {
        bool isNewEntity = false;
        private readonly int tenant;
        private const string CACHE_KEY_FORMAT = "ExternalLink_{0}";

        public ExternalLink Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            private set { objectContext = value; }
        }

        private ExternalLinkPM entityPM;
        private ICommonDataContext objectContext;
        private ExternalLinkRepository entityRepository;

        public ExternalLinkService(ICommonDataContext objectContext,int tenant)
        {
          
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ExternalLinkRepository(objectContext);
        }

        public void Create(ExternalLinkPM entityPM)
        {
            if (entityPM == null)
                throw new ArgumentNullException(nameof(entityPM));

            this.entityPM = entityPM;
            this.isNewEntity = true;

            this.entityPM.Id = IdCounter.GetNumber("ExternalLink", tenant).ToString();
            this.Poco = new ExternalLink();
            this.Poco.Id = this.entityPM.Id;

            ExternalLinkMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ExternalLinkPM entityPM)
        {            
            this.entityPM = entityPM;
            this.isNewEntity = false;
            this.Poco = entityRepository.GetSingleExternalLink(entityPM.Id);

            if (Poco == null)
                throw new InvalidOperationException($"Poco with Id {entityPM.Id} not found.");

            ExternalLinkMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            
            string cacheKey = string.Format(CACHE_KEY_FORMAT, entityPM.Id);
            if (HttpContext.Current != null)
                CacheManager.CacheWrapper.Remove(cacheKey);
        }
    }
}
