using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.IntegrationTestModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.IntegrationTestModel.EntityQueries
{
    public class PartnersPreparationVariables
    {
        PartnersVariables Variables = new PartnersVariables();
        ICommonDataContext commonDataContext;

        int tenant;

        public PartnersPreparationVariables(int tenant)
        {
            this.tenant = tenant;
            commonDataContext = CommonDataContext.GetContext(tenant);

        }

        public PartnersVariables GetBasePartnersVaribles()
        {

            Variables.VendorId = GetVendor("TestVendor");
            Variables.AgentId = GetAgent("IntegrationAgent");
            Variables.ShipperExport1 = GetCustomer("ShipperExport1");
            Variables.CustomAgentId = GetCustomsAgent("InegrationCustomsAgent");
            Variables.ShippingAgentId = GetShippingAgent("IntegrationShippingAgent");
            Variables.CustomerId = GetCustomer("IntegrationCustomer");

            return Variables;
        }

        private string GetVendor(string vendorName)
        {
            VendorRepository vendorRepository = new VendorRepository(commonDataContext);
            Vendor vendor = vendorRepository.GetFirstSingleByName(vendorName, tenant);
            if (vendor == null)
            {
                InsertNewVendor(vendorName);
                vendor = vendorRepository.GetFirstSingleByName(vendorName, tenant);
            }
            //IQueryable<Vendor> iqueryable = vendorRepository.GetVendors( tenant);
            //Vendor vendor = iqueryable.Where(d => d.Card.EnglishName == vendorName).FirstOrDefault();

            return vendor.Id;
        }
        private void InsertNewVendor(string vendorName)
        {
            VendorService vendorService = new VendorService(commonDataContext, tenant);
            vendorService.Create(CreateVendorPM(vendorName));
        }
        public VendorPM CreateVendorPM(string vendorName)
        {
            VendorPM vendorPM = new VendorPM();
            vendorPM.Tenant = tenant;
            vendorPM.EnglishName = vendorName;
            vendorPM.CityName = "AKD";
            vendorPM.PartnerTypeId = "VD";
            vendorPM.CountryId = Variables.CountryUSId;
            vendorPM.Addresses.Add(Address("M", vendorName));
            return vendorPM;
        }
        public AddressPM Address(string addressTypeId, string partnerName)
        {
            AddressPM address = new AddressPM();
            address.Tenant = tenant;
            address.AddressTypeId = addressTypeId;
            address.Description = "Main Address";
            address.Name = partnerName;
            address.City = "XSD";
            address.CountryId = Variables.CountryGBId;
            address.IsCreatedWithPartner = true;

            return address;
        }
        private string GetAgent(string agentName)
        {
            AgentRepository agentRepository = new AgentRepository(commonDataContext);
            Agent agent = agentRepository.GetFirstSingleByName(agentName, tenant);
            if (agent == null)
            {
                InsertNewAgent(agentName);
                agent = agentRepository.GetFirstSingleByName(agentName, tenant);
            }
            return agent.Id;
        }
        private void InsertNewAgent(string agentName)
        {
            AgentService agentService = new AgentService(commonDataContext, tenant);
            agentService.Create(CreateAgentPM(agentName));
        }

        public AgentPM CreateAgentPM(string agentName)
        {
            AgentPM agentPM = new AgentPM();
            agentPM.Tenant = tenant;
            agentPM.EnglishName = agentName;
            agentPM.CityName = "AKD";
            agentPM.PartnerTypeId = "AG";
            agentPM.CountryId = Variables.CountryUSId;
            agentPM.Addresses.Add(Address("M", agentName));
            return agentPM;
        }
        private string GetCustomer(string customerName)
        {
            CustomerRepository customerRepository = new CustomerRepository(commonDataContext);
            Customer customer = customerRepository.GetFirstSingleByName(customerName, tenant);
            if (customer == null)
            {
                InsertNewCustomer(customerName);
                customer = customerRepository.GetFirstSingleByName(customerName, tenant);
            }
            return customer.Id;
        }
        private void InsertNewCustomer(string customerName)
        {
            CustomerService customerService = new CustomerService(commonDataContext, CreateCustomertPM(customerName));
            customerService.Create();
        }

        public CustomerPM CreateCustomertPM(string agentName)
        {
            CustomerPM customerPM = new CustomerPM();
            customerPM.Tenant = tenant;
            customerPM.EnglishName = agentName;
            customerPM.CityName = "AKD";
            customerPM.PartnerTypeId = "CS";
            customerPM.CountryId = Variables.CountryUSId;
            customerPM.Addresses.Add(Address("M", agentName));
            return customerPM;
        }
        private string GetCustomsAgent(string customAgentName)
        {
            CustomAgentRepository customAgentRepository = new CustomAgentRepository(commonDataContext);
            CustomAgent customAgent = customAgentRepository.GetFirstSingleByName(customAgentName, tenant);
            if (customAgent == null)
            {
                InsertNewCustomAgent(customAgentName);
                customAgent = customAgentRepository.GetFirstSingleByName(customAgentName, tenant);
            }
            return customAgent.Id;
        }
        private void InsertNewCustomAgent(string customAgentName)
        {
            CustomAgentService customAgentService = new CustomAgentService(commonDataContext, tenant);
            customAgentService.Create(CreateCustomAgentPM(customAgentName));
        }

        public CustomAgentPM CreateCustomAgentPM(string customAgentName)
        {
            CustomAgentPM customAgentPM = new CustomAgentPM();
            customAgentPM.Tenant = tenant;
            customAgentPM.EnglishName = customAgentName;
            customAgentPM.CityName = "AKD";
            customAgentPM.PartnerTypeId = "CG";
            customAgentPM.CountryId = Variables.CountryUSId;
            customAgentPM.Addresses.Add(Address("M", customAgentName));
            return customAgentPM;
        }

        private string GetShippingAgent(string shippingAgentName)
        {
            ShippingAgentRepository shippingAgentRepository = new ShippingAgentRepository(commonDataContext);
            ShippingAgent shippingAgent = shippingAgentRepository.GetFirstSingleByName(shippingAgentName, tenant);
            if (shippingAgent == null)
            {
                InsertNewShippingAgent(shippingAgentName);
                shippingAgent = shippingAgentRepository.GetFirstSingleByName(shippingAgentName, tenant);
            }
            return shippingAgent.Id;
        }
        private void InsertNewShippingAgent(string shippingAgentName)
        {
            ShippingAgentService shippingAgentService = new ShippingAgentService(commonDataContext, tenant);
            shippingAgentService.Create(CreateShippingAgentPM(shippingAgentName));
        }

        public ShippingAgentPM CreateShippingAgentPM(string shippingAgentName)
        {
            ShippingAgentPM shippingAgentPM = new ShippingAgentPM();
            shippingAgentPM.Tenant = tenant;
            shippingAgentPM.EnglishName = shippingAgentName;
            shippingAgentPM.CityName = "AKD";
            shippingAgentPM.PartnerTypeId = "SG";
            shippingAgentPM.CountryId = Variables.CountryUSId;
            return shippingAgentPM;

        }
    }
}
