using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Simplog.Data.Helpers;
using Logitude.BL.QuoteModel.Tools.DataMapping;
using Logitude.BL.QuoteModel.Tools.TraceEvents;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public class QuoteClosingReasonService
    {
        private int tenant;
        private bool isNewEntity;
        private Contact loggedContact;
        public QuoteClosingReason entityPoco { get; set; }
        private QuoteClosingReasonPM entityPM;
        private IQuotesContext objectContext;
        private QuoteClosingReasonRepository entityRepository;
        public QuoteClosingReasonService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new QuoteClosingReasonRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(tenant);
            this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
        }

        public void Create(QuoteClosingReasonPM entity)
        {
            this.isNewEntity = true;
            this.entityPM = entity;
            this.entityPM.Id = IdCounter.GetNumber("QuoteClosingReason", tenant).ToString();
            this.entityPoco = new QuoteClosingReason()
            {
                Id = entityPM.Id,
                Tenant = entityPM.Tenant
            };

            entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.CreatedByUserId = loggedContact.Id;
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.UpdatedByUserId = loggedContact.Id;

            this.ValidateCode(entityPM, true);

            QuoteTracing.Trace(entityPM, entityPoco, isNewEntity);
            QuoteMapping.MapEntity(entityPM, entityPoco, isNewEntity);
            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();
        }

        public void Update(QuoteClosingReasonPM entity)
        {
            this.isNewEntity = false;
            this.entityPM = entity;
            this.entityPoco = entityRepository.GetSingleQuoteClosingReason(entityPM.Id, entityPM.Tenant);

            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.UpdatedByUserId = loggedContact.Id;

            this.ValidateCode(entityPM, false);

            QuoteTracing.Trace(entityPM, entityPoco, isNewEntity);
            QuoteMapping.MapEntity(entityPM, entityPoco, isNewEntity);
            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }

        private void ValidateCode(QuoteClosingReasonPM entityPM, bool isNewEntity)
        {
            bool exist = false;
            if (isNewEntity)
            {
                exist = (from a in objectContext.QuoteClosingReasons
                         where a.Code.ToLower() == entityPM.Code.ToLower() && a.Tenant == entityPM.Tenant
                         select a).Any();
            }

            else
            {
                exist = (from a in objectContext.QuoteClosingReasons
                         where a.Code.ToLower() == entityPM.Code.ToLower()
                         && a.Id != entityPM.Id
                         && a.Tenant == entityPM.Tenant
                         select a).Any();
            }

            if (exist)
            {
                throw new ApplicationException("Code already exists");
            }
        }
    }
}
