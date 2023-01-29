import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent } from '../../CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { ExchangeRatesQueryRequestParams } from '../../../Customs/DataContract/RequestParams/ExchangeRatesQueryRequestParams';
import { ExchangeRatesQueryResponseData, ExchangeRatesQueryResult } from '../../../Customs/DataContract/ResponseData/ExchangeRatesQueryResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs, RequestParamsBase } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { CustomMessageProgressComponent } from '../../CustomsControls/Components/CustomMessageProgressComponent';
import { LuhnAlgorithm } from 'Customs/Utilities/LuhnAlgorithm';
import { GITITEMCacheService } from 'Customs/Services/Others/GITITEMCacheService';
import { CustomsItemDetailsQueryRequestParams } from 'Customs/DataContract/RequestParams/CustomsItemDetailsQueryRequestParams';


@Component({
    selector: 'CustomsItemDetailsQueryComponent',

    templateUrl: './CustomsItemDetailsQueryComponent.html',
})


export class CustomsItemDetailsQueryComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {
    public DataContext: CustomsItemDetailsQueryComponent = this;
    public ObjectTableName: string = "Customs.Declaration";//TODO

    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    public ExchangeRatesQueryObservableList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ExchangeRatesQueryObservableList = new ObservableCollection([]);
        this.UIProperties.SetRequired("Classification", "Customs.Classification", true);
        this.RequestParams.CustomsBookType = "1";
        this.RequestParams.ValidToDate = new Date()
    }

    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null" + this.SuperCustomMessageWrapperComponent.MyGuid);

        }

        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();

        ///this.CurrentSession.StartBusyIndicator("Test");
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new ExchangeRatesQueryRequestParams();
            this.UIProperties.SetRequired("ValidToDate", this.ObjectTableName, true);
           

        }

        if (this.ResponseData && this.ResponseData.CurrencyRateList) {
            this.ExchangeRatesQueryObservableList.InsertCollection(this.ResponseData.CurrencyRateList);
        }
    }


    get ValidToDate() {


        return this.RequestParams.ValidToDate;
    }
    set ValidToDate(value: Date) {

        if (this.RequestParams.ValidToDate != value) {
            this.RequestParams.ValidToDate = value;

        }
        if (value) {
            this.UIProperties.SetRequired("ValidToDate", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("ValidToDate", this.ObjectTableName, true);
        }


    }



    get CurrencyTypeCode() { return this.RequestParams.CurrencyTypeCode; }
    set CurrencyTypeCode(value: string) {
        if (this.RequestParams.CurrencyTypeCode != value) {
            this.RequestParams.CurrencyTypeCode = value;
        }
    }
    get CustomsBookType() {
        return this.RequestParams.CustomsBookType;
    }
    set CustomsBookType(value: number) {
        if (this.RequestParams.CustomsBookType != value) {
            this.RequestParams.CustomsBookType = value;
        }
        if (value) {
            this.UIProperties.SetRequired("CustomsBookType", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("CustomsBookType", this.ObjectTableName, true);
        }
    }
    get Classification() {
        return this.RequestParams.Classification;
    }
    set Classification(value: string) {
  
        if (this.RequestParams.Classification != value) {
            this.RequestParams.Classification = value;
        }
        if (value) {
            this.UIProperties.SetRequired("Classification", "Customs.Classification", false);
        }
        else {
            this.UIProperties.SetRequired("Classification", "Customs.Classification", true);
        }
    }

    get CustomsBookTypeName() { return this.ResponseData ? this.ResponseData.CustomsBookTypeName : null; }
    set CustomsBookTypeName(value: string) {
        if (this.ResponseData.CustomsBookTypeName != value) {
            this.ResponseData.CustomsBookTypeName = value;
        }
    }

    get FullClassification() { return this.RequestParams.fullClassification; }
    set FullClassification(value: string) {
        if (this.RequestParams.fullClassification != value) {
            this.RequestParams.fullClassification = value;
        }
    }

    get StatisticMeasurementUnitCode() { return this.RequestParams.statisticMeasurementUnitCode; }
    set StatisticMeasurementUnitCode(value: string) {
        if (this.RequestParams.statisticMeasurementUnitCode != value) {
            this.RequestParams.statisticMeasurementUnitCode = value;
        }
    }

    get GoodsDescription() { return this.RequestParams.GoodsDescription; }
    set GoodsDescription(value: string) {
        if (this.RequestParams.GoodsDescription != value) {
            this.RequestParams.GoodsDescription = value;
        }
    }
    get IsDiscountCode() { return this.RequestParams.IsDiscountCode; }
    set IsDiscountCode(value: string) {
        if (this.RequestParams.IsDiscountCode != value) {
            this.RequestParams.IsDiscountCode = value;
        }
    }



    FillErrors() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.ValidToDate)) {
            var msg = "תאריך שליפה שדה חובה";
            this.ValidationErrorsList.push(msg);
        }
        if (AppTool.IsNullOrEmpty(this.CustomsBookType)) {
            var msg = "סוג ספר מכס שדה חובה"
            this.ValidationErrorsList.push(msg);
        }
        if (AppTool.IsNullOrEmpty(this.Classification)) {
            var msg = "פרט  מכס שדה חובה";
            this.ValidationErrorsList.push(msg);
        }

    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        
        //alert(customSendOptionsArgs.Option);
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        this.ExchangeRatesQueryObservableList.Clear();




        var currRequestParams = new CustomsItemDetailsQueryRequestParams();///Force new GUID On Each Send !!
        currRequestParams.ValidToDate=this.ValidToDate;
        currRequestParams.Classification=this.Classification;
        currRequestParams.CustomsBookType=this.CustomsBookType;

        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        CustomMessageProgressComponent
            //.ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לשערי מטבע", true)
            .ShowProgressBar(this.CurrentSession, currRequestParams.PBId, "שאילתא לנתוני פרט מכס", false)
            .then((res) => {
                this.ResponseData = res;
                this.MyLastCustomsRequestSheetId = currRequestParams.PBId;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._IIGGeneralMessagesService.PostCustomsItemDetailsQuery(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
                //this.CurrentSession.StopBusyIndicator();
                //console.log(myServiceResponse);
                //this.ResponseData = myServiceResponse.Result;
                //this.OnMassageDisplayMethod();
            });
    }

    ClassificationKeyUp(event, logCellTemplate: any, classificationTextBox: any) {
        var key = event.keyCode;
        if (key == 13)
            this.OnClassificationLostFocus(logCellTemplate, classificationTextBox);

    }


    valid: boolean = true;
    checkDigit: number = 0;
    digit: string = null;
    async OnClassificationLostFocus(logCellTemplate: any, classificationTextBox: any) {

        debugger;
       
       
        var newValue = this.Classification;
        this.Classification = newValue;
        this.valid = true;
        this.UIProperties.SetValidity("Classification", "Customs.Classification", true, "");

        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("Classification", "Customs.Classification", true, "");
        }
        else if (newValue.toString().length > 11) {
            this.valid = false;
            this.UIProperties.SetValidity("Classification", "Customs.Classification", false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong"));

        }
        else if (newValue.toString().length < 8) {
            this.valid = false;
            this.UIProperties.SetValidity("Classification", "Customs.Classification", false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort"));

        }
        else if (newValue.toString().length == 8) {
            newValue = newValue + "00";
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + this.checkDigit;
            this.valid = true;;

        }
        else if (newValue.toString().length == 9) {
            this.digit = newValue.toString().substring(8);

            newValue = newValue.toString().substring(0, 8) + "00" + newValue.toString().substring(8);
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.substring(0, 10));

            if (this.digit != this.checkDigit.toString()) {
                this.valid = false;
                this.UIProperties.SetValidity("Classification", "Customs.Classification", false, TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString());

            }
            else {
                this.valid = true;
            }

        }
        else if (newValue.toString().length == 10) {
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + "" + this.checkDigit;
            this.valid = true;

        }
        else if (newValue.toString().length == 11) {
            this.digit = newValue.toString().substring(10);
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.toString().substring(0, 10));
            if (this.digit != this.checkDigit.toString()) {
                this.valid = false;
                this.UIProperties.SetValidity("Classification", "Customs.Classification", false, TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString());

            }
            else {
                this.valid = true;
            }


        }

        else {
            this.valid = true;
            this.UIProperties.SetValidity("Classification", "Customs.Classification", true, "");
        }

        

        if (this.valid) {

        }
        else {
            SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: classificationTextBox.InputId });

        }

    }

}
