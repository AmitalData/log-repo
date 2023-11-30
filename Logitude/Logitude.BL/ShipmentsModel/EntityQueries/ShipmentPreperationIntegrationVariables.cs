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
using System.Linq;
using System.Collections.Generic;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.Security;
using Simplog.Data.ShipmentsModel.Repositories;
using System.Data.Entity;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentPreperationIntegrationVariables
    {
        ShipmentIntegrationVariables vars = new ShipmentIntegrationVariables();
        IShipmentsContext shipmentContext;
        ICommonDataContext commonDataContext;
        IWebFreightContext webFreightContext;
        IQuotesContext quoteContext;
        ShipmentPM shipment;
        UserPM loggedUser;
        TenantPM tenantPM;
        int tenant;

        public ShipmentPreperationIntegrationVariables(int tenant)
        {
            this.tenant = tenant;
            shipmentContext = ShipmentsContext.GetContext(tenant);
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
            vars.PortLHRId = GetPort("LHR");
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
            vars.AgentId = GetAgent("IntegrationAgent");
            vars.ShipperExport1 = GetCustomer("ShipperExport1");
            vars.CustomAgentId = GetCustomsAgent("InegrationCustomsAgent");
            vars.ShippingAgentId = GetShippingAgent("IntegrationShippingAgent");
            vars.WarehouseId = GetWarehouse("IntegrationWarehouse", "WR9");
            vars.CustomerId = GetCustomer("IntegrationCustomer");
            //vars.AWBShipmentId = CreateAWBShipment();
            //vars.ShipmentNumber = shipment.ShipmentNumber;
            vars.ChargesTypes = this.FillChargesTypes();
            vars.VatTypes = this.FillVatTayes();
            vars.Currencies = this.FillCurrencies();
            vars.Rates = this.FillRates();
            return vars;
        }

        /*   private string CreateAWBShipment()
           {
               GetUserPM();
               GetTenantPM();
               CreatShipmentPM();
               InsertShipment();
              // GetShipment();
               return shipment.Id;
           }

           private void GetShipment()
           {
               ShipmentRepository updatedEntityRepository = new ShipmentRepository(shipmentContext);
               ShipmentQuery updatedShipmentQuery = new ShipmentQuery(updatedEntityRepository);
               shipment = updatedShipmentQuery.GetSingleShipmentPMByNumber(shipment.ShipmentNumber, tenant);
           }

           private void InsertShipment()
           {
               ShipmentService service = new ShipmentService(shipmentContext, shipment, SecurityUtility.GetAuthenticatedUser());
               service.Create();
           }

           private void GetUserPM()
           {
               UserRepository userRepository = new UserRepository(commonDataContext);
               UserQuery userQuery = new UserQuery(userRepository);
               string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
               loggedUser = userQuery.GetSingleUserPMByEmail(loggedUserEmail, tenant, true);
           }

           private void GetTenantPM()
           {
               TenantQuery tenantQuery = new TenantQuery(tenant);
               tenantPM = tenantQuery.GetTenantFromDB(tenant);
           }

           private void CreatShipmentPM()
           {
               shipment = new ShipmentPM();
               shipment.Tenant = tenant;
               shipment.ShipmentNumber = "AWBShipmentIntgTest";
               shipment.CreatedByUserId = loggedUser.Id;
               shipment.BranchId = loggedUser.BranchId;
               shipment.DepartmentId = loggedUser.DepartmentId;
               shipment.ProfitCurrencyId = tenantPM.ProfitCurrencyId;
               shipment.VolumeUnitCode = tenantPM.VolumeUnitCode;
               shipment.DimensionsUnitCode = tenantPM.DimensionsUnitCode;
               shipment.GrossWeightUnitCode = tenantPM.GrossWeightUnitCode;
               shipment.ChargeableWeightUnitCode = tenantPM.ChargeableWeightUnitCode;
               shipment.ShipmentLevelCode = "D";
               shipment.DirectionId = "E";
               shipment.TransportModeId = "A";
               shipment.FreightPrepaidCollectId = "C";
               shipment.OtherPrepaidCollectId = "C";
               shipment.CreatedByUserId = loggedUser.Id;
               shipment.UpdatedByUserId = loggedUser.Id;
               shipment.CustomerId = vars.CustomerId;
               shipment.ShipperId = vars.AgentId;
               shipment.IssuingCarrierAgentId = vars.AgentId;
               shipment.AgentId = vars.AgentId;
               shipment.FromPortId = vars.PortLHRId;
               shipment.ToPortId = vars.PortJFKId;
               shipment.MainCarriageFromPortId = vars.PortLHRId;
               shipment.MainCarriageToPortId = vars.PortJFKId;
               shipment.OriginMainCarriageFromPortId = vars.PortLHRId;
               shipment.AWBCurrencyId = vars.CurrencyEURId;
               shipment.ValueOfGoodsCurrencyId = vars.CurrencyEURId;
               shipment.AccountManagerUserId = loggedUser.Id;
               vars.ConcurrencyGUID = shipment.NewConcurrencyGUID = Guid.NewGuid().ToString();
           }*/

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
            WarehousePM.CountryId = vars.CountryUSId;
            WarehousePM.Code = code;
            return WarehousePM;

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
            shippingAgentPM.CountryId = vars.CountryUSId;
            return shippingAgentPM;

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
            customAgentPM.CountryId = vars.CountryUSId;
            customAgentPM.Addresses.Add(Address("M", customAgentName));
            return customAgentPM;
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
            customerPM.CountryId = vars.CountryUSId;
            customerPM.Addresses.Add(Address("M", agentName));
            return customerPM;
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
            agentPM.CountryId = vars.CountryUSId;
            agentPM.Addresses.Add(Address("M", agentName));
            return agentPM;
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

        private List<PreparationShortClass> FillChargesTypes()
        {
            List<PreparationShortClass> charges = (from a in commonDataContext.ChargesTypes
                                                   where a.Tenant == tenant
                                                   && a.IsAutoDisplayInShipment == true
                                                   select new PreparationShortClass
                                                   {
                                                       Id = a.Id,
                                                       Code = a.Code
                                                   }).ToList();

            return charges;
        }

        private List<PreparationShortClass> FillVatTayes()
        {
            List<PreparationShortClass> vatTypes = (from a in commonDataContext.VatTypes
                                                   where a.Tenant == tenant
                                                   select new PreparationShortClass
                                                   {
                                                       Id = a.Id,
                                                       Code = a.Code
                                                   }).ToList();

            return vatTypes;
        }

        private List<PreparationShortClass> FillCurrencies()
        {
            List<PreparationShortClass> currencies = (from a in commonDataContext.Currencies
                                                   where a.Tenant == tenant
                                                   select new PreparationShortClass
                                                   {
                                                       Id = a.Id,
                                                       Code = a.Code
                                                   }).ToList();

            return currencies;
        }

        public List<PreparationShortClass> FillRates()
        {
            List<PreparationShortClass> rates = (from a in webFreightContext.RatesTable.Include("ForeignCurrency")
                                                 where a.Tenant == tenant
                                                 select new PreparationShortClass()
                                                 {
                                                     Id = a.Id,                                                     
                                                     Rate = a.Rate,
                                                     ForeignCurrencyId = a.ForeignCurrencyId,
                                                     ForeignCurrencyCode = a.ForeignCurrency.Code,
                                                     BaseCurrencyId = a.BaseCurrencyId,
                                                     ValueDate = a.ValueDate,
                                                 }).ToList();
            return rates;
        }

    }

    public class PreparationShortClass
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public double? Rate { get; set; }
        public string ForeignCurrencyId { get; set; }
        public string ForeignCurrencyCode { get; set; }
        public string BaseCurrencyId { get; set; }
        public DateTime? ValueDate { get; set; }
    }
}
