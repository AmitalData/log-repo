import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { ExchangeRatesQueryRequestParams } from '../../../Customs/DataContract/RequestParams/ExchangeRatesQueryRequestParams';
import { ExchangeRatesQueryResponseData, ExchangeRatesQueryResult } from '../../../Customs/DataContract/ResponseData/ExchangeRatesQueryResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';


@Component({
    selector: 'ExchangeRatesQueryComponent',
    
    templateUrl: './ExchangeRatesQueryComponent.html',
})


export class ExchangeRatesQueryComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {
    public DataContext: ExchangeRatesQueryComponent = this;
    public ObjectTableName: string = "Customs.Declaration";//TODO

    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    public ExchangeRatesQueryObservableList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ExchangeRatesQueryObservableList = new ObservableCollection([]);
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
        this.subscribeWrapperComponent()
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new ExchangeRatesQueryRequestParams();
            this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
            this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);

        }

        if (this.ResponseData && this.ResponseData.CurrencyRateList) {
            this.ExchangeRatesQueryObservableList.InsertCollection(this.ResponseData.CurrencyRateList);
        }
    }


    get FromDate() { return this.RequestParams.FromDate; }
    set FromDate(value: Date) {
        if (this.RequestParams.FromDate != value) {
            this.RequestParams.FromDate = value;
            if (value) {
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
            }
        }
    }

    get ToDate() { return this.RequestParams.ToDate; }
    set ToDate(value: Date) {
        if (this.RequestParams.ToDate != value) {
            this.RequestParams.ToDate = value;
            if (value) {
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
            }
        }
    }

    get CurrencyTypeCode() { return this.RequestParams.CurrencyTypeCode; }
    set CurrencyTypeCode(value: string) {
        if (this.RequestParams.CurrencyTypeCode != value) {
            this.RequestParams.CurrencyTypeCode = value;
        }
    }

    get CurrencyTypeName() { return this.RequestParams.CurrencyTypeName; }
    set CurrencyTypeName(value: string) {
        if (this.RequestParams.CurrencyTypeName != value) {
            this.RequestParams.CurrencyTypeName = value;
        }
    }

    get CustomsCurrencyRate() { return this.RequestParams.CustomsCurrencyRate; }
    set CustomsCurrencyRate(value: string) {
        if (this.RequestParams.CustomsCurrencyRate != value) {
            this.RequestParams.CustomsCurrencyRate = value;
        }
    }

    get StartDate() { return this.RequestParams.StartDate; }
    set StartDate(value: string) {
        if (this.RequestParams.StartDate != value) {
            this.RequestParams.StartDate = value;
        }
    }

    FillErrors() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.FromDate)) {
            var msg = TextCodeTranslator.Translate("Customs.ExchangeRate.O.FromDateMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.ToDate)) {
            var msg = TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
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

        


        var currRequestParams = new ExchangeRatesQueryRequestParams();///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.FromDate = this.FromDate;
        currRequestParams.ToDate = this.ToDate;
        currRequestParams.CurrencyTypeId = this.CurrencyTypeCode;

        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לשערי מטבע", true)
            .then((res) => {
                this.ResponseData = res;
                this.MyLastCustomsRequestSheetId = currRequestParams.PBId;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._IIGGeneralMessagesService.PostExchangeRatesQuery(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
                //this.CurrentSession.StopBusyIndicator();
                //console.log(myServiceResponse);
                //this.ResponseData = myServiceResponse.Result;
                //this.OnMassageDisplayMethod();
            });
    }

    _LOVListCurrencys: any[] = [];
    get LOVListCurrencys() { return this._LOVListCurrencys; }
    set LOVListCurrencys(value) {
        if (this._LOVListCurrencys != value) {
            this._LOVListCurrencys = value;
        }
    }
    
}
