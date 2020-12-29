import { Output, EventEmitter } from '@angular/core';
import { QuotePM } from '../../../Quote/EntityPMs/QuotePM';
import { QuoteChargePM } from '../../../Quote/EntityPMs/QuoteChargePM';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { QuoteUtilities } from '../../../Quote/Utilities/QuoteUtilities';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
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

    OnQuoteSaleCurrencyModeChanged() {

    }
}
