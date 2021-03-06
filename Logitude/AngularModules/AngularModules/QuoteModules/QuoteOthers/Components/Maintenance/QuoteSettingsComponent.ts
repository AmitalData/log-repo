import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {QuoteSettingPM} from '../../../../Quote/EntityPMs/QuoteSettingPM';
import {QuoteDomainService} from '../../../../Quote/Services/QuoteDomainService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { FeatureToggleList } from '../../../../Infrastructure/EntityLists/FeatureToggleList';

@Component({    
    templateUrl: './QuoteSettingsComponent.html',
})

export class QuoteSettingsComponent extends BaseComponent {
    public EntityPM: QuoteSettingPM;
    public ObjectTableName = "QuoteSetting";
    public DataContext = this;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    public SaleCurrencySettings: CodeNameClass[] = [];
    private myService: QuoteDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    private MultiCurrencyToggleFeature: FeatureToggleList;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.MultiCurrencyToggleFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "QMC")[0]

        this.SaleCurrencySettings.push(new CodeNameClass("F", "Fixed"));
        this.SaleCurrencySettings.push(new CodeNameClass("S", "Same as cost currency"));

        if (this.MultiCurrencyToggleFeature) {
            this.SaleCurrencySettings.push(new CodeNameClass("M", "Multi Currency"));
        }

        this.myService = new QuoteDomainService();

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res1: any) => {
            this.myService.GetQuoteSettings().subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.EntityPM = myResponse.Result;

                    if (!this.EntityPM) {
                        this.EntityPM = new QuoteSettingPM();
                        this.EntityPM.Tenant = SessionLocator.Tenant;
                    }

                    if (this.EntityPM.IsSaleAsCostCurrency) {
                        this.selectedSaleCurrencySetting = this.SaleCurrencySettings.filter(f => f.Code == "S")[0];
                    }

                    else if (this.EntityPM.IsMultiCurrency && this.MultiCurrencyToggleFeature) {
                        this.selectedSaleCurrencySetting = this.SaleCurrencySettings.filter(f => f.Code == "M")[0];
                    }

                    else {
                        this.selectedSaleCurrencySetting = this.SaleCurrencySettings.filter(f => f.Code == "F")[0];
                    }
                    
                    this.IsResourcesReady = true;
                }
            });
        });
    }

    get CopyShipper() { return this.EntityPM.CopyShipper; }
    set CopyShipper(value: boolean) {
        if (this.EntityPM.CopyShipper != value) {
            this.EntityPM.CopyShipper = value;
        }
    }

    get CopyConsignee() { return this.EntityPM.CopyConsignee; }
    set CopyConsignee(value: boolean) {
        if (this.EntityPM.CopyConsignee != value) {
            this.EntityPM.CopyConsignee = value;
        }
    }

    get CopyAgent() { return this.EntityPM.CopyAgent; }
    set CopyAgent(value: boolean) {
        if (this.EntityPM.CopyAgent != value) {
            this.EntityPM.CopyAgent = value;
        }
    }

    get CopyNotify() { return this.EntityPM.CopyNotify; }
    set CopyNotify(value: boolean) {
        if (this.EntityPM.CopyNotify != value) {
            this.EntityPM.CopyNotify = value;
        }
    }

    get CopyMainCarriage() { return this.EntityPM.CopyMainCarriage; }
    set CopyMainCarriage(value: boolean) {
        if (this.EntityPM.CopyMainCarriage != value) {
            this.EntityPM.CopyMainCarriage = value;
        }
    }

    get CopyPickup() { return this.EntityPM.CopyPickup; }
    set CopyPickup(value: boolean) {
        if (this.EntityPM.CopyPickup != value) {
            this.EntityPM.CopyPickup = value;
        }
    }

    get CopyDelivery() { return this.EntityPM.CopyDelivery; }
    set CopyDelivery(value: boolean) {
        if (this.EntityPM.CopyDelivery != value) {
            this.EntityPM.CopyDelivery = value;
        }
    }

    get CopyChargesTypes() { return this.EntityPM.CopyChargesTypes; }
    set CopyChargesTypes(value: boolean) {
        if (this.EntityPM.CopyChargesTypes != value) {
            this.EntityPM.CopyChargesTypes = value;
        }
    }

    get CopyChargesCost() { return this.EntityPM.CopyChargesCost; }
    set CopyChargesCost(value: boolean) {
        if (this.EntityPM.CopyChargesCost != value) {
            this.EntityPM.CopyChargesCost = value;
        }
    }

    get CopyChargesSale() { return this.EntityPM.CopyChargesSale; }
    set CopyChargesSale(value: boolean) {
        if (this.EntityPM.CopyChargesSale != value) {
            this.EntityPM.CopyChargesSale = value;
        }
    }

    get CopyExchangeRates() { return this.EntityPM.CopyExchangeRates; }
    set CopyExchangeRates(value: boolean) {
        if (this.EntityPM.CopyExchangeRates != value) {
            this.EntityPM.CopyExchangeRates = value;
        }
    }

    get EditMainCarriage() { return this.EntityPM.EditMainCarriage; }
    set EditMainCarriage(value: boolean) {
        if (this.EntityPM.EditMainCarriage != value) {
            this.EntityPM.EditMainCarriage = value;
        }
    }

    get IsSaleAsCostCurrency() { return this.EntityPM.IsSaleAsCostCurrency; }
    set IsSaleAsCostCurrency(value: boolean) {
        if (this.EntityPM.IsSaleAsCostCurrency != value) {
            this.EntityPM.IsSaleAsCostCurrency = value;
        }
    }

    get IsMultiCurrency() { return this.EntityPM.IsMultiCurrency; }
    set IsMultiCurrency(value: boolean) {
        if (this.EntityPM.IsMultiCurrency != value) {
            this.EntityPM.IsMultiCurrency = value;
        }
    }

    get AutomaticallyCloseDays() { return this.EntityPM.AutomaticallyCloseDays; }
    set AutomaticallyCloseDays(value: number) {
        if (this.EntityPM.AutomaticallyCloseDays != value) {
            this.EntityPM.AutomaticallyCloseDays = value;
        }
    }

    private selectedSaleCurrencySetting: CodeNameClass;
    get SelectedSaleCurrencySetting() { return this.selectedSaleCurrencySetting; }
    set SelectedSaleCurrencySetting(value: CodeNameClass) {
        if (this.selectedSaleCurrencySetting != value) {
            this.selectedSaleCurrencySetting = value;

            var isSaleAsCostCurrency: boolean = false;
            var isMultiCurrency: boolean = false;

            if (value) {
                if (value.Code == "S") {
                    isSaleAsCostCurrency = true;
                }

                else if (value.Code == "M") {
                    isMultiCurrency = true;
                }
            }

            this.IsSaleAsCostCurrency = isSaleAsCostCurrency;
            this.IsMultiCurrency = isMultiCurrency;
        }
    }
    
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];

        var isAnyOptionChecked: boolean = this.ValidateAnyOptionIsChecked();

        if (isAnyOptionChecked == false) {
            errors.push("One Option at least  should be selected");
        }

        if (this.AutomaticallyCloseDays == 0) {
            errors.push("Automatically Close Days field should not be Zero");
        }

        if (this.AutomaticallyCloseDays == null) {
            errors.push("Automatically Close Days field is required");
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            this.CurrentSession.StartBusyIndicatorSaving();

            this.myService.UpdateQuoteSettings(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
    }
    ValidateAnyOptionIsChecked() {
        var myResult: boolean = false;

        if (this.CopyShipper) {
            myResult = true;
        }

        else if (this.CopyConsignee) {
            myResult = true;
        }

        else if (this.CopyAgent) {
            myResult = true;
        }

        else if (this.CopyNotify) {
            myResult = true;
        }

        else if (this.CopyMainCarriage) {
            myResult = true;
        }

        else if (this.CopyPickup) {
            myResult = true;
        }

        else if (this.CopyDelivery) {
            myResult = true;
        }

        else if (this.CopyChargesTypes) {
            myResult = true;
        }

        else if (this.CopyChargesCost) {
            myResult = true;
        }

        else if (this.CopyChargesSale) {
            myResult = true;
        }

        else if (this.EditMainCarriage) {
            myResult = true;
        }

        else if (this.AutomaticallyCloseDays != null) {
            myResult = true;
        }

        return myResult;
    }
}
