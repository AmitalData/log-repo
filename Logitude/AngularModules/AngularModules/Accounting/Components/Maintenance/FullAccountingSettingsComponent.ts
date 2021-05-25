import {Component, OnInit, AfterViewInit, ChangeDetectorRef} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogLabelComponent} from '../../../Infrastructure/Components/LogitudeComponents/LogLabelComponent';
import {LogTextBoxComponent} from '../../../Infrastructure/Components/LogitudeComponents/LogTextBoxComponent';
import {LogLovComponent} from '../../../Infrastructure/Components/LogitudeComponents/LogLovComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslationPipe} from '../../../Controls/Pipes/TextCodeTranslationPipe';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {FullAccountingSettingPM} from '../../EntityPMs/FullAccountingSettingPM';
import {FullAccountingSettingList} from '../../EntityLists/FullAccountingSettingList';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPMService} from '../../../Common/Services/StandardPMs/TenantPMService';
import {PaymentTermList} from '../../../Common/EntityLists/PaymentTermList';
import {FullAccountingSettingPMService} from '../../Services/StandardPMs/FullAccountingSettingPMService';
import {FullAccountingSettingListService} from '../../Services/StandardLists/FullAccountingSettingListService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { GLAccountPM } from '../../EntityPMs/GLAccountPM';
//import {AutomaticExternalRconcilMthodsPM}  '../../Services/StandardPMs/AutomaticExternalRconcilMthodsPM';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import {ObjectsUpdater} from '../../../Infrastructure/Locators/ObjectsUpdater';
import { AppTool } from '../../../Infrastructure/Tools';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { BookingWizardPackageItem } from 'Booking/Components/BookingWizard/Packages/PackagesTabComponent';

@Component({

    selector: 'FullAccountingSettingsComponent',
    templateUrl: './FullAccountingSettingsComponent.html',
    providers: [ServiceArgs]
})

export class FullAccountingSettingsComponent extends BaseComponent implements OnInit, AfterViewInit {


    public DataContext: FullAccountingSettingsComponent = this;
    //public myForm: ControlGroup;
    public ObjectTableName: string = "FullAccountingSetting";
    public TenantPM: TenantPM;
    public EntityPM: FullAccountingSettingPM;
    public isRTL: boolean = false;
    ImageId: string;
    EntityId: string;
    fullAccountingSettingPMService: FullAccountingSettingPMService = new FullAccountingSettingPMService();;
    fullAccountingSettingListService: FullAccountingSettingListService;
    tenantPMService: TenantPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public serviceArgs: ServiceArgs, private _entityResourceService: EntityResourceService, private cd: ChangeDetectorRef) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.CurrentSession.StartBusyIndicatorLoading();

        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((responseGLAccount: any) =>
        {
            this._entityResourceService.getEntityResourceByTableName("ChartOfAccount").subscribe((response1: any) =>
            {
                this._entityResourceService.getEntityResourceByTableName("FullAccountingSetting").subscribe((response1: any) =>
                {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(response =>
                    {
                        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => { });
                    });

                });
            });
        });

        this.fullAccountingSettingPMService.get(SessionLocator.Tenant.toString()).subscribe((myResult: any) => {
            this.CurrentSession.StopBusyIndicator();

            this.EntityPM = myResult.Result;
            //this.ImageId = this.EntityPM.PaymentChequesLogoId;
            //this.EntityId = this.EntityPM.Id;

            if (this.EntityPM == null || this.EntityPM == undefined) {
                this.InsertIfNotExist();
            } else {


                this.ImageId = this.EntityPM.PaymentChequesLogoId;
                this.EntityId = this.EntityPM.Id;


                this.AccountingActivationDate = this.EntityPM.AccountingActivationDate;
                this.SetUIProperties();
                this.SetSelectedAgingPeriods();
            }

        });

        this.UIProperties.SetEnabled("AccountingActivationDate", "Tenant", false);

    }
    InsertIfNotExist() {
        console.log("There is no F. Accounting setting found for tenant: " + SessionLocator.Tenant);
        this.EntityPM = new FullAccountingSettingPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.Id = SessionLocator.Tenant.toString();
        this.fullAccountingSettingPMService.insert(this.EntityPM).subscribe(myResult => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) { // Success
                this.EntityPM = mm.Result;
                this.ImageId = this.EntityPM.PaymentChequesLogoId;
                this.EntityId = this.EntityPM.Id;


                this.AccountingActivationDate = this.EntityPM.AccountingActivationDate;
                this.SetUIProperties();

            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        }, error => {
            this.CurrentSession.StopBusyIndicator();
            var dd: Response = error;
            console.log(dd.text);
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push('Server Error!');
        });

    }
    ngOnInit() {
        this.BuildTabs();
    }

    ngAfterViewInit() {
        //this.SetUIProperties();
    }
    ReloadTenantPM(): any {

        SessionLocator.TenantPM.AccountingActivated = this.AccountingActivated;

    }
    SetUIProperties() {

        var enableAllFields = false;
        if (this.AccountingActivated && this.AccountingActivationDate != null) {
            enableAllFields = true;
        }
        this.UIProperties.SetEnabled("DeductionFileNumber", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("ConsolidationVAT", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("DefaultVATTypeId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("PaymentTermId", "Tenant", enableAllFields);
        this.UIProperties.SetEnabled("VATInputsGLAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("VATOutputGLAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("AutomaticReconcileMethodId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("ExchangeRateDiffGLAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("RevenueExpenseGLAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("CustomerControlAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("VendorControlAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("FileControlAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("OceanExportJobControlAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("OceanImportJobControlAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("AirExportJobControlAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("AirImportJobControlAccountId", this.ObjectTableName, enableAllFields);
    }

    //#region Full Accounting Setting Properties
    get AccountingActivationDate() { return this.EntityPM.AccountingActivationDate; }
    set AccountingActivationDate(value: Date) {
        if (this.EntityPM.AccountingActivationDate != value) {
            this.EntityPM.AccountingActivationDate = value;
            this.SetUIProperties();
        }
    }

 get AllowMultiRatesInInvoiceLines() { return this.EntityPM.AllowMultiRatesInInvoiceLines; }
    set AllowMultiRatesInInvoiceLines(value: boolean) {
        if (this.EntityPM.AllowMultiRatesInInvoiceLines != value) {
            this.EntityPM.AllowMultiRatesInInvoiceLines = value;
            this.SetUIProperties();
        }
    }
    get AccountingActivated() { return this.EntityPM.AccountingActivated; }
    set AccountingActivated(value: boolean) {
        if (this.EntityPM.AccountingActivated != value) {
            this.EntityPM.AccountingActivated = value;
            if (value == true) {
                this.AccountingActivationDate = new Date();
            } else if (value == false) {
                this.AccountingActivationDate = null;
            }
            this.ReloadTenantPM();
            this.SetUIProperties();
        }
    }

    get IsPaymentChequesActivated() { return this.EntityPM.IsPaymentChequesActivated; }
    set IsPaymentChequesActivated(value: boolean) {
        if (this.EntityPM.IsPaymentChequesActivated != value) {
            this.EntityPM.IsPaymentChequesActivated = value;

            this.SetUIProperties();
        }
    }

    get DeductionFileNumber() { return this.EntityPM.DeductionFileNumber; }
    set DeductionFileNumber(value: string) {
        if (this.EntityPM.DeductionFileNumber != value) {
            this.EntityPM.DeductionFileNumber = value;
        }
    }


    get GLAccounterCounterLength() { return this.EntityPM.GLAccounterCounterLength; }
    set GLAccounterCounterLength(value: number) {
        if (this.EntityPM.GLAccounterCounterLength != value) {
            this.EntityPM.GLAccounterCounterLength = value;
        }
    }

    get ExternalReconciliationDefault() { return this.EntityPM.ExternalReconciliationDefault; }
    set ExternalReconciliationDefault(value: string) {
        if (this.EntityPM.ExternalReconciliationDefault != value) {
            this.EntityPM.ExternalReconciliationDefault = value;
        }
    }
    get TaxWithholdingGLAccountId() { return this.EntityPM.TaxWithholdingGLAccountId; }
    set TaxWithholdingGLAccountId(value: string) {
        if (this.EntityPM.TaxWithholdingGLAccountId != value) {
            this.EntityPM.TaxWithholdingGLAccountId = value;
        }
    }

    get ConsolidationVAT() { return this.EntityPM.ConsolidationVAT; }
    set ConsolidationVAT(value: string) {
        if (this.EntityPM.ConsolidationVAT != value) {
            this.EntityPM.ConsolidationVAT = value;
        }
    }

    get DefaultVATTypeId() { return this.EntityPM.DefaultVATTypeId; }
    set DefaultVATTypeId(value: string) {
        if (this.EntityPM.DefaultVATTypeId != value) {
            this.EntityPM.DefaultVATTypeId = value;
        }
    }

    get PaymentTermId() { return this.EntityPM.TenantPaymentTermId; }
    set PaymentTermId(value: string) {
        if (this.EntityPM.TenantPaymentTermId != value) {
            this.EntityPM.TenantPaymentTermId = value;
        }
    }

    get DefaultTaxWithholdPercentage() { return this.EntityPM.DefaultTaxWithholdPercentage; }
    set DefaultTaxWithholdPercentage(value: number) {
        if (this.EntityPM.DefaultTaxWithholdPercentage != value) {
            this.EntityPM.DefaultTaxWithholdPercentage = value;
        }
    }

    get VATInputsGLAccountId() { return this.EntityPM.VATInputsGLAccountId; }
    set VATInputsGLAccountId(value: string) {
        if (this.EntityPM.VATInputsGLAccountId != value) {
            this.EntityPM.VATInputsGLAccountId = value;
        }
    }

    get VATOutputGLAccountId() { return this.EntityPM.VATOutputGLAccountId; }
    set VATOutputGLAccountId(value: string) {
        if (this.EntityPM.VATOutputGLAccountId != value) {
            this.EntityPM.VATOutputGLAccountId = value;
        }
    }

    get AutomaticReconcileMethodId() { return this.EntityPM.AutomaticReconcileMethodId; }
    set AutomaticReconcileMethodId(value: string) {
        if (this.EntityPM.AutomaticReconcileMethodId != value) {
            this.EntityPM.AutomaticReconcileMethodId = value;
        }
    }

    get ExchangeRateDiffGLAccountId() { return this.EntityPM.ExchangeRateDiffGLAccountId; }
    set ExchangeRateDiffGLAccountId(value: string) {
        if (this.EntityPM.ExchangeRateDiffGLAccountId != value) {
            this.EntityPM.ExchangeRateDiffGLAccountId = value;
        }
    }

    get RevenueExpenseGLAccountId() { return this.EntityPM.RevenueExpenseGLAccountId; }
    set RevenueExpenseGLAccountId(value: string) {
        if (this.EntityPM.RevenueExpenseGLAccountId != value) {
            this.EntityPM.RevenueExpenseGLAccountId = value;
        }
    }

    get CustomerControlAccountId() { return this.EntityPM.CustomerControlAccountId; }
    set CustomerControlAccountId(value: string) {
        if (this.EntityPM.CustomerControlAccountId != value) {
            this.EntityPM.CustomerControlAccountId = value;
        }
    }

    get VendorControlAccountId() { return this.EntityPM.VendorControlAccountId; }
    set VendorControlAccountId(value: string) {
        if (this.EntityPM.VendorControlAccountId != value) {
            this.EntityPM.VendorControlAccountId = value;
        }
    }

    get FileControlAccountId() { return this.EntityPM.FileControlAccountId; }
    set FileControlAccountId(value: string) {
        if (this.EntityPM.FileControlAccountId != value) {
            this.EntityPM.FileControlAccountId = value;
        }
    }

    get OceanExportJobControlAccountId() { return this.EntityPM.OceanExportJobControlAccountId; }
    set OceanExportJobControlAccountId(value: string) {
        if (this.EntityPM.OceanExportJobControlAccountId != value) {
            this.EntityPM.OceanExportJobControlAccountId = value;
        }
    }

    get OceanImportJobControlAccountId() { return this.EntityPM.OceanImportJobControlAccountId; }
    set OceanImportJobControlAccountId(value: string) {
        if (this.EntityPM.OceanImportJobControlAccountId != value) {
            this.EntityPM.OceanImportJobControlAccountId = value;
        }
    }

    get AirExportJobControlAccountId() { return this.EntityPM.AirExportJobControlAccountId; }
    set AirExportJobControlAccountId(value: string) {
        if (this.EntityPM.AirExportJobControlAccountId != value) {
            this.EntityPM.AirExportJobControlAccountId = value;
        }
    }

  get CustomsGLAccountId() { return this.EntityPM.CustomsGLAccountId; }
  set CustomsGLAccountId(value: string) {
    if (this.EntityPM.CustomsGLAccountId != value) {
      this.EntityPM.CustomsGLAccountId = value;
    }
    }

    get PaymentChequesLogoId() { return this.EntityPM.PaymentChequesLogoId; }
    set PaymentChequesLogoId(value: string) {
        if (this.EntityPM.PaymentChequesLogoId != value) {
            this.EntityPM.PaymentChequesLogoId = value;


        }
    }

    get AirImportJobControlAccountId() { return this.EntityPM.AirImportJobControlAccountId; }
    set AirImportJobControlAccountId(value: string) {
        if (this.EntityPM.AirImportJobControlAccountId != value) {
            this.EntityPM.AirImportJobControlAccountId = value;
        }
    }

    get DefaultDifferencesGLAccountId() { return this.EntityPM.DefaultDifferencesGLAccountId; }
    set DefaultDifferencesGLAccountId(value: string) {
        if (this.EntityPM.DefaultDifferencesGLAccountId != value) {
            this.EntityPM.DefaultDifferencesGLAccountId = value;
        }
    }

    get DefaultExternalDiffGLAccountId() { return this.EntityPM.DefaultExternalDiffGLAccountId; }
    set DefaultExternalDiffGLAccountId(value: string) {
        if (this.EntityPM.DefaultExternalDiffGLAccountId != value) {
            this.EntityPM.DefaultExternalDiffGLAccountId = value;
        }
    }

    // Properties
    revenueExpenseGLAccount: GLAccountPM;
    get RevenueExpenseGLAccount() { return this.revenueExpenseGLAccount; }
    set RevenueExpenseGLAccount(value: GLAccountPM) {
        if (this.revenueExpenseGLAccount != value) {
            this.revenueExpenseGLAccount = value;
            this.ValidateMulticurrencyAccounts();
        }
    }

    customerControlAccount: GLAccountPM;
    get CustomerControlAccount() { return this.customerControlAccount; }
    set CustomerControlAccount(value: GLAccountPM) {
        if (this.customerControlAccount != value) {
            this.customerControlAccount = value;
            this.ValidateMulticurrencyAccounts();
        }
    }

    taxWithholdingGLAccount: GLAccountPM;
    get TaxWithholdingGLAccount() { return this.taxWithholdingGLAccount; }
    set TaxWithholdingGLAccount(value: GLAccountPM) {
        if (this.taxWithholdingGLAccount != value) {
            this.taxWithholdingGLAccount = value;
            this.ValidateMulticurrencyAccounts();
        }
    }

    get SoftwareVersion() { return this.EntityPM.SoftwareVersion; }
    set SoftwareVersion(value: string) {
        if (this.EntityPM.SoftwareVersion != value) {
            this.EntityPM.SoftwareVersion = value;
        }
    }

    //automaticExternalRconcilMthods: AutomaticExternalRconcilMthodsPM;
    //get AutomaticExternalRconcilMthods() { return this.taxWithholdingGLAccount; }
    //set AutomaticExternalRconcilMthods(value: GLAccountPM) {
    //    if (this.automaticExternalRconcilMthods != value) {
    //        this.automaticExternalRconcilMthods = value;

    //    }
    //}



    vendorControlAccount: GLAccountPM;
    get VendorControlAccount() { return this.vendorControlAccount; }
    set VendorControlAccount(value: GLAccountPM) {
        if (this.vendorControlAccount != value) {
            this.vendorControlAccount = value;
            this.ValidateMulticurrencyAccounts();
        }
    }

    fileControlAccount: GLAccountPM;
    get FileControlAccount() { return this.fileControlAccount; }
    set FileControlAccount(value: GLAccountPM) {
        if (this.fileControlAccount != value) {
            this.fileControlAccount = value;
            this.ValidateMulticurrencyAccounts();
        }
    }

    jobControlAccount: GLAccountPM;
    get JobControlAccount() { return this.jobControlAccount; }
    set JobControlAccount(value: GLAccountPM) {
        if (this.jobControlAccount != value) {
            this.jobControlAccount = value;
            this.ValidateMulticurrencyAccounts();
        }
    }


    get NumberOfAgingMonths () { return this.EntityPM.NumberOfAgingMonths ; }
    set NumberOfAgingMonths (value: number) {
        if (this.EntityPM.NumberOfAgingMonths  != value) {
            this.EntityPM.NumberOfAgingMonths  = value;

            if (value < 1 || value > 9) {
                this.UIProperties.SetValidity("NumberOfAgingMonths", this.ObjectTableName, false, TextCodeTranslator.Translate("FullAccountingSetting.O.NoOfAgingMonthsBW1n9"));
            }else{
                this.UIProperties.SetValidity("NumberOfAgingMonths", this.ObjectTableName, true, "");
            }
        }
    }

    //#endregion

    //Commands

    ImageUploadedCompleted(code) {
        this.ImageId = code;
        this.EntityPM.PaymentChequesLogoId = code;
    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];

    OkButtonClicked() {
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (errors.length == 0)
            this.ValidateMulticurrencyAccounts();


        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
            this.SubmitChanges("");
        }


    }

SubmitChanges(ControlAccountId:string) {
    //console.log("EntityPM: ", this.EntityPM);

    this.SetAgingPeriodsFields();

    this.fullAccountingSettingPMService.update(this.EntityPM).subscribe(myResult => {

        var mm: ServiceResponse = myResult;
        if (!mm.HasError) { // Success
            this.CurrentSession.CloseCurrentWindow();
            this.CurrentSession.StopBusyIndicator();
            if (!AppTool.IsNullOrEmpty(ControlAccountId)) {
                this.FullAccountingAddControl(ControlAccountId);
                }
            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        }, error => {
            this.CurrentSession.StopBusyIndicator();
            var dd: Response = error;
            console.log(dd.text);
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push('Server Error!');
        });
    }

    SetAgingPeriodsFields(){
        this.EntityPM.FirstPeriodsMonths = this.JoinCodesOfPeriods(this.SelectedPeriods1);
        this.EntityPM.SecondPeriodsMonths = this.JoinCodesOfPeriods(this.SelectedPeriods2);
        this.EntityPM.ThirdsPeriodsMonths = this.JoinCodesOfPeriods(this.SelectedPeriods3);
    }

    ResetAgingPeriodsFields(){
        if(this.NumberOfPeriods <= 2){
            this.EntityPM.ThirdsPeriodsMonths = null;
            this.SelectedPeriods3 = [];
        }

        if(this.NumberOfPeriods == 1){
            this.EntityPM.SecondPeriodsMonths = null;
            this.SelectedPeriods2 = [];
        }
    }

    SetSelectedAgingPeriods(){

        this.SelectedPeriods1 = this.SetSelectedPeriods(this.EntityPM.FirstPeriodsMonths);
        this.SelectedPeriods2 = this.SetSelectedPeriods(this.EntityPM.SecondPeriodsMonths);
        this.SelectedPeriods3 = this.SetSelectedPeriods(this.EntityPM.ThirdsPeriodsMonths);
    }

    JoinCodesOfPeriods(periods: any[]){
        if(periods)
            return periods.map(d=>d.Code)?.join(',');
    }

    SetSelectedPeriods(joinedPeriodsCodes: string){
        if(joinedPeriodsCodes){
            var codes = joinedPeriodsCodes.split(',');
            return this.Periods.filter(d=>codes.includes(d.Code));
        }
        return [];
    }


    ValidateMulticurrencyAccounts() {
        console.log("ValidateMulticurrencyAccounts");
        this.ValidationErrorsList = [];

        this.UIProperties.SetValidity("FileControlAccountId", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("RevenueExpenseGLAccountId", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("VendorControlAccountId", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("CustomerControlAccountId", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("JobControlAccountId", this.ObjectTableName, true, "");

        if (this.revenueExpenseGLAccount && !this.revenueExpenseGLAccount.IsMultiCurrency) {
            this.ValidationErrorsList.push("Revenue Expense Account must be multi currency");
            this.UIProperties.SetValidity("RevenueExpenseGLAccountId", this.ObjectTableName, false, "Revenue Expense Account must be multi currency");
        }
        if (this.customerControlAccount && !this.customerControlAccount.IsMultiCurrency) {
            this.ValidationErrorsList.push("Customer Control Account must be multi currency");
            this.UIProperties.SetValidity("CustomerControlAccountId", this.ObjectTableName, false, "Customer Control Account must be multi currency");

        }
        if (this.vendorControlAccount && !this.vendorControlAccount.IsMultiCurrency) {
            this.ValidationErrorsList.push("Vendor Control Account must be multi currency");
            this.UIProperties.SetValidity("VendorControlAccountId", this.ObjectTableName, false, "Vendor Control Account must be multi currency");

        }
        if (this.fileControlAccount && !this.fileControlAccount.IsMultiCurrency) {
            this.ValidationErrorsList.push("File Control Account must be multi currency");
            this.UIProperties.SetValidity("FileControlAccountId", this.ObjectTableName, false, "File Control Account must be multi currency");

        }
        if (this.jobControlAccount && !this.jobControlAccount.IsMultiCurrency) {
            this.ValidationErrorsList.push("Job Control Account must be multi currency");
            this.UIProperties.SetValidity("JobControlAccountId", this.ObjectTableName, false, "Job Control Account must be multi currency");

        }
    }


    //#region Tabs Code
    TabsSource: any[] = [];
    SelectedTab: string = "";

    BuildTabs() {
        this.SelectedTab = "FullAccoutingSetting";
        this.TabsSource.push({ Name: "FullAccoutingSetting", isSelected: true, Header: TextCodeTranslator.Translate("General.O.General") }); //Accounting.O.FullAccountingSettings
        this.TabsSource.push({ Name: "ControlAccounts", isSelected: false, Header: TextCodeTranslator.Translate("Accounting.O.ControlGLAccounts") });
        this.TabsSource.push({ Name: "Logo", isSelected: false, Header: TextCodeTranslator.Translate("Accounting.General.O.Cheques") });


        const isAgingDefinitionEnabled = FeatureLocator.HasFeaturePermession("FullAccountingSetting", "AgingDefenetionSettings");
        if(isAgingDefinitionEnabled)
            this.TabsSource.push({ Name: "AgingDefinition", isSelected: false, Header: TextCodeTranslator.Translate("FullAccountingSetting.O.AgingDefinition") });

    }
    SelectionChanged(tab: any) {

        this.TabsSource.forEach(item => { // reset selection
            item.isSelected = false;
        });

        var index = this.TabsSource.indexOf(tab);
        if (index < 0) {
            console.log("The tab was not found, cant not delete it :( ", tab); return;
        }
        var item = this.TabsSource[index];
        item.isSelected = true;
        this.SelectedTab = item.Name;
    }
    //#endregion
    _ControlAccountId: string;
    CreateControlAccount(ControlAccountId: string) {
        this._ControlAccountId = ControlAccountId
        if (!AppTool.IsNullOrEmpty(this[ControlAccountId])) {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Show("כרטיס מרכז מוגדר");
            return;
        }

        let confirmWindow = new ConfirmWindow();
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.StartBusyIndicatorLoading();
                this.SubmitChanges(ControlAccountId);
            }
        });
        confirmWindow.Show("אנא אשר שמירה והוספה של חשבון מרכז");
    }
    FullAccountingAddControl(ControlAccountId:string) {
        var windowTitle = TextCodeTranslator.Translate("TaxReport.B.Download");

        var windowArgs: any = {};
        windowArgs.ControlAccountId = this._ControlAccountId;
        this.CurrentSession.StartBusyIndicatorLoading();

        var logWindow = new LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = 250;
        logWindow.Title = "בניית כרטיס מרכז";
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        //logWindow.WindowClosed.subscribe(($event: any) => {
        //    if ($event != null) {
        //        this[ControlAccountId] = $event;
        //    }
        //});
        logWindow.Show('./Accounting/Components/Maintenance/FullAccountingAddControlComponent');

    }


    public ShowLocals: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    isRTL = ObjectsLocator.GlobalSetting ? (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl") : false;


    public get NumberOfPeriods() : number {
        return this.EntityPM.NumberofPeriods;
    }
    public set NumberOfPeriods(value : number) {
        this.EntityPM.NumberofPeriods = value;

        this.ResetAgingPeriodsFields();
    }

    Periods: any[] = [
        {EnglishName: 'Period 0', LocalName: 'תקופה גיול 0', Code: 'period0'},
        {EnglishName: 'Period 1', LocalName: 'תקופה גיול 1', Code: 'period1'},
        {EnglishName: 'Period 2', LocalName: 'תקופה גיול 2', Code: 'period2'},
        {EnglishName: 'Period 3', LocalName: 'תקופה גיול 3', Code: 'period3'},
        {EnglishName: 'Period 4', LocalName: 'תקופה גיול 4', Code: 'period4'},
        {EnglishName: 'Period 5', LocalName: 'תקופה גיול 5', Code: 'period5'},
        {EnglishName: 'Period Past', LocalName: 'לפני התקופה', Code: 'period-past'}
    ];

    // fill these arrays from database
    SelectedPeriods1: any[] = [];
    SelectedPeriods2: any[] = [];
    SelectedPeriods3: any[] = [];

}
