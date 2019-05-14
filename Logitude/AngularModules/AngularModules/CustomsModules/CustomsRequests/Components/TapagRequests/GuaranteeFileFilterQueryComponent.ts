import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TapagMessagesService } from '../../../../Customs/Services/WebServices/TapagMessagesService'; 
import { GuaranteeFileFilterRequestParams } from '../../../../Customs/DataContract/RequestParams/GuaranteeFileFilterRequestParams';
import { GuaranteeFileFilterResponseData } from '../../../../Customs/DataContract/ResponseData/GuaranteeFileFilterResponseData';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';

import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
@Component({
    selector: 'GuaranteeFileFilterQueryComponent',
    moduleId: module.id,
    templateUrl: './GuaranteeFileFilterQueryComponent.html',
})

export class GuaranteeFileFilterQueryComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent { 

    public DataContext: GuaranteeFileFilterQueryComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _TapagMessagesService: TapagMessagesService = new TapagMessagesService();

    public GuaranteeLettersList: ObservableCollection;
    public CreditTransactionsList: ObservableCollection;
    public RequireDocumentsList: ObservableCollection;
    public GuranteeTypeFilterList: CodeNameClass[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.GuaranteeLettersList = new ObservableCollection([]);
        this.CreditTransactionsList = new ObservableCollection([]);
        this.RequireDocumentsList = new ObservableCollection([]);
        this.BuildGuranteeTypeGroupFilterList();
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
            this.RequestParams = new GuaranteeFileFilterRequestParams();
            this.UIProperties.SetRequired("GuranteeType", null, true);
            this.UIProperties.SetRequired("FileNumber", null, true);
            this.UIProperties.SetRequired("Numeral", null, true);
        }

        if (this.ResponseData) {
            if (this.ResponseData.GuaranteeLettersList) {
                this.GuaranteeLettersList.InsertCollection(this.ResponseData.GuaranteeLettersList); 
            }

            if (this.ResponseData.CreditTransactionsList) {
                this.CreditTransactionsList.InsertCollection(this.ResponseData.CreditTransactionsList); 
            }

            if (this.ResponseData.RequireDocumentsList) {
                this.RequireDocumentsList.InsertCollection(this.ResponseData.RequireDocumentsList);
            }
        }
        else {
            this.ResponseData = new GuaranteeFileFilterResponseData();
        }
    }

    //#region Properties
    get GuranteeType() { return this.RequestParams.GuranteeType; }
    set GuranteeType(value: string) {
        if (this.RequestParams.GuranteeType != value) {
            this.RequestParams.GuranteeType = value;
        }
        if (value) {
            this.UIProperties.SetRequired("GuranteeType", null, false);
        }
        else {
            this.UIProperties.SetRequired("GuranteeType", null, true);
        }
    }

    get FileNumber() { return this.RequestParams.FileNumber; }
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

    get Numeral() { return this.RequestParams.Numeral; }
    set Numeral(value: string) {
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
    //#endregion Properties

    //#region Response Properties
    get DisplayFileNumber() { return this.ResponseData.DisplayFileNumber; }
    set DisplayFileNumber(value: string) {
        if (this.ResponseData.DisplayFileNumber != value) {
            this.ResponseData.DisplayFileNumber = value;
        }
    }

    get CustomOfficeName() { return this.ResponseData.CustomOfficeName; }
    set CustomOfficeName(value: string) {
        if (this.ResponseData.CustomOfficeName != value) {
            this.ResponseData.CustomOfficeName = value;
        }
    }

    get CreditLimit() { return this.ResponseData.CreditLimit; }
    set CreditLimit(value: string) {
        if (this.ResponseData.CreditLimit != value) {
            this.ResponseData.CreditLimit = value;
        }
    }

    get StatusName() { return this.ResponseData.StatusName; }
    set StatusName(value: string) {
        if (this.ResponseData.StatusName != value) {
            this.ResponseData.StatusName = value;
        }
    }

    get EntityTypeName() { return this.ResponseData.EntityTypeName; }
    set EntityTypeName(value: string) {
        if (this.ResponseData.EntityTypeName != value) {
            this.ResponseData.EntityTypeName = value;
        }
    }

    get CreditBalance() { return this.ResponseData.CreditBalance; }
    set CreditBalance(value: string) {
        if (this.ResponseData.CreditBalance != value) {
            this.ResponseData.CreditBalance = value;
        }
    }

    get GuaranteedName() { return this.ResponseData.GuaranteedName; }
    set GuaranteedName(value: string) {
        if (this.ResponseData.GuaranteedName != value) {
            this.ResponseData.GuaranteedName = value;
        }
    }

    get EntityNumber() { return this.ResponseData.EntityNumber; }
    set EntityNumber(value: string) {
        if (this.ResponseData.EntityNumber != value) {
            this.ResponseData.EntityNumber = value;
        }
    }

    get GuaranteeExecutedAmountAdjusted() { return this.ResponseData.GuaranteeExecutedAmountAdjusted; }
    set GuaranteeExecutedAmountAdjusted(value: string) {
        if (this.ResponseData.GuaranteeExecutedAmountAdjusted != value) {
            this.ResponseData.GuaranteeExecutedAmountAdjusted = value;
        }
    }

    get AgentName() { return this.ResponseData.AgentName; }
    set AgentName(value: string) {
        if (this.ResponseData.AgentName != value) {
            this.ResponseData.AgentName = value;
        }
    }

    get Validity() { return this.ResponseData.Validity; }
    set Validity(value: string) {
        if (this.ResponseData.Validity != value) {
            this.ResponseData.Validity = value;
        }
    }

    get GuaranteeAmount() { return this.ResponseData.GuaranteeAmount; }
    set GuaranteeAmount(value: string) {
        if (this.ResponseData.GuaranteeAmount != value) {
            this.ResponseData.GuaranteeAmount = value;
        }
    }

    //#endregion Properties


    //#region Gurantee Type Group
    private BuildGuranteeTypeGroupFilterList() {
        this.GuranteeTypeFilterList = [];

        var myGuranteeType: CodeNameClass = new CodeNameClass();
        myGuranteeType.Code = "4"; // "Gurantee Type"
        myGuranteeType.Name = TextCodeTranslator.Translate("Customs.GuaranteeFileFilterQuery.F.GuranteeType.File");;
        this.GuranteeTypeFilterList.push(myGuranteeType);

        var myGuranteeRequest: CodeNameClass = new CodeNameClass();
        myGuranteeRequest.Code = "6"; // "Gurantee Request"
        myGuranteeRequest.Name = TextCodeTranslator.Translate("Customs.GuaranteeFileFilterQuery.F.GuranteeType.Req");;
        this.GuranteeTypeFilterList.push(myGuranteeRequest);

        if (this.RequestParams != null && !AppTool.IsNullOrEmpty(this.RequestParams.GuranteeType)) {
            if (this.RequestParams.GuranteeType == "4") {
                this.SelectedGuranteeTypeFilter = myGuranteeType;
            }
            else if (this.RequestParams.GuranteeType == "6") {
                this.SelectedGuranteeTypeFilter = myGuranteeRequest;
            }
        }
    }

    private selectedGuranteeTypeFilter: CodeNameClass = new CodeNameClass();
    get SelectedGuranteeTypeFilter() {
        return this.selectedGuranteeTypeFilter;
    }
    set SelectedGuranteeTypeFilter(newValue: CodeNameClass) {
        if (this.selectedGuranteeTypeFilter != newValue) {
            this.selectedGuranteeTypeFilter = newValue;
            this.GuranteeType = newValue.Code;
        }
        if (newValue) {
            this.UIProperties.SetRequired("GuranteeType", null, false);
        }
        else {
            this.UIProperties.SetRequired("GuranteeType", null, true);
        }
    }
    //#endregion


    //#region Commands

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.RequestParams.GuranteeType)) {
            var msg = TextCodeTranslator.Translate("Customs.GuaranteeFileFilterQuery.O.GuaranteeType");
            this.ValidationErrorsList.push(msg);
        }
        if (AppTool.IsNullOrEmpty(this.RequestParams.FileNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.GuaranteeFileFilterQuery.O.FileNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.RequestParams.Numeral)) {
            var msg = TextCodeTranslator.Translate("Customs.GuaranteeFileFilterQuery.O.NumeralMandatory");
            this.ValidationErrorsList.push(msg);
        }

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        this.GuaranteeLettersList.Clear();
        this.CreditTransactionsList.Clear();
        this.RequireDocumentsList.Clear();

        var currRequestParams = new GuaranteeFileFilterRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.GuranteeType = this.GuranteeType;
        currRequestParams.FileNumber = this.FileNumber;
        currRequestParams.Numeral = this.Numeral;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;



        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
            "שליחת שאילתא לערבויות", true)
            .then((res) => {
                this.ResponseData = res;
                this.MyLastCustomsRequestSheetId = currRequestParams.PBId;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._TapagMessagesService.PostGuaranteeFileFilterQueryRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }

    //#endregion Commands
}


class CodeNameClass {
    public Code: string
    public Name: string
}
