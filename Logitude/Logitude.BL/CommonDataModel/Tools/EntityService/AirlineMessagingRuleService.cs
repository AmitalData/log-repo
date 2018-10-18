using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class AirlineMessagingRuleService
    {
         bool isNewEntity;
        private int tenant;
        private AirlineMessagingRulePM entityPM;
        private ICommonDataContext objectContext;
        private AirlineMessagingRuleRepository entityRepository;
        public AirlineMessagingRule Poco { get; set; }

        public AirlineMessagingRuleService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new AirlineMessagingRuleRepository(objectContext);
        }

        public void Create(AirlineMessagingRulePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("AirlineMessagingRule", tenant).ToString();
            this.Poco = new AirlineMessagingRule();
            this.Poco.Id = this.entityPM.Id;

            if (tenant == 0)
            {
                if (!string.IsNullOrEmpty(entityPM.AirlineId))
                {
                    AirlineRepository rep = new AirlineRepository(tenant);
                    Airline airline = rep.GetSingleAirline(entityPM.AirlineId, tenant);
                    if (airline != null)
                    {
                        airline.HasAdaptations = true;

                        rep.Update(airline);
                        rep.SubmitChanges();
                    }
                }
            }

            AirlineMessagingRuleMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(AirlineMessagingRulePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
            if (loggedContact != null)
            {
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            this.Poco = entityRepository.GetSingleAirlineMessagingRule(entityPM.Id, tenant);

            AirlineMessagingRuleMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
