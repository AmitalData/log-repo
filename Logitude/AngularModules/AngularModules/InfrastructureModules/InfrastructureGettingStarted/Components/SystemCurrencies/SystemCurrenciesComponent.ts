import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TenantPMService} from '../../../../Common/Services/StandardPMs/TenantPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    selector: 'SystemCurrenciesComponent',
    templateUrl: './SystemCurrenciesComponent.html',
})

export class SystemCurrenciesComponent extends BaseComponent {
    public DataContext: SystemCurrenciesComponent = this;
    public ObjectTableName: string = "Tenant";
    public TenantPM: TenantPM = null;
    public IsResourcesReady: boolean = false;
    public DemoMessageVisibility: boolean = false;
    public ValidationErrorsList: string[];
    private ShipmentsQuotesCount: number = 0;
    private entityPMService: TenantPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsChangeCurrencyVisible: boolean = false;
    constructor(private entityResourceService: EntityResourceService) {
        super();

        this.entityPMService = new TenantPMService();

        entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((res: any) => {
            this.GetDemoMessageVisibility();

            this.entityPMService.get(SessionLocator.TenantPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.TenantPM = myResponse.Result;


                    var myShipmentDomainService: ShipmentDomainService = new ShipmentDomainService();
                    myShipmentDomainService.GetShipmentsQuotesCount().subscribe((myResponse2: ServiceResponse) => {

                        if (!myResponse2.HasError) {
                            this.ShipmentsQuotesCount = myResponse2.Result;
                        }

                        if (FeatureLocator.HasFeaturePermession("General", "ChangeLocalProfitCurrency")) {
                            this.IsChangeCurrencyVisible = true;
                        }

                        this.SetUIProperties();
                        this.IsResourcesReady = true;
                    });
                }
            });
        });
    }

    GetDemoMessageVisibility() {
        var myResult = false;

        if (ObjectsLocator.IsDemoTenant(SessionLocator.Tenant.toString())) {
            myResult = true;

            if (SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                myResult = false;
            }
        }

        this.DemoMessageVisibility = myResult;
    }

    public IsEditingEnabled: boolean = false;
    SetUIProperties() {

        var isFeildEnabled: boolean = true;

        if (ObjectsLocator.IsDemoTenant(this.TenantPM.Id.toString())) {
            isFeildEnabled = false;

            if (SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                isFeildEnabled = true;
            }
        }

        this.IsEditingEnabled = isFeildEnabled;
        this.UIProperties.SetEnabled("CurrencyId", "Tenant", false);
        this.UIProperties.SetEnabled("ProfitCurrencyId", "Tenant", this.ShipmentsQuotesCount == 0 ? true : false);
        this.UIProperties.SetEnabled("FreightCurrencyId", "Tenant", isFeildEnabled);
        this.UIProperties.SetEnabled("OtherChargesCurrencyId", "Tenant", isFeildEnabled);
        this.UIProperties.SetEnabled("QuoteSaleCurrencyId", "Tenant", isFeildEnabled);
    }

    get CurrencyId() { return this.TenantPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.TenantPM.CurrencyId != value) {
            this.TenantPM.CurrencyId = value;
        }
    }

    get FreightCurrencyId() { return this.TenantPM.FreightCurrencyId; }
    set FreightCurrencyId(value: string) {
        if (this.TenantPM.FreightCurrencyId != value) {
            this.TenantPM.FreightCurrencyId = value;
        }
    }

    get OtherChargesCurrencyId() { return this.TenantPM.OtherChargesCurrencyId; }
    set OtherChargesCurrencyId(value: string) {
        if (this.TenantPM.OtherChargesCurrencyId != value) {
            this.TenantPM.OtherChargesCurrencyId = value;
        }
    }

    get QuoteSaleCurrencyId() { return this.TenantPM.QuoteSaleCurrencyId; }
    set QuoteSaleCurrencyId(value: string) {
        if (this.TenantPM.QuoteSaleCurrencyId != value) {
            this.TenantPM.QuoteSaleCurrencyId = value;
        }
    }

    get ProfitCurrencyId() { return this.TenantPM.ProfitCurrencyId; }
    set ProfitCurrencyId(value: string) {
        if (this.TenantPM.ProfitCurrencyId != value) {
            this.TenantPM.ProfitCurrencyId = value;
        }
    }

    DailyExchangeRates() {
        var windowTitle = "Edit exchange rates";
        var logWindow = new LogitudeWindow();
        logWindow.Title = windowTitle;
        this.entityResourceService.getEntityResourceByTableName("RatesTable").subscribe(response => {
            logWindow.Show('./Common/Components/Maintenance/RatesMainTabComponent');
        });
    }

    EditCurrency() {
        if (this.IsEditingEnabled) {
            var windowTitle = "Edit Accounting Currency";
            var logWindow = new LogitudeWindow();
            logWindow.Title = windowTitle;
            logWindow.Width = 650;
            logWindow.Height = 450;
            logWindow.WindowArgs = { EntityPM: this.TenantPM, ShipmentsQuotesCount: this.ShipmentsQuotesCount };

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.TenantPM = comp.EntityPM;
                    }
                });
            });

            logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/SystemCurrencies/CurrencyRatesComponent');
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked(isChanging: boolean = false) {
        if (!this.TenantPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }

        else {
            var errors: string[] = [];
            Validator.TryValidateObject(this.TenantPM, this.ObjectTableName, errors);

            if (AppTool.IsNullOrEmpty(this.CurrencyId)) {
                errors.push("Accounting Currency Is Required");
            }

            if (AppTool.IsNullOrEmpty(this.ProfitCurrencyId)) {
                errors.push("Profit Currency Is Required");
            }

            this.ValidationErrorsList = errors;

            if (this.ValidationErrorsList.length == 0) {

                this.CurrentSession.StartBusyIndicatorSaving();

                this.entityPMService.update(this.TenantPM).subscribe((myResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();

                    if (!myResponse.HasError) {
                        InfraSettings.TenantPM = myResponse.Result;
                        this.CurrentSession.CloseCurrentWindowEmit("ok");

                        if (isChanging) {
                            this.OpenChangeCurrencyWindow();
                        }
                    }

                    else {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                });
            }
        }
    }

    private Type: string;
    ChangeCurrencyClicked(type: string) {
        this.Type = type;

        if (this.TenantPM.IsDirty) {
            this.OkButtonClicked(true);
        }

        else {
            this.OpenChangeCurrencyWindow();
        }        
    }

    private OpenChangeCurrencyWindow() {
        this.entityResourceService.getEntityResourceByTableName("RatesTable").subscribe(response => {
            var confirmWindow: ConfirmWindow = new ConfirmWindow();
            confirmWindow.Show("Are you sure you want to change " + this.Type + " currency?");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    var logWindow = new LogitudeWindow();
                    logWindow.IsOverWindow = true;
                    logWindow.WindowArgs = { TenantPM: this.TenantPM, Type: this.Type };
                    logWindow.Title = "Change " + this.Type + " Currency";
                    logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/SystemCurrencies/ChangeCurrencyComponent');
                }
            });
        });
    }
}
