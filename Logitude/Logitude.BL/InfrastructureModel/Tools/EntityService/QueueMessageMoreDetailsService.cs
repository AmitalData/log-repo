using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class QueueMessageMoreDetailsService
    {
        bool isNewEntity;
        private int tenant;
        public QueueMessageMoreDetails Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QueueMessageMoreDetailsPM entityPM;
        private IWebFreightContext objectContext;
        private QueueMessageMoreDetailsRepository entityRepository;
        public QueueMessageMoreDetailsService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QueueMessageMoreDetailsRepository(objectContext);
        }

        public void Create(QueueMessageMoreDetailsPM theEntityPm)
        {
            
            this.entityPM = theEntityPm;
            this.Poco = new QueueMessageMoreDetails();
            QueueMessageMoreDetailsMapping.MapEntity(theEntityPm, Poco);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(QueueMessageMoreDetailsPM theEntityPm)
        {
             
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleQueueMessage(theEntityPm.Id);
            QueueMessageMoreDetailsMapping.MapEntity(theEntityPm, Poco);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        public void Delete(long Id)
        { 
            this.Poco = entityRepository.GetSingleQueueMessage(Id); 
            entityRepository.Remove(Poco);
            entityRepository.SubmitChanges();
        }

    }
}