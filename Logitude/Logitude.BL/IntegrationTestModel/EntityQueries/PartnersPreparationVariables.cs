using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.IntegrationTestModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;

namespace Logitude.BL.IntegrationTestModel.EntityQueries
{
    public class PartnersPreparationVariables
    {
        PartnersVariables Variables = new PartnersVariables();
        ICommonDataContext commonDataContext;
        IWebFreightContext webFreightContext;
        int tenant;

        public PartnersPreparationVariables(int tenant)
        {
            this.tenant = tenant;
            commonDataContext = CommonDataContext.GetContext(tenant);
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public PartnersVariables GetBasePartnersVaribles()
        {
            Variables.VendorId = GetVendor("TestVendor");
            Variables.AgentId = GetAgent("IntegrationAgent");
            Variables.ShipperExport1 = GetCustomer("ShipperExport1");
            Variables.CustomAgentId = GetCustomsAgent("InegrationCustomsAgent");
            Variables.ShippingAgentId = GetShippingAgent("IntegrationShippingAgent");
            Variables.CustomerId = GetCustomer("IntegrationCustomer");
            Variables.AirlineAAId = GetAirline("AA");
            Variables.AirlineBAId = GetAirline("BA");
            //Variables.TruckerId =                     //Must be implmented 
            // Variables.PotentialCustomerId =          //Must be implmented 
            Variables.ShippingLineMSCUId = GetShippingLine("MSCU");
            Variables.ShippingLineMAEUId = GetShippingLine("MAEU");   
            Variables.WarehouseId = GetWarehouse("IntegrationWarehouse", "WR9");
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
            vendorPM.CountryId = GetCountry("US");
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
            address.CountryId = GetCountry("GB");
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
            agentPM.CountryId = GetCountry("US");
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
            customerPM.CountryId = GetCountry("US");
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
            customAgentPM.CountryId = GetCountry("US");
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
            shippingAgentPM.CountryId = GetCountry("US");
            return shippingAgentPM;

        }
        private string GetAirline(string code)
        {
            string airlineId = "";
            AirlineRepository airlineRepository = new AirlineRepository(commonDataContext);
            Airline airline = airlineRepository.GetSingleAirlineByCode(code, tenant);
            if (airline == null)
            {
                airlineId = CopyAirlineFromTenantZero(code, airlineRepository);
            }
            else
            {
                airlineId = airline.Id;
            }
            return airlineId;
        }

        private string CopyAirlineFromTenantZero(string code, AirlineRepository airlineRepository)
        {
            Airline tenantZeroAirline = airlineRepository.GetSingleAirlineByCode(code, 0);
            string copiedAirlineId = "";
            if (tenantZeroAirline != null)
            {
                CardQuery cardQuery = new CardQuery(tenant);
                CardList copiedAirline = cardQuery.GetCarrierCopyToCurrentTenant(tenantZeroAirline.Id, tenant, null, null, false, null);
                copiedAirlineId = copiedAirline != null ? copiedAirline.Id : "";
            }
            return copiedAirlineId;
        }

        private string GetShippingLine(string code)
        {
            string shippingLineId = "";
            ShippingLineRepository shippingLineRepository = new ShippingLineRepository(commonDataContext);
            ShippingLine shippingLine = shippingLineRepository.GetSingleShippingLineByCode(code, tenant);
            if (shippingLine == null)
            {
                shippingLineId = CopyShippingLineFromTenantZero(code, shippingLineRepository);
            }
            else
            {
                shippingLineId = shippingLine.Id;
            }
            return shippingLineId;
        }

        private string CopyShippingLineFromTenantZero(string code, ShippingLineRepository shippingLineRepository)
        {
            ShippingLine tenantZeroShippingLine = shippingLineRepository.GetSingleShippingLineByCode(code, 0);
            string copiedShippingLineId = "";
            if (tenantZeroShippingLine != null)
            {
                CardQuery cardQuery = new CardQuery(tenant);
                CardList copiedAirline = cardQuery.GetCarrierCopyToCurrentTenant(tenantZeroShippingLine.Id, tenant, null, null, false, null);
                copiedShippingLineId = copiedAirline != null ? copiedAirline.Id : "";
            }
            return copiedShippingLineId;
        }

        private string GetWarehouse(string warehouseName, string code)
        {
            WarehouseRepository warehouseRepository = new WarehouseRepository(commonDataContext);
            Warehouse warehouse = warehouseRepository.GetFirstSingleByCode(code, tenant);
            if (warehouse == null)
            {
                InsertNewWarehouse(warehouseName, code);
                warehouse = warehouseRepository.GetFirstSingleByCode(code, tenant);
            }
            return warehouse.Id;
        }
        private void InsertNewWarehouse(string warehouseName, string code)
        {
            WarehouseService warehouseService = new WarehouseService(commonDataContext, tenant);
            warehouseService.Create(CreateWarehousePM(warehouseName, code));
        }

        public WarehousePM CreateWarehousePM(string warehouseName, string code)
        {
            WarehousePM WarehousePM = new WarehousePM();
            WarehousePM.Tenant = tenant;
            WarehousePM.EnglishName = warehouseName;
            WarehousePM.CityName = "AKD";
            WarehousePM.PartnerTypeId = "WH";
            WarehousePM.CountryId = GetCountry("US");
            WarehousePM.Code = code;
            return WarehousePM;

        }
        private string GetCountry(string code)
        {
            string countryId = "";
            CountryRepository countryRepository = new CountryRepository(commonDataContext);
            countryId = countryRepository.GetCountryIdByCode(code, tenant);
            if (string.IsNullOrEmpty(countryId))
            {
                InsertNewCountry(code);
                countryId = countryRepository.GetCountryIdByCode(code, tenant);
            }
            return countryId;
        }
        private void InsertNewCountry(string code)
        {
            CountryService countryService = new CountryService(commonDataContext, tenant);
            countryService.Create(CreateCountryPM(code));
        }

        public CountryPM CreateCountryPM(string countryCode)
        {
            CountryPM countryPM = new CountryPM();
            countryPM.Tenant = tenant;
            countryPM.Code = countryCode;
            countryPM.EnglishName = countryCode + " Country";
            countryPM.GlobalZoneId = GetGlobalZone("EU");
            return countryPM;
        }

        private string GetGlobalZone(string code)
        {
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(commonDataContext);
            GlobalZone globalZone = globalZoneRepository.GetSingleGlobalZoneByCode(code, tenant);
            if (globalZone == null)
            {
                InsertNewGlobalZone(code);
                globalZone = globalZoneRepository.GetSingleGlobalZoneByCode(code, tenant);
            }
            return globalZone.Id;
        }

        private void InsertNewGlobalZone(string code)
        {
            GlobalZoneService globalZoneService = new GlobalZoneService(commonDataContext, tenant);
            globalZoneService.Create(CreateGlobalZonePM(code));
        }
        public GlobalZonePM CreateGlobalZonePM(string code)
        {
            GlobalZonePM globalZonePM = new GlobalZonePM();
            globalZonePM.Tenant = tenant;
            globalZonePM.Code = code;
            globalZonePM.EnglishName = code + " Global Zone";
            return globalZonePM;
        }
    }
}
