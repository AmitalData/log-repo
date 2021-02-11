using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.IntegrationTestModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;


namespace Logitude.BL.IntegrationTestModel.EntityQueries
{
    public class LocationsPreparationVariables
    {
        LocationsVariables Variables = new LocationsVariables();
        ICommonDataContext commonDataContext;
      
    

        int tenant;

        public LocationsPreparationVariables(int tenant)
        {
            this.tenant = tenant;
            commonDataContext = CommonDataContext.GetContext(tenant);
     

        }

        public LocationsVariables GetBaseLocationsVaribles()
        {

            Variables.PortJFKId = GetPort("JFK");
            Variables.PortMIAId = GetPort("MIA");
            Variables.PortLASId = GetPort("LAS");
            Variables.PortLHRId = GetPort("LHR");
            Variables.PortSOUId = GetPort("SOU");
            Variables.PortNYCId = GetPort("NYC");
            Variables.PortLONId = GetPort("LON");
            Variables.PortMANId = GetPort("MAN");
            Variables.GlobalZoneEUId = GetGlobalZone("EU");
            Variables.StateAKId = GetState("AK");
            Variables.CountryGBId = GetCountry("GB");
            Variables.CountryUSId = GetCountry("US");


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
            countryPM.GlobalZoneId = Variables.GlobalZoneEUId;
            return countryPM;
        }



    }
}
