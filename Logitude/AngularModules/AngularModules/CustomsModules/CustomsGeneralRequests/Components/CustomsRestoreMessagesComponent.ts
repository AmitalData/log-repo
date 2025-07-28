import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { MessageRestoreRequestParams, MessageWaitingRequestParams } from '../../../Customs/DataContract/RequestParams/MessageRestoreRequestParams';
import { ExchangeRatesQueryResponseData, ExchangeRatesQueryResult } from '../../../Customs/DataContract/ResponseData/ExchangeRatesQueryResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs, SendRequestVIA, TestCase } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { InterfaceManagementList } from '../../../Customs/EntityLists/InterfaceManagementList';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';


@Component({
    selector: 'CustomsRestoreMessagesComponent',

    templateUrl: './CustomsRestoreMessagesComponent.html',
})


export class CustomsRestoreMessagesComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {
    public DataContext: CustomsRestoreMessagesComponent = this;
    public ObjectTableName: string = "Customs.Declaration";//TODO


    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    //_LastFetchDeclarationList: DeclarationList;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    ResponseStatusXML: string = "";
    _isVisible: boolean = false;

    private _FromDateTime: Date;
    private _ToDateTime: Date;
    private _TodayDate: Date;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this._entityResourceService.getEntityResourceByTableName("Customs.CustomsExchangeRate", 0).subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("Customs.Declaration", 0).subscribe((response: any) => {
                this._isVisible = true;
                this._TodayDate = DateTool.GetCurrentDateTimeAsUtc();
            });
        });
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
            this.RequestParams = new MessageRestoreRequestParams();
            //this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
        }

        if (this.ResponseData) {
            if (this.ResponseData != null) {
                if (!this.ResponseData.HasException && this.ResponseData.Succeeded) {
                    this.ResponseStatusXML = TextCodeTranslator.Translate("Customs.Declaration.O.SentToDCA");//, TenantContext.Current.Id);
                    //נשלח למכס בהצלחה , משוב יתקבל בכספת
                }
                else {
                    //message = responseData.UserMessage;
                }
            }
            else {
                //message = "Service returned a null response!";
            }
        }
    }
    CorrelationNoTextChanged(ev) {
        let pattren = new RegExp('^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i');
        if (pattren.test(ev)) {

        } else {
            //this.UIProperties.SetValidity("CorrelationNo", this.ObjectTableName, false,"GUID");
        }

    }

    get CorrelationNo() { return this.RequestParams.CorrelationID; }
    set CorrelationNo(value: string) {
        if (this.RequestParams.CorrelationID != value) {
            this.RequestParams.CorrelationID = value;
            if (!AppTool.IsNullOrEmpty(value)) {
                this.UIProperties.SetRequired("CorrelationNo", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("CorrelationNo", this.ObjectTableName, true);
            }
        }
    }

    get FromDate() { return this.RequestParams.FromDate; }
    set FromDate(value: Date) {
        if (this.RequestParams.FromDate != value) {
            this.RequestParams.FromDate = value;
            this.FromDateLostFocusMethod(value);
            if (value && !AppTool.IsNullOrEmpty(this.FromDateTime)) {
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
            }
        }
    }

    FromDateLostFocusMethod(startDateItem: Date) {

        this.ValidationErrorsList = [];
        this.FromDateTime = null;

        if (AppTool.IsNullOrEmpty(startDateItem)) {
            return;
        }

        if (!AppTool.IsNullOrEmpty(startDateItem) && startDateItem > this.ToDate) {
            var msg = "תאריך התחלה לא יכול להיות אחרי תאריך סיום";
            this.ValidationErrorsList.push(msg);
        }

        if (startDateItem.getDate() == this._TodayDate.getDate()) {
            var date: Date = new Date();
            date.setMinutes(0);
            this.FromDateTime = DateTool.AddHour(date, 3);
        }
    }

    get FromDateTime() { return this._FromDateTime; }
    set FromDateTime(value: Date) {
        if (this._FromDateTime != value) {
            this._FromDateTime = value;
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
            this.ToDateLostFocusMethod(value);
            if (value && !AppTool.IsNullOrEmpty(this.ToDateTime)) {
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
            }
        }
    }

    ToDateLostFocusMethod(endDateItem: Date) {
        this.ValidationErrorsList = [];
        this.ToDateTime = null;

        if (AppTool.IsNullOrEmpty(endDateItem)) {
            return;
        }

        if (!AppTool.IsNullOrEmpty(this.FromDate) && this.FromDate > endDateItem) {
            var msg = "תאריך התחלה לא יכול להיות אחרי תאריך סיום";
            this.ValidationErrorsList.push(msg);
        }

        if (endDateItem.getDate() == this._TodayDate.getDate()) {
            var date: Date = new Date();
            date.setMinutes(0);
            this.ToDateTime = DateTool.AddHour(date, 3);
        }
    }

    get ToDateTime() { return this._ToDateTime; }
    set ToDateTime(value: Date) {
        if (this._ToDateTime != value) {
            this._ToDateTime = value;
            if (value) {
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
            }
        }
    }

    _InterfaceManagementsCode: string
    get InterfaceManagementsCode() { return this._InterfaceManagementsCode; }
    set InterfaceManagementsCode(value: string) {
        if (this._InterfaceManagementsCode != value) {
            this._InterfaceManagementsCode = value;
        }
    }

    _InterfaceManagementSelectedItem: InterfaceManagementList;

    get InterfaceManagementSelectedItem() { return this._InterfaceManagementSelectedItem; }
    set InterfaceManagementSelectedItem(value: InterfaceManagementList) {
        this._InterfaceManagementSelectedItem = value;
    }

    _IsRestoreByDates: boolean = false;
    get IsRestoreByDates() { return this._IsRestoreByDates; }
    set IsRestoreByDates(value: boolean) {
        if (this._IsRestoreByDates == value) {
            return;
        }
        this._IsRestoreByDates = value;
        if (!this._IsRestoreByDates) {
            this._IsRestoreByCorrelation = true;
        } else {
            this._IsRestoreByCorrelation = false;
        }
        this.ClearData();
    }


    _IsRestoreByCorrelation: boolean = true;
    get IsRestoreByCorrelation() { return this._IsRestoreByCorrelation; }
    set IsRestoreByCorrelation(value: boolean) {
        if (this._IsRestoreByCorrelation == value) {
            return;
        }
        this._IsRestoreByCorrelation = value;
        if (!this._IsRestoreByCorrelation) {
            this._IsRestoreByDates = true;
        } else {
            this._IsRestoreByDates = false;
        }
        this.ClearData();
    }
    ClearData() {
        this.InterfaceManagementsCode = null;
        this.ToDate = this.FromDate = null;

    }

    FillErrors() {
        //var errors: string[] = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        //this.ValidationErrorsList = errors;
        if (this.IsRestoreByCorrelation) {

            if (AppTool.IsNullOrEmpty(this.CorrelationNo)) {

                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Declaration.O.CorrelationNumberIsMandatory"));
            }
        } else {

            let LoggedUserPMCode = SessionLocator.LoggedUserPM.Code || "";
            LoggedUserPMCode = LoggedUserPMCode.toLowerCase();
            if (LoggedUserPMCode == "amital" || LoggedUserPMCode.startsWith("amital.")) {
                console.warn("User Amital* suppress check InterfaceManagements");
            }
            else {
                if (AppTool.IsNullOrEmpty(this.InterfaceManagementsCode)) {
                    var msg = TextCodeTranslator.Translate("Customs.Declaration.O.InterfaceManagementsCodeIsMandatory");
                    this.ValidationErrorsList.push(msg);
                }

                if (!AppTool.IsNullOrEmpty(this.InterfaceManagementSelectedItem)) {
                    if (AppTool.IsNullOrEmpty(this._InterfaceManagementSelectedItem.DcaPrefixName)) {
                        var msg = "No Dca";
                        this.ValidationErrorsList.push(msg);
                    }
                }
            }

            if (AppTool.IsNullOrEmpty(this.FromDate) || AppTool.IsNullOrEmpty(this.FromDateTime)) {
                var msg = TextCodeTranslator.Translate("Customs.ExchangeRate.O.FromDateMandatory");
                if (AppTool.IsNullOrEmpty(this.FromDateTime)) {
                    msg = msg + " (כולל שעה)";
                }
                this.ValidationErrorsList.push(msg);
            }


            if (AppTool.IsNullOrEmpty(this.ToDate) || AppTool.IsNullOrEmpty(this.ToDateTime)) {
                var msg = TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
                if (AppTool.IsNullOrEmpty(this.ToDateTime)) {
                    msg = msg + " (כולל שעה)";
                }
                this.ValidationErrorsList.push(msg);
            }
            //if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.ToDate)) {
            //    if (DateTool.GetDateFromDate(this.FromDate) > DateTool.GetDateFromDate(this.ToDate)) {
            //        var msg = TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
            //        this.ValidationErrorsList.push(msg);
            //    }
            //}
        }


    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        //alert(customSendOptionsArgs.Option);
        this.ValidationErrorsList = [];
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        let dcaPrefix = "";
        let interfaceManagementsCodeValue = "";
        if (!AppTool.IsNullOrEmpty(this.InterfaceManagementsCode)) {
            if (this._InterfaceManagementSelectedItem.DcaPrefixName.endsWith("_Out.")) {
                dcaPrefix = this._InterfaceManagementSelectedItem.DcaPrefixName.substring(0, this._InterfaceManagementSelectedItem.DcaPrefixName.length - 5);
            }
            else {
                dcaPrefix = this._InterfaceManagementSelectedItem.DcaPrefixName;
            }
            interfaceManagementsCodeValue = this._InterfaceManagementSelectedItem.Code;
        }


        this.PostMessageWaitingRequestParams(dcaPrefix, interfaceManagementsCodeValue, customSendOptionsArgs);
        //this.PostMessageRestoreRequestParams(dcaPrefix, interfaceManagementsCodeValue, customSendOptionsArgs);
    }


    private PostMessageWaitingRequestParams(dcaPrefix: string, interfaceManagementsCodeValue: string, customSendOptionsArgs: CustomSendOptionsArgs) {
        var currRequestParams = new MessageWaitingRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        if (AppTool.IsNullOrEmpty(this.CorrelationNo)) {
            currRequestParams.CorrelationID = "";
        } else {
            currRequestParams.CorrelationID = this.CorrelationNo;
        }
        currRequestParams.ServiceName = dcaPrefix;
        currRequestParams.ServiceNameCode = this.InterfaceManagementsCode;
        currRequestParams.FromDate = this.FromDate;
        if (this.FromDateTime != null) {
            var fromDate = this.FromDate;
            fromDate.setHours(this.FromDateTime.getHours());
            fromDate.setMinutes(this.FromDateTime.getMinutes());
            currRequestParams.FromDate = fromDate;
        }
        currRequestParams.ToDate = this.ToDate;
        if (this.ToDateTime != null) {
            var toDate = this.ToDate;
            toDate.setHours(this.ToDateTime.getHours());
            toDate.setMinutes(this.ToDateTime.getMinutes());
            currRequestParams.ToDate = toDate;
        }
        currRequestParams.RequestVIA = SendRequestVIA.WebServiceBatch;//must 
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        if (customSendOptionsArgs.TestCase) {
            const logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 400;
            logWindow.Title = "תרחשי 9100";
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = { SincroScreen: "SincroSend9100" };

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(result => {
                    if (!AppTool.IsNullOrEmpty(result) && result === "Ok") {

                        const tc = new TestCase();
                        tc.Code = comp._ScenarioCode;
                        tc.Param1 = comp.Param1;
                        tc.Param2 = comp.Param2;
                        currRequestParams.TestCase = tc;
                        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;

                        this._IIGGeneralMessagesService.PostMessageWaitingRequestParams(currRequestParams)
                            .subscribe(
                                (myServiceResponse: ServiceResponse) => { },
                                (err) => this.ValidationErrorsList.push(err),
                                () => this.CurrentSession.StopBusyIndicator()
                            );
                    }
                });
            });

            logWindow.Show('./CustomsModules/CustomsControls/Components/TestCase/SendDeclarationTastCaseComponent');
            return;
        }



        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession, currRequestParams.PBId,
                "שליחת שאילתא לשיחזור מסרים",
                false)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });
        ////
        this._IIGGeneralMessagesService.PostMessageWaitingRequestParams(currRequestParams)
            .subscribe(
                (myServiceResponse: ServiceResponse) => { },
                (err) => this.ValidationErrorsList.push(err),
                () => this.CurrentSession.StopBusyIndicator()
            );
    }

    private PostMessageRestoreRequestParams(dcaPrefix: string, interfaceManagementsCodeValue: string, customSendOptionsArgs: CustomSendOptionsArgs) {
        var currRequestParams = new MessageRestoreRequestParams(); ///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        if (AppTool.IsNullOrEmpty(this.CorrelationNo)) {
            currRequestParams.CorrelationID = "";
        } else {
            currRequestParams.CorrelationID = this.CorrelationNo;
        }
        currRequestParams.InterfaceManagementsCode = dcaPrefix;
        currRequestParams.InterfaceManagementsCodeValue = interfaceManagementsCodeValue;
        currRequestParams.FromDate = this.FromDate;
        if (this.FromDateTime != null) {
            var fromDate = this.FromDate;
            fromDate.setHours(this.FromDateTime.getHours());
            fromDate.setMinutes(this.FromDateTime.getMinutes());
            currRequestParams.FromDate = fromDate;
        }
        currRequestParams.ToDate = this.ToDate;
        if (this.ToDateTime != null) {
            var toDate = this.ToDate;
            toDate.setHours(this.ToDateTime.getHours());
            toDate.setMinutes(this.ToDateTime.getMinutes());
            currRequestParams.ToDate = toDate;
        }
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession, currRequestParams.PBId,
                "שליחת שאילתא לשיחזור מסרים",
                false)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });
        ////
        this._IIGGeneralMessagesService.PostMessageRestoreRequestParams(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }
}
