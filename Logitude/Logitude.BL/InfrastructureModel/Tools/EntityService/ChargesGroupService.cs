
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
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class ChargesGroupService
    {
        bool isNewEntity;
        private int tenant;
        public ChargesGroup Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ChargesGroupPM entityPM;
        private IWebFreightContext objectContext;
        private ChargesGroupRepository entityRepository;
        public ChargesGroupService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ChargesGroupRepository(objectContext);
        }

        public void Create(ChargesGroupPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ChargesGroup", tenant).ToString();
            this.Poco = new ChargesGroup();
            this.Poco.Id = this.entityPM.Id;

            ChargesGroupMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            //ContactRepository contactsRepository = new ContactRepository(0);
            //ContactQuery contactQuery = new ContactQuery(contactsRepository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), 0, true);
            //if (contact != null)
            //{
            //    EventTracer.CreateTraceEvent(new EventTracerArgs()
            //    {
            //        Tenant = 0,
            //        EventTypeCode = "CRCG",
            //        UserId = contact.Id,
            //        EntityId = Poco.Code,
            //        ObjectTableName = "ChargesGroup",
            //    });
            //}


        }

        public void Update(ChargesGroupPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleChargesGroup(theEntityPm.Id, theEntityPm.Tenant);
            ChargesGroupMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            //ContactRepository contactsRepository = new ContactRepository(0);
            //ContactQuery contactQuery = new ContactQuery(contactsRepository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), 0, true);
            //if (contact != null)
            //{
            //    EventTracer.CreateTraceEvent(new EventTracerArgs()
            //    {
            //        Tenant = 0,
            //        EventTypeCode = "UPCG",
            //        UserId = contact.Id,
            //        EntityId = Poco.Code,
            //        ObjectTableName = "ChargesGroup",
            //    });
            //}


        }

    }
}