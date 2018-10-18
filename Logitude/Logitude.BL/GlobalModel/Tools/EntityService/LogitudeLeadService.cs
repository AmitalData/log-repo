using Logitude.Server.Tools.Counters;
using Simplog.Data.QuoteModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.Tools.DataMapping;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    public class LogitudeLeadService
    {
        bool isNewEntity;
        private int tenant;
        public LogitudeLead Poco { get; set; }

        public IGlobalContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private LogitudeLeadPM entityPm;
        private IGlobalContext objectContext;
        private LogitudeLeadRepository entityRepository;

        public LogitudeLeadService(IGlobalContext objectContext)
        {
         
            this.ObjectContext = objectContext;
            this.entityRepository = new LogitudeLeadRepository(objectContext);
        }

        public void Create(LogitudeLeadPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = Guid.NewGuid().ToString();
            this.Poco = new LogitudeLead();
            this.Poco.Id = this.entityPm.Id;

            LogitudeLeadMapping.MappingLogitudeLead(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(LogitudeLeadPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
     
            this.Poco = entityRepository.GetSingleLogitudeLead(entityPM.Id);

            string entityName = "LogitudeLead" + entityPM.Id;
            string entityPmName = "LogitudeLeadPM" + entityPM.Id;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            LogitudeLeadMapping.MappingLogitudeLead(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
