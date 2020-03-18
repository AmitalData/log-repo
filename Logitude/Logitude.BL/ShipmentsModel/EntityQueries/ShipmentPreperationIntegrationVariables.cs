using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentPreperationIntegrationVariables
    {
        ShipmentIntegrationVariables vars = new ShipmentIntegrationVariables();
        ICommonDataContext commonDataContext;
        IWebFreightContext webFreightContext;
        IQuotesContext quoteContext;
        int tenant;
        public ShipmentPreperationIntegrationVariables(int tenant)
        {
            this.tenant = tenant;
            commonDataContext = CommonDataContext.GetContext(tenant);
            webFreightContext = WebFreightContext.GetContext(tenant);
            quoteContext = QuotesContext.GetContext(tenant);
        }

        public ShipmentIntegrationVariables GetShipmentVars()
        {
            vars.CurrencyEURId = GetCurrency("EUR");
            vars.IncotermLDEId = GetIncoterm("LDE");
            vars.MeasurementGRWTId = GetMeasurement("GRWT");
            ChargesGroup chargesGroup = GetChargesGroup("COMM");
            vars.ChargeGroupCOMMId = chargesGroup != null ? chargesGroup.Id : "";
            vars.ChargeGroupCOMMCode = chargesGroup != null ? chargesGroup.Code : "";
            vars.ChargeTypeAFTId = GetChargeType("AFT");
            vars.PortJFKId = GetPort("JFK");
            vars.PortMIAId = GetPort("MIA");
            vars.PortJFKId = GetPort("JFK");
            vars.PortSOUId = GetPort("SOU");
            vars.PortNYCId = GetPort("NYC");
            vars.PortLONId = GetPort("LON");
            vars.PortMANId = GetPort("MAN");
            vars.GlobalZoneEUId = GetGlobalZone("EU");
            vars.CountryGBId = GetCountry("GB");
            vars.CountryUSId = GetCountry("US");
            vars.StateAKId = GetState("AK");
            vars.AirlineAAId = GetAirline("AA");
            vars.AirlineBAId = GetAirline("BA");
            vars.ShippingLineMSCUId = GetShippingLine("MSCU");
            vars.ShippingLineMAEUId = GetShippingLine("MAEU");
            vars.MoveTypeMTAId = GetMoveType("MTA", "A");
            vars.MoveTypeMTOId = GetMoveType("MTO", "O");
            vars.VesselPTId = GetVessel("PT");
            vars.PackageTypePC1Id = GetPackageType("PC1", "O", true);
            vars.PackageTypePC2Id = GetPackageType("PC2", "O", true);
            vars.PackageTypePP1Id = GetPackageType("PP1", "A", false);
            vars.PackageTypePP2Id = GetPackageType("PP2", "A", false);
            vars.PaymentTermCashId = GetPaymentTerm("Cash");
            vars.VATTypeZeroId = GetVATType("ZERO");
            vars.QuoteStageQTDRId = GetQuoteStage("QTDR");
            vars.VendorId = GetVendor("TestVendor");
            vars.AgentId = GetAgent("TestAgentExport1");
            //vars.CustomerId =  GetCustomer("TestShipperExport1");
            vars.CustomAgentId = GetCustomsAgent("TestCustomAgentExport1");
            vars.ShippingAgentId = GetShippingAgent("TestShippingAgentExport1");
            //    ShipmentVariables.WarehouseId = await GetWarehouseId("TestWarehouseExport1", "WR2");
            //    ShipmentVariables.ShipperExport1 = await GetCustomerId("TstShipExport1");


            return vars;
        }
        private string GetShippingAgent(string shippingAgentName)
        {
            ShippingAgentRepository shippingAgentRepository = new ShippingAgentRepository(commonDataContext);
            ShippingAgent shippingAgent = shippingAgentRepository.GetSingleShippingAgentByCode(shippingAgentName, tenant);
            if (shippingAgent == null)
            {
                InsertNewShippingAgent(shippingAgentName);
                shippingAgent = shippingAgentRepository.GetSingleShippingAgentByCode(shippingAgentName, tenant);
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
            shippingAgentPM.PartnerTypeId = "AG";
            shippingAgentPM.CountryId = vars.CountryUSId;
            return shippingAgentPM;

        }
        private string GetCustomsAgent(string customAgentName)
        {
            CustomAgentRepository customAgentRepository = new CustomAgentRepository(commonDataContext);
            CustomAgent customAgent = customAgentRepository.GetSingleCustomAgentByCode(tenant, customAgentName);
            if (customAgent == null)
            {
                InsertNewCustomAgent(customAgentName);
                customAgent = customAgentRepository.GetSingleCustomAgentByCode(tenant, customAgentName);
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
            customAgentPM.PartnerTypeId = "AG";
            customAgentPM.CountryId = vars.CountryUSId;
            customAgentPM.Addresses.Add(Address("M", customAgentName));
            return customAgentPM;
        }
        //private string GetCustomer(string customerName)
        //{
        //    CustomerRepository customerRepository = new CustomerRepository(commonDataContext);
        //    Customer customer = customerRepository.GetSingleCustomerByCode(customerName, tenant,false);
        //    if (customer == null)
        //    {
        //        InsertNewCustomer(customerName);
        //        customer = customerRepository.GetSingleCustomerByCode(customerName, tenant,false);
        //    }
        //    return customer.Id;
        //}
        //private void InsertNewCustomer(string customerName)
        //{
        //    CustomerService customerService = new PartnerService(commonDataContext, tenant);
        //    customerService.Create(CreateCustomertPM(customerName));
        //}

        //public AgentPM CreateCustomertPM(string agentName)
        //{
        //    AgentPM agentPM = new AgentPM();
        //    agentPM.Tenant = tenant;
        //    agentPM.EnglishName = agentName;
        //    agentPM.CityName = "AKD";
        //    agentPM.PartnerTypeId = "AG";
        //    agentPM.CountryId = vars.CountryUSId;
        //    agentPM.Addresses.Add(Address("M", agentName));
        //    return agentPM;
        //}
        private string GetAgent(string agentName)
        {
            AgentRepository agentRepository = new AgentRepository(commonDataContext);
            Agent agent = agentRepository.GetSingleAgentByCode(agentName, tenant);
            if (agent == null)
            {
                InsertNewAgent(agentName);
                agent = agentRepository.GetSingleAgentByCode(agentName, tenant);
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
            agentPM.CountryId = vars.CountryUSId;
            agentPM.Addresses.Add(Address("M", agentName));
            return agentPM;
        }

        private string GetVendor(string vendorName)
        {
            VendorRepository vendorRepository = new VendorRepository(commonDataContext);
            Vendor vendor = vendorRepository.GetSingleVendorByCode(vendorName, tenant);
            if (vendor == null)
            {
                InsertNewVendor(vendorName);
                vendor = vendorRepository.GetSingleVendorByCode(vendorName, tenant);
            }
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
            vendorPM.CountryId = vars.CountryUSId;
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
            address.CountryId = vars.CountryGBId;
            address.IsCreatedWithPartner = true;

            return address;
        }

        private string GetQuoteStage(string code)
        {
            QuoteStageRepository quoteStageRepository = new QuoteStageRepository(quoteContext);
            QuoteStage quoteStage = quoteStageRepository.GetSingleQuoteStageByCode(code, tenant);
            return quoteStage != null ? quoteStage.Id : "";
        }

        private string GetVATType(string code)
        {
            VatTypeRepository vatTypeRepository = new VatTypeRepository(commonDataContext);
            VatType vatType = vatTypeRepository.GetSingleVatTypeByCode(code, tenant);
            if (vatType == null)
            {
                InsertNewVatType(code);
                vatType = vatTypeRepository.GetSingleVatTypeByCode(code, tenant);
            }
            return vatType.Id;
        }

        private void InsertNewVatType(string code)
        {
            VatTypeService vatTypeService = new VatTypeService(commonDataContext, tenant);
            vatTypeService.Create(CreateVatTypePM(code));
        }

        public VatTypePM CreateVatTypePM(string code)
        {
            VatTypePM vatTypePM = new VatTypePM();
            vatTypePM.Tenant = tenant;
            vatTypePM.Code = code;
            vatTypePM.EnglishName = code;
            vatTypePM.LocalName = code;
            vatTypePM.NewEntityPercentage = 0;
            vatTypePM.NewEntityPercentageDate = DateTime.Now;
            return vatTypePM;
        }

        private string GetPaymentTerm(string code)
        {
            PaymentTermRepository paymentTermRepository = new PaymentTermRepository(commonDataContext);
            PaymentTerm paymentTerm = paymentTermRepository.GetSinglePaymentTermByCode(code, tenant);
            if (paymentTerm == null)
            {
                InsertNewPaymentTerm(code);
                paymentTerm = paymentTermRepository.GetSinglePaymentTermByCode(code, tenant);
            }
            return paymentTerm.Id;
        }

        private void InsertNewPaymentTerm(string code)
        {
            PaymentTermService paymentTermService = new PaymentTermService(commonDataContext, tenant);
            paymentTermService.Create(CreatePaymentTermPM(code));
        }

        public PaymentTermPM CreatePaymentTermPM(string paymentTermNameCode)
        {
            PaymentTermPM paymentTermPM = new PaymentTermPM();
            paymentTermPM.Tenant = tenant;
            paymentTermPM.Code = paymentTermNameCode;
            paymentTermPM.EnglishName = paymentTermNameCode;
            paymentTermPM.LocalName = paymentTermNameCode;
            paymentTermPM.FromDateTypeCode = "SHI";
            return paymentTermPM;
        }

        private string GetPackageType(string packageTypeCode, string transportModeCode, bool isContainer)
        {
            PackageTypeRepository packageRepository = new PackageTypeRepository(commonDataContext);
            PackageType packageType = packageRepository.GetSinglePackageTypeByCode(packageTypeCode, tenant, false);
            if (packageType == null)
            {
                InsertNewPackageType(packageTypeCode, transportModeCode, isContainer);
                packageType = packageRepository.GetSinglePackageTypeByCode(packageTypeCode, tenant, false);
            }
            return packageType.Id;
        }

        private void InsertNewPackageType(string packageTypeCode, string transportModeCode, bool isContainer)
        {
            PackageTypeService packageTypeService = new PackageTypeService(commonDataContext, tenant);
            packageTypeService.Create(CreatePackageTypePM(packageTypeCode, transportModeCode, isContainer));
        }

        public PackageTypePM CreatePackageTypePM(string packageTypeCode, string transportModeCode, bool isContainer)
        {
            PackageTypePM packageTypePM = new PackageTypePM();
            packageTypePM.Tenant = tenant;
            packageTypePM.Code = packageTypeCode;
            packageTypePM.EnglishName = "ContainerId" + packageTypeCode;
            packageTypePM.PrintAs = packageTypeCode;
            packageTypePM.IsAir = transportModeCode == "A" ? true : false;
            packageTypePM.IsOcean = transportModeCode == "O" ? true : false;
            packageTypePM.IsInland = transportModeCode == "I" ? true : false;
            packageTypePM.IsContainer = isContainer;
            return packageTypePM;
        }

        private string GetVessel(string code)
        {
            VesselRepository vesselRepository = new VesselRepository(commonDataContext);
            Vessel vessel = vesselRepository.GetSingleVesselByCode(code, tenant);
            if (vessel == null)
            {
                InsertNewVessel(code);
                vessel = vesselRepository.GetSingleVesselByCode(code, tenant);
            }
            return vessel.Id;
        }

        private void InsertNewVessel(string code)
        {
            VesselService vesselService = new VesselService(commonDataContext, tenant);
            vesselService.Create(CreateVesselPM(code));
        }

        public VesselPM CreateVesselPM(string vesselCode)
        {
            VesselPM vesselPM = new VesselPM();
            vesselPM.Tenant = tenant;
            vesselPM.Code = vesselCode;
            vesselPM.EnglishName = " vesselId" + vesselCode;
            vesselPM.IMOCode = "IMOCode " + vesselCode;
            return vesselPM;
        }

        private string GetMoveType(string moveTypeCode, string transportModeCode)
        {
            MoveTypeRepository moveTypeRepository = new MoveTypeRepository(webFreightContext);
            MoveType moveType = moveTypeRepository.GetSingleMoveTypesByCode(moveTypeCode, tenant);
            if (moveType == null)
            {
                InsertNewMoveType(moveTypeCode, transportModeCode);
                moveType = moveTypeRepository.GetSingleMoveTypesByCode(moveTypeCode, tenant);
            }
            return moveType.Id;
        }

        private void InsertNewMoveType(string moveTypeCode, string transportModeCode)
        {
            MoveTypeService moveTypeService = new MoveTypeService(webFreightContext, tenant);
            moveTypeService.Create(CreateMoveTypePM(moveTypeCode, transportModeCode));
        }

        public MoveTypePM CreateMoveTypePM(string moveTypeCode, string moveTypeTransportMode)
        {
            MoveTypePM moveTypePM = new MoveTypePM();
            moveTypePM.Tenant = tenant;
            moveTypePM.Code = moveTypeCode;
            moveTypePM.MoveTypeEnglishName = "TestMoveTypeId" + moveTypeCode;
            moveTypePM.MoveTypeLocalName = moveTypeCode + " Move Type LocalName";
            moveTypePM.TransportModeId = moveTypeTransportMode;
            moveTypePM.IsAir = moveTypeTransportMode == "A" ? true : false;
            moveTypePM.IsOcean = moveTypeTransportMode == "O" ? true : false;
            moveTypePM.IsInland = moveTypeTransportMode == "I" ? true : false;
            return moveTypePM;
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

        private string GetState(string code)
        {
            StateRepository stateRepository = new StateRepository(commonDataContext);
            State state = stateRepository.GetSingleStateByCode(code, tenant);
            if (state == null)
            {
                InserNewState(code);
                state = stateRepository.GetSingleStateByCode(code, tenant);
            }
            return state.Id;
        }

        private void InserNewState(string code)
        {
            StateService stateService = new StateService(commonDataContext, tenant);
            stateService.Create(CreateStatePM(code));
        }
        public StatePM CreateStatePM(string stateCode)
        {
            StatePM statePM = new StatePM();
            statePM.Tenant = tenant;
            statePM.Code = stateCode;
            statePM.EnglishName = stateCode + " State";
            statePM.CountryId = vars.CountryUSId;
            return statePM;
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
            countryPM.GlobalZoneId = vars.GlobalZoneEUId;
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

        private string GetPort(string code)
        {
            string portId = "";
            PortRepository portRepository = new PortRepository(commonDataContext);
            Port port = portRepository.GetSinglePortByCode(tenant, code, false);
            if (port == null)
            {
                portId = CopyPortFromTenantZero(code);
            }
            else
            {
                portId = port.Id;
            }
            return portId;
        }

        private string CopyPortFromTenantZero(string code)
        {
            PortRepository portRepository = new PortRepository(commonDataContext);
            Port tenantZeroPort = portRepository.GetSinglePortByCode(0, code, false);
            string copiedPortId = "";
            if (tenantZeroPort != null)
            {
                PortQuery portQuery = new PortQuery(portRepository);
                PortList copiedPort = portQuery.GetPortCopyToCurrentTenant(tenantZeroPort.Id, tenant);
                copiedPortId = copiedPort != null ? copiedPort.Id : "";
            }
            return copiedPortId;
        }

        private string GetChargeType(string code)
        {
            ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(commonDataContext);
            ChargesType chargesType = chargesTypeRepository.GetSingleChargesTypeByCode(code, tenant);
            if (chargesType == null)
            {
                InsertNewChargeType(code);
                chargesType = chargesTypeRepository.GetSingleChargesTypeByCode(code, tenant);
            }
            return chargesType.Id;
        }

        private void InsertNewChargeType(string code)
        {
            ChargesTypeService chargesTypeService = new ChargesTypeService(commonDataContext, tenant);
            chargesTypeService.Create(CreateChargeTypePM(code));
        }

        public ChargesTypePM CreateChargeTypePM(string code)
        {
            ChargesTypePM chargesTypePM = new ChargesTypePM();
            chargesTypePM.Tenant = tenant;
            chargesTypePM.Code = code;
            chargesTypePM.EnglishName = code + " Charge Type";
            chargesTypePM.ChargesGroupId = vars.ChargeGroupCOMMId;
            chargesTypePM.ChargesGroupCode = vars.ChargeGroupCOMMCode;
            chargesTypePM.MeasurementId = vars.MeasurementGRWTId;
            chargesTypePM.IsAutoDisplayInShipment = true;
            chargesTypePM.IsAutoDisplayInQuote = true;
            chargesTypePM.IsInland = true;
            chargesTypePM.IsAir = true;
            chargesTypePM.IsOcean = true;
            chargesTypePM.IsInland = true;
            chargesTypePM.IsExport = true;
            chargesTypePM.IsImport = true;
            chargesTypePM.IsDrop = true;
            chargesTypePM.IsDomestic = true;
            return chargesTypePM;
        }
        private ChargesGroup GetChargesGroup(string code)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
            ChargesGroupRepository chargesGroupRepository = new ChargesGroupRepository(webFreightContext);
            return chargesGroupRepository.GetSingleChargesGroupByCode(code, tenant);
        }

        private string GetMeasurement(string code)
        {
            MeasurementRepository measurementRepository = new MeasurementRepository(commonDataContext);
            string measurementId = measurementRepository.GetMeasurementIdbyCode(code, tenant);
            if (string.IsNullOrEmpty(measurementId))
            {
                measurementId = "";
            }
            return measurementId;
        }

        private string GetIncoterm(string code)
        {
            IncotermRepository incotermRepository = new IncotermRepository(commonDataContext);
            string incotermId = incotermRepository.GetIncotermIdByCode(code, tenant);
            if (string.IsNullOrEmpty(incotermId))
            {
                InsertNewIncoterm(code);
                incotermId = incotermRepository.GetIncotermIdByCode(code, tenant);
            }
            return incotermId;
        }

        private void InsertNewIncoterm(string code)
        {
            IncotermService service = new IncotermService(commonDataContext, tenant);
            service.Create(CreateIncotermPM(code));
        }

        private string GetCurrency(string code)
        {
            string currencyId = "";
            Currency currency = CurrencyRepository.GetSingleCurrencyByCode(code, tenant, false);

            if (currency == null)
            {
                currencyId = CopyCurrencyFromTenantZero(code);
            }
            else
            {
                currencyId = currency.Id;
            }
            return currencyId;
        }

        private string CopyCurrencyFromTenantZero(string code)
        {
            Currency tenantZeroCurrency = CurrencyRepository.GetSingleCurrencyByCode(code, 0, false);
            string copiedCurrencyId = "";
            if (tenantZeroCurrency != null)
            {
                //CommonDataDomainService commonDomain = new CommonDataDomainService();
                //CurrencyList copiedCurrency = commonDomain.CopyCurrencyToTenant(tenantZeroCurrency.Id, tenant, 4, DateTime.Today);
                //copiedCurrencyId = copiedCurrency != null ? copiedCurrency.Id : "";
            }
            return copiedCurrencyId;
        }
        public IncotermPM CreateIncotermPM(string code)
        {
            IncotermPM incotermPM = new IncotermPM();
            incotermPM.Tenant = tenant;
            incotermPM.Code = code;
            incotermPM.Name = code + " Incoterm";
            incotermPM.Freight = "P";
            incotermPM.OtherCharges = "P";
            return incotermPM;
        }
    }
}
