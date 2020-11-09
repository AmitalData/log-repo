using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Controllers.Integration.Factories;

namespace WebFreight.Web.Controllers.Integration.Shipment
{
    public class ShipmentIntegrationBuilder
    {
        private int tenant;
        private string loggedUserEmail;
        private ICommonDataContext commonContext;
        private ShipmentIntegrationVariables variables;
        public ShipmentIntegrationBuilder(int tenant, string loggedUserEmail)
        {
            this.tenant = tenant;
            this.loggedUserEmail = loggedUserEmail;
            this.variables = new ShipmentIntegrationVariables();
            this.commonContext = CommonDataContext.GetContext(tenant);
        }

        public ShipmentIntegrationVariables BuildVariables()
        {
            this.PrepaireLoggedUser();

            this.PrepaireLoggedTenant();

            this.PrepairePartners();

            return this.variables;
        }

        private void PrepaireLoggedUser()
        {
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
            UserRepository userRepository = new UserRepository(commonContext);
            UserQuery userQuery = new UserQuery(userRepository);
            variables.LoggedUserPM = userQuery.GetSinglePM(loggedContact.Id, tenant);

            //if (loggedContact == null)
            //{
            //    ContactRepository contactRepository = new ContactRepository(commonContext);
            //    ContactQuery contactQuery = new ContactQuery(contactRepository);
            //    loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
            //}
        }

        private void PrepaireLoggedTenant()
        {
            TenantRepository tenantRepository = new TenantRepository(commonContext);
            TenantQuery tenantQuery = new TenantQuery(tenantRepository);
            variables.TenantPM = tenantQuery.GetSinglePM(tenant);
        }

        private void PrepairePartners()
        {
            IntegrationPartnerFactory factory = new IntegrationPartnerFactory(tenant, null, commonContext);
            variables.Partners.Add(factory.PrepairePartner("CS", "Shipper"));
            variables.Partners.Add(factory.PrepairePartner("CS", "Consignee"));
            variables.Partners.Add(factory.PrepairePartner("AG", "Agent"));
        }
    }
}