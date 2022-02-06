import {Component, OnInit, ViewChild, ViewContainerRef, AfterViewInit} from '@angular/core';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';

@Component({
    
    templateUrl: './CustomerBillingTabComponent.html',
})

export class CustomerBillingTabComponent extends BaseComponent implements OnInit,AfterViewInit {
    public EntityPM: CustomerPM;
    public ObjectTableName: string = "Customer";
    public DataContext = this;
    public HasCreditLimitFeature: boolean = false;
    public HasEditCreditAmountFeature: boolean = false;
    public IsCreditLimitActivated: boolean = false;
    public LocalCurrencyCode: string;
    public IsAccountingActivated: boolean;
    public SatInterfaceSettingCode: string;

    @ViewChild('BillingChild', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    public DisplaySATSettings: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
     

        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;

        this.HasCreditLimitFeature = FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module");
        this.HasEditCreditAmountFeature = FeatureLocator.HasFeaturePermession("Customer", "EDITCREDITAMOUNT");

        if (this.HasCreditLimitFeature) {
            this.IsCreditLimitActivated = ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
            this.SetLabels();
        }

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            this.DisplaySATSettings = true;
            this.SatInterfaceSettingCode = SessionLocator.SATInterfaceSettings.SATInterfaceCode;
        }

        this.Listen();
    }

    ngAfterViewInit(): void {
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this.SetUIProperties();
        this.RunComponent();
        this.LoadCreditLimitData();
    }

    ngOnInit() {
        // this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        // this.SetUIProperties();
        // this.RunComponent();
        // this.LoadCreditLimitData();
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private GeneratedComponent: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    private LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                this.GeneratedComponent = cmpRef.instance;

                cmpRef.instance.LoadCompleted.subscribe(s => {
                    this.SetUIProperties_GeneratedComponent();
                });

                var screenCode = "Customer.BillingTabScreen";
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, screenCode);
            });
    }
    SetUIProperties_GeneratedComponent() {
        if (this.GeneratedComponent) {
            var enabled: boolean = true;
            if (SessionLocator.TenantPM.IsHybrid && (this.EntityPM.CustomerStatusCode == "ACT" || this.EntityPM.CustomerStatusCode == "WAC")) {
                enabled = false;
            }

            this.GeneratedComponent.SetEnabled(enabled);
        }

        if (SessionLocator.TenantPM.IsHybrid === true && this.IsAccountingActivated === true) {

            this.UIProperties.SetEnabled("CreditLimitAmount", this.ObjectTableName, this.HasEditCreditAmountFeature);
            this.EntityPM.UIProperties.SetEnabled("CreditLimitAmount", this.ObjectTableName, this.HasEditCreditAmountFeature);
            this.SetInsuredCreditLimitEnablitity();

        }
    }

    private SetInsuredCreditLimitEnablitity() {
        this.UIProperties.SetEnabled("InsuredcreditLimit", this.ObjectTableName, true);
        this.EntityPM.UIProperties.SetEnabled("InsuredcreditLimit", this.ObjectTableName, true);
    }

    public CreditLimitAmountLabel: string;
    public CreditLimitOpenBalanceLabel: string;
    public CreditLimitActualBalanceLabel: string;

    SetLabels() {
        this.CreditLimitAmountLabel = TextCodeTranslator.Translate('Customer.F.CreditLimitAmount') + " (" + this.LocalCurrencyCode + ")";
        this.CreditLimitOpenBalanceLabel = TextCodeTranslator.Translate('Customer.F.CreditLimitOpenBalance') + " (" + this.LocalCurrencyCode + ")";
        this.CreditLimitActualBalanceLabel = "Actual Balance" + " (" + this.LocalCurrencyCode + "):";
    }

    SetUIProperties() {
       
        var isFieldActivated: boolean = false;
        if (this.HasCreditLimitFeature) {
            if (this.IsCreditLimitActivated) {
                if (this.IsCreditLimitEnabled) {
                    isFieldActivated = true;
                }
            }
        }

        

        this.UIProperties.SetEnabled("IsCreditLimitEnabled", this.ObjectTableName, this.IsCreditLimitActivated);
        this.UIProperties.SetEnabled("CreditLimitAmount", this.ObjectTableName, isFieldActivated);
        this.UIProperties.SetEnabled("CreditLimitOpenBalance", this.ObjectTableName, isFieldActivated);
        this.UIProperties.SetEnabled("CreditLimitWarningPercentage", this.ObjectTableName, isFieldActivated);
        this.UIProperties.SetEnabled("CreditLimitActualBalance", this.ObjectTableName, isFieldActivated);
        this.UIProperties.SetEnabled("BlockNewInvoiceCreation", this.ObjectTableName, isFieldActivated);
        this.UIProperties.SetEnabled("BlockNewShipmentCreation", this.ObjectTableName, isFieldActivated);

        var isLimitAmountRequired = false;
        var isWarningPercentageRequired = false;
        var isWarningPercentageInvalid = false;
        if (isFieldActivated) {
            if (AppTool.IsNullOrEmpty(this.CreditLimitAmount)) {
                isLimitAmountRequired = true;
            }

            if (AppTool.IsNullOrEmpty(this.CreditLimitWarningPercentage)) {
                isWarningPercentageRequired = true;
            }

            else if (this.EntityPM.CreditLimitWarningPercentage < 0 || this.EntityPM.CreditLimitWarningPercentage > 100) {
                isWarningPercentageInvalid = true;
            }
        }

        this.UIProperties.SetRequired("CreditLimitAmount", this.ObjectTableName, isLimitAmountRequired);

        this.UIProperties.SetRequired("CreditLimitWarningPercentage", this.ObjectTableName, false);
        this.UIProperties.SetValidity("CreditLimitWarningPercentage", this.ObjectTableName, true, null);

        if (isWarningPercentageRequired) {
            this.UIProperties.SetRequired("CreditLimitWarningPercentage", this.ObjectTableName, isWarningPercentageRequired);
        }

        else if (isWarningPercentageInvalid) {
            this.UIProperties.SetValidity("CreditLimitWarningPercentage", this.ObjectTableName, false, "Credit limit warning percentage field must be more than 0 and less than 100");
        }

        this.SetUIProperties_GeneratedComponent();

        
    }
    
    private SessionEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "EntityActivated") {
                    this.SetUIProperties_GeneratedComponent();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
    }

    private isLimitAmountLoaded: boolean = false;
    private CreditLimitLoadedAmount: number = null;
    LoadCreditLimitData() {
        if (this.HasCreditLimitFeature) {
            if (this.IsCreditLimitActivated) {
                if (this.IsCreditLimitEnabled) {
                    if (!this.isLimitAmountLoaded) {
                        this.isLimitAmountLoaded = true;

                        var myService = new PartnersDomainService();
                        myService.GetCustomerCreditLimitActualAmount(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                this.CreditLimitLoadedAmount = myResponse.Result;
                                this.ComputeActualBalance();
                            }
                        });
                    }
                }
            }
        }
    }

    CreditLimitManagementClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Credit Limit Settings";
        logitudeWindow.Show('./Common/Components/Maintenance/CreditLimit/CreditLimitSettingsComponent');
        logitudeWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.IsCreditLimitActivated = ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
                this.SetUIProperties();
                this.LoadCreditLimitData();
            }
        });
    }

    get IsCreditLimitEnabled() { return this.EntityPM.IsCreditLimitEnabled; }
    set IsCreditLimitEnabled(value: boolean) {
        if (this.EntityPM.IsCreditLimitEnabled != value) {
            this.EntityPM.IsCreditLimitEnabled = value;
            this.SetUIProperties();
            this.LoadCreditLimitData();
        }
    }

    get CreditLimitAmount() { return this.EntityPM.CreditLimitAmount; }
    set CreditLimitAmount(value: number) {
        if (this.EntityPM.CreditLimitAmount != value) {
            this.EntityPM.CreditLimitAmount = AppTool.Round(value, 2);
            this.SetUIProperties();
        }
    }

    get InsuredcreditLimit() { return this.EntityPM.InsuredcreditLimit; }
    set InsuredcreditLimit(value: number) {
        if (this.EntityPM.InsuredcreditLimit != value) {
            this.EntityPM.InsuredcreditLimit = AppTool.Round(value, 2);
            this.SetUIProperties();
        }
    }

    get CreditLimitOpenBalance() { return this.EntityPM.CreditLimitOpenBalance; }
    set CreditLimitOpenBalance(value: number) {
        if (this.EntityPM.CreditLimitOpenBalance != value) {
            this.EntityPM.CreditLimitOpenBalance = AppTool.Round(value, 2);
            this.SetUIProperties();
            this.ComputeActualBalance();
        }
    }

    get CreditLimitWarningPercentage() { return this.EntityPM.CreditLimitWarningPercentage; }
    set CreditLimitWarningPercentage(value: number) {
        if (this.EntityPM.CreditLimitWarningPercentage != value) {
            this.EntityPM.CreditLimitWarningPercentage = value;
            this.SetUIProperties();
        }
    }

    ComputeActualBalance() {        
        this.CreditLimitActualBalance = AppTool.AddAmounts(this.CreditLimitLoadedAmount, this.CreditLimitOpenBalance);
    }

    private creditLimitActualBalance: number;
    get CreditLimitActualBalance() { return this.creditLimitActualBalance; }
    set CreditLimitActualBalance(value: number) {
        if (this.creditLimitActualBalance != value) {
            this.creditLimitActualBalance = AppTool.Round(value, 2);
        }
    }

    get PaymentMethodCode() { return this.EntityPM.PaymentMethodCode; }
    set PaymentMethodCode(newValue: string) {
        if (this.EntityPM.PaymentMethodCode != newValue) {
            this.EntityPM.PaymentMethodCode = newValue;
        }
    }

    get MetodoPagoCode() { return this.EntityPM.MetodoPagoCode; }
    set MetodoPagoCode(newValue: string) {
        if (this.EntityPM.MetodoPagoCode != newValue) {
            this.EntityPM.MetodoPagoCode = newValue;
        }
    }

    get UsoCFDICode() { return this.EntityPM.UsoCFDICode; }
    set UsoCFDICode(newValue: string) {
        if (this.EntityPM.UsoCFDICode != newValue) {
            this.EntityPM.UsoCFDICode = newValue;
        }
    }

    get RegimenFiscalCode() { return this.EntityPM.RegimenFiscalCode; }
    set RegimenFiscalCode(newValue: string) {
        if (this.EntityPM.RegimenFiscalCode != newValue) {
            this.EntityPM.RegimenFiscalCode = newValue;
        }
    }

    get SATForeignRFC() { return this.EntityPM.SATForeignRFC; }
    set SATForeignRFC(newValue: string) {
        if (this.EntityPM.SATForeignRFC != newValue) {
            this.EntityPM.SATForeignRFC = newValue;
        }
    }

    get BlockNewInvoiceCreation() { return this.EntityPM.BlockNewInvoiceCreation; }
    set BlockNewInvoiceCreation(value: boolean) {
        if (this.EntityPM.BlockNewInvoiceCreation != value) {
            this.EntityPM.BlockNewInvoiceCreation = value;
        }
    }

    get BlockNewShipmentCreation() { return this.EntityPM.BlockNewShipmentCreation; }
    set BlockNewShipmentCreation(value: boolean) {
        if (this.EntityPM.BlockNewShipmentCreation != value) {
            this.EntityPM.BlockNewShipmentCreation = value;
        }
    }

}
