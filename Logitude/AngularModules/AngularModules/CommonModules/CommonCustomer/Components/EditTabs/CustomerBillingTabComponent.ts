import {Component, OnInit, ViewChild, ViewContainerRef, AfterViewInit, ChangeDetectorRef} from '@angular/core';
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
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';

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
    public Profact4Enabled: boolean = false;
    public IsBlockMessageVisible: boolean = false;
    @ViewChild('BillingChild', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    @ViewChild('ARInvoiceDocumentTypeTemplateArea', { read: ViewContainerRef, static: false }) documentTemplateViewContainerRef: ViewContainerRef;
    public DisplaySATSettings: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    private entityResourceService: EntityResourceService = new EntityResourceService();
    public isDataLoaded: boolean = false;
    constructor(public entityArgs: EntityArgs, private CD: ChangeDetectorRef) {
        super();
        this.entityResourceService.getEntityResourceByTableName("Card").subscribe((response: any) => {


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
                this.Profact4Enabled = SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF40";
            }
            this.isDataLoaded = true;     
            this.Listen();
        })
      
    }

    ngAfterViewInit(): void {
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this.SetUIProperties();
        this.LoadGeneratedComponents();
        this.LoadCreditLimitData();
        this.CD.detectChanges();
        this.SetUIProperties_GeneratedComponent();
    }

    ngOnInit() {
      
    }

    LoadGeneratedComponents() {
        if (!this.viewContainerRef) {
            this.RunComponentTimer("Child");
            return;
        }
        this.LoadChildComponent(this.viewContainerRef);
    }

    private Retries: number = 0;
    private timerToken: any;
    private GeneratedComponent: any;
    private readonly MAX_RETRIES: number = 100;
    private readonly DELAY_MS: number = 500;
    private RunComponentTimer(componentName: String) {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < this.MAX_RETRIES) {
            this.timerToken = componentName == "ARInvoiceDocumentTypeTemplateComponent" ? setTimeout(() => this.LoadARInvoiceDocumentTypeTemplateComponent(), this.DELAY_MS) : setTimeout(() => this.LoadGeneratedComponents(), this.DELAY_MS);
        }
    }
    private LoadChildComponent(viewContainerRef) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                this.GeneratedComponent = cmpRef.instance;

                cmpRef.instance.LoadCompleted.subscribe(s => {
                    this.SetUIProperties_GeneratedComponent();
                });

                var screenCode = "Customer.BillingTabScreen";
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, screenCode);
                if (viewContainerRef == this.viewContainerRef) this.LoadARInvoiceDocumentTypeTemplateComponent();
            });
    }

    IsARInvoiceDocumentTypeTemplateAreaLoaded: boolean = false;
    public LoadARInvoiceDocumentTypeTemplateComponent() {
        if (this.IsARInvoiceDocumentTypeTemplateAreaLoaded) return;
        this.Retries = 0;
        if (!this.documentTemplateViewContainerRef) {
            this.RunComponentTimer("ARInvoiceDocumentTypeTemplateComponent");
            return;
        }
        SessionLocator.DynamicLoader.Load('./CommonModules/CommonPartners/Components/Templates/PartnerARInvoiceDocumentTypeTemplateComponent', this.documentTemplateViewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.Run(this.EntityPM);
            });
        this.IsARInvoiceDocumentTypeTemplateAreaLoaded = true;
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
            this.EntityPM?.UIProperties.SetEnabled("CreditLimitAmount", this.ObjectTableName, this.HasEditCreditAmountFeature);
            this.SetInsuredCreditLimitEnablitity();

        }
    }

    private SetInsuredCreditLimitEnablitity() {
        this.UIProperties.SetEnabled("InsuredcreditLimit", this.ObjectTableName, true);
        this.EntityPM?.UIProperties.SetEnabled("InsuredcreditLimit", this.ObjectTableName, true);
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

        if (this.EntityPM?.Card?.ExternalSystem == "UNIFREIGHT") {
            this.IsBlockMessageVisible = true;
            this.UIProperties.SetEnabled("IsAutonomy", "Card", false);

        }
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
    get IsAutonomy() { return this.EntityPM.Card?.IsAutonomy; }
    set IsAutonomy(newValue: boolean) {
        if (this.EntityPM?.Card != null && this.EntityPM.Card?.IsAutonomy != newValue) {
            this.EntityPM.Card.IsAutonomy = newValue;
            this.EntityPM.IsDirty = true;
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

    get SATCustomerName() { return this.EntityPM.SATCustomerName; }
    set SATCustomerName(newValue: string) {
        if (this.EntityPM.SATCustomerName != newValue) {
            this.EntityPM.SATCustomerName = newValue;
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
