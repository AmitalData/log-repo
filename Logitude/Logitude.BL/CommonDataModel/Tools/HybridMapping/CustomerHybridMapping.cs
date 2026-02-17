using Logitude.BL.CommonDataModel.EntityOtherServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.HybridMapping
{
    public class CustomerHybridMapping
    {
        public static CustomerPM MapEntityToHybrid(CustomerPM originalPM)
        {

            byte[] serializedEntity = LogitudeXmlSerializer.SerializeObject(originalPM);
            CustomerPM entityPM = LogitudeXmlSerializer.DeserializeObject<CustomerPM>(serializedEntity);

            ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            UserRepository userReporistory = new UserRepository(objectContext);
            CardRepository cardRepository = new CardRepository(objectContext);
            ContactRepository contactRepository = new ContactRepository(objectContext);
            CountryRepository countryRepository = new CountryRepository(objectContext);

            if (entityPM.CollectorId != null)
            {
                User user = userReporistory.GetSingleUser(entityPM.CollectorId, entityPM.Tenant, false);
                if (user != null)
                {
                    entityPM.CollectorId = user.Code;
                }
            }

            if (entityPM.ClassifierId != null)
            {
                User user = userReporistory.GetSingleUser(entityPM.ClassifierId, entityPM.Tenant, false);
                if (user != null)
                {
                    entityPM.ClassifierId = user.Code;
                }
            }

             if (entityPM.UpdatedByUserId != null)
            {
                User user = userReporistory.GetSingleUser(entityPM.UpdatedByUserId, entityPM.Tenant, false);
                if (user != null)
                {
                    entityPM.UpdatedByUserCode = user.Code;
                }
            }

            

            if (entityPM.FreelancerId != null)
            {
                User user = userReporistory.GetSingleUser(entityPM.FreelancerId, entityPM.Tenant, false);
                if (user != null)
                {
                    entityPM.FreelancerId = user.Code;
                }
            }

            if (entityPM.SalesmanUserId != null)
            {
                User user = userReporistory.GetSingleUser(entityPM.SalesmanUserId, entityPM.Tenant, false);
                if (user != null)
                {
                    entityPM.SalesmanUserId = user.Code;
                }
            }

            if (entityPM.AccountManagerUserId != null)
            {
                User user = userReporistory.GetSingleUser(entityPM.AccountManagerUserId, entityPM.Tenant, false);
                if (user != null)
                {
                    entityPM.AccountManagerUserId = user.Code;
                }
            }

            if (entityPM.ForwarderId != null)
            {
                Card card = cardRepository.GetSingleCard(entityPM.ForwarderId, entityPM.Tenant);
                if (card != null)
                {
                    entityPM.ForwarderId = card.Code;
                }
            }

            if (entityPM.CustomsAgentId != null)
            {
                Card card = cardRepository.GetSingleCard(entityPM.CustomsAgentId, entityPM.Tenant);
                if (card != null)
                {
                    entityPM.CustomsAgentId = card.Code;
                }
            }

            if (entityPM.MediatorId != null)
            {
                Card card = cardRepository.GetSingleCard(entityPM.MediatorId, entityPM.Tenant);
                if (card != null)
                {
                    entityPM.MediatorId = card.Code;
                }
            }

            if (entityPM.PrimaryContactId != null)
            {
                Contact contact = contactRepository.GetSingleContact(entityPM.PrimaryContactId, entityPM.Tenant);
                if (contact != null && !string.IsNullOrEmpty(contact.ExternalId))
                {
                    entityPM.PrimaryContactId = contact.ExternalId;
                }
            }

            foreach (CustomerSalesmanByProductPM customerSalesManByProduct in entityPM.CustomerSalesmanByProducts)
            {

                if (customerSalesManByProduct.SalesmanUserId != null)
                {
                    User user = userReporistory.GetSingleUser(customerSalesManByProduct.SalesmanUserId, entityPM.Tenant, false);
                    if (user != null)
                    {
                        customerSalesManByProduct.SalesmanUserId = user.Code;
                    }
                }
            }

            foreach (CustomerAccountManagerByProductPM accountmanager in entityPM.CustomerAccountManagerByProducts)
            {
                if (accountmanager.AccountManagerId != null)
                {
                    User user = userReporistory.GetSingleUser(accountmanager.AccountManagerId, entityPM.Tenant, false);
                    if (user != null)
                    {
                        accountmanager.AccountManagerId = user.Code;
                    }
                }
            }

            foreach (CustomerCustomsAgentByProductPM customsagent in entityPM.CustomerCustomsAgentByProducts)
            {
                if (customsagent.CustomsAgentId != null)
                {
                    Card card = cardRepository.GetSingleCard(customsagent.CustomsAgentId, entityPM.Tenant);
                    if (card != null)
                    {
                        customsagent.CustomsAgentId = card.Code;
                    }
                }
            }

            foreach (CustomerForwarderByProductPM forwarder in entityPM.CustomerForwarderByProducts)
            {
                if (forwarder.ForwarderId != null)
                {
                    Card card = cardRepository.GetSingleCard(forwarder.ForwarderId, entityPM.Tenant);
                    if (card != null)
                    {
                        forwarder.ForwarderId = card.Code;
                    }
                }
            }
            foreach (CustomerMediatorByProductPM mediator in entityPM.CustomerMediatorByProducts)
            {
                if (mediator.MediatorId != null)
                {
                    Card card = cardRepository.GetSingleCard(mediator.MediatorId, entityPM.Tenant);
                    if (card != null)
                    {
                        mediator.MediatorId = card.Code;
                    }
                }
            }

            TenantRepository tenantRepository = new TenantRepository(objectContext);
            ObjectTableRepository objecttableRepository = new ObjectTableRepository(entityPM.Tenant);
            Tenant currentTenant = tenantRepository.GetSingleTenant(entityPM.Tenant);
            if (!string.IsNullOrEmpty(currentTenant.DefaultQuestionnaireId))
            {
                CustomerWcfEntityService customerap = new CustomerWcfEntityService();
                ObjectTable table = objecttableRepository.GetObjectTableByName("Customer", 0, false);
                entityPM.QuestionnaireAnsewrsHtmlString = customerap.GetActivationQuestionnaireAnswers(currentTenant.DefaultQuestionnaireId, entityPM.Tenant, table.Id, entityPM.Id);
            }

            return entityPM;
        }

        public static Response MapEntityToLogitude(CustomerPM customerPM)
        {
            Response response = new Response();
            return response;
        }
    }
}
