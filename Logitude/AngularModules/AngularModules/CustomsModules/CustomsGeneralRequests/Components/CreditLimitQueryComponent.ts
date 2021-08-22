import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CreditQueryRequestParams } from '../../../Customs/DataContract/RequestParams/CreditQueryRequestParams';
import { CreditQueryResponseData } from '../../../Customs/DataContract/ResponseData/CreditQueryResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { ClientList } from '../../../Customs/EntityLists/ClientList';

import { CustomsSettingListService } from '../../../Customs/Services/StandardLists/CustomsSettingListService';


@Component({ 
    selector: 'CreditLimitQueryComponent',
    
    templateUrl: './CreditLimitQueryComponent.html',
})

export class CreditLimitQueryComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {

    public DataContext: CreditLimitQueryComponent = this;
    public ObjectTableName: string = "Customs.Declaration";
    public _ImporterName: string;
    public IsAgentNoDisplayOnly: boolean = true;

    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    _CustomsSettingListService: CustomsSettingListService = new CustomsSettingListService();

    public BalanceDetailsResultList: ObservableCollection;
    public BankAccountsResultList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.BalanceDetailsResultList = new ObservableCollection([]);
        this.BankAccountsResultList = new ObservableCollection([]);
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
            this.RequestParams = new CreditQueryRequestParams();
            this.StartDate = new Date();
            this.EndDate = new Date();

            if (this._CustomsSettingListService == null) {
                this._CustomsSettingListService = new CustomsSettingListService();
            }
            this._CustomsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString())
                .subscribe((customsSettingList: ServiceResponse) => {
                    if (customsSettingList) {
                        this.AgentCode = customsSettingList.Result ? customsSettingList.Result.CustomsAgentId : null;

                    }
                });
        }

        if (this.ResponseData) {
            if (this.ResponseData.BalanceDetailsList) {
                this.BalanceDetailsResultList.InsertCollection(this.ResponseData.BalanceDetailsList);
            }
            if (this.ResponseData.BankAccountsList) {
                this.BankAccountsResultList.InsertCollection(this.ResponseData.BankAccountsList);
            }
        }
    }

    //#region Properties
    get AgentCode() { return this.RequestParams ? this.RequestParams.AgentExternalId : null; }
    set AgentCode(value: string) {
        if (this.RequestParams.AgentExternalId != value) {
            this.RequestParams.AgentExternalId = value;
        }
        if (value) {
            this.UIProperties.SetEnabled("AgentNo", null, false);
        }
        else {
            this.UIProperties.SetEnabled("AgentNo", null, true);
        }
    }

    get AgentNo() { return this.RequestParams ? this.RequestParams.AgentID : null; }
    set AgentNo(value: string) {
        if (this.RequestParams.AgentID != value) {
            this.RequestParams.AgentID = value;
        }
        if (value) {
            this.UIProperties.SetEnabled("AgentCode", null, false);
        }
        else {
            this.UIProperties.SetEnabled("AgentCode", null, true);
        }
    }

    get StartDate() { return this.RequestParams ? this.RequestParams.DateFrom : null; }
    set StartDate(value: Date) {
        if (this.RequestParams.DateFrom != value) {
            this.RequestParams.DateFrom = value;
        }
    }

    get EndDate() { return this.RequestParams ? this.RequestParams.DateTo : null; }
    set EndDate(value: Date) {
        if (this.RequestParams.DateTo != value) {
            this.RequestParams.DateTo = value;
        }
    }

    get ImporterCode() { return this.RequestParams ? this.RequestParams.ExtertnalID : null; }
    set ImporterCode(value: string) {
        if (this.RequestParams.ExtertnalID != value) {
            this.RequestParams.ExtertnalID = value;
        }
    }

    get ImporterName() { return this._ImporterName; }
    set ImporterName(value: string) {
        if (this._ImporterName != value) {
            this._ImporterName = value;
        }
    }
    //#endregion Properties

    //#region Importer Commands
    ImporterClicked(client: ClientList) {
        this.ImporterCode = client.Code;
        this.ImporterName = AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
    }

    ImporterTextChanged(item) {
        if (item == "") {
            this.ImporterCode = null;
            this.ImporterName = "";
        }
    }
    //#endregion

    //#region General Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {

        var currRequestParams = new CreditQueryRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.DateFrom = this.StartDate;
        currRequestParams.DateTo = this.EndDate;
        currRequestParams.AgentID = this.AgentNo;
        currRequestParams.AgentExternalId = this.AgentCode;
        currRequestParams.ExtertnalID = this.ImporterCode;

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
            "שליחת שאילתא לתקרת אשראי", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._IIGGeneralMessagesService.PostCreditQueryRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

    }

    //#endregion Commands
}
