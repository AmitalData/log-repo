import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {AccountingSettingPM} from '../../../../Common/EntityPMs/AccountingSettingPM';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPMService} from '../../../../Common/Services/StandardPMs/TenantPMService';
import {AccountingSettingPMService} from '../../../../Common/Services/StandardPMs/AccountingSettingPMService';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ObjectsUpdater} from '../../../../Infrastructure/Locators/ObjectsUpdater';

@Component({
    selector: 'AccountingSettingsComponent',
    moduleId: module.id,
    templateUrl: './AccountingSettingsComponent.html',
})

export class AccountingSettingsComponent extends BaseComponent {
    public EntityPM: AccountingSettingPM;
    public DataContext = this;
    public ObjectTableName: string = "AccountingSetting";
    public ValidationErrorsList: string[];
    public IsResourcesReady: boolean = false;
    public IsEnableMultiRateAPInvoicesVisible: boolean = false;
    public IsEnableMultiCurrencyAPPaymentsVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService){
        super();

        if (FeatureLocator.HasFeaturePermession("APInvoice", "EnableMultiRate")) {
            this.IsEnableMultiRateAPInvoicesVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("APPayment", "EnableMultiCurrency")) {
            this.IsEnableMultiCurrencyAPPaymentsVisible = true;
        }

        this.InitializeServices();
        this.LoadData();
    }

    private myTenantPMService: TenantPMService;
    private myAccountingSettingPMService: AccountingSettingPMService;
    InitializeServices() {
        this.myTenantPMService = new TenantPMService();
        this.myAccountingSettingPMService = new AccountingSettingPMService();
    }

    private LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.entityResourceService.getEntityResourceByTableName("AccountingSetting").subscribe(res2 => {
            this.myAccountingSettingPMService.get(SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result;

                    if (this.EntityPM) {
                        this.enableMultiPercentageVATTypes_Old = this.EntityPM.EnableMultiPercentageVATTypes;
                        this.IsResourcesReady = true;
                        this.SetUIProperties();
                    }
                }
            });
        });
    }

    public IsDemoTenant: boolean = false;
    public ShowRealSingleTaxCheckBox: boolean = false;
    public ShowDummySingleTaxCheckBox: boolean = false;
    SetUIProperties() {
        var isShowDummySingleTaxCheckBox = false;
        if (SessionLocator.AccountingSystemPM) {
            if (SessionLocator.AccountingSystemPM.IsSingleTaxPerInvoice) {
                isShowDummySingleTaxCheckBox = true;
            }
        }

        this.ShowRealSingleTaxCheckBox = !isShowDummySingleTaxCheckBox;
        this.ShowDummySingleTaxCheckBox = isShowDummySingleTaxCheckBox;

        var isDemoTenant = false;
        if (SessionLocator.Tenant == 65) {
            isDemoTenant = true;

            if (SessionLocator.LoggedUserPM.Email) {
                if (SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                    isDemoTenant = false;
                }
            }
        }

        this.IsDemoTenant = isDemoTenant;
        if (isDemoTenant) {
            this.UIProperties.SetEnabled("AllowVoidARI", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowVoidARP", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsVatNumberMandatoryInAR", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowMinusInvoicelines", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowPositiveAmountsInTheCreditNote", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowManualInvoiceNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsARInvoiceChronologicalDates", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsARPaymentChronologicalDates", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsARInvoicesTransferEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsARPaymentsTransferEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsSingleTaxPerInvoice", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowVoidAPI", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowVoidAPP", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsVatNumberMandatoryInAP", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowClosureWithoutPayables", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsAPInvoicesTransferEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("EnableMultiPercentageVATTypes", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("NotifyPastDateOnInvoiceEdit", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RegistryDateTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowManualARPaymentNumber", this.ObjectTableName, false);            
        }

        else {
            this.UIProperties.SetEnabled("IsARInvoiceChronologicalDates", this.ObjectTableName, !this.AllowManualInvoiceNumber);

            var isARInvoicesTransferEnabled = false;
            var isAPInvoicesTransferEnabled = false;
            var isARPaymentsTransferEnabled = false;
            //var isSingleTaxPerInvoiceEnabled = false;
            if (SessionLocator.AccountingSystemPM) {
                isARInvoicesTransferEnabled = SessionLocator.AccountingSystemPM.AllowARInvoicesTransfer;
                isAPInvoicesTransferEnabled = SessionLocator.AccountingSystemPM.AllowAPInvoicesTransfer;
                isARPaymentsTransferEnabled = SessionLocator.AccountingSystemPM.AllowARPaymentsTransfer;
                //isSingleTaxPerInvoiceEnabled = !SessionLocator.AccountingSystemPM.IsSingleTaxPerInvoice;
            }

            this.UIProperties.SetEnabled("IsARInvoicesTransferEnabled", this.ObjectTableName, isARInvoicesTransferEnabled);
            this.UIProperties.SetEnabled("IsAPInvoicesTransferEnabled", this.ObjectTableName, isAPInvoicesTransferEnabled);
            this.UIProperties.SetEnabled("IsARPaymentsTransferEnabled", this.ObjectTableName, isARPaymentsTransferEnabled);
            //this.UIProperties.SetEnabled("IsSingleTaxPerInvoice", this.ObjectTableName, isSingleTaxPerInvoiceEnabled);


            if (SessionLocator.TenantPM.CountryCode == "IL" && SessionLocator.LoggedUserPM.IsCustomerCare == false) {
                this.UIProperties.SetEnabled("AllowVoidARI", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("AllowVoidARP", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("IsVatNumberMandatoryInAR", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("AllowManualInvoiceNumber", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("IsARInvoiceChronologicalDates", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("IsARPaymentChronologicalDates", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("AllowVoidAPI", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("AllowVoidAPP", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("IsVatNumberMandatoryInAP", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("AllowManualARPaymentNumber", this.ObjectTableName, false);
                
            }
        }

        this.UIProperties.SetEnabled("EnableInvoiceStocksManagement", this.ObjectTableName, !this.IsARInvoiceChronologicalDates);
        this.SetUIProperties_RegistryDate();
    }
    SetUIProperties_RegistryDate() {
        var isEnabled: boolean = false;

        if (SessionLocator.LoggedUserPM.IsCustomerCare) {
            isEnabled = true;
        }

        else if (FeatureLocator.IsPackage_DVMT()) {
            isEnabled = true;
        }

        this.UIProperties.SetEnabled("RegistryDateTypeCode", this.ObjectTableName, isEnabled);
    }
    // Accounting Receivable
    get AllowVoidARI() { return this.EntityPM.AllowVoidARI; }
    set AllowVoidARI(value: boolean)
    {
        if (this.EntityPM.AllowVoidARI != value) {
            this.EntityPM.AllowVoidARI = value;
        }
    }

    get AllowVoidARP() { return this.EntityPM.AllowVoidARP; }
    set AllowVoidARP(value: boolean)
    {
        if (this.EntityPM.AllowVoidARP != value) {
            this.EntityPM.AllowVoidARP = value;
        }
    }

    get IsVatNumberMandatoryInAR() { return this.EntityPM.IsVatNumberMandatoryInAR; }
    set IsVatNumberMandatoryInAR(value: boolean) {
        if (this.EntityPM.IsVatNumberMandatoryInAR != value) {
            this.EntityPM.IsVatNumberMandatoryInAR = value;
        }
    }

    get AllowMinusInvoicelines() { return this.EntityPM.AllowMinusInvoicelines; }
    set AllowMinusInvoicelines(value: boolean) {
        if (this.EntityPM.AllowMinusInvoicelines != value) {
            this.EntityPM.AllowMinusInvoicelines = value;
        }
    }

    get AllowPositiveAmountsInTheCreditNote() { return this.EntityPM.AllowPositiveAmountsInTheCreditNote; }
    set AllowPositiveAmountsInTheCreditNote(value: boolean) {
        if (this.EntityPM.AllowPositiveAmountsInTheCreditNote != value) {
            this.EntityPM.AllowPositiveAmountsInTheCreditNote = value;
        }
    }

    get AllowManualInvoiceNumber() { return this.EntityPM.AllowManualInvoiceNumber; }
    set AllowManualInvoiceNumber(value: boolean) {
        if (this.EntityPM.AllowManualInvoiceNumber != value) {
            this.EntityPM.AllowManualInvoiceNumber = value;

            if (value) {
                this.IsARInvoiceChronologicalDates = false;
            }

            this.UIProperties.SetEnabled("IsARInvoiceChronologicalDates", this.ObjectTableName, !this.AllowManualInvoiceNumber);
        }
    }

    get AllowManualARPaymentNumber() { return this.EntityPM.AllowManualARPaymentNumber; }
    set AllowManualARPaymentNumber(value: boolean) {
        if (this.EntityPM.AllowManualARPaymentNumber != value) {
            this.EntityPM.AllowManualARPaymentNumber = value;
        }
    }

    get IsARInvoiceChronologicalDates() { return this.EntityPM.IsARInvoiceChronologicalDates; }
    set IsARInvoiceChronologicalDates(value: boolean) {
        if (this.EntityPM.IsARInvoiceChronologicalDates != value) {
            this.EntityPM.IsARInvoiceChronologicalDates = value;

            if (value) {
                this.EnableInvoiceStocksManagement = false;
            }

            this.UIProperties.SetEnabled("EnableInvoiceStocksManagement", this.ObjectTableName, !value);
        }
    }

    get IsARPaymentChronologicalDates() { return this.EntityPM.IsARPaymentChronologicalDates; }
    set IsARPaymentChronologicalDates(value: boolean) {
        if (this.EntityPM.IsARPaymentChronologicalDates != value) {
            this.EntityPM.IsARPaymentChronologicalDates = value;
        }
    }


    get IsARInvoicesTransferEnabled() { return this.EntityPM.IsARInvoicesTransferEnabled; }
    set IsARInvoicesTransferEnabled(value: boolean) {
        if (this.EntityPM.IsARInvoicesTransferEnabled != value) {
            this.EntityPM.IsARInvoicesTransferEnabled = value;
        }
    }

    get IsARPaymentsTransferEnabled() { return this.EntityPM.IsARPaymentsTransferEnabled; }
    set IsARPaymentsTransferEnabled(value: boolean) {
        if (this.EntityPM.IsARPaymentsTransferEnabled != value) {
            this.EntityPM.IsARPaymentsTransferEnabled = value;
        }
    }

    get IsSingleTaxPerInvoice() { return this.EntityPM.IsSingleTaxPerInvoice; }
    set IsSingleTaxPerInvoice(value: boolean) {
        if (this.EntityPM.IsSingleTaxPerInvoice != value) {
            this.EntityPM.IsSingleTaxPerInvoice = value;
        }
    }

   
    // Accounting Payables
    get AllowVoidAPI() { return this.EntityPM.AllowVoidAPI; }
    set AllowVoidAPI(value: boolean) {
        if (this.EntityPM.AllowVoidAPI != value) {
            this.EntityPM.AllowVoidAPI = value;
        }
    }

    get AllowVoidAPP() { return this.EntityPM.AllowVoidAPP; }
    set AllowVoidAPP(value: boolean) {
        if (this.EntityPM.AllowVoidAPP != value) {
            this.EntityPM.AllowVoidAPP = value;
        }
    }
  
    get IsVatNumberMandatoryInAP() { return this.EntityPM.IsVatNumberMandatoryInAP; }
    set IsVatNumberMandatoryInAP(value: boolean) {
        if (this.EntityPM.IsVatNumberMandatoryInAP != value) {
            this.EntityPM.IsVatNumberMandatoryInAP = value;
        }
    }

    get AllowClosureWithoutPayables() { return this.EntityPM.AllowClosureWithoutPayables; }
    set AllowClosureWithoutPayables(value: boolean)
    {
        if (this.EntityPM.AllowClosureWithoutPayables != value) {
            this.EntityPM.AllowClosureWithoutPayables = value;
        }
    }

    get IsAPInvoicesTransferEnabled() { return this.EntityPM.IsAPInvoicesTransferEnabled; }
    set IsAPInvoicesTransferEnabled(value: boolean)
    {
        if (this.EntityPM.IsAPInvoicesTransferEnabled != value) {
            this.EntityPM.IsAPInvoicesTransferEnabled = value;
        }
    }

    get EnableMultiRateAPInvoices() { return this.EntityPM.EnableMultiRateAPInvoices; }
    set EnableMultiRateAPInvoices(value: boolean) {
        if (this.EntityPM.EnableMultiRateAPInvoices != value) {
            this.EntityPM.EnableMultiRateAPInvoices = value;
        }
    }

    // Tenant Properties
    get VatNumber() { return this.EntityPM.VatNumber; }
    set VatNumber(value: string)
    {
        if (this.EntityPM.VatNumber != value) {
            this.EntityPM.VatNumber = value;
        }
    }

    get PaymentTermId() { return this.EntityPM.PaymentTermId; }
    set PaymentTermId(value: string) {
        if (this.EntityPM.PaymentTermId != value) {
            this.EntityPM.PaymentTermId = value;
        }
    }   

    private enableMultiPercentageVATTypes_Old: boolean;
    get EnableMultiPercentageVATTypes() { return this.EntityPM.EnableMultiPercentageVATTypes; }
    set EnableMultiPercentageVATTypes(value: boolean) {
        if (this.EntityPM.EnableMultiPercentageVATTypes != value) {
            this.EntityPM.EnableMultiPercentageVATTypes = value;
        }
    }

    get NotifyPastDateOnInvoiceEdit() { return this.EntityPM.NotifyPastDateOnInvoiceEdit; }
    set NotifyPastDateOnInvoiceEdit(value: boolean) {
        if (this.EntityPM.NotifyPastDateOnInvoiceEdit != value) {
            this.EntityPM.NotifyPastDateOnInvoiceEdit = value;
        }
    }

    get RegistryDateTypeCode() { return this.EntityPM.RegistryDateTypeCode; }
    set RegistryDateTypeCode(value: string) {
        if (this.EntityPM.RegistryDateTypeCode != value) {
            this.EntityPM.RegistryDateTypeCode = value;
        }
    }

    get EnableMultiCurrencyARPayments() { return this.EntityPM.EnableMultiCurrencyARPayments; }
    set EnableMultiCurrencyARPayments(value: boolean) {
        if (this.EntityPM.EnableMultiCurrencyARPayments != value) {
            this.EntityPM.EnableMultiCurrencyARPayments = value;
        }
    }

    get EnableMultiCurrencyAPPayments() { return this.EntityPM.EnableMultiCurrencyAPPayments; }
    set EnableMultiCurrencyAPPayments(value: boolean) {
        if (this.EntityPM.EnableMultiCurrencyAPPayments != value) {
            this.EntityPM.EnableMultiCurrencyAPPayments = value;
        }
    }

    get EnableNegativeOffsetARPayments() { return this.EntityPM.EnableNegativeOffsetARPayments; }
    set EnableNegativeOffsetARPayments(value: boolean) {
        if (this.EntityPM.EnableNegativeOffsetARPayments != value) {
            this.EntityPM.EnableNegativeOffsetARPayments = value;
        }
    }

    get EnableNegativeOffsetAPPayments() { return this.EntityPM.EnableNegativeOffsetAPPayments; }
    set EnableNegativeOffsetAPPayments(value: boolean) {
        if (this.EntityPM.EnableNegativeOffsetAPPayments != value) {
            this.EntityPM.EnableNegativeOffsetAPPayments = value;
        }
    }

    get EnableInvoiceStocksManagement() { return this.EntityPM.EnableInvoiceStocksManagement; }
    set EnableInvoiceStocksManagement(value: boolean) {
        if (this.EntityPM.EnableInvoiceStocksManagement != value) {
            this.EntityPM.EnableInvoiceStocksManagement = value;
        }
    }

    //Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        if (this.EntityPM.IsDirty) {

            this.CurrentSession.StartBusyIndicatorSaving();

            var isLoadingVatGroups = false;
            if (this.EnableMultiPercentageVATTypes != this.enableMultiPercentageVATTypes_Old) {
                if (this.EnableMultiPercentageVATTypes) {
                    isLoadingVatGroups = true;
                }
            }

            this.myAccountingSettingPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    this.EntityPM = myResponse.Result;     

                    ObjectsUpdater.UpdateAccountingSettingPM(this.EntityPM);

                    this.myTenantPMService.get(SessionLocator.Tenant).subscribe((myResponse1: ServiceResponse) => {
                        if (!myResponse1.HasError) {
                            InfraSettings.TenantPM = myResponse1.Result;
                        }

                        if (isLoadingVatGroups) {
                            var myCommonDomain = new CommonDomainService();
                            myCommonDomain.GetAllVatTypesGroups().subscribe((myResponse: ServiceResponse) => {
                                if (!myResponse.HasError) {
                                    SessionLocator.AllVatTypesGroups = myResponse.Result;
                                }

                                this.CurrentSession.CloseCurrentWindowEmit("OK");
                            });
                        }

                        else {
                            SessionLocator.AllVatTypesGroups = [];
                            this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                    });                   
                }
            });
        }

        else {
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }
    ViewAdvancedSettings() {
        var windowTitle = "Advanced Accounting Settings ";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        logWindow.DataContext = this;
        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/AccountingSettings/AccountingAdvancedSettingsComponent');
    }

    ManageStocksClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen_90 = true;
        logWindow.IsShowCloseButton = true;
        logWindow.Title = "Invoice Stocks";
        logWindow.Show('./InvoiceModules/InvoiceStocks/Components/ManageStocksComponent');
    }
}
