using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.DataMapping;
using Logitude.BL.QuoteModel.Tools.TraceEvents;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public class QuoteStageService
    {
        private int tenant;
        private bool isNewEntity;
        private Contact loggedContact;
        public QuoteStage entityPoco { get; set; }
        private QuoteStagePM entityPM;
        private IQuotesContext objectContext;
        private QuoteStageRepository entityRepository;
        public QuoteStageService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new QuoteStageRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(tenant);
            this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
        }

        public void Create(QuoteStagePM entity)
        {
            this.isNewEntity = true;
            this.entityPM = entity;
            this.entityPM.Id = IdCounter.GetNumber("QuoteStage", tenant).ToString();
            this.entityPoco = new QuoteStage()
            {
                Id = entityPM.Id,
                Tenant = entityPM.Tenant
            };

            //this.InitializeComponent();
            QuoteTracing.Trace(entityPM, entityPoco, isNewEntity);
            QuoteMapping.MapEntity(entityPM, entityPoco, isNewEntity);
            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();
        }

        public void Update(QuoteStagePM entity)
        {
            this.isNewEntity = false;
            this.entityPM = entity;
            this.entityPoco = entityRepository.GetSingleQuoteStage(entityPM.Id, entityPM.Tenant);
            
            this.InitializeComponent();
            QuoteTracing.Trace(entityPM, entityPoco, isNewEntity);
            QuoteMapping.MapEntity(entityPM, entityPoco, isNewEntity);
            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }

        private void InitializeComponent()
        {
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.UpdatedByUserId = loggedContact.Id;
        }
    }
}