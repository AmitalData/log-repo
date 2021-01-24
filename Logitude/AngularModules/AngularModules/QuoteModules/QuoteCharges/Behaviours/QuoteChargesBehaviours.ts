import { Output, EventEmitter } from '@angular/core';
import { QuotePM } from '../../../Quote/EntityPMs/QuotePM';
import { QuoteChargePM } from '../../../Quote/EntityPMs/QuoteChargePM';
import { QuoteTotalVATPM } from '../../../Quote/EntityPMs/QuoteTotalVATPM';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { QuoteUtilities } from '../../../Quote/Utilities/QuoteUtilities';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { VatTypeList } from '../../../Common/EntityLists/VatTypeList';
import { CurrencyList } from '../../../Common/EntityLists/CurrencyList';
import { ChargesTypeList } from '../../../Common/EntityLists/ChargesTypeList';
import { MeasurementList } from '../../../Common/EntityLists/MeasurementList';
import { PackageTypeList } from '../../../Common/EntityLists/PackageTypeList';
import { CardListService } from '../../../Common/Services/StandardLists/CardListService';
import { VatTypeListService } from '../../../Common/Services/StandardLists/VatTypeListService';
import { CurrencyListService } from '../../../Common/Services/StandardLists/CurrencyListService';
import { ChargesTypeListService } from '../../../Common/Services/StandardLists/ChargesTypeListService';
import { MeasurementListService } from '../../../Common/Services/StandardLists/MeasurementListService';
import { PackageTypeListService } from '../../../Common/Services/StandardLists/PackageTypeListService';
import { CommonDomainService } from '../../../Common/Services/CommonDomainService';
import { CurrencyRatesService, LastRate } from '../../../Common/Services/CurrencyRatesService';
import { VatTypePercentagePM } from '../../../Common/EntityPMs/VatTypePercentagePM';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

export class QuoteChargesBehaviours {
    public EntityPM: QuotePM = null;
    @Output() DeleteChargeCompleted = new EventEmitter();
    @Output() PackageTypesLoadCompleted = new EventEmitter();

    constructor(entityPM: QuotePM) {
        this.EntityPM = entityPM;

        this.Translate();
        this.InitializeServices();
        this.LoadRequiredData();
    }

    AddChargeLabel: string = "";
    EditChargeLabel: string = "";

    Translate() {
        this.AddChargeLabel = TextCodeTranslator.Translate("Quote.O.Charges.AddCharges");
        this.EditChargeLabel = TextCodeTranslator.Translate("Quote.O.Charges.EditCharges");

    }

    CardListService: CardListService;
    VatTypeListService: VatTypeListService;
    CurrencyListService: CurrencyListService;
    ChargesTypeListService: ChargesTypeListService;
    MeasurementListService: MeasurementListService;
    PackageTypeListService: PackageTypeListService;
    CurrencyRatesService: CurrencyRatesService;
    CommonDomainService: CommonDomainService;
    private InitializeServices() {
        this.CardListService = new CardListService();
        this.VatTypeListService = new VatTypeListService();
        this.CurrencyListService = new CurrencyListService();
        this.ChargesTypeListService = new ChargesTypeListService();
        this.MeasurementListService = new MeasurementListService();
        this.PackageTypeListService = new PackageTypeListService();
        this.CurrencyRatesService = new CurrencyRatesService();
        this.CommonDomainService = new CommonDomainService();
    }

    AllRates: LastRate[] = [];
    AllCurrencies: CurrencyList[] = [];
    AllMeasurements: MeasurementList[] = [];
    AllVatTypes: VatTypeList[] = [];
    AllVatPercentages: VatTypePercentagePM[] = [];
    AllChargesTypes: ChargesTypeList[] = [];
    AllPackageTypes: PackageTypeList[] = [];
    LoadRequiredData() {
        var isEditingEnabled = QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);

        if (isEditingEnabled) {
            this.LoadCurrencyRates();
            this.LoadVatPercentages();
        }

        this.LoadCurrencies();
        this.LoadMeasurements();
        this.LoadVatTypes();
        this.LoadChargesTypes();
        this.LoadPackageTypes();
    }
    private LoadCurrencyRates() {
        this.CurrencyRatesService.getAll(SessionLocator.LocalCurrencyId, DateTool.GetCurrentDateAsUtc()).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllRates = myResponse.Result;
            }
        });
    }
    private LoadVatPercentages() {
        this.CommonDomainService.GetVatTypePercentagePMByDate(DateTool.GetCurrentDateAsUtc()).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllVatPercentages = myResponse.Result;
            }
        });
    }
    private LoadCurrencies() {
        this.CurrencyListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllCurrencies = myResponse.Result;
            }
        });
    }
    private LoadMeasurements() {
        this.MeasurementListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllMeasurements = myResponse.Result;
            }
        });
    }
    private LoadVatTypes() {
        this.VatTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllVatTypes = myResponse.Result;
            }
        });
    }
    private LoadChargesTypes() {
        this.ChargesTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllChargesTypes = myResponse.Result;
            }
        });
    }
    private LoadPackageTypes() {
        this.PackageTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllPackageTypes = myResponse.Result;
                this.PackageTypesLoadCompleted.emit();
            }
        });
    }

    IsRegionalTaxVisible() {
        var isVisible: boolean = false;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.RegionalTaxId)) {
            isVisible = true;
        }

        else if (FeatureLocator.HasFeaturePermession("General", "REGIONALTAX")) {
            if (SessionLocator.AccountingSettingPM.AllowRegionalTaxManagement) {
                isVisible = true;
            }
        }

        return isVisible;
    }

    GetVatTypePercentage(vatTypeId: string) {
        var myResult: number = null;

        var vatTypePercentagePM = this.AllVatPercentages.filter(d => d.VatTypeId == vatTypeId)[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }

        return myResult;
    }
    GetCurrencyCode(myCurrencyId: string) {
        var myCode = null;

        if (!AppTool.IsNullOrEmpty(myCurrencyId)) {
            var list: CurrencyList = this.AllCurrencies.filter(d => d.Id == myCurrencyId)[0];
            if (list != null) {
                myCode = list.Code;
            }
        }

        return myCode;
    }
    GetCurrencyRate(myCurrencyId: string) {
        var myResult = null;

        if (!AppTool.IsNullOrEmpty(myCurrencyId)) {
            if (myCurrencyId == SessionLocator.LocalCurrencyId) {
                myResult = 1;
            }

            else if (myCurrencyId == this.EntityPM.SaleCurrencyId) {
                myResult = this.EntityPM.ExchangeRate;
            }

            else {
                var lastRate: LastRate = this.AllRates.filter(d => d.ForeignCurrencyId == myCurrencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.Rate;
                }
            }
        }

        return myResult;
    }
    GetQuoteSaleCurrencyRate(myCurrencyId: string) {
        var myResult = null;

        if (!AppTool.IsNullOrEmpty(myCurrencyId)) {
            if (myCurrencyId == SessionLocator.LocalCurrencyId) {
                myResult = 1;
            }

            else {
                var lastRate: LastRate = this.AllRates.filter(d => d.ForeignCurrencyId == myCurrencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.Rate;
                }
            }
        }

        return myResult;
    }
    GetCostCurrencyOnChargeTypeChanged(list: ChargesTypeList) {

        var output: string = null;

        if (list) {
            if (!AppTool.IsNullOrEmpty(list.PayablesDefaultCurrencyId)) {
                output = list.PayablesDefaultCurrencyId;
            }

            else {
                if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                    output = SessionLocator.TenantPM.FreightCurrencyId;
                }

                else {
                    output = SessionLocator.TenantPM.OtherChargesCurrencyId;
                }
            }
        }

        return output;
    }
    GetSaleCurrencyOnChargeTypeChanged(list: ChargesTypeList, itemPM: QuoteChargePM) {

        var output: string = null;

        if (this.EntityPM.IsSaleCurrencySameAsCost) {
            output = itemPM.CostCurrencyId;
        }

        else if (this.EntityPM.IsMultiCurrency) {
            if (list) {
                if (!AppTool.IsNullOrEmpty(list.ReceivablesDefaultCurrencyId)) {
                    output = list.ReceivablesDefaultCurrencyId;
                }

                else {
                    if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                        output = SessionLocator.TenantPM.FreightCurrencyId;
                    }

                    else {
                        output = SessionLocator.TenantPM.OtherChargesCurrencyId;
                    }
                }
            }
        }

        else {
            output = this.EntityPM.SaleCurrencyId;
        }

        return output;
    }
    GetCurrencyRateDate(myCurrencyId: string) {
        var myResult = null;

        if (!AppTool.IsNullOrEmpty(myCurrencyId)) {
            if (myCurrencyId == SessionLocator.LocalCurrencyId) {
                myResult = null;
            }

            else {
                var lastRate: LastRate = this.AllRates.filter(d => d.ForeignCurrencyId == myCurrencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.ValueDate;
                }
            }
        }

        return myResult;
    }
    CreateQuoteCharge(): QuoteChargePM {
        var newItem = new QuoteChargePM(null);
        newItem.Tenant = SessionLocator.Tenant;
        newItem.QuoteId = this.EntityPM.Id;
        newItem.UpdatedByUserId = SessionLocator.LoggedUserId;
        newItem.MarkUpTypeCode = "F";
        newItem.MarkUpValue = 0;
        newItem.QuoteTypeCode = this.EntityPM.QuoteTypeCode;
        newItem.SaleCurrencyId = this.EntityPM.SaleCurrencyId;
        newItem.SaleCurrencyCode = this.EntityPM.SaleCurrencyCode;
        newItem.SaleExchangeRate = this.EntityPM.ExchangeRate;
        newItem.IsAllIN = false;
        newItem.CostIsFixedRate = false;
        newItem.SaleIsFixedRate = false;
        newItem.IsChargeBySteps = false;
        return newItem;
    }
    DeleteCharge(itemPM: QuoteChargePM) {

        if ((itemPM.ChargesGroupCode == "FRT" && this.EntityPM.QuoteCharges.filter(d => d.IsAllIN).length > 0) ||
            (itemPM.ChargesGroupCode == "FRT" && this.EntityPM.QuoteCharges.filter(d => d.IsCostAllIn).length > 0)) {
            var window = new MessageWindow();
            window.Show("Can't delete this charge because it's connected to other All In charges");
        }

        else if (itemPM.IsAllIN) {
            var window = new MessageWindow();
            window.Show("Can't delete this charge because it's All In");
        }

        else {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator.Translate("Quote.M.DeleteThisCharge"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.EntityPM.RemoveQuoteChargePM(itemPM);
                    this.DeleteChargeCompleted.emit();
                }
            });
        }
    }
    BuildTotalVATs() {
        this.EntityPM.TotalVATs = [];

        if (this.EntityPM.QuoteTypeCode == "A") {
            if (this.EntityPM.IsChargesByVAT) {
                var myCharges: QuoteChargePM[] = this.EntityPM.QuoteCharges.filter(f => f.IsAllIN == false && f.VatTypeId != null);
                if (myCharges.length > 0) {

                    var entityRegionalTaxPercentage: number = 0;

                    if (this.EntityPM.RegionalTaxPercentage) {
                        entityRegionalTaxPercentage = this.EntityPM.RegionalTaxPercentage;
                    }

                    // Build Group Source
                    var group_Source: QuoteTotalVATPM[] = [];
                    myCharges.forEach(item => {
                        var lineVatType = this.AllVatTypes.filter(f => f.Id == item.VatTypeId)[0];
                        if (lineVatType) {

                            if (AppTool.IsNullOrEmpty(item.SaleTotalAmount)) {
                                item.SaleTotalAmount = 0;
                            }

                            if (AppTool.IsNullOrEmpty(item.SaleTotalAmountLocal)) {
                                item.SaleTotalAmountLocal = 0;
                            }

                            if (AppTool.IsNullOrEmpty(item.SaleAmountInSaleCurrency)) {
                                item.SaleAmountInSaleCurrency = 0;
                            }

                            if (!lineVatType.IsMultiPercentage) {
                                if (item.VatPercentage != null) {
                                    var myQroupItem = new QuoteTotalVATPM(null);
                                    myQroupItem.Tenant = SessionLocator.Tenant;
                                    myQroupItem.QuoteId = this.EntityPM.Id;
                                    myQroupItem.Id = item.VatTypeId;
                                    myQroupItem.VatTypeId = item.VatTypeId;
                                    myQroupItem.VatPercent = item.VatPercentage;
                                    myQroupItem.ExternalVATCard = lineVatType.ReceivablesExternalId;
                                    myQroupItem.ExternalTAXItemId = lineVatType.ExternalTAXItemId;
                                    myQroupItem.QuoteCurrencyVatableAmount = item.SaleAmountInSaleCurrency;
                                    myQroupItem.LocalCurrencyVatableAmount = item.SaleTotalAmountLocal;

                                    if (item.IsRegionalTax) {
                                        myQroupItem.QuoteCurrencyVatableAmount = item.SaleAmountInSaleCurrency + item.SaleAmountInSaleCurrency * (entityRegionalTaxPercentage / 100);
                                        myQroupItem.LocalCurrencyVatableAmount = item.SaleTotalAmountLocal + item.SaleTotalAmountLocal * (entityRegionalTaxPercentage / 100);

                                        var regionalTaxItem = new QuoteTotalVATPM(null);
                                        regionalTaxItem.Tenant = SessionLocator.Tenant;
                                        regionalTaxItem.QuoteId = this.EntityPM.Id;
                                        regionalTaxItem.Id = this.EntityPM.RegionalTaxId;
                                        regionalTaxItem.VatTypeId = this.EntityPM.RegionalTaxId;
                                        regionalTaxItem.VatPercent = entityRegionalTaxPercentage;
                                        regionalTaxItem.ExternalVATCard = lineVatType.ReceivablesExternalId;
                                        regionalTaxItem.ExternalTAXItemId = lineVatType.ExternalTAXItemId;
                                        regionalTaxItem.QuoteCurrencyVatableAmount = item.SaleAmountInSaleCurrency;
                                        regionalTaxItem.LocalCurrencyVatableAmount = item.SaleTotalAmountLocal;
                                        group_Source.push(regionalTaxItem);
                                    }

                                    group_Source.push(myQroupItem);
                                }
                            }

                            else {
                                var myVatGroups = SessionLocator.AllVatTypesGroups.filter(f => f.GroupVATTypeId == item.VatTypeId);

                                myVatGroups.forEach(itemGroup => {

                                    var lineSingleVatType = this.AllVatTypes.filter(f => f.Id == itemGroup.SingleVATTypeId)[0];

                                    if (lineSingleVatType) {

                                        var myQroupItem = new QuoteTotalVATPM(null);
                                        myQroupItem.Tenant = SessionLocator.Tenant;
                                        myQroupItem.QuoteId = this.EntityPM.Id;
                                        myQroupItem.Id = itemGroup.SingleVATTypeId;
                                        myQroupItem.VatTypeId = itemGroup.SingleVATTypeId;
                                        myQroupItem.VatPercent = this.GetVatTypePercentage(itemGroup.SingleVATTypeId);
                                        myQroupItem.ExternalVATCard = lineSingleVatType.ReceivablesExternalId;
                                        myQroupItem.ExternalTAXItemId = lineSingleVatType.ExternalTAXItemId;
                                        myQroupItem.QuoteCurrencyVatableAmount = item.SaleAmountInSaleCurrency;
                                        myQroupItem.LocalCurrencyVatableAmount = item.SaleTotalAmountLocal;
                                        group_Source.push(myQroupItem);
                                    }
                                });
                            }
                        }
                    });

                    // Build Grouped Data
                    var group_data: QuoteTotalVATPM[] = [];
                    group_Source.forEach(item => {
                        var record: QuoteTotalVATPM = group_data.filter(f => f.VatTypeId == item.VatTypeId && f.VatPercent == item.VatPercent && f.ExternalVATCard == item.ExternalVATCard && f.ExternalTAXItemId == item.ExternalTAXItemId)[0];
                        if (record) {
                            record.QuoteCurrencyVatableAmount += item.QuoteCurrencyVatableAmount;
                            record.LocalCurrencyVatableAmount += item.LocalCurrencyVatableAmount;
                        }

                        else {
                            record = new QuoteTotalVATPM(null);
                            record.Id = item.Id;
                            record.VatTypeId = item.VatTypeId;
                            record.VatPercent = item.VatPercent;
                            record.ExternalVATCard = item.ExternalVATCard;
                            record.ExternalTAXItemId = item.ExternalTAXItemId;
                            record.QuoteCurrencyVatableAmount = item.QuoteCurrencyVatableAmount;
                            record.LocalCurrencyVatableAmount = item.LocalCurrencyVatableAmount;
                            group_data.push(record);
                        }
                    });

                    // Build Quote Total VATs
                    group_data.forEach(item => {

                        var itemVatType = this.AllVatTypes.filter(f => f.Id == item.VatTypeId)[0];

                        var itemTotalVAT = new QuoteTotalVATPM(null);
                        itemTotalVAT.Tenant = SessionLocator.Tenant;
                        itemTotalVAT.QuoteId = this.EntityPM.Id;
                        itemTotalVAT.VatTypeId = item.VatTypeId;
                        itemTotalVAT.VatTypeName = itemVatType ? itemVatType.EnglishName : "";
                        itemTotalVAT.VatPercent = AppTool.Round(item.VatPercent, 3);
                        itemTotalVAT.VatTypeCell = itemTotalVAT.VatTypeName + " (" + itemTotalVAT.VatPercent + "%)";
                        itemTotalVAT.ExternalVATCard = item.ExternalVATCard;
                        itemTotalVAT.ExternalTAXItemId = item.ExternalTAXItemId;
                        itemTotalVAT.LocalCurrencyVatableAmount = AppTool.Round(item.LocalCurrencyVatableAmount, 2);
                        itemTotalVAT.QuoteCurrencyVatableAmount = AppTool.Round(item.QuoteCurrencyVatableAmount, 2);
                        itemTotalVAT.LocalCurrencyVATAmount = AppTool.Round((itemTotalVAT.LocalCurrencyVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                        itemTotalVAT.QuoteCurrencyVATAmount = AppTool.Round((itemTotalVAT.QuoteCurrencyVatableAmount * itemTotalVAT.VatPercent / 100), 2);
                        this.EntityPM.AddQuoteTotalVATPM(itemTotalVAT);
                    });
                }
            }
        }
    }

    ComputeMarkUp(itemPM: QuoteChargePM, index: number = 0) {

        var value: number = itemPM.MarkUpValue;
        var valueCode: string = itemPM.MarkUpTypeCode;
        var costPrice: number = itemPM.CostUnitPriceInSaleCurrency;
        var salePrice: number = itemPM.SaleUnitPrice;

        if (index > 0) {
            value = itemPM['ContainerType' + index + 'MarkUpValue'];
            valueCode = itemPM['ContainerType' + index + 'MarkUpTypeCode'];
            costPrice = itemPM['CostUnitPrice' + index + 'InSaleCurrency'];
            salePrice = itemPM['SaleContainerType' + index + 'UnitPrice'];
        }

        if (!AppTool.IsNullOrEmpty(costPrice) && !AppTool.IsNullOrEmpty(salePrice)) {
            if (valueCode == "P") {
                value = ((salePrice - costPrice) * 100) / costPrice;
            }

            else {
                value = salePrice - costPrice;
            }
        }

        if (value == null) {
            value = 0;
        }

        return value;
    }
    GetMarkUpString(itemPM: QuoteChargePM, index: number = 0) {

        var output: string = null;

        var value: number = itemPM.MarkUpValue;
        var valueCode: string = itemPM.MarkUpTypeCode;
        var costPrice: number = itemPM.CostUnitPriceInSaleCurrency;
        var salePrice: number = itemPM.SaleUnitPrice;

        if (index > 0) {
            value = itemPM['ContainerType' + index + 'MarkUpValue'];
            valueCode = itemPM['ContainerType' + index + 'MarkUpTypeCode'];
            costPrice = itemPM['CostUnitPrice' + index + 'InSaleCurrency'];
            salePrice = itemPM['SaleContainerType' + index + 'UnitPrice'];
        }

        if (!AppTool.IsNullOrZero(value)) {
            if (AppTool.IsNullOrEmpty(costPrice)) {
                costPrice = 0;
            }

            if (AppTool.IsNullOrEmpty(salePrice)) {
                salePrice = 0;
            }

            if (salePrice > costPrice) {
                output = "+" + value;
            }

            else if (salePrice < costPrice) {
                if (value < 0) {
                    output = "" + value;
                }

                else {
                    output = "-" + value;
                }
            }

            else {
                output = "+" + value;
            }

            if (valueCode == "P") {
                output = output + "%";
            }
        }

        return output;
    }
    ComputeCostPriceInSaleCurrency(itemPM: QuoteChargePM, index: number = 0) {

        var output: number = null

        var itemCostPrice: number = itemPM.CostUnitPrice;

        if (index > 0) {
            itemCostPrice = itemPM['CostContainerType' + index + 'UnitPrice'];
        }

        if (itemPM.CostCurrencyId == itemPM.SaleCurrencyId) {
            if (!AppTool.IsNullOrEmpty(itemCostPrice)) {
                output = itemCostPrice;
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(itemCostPrice) && !AppTool.IsNullOrEmpty(itemPM.CostExchangeRate) && !AppTool.IsNullOrEmpty(itemPM.SaleExchangeRate)) {
                output = itemCostPrice * itemPM.CostExchangeRate / itemPM.SaleExchangeRate;
            }
        }

        return output;
    }

    OnQuoteSaleCurrencyModeChanged() {

    }

}
