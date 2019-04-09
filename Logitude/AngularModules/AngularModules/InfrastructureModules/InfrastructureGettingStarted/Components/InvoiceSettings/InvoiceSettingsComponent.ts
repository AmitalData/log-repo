import {Component, OnInit, AfterViewInit} from '@angular/core';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {AccountingSettingPM} from '../../../../Common/EntityPMs/AccountingSettingPM';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {AccountingSettingPMService} from '../../../../Common/Services/StandardPMs/AccountingSettingPMService';
import {TenantPMService} from '../../../../Common/Services/StandardPMs/TenantPMService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ObjectsUpdater} from '../../../../Infrastructure/Locators/ObjectsUpdater';

@Component({
    selector: 'InvoiceSettingsComponent',
    moduleId: module.id,
    templateUrl: './InvoiceSettingsComponent.html',
})

export class InvoiceSettingsComponent extends BaseComponent implements OnInit {
    public DataContext: InvoiceSettingsComponent = this;
    public ObjectTableName: string = "Tenant";
    private accountingSettings: AccountingSettingPM = new AccountingSettingPM();
    public ValidationErrorsList: string[];
    public IsVisible = false;

    private tenantPM: TenantPM = new TenantPM();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    ngOnInit() {
        this.GetTenant();
        this.GetAccountingSettings();
    }

    private isTenantFinished = false;
    private isAccountingSettingFinished = false;
    SetIsVisible() {
        if (this.isTenantFinished && this.isAccountingSettingFinished) {
            this.IsVisible = true;
        }
    }
    GetTenant() {
        var tenantService = new TenantPMService();
        tenantService.get(SessionLocator.Tenant).subscribe((myResult: ServiceResponse) => {
            if (!myResult.HasError) {
                this.tenantPM = myResult.Result;
                this.isTenantFinished = true;
                this.SetIsVisible();
            }
        });
    }
    GetAccountingSettings() {
        var accountingSettingPMService = new AccountingSettingPMService();
        accountingSettingPMService.get(SessionLocator.AccountingSettingPM.Id).subscribe((myResult: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResult.HasError) {
                this.accountingSettings = myResult.Result;
                this.isAccountingSettingFinished = true;
                this.SetIsVisible();
            }
        });
    }
    get InvoiceAddress1() { return this.tenantPM.InvoiceSection1; }
    set InvoiceAddress1(value: string) { this.tenantPM.InvoiceSection1 = value; }

    get InvoiceAddress2() { return this.tenantPM.InvoiceSection2; }
    set InvoiceAddress2(value: string) { this.tenantPM.InvoiceSection2 = value; }

    get BankDetails() { return this.tenantPM.BankDetails; }
    set BankDetails(value: string) { this.tenantPM.BankDetails = value; }

    get Voidinvoice() {
        return this.accountingSettings.AllowVoidARI;
    }
    set Voidinvoice(value: boolean) {
        this.accountingSettings.AllowVoidARI = value;
    }

    get AllowManualInvoiceNumber() {
        return this.accountingSettings.AllowManualInvoiceNumber;
    }
    set AllowManualInvoiceNumber(value: boolean) {
        this.accountingSettings.AllowManualInvoiceNumber = value;

        if (value) {
            this.IsARInvoiceChronologicalDates = false;
        }
    }

    get IsVatNumberMandatoryInAR() {
        return this.accountingSettings.IsVatNumberMandatoryInAR;
    }
    set IsVatNumberMandatoryInAR(value: boolean) {
        this.accountingSettings.IsVatNumberMandatoryInAR = value;
    }

    get IsARInvoiceChronologicalDates() {
        return this.accountingSettings.IsARInvoiceChronologicalDates;
    }
    set IsARInvoiceChronologicalDates(value: boolean) {
        this.accountingSettings.IsARInvoiceChronologicalDates = value;
    }

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.tenantPM, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
            var tenantService = new TenantPMService();
            tenantService.update(this.tenantPM).subscribe((myResult:ServiceResponse) => {
                if (!myResult.HasError) { // Success
                    this.UpdateAccountingSettings();
                }
                else {
                    this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
    }

    UpdateAccountingSettings() {
        var accountingSettingPMService = new AccountingSettingPMService();
        accountingSettingPMService.update(this.accountingSettings).subscribe((myResult: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResult.HasError) { // Success

                ObjectsUpdater.UpdateTenantPM(this.tenantPM);
                ObjectsUpdater.UpdateAccountingSettingPM(this.accountingSettings);
                this.CurrentSession.CloseCurrentWindow();
            }
            else {
                this.CurrentSession.CloseCurrentWindow();
            }
        });
    }
}

