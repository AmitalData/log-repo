using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.Tools.Validating;
using Logitude.BL.InvoiceModel.Tools.TraceEvents;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class AccountService
    {
        bool isNewEntity;
        private int tenant;
        public Account Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private AccountPM entityPM;
        private IInvoiceContext objectContext;
        private AccountRepository entityRepository;
        public AccountService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AccountRepository(objectContext);
        }

        public void Create(AccountPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("Account", tenant).ToString();
            this.Poco = new Account();
            this.Poco.Id = this.entityPM.Id;

           
          //  MapAccountAccountPM(entityPM, newEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            ////create TraceEvent
            //WebFreightDomainService webfreightService = new WebFreightDomainService();
            //ContactRepository contactsRepository = new ContactRepository(entityPM.Tenant);
            //ContactQuery contactQuery = new ContactQuery(contactsRepository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant, true);
            //if (contact != null)
            //{
            //    EventTracer.CreateTraceEvent(new TraceEvent(), "CRAC", entityPM.Tenant, contact.Id, entityPM.Id, null, "Account", null, null, false);
            //}


            //AccountValidator.Validate(theEntityPm);
            //AccountTracing.Trace(theEntityPm, Poco, isNewEntity);
            //AccountMapping.MapEntity(theEntityPm, Poco, isNewEntity);
           

        }

        public void Update(AccountPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleAccount(theEntityPm.Id , theEntityPm.Tenant );

           
          //  MapAccountAccountPM(currentEntityPM, updatedEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            //WebFreightDomainService webfreightService = new WebFreightDomainService();
            //ContactRepository contactsRepository = new ContactRepository(currentEntityPM.Tenant);
            //ContactQuery contactQuery = new ContactQuery(contactsRepository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), currentEntityPM.Tenant, true);
            //if (contact != null) EventTracer.CreateTraceEvent(new TraceEvent(), "UPAC", currentEntityPM.Tenant, contact.Id, currentEntityPM.Id, null, "Account", null, null, false);
         
            

            //AccountValidator.Validate(theEntityPm);
            //AccountTracing.Trace(theEntityPm, Poco, isNewEntity);
            //AccountMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            
        }
    }
}