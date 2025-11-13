import { Component, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { ServiceArgs } from '../../../Infrastructure/DataContracts/ServiceArgs';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogLabelComponent } from '../../../Infrastructure/Components/LogitudeComponents/LogLabelComponent';
import { LogTextBoxComponent } from '../../../Infrastructure/Components/LogitudeComponents/LogTextBoxComponent';
import { LogLovComponent } from '../../../Infrastructure/Components/LogitudeComponents/LogLovComponent';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslationPipe } from '../../../Controls/Pipes/TextCodeTranslationPipe';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { FullAccountingSettingPM } from '../../EntityPMs/FullAccountingSettingPM';
import { FullAccountingSettingList } from '../../EntityLists/FullAccountingSettingList';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { TenantPMService } from '../../../Common/Services/StandardPMs/TenantPMService';
import { PaymentTermList } from '../../../Common/EntityLists/PaymentTermList';
import { FullAccountingSettingPMService } from '../../Services/StandardPMs/FullAccountingSettingPMService';
import { FullAccountingSettingListService } from '../../Services/StandardLists/FullAccountingSettingListService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { GLAccountPM } from '../../EntityPMs/GLAccountPM';
//import {AutomaticExternalRconcilMthodsPM}  '../../Services/StandardPMs/AutomaticExternalRconcilMthodsPM';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ObjectsUpdater } from '../../../Infrastructure/Locators/ObjectsUpdater';
import { AppTool } from '../../../Infrastructure/Tools';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { BookingWizardPackageItem } from 'Booking/Components/BookingWizard/Packages/PackagesTabComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
// import { Data } from '@microsoft/applicationinsights-common';
import { CopyFromTenant0ExtendedListService } from 'Accounting/Services/ExtendedLists/CopyFromTenant0ExtendedListService';
import { CopyFromTenant0PM } from 'Accounting/EntityPMs/CopyFromTenant0PM';
import { List } from 'cypress/types/lodash';
import { CopyFromTenant0PMService } from 'Accounting/Services/StandardPMs/CopyFromTenant0PMService';
import { any } from 'cypress/types/bluebird';
import { ChargesTypeListService } from 'Common/Services/StandardLists/ChargesTypeListService';
import { ChargesTypePMService } from 'Common/Services/StandardPMs/ChargesTypePMService';
import { ChargesTypePM } from 'Common/EntityPMs/ChargesTypePM';
import { ChargesTypePMInitService } from 'Common/EntityPMInitServices/ChargesTypePMInitService';
const DebtorsAndCreditorsChartOfAccountTypeCode = '7';
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
    //public EntityPM: FullAccountingSettingPM;
    public listCopyFromTenant0: CopyFromTenant0PM[];
    public CopyFromTenant0PM: CopyFromTenant0PM[];
    public isRTL: boolean = false;
    ImageId: string;
    EntityId: string;
    fullAccountingSettingPMService: FullAccountingSettingPMService = new FullAccountingSettingPMService();;
    copyFromTenant0ExtendedListService: CopyFromTenant0ExtendedListService = new CopyFromTenant0ExtendedListService();;
    copyFromTenant0PMService: CopyFromTenant0PMService = new CopyFromTenant0PMService()
    fullAccountingSettingListService: FullAccountingSettingListService;
    tenantPMService: TenantPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    public TaxInstitutionGLAccountFilterItems: ApiQueryFilters = new ApiQueryFilters();
    disabledCopyFromTenant0 = true
    date = new Date()
    user = "amital "
    private indexHyphenSholudInHSMTokken = [8,13,18,23];
    _chargesTypeListService = new ChargesTypeListService();
    _chargesTypePMService = new ChargesTypePMService();

    constructor(public serviceArgs: ServiceArgs, private _entityResourceService: EntityResourceService, private cd: ChangeDetectorRef) {
        super();
        this.isRTL = ObjectsLocator.GlobalSetting ? (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl") : false;
        this.CurrentSession.StartBusyIndicatorLoading();

        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((responseGLAccount: any) => {
            this._entityResourceService.getEntityResourceByTableName("ChartOfAccount").subscribe((response1: any) => {
                this._entityResourceService.getEntityResourceByTableName("FullAccountingSetting").subscribe((response1: any) => {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(response => {
                        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {

                        });
                    });

                });
            });
        });

        this.fullAccountingSettingPMService.get(SessionLocator.Tenant.toString()).subscribe((myResult: any) => {
            this.CurrentSession.StopBusyIndicator();

            this.EntityPM = myResult.Result;
            //this.ImageId = this.EntityPM.PaymentChequesLogoId;
            //this.EntityId = this.EntityPM.Id;

            this.BuildTabs();

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

        this.GetValueCopyFromTenant0()


        this.UIProperties.SetEnabled("AccountingActivationDate", "Tenant", false);


        this.BuildTaxInstituationFilterItems();


    }
    GetValueCopyFromTenant0() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.StartBusyIndicatorLoading();
        this.copyFromTenant0ExtendedListService.getAll(SessionLocator.Tenant).subscribe((myResult: any) => {

            this.CopyFromTenant0PM = myResult;
            this.copyFromTenant0ExtendedListService.getAll(0).subscribe((myResult: any) => {

                this.listCopyFromTenant0 = myResult;
                if (this.CopyFromTenant0PM && this.listCopyFromTenant0) {
                    this.listCopyFromTenant0.forEach(element => {
                        var value = this.CopyFromTenant0PM.find(t => t.TableName == element.TableName)
                        if (value) {

                            element.CreateDate = value.CreateDate;
                            element.CreatedByUserId = value.CreatedByUserId;
                            element.CreatedByUserName = value.CreatedByUserName;
                        }
                    });
                }
                this.listCopyFromTenant0.sort(function(a, b){return (a.Id < b.Id ? -1 : 1)});


            });


            this.CurrentSession.StopBusyIndicator();
        });

    }
    private BuildTaxInstituationFilterItems() {
        this.TaxInstitutionGLAccountFilterItems.addAdditionalFilter('ChartOfAccountsTypeCode', DebtorsAndCreditorsChartOfAccountTypeCode, null, null, 'Equals', false, false, false, 'string', false);
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
    }

    ngAfterViewInit() {
        //this.SetUIProperties();

    }
    ReloadTenantPM(): any {

        SessionLocator.TenantPM.AccountingActivated = this.AccountingActivated;

    }

    public activateSecurityLevel: boolean = false;
    public enableAllFields: boolean = false;
    SetUIProperties() {


        if (this.AccountingActivated && this.AccountingActivationDate != null) {
            this.enableAllFields = true;
            this.UIProperties.SetEnabled("AccountingActivationDate", "Tenant", true);
        }
        this.UIProperties.SetEnabled("DeductionFileNumber", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("ConsolidationVAT", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("DefaultVATTypeId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("PaymentTermId", "Tenant", this.enableAllFields);
        this.UIProperties.SetEnabled("VATInputsGLAccountId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("VATOutputGLAccountId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("AutomaticReconcileMethodId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("ExchangeRateDiffGLAccountId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("RevenueExpenseGLAccountId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("CustomerControlAccountId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("VendorControlAccountId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("FileControlAccountId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("OceanExportJobControlAccountId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("OceanImportJobControlAccountId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("AirExportJobControlAccountId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("AirImportJobControlAccountId", this.ObjectTableName, this.enableAllFields);
        this.UIProperties.SetEnabled("AllowEditingExchangeRate", this.ObjectTableName, this.enableAllFields);
        var UsingSecurityLevelFeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "SAL")[0];
        if (UsingSecurityLevelFeatureToggle) this.activateSecurityLevel = true;
        this.UIProperties.SetEnabled("IsSecurityLevelActivated", this.ObjectTableName, (this.enableAllFields && this.activateSecurityLevel));
        this.UIProperties.SetEnabled("OppositeAccountNumber", this.ObjectTableName, this.enableAllFields);

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
                this.UIProperties.SetEnabled("AccountingActivationDate", "Tenant", true);
            } else if (value == false) {
                this.AccountingActivationDate = null;
                this.UIProperties.SetEnabled("AccountingActivationDate", "Tenant", false);
            }

            this.ToggleAgingDefinitionTab(value);
            this.ToggleCopyingDataFromTenant0Tab(value);

            this.ReloadTenantPM();
            this.SetUIProperties();
        }
    }

    get VATreportEveryTwoMonths() { return this.EntityPM.VATreportEveryTwoMonths; }
    set VATreportEveryTwoMonths(value: boolean) {
        if (this.EntityPM.VATreportEveryTwoMonths != value) {
            this.EntityPM.VATreportEveryTwoMonths = value;
            this.SetUIProperties();
        }
    }

    private ToggleAgingDefinitionTab(value: boolean) {
        if (value)
            this.AddAgingDefinitionTab();
        else
            this.RemoveAgingDefinitionTab();
    }



   

    private ToggleCopyingDataFromTenant0Tab(value: boolean) {
        if (value)
            this.disabledCopyFromTenant0 = false
        else
            this.disabledCopyFromTenant0 = true
    }

    
    get IsPaymentChequesActivated() { return this.EntityPM.IsPaymentChequesActivated; }
    set IsPaymentChequesActivated(value: boolean) {
        if (this.EntityPM.IsPaymentChequesActivated != value) {
            this.EntityPM.IsPaymentChequesActivated = value;

            this.SetUIProperties();
        }
    }

    get NumberingByChartOfAccount() { return this.EntityPM.NumberingByChartOfAccount; }
    set NumberingByChartOfAccount(value: boolean) {
        if (this.EntityPM.NumberingByChartOfAccount != value) {
            this.EntityPM.NumberingByChartOfAccount = value;

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

    get TenantForConfirmationNumberApi() { return this.EntityPM.TenantForConfirmationNumberApi; }
    set TenantForConfirmationNumberApi(value: string) {
        if (this.EntityPM.TenantForConfirmationNumberApi != value) {
            this.EntityPM.TenantForConfirmationNumberApi = value;
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

    get IsSecurityLevelActivated() { return this.EntityPM.IsSecurityLevelActivated; }
    set IsSecurityLevelActivated(value: boolean) {
        if (this.EntityPM.IsSecurityLevelActivated != value) {
            this.EntityPM.IsSecurityLevelActivated = value;
            this.SetUIProperties();
        }
    }
    get OppositeAccountNumber() { return this.EntityPM.OppositeAccountNumber; }
    set OppositeAccountNumber(value: boolean) {
        if (this.EntityPM.OppositeAccountNumber != value) {
            this.EntityPM.OppositeAccountNumber = value;
            this.SetUIProperties();
        }
    }
    get AllowEditingExchangeRate() { return this.EntityPM.AllowEditingExchangeRate; }
    set AllowEditingExchangeRate(value: boolean) {
        if (this.EntityPM.AllowEditingExchangeRate != value) {
            this.EntityPM.AllowEditingExchangeRate = value;
            this.SetUIProperties();
        }
    }

    get CreateRevaluationJournal() { return this.EntityPM.CreateRevaluationJournal; }
    set CreateRevaluationJournal(value: boolean) {
        if (this.EntityPM.CreateRevaluationJournal != value) {
            this.EntityPM.CreateRevaluationJournal = value;
            this.SetUIProperties();
        }
    }

    get NumberOfAgingMonths() { return this.EntityPM.NumberOfAgingMonths; }
    set NumberOfAgingMonths(value: number) {
        if (this.EntityPM.NumberOfAgingMonths != value) {
            this.EntityPM.NumberOfAgingMonths = value;

            if (value < 1 || value > 9) {
                this.UIProperties.SetValidity("NumberOfAgingMonths", this.ObjectTableName, false, TextCodeTranslator.Translate("FullAccountingSetting.O.NoOfAgingMonthsBW1n9"));
            } else {
                this.UIProperties.SetValidity("NumberOfAgingMonths", this.ObjectTableName, true, "");
            }
        }
    }


    get TaxInstitutionGLAccountId() { return this.EntityPM.TaxInstitutionGLAccountId; }
    set TaxInstitutionGLAccountId(value: string) {
        if (this.EntityPM.TaxInstitutionGLAccountId != value) {
            this.EntityPM.TaxInstitutionGLAccountId = value;
        }
    }

    get PrepaidExpensesGLAccountId() { return this.EntityPM.PrepaidExpensesGLAccountId; }
    set PrepaidExpensesGLAccountId(value: string) {
        if (this.EntityPM.PrepaidExpensesGLAccountId != value) {
            this.EntityPM.PrepaidExpensesGLAccountId = value;
        }
    }



    // Signed 

    get HSM(){return this.EntityPM.HSM;}
    set HSM(hsm:string){
        var validateHsmResult=this.ValidateHsm(hsm);
        this.UIProperties.SetValidity("HSM", this.ObjectTableName, validateHsmResult.valid, validateHsmResult.errorMsg);

        if(this.EntityPM.HSM != hsm) {
            this.EntityPM.HSM = hsm;
        }
    }


    get HSMtoken(){return this.EntityPM.HSMtoken;}
    set HSMtoken(hsmToken:string){

      
        this.ValidateInputHMSToken(hsmToken)
        if(this.EntityPM.HSMtoken != hsmToken) {
            this.EntityPM.HSMtoken = hsmToken;
        }
    }


    ValidateInputHMSToken(hsmToken){
        if(hsmToken != null) {
        if(hsmToken.toString().length != 36 )
        
        {
            this.UIProperties.SetValidity("HSMtoken", this.ObjectTableName, false, TextCodeTranslator.Translate("FullAccountingSetting.O.HSMTokenLong"));
        } else if(!this.ValidateFormatHSMToken(hsmToken)) {
            this.UIProperties.SetValidity("HSMtoken", this.ObjectTableName, false,TextCodeTranslator.Translate("FullAccountingSetting.O.HSMTokenFormat"));

        } else {
            this.UIProperties.SetValidity("HSMtoken", this.ObjectTableName, true, "");
        }
    }

    }
    
    ValidateFormatHSMToken(hsmToken){

        if(hsmToken != null) {
        if(hsmToken.toString().length == 36 && (hsmToken.toString().indexOf('-') == this.indexHyphenSholudInHSMTokken[0]
        && hsmToken.toString().indexOf('-',this.indexHyphenSholudInHSMTokken[0]+1) == this.indexHyphenSholudInHSMTokken[1]
        && hsmToken.toString().indexOf('-',this.indexHyphenSholudInHSMTokken[1]+1) == this.indexHyphenSholudInHSMTokken[2]
        && (hsmToken.toString().indexOf('-',this.indexHyphenSholudInHSMTokken[2]+1) == this.indexHyphenSholudInHSMTokken[3]) && hsmToken.toString().indexOf('-',this.indexHyphenSholudInHSMTokken[3]+1) == -1)
        ) {
           return true
        } else {
            return false
        }
      }
    }


    get HSMaddress(){return this.EntityPM.HSMaddress;}
    set HSMaddress(hsmAddress:string){
        
        if(hsmAddress != null) {
        if(hsmAddress.toString().length >= 50) {
            this.UIProperties.SetValidity("HSMaddress", this.ObjectTableName, false, TextCodeTranslator.Translate("FullAccountingSetting.O.HSMAddressLong"));
        } else {
            this.UIProperties.SetValidity("HSMaddress", this.ObjectTableName, true, "");
        }

      }

        if(this.EntityPM.HSMaddress != hsmAddress) {
            this.EntityPM.HSMaddress = hsmAddress;
        }
    }
    get InterestInvoiceNotes(){return this.EntityPM.InterestInvoiceNotes;}
    set InterestInvoiceNotes(interestInvoiceNotes:string){
        
       
        if(this.EntityPM.InterestInvoiceNotes != interestInvoiceNotes) {
            this.EntityPM.InterestInvoiceNotes = interestInvoiceNotes;
        }
    }
    get InvoiceNotes(){return this.EntityPM.InvoiceNotes;}
    set InvoiceNotes(invoiceNotes:string){
        
       
        if(this.EntityPM.InvoiceNotes != invoiceNotes) {
            this.EntityPM.InvoiceNotes = invoiceNotes;
        }
    }

    ValidateHsm(hsm){
        var res={
            valid:true,
            errorMsg:''
        };
        if(hsm != null) {
            if (hsm.length >15) {
                res.valid=false;
                res.errorMsg="HSM maximum size can be 15 digits";
            }
        }
        return res;
    }


    ValidateSigned() {
       
        this.ValidationErrorsList = [];

        this.UIProperties.SetValidity("HSM", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("HSMtoken", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("HSMaddress", this.ObjectTableName, true, "");

        var validateHsmResult=this.ValidateHsm(this.HSM);
        if (!validateHsmResult.valid) {
            this.ValidationErrorsList.push(validateHsmResult.errorMsg);
        }
        this.UIProperties.SetValidity("HSM", this.ObjectTableName, validateHsmResult.valid, validateHsmResult.errorMsg);
        
        

        if(this.HSMtoken != null ){

            if(this.HSMtoken.toString().length != 36 ){
                this.ValidationErrorsList.push( TextCodeTranslator.Translate("FullAccountingSetting.O.HSMTokenLong"));
                this.UIProperties.SetValidity("HSMtoken", this.ObjectTableName, false, TextCodeTranslator.Translate("FullAccountingSetting.O.HSMTokenLong"));
    
            }
    
            if(!this.ValidateFormatHSMToken(this.HSMtoken)) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("FullAccountingSetting.O.HSMTokenFormat"));
                this.UIProperties.SetValidity("HSMtoken", this.ObjectTableName, false, TextCodeTranslator.Translate("FullAccountingSetting.O.HSMTokenFormat"));
    
            }

        }

        

        if(this.HSMaddress != null) {
            if(this.HSMaddress.toString().length >= 50) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("FullAccountingSetting.O.HSMAddressLong"));
                this.UIProperties.SetValidity("HSMaddress", this.ObjectTableName, false, TextCodeTranslator.Translate("FullAccountingSetting.O.HSMAddressLong"));
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


        if (errors.length == 0)
            this.ValidateSigned();


        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
            this.SubmitChanges("");
        }


    }

    SubmitChanges(ControlAccountId: string) {
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
                if (SessionLocator.TenantPM.IsHybrid) {
                    this.AddOtherChargeType();
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

    SetAgingPeriodsFields() {
        this.EntityPM.FirstPeriodsMonths = this.JoinCodesOfPeriods(this.SelectedPeriods1);
        this.EntityPM.SecondPeriodsMonths = this.JoinCodesOfPeriods(this.SelectedPeriods2);
        this.EntityPM.ThirdsPeriodsMonths = this.JoinCodesOfPeriods(this.SelectedPeriods3);
    }

    ResetAgingPeriodsFields() {
        const thirdPeriodIsNotSelected = this.NumberOfPeriods <= 2;
        if (thirdPeriodIsNotSelected)
            this.ResetThirdPeriod();

        const secondPeriodIsNotSelected = this.NumberOfPeriods == 1;
        if (secondPeriodIsNotSelected)
            this.ResetSecondPeriod();
    }

    private ResetSecondPeriod() {
        this.EntityPM.SecondPeriodsMonths = null;
        this.SelectedPeriods2 = [];
    }


    private ResetThirdPeriod() {
        this.EntityPM.ThirdsPeriodsMonths = null;
        this.SelectedPeriods3 = [];
    }

    SetSelectedAgingPeriods() {

        this.SelectedPeriods1 = this.SetSelectedPeriods(this.EntityPM.FirstPeriodsMonths);
        this.SelectedPeriods2 = this.SetSelectedPeriods(this.EntityPM.SecondPeriodsMonths);
        this.SelectedPeriods3 = this.SetSelectedPeriods(this.EntityPM.ThirdsPeriodsMonths);
    }

    JoinCodesOfPeriods(periods: any[]) {
        if (periods)
            return periods.map(d => d.Code)?.join(',');
    }

    SetSelectedPeriods(joinedPeriodsCodes: string) {
        if (joinedPeriodsCodes) {
            var codes = joinedPeriodsCodes.split(',');
            return this.Periods.filter(d => codes.includes(d.Code));
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

        this.TabsSource.push({ Name: "Signed", isSelected: false, Header: TextCodeTranslator.Translate("Accounting.General.O.Signeds") });
        if(this.AccountingActivated){
            this.AddAgingDefinitionTab();
            this.disabledCopyFromTenant0 = false
        }
        this.TabsSource.push({ Name: "CopyFromTenant0", isSelected: false, Header: TextCodeTranslator.Translate("FullAccountingSetting.O.CopyFromTenant0") });


    }

    RemoveAgingDefinitionTab() {
        var tabIndex = this.TabsSource.findIndex(d => d.Name == "AgingDefinition");
        if (tabIndex > 0)
            this.TabsSource.splice(tabIndex, 1);
    }
    AddAgingDefinitionTab() {

        var tabIndex = this.TabsSource.findIndex(d => d.Name == "AgingDefinition");
        if (tabIndex < 0)
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
    CopyTable(tableName: string) {


        var copyfromtenant0 = new CopyFromTenant0PM();
        if (!AppTool.IsNullOrEmpty(tableName)) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
            this.copyFromTenant0ExtendedListService.copyTableFromTenant0(tableName).subscribe((myResult: any) => {

                var result: ServiceResponse = myResult;
                if (!result.HasError) {
                    copyfromtenant0.TableName = tableName;
                    copyfromtenant0.CreatedByUserId = SessionLocator.LoggedUserId;
                    copyfromtenant0.CreatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
                    this.copyFromTenant0PMService.insert(copyfromtenant0).subscribe((res: any) => {
                        var result2: ServiceResponse = res;
                        if (!result2.HasError)
                            this.GetValueCopyFromTenant0()
                         this.CurrentSession.StopBusyIndicator();

                    });
                }
                else{
                    this.CurrentSession.StopBusyIndicator();
                    var myMessageWindow = new MessageWindow();
                    var error=result.ErrorsArray.join();
                    myMessageWindow.Show(error);
                }
            });



        }


    }
    
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
    FullAccountingAddControl(ControlAccountId: string) {
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

    AddOtherChargeType() {
        var filters = new ApiQueryFilters(true);
        const chargeTypeCode = "OTHC";
        filters.addAdditionalFilter("Code", chargeTypeCode, null, null, "Equals", false, false, false, "string");

        this._chargesTypeListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null && !myResponse.HasError && myResponse?.Result?.length == 0) {
                var chargesTypePM = this._chargesTypePMService.GetNewEntityPM();
                ChargesTypePMInitService.InitValuesForAccounting(chargesTypePM, true);
                chargesTypePM.Code = chargeTypeCode;
                chargesTypePM.LocalName = "Other Charges (from Unifreight)";
                chargesTypePM.EnglishName = "Other Charges (from Unifreight)";

                this._chargesTypePMService.insert(chargesTypePM).subscribe((myResponse: ServiceResponse) => {
                });
            }
        });
    }

    public ShowLocals: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
     //public isRTL = ObjectsLocator.GlobalSetting ? (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl") : false;


    public get NumberOfPeriods(): number {
        return this.EntityPM.NumberofPeriods;
    }
    public set NumberOfPeriods(value: number) {
        this.EntityPM.NumberofPeriods = value;

        this.ResetAgingPeriodsFields();
    }

    Periods: any[] = [
        { EnglishName: 'Period 0', LocalName: 'תקופה גיול 0', Code: 'Period0' },
        { EnglishName: 'Period 1', LocalName: 'תקופה גיול 1', Code: 'Period1' },
        { EnglishName: 'Period 2', LocalName: 'תקופה גיול 2', Code: 'Period2' },
        { EnglishName: 'Period 3', LocalName: 'תקופה גיול 3', Code: 'Period3' },
        { EnglishName: 'Period 4', LocalName: 'תקופה גיול 4', Code: 'Period4' },
        { EnglishName: 'Period 5', LocalName: 'תקופה גיול 5', Code: 'Period5' },
        { EnglishName: 'Period Past', LocalName: 'לפני התקופה', Code: 'PeriodPast' }
        // {EnglishName: 'Period Future', LocalName: 'xxxx התקופה', Code: 'PeriodFuture'}
    ];

    // fill these arrays from database
    SelectedPeriods1: any[] = [];
    SelectedPeriods2: any[] = [];
    SelectedPeriods3: any[] = [];

}
