using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.IntegrationTestModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.IntegrationTestModel.EntityQueries
{
    public class LocationsPreparationVariables
    {
        LocationsVariables Variables = new LocationsVariables();
        ICommonDataContext commonDataContext;
        IWebFreightContext webFreightContext;
    

        int tenant;

        public LocationsPreparationVariables(int tenant)
        {
            this.tenant = tenant;
            commonDataContext = CommonDataContext.GetContext(tenant);
            webFreightContext = WebFreightContext.GetContext(tenant);

        }

        public LocationsVariables GetBaseLocationsVaribles()
        {

            Variables.PortJFKId = GetPort("JFK");
            Variables.PortMIAId = GetPort("MIA");
            Variables.PortLHRId = GetPort("LHR");
            Variables.PortSOUId = GetPort("SOU");
            Variables.PortNYCId = GetPort("NYC");
            Variables.PortLONId = GetPort("LON");
            Variables.PortMANId = GetPort("MAN");
            Variables.GlobalZoneEUId = GetGlobalZone("EU");
            Variables.StateAKId = GetState("AK");
            Variables.AirlineAAId = GetAirline("AA");
            Variables.AirlineBAId = GetAirline("BA");
            Variables.ShippingLineMSCUId = GetShippingLine("MSCU");
            Variables.ShippingLineMAEUId = GetShippingLine("MAEU");
            Variables.MoveTypeMTAId = GetMoveType("MTA", "A");
            Variables.MoveTypeMTOId = GetMoveType("MTO", "O");
            Variables.WarehouseId = GetWarehouse("IntegrationWarehouse", "WR9");

            return Variables;
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
            statePM.CountryId = Variables.CountryUSId;
            return statePM;
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
            WarehousePM.CountryId = Variables.CountryUSId;
            WarehousePM.Code = code;
            return WarehousePM;

        }

    }
}
