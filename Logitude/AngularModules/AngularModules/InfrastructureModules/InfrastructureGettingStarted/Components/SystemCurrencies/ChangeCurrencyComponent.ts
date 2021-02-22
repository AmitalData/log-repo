import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { TenantPMService } from '../../../../Common/Services/StandardPMs/TenantPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ShipmentDomainService } from '../../../../Shipment/Services/ShipmentDomainService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { LastRate, CurrencyRatesService, ChangeCurrencyArgs } from '../../../../Common/Services/CurrencyRatesService';
import { RatesTablePM } from '../../../../Infrastructure/EntityPMs/RatesTablePM';
import { CurrencyListService } from '../../../../Common/Services/StandardLists/CurrencyListService';
import { CurrencyList } from '../../../../Common/EntityLists/CurrencyList';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';

@Component({
    selector: 'ChangeCurrencyComponent',
    templateUrl: './ChangeCurrencyComponent.html',
})

export class ChangeCurrencyComponent extends BaseComponent {
    public TenantPM: TenantPM = null;
    public Type: string;
    public ValidationErrorsList: string[];
    public DataContext: ChangeCurrencyComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    private oldCurrencyId: string;
    public ItemsSource: RatesItem[] = [];
    private currencyRatesService: CurrencyRatesService;
    constructor() {
        super();
        this.currencyRatesService = new CurrencyRatesService()
    }

    SetWindowArgs(args: any) {
        this.TenantPM = args["TenantPM"];
        this.Type = args["Type"];

        this.SetOldCurrency();

        switch (this.Type) {
            case "Accounting":
                {
                    this.LoadTenantCurrencies();
                    break
                }

            case "Profit":
                {
                    this.LoadProfitCurrencyRate();
                    break
                }
        }        
    }

    private SetOldCurrency() {
        switch (this.Type) {
            case "Accounting":
                {
                    this.oldCurrencyId = this.TenantPM.CurrencyId;
                    break
                }

            case "Profit":
                {
                    this.oldCurrencyId = this.TenantPM.ProfitCurrencyId;
                    break
                }
        }
    }

    private currencies: CurrencyList[] = [];
    private profitCurrencyLastRate: LastRate;
    private LoadTenantCurrencies() {
        var service: CurrencyListService = new CurrencyListService();
        service.getAllFromCache().subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.currencies = response.Result;
            }
        });
    }
    private LoadProfitCurrencyRate() {
        this.currencyRatesService.GetProfitCurrencyLastRate(this.TenantPM.CurrencyId, this.NewCurrencyId, DateTool.GetCurrentDateTimeAsUtc()).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.profitCurrencyLastRate = response.Result;
                if (this.profitCurrencyLastRate) {
                    this.ProfitCurrencyRate = this.profitCurrencyLastRate.Rate;
                }
            }
        });
    }

    private BuildRates() {
        this.ItemsSource = [];

        this.currencies.forEach(item => {
            if (item.Id != this.NewCurrencyId) {
                var itemData: RatesItem = new RatesItem(item);
                this.ItemsSource.push(itemData);
            }
        });
    }

    private newCurrencyId: string;
    get NewCurrencyId() { return this.newCurrencyId; }
    set NewCurrencyId(value: string) {
        if (this.newCurrencyId != value) {
            this.newCurrencyId = value;

            if (value) {
                if (this.Type == "Profit") {
                    this.LoadProfitCurrencyRate();
                }

                else {
                    this.BuildRates();
                }
            }
        }
    }

    private newCurrency: CurrencyList;
    get NewCurrency() { return this.newCurrency; }
    set NewCurrency(value: CurrencyList) {
        if (this.newCurrency != value) {
            this.newCurrency = value;
        }

        if (!AppTool.IsNullOrEmpty(value)) {
            this.NewCurrencyCode = value.Code;
        }
        else {
            this.NewCurrencyCode = null;
        }
    }

    private newCurrencyCode: string;
    get NewCurrencyCode() { return this.newCurrencyCode; }
    set NewCurrencyCode(value: string) {
        if (this.newCurrencyCode != value) {
            this.newCurrencyCode = value;
        }
    }

    private profitCurrencyRate: number;
    get ProfitCurrencyRate() { return this.profitCurrencyRate; }
    set ProfitCurrencyRate(value: number) {
        if (this.profitCurrencyRate != value) {
            this.profitCurrencyRate = value;            
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.NewCurrencyId)) {
            errors.push("New currency is required");
        }

        else {
            if (this.NewCurrencyId == this.oldCurrencyId) {
                errors.push("You have to select different currency");
            }

            else {
                if (this.ItemsSource.filter(d => AppTool.IsNullOrZero(d.Rate)).length > 0) {
                    errors.push("Some rates are missing");
                }
            }
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            var myRates: LastRate[] = [];
            this.ItemsSource.forEach(item => {
                var lastRateItem: LastRate = new LastRate();
                lastRateItem.Tenant = this.TenantPM.Id;
                lastRateItem.BaseCurrencyId = this.NewCurrencyId;
                lastRateItem.ForeignCurrencyId = item.Id;
                lastRateItem.LogDateTime = DateTool.GetCurrentDateTimeAsUtc();
                lastRateItem.ValueDate = DateTool.GetCurrentDateTimeAsUtc();
                lastRateItem.Rate = item.Rate;
                myRates.push(lastRateItem);
            });
                       
            var myArgs = new ChangeCurrencyArgs();
            myArgs.Type = this.Type;
            myArgs.NewCurrencyId = this.NewCurrencyId;
            myArgs.NewCurrencyCode = this.NewCurrencyCode;
            myArgs.LastRates = myRates;
            myArgs.ProfitCurrencyRate = this.ProfitCurrencyRate;

            this.currencyRatesService.PostChangeCurrency(myArgs).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Show("Please logout and login again to refresh data");
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }
}

export class RatesItem extends BaseComponent{    
    public DataContext: RatesItem = this;
    private Currency: CurrencyList;
    constructor(currency: CurrencyList) {
        super();
        this.Currency = currency;
    }

    get Code() {
        return this.Currency.Code;
    }

    get Id() {
        return this.Currency.Id;
    }

    private rate: number;
    get Rate() {
        return this.rate;
    }
    set Rate(value: number) {
        this.rate = value;
    }
}
