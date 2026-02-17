import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CustomItemLegalDemandsQueryRequestParams } from '../../../Customs/DataContract/RequestParams/CustomItemLegalDemandsQueryRequestParams';//TODO
import { CustomItemLegalDemandsQueryResponseData, CustomItemLegalDemandsQueryResult } from '../../../Customs/DataContract/ResponseData/CustomItemLegalDemandsQueryResponseData';//TODO
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { LuhnAlgorithm } from '../../../Customs/Utilities/LuhnAlgorithm';


@Component({
    selector: 'CustomItemLegalDemands',
    moduleId: module.id,
    templateUrl: './CustomItemLegalDemandsQueryComponent.html',
})


export class CustomItemLegalDemandsQueryComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {
    public DataContext: CustomItemLegalDemandsQueryComponent = this;
    public ObjectTableName: string = "Customs.Declaration";//TODO


    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    //_LastFetchDeclarationList: DeclarationList;
    public CustomItemLegalDemandsQueryObservableList: ObservableCollection;
    public CountriesExclusionListObservableList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.CustomItemLegalDemandsQueryObservableList = new ObservableCollection([]);
        this.CountriesExclusionListObservableList = new ObservableCollection([]);
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
            this.RequestParams = new CustomItemLegalDemandsQueryRequestParams();
            this.ValidToDate = new Date();
            this.UIProperties.SetRequired("CustomsBookType", this.ObjectTableName, true);
            this.UIProperties.SetRequired("ClassificationCode", this.ObjectTableName, true);
        }

        if (this.ResponseData){
            if (this.ResponseData.CustomsLegalDemandsList) {
                this.CustomItemLegalDemandsQueryObservableList.InsertCollection(this.ResponseData.CustomsLegalDemandsList);
            }
            if (this.ResponseData.CountriesExclusionList) {
                this.CountriesExclusionListObservableList.InsertCollection(this.ResponseData.CountriesExclusionList);
            }
        }
    }

    get ValidToDate() { return this.RequestParams.ValidToDate; }
    set ValidToDate(value: Date) {
        if (this.RequestParams.ValidToDate != value) {
            this.RequestParams.ValidToDate = value;
            if (value)
            {
                this.UIProperties.SetRequired("ValidToDate", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("ValidToDate", this.ObjectTableName, true);
            }
        }
    }


    get ClassificationCode() { return this.RequestParams.ClassificationCode; }
    set ClassificationCode(value: string) {
        if (this.RequestParams.ClassificationCode != value) {
            this.RequestParams.ClassificationCode = value;
            if (value) {
                this.UIProperties.SetRequired("ClassificationCode", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("ClassificationCode", this.ObjectTableName, true);
            }

        }
    }

    get CustomsBookTypeName() { return this.RequestParams.CustomsBookTypeName; }
    set CustomsBookTypeName(value: string) {
        if (this.RequestParams.CustomsBookTypeName != value) {
            this.RequestParams.CustomsBookTypeName = value;
        }
    }

    get CustomsBookType() { return this.RequestParams.CustomsBookType; }
    set CustomsBookType(value: string) {
        if (this.RequestParams.CustomsBookType != value) {
            this.RequestParams.CustomsBookType = value;
            if (value) {
                this.UIProperties.SetRequired("CustomsBookType", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("CustomsBookType", this.ObjectTableName, true);
            }
        }
    }

    OnCustomsItemLostFocus(customsItemTextBox: any) {
        //var newValue = this.CustomsItem;
        var newValue = customsItemTextBox.textValue
        var valid = true;
        this.UIProperties.SetValidity("ClassificationCode", this.ObjectTableName, true, "");

        if (AppTool.IsNullOrEmpty(newValue)) {
            return;
        }

        //if (newValue.toString().length == 12) {
        //    if (newValue.toString().substring(10, 11) == "/") {
        //        newValue = newValue.toString().substring(0, 10) + newValue.toString().substring(11);
        //    }
        //}
        if (newValue.toString().length > 11) {
            valid = false;
            this.UIProperties.SetValidity("ClassificationCode", this.ObjectTableName, false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong"));
        }
        else if (newValue.toString().length < 8) {
            valid = false;
            this.UIProperties.SetValidity("ClassificationCode", this.ObjectTableName, false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort"));
        }
        else if (newValue.toString().length == 8) {
            newValue = newValue + "00";
            var checkDigit: number = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + checkDigit;
            valid = true;;
        }
        else if (newValue.toString().length == 9) {
            var digit: string = newValue.toString().substring(8);
            newValue = newValue.toString().substring(0, 8) + "00" + newValue.toString().substring(8);
            var checkDigit: number = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.substring(0, 10));

            if (digit != checkDigit.toString()) {
                valid = false;
                this.UIProperties.SetValidity("ClassificationCode", this.ObjectTableName, false, TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString());
            }
            else {
                valid = true;
            }
        }
        else if (newValue.toString().length == 10) {
            var checkDigit: number = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + "" + checkDigit;
            valid = true;
        }
        else if (newValue.toString().length == 11) {
            var digit: string = newValue.toString().substring(10);
            var checkDigit: number = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.toString().substring(0, 10));
            if (digit != checkDigit.toString()) {
                valid = false;
                this.UIProperties.SetValidity("ClassificationCode", this.ObjectTableName, false, TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString());
            }
            else {
                valid = true;
            }
        }
        else {
            valid = true;
            this.UIProperties.SetValidity("ClassificationCode", this.ObjectTableName, true, "");
        }
        this.ClassificationCode = newValue;
        if (valid) {
            SessionLocator.SustainFocusOnCell = false;
        }
        else {
            SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, LogTextBoxId: customsItemTextBox.InputId });
        }

    }

    FillErrors() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.ClassificationCode)) {
            var msg = TextCodeTranslator.Translate("Customs.CustomItemLegalDemandsQuery.O.ClassificationCodeMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.CustomsBookType)) {
            var msg = TextCodeTranslator.Translate("Customs.CustomItemLegalDemandsQuery.O.CustomsBookTypeMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.ValidToDate)){
            var msg = TextCodeTranslator.Translate("Customs.CustomItemLegalDemandsQuery.O.ValidToDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        //alert(customSendOptionsArgs.Option);
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        this.CustomItemLegalDemandsQueryObservableList.Clear();
        this.CountriesExclusionListObservableList.Clear();

        var currRequestParams = new CustomItemLegalDemandsQueryRequestParams();///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.CustomsBookType = this.CustomsBookType;
        currRequestParams.ClassificationCode = this.ClassificationCode;
        currRequestParams.ValidToDate = this.ValidToDate;

        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, TextCodeTranslator.Translate("Customs.General.O.CustomItemLegalDemandsQuery"), true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._IIGGeneralMessagesService.PostCustomItemLegalDemandsQuery(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }



}
