import { Component, OnDestroy } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { AccountingSettingPM } from '../../../../Common/EntityPMs/AccountingSettingPM';
import { AccountingSettingPMService } from '../../../../Common/Services/StandardPMs/AccountingSettingPMService';
import { GlobalDomainService } from '../../../../Common/Services/GlobalDomainService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CommonDomainService } from '../../../../Common/Services/CommonDomainService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsUpdater } from '../../../../Infrastructure/Locators/ObjectsUpdater';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    templateUrl: './TransferSettingsComponent.html',
})

export class TransferSettingsComponent extends BaseComponent implements OnDestroy {
    public EntityPM: AccountingSettingPM;
    public DataContext = this;
    public ObjectTableName: string = "AccountingSetting";
    public ValidationErrorsList: string[];
    public IsResourcesReady: boolean = false;
    public CanTransferToDropbox: boolean = false;
    public OldSessionAccountingSystem: any;
    private entityPMService: AccountingSettingPMService;
    private myGlobalDomainService: GlobalDomainService;
    private CommonDomainService: CommonDomainService;

    get NoEntity() {
        if (AppTool.IsNullOrEmpty(this.AccountingSystemCode))
            return true;
        return false;
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.QBOWindowSessionEvent);
    }
    private QBOWindowSessionEvent: any = null;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entityResourceService: EntityResourceService) {
        super();
        this.OldSessionAccountingSystem = SessionLocator.AccountingSystemPM;
        this.QBOWindowSessionEvent = this.CurrentSession.SessionEvent.subscribe(res => {
            if (res.Name == "QBOWindowCLosed") {
                this.QBOWindowCLosed(res.Timer);
                this.RefreshData();
            }
        });

        entityResourceService.getEntityResourceByTableName("AccountingSetting").subscribe(res => {
            this.InitializeServices();
            this.LoadData();
        });
    }

    InitializeServices() {
        this.entityPMService = new AccountingSettingPMService();
        this.myGlobalDomainService = new GlobalDomainService();
        this.CommonDomainService = new CommonDomainService();
    }

    RefreshData() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.entityPMService.get(SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.EntityPM.QBOAccessToken = myResponse.Result.QBOAccessToken;
                this.EntityPM.QBOAccessTokenSecret = myResponse.Result.QBOAccessTokenSecret;
                this.EntityPM.RefreshToken = myResponse.Result.RefreshToken;
                this.EntityPM.QBOOAuth = myResponse.Result.QBOOAuth;

                var temp = this.EntityPM.QBOrealMeID;
                this.EntityPM.QBOrealMeID = myResponse.Result.QBOrealMeID;
            }

            this.SetQuickBookProperties();
            this.CurrentSession.StopBusyIndicator();

            if (!AppTool.IsNullOrEmpty(temp) && this.EntityPM.QBOrealMeID != temp) {
                var window: MessageWindow = new MessageWindow();
                window.ShowWarningIcon = true;
                window.Show("You are connecting to a different QBO environment than the previously connected. Please notice that old QBO translations will be erased");
                window.WindowClosed.subscribe((P: any) => {
                    this.IsResourcesReady = true;
                });
            }
            else {
                this.IsResourcesReady = true;
            }
        });
    }

    LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.entityPMService.get(SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.EntityPM = myResponse.Result;
                if (this.EntityPM.AccountingSystemCode != "QBO" && this.EntityPM.AccountingSystemCode != "QBOG") {
                    this.EntityPM.QBOAccessToken = null;
                    this.EntityPM.QBOAccessTokenSecret = null;
                    this.EntityPM.QBOrealMeID = null;
                    this.EntityPM.RefreshToken = null;
                    this.EntityPM.QBOOAuth = 0;

                }
            }

            this.SetUIProperties();
            this.SetQuickBookProperties();
            this.CurrentSession.StopBusyIndicator();
            this.IsResourcesReady = true;
        });
    }

    public IsAccountingSystem_NO: boolean = false;
    public IsAccountingSystem_HV_RH: boolean = false;
    public IsAccountingSystem_QB_QBO: boolean = false;
    public IsAccountingSystem_AI_GI: boolean = false;
    SetUIProperties() {
        var isAccountingSystem_NO: boolean = false;
        var isAccountingSystem_HV_RH: boolean = false;
        var isAccountingSystem_QB_QBO: boolean = false;
        var isAccountingSystem_AI_GI: boolean = false;

        if (this.AccountingSystemCode == "NO") {
            isAccountingSystem_NO = true;
        }

        else if (this.AccountingSystemCode == "HV" || this.AccountingSystemCode == "RH") {
            isAccountingSystem_HV_RH = true;
        }

        else if (this.AccountingSystemCode == "QB" || this.AccountingSystemCode == "QBO" || this.AccountingSystemCode == "QBOG") {
            isAccountingSystem_QB_QBO = true;
        }

        else if (this.AccountingSystemCode == "GI" || this.AccountingSystemCode == "AI") {
            isAccountingSystem_AI_GI = true;
        }

        this.IsAccountingSystem_NO = isAccountingSystem_NO;
        this.IsAccountingSystem_HV_RH = isAccountingSystem_HV_RH;
        this.IsAccountingSystem_QB_QBO = isAccountingSystem_QB_QBO;
        this.IsAccountingSystem_AI_GI = isAccountingSystem_AI_GI;

        var isDemoTenant = false;
        if (SessionLocator.Tenant == 65) {
            isDemoTenant = true;
            if (SessionLocator.LoggedUserPM.Email) {
                if (SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                    isDemoTenant = false;
                }
            }
        }

        if (isDemoTenant) {
            this.UIProperties.SetEnabled("IsARInvoicesTransferEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsAPInvoicesTransferEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsARPaymentsTransferEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsAPPaymentsTransferEnabled", this.ObjectTableName, false);
        }

        else {
            var isARInvoicesTransferEnabled = false;
            var isAPInvoicesTransferEnabled = false;

            var isARPaymentsTransferEnabled = false;
            var isAPPaymentsTransferEnabled = false;

            if (SessionLocator.AccountingSystemPM) {
                isARInvoicesTransferEnabled = SessionLocator.AccountingSystemPM.AllowARInvoicesTransfer;
                isAPInvoicesTransferEnabled = SessionLocator.AccountingSystemPM.AllowAPInvoicesTransfer;
                isARPaymentsTransferEnabled = SessionLocator.AccountingSystemPM.AllowARPaymentsTransfer;
                isAPPaymentsTransferEnabled = SessionLocator.AccountingSystemPM.AllowAPPaymentsTransfer;
            }

            this.UIProperties.SetEnabled("IsARInvoicesTransferEnabled", this.ObjectTableName, isARInvoicesTransferEnabled);
            this.UIProperties.SetEnabled("IsAPInvoicesTransferEnabled", this.ObjectTableName, isAPInvoicesTransferEnabled);

            this.UIProperties.SetEnabled("IsARPaymentsTransferEnabled", this.ObjectTableName, isARPaymentsTransferEnabled);
            this.UIProperties.SetEnabled("IsAPPaymentsTransferEnabled", this.ObjectTableName, isAPPaymentsTransferEnabled);
        }

        this.UIProperties.SetEnabled("ARInvoiceTransferStartDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("APInvoiceTransferStartDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TransferToDropboxActivated", this.ObjectTableName, this.IsDropBoxConnected);

        var isReceivableVATableTempCardRequired = false;
        var isReceivableVATExemptTempCardRequired = false;
        var isPayableVATableTempCardRequired = false;
        var isPayableVATExemptTempCardRequired = false;
        var isReceivableVATCardRequired = false;
        var isPayableVATCardRequired = false;

        if (this.IsAccountingSystem_HV_RH) {
            if (this.IsARInvoicesTransferEnabled) {
                if (AppTool.IsNullOrEmpty(this.ReceivableVATableTempCard)) {
                    isReceivableVATableTempCardRequired = true;
                }

                if (AppTool.IsNullOrEmpty(this.ReceivableVATExemptTempCard)) {
                    isReceivableVATExemptTempCardRequired = true;
                }

                if (AppTool.IsNullOrEmpty(this.ReceivableVATCard)) {
                    isReceivableVATCardRequired = true;
                }
            }

            if (this.IsAPInvoicesTransferEnabled) {
                if (AppTool.IsNullOrEmpty(this.PayableVATableTempCard)) {
                    isPayableVATableTempCardRequired = true;
                }

                if (AppTool.IsNullOrEmpty(this.PayableVATExemptTempCard)) {
                    isPayableVATExemptTempCardRequired = true;
                }

                if (AppTool.IsNullOrEmpty(this.PayableVATCard)) {
                    isPayableVATCardRequired = true;
                }
            }
        }

        this.UIProperties.SetVisibility("ReceivableVATableTempCard", this.ObjectTableName, this.IsAccountingSystem_HV_RH);
        this.UIProperties.SetVisibility("ReceivableVATExemptTempCard", this.ObjectTableName, this.IsAccountingSystem_HV_RH);
        this.UIProperties.SetVisibility("PayableVATableTempCard", this.ObjectTableName, this.IsAccountingSystem_HV_RH);
        this.UIProperties.SetVisibility("PayableVATExemptTempCard", this.ObjectTableName, this.IsAccountingSystem_HV_RH);
        this.UIProperties.SetVisibility("ReceivableVATCard", this.ObjectTableName, this.IsAccountingSystem_HV_RH);
        this.UIProperties.SetVisibility("PayableVATCard", this.ObjectTableName, this.IsAccountingSystem_HV_RH);

        this.UIProperties.SetRequired("ReceivableVATableTempCard", this.ObjectTableName, isReceivableVATableTempCardRequired);
        this.UIProperties.SetRequired("ReceivableVATExemptTempCard", this.ObjectTableName, isReceivableVATExemptTempCardRequired);
        this.UIProperties.SetRequired("PayableVATableTempCard", this.ObjectTableName, isPayableVATableTempCardRequired);
        this.UIProperties.SetRequired("PayableVATExemptTempCard", this.ObjectTableName, isPayableVATExemptTempCardRequired);
        this.UIProperties.SetRequired("ReceivableVATCard", this.ObjectTableName, isReceivableVATCardRequired);
        this.UIProperties.SetRequired("PayableVATCard", this.ObjectTableName, isPayableVATCardRequired);
    }

    public isLogedInQBO: boolean = false;
    public isQBO: boolean = false;
    SetQuickBookProperties() {

        if (this.EntityPM.RefreshToken != null) {
            this.isLogedInQBO = true;
        }

        else {
            this.isLogedInQBO = false;
        }
    }

    QBOWindowCLosed(timer: any = null) {
        if (timer != null) {
            clearInterval(timer);
        }
    }

    public get AccountingSystemCode() { return this.EntityPM.AccountingSystemCode; }
    public set AccountingSystemCode(value: string) {
        if (this.EntityPM.AccountingSystemCode != value) {
            this.EntityPM.AccountingSystemCode = value;

            // For Disabling QBO Submit if not connected .
            if (value == "QBO" || value == "QBOG")
                this.isQBO = true;
            else
                this.isQBO = false;

            if (!AppTool.IsNullOrEmpty(value)) {
                if (value == "NO") {
                    this.ReceivableVATableTempCard = null;
                    this.ReceivableVATExemptTempCard = null;
                    this.PayableVATableTempCard = null;
                    this.PayableVATExemptTempCard = null;
                    this.ReceivableVATCard = null;
                    this.PayableVATCard = null;
                }

                this.myGlobalDomainService.GetAccountingSystem(this.AccountingSystemCode).subscribe((myResponse2: ServiceResponse) => {
                    if (!myResponse2.HasError) {
                        SessionLocator.AccountingSystemPM = myResponse2.Result;
                        this.IsARInvoicesTransferEnabled = SessionLocator.AccountingSystemPM.AllowARInvoicesTransfer;
                        this.IsAPInvoicesTransferEnabled = SessionLocator.AccountingSystemPM.AllowAPInvoicesTransfer;
                        this.IsARPaymentsTransferEnabled = SessionLocator.AccountingSystemPM.AllowARPaymentsTransfer;
                        this.IsAPPaymentsTransferEnabled = SessionLocator.AccountingSystemPM.AllowAPPaymentsTransfer;

                        this.SetUIProperties();
                    }
                });
            }
        }
    }
    SelectedItemChanged(AccountingSystem) {
        if (AccountingSystem != null) {
            this.SetUIProperties();

            this.CanTransferToDropbox = AccountingSystem.CanTransferToDropbox;
            if (this.CanTransferToDropbox == true) {
                this.CheckDropBoxConnection();
            }
        }
    }

    public get ARInvoiceTransferStartDate() { return this.EntityPM.ARInvoiceTransferStartDate; }
    public set ARInvoiceTransferStartDate(value: Date) {
        if (this.EntityPM.ARInvoiceTransferStartDate != value) {
            this.EntityPM.ARInvoiceTransferStartDate = value;
        }
    }

    public get APInvoiceTransferStartDate() { return this.EntityPM.APInvoiceTransferStartDate; }
    public set APInvoiceTransferStartDate(value: Date) {
        if (this.EntityPM.APInvoiceTransferStartDate != value) {
            this.EntityPM.APInvoiceTransferStartDate = value;
        }
    }

    public get ReceivableVATableTempCard() { return this.EntityPM.ReceivableVATableTempCard }
    public set ReceivableVATableTempCard(value: string) {
        if (this.EntityPM.ReceivableVATableTempCard != value) {
            this.EntityPM.ReceivableVATableTempCard = value;
            this.SetUIProperties();
        }
    }

    public get ReceivableVATExemptTempCard() { return this.EntityPM.ReceivableVATExemptTempCard }
    public set ReceivableVATExemptTempCard(value: string) {
        if (this.EntityPM.ReceivableVATExemptTempCard != value) {
            this.EntityPM.ReceivableVATExemptTempCard = value;
            this.SetUIProperties();
        }
    }

    public get PayableVATableTempCard() { return this.EntityPM.PayableVATableTempCard }
    public set PayableVATableTempCard(value: string) {
        if (this.EntityPM.PayableVATableTempCard != value) {
            this.EntityPM.PayableVATableTempCard = value;
            this.SetUIProperties();
        }
    }

    public get PayableVATExemptTempCard() { return this.EntityPM.PayableVATExemptTempCard }
    public set PayableVATExemptTempCard(value: string) {
        if (this.EntityPM.PayableVATExemptTempCard != value) {
            this.EntityPM.PayableVATExemptTempCard = value;
            this.SetUIProperties();
        }
    }

    public get ReceivableVATCard() { return this.EntityPM.ReceivableVATCard }
    public set ReceivableVATCard(value: string) {
        if (this.EntityPM.ReceivableVATCard != value) {
            this.EntityPM.ReceivableVATCard = value;
            this.SetUIProperties();
        }
    }

    public get PayableVATCard() { return this.EntityPM.PayableVATCard }
    public set PayableVATCard(value: string) {
        if (this.EntityPM.PayableVATCard != value) {
            this.EntityPM.PayableVATCard = value;
            this.SetUIProperties();
        }
    }

    get IsAPInvoicesTransferEnabled() { return this.EntityPM.IsAPInvoicesTransferEnabled; }
    set IsAPInvoicesTransferEnabled(value: boolean) {
        if (this.EntityPM.IsAPInvoicesTransferEnabled != value) {
            this.EntityPM.IsAPInvoicesTransferEnabled = value;

            this.SetUIProperties();
        }
    }

    get IsARPaymentsTransferEnabled() { return this.EntityPM.IsARPaymentsTransferEnabled; }
    set IsARPaymentsTransferEnabled(value: boolean) {
        if (this.EntityPM.IsARPaymentsTransferEnabled != value) {
            this.EntityPM.IsARPaymentsTransferEnabled = value;
        }
    }

    get IsARInvoicesTransferEnabled() { return this.EntityPM.IsARInvoicesTransferEnabled; }
    set IsARInvoicesTransferEnabled(value: boolean) {
        if (this.EntityPM.IsARInvoicesTransferEnabled != value) {
            this.EntityPM.IsARInvoicesTransferEnabled = value;

            this.SetUIProperties();
        }
    }


    get IsAPPaymentsTransferEnabled() { return this.EntityPM.IsAPPaymentsTransferEnabled; }
    set IsAPPaymentsTransferEnabled(value: boolean) {
        if (this.EntityPM.IsAPPaymentsTransferEnabled != value) {
            this.EntityPM.IsAPPaymentsTransferEnabled = value;
        }
    }




    public get TransferToDropboxActivated() { return this.EntityPM.TransferToDropboxActivated; }
    public set TransferToDropboxActivated(value: boolean) {
        if (this.EntityPM.TransferToDropboxActivated != value) {
            this.EntityPM.TransferToDropboxActivated = value;
        }
    }

    private isDropBoxConnected: boolean = false;
    get IsDropBoxConnected() {
        return this.isDropBoxConnected;
    }
    set IsDropBoxConnected(value: boolean) {
        this.isDropBoxConnected = value;
    }

    RunConnectQuickBooks() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "";
        logWindow.Show('./Invoice/Components/Workspaces/QuickBooksLogin');
    }

    DissConnectQBO(loadeding = true) {
        if (loadeding) {
            this.CurrentSession.StartBusyIndicator("Disconnecting..");
            this.EntityPM.QBOAccessToken = null;
            this.EntityPM.QBOAccessTokenSecret = null;
            this.EntityPM.RefreshToken = null;
            this.EntityPM.QBOOAuth = 0;


        }

        if (this.EntityPM.AccountingSystemCode == "QBO" || this.EntityPM.AccountingSystemCode == "QBOG") {
            this.EntityPM.AccountingSystemCode = "NO";

            this.entityPMService.update(this.EntityPM).subscribe((myResponse1: ServiceResponse) => {
                if (myResponse1.HasError) {
                    this.ValidationErrorsList = myResponse1.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    ObjectsUpdater.UpdateAccountingSettingPM(this.EntityPM);

                    this.myGlobalDomainService.GetAccountingSystem(this.AccountingSystemCode).subscribe((myResponse2: ServiceResponse) => {
                        if (!myResponse2.HasError) {
                            SessionLocator.AccountingSystemPM = myResponse2.Result;
                        }
                    });

                    this.SetQuickBookProperties();
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }

    private IsQuickBooksWindowOpened: boolean = false;
    ViewXMLClicked() {
        var dualScreenLeft = window.screenLeft;
        var dualScreenTop = window.screenTop;

        var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
        var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

        var left = ((width / 2) - (1000 / 2)) + dualScreenLeft;
        var top = ((height / 2) - (650 / 2)) + dualScreenTop;
        var link = AppTool.GetLogitudeURL() + "QuickbooksOnlineAuth2.aspx?connect=true&tenant=" + SessionLocator.Tenant;

        var new_window = window.open(link, "Authenticate with Quickbooks Online", 'scrollbars=yes, width=' + 1000 + ', height=' + 650 + ', top=' + top + ', left=' + left + ',directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no');

        if (window.focus) {
            new_window.focus();
        }
        var timer = setInterval(function () {
            if (new_window) {
                if (new_window.closed) {
                    if (this.CurrentSession == null)
                        this.CurrentSession = SessionLocator.SelectedSession;
                    this.CurrentSession.SessionEvent.emit({ Name: "QBOWindowCLosed", Timer: timer });
                }
            }
        }, 500);

        this.IsQuickBooksWindowOpened = true;
    }

    EditTransferStartDateClicked(typeCode: string) {
        var logWindowTitle: string = null;
        switch (typeCode) {
            case "ARInvoice": { logWindowTitle = "AR Invoice"; break; }
            case "APInvoice": { logWindowTitle = "AP Invoice"; break; }
            case "ARPayment": { logWindowTitle = "AR Payment"; break; }
        }

        var logWindow = new LogitudeWindow();
        logWindow.Width = 550;
        logWindow.Height = 300;
        logWindow.WindowArgs = { EntityPM: this.EntityPM, Code: typeCode };
        logWindow.Title = "Edit " + logWindowTitle + " Transfer Start Date";
        logWindow.Show('./Invoice/Components/Workspaces/Windows/TransferStartDateComponent');
    }

    CancelButtonClicked() {
        this.ResetQBOSettings();
        SessionLocator.AccountingSystemPM = this.OldSessionAccountingSystem;
        this.CurrentSession.CloseCurrentWindow();
    }

    ResetQBOSettings() {
        this.entityPMService.get(SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
            var loadedEntity: AccountingSettingPM = myResponse.Result;
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            if (loadedEntity.AccountingSystemCode != "QBO" && loadedEntity.AccountingSystemCode != "QBOG") {
                this.EntityPM.AccountingSystemCode = loadedEntity.AccountingSystemCode;
                this.EntityPM.QBOrealMeID = null;
                this.EntityPM.QBOAccessToken = null;
                this.EntityPM.QBOAccessTokenSecret = null;
                this.EntityPM.RefreshToken = null;
                this.EntityPM.QBOOAuth = 0;


                this.DissConnectQBO(false);
            }
        });
    }

    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (this.AccountingSystemCode == "HV" || this.AccountingSystemCode == "RH") {
            if (this.IsARInvoicesTransferEnabled) {
                if (AppTool.IsNullOrEmpty(this.ReceivableVATableTempCard)) {
                    errors.push(TextCodeTranslator.Translate("AccountingSetting.F.ReceivableVATableTempCard"));
                }

                if (AppTool.IsNullOrEmpty(this.ReceivableVATExemptTempCard)) {
                    errors.push(TextCodeTranslator.Translate("AccountingSetting.F.ReceivableVATExemptTempCard"));
                }

                if (AppTool.IsNullOrEmpty(this.ReceivableVATCard)) {
                    errors.push(TextCodeTranslator.Translate("AccountingSetting.F.ReceivableVATCard"));
                }
            }

            if (this.IsAPInvoicesTransferEnabled) {
                if (AppTool.IsNullOrEmpty(this.PayableVATableTempCard)) {
                    errors.push(TextCodeTranslator.Translate("AccountingSetting.F.PayableVATableTempCard"));
                }

                if (AppTool.IsNullOrEmpty(this.PayableVATExemptTempCard)) {
                    errors.push(TextCodeTranslator.Translate("AccountingSetting.F.PayableVATExemptTempCard"));
                }

                if (AppTool.IsNullOrEmpty(this.PayableVATCard)) {
                    errors.push(TextCodeTranslator.Translate("AccountingSetting.F.PayableVATCard"));
                }
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            if (this.AccountingSystemCode != "QBO" && this.AccountingSystemCode != "QBOG") {
                //if (this.EntityPM.QBOrealMeID) {
                //    this.EntityPM.QBOrealMeID = null;
                //}

                if (this.EntityPM.QBOAccessToken) {
                    this.EntityPM.QBOAccessToken = null;
                }

                if (this.EntityPM.QBOAccessTokenSecret) {
                    this.EntityPM.QBOAccessTokenSecret = null;
                }

                if (this.EntityPM.RefreshToken) {
                    this.EntityPM.RefreshToken = null;
                }

                if (this.EntityPM.QBOOAuth) {
                    this.EntityPM.QBOOAuth = 0;
                }
            }

            if (this.IsQuickBooksWindowOpened && (this.AccountingSystemCode == "QBO" || this.AccountingSystemCode == "QBOG")) {
                this.entityPMService.get(SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }

                    else {
                        var loadedEntity: AccountingSettingPM = myResponse.Result;

                        if (this.EntityPM.QBOrealMeID != loadedEntity.QBOrealMeID) {
                            this.EntityPM.QBOrealMeID = loadedEntity.QBOrealMeID;
                        }

                        if (this.EntityPM.QBOAccessToken != loadedEntity.QBOAccessToken) {
                            this.EntityPM.QBOAccessToken = loadedEntity.QBOAccessToken;
                        }

                        if (this.EntityPM.QBOAccessTokenSecret != loadedEntity.QBOAccessTokenSecret) {
                            this.EntityPM.QBOAccessTokenSecret = loadedEntity.QBOAccessTokenSecret;
                        }

                        if (this.EntityPM.RefreshToken != loadedEntity.RefreshToken) {
                            this.EntityPM.RefreshToken = loadedEntity.RefreshToken;
                        }

                        if (this.EntityPM.QBOOAuth != loadedEntity.QBOOAuth) {
                            this.EntityPM.QBOOAuth = loadedEntity.QBOOAuth;
                        }

                        this.SaveChanges();
                    }
                });
            }

            else {
                this.SaveChanges();
            }
        }
    }

    SaveChanges() {
        if (!this.EntityPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }

        else {
            this.entityPMService.update(this.EntityPM).subscribe((myResponse1: ServiceResponse) => {
                if (myResponse1.HasError) {
                    this.ValidationErrorsList = myResponse1.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    SessionLocator.AccountingSettingPM = this.EntityPM;

                    this.myGlobalDomainService.GetAccountingSystem(this.AccountingSystemCode).subscribe((myResponse2: ServiceResponse) => {
                        if (!myResponse2.HasError) {
                            SessionLocator.AccountingSystemPM = myResponse2.Result;
                            this.OldSessionAccountingSystem = SessionLocator.AccountingSystemPM;
                        }

                        this.CurrentSession.FireEvent("RefreshTransferComponent");
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    });
                }
            });
        }
    }

    SendDropBoxChecked(arg) {
        if (arg == true) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Show("Activating this option will send the invoice transfer file to the connected dropbox account on invoice approval");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    //nth
                }
                else if (confirmWindow.No) {
                    this.TransferToDropboxActivated = false;
                }
            });
        }
    }

    CheckDropBoxConnection() {
        this.CommonDomainService.GetDropBoxAccessTocken(SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
            if (response.HasError) {
                this.IsDropBoxConnected = false;
            }
            else {
                this.IsDropBoxConnected = true;
            }

            this.SetUIProperties();
        });
    }

    ConnectDropBox() {
        var windowTitle = "Dropbox Connection";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = 225;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = false;
        logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/DropBox/DropBoxConnectionComponent');
    }
}
