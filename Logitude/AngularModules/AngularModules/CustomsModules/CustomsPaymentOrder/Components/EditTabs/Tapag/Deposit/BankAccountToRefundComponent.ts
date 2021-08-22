import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent } from '../../../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { AppTool, DateTool } from '../../../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent'; import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DepositPM } from '../../../../../../Customs/EntityPMs/DepositPM';
import { CustomBankListService } from '../../../../../../Customs/Services/StandardLists/CustomBankListService';
import { BankAccountToRefundRequestParams } from '../../../../../../Customs/DataContract/RequestParams/BankAccountToRefundRequestParams';
import { EntityResourceService } from '../../../../../../Infrastructure/Services/EntityResourceService';
import { CustomBankList } from '../../../../../../Customs/EntityLists/CustomBankList';
import { TapagMessagesService } from '../../../../../../Customs/Services/WebServices/TapagMessagesService';

declare var window: any;

@Component({
    
    templateUrl: './BankAccountToRefundComponent.html',
    selector: 'BankAccountToRefundComponent',
})

export class BankAccountToRefundComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {

    public EntityPM: DepositPM = new DepositPM();
    public ObjectTableName = "Customs.Deposit";
    public DataContext: BankAccountToRefundComponent = this;
    public ObjectTableId: string;
    public CurrentEditComponentId: string;
    public integer: any;
    _TapagMessagesService: TapagMessagesService = new TapagMessagesService();
    _CustomBankListService: CustomBankListService = new CustomBankListService();
    public banksList: CustomBankList[] = [];
    private declarationId: string;

    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.ValidationErrorsList = [];

        if (!AppTool.IsNullOrEmpty(entityArgs)) {
            this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.Deposit").subscribe((response: any) => {
                    this.LoadBanks();
                });
            });
        }
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new BankAccountToRefundRequestParams();

            this.UIProperties.SetRequired("FileTypeCode", null, true);
            this.UIProperties.SetRequired("FileNumber", null, true);
            this.UIProperties.SetRequired("Numeral", null, true);
            this.UIProperties.SetRequired("IdentifierType", null, true);
            this.UIProperties.SetRequired("IdentifierCode", null, true);
            this.UIProperties.SetRequired("CountryCode", null, true);
            this.UIProperties.SetRequired("BankCode", null, true);
            this.UIProperties.SetRequired("AccountBranch", null, true);
            this.UIProperties.SetRequired("AccountNumber", null, true);
        }
    }

    SetMenuArg(MenuArg) {
        this.OnMassageDisplayMethod();
        this.declarationId = MenuArg.DeclarationId;
    }

    get FileTypeCode() { return this.RequestParams ? this.RequestParams.FileType : null; }
    set FileTypeCode(value: string) {
        if (this.RequestParams.FileType != value) {
            this.RequestParams.FileType = value;
        }
        if (value) {
            this.UIProperties.SetRequired("FileTypeCode", null, false);
        }
        else {
            this.UIProperties.SetRequired("FileTypeCode", null, true);
        }
    }

    get FileNumber() { return this.RequestParams ? this.RequestParams.FileNumber : null; }
    set FileNumber(value: string) {
        if (this.RequestParams.FileNumber != value) {
            this.RequestParams.FileNumber = value;
        }
        if (value) {
            this.UIProperties.SetRequired("FileNumber", null, false);
        }
        else {
            this.UIProperties.SetRequired("FileNumber", null, true);
        }
    }

    get Numeral() { return this.RequestParams ? this.RequestParams.Numeral : null; }
    set Numeral(value: number) {
        if (this.RequestParams.Numeral != value) {
            this.RequestParams.Numeral = value;
        }
        if (value) {
            this.UIProperties.SetRequired("Numeral", null, false);
        }
        else {
            this.UIProperties.SetRequired("Numeral", null, true);
        }
    }

    get IdentifierType() { return this.RequestParams ? this.RequestParams.IdentifierType : null; }
    set IdentifierType(value: string) {
        if (this.RequestParams.IdentifierType != value) {
            this.RequestParams.IdentifierType = value;
        }
        if (value) {
            this.UIProperties.SetRequired("IdentifierType", null, false);
        }
        else {
            this.UIProperties.SetRequired("IdentifierType", null, true);
        }
    }

    get IdentifierCode() { return this.RequestParams ? this.RequestParams.IdentifierCode : null; }
    set IdentifierCode(value: string) {
        if (this.RequestParams.IdentifierCode != value) {
            this.RequestParams.IdentifierCode = value;
        }
        if (value) {
            this.UIProperties.SetRequired("IdentifierCode", null, false);
        }
        else {
            this.UIProperties.SetRequired("IdentifierCode", null, true);
        }
    }

    get CountryCode() { return this.RequestParams ? this.RequestParams.CountryCode : null; }
    set CountryCode(value: string) {
        if (this.RequestParams.CountryCode != value) {
            this.RequestParams.CountryCode = value;
        }
        if (value) {
            this.UIProperties.SetRequired("CountryCode", null, false);
        }
        else {
            this.UIProperties.SetRequired("CountryCode", null, true);
        }
        this.SetBankFieldsEnabled();
    }

    private _InternalBank: string;
    public get InternalBankId() { return this._InternalBank; }
    public set InternalBankId(newValue: string) { this._InternalBank = newValue; }

    get BankCode() { return this.RequestParams ? this.RequestParams.BankCode : null; }
    set BankCode(value: string) {
        if (this.RequestParams.BankCode != value) {
            this.RequestParams.BankCode = value;
        }
        if (value) {
            this.UIProperties.SetRequired("BankCode", null, false);
        }
        else {
            this.UIProperties.SetRequired("BankCode", null, true);
        }
        this.AccountBranch = null;
        this.AccountNumber = null;
    }

    get AccountBranch() { return this.RequestParams ? this.RequestParams.AccountBranch : null; }
    set AccountBranch(value: string) {
        if (this.RequestParams.AccountBranch != value) {
            this.RequestParams.AccountBranch = value;
        }
        if (value) {
            this.UIProperties.SetRequired("AccountBranch", null, false);
        }
        else {
            this.UIProperties.SetRequired("AccountBranch", null, true);
        }
        this.AccountNumber = null;
    }

    get AccountNumber() { return this.RequestParams ? this.RequestParams.AccountNumber : null; }
    set AccountNumber(value: string) {
        if (this.RequestParams.AccountNumber != value) {
            this.RequestParams.AccountNumber = value;
        }
        if (value) {
            this.UIProperties.SetRequired("AccountNumber", null, false);
        }
        else {
            this.UIProperties.SetRequired("AccountNumber", null, true);
        }
    }

    get AccountCurrency() { return this.RequestParams ? this.RequestParams.AccountCurrency : null; }
    set AccountCurrency(value: string) {
        if (this.RequestParams.AccountCurrency != value) {
            this.RequestParams.AccountCurrency = value;
        }
        this.UIProperties.SetRequired("AccountCurrency", null, false);
        if (!AppTool.IsNullOrEmpty(this.CountryCode) && this.CountryCode != "IL") {
            if (value) {
                this.UIProperties.SetRequired("AccountCurrency", null, false);
            }
            else {
                this.UIProperties.SetRequired("AccountCurrency", null, true);
            }
        }
    }

    private _IsraelBankFieldsEnabled: boolean = false;
    public get IsraelBankFieldsEnabled() { return this._IsraelBankFieldsEnabled; }
    public set IsraelBankFieldsEnabled(newValue: boolean) { this._IsraelBankFieldsEnabled = newValue; }

    SetBankFieldsEnabled() {
        if (AppTool.IsNullOrEmpty(this.CountryCode)) {
            this.SetIsraelBankFieldsDisabled();
            return;
        }

        this.InternalBankId = null;
        this.SelectedBankIndex = null;
        this.BankCode = null;
        this.AccountBranch = null;
        this.AccountNumber = null;
        this.AccountCurrency = null;

        switch (this.CountryCode) {
            case "IL": // Israel
                this.SetIsraelBankFieldsEnabled();
                break;
            default: // Foreign
                this.SetIsraelBankFieldsDisabled();
                break;
        }
    }

    SetIsraelBankFieldsDisabled() {
        this.IsraelBankFieldsEnabled = false;
        this.UIProperties.SetEnabled("InternalBankId", null, false);
        this.UIProperties.SetEnabled("AccountCurrency", null, true);
        this.UIProperties.SetRequired("AccountCurrency", null, true);
    }

    SetIsraelBankFieldsEnabled() {
        this.IsraelBankFieldsEnabled = true;
        this.UIProperties.SetEnabled("InternalBankId", null, true);
        this.UIProperties.SetEnabled("AccountCurrency", null, false);
        this.UIProperties.SetRequired("AccountCurrency", null, false);
    }

    _SelectedBankIndex: CustomBankList;
    get SelectedBankIndex() { return this._SelectedBankIndex; }
    set SelectedBankIndex(value: CustomBankList) {
        if (this._SelectedBankIndex != value) {
            this._SelectedBankIndex = value;

            if (value != null) {
                this.BankCode = value.BankCode;
                this.AccountBranch = Number(value.BranchCode).toString() + "," + value.BankCode;
                this.AccountNumber = value.AccountNumber;
            }
            else {
                this.BankCode = null;
                this.AccountBranch = null;
                this.AccountNumber = null;
            }
        }
    }

    LoadBanks() {
        this._CustomBankListService.getAllFromCache().subscribe((response: ServiceResponse) => {
            if (response) {
                if (!response.HasError) {
                    this.banksList = response.Result.filter(d => d.PayerTypeCode == "3" && !d.InActive);
                }
            }
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }

    FillErrors() {
        var errors: string[] = [];
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.RequestParams.FileType)) {
            var msg = TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.FileTypeMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.RequestParams.FileNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.FileNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.RequestParams.IdentifierType)) {
            var msg = TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.IdentifierMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.RequestParams.IdentifierCode)) {
            var msg = TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.IdentifierCodeMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.RequestParams.CountryCode)) {
            var msg = TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.CountryCodeMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.RequestParams.BankCode)) {
            var msg = TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.BankCodeMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.RequestParams.AccountBranch)) {
            var msg = TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.BankBranchMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.RequestParams.AccountNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.AccountNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (!AppTool.IsNullOrEmpty(this.CountryCode) && this.CountryCode != "IL") {
            if (AppTool.IsNullOrEmpty(this.RequestParams.AccountCurrency)) {
                var msg = TextCodeTranslator.Translate("Customs.BankAccountToRefundQuery.O.AccountCurrencyMandatory");
                this.ValidationErrorsList.push(msg);
            }
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        this.CurrentSession.StartBusyIndicator("");

        var currRequestParams = new BankAccountToRefundRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.FileType = this.FileTypeCode;
        currRequestParams.FileNumber = this.FileNumber;
        currRequestParams.Numeral = this.Numeral;
        currRequestParams.IdentifierType = this.IdentifierType;
        currRequestParams.IdentifierCode = this.IdentifierCode;
        currRequestParams.CountryCode = this.CountryCode;
        currRequestParams.BankCode = this.BankCode;
        currRequestParams.AccountBranch = this.AccountBranch;
        currRequestParams.AccountNumber = this.AccountNumber;
        currRequestParams.AccountCurrency = this.AccountCurrency;
        currRequestParams.DeclarationId = this.declarationId;

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
                "שליחת בקשה להחזר פקדון", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._TapagMessagesService.PostBankAccountToRefundQueryRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

    }
}
