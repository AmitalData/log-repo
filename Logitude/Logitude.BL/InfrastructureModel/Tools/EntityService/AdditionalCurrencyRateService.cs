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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class AdditionalCurrencyRateService
    {
        bool isNewEntity;
        private int tenant;
        public AdditionalCurrencyRate Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private AdditionalCurrencyRatePM entityPM;
        private IWebFreightContext objectContext;
        private AdditionalCurrencyRateRepository entityRepository;
        public AdditionalCurrencyRateService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AdditionalCurrencyRateRepository(objectContext);
        }
        public void Create(AdditionalCurrencyRatePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("AdditionalCurrencyRate", tenant).ToString();
            this.Poco = new AdditionalCurrencyRate();
            this.Poco.Id = this.entityPM.Id;

            this.SetUpdatedByUser();

            AdditionalCurrencyRateValidating.Validate(theEntityPm);
            AdditionalCurrencyRateTracing.Trace(theEntityPm, Poco, isNewEntity);
            AdditionalCurrencyRateMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(AdditionalCurrencyRatePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingle(theEntityPm.Id, theEntityPm.Tenant);
            this.SetUpdatedByUser();

            AdditionalCurrencyRateValidating.Validate(theEntityPm);
            AdditionalCurrencyRateTracing.Trace(theEntityPm, Poco, isNewEntity);
            AdditionalCurrencyRateMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private void SetUpdatedByUser()
        {
            var loggedUser = GetLoggedUser();
            if (isNewEntity)
            {
                this.entityPM.CreatedByUserId = loggedUser != null ? loggedUser.Contact.Id : null;
                this.entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            }
            this.entityPM.UpdatedByUserId = loggedUser != null ? loggedUser.Contact.Id : null;
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
        }
        private User GetLoggedUser()
        {
            string email = AuthenticationUtil.GetLoggedUserEmail(tenant);
            UserRepository userRepository = new UserRepository(tenant);
            var loggedUser = userRepository.GetSingleUserByEmail(email, tenant, true);
            return loggedUser;
        }
    }
}
