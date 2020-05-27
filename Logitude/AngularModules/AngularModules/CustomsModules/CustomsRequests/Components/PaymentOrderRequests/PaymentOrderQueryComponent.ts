import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { PaymentMessagesService } from '../../../../Customs/Services/WebServices/PaymentMessagesService';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { CustomsSettingListService } from '../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { CustomBankListService } from '../../../../Customs/Services/StandardLists/CustomBankListService';
import { PartnersDomainService } from '../../../../Common/Services/PartnersDomainService';
import { PaymentOrderRequestParams } from '../../../../Customs/DataContract/RequestParams/PaymentOrderRequestParams';
import { PaymentOrderResponseData } from '../../../../Customs/DataContract/ResponseData/PaymentOrderResponseData';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ClientList } from '../../../../Customs/EntityLists/ClientList';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';


@Component({
    selector: 'PaymentOrderQueryComponent',
    
    templateUrl: './PaymentOrderQueryComponent.html',
})

export class PaymentOrderQueryComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {

    public DataContext: PaymentOrderQueryComponent = this;
    public ObjectTableName: string = "Customs.Declaration";
    public _ImporterName: string;
    public _IsImporerCodeEnabled: boolean = false;
    _LastFetchDeclarationList: DeclarationList;

    _PaymentMessagesService: PaymentMessagesService = new PaymentMessagesService();
    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _CustomsSettingListService: CustomsSettingListService = new CustomsSettingListService();
    _PartnersDomainService: PartnersDomainService = new PartnersDomainService();
    _CustomBankListService: CustomBankListService = new CustomBankListService();

    public PaymentsDetailsList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.PaymentsDetailsList = new ObservableCollection([]);
    }

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

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new PaymentOrderRequestParams();

            if (this._CustomsSettingListService == null) {
                this._CustomsSettingListService = new CustomsSettingListService();
            }
            this._CustomsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString())
                .subscribe((customsSettingList: any) => {
                    if (customsSettingList) {
                        this.AgentExternalId = customsSettingList.Result ? customsSettingList.Result.CustomsAgentId : null;
                        this.UIProperties.SetEnabled("AgentID", null, false);
                    }
                });
        }

        if (this.ResponseData) {
            if (this.ResponseData.PaymentsDetailsList) {
                this.PaymentsDetailsList.InsertCollection(this.ResponseData.PaymentsDetailsList);
            }
        }
    }

    //#region Properties
    get IsImporerCodeEnabled() { return this._IsImporerCodeEnabled; }
    set IsImporerCodeEnabled(newValue: boolean) {
        if (this._IsImporerCodeEnabled != newValue) {
            this._IsImporerCodeEnabled = newValue;
        }
    }

    get AgentExternalId() { return this.RequestParams.AgentExternalId; }
    set AgentExternalId(value: string) {
        if (this.RequestParams.AgentExternalId != value) {
            this.RequestParams.AgentExternalId = value;
        }
        if (value) {
            this.UIProperties.SetEnabled("AgentID", null, false);
        }
        else {
            this.UIProperties.SetEnabled("AgentID", null, true);
        }
    }

    get AgentID() { return this.RequestParams.AgentID; }
    set AgentID(value: string) {
        if (this.RequestParams.AgentID != value) {
            this.RequestParams.AgentID = value;
        }
        if (value) {
            this.UIProperties.SetEnabled("AgentExternalID", null, false);
        }
        else {
            this.UIProperties.SetEnabled("AgentExternalID", null, true);
        }
    }

    get CustomFileNo() { return this.RequestParams.CustomFileNo; }
    set CustomFileNo(value: string) {
        if (this.RequestParams.CustomFileNo != value) {
            this.RequestParams.CustomFileNo = value;
        }
    }

    get PaymentDateFrom() { return this.RequestParams.PaymentDateFrom; }
    set PaymentDateFrom(value: Date) {
        if (this.RequestParams.PaymentDateFrom != value) {
            this.RequestParams.PaymentDateFrom = value;
        }
    }

    get PaymentMethodType() { return this.RequestParams.PaymentMethodType; }
    set PaymentMethodType(value: string) {
        if (this.RequestParams.PaymentMethodType != value) {
            this.RequestParams.PaymentMethodType = value;
        }
    }

    get ClientId() { return this.RequestParams.ClientId; }
    set ClientId(value: string) {
        if (this.RequestParams.ClientId != value) {
            this.RequestParams.ClientId = value;
        }
        if (value) {
            this._PartnersDomainService.GetCustomerById(value)
                .subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError && myResponse.Result) {
                        this.ImporterCode = myResponse.Result.VatNumber;
                        this.IsImporerCodeEnabled = true;
                    }
                });
        }
        else {
            this.IsImporerCodeEnabled = false;
        }
    }

    get EntityType() { return this.RequestParams.EntityType; }
    set EntityType(value: string) {
        if (this.RequestParams.EntityType != value) {
            this.RequestParams.EntityType = value;
        }
    }

    get PaymentDateTo() { return this.RequestParams.PaymentDateTo; }
    set PaymentDateTo(value: Date) {
        if (this.RequestParams.PaymentDateTo != value) {
            this.RequestParams.PaymentDateTo = value;
        }
    }

    get CustomBankId() { return this.RequestParams.CustomBankId; }
    set CustomBankId(value: string) {
        if (this.RequestParams.CustomBankId != value) {
            this.RequestParams.CustomBankId = value;
        }
        if (value) {
            this._CustomBankListService.getSingleFromCache(value)
                .subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.BankID = myResponse.Result.BankCode;
                        this.BranchID = Number(myResponse.Result.BranchCode).toString() + "," + myResponse.Result.BankCode;
                        this.BankAccount = myResponse.Result.AccountNumber;
                        this.UIProperties.SetEnabled("BankID", "Customs.Bank", false);
                        this.UIProperties.SetEnabled("BranchID", "Customs.CustomsBranch", false);
                        this.UIProperties.SetEnabled("BankAccount", null, false);
                    }
                });
        }
        else {
            this.BankID = null;
            this.BranchID = null;
            this.BankAccount = null;
            this.UIProperties.SetEnabled("BankID", "Customs.Bank", true);
            this.UIProperties.SetEnabled("BranchID", "Customs.CustomsBranch", true);
            this.UIProperties.SetEnabled("BankAccount", null, true);
        }
    }

    get ImporterCode() { return this.RequestParams.ImporterCode; }
    set ImporterCode(value: string) {
        if (this.RequestParams.ImporterCode != value) {
            this.RequestParams.ImporterCode = value;
        }
    }

    get ImporterName() { return this._ImporterName; }
    set ImporterName(value: string) {
        if (this._ImporterName != value) {
            this._ImporterName = value;
        }
    }

    get EntityExternalID() { return this.RequestParams.EntityExternalID; }
    set EntityExternalID(value: string) {
        if (this.RequestParams.EntityExternalID != value) {
            this.RequestParams.EntityExternalID = value;
        }
    }

    get EffectiveDateFrom() { return this.RequestParams.EffectiveDateFrom; }
    set EffectiveDateFrom(value: Date) {
        if (this.RequestParams.EffectiveDateFrom != value) {
            this.RequestParams.EffectiveDateFrom = value;
        }
    }

    get BankID() { return this.RequestParams.BankID; }
    set BankID(value: string) {
        if (this.RequestParams.BankID != value) {
            this.RequestParams.BankID = value;
        }
    }

    get PaymentOrderStatus() { return this.RequestParams.PaymentOrderStatus; }
    set PaymentOrderStatus(value: string) {
        if (this.RequestParams.PaymentOrderStatus != value) {
            this.RequestParams.PaymentOrderStatus = value;
        }
    }

    get PaymentID() { return this.RequestParams.PaymentID; }
    set PaymentID(value: string) {
        if (this.RequestParams.PaymentID != value) {
            this.RequestParams.PaymentID = value;
        }
    }

    get EffectiveDateTo() { return this.RequestParams.EffectiveDateTo; }
    set EffectiveDateTo(value: Date) {
        if (this.RequestParams.EffectiveDateTo != value) {
            this.RequestParams.EffectiveDateTo = value;
        }
    }

    get BranchID() { return this.RequestParams.BranchID; }
    set BranchID(value: string) {
        if (this.RequestParams.BranchID != value) {
            this.RequestParams.BranchID = value;
        }
    }

    get PaymentType() { return this.RequestParams.PaymentType; }
    set PaymentType(value: string) {
        if (this.RequestParams.PaymentType != value) {
            this.RequestParams.PaymentType = value;
        }
    }

    get PaymentProcess() { return this.RequestParams.PaymentProcess; }
    set PaymentProcess(value: string) {
        if (this.RequestParams.PaymentProcess != value) {
            this.RequestParams.PaymentProcess = value;
        }
    }

    get PaymentAmount() { return this.RequestParams.PaymentAmount; }
    set PaymentAmount(value: string) {
        if (this.RequestParams.PaymentAmount != value) {
            this.RequestParams.PaymentAmount = value;
        }
    }

    get BankAccount() { return this.RequestParams.BankAccount; }
    set BankAccount(value: string) {
        if (this.RequestParams.BankAccount != value) {
            this.RequestParams.BankAccount = value;
        }
    }

    //#endregion Properties


    //#region Importer Commands
    ImporterClicked(type, client: ClientList) {
        this.ImporterCode = client.Code;
        this.ImporterName = AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
    }

    ImporterTextChanged(type, item) {

        this.ImporterName = "";
    }
    //#endregion


    //#region Declaration Commands
    DueChangeClearChildField(sourceIsCostomFile: boolean): void {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");

        if (sourceIsCostomFile) {
            this.EntityExternalID = "";
            this.EntityType = "";
        } else {

            this.CustomFileNo = "";
        }
        this._LastFetchDeclarationList = null;
    }

    CustomFileNoTextChanged(searchtext) {

        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.UIProperties.SetEnabled("EntityType", "Customs.EntityTypeLookup", true);
            this.UIProperties.SetEnabled("EntityExternalID", null, true);
            return;
        }
        if (this._LastFetchDeclarationList != null) {
            if (this.CustomFileNo == this._LastFetchDeclarationList.CustomFileNo) {
                return;
            }
        }

        this.DueChangeClearChildField(true);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.FetchDeclaration(myResponse, true);
            });
    }

    FetchDeclaration(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        this._LastFetchDeclarationList = myResponse.Result
        if (this._LastFetchDeclarationList != null) {
            this.EntityType = "1055";
            this.EntityExternalID = this._LastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = this._LastFetchDeclarationList.CustomFileNo;
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetEnabled("EntityType", "Customs.EntityTypeLookup", false);
            this.UIProperties.SetEnabled("EntityExternalID", null, false);

        } else {

            if (sourceIsCostomFile) {
                this.SetValidityCustomFileNo();
            } 

        }
    }

    SetValidityCustomFileNo() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    }

    //#endregion


    //#region General Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.PaymentsDetailsList.Clear();

        var currRequestParams = new PaymentOrderRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.CustomFileNo = this.CustomFileNo;
        currRequestParams.AgentID = this.AgentID;
        currRequestParams.AgentExternalId = this.AgentExternalId;
        currRequestParams.BankAccount = this.BankAccount;
        currRequestParams.BankID = this.BankID;
        currRequestParams.BranchID = this.BranchID;
        currRequestParams.ClientId = this.ClientId;
        currRequestParams.CustomBankId = this.CustomBankId;
        currRequestParams.EffectiveDateFrom = this.EffectiveDateFrom;
        currRequestParams.EffectiveDateTo = this.EffectiveDateTo;
        currRequestParams.EntityExternalID = this.EntityExternalID;
        currRequestParams.EntityType = this.EntityType;
        currRequestParams.ExternalID = this.ImporterCode;
        currRequestParams.PaymentAmount = this.PaymentAmount;
        currRequestParams.paymentDateFrom = this.PaymentDateFrom;
        currRequestParams.paymentDateTo = this.PaymentDateTo;
        currRequestParams.PaymentID = this.PaymentID;
        currRequestParams.PaymentMethodType = this.PaymentMethodType;
        currRequestParams.PaymentOrderStatus = this.PaymentOrderStatus;
        currRequestParams.PaymentProcess = this.PaymentProcess;
        currRequestParams.PaymentType = this.PaymentType;

        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
            "שליחת שאילתא להוראות תשלום", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._PaymentMessagesService.PostPaymentOrderQueryRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }

    //#endregion 
}
