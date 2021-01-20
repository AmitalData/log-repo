using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.IntegrationTestModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.IntegrationTestModel.EntityQueries
{
    public class ShipmentPreparationVariables
    {
        ShipmentVariables Variables = new ShipmentVariables();
        ICommonDataContext commonDataContext;
        IWebFreightContext webFreightContext;
        IQuotesContext quoteContext;

        int tenant;

        public ShipmentPreparationVariables(int tenant)
        {
            this.tenant = tenant;
            commonDataContext = CommonDataContext.GetContext(tenant);
            webFreightContext = WebFreightContext.GetContext(tenant);
            quoteContext = QuotesContext.GetContext(tenant);
        }

        public ShipmentVariables GetBaseShipmentVaribles()
        {
            Variables.CurrencyEURId = GetCurrency("EUR");
            Variables.IncotermLDEId = GetIncoterm("LDE");
            Variables.MeasurementGRWTId = GetMeasurement("GRWT");
            ChargesGroup chargesGroup = GetChargesGroup("COMM");
            Variables.ChargeGroupCOMMId = chargesGroup != null ? chargesGroup.Id : "";
            Variables.ChargeGroupCOMMCode = chargesGroup != null ? chargesGroup.Code : "";
            Variables.ChargeTypeAFTId = GetChargeType("AFT");
            Variables.VesselPTId = GetVessel("PT");
            Variables.PackageTypePC1Id = GetPackageType("PC1", "O", true);
            Variables.PackageTypePC2Id = GetPackageType("PC2", "O", true);
            Variables.PackageTypePP1Id = GetPackageType("PP1", "A", false);
            Variables.PackageTypePP2Id = GetPackageType("PP2", "A", false);
            Variables.PaymentTermCashId = GetPaymentTerm("Cash");
            Variables.VATTypeZeroId = GetVATType("ZERO");
            Variables.QuoteStageQTDRId = GetQuoteStage("QTDR");
            Variables.MoveTypeMTAId = GetMoveType("MTA", "A");
            Variables.MoveTypeMTOId = GetMoveType("MTO", "O");
            Variables.ChargesTypes = this.FillChargesTypes();
            Variables.VatTypes = this.FillVatTayes();
            Variables.Currencies = this.FillCurrencies();
            Variables.Rates = this.FillRates();
            return Variables;

        }
        private string GetCurrency(string code)
        {
            Currency currency = CurrencyRepository.GetSingleCurrencyByCode(code, tenant, false);
            return currency == null ? CopyCurrencyFromTenantZero(code) : currency.Id;
        }

        private string CopyCurrencyFromTenantZero(string code) // we need to fix this method Abdullah.b
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

        private ChargesGroup GetChargesGroup(string code)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
            ChargesGroupRepository chargesGroupRepository = new ChargesGroupRepository(webFreightContext);
            return chargesGroupRepository.GetSingleChargesGroupByCode(code, tenant);
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
            chargesTypePM.ChargesGroupId = Variables.ChargeGroupCOMMId;
            chargesTypePM.ChargesGroupCode = Variables.ChargeGroupCOMMCode;
            chargesTypePM.MeasurementId = Variables.MeasurementGRWTId;
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
        private string GetQuoteStage(string code)
        {
            QuoteStageRepository quoteStageRepository = new QuoteStageRepository(quoteContext);
            QuoteStage quoteStage = quoteStageRepository.GetSingleQuoteStageByCode(code, tenant);
            return quoteStage != null ? quoteStage.Id : "";
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
}
