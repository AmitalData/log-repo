import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CustomsBookInRequestParams } from '../../../Customs/DataContract/RequestParams/CustomsBookInRequestParams';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';

import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';

import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { CachedDataManager } from '../../../Infrastructure/Utilities/CachedDataManager';
import { CustomsBookListService } from '../../../Customs/Services/StandardLists/CustomsBookListService';
import { CustomsBookList } from '../../../Customs/EntityLists/CustomsBookList';


@Component({
    selector: 'CustomsBookQueryComponent',
    moduleId: module.id,
    templateUrl: './CustomsBookQueryComponent.html',
})


export class CustomsBookQueryComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {
    public DataContext: CustomsBookQueryComponent = this;
    public ObjectTableName: string = "Customs.Declaration";//TODO

    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    _CustomsBookListService: CustomsBookListService = new CustomsBookListService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

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

        if (this._CustomsBookListService == null) {
            this._CustomsBookListService = new CustomsBookListService();
        }
        this._CustomsBookListService.getAll().subscribe(serviceResponse => {
            if (this.RequestParams == null) {
                this.RequestParams = new CustomsBookInRequestParams();
            }
            let allCustomsBookList: CustomsBookList[] = serviceResponse.Result;
            let tenantCustomsBookList = allCustomsBookList.filter(rec => rec.Tenant == SessionLocator.Tenant)[0];
                
            this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
            this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);

            if (AppTool.IsNullOrEmpty(tenantCustomsBookList)) {
                this.FromDate = DateTool.GetDateByDay(-30);
                this.ToDate = DateTool.GetDateByDay(+0);

            } else {
                this.FromDate = new Date(tenantCustomsBookList.LastUpdateDate.valueOf());
                this.ToDate = DateTool.AddDays(new Date(tenantCustomsBookList.LastUpdateDate.valueOf()),29);
            }   
            this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, false);
        });  
 

        if (this.ResponseData
            //&& this.ResponseData.CurrencyRateList
        ) {
            //this.ExchangeRatesQueryObservableList.InsertCollection(this.ResponseData.CurrencyRateList);
            try {
                this._MyResponseObjectToShow = JSON.parse(this.ResponseData.ResponseStatusXML)
            } catch (err) {
                console.log(err);
            }
        }
    }
    _MyResponseObjectToShow: any = null;
    get FromDate() { return this.RequestParams != null ? this.RequestParams.fromDate : null; }
    set FromDate(value: Date) {
        if (this.RequestParams.fromDate != value) {
            this.RequestParams.fromDate = value;
            if (value) {
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
            }
        }
    }

    get ToDate() { return this.RequestParams != null ? this.RequestParams.toDate : null; }
    set ToDate(value: Date) {
        if (this.RequestParams.toDate != value) {
            this.RequestParams.toDate = value;
            if (value) {
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
            }
        }
    }



    get IsGetHistoricalData() {
        return this.RequestParams.isGetHistoricalData;
    }
    set IsGetHistoricalData(value: boolean) {
        this.RequestParams.isGetHistoricalData = value;

    }





    FillErrors() {
        //var errors: string[] = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        //this.ValidationErrorsList = errors;
        this.ValidationErrorsList = [];

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






        var currRequestParams = new CustomsBookInRequestParams();///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.fromDate = this.FromDate;
        currRequestParams.toDate = this.ToDate;
        currRequestParams.fromDateSpecified = true;
        currRequestParams.toDateSpecified = true;
        currRequestParams.isGetHistoricalData = this.IsGetHistoricalData;


        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "עדכון ספר סיווג", true)
            .then((res) => {
                //this.ResponseData = res;
                
                //this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._IIGGeneralMessagesService.PostCustomsBookInRequestParams(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
                
                //this.CurrentSession.StopBusyIndicator();
                //console.log(myServiceResponse);
                this.ResponseData = myServiceResponse.Result;
                if (this.ResponseData) {
                    if (!this.ResponseData.HasException && this.ResponseData.Succeeded) {
                        CachedDataManager.RefreshTableData("Customs.CustomsItem", true);
                        //return responseData.ResponseStatusXML;
                        //return "ספר סיווג עודכן בהצלחה";;
                    }
                }
                
                this.OnMassageDisplayMethod();
            });
    }



}
