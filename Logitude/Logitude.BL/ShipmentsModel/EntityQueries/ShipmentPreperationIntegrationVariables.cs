using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentPreperationIntegrationVariables
    {
        ShipmentIntegrationVariables vars = new ShipmentIntegrationVariables();
        ICommonDataContext commonDataContext;
        int tenant;
        public ShipmentPreperationIntegrationVariables(int tenant)
        {
            this.tenant = tenant;
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public ShipmentIntegrationVariables GetShipmentVars()
        {
            
            vars.CurrencyEURId = GetCurrency("EUR");
            vars.IncotermLDEId = GetIncoterm("LDE");
            vars.MeasurementGRWTId = GetMeasurement("GRWT");

            return vars;
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
            IncotermPM entityPM = CreateIncotermPM(code);
            IncotermService service = new IncotermService(commonDataContext, tenant);
            service.Create(entityPM);
        }

        private string GetCurrency(string code)
        {
            string currencyId = "";
            Currency currency = CurrencyRepository.GetSingleCurrencyByCode(code, tenant, false);
            
            if (currency == null)
            {
                currencyId = CopyCurrencyFromTenantZero(code);
            } else {
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
                CommonDataDomainService commonDomain = new CommonDataDomainService();
                CurrencyList copiedCurrency = commonDomain.CopyCurrencyToTenant(tenantZeroCurrency.Id, tenant, 4, DateTime.Today);
                copiedCurrencyId = copiedCurrency != null ? copiedCurrency.Id : "";
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
