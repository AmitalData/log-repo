import { Component, AfterViewInit, ChangeDetectorRef, ViewChildren, QueryList, OnInit, ViewEncapsulation } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { LocationDirective } from '../../../../../Infrastructure/Utilities/LocationDirective';
import { ApiQueryFilters, FilterItem } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool, ArrayTool, DateTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
declare var window: any;
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';

import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { DeclarationPaymentMethodPM } from '../../../../../Customs/EntityPMs/DeclarationPaymentMethodPM';
import { DeclarationPaymentProtestPM } from '../../../../../Customs/EntityPMs/DeclarationPaymentProtestPM';
import { DeclarationPaymentPM } from '../../../../../Customs/EntityPMs/DeclarationPaymentPM';

import { DeclarationEventManager } from '../../../../../Customs/Utilities/DeclarationEventManager';
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';

import { CustomBanksCardPM } from '../../../../../Customs/EntityPMs/CustomBanksCardPM';
import { CustomerActivityTypePM } from '../../../../../Customs/EntityPMs/CustomerActivityTypePM';
import { UserList } from '../../../../../Common/EntityLists/UserList';
import { CustomBankList } from '../../../../../Customs/EntityLists/CustomBankList';
import { AmitalGatewayUtil } from '../../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { UnifreightController } from '../../../../../Customs/Controller/UnifreightController';
import { CustomMessageProgressHelper, CustomMessageProgressComponent, ShowProgressBarParams } from '../../../../CustomsControls/Components/CustomMessageProgressComponent';

import { ObjectTablePM } from '../../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { CustomFileCreditRequestParams } from '../../../../../Customs/DataContract/RequestParams/CustomFileCreditRequestParams';
import { CustomFileCreditResponseData } from '../../../../../Customs/DataContract/ResponseData/CustomFileCreditResponseData';
import { INF_MSG_GenericResponseData } from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { CustomSendOptionsArgs, SendRequestVIA, TestCase } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomsRequiredFieldErrors } from '../../../../../Customs/DataContract/CustomsRequiredFieldErrors';
import { CustomsRequiredFieldListService } from '../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService';


// Services
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { UserListService } from '../../../../../Common/Services/StandardLists/UserListService';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { CustomBankListService } from '../../../../../Customs/Services/StandardLists/CustomBankListService';
import { CustomBankCardExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/CustomBankCardExtendedPMService';
import { PaymentMethodTypeListService } from '../../../../../Customs/Services/StandardLists/PaymentMethodTypeListService';
import { CustomerActivityTypeListService } from '../../../../../Customs/Services/StandardLists/CustomerActivityTypeListService';
import { IIGGeneralMessagesService } from '../../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { DeclarationMessagesService } from '../../../../../Customs/Services/WebServices/DeclarationMessagesService';
import { DeclarationPaymentPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPaymentPMService';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { CustomsSettingExtendedListService } from '../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { ErrorLogPMFileLoggerService } from '../../../../../Infrastructure/Services/ExtendedPMs/ErrorLogPMFileLoggerService';
import { ErrorLogPM } from '../../../../../Infrastructure/EntityPMs/ErrorLogPM';
import { DateTimeFormat } from '../../../../../Infrastructure/Utilities/DateTimeZone';
import { DeclarationCourierStatusList } from '../../../../../Customs/EntityLists/DeclarationCourierStatusList';
import { DeclarationCourierStatusListService } from '../../../../../Customs/Services/StandardLists/DeclarationCourierStatusListService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { Observable } from 'rxjs';
import { SupplierInvoiceExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService';
import { CustomsRequiredFieldExtendedListService } from '../../../../../Customs/Services/ExtendedLists/CustomsRequiredFieldExtendedListService';
@Component({

    templateUrl: './DeclarationPaymentComponent.html',
    providers: [DeclarationExtendedListService]
})

export class DeclarationPaymentComponent extends BaseComponent implements OnInit {
    public PayerActivityTypeCode: any;

    public DataContext: any = this;
    public DeclarationPM: DeclarationPM;
    public paymentPM: DeclarationPaymentPM;
    public ObjectTableName: string = "Customs.DeclarationPayment"; //Customs.Declaration";
    ValidationErrorsList: any[] = [];

    ErrorMessage: string = "";
    entityCreated: boolean = false;
    _PayWithProtest_Default: string;

    //Services
    declarationWebService: DeclarationWebService = new DeclarationWebService();
    declarationService: DeclarationPMService = new DeclarationPMService();
    declarationPaymentPMService: DeclarationPaymentPMService = new DeclarationPaymentPMService();
    entityResourceService: EntityResourceService = new EntityResourceService();
    userListService: UserListService = new UserListService();
    customsSettingListService: CustomsSettingListService = new CustomsSettingListService;
    paymentMethodTypeListService: PaymentMethodTypeListService = new PaymentMethodTypeListService();
    customerActivityTypeListService: CustomerActivityTypeListService = new CustomerActivityTypeListService();
    declarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();
    _CustomsSettingExtendedListService: CustomsSettingExtendedListService = new CustomsSettingExtendedListService();
    supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();
    PaymentMethodsList: ObservableCollection;
    PaymentProtestsList: ObservableCollection;
    SelectedInvoiceItems: ObservableCollection;
    SelectedInvoices: ObservableCollection;
    IsDisplayOnlyAutomaticPayment: boolean;
    BetweenMinAndMax: boolean=false;
    sumBtl: any = 0.0;
    _ErrorLogPMFileLoggerService: ErrorLogPMFileLoggerService;
    _2LogBankList: boolean = false;
    IsDisplayMessage: boolean;
    DisplayAutomaticPayment: boolean = true;
    ClientBankListLogUntilDateyyyyMMdd = "20180820.ClientBankListLogUntilDateyyyyMMdd";
    _CourierWorksheet: DeclarationCourierStatusList;
    _TestCase: TestCase;
    IsAutomaticPayment: boolean;
    constructor(public declarationExtendedListService: DeclarationExtendedListService) {
        super();
        this.PaymentMethodsList = new ObservableCollection([]);
        this.PaymentProtestsList = new ObservableCollection([]);
        this.SelectedInvoiceItems = new ObservableCollection([]);
        this.SelectedInvoices = new ObservableCollection([]);
        this._ErrorLogPMFileLoggerService = new ErrorLogPMFileLoggerService();
        this._ErrorLogPMFileLoggerService.get(this.ClientBankListLogUntilDateyyyyMMdd)
            .subscribe((response: ServiceResponse) => {

                this._2LogBankList = response.Result.IsLogInOn;

                this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_AVA_AUTOPAY", "NON", "NON", SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
                    let obj = response.Result;
                    if (obj) {
                        let DefaultValue = obj['DefaultValue'];
                        if (!AppTool.IsNullOrEmpty(DefaultValue) && DefaultValue == "Y") {
                            this.IsAutomaticPayment = true;

                        }
                    }
                });
            });
    }

    ngOnInit() {


    }



    OnCheckedAutomaticPayment(event) {
        if (event.target.checked && this.FuturePaymentDateTime != null) {
            this.AutomaticPayment = 0;
            event.preventDefault();
            event.target.checked = false;
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Show("לא ניתן לבצע תשלום בזמינות עם תאריך תשלום עתידי");//TextCodeTranslator.Translate("")

        }

        else {
            this.AutomaticPayment = Number(event.target.checked);
            if (!this.AutomaticPayment) {
                this.ErrorMessage = null;
                this.IsDisplayMessage = false;
                this.RefreshScreen()
            }
        }
    }



    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {

            this.entityResourceService.getEntityResourceByTableName("Customs.PaymentMethodType").subscribe((response: any) => {
                this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                    this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPayment").subscribe((response: any) => {
                        this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPaymentMethod").subscribe((response: any) => {
                            this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPaymentProtest").subscribe((response: any) => {
                                this.entityResourceService.getEntityResourceByTableName("Customs.CustomBank").subscribe((response: any) => {


                                    this.DeclarationPM = args.EntityPM;

                                    if (this.DeclarationPM.IsCourierDeclaration) {
                                        let myDeclarationCourierStatusListService: DeclarationCourierStatusListService = new DeclarationCourierStatusListService();

                                        let filters = new ApiQueryFilters();

                                        filters.addAdditionalFilter("DeclarationId", this.DeclarationPM.Id, null, null, "Equals", false, false, false, "string");
                                        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
                                        filters.PageSize = 1;
                                        myDeclarationCourierStatusListService.getByFilters(filters)
                                            .subscribe((serviceResponse1: ServiceResponse) => {
                                                let mappedDeclarationCourierStatusList: Array<DeclarationCourierStatusList> = serviceResponse1.Result;
                                                if (mappedDeclarationCourierStatusList != null && mappedDeclarationCourierStatusList.length > 0) {
                                                    this._CourierWorksheet = mappedDeclarationCourierStatusList[0];
                                                }

                                            });

                                    }
                                    if ((this.DeclarationPM.ImporterEntitlementTypeCode == "17" || this.DeclarationPM.ImporterEntitlementTypeCode == "18") && FeatureLocator.HasFeaturePermession("Customs.Declaration", "BTPA")) {
                                        this.supplierInvoiceExtendedPMService.GetSupplierInvoicesPMsForDeclaration(this.DeclarationPM.Id).subscribe(
                                            invoices => {
                                                if (invoices.Result != null) {
                                                    this.sumBtl = 0.0;
                                                    invoices.Result.forEach((el) => {
                                                        if (el.SupplierInvoiceItems != null && el.SupplierInvoiceItems.length > 0) {
                                                            el.SupplierInvoiceItems.forEach(si => {
                                                                if (si.SupplierInvoiceItemTaxes != null && si.SupplierInvoiceItemTaxes.length > 0) {
                                                                    si.SupplierInvoiceItemTaxes.forEach(it => {
                                                                        if (it.TotalBtlCoverageNIS != null)
                                                                            this.sumBtl += it.TotalBtlCoverageNIS;
                                                                    })
                                                                }
                                                            })
                                                        }

                                                    });
                                                }
                                                this.LoadPayment();
                                                this.CheckRequrierdFieldsForSend();
                                            });
                                    }

                                    else {
                                        this.LoadPayment();

                                        this.CheckRequrierdFieldsForSend();
                                    }
                                   

                                });
                            });
                        });
                    });
                });
            });

        }
    }

    CheckRequrierdFieldsForSend() {
        var isExport = false;
        if (this.DeclarationPM.Direction == 'E') {
            isExport = true;
        }

        var customsRequiredFieldListService: CustomsRequiredFieldListService = new CustomsRequiredFieldListService();
        var table = window.ObjectTables.filter(d => d.Name == 'Customs.DeclarationPayment')[0];
        var filters = new ApiQueryFilters();
        filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");

        var customsRequiredFieldExtendedListService: CustomsRequiredFieldExtendedListService = new CustomsRequiredFieldExtendedListService();
        filters = customsRequiredFieldExtendedListService.GetFilter(filters, isExport)


        customsRequiredFieldListService.getAllFromCache(filters).subscribe((response: ServiceResponse) => {
            var requiredFields = response.Result;
            requiredFields.forEach((field) => {
                var objectField = window.ObjectFields.filter(d => d.FieldCode == field.ObjectfieldCode)[0];
                this.UIProperties.SetWarning(objectField.FieldName, 'Customs.DeclarationPayment', true);
            });
        });
    }

    FillGridsData() {

        // PaymentMethods List
        this.PaymentMethodsList = new ObservableCollection([]);
        if (!AppTool.IsNullOrEmpty(this.paymentPM)) {
            for (let item of this.paymentPM.DeclarationPaymentMethods) {
                this.PaymentMethodsList.Insert(new PaymentMethodModel(item, this));
            }
        }

        // Protests List
        this.PaymentProtestsList = new ObservableCollection([]);
        if (!AppTool.IsNullOrEmpty(this.paymentPM)) {
            for (let item of this.paymentPM.DeclarationPaymentProtests) {
                this.PaymentProtestsList.Insert(new PaymentProtestModel(item, this));
            }
        }

        this.CalculateTotalAmount();

    }

    //#region Properties

    public get PaymentDate() {
        return this.paymentPM.PaymentDate;
    }
    public set PaymentDate(newValue: Date) {
        this.paymentPM.PaymentDate = newValue;
    }

    public get TotalTax() { return this.DeclarationPM.TotalTax; }
    public set TotalTax(newValue: number) {
        this.DeclarationPM.TotalTax = newValue;
    }

    public get ProcessADescription() { return this.paymentPM.ProcessADescription; }
    public set ProcessADescription(newValue: string) {
        this.paymentPM.ProcessADescription = newValue;
    }

    public get IsProcessA() { return this.paymentPM.IsProcessA == null ? false : this.paymentPM.IsProcessA; }
    public set IsProcessA(newValue: boolean) {
        this.paymentPM.IsProcessA = newValue;
    }

    public get SignatoryIdentification() { return this.paymentPM.SignatoryIdentification; }
    public set SignatoryIdentification(newValue: string) {
        this.paymentPM.SignatoryIdentification = newValue;
    }

    public get CreatedByUserId() { return this.paymentPM.CreatedByUserId; }
    public set CreatedByUserId(newValue: string) {
        this.paymentPM.CreatedByUserId = newValue;
    }

    public get FuturePaymentDateTime() { return this.paymentPM.FuturePaymentDateTime; }
    public set FuturePaymentDateTime(newValue: Date) {
        this.paymentPM.FuturePaymentDateTime = newValue;
    }

    public get AutomaticPayment() { return this.paymentPM.AutomaticPayment; }
    public set AutomaticPayment(newValue: number) {
        this.paymentPM.AutomaticPayment = newValue;
    }

    _FuturePaymentTime: Date;
    public get FuturePaymentTime() { return this._FuturePaymentTime; }
    public set FuturePaymentTime(newValue: Date) {
        if (newValue) {
            var date: Date = this.FuturePaymentDateTime;
            if (this.paymentPM.FuturePaymentDateTime && typeof (this.paymentPM.FuturePaymentDateTime) == 'string') {
                date = this.GetDateFromString(this.paymentPM.FuturePaymentDateTime);
            }

            // var date = new Date(Date.parse(this.paymentPM.FuturePaymentDateTime + "")); // sometimes this variable contains string value of date, so convert it to date
            //else
            //    var date = this.GetTodaysDate();// new Date();
            if (date != null && !this.AutomaticPayment) {
                var datetime = this.GetDate(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), newValue.getUTCHours(), newValue.getUTCMinutes(), newValue.getUTCSeconds());//new Date(date.getFullYear(), date.getMonth(), date.getDate(), newValue.getHours(), newValue.getMinutes(), newValue.getSeconds());
                this.FuturePaymentDateTime = datetime;
                this._FuturePaymentTime = datetime;
            }
            else if (date == null && !this.AutomaticPayment) {
                this._FuturePaymentTime = newValue;
            }

        }
        else {
            this._FuturePaymentTime = newValue;

        }
    }

    private _GetCreditInternalBankId: string;
    get GetCreditInternalBankId() { return this._GetCreditInternalBankId; }
    set GetCreditInternalBankId(value: string) {
        this._GetCreditInternalBankId = value;
    }

    //#endregion
    GetTodaysDate() {
        var today: Date = new Date();
        today.setUTCFullYear(today.getFullYear());
        today.setUTCMonth(today.getMonth());
        today.setUTCDate(today.getDate());
        today.setUTCHours(today.getHours());
        today.setUTCMinutes(today.getMinutes());
        today.setUTCSeconds(today.getSeconds());
        today.setUTCMilliseconds(0);
        return today;
    }

    GetDateFromString(datestring: string) {
        //2016/08/14 05:00:00
        //2016-08-14T05:00:00
        //2016/08/14 05:00:00 PM
        //console.log("this is the date string that arrived " + datestring);
        var dateAndTime: string[];
        var suffix: string;
        if (datestring.indexOf('T') > -1) {
            dateAndTime = datestring.split('T');
        }
        else {
            dateAndTime = datestring.split(' ');
        }
        var dateArray: string[];
        if (dateAndTime[0].indexOf('/') > -1) {
            dateArray = dateAndTime[0].split('/');
        }
        else if (dateAndTime[0].indexOf('-') > -1) {
            dateArray = dateAndTime[0].split('-');
        }
        else if (dateAndTime[0].indexOf('.') > -1) {
            dateArray = dateAndTime[0].split('.');
        }

        if (dateAndTime.length > 2) {
            suffix = dateAndTime[2];
        }

        var timeArray: string[];
        var hour: number = 0;
        var minute: number = 0;
        var second: number = 0;
        if (dateAndTime.length >= 2) {
            if (dateAndTime[1].indexOf('.') > -1) {
                timeArray = dateAndTime[1].split('.')[0].split(':');
            }
            else {
                timeArray = dateAndTime[1].split(':');
            }
            var hour: number = this.GetTimeFor24Mode(Number(timeArray[0]), suffix);
            var minute: number = Number(timeArray[1]);
            var second: number = Number(timeArray[2].substring(0, 2));
        }

        var year: number = Number(dateArray[0]);
        var month: number = Number(dateArray[1]) - 1;
        var day: number = Number(dateArray[2]);

        var date: Date = this.GetDate(year, month, day, hour, minute, second);
        return date;
    }

    GetTimeFor24Mode(hours: number, suffix: string) {
        if (suffix) {
            var convHour;
            if (suffix.toLowerCase() == 'am') {
                if (hours >= 12) {
                    switch (hours) {
                        case 12: {
                            convHour = 0;
                            break;
                        }
                        case 13: {
                            convHour = 1;
                            break;
                        }
                        case 14: {
                            convHour = 2;
                            break;
                        }
                        case 15: {
                            convHour = 3;
                            break;
                        }
                        case 16: {
                            convHour = 4;
                            break;
                        }
                        case 17: {
                            convHour = 5;
                            break;
                        }
                        case 18: {
                            convHour = 6;
                            break;
                        }
                        case 19: {
                            convHour = 7;
                            break;
                        }
                        case 20: {
                            convHour = 8;
                            break;
                        }
                        case 21: {
                            convHour = 9;
                            break;
                        }
                        case 22: {
                            convHour = 10;
                            break;
                        }
                        case 23: {
                            convHour = 11;
                            break;
                        }
                    }
                    return convHour.toString();

                }
                return hours.toString();
            }
            if (suffix.toLowerCase() == 'pm') {
                if (hours < 12) {
                    switch (hours) {
                        case 0: {
                            convHour = 12;
                            break;
                        }
                        case 1: {
                            convHour = 13;
                            break;
                        }
                        case 2: {
                            convHour = 14;
                            break;
                        }
                        case 3: {
                            convHour = 15;
                            break;
                        }
                        case 4: {
                            convHour = 16;
                            break;
                        }
                        case 5: {
                            convHour = 17;
                            break;
                        }
                        case 6: {
                            convHour = 18;
                            break;
                        }
                        case 7: {
                            convHour = 19;
                            break;
                        }
                        case 8: {
                            convHour = 20;
                            break;
                        }
                        case 9: {
                            convHour = 21;
                            break;
                        }
                        case 10: {
                            convHour = 22;
                            break;
                        }
                        case 11: {
                            convHour = 23;
                            break;
                        }
                    }
                    return convHour.toString();

                }
                return hours.toString();
            }
        }
        return hours;
    }

    GetDate(year: number, month: number, day: number, hour: number, minute: number, second: number) {
        var date: Date = new Date();
        date.setUTCDate(1);
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(hour);
        date.setUTCMinutes(minute);
        date.setUTCSeconds(second);
        date.setUTCMilliseconds(0);
        return date;
    }

    LoadPayment() {
        //if (!dontPerformCheckEnabled) {
        //    CheckEnable();
        //}
        //else {
        this.declarationWebService.GetSingleDeclarationPaymentPMandDefaultExplain(this.DeclarationPM.Id, this.DeclarationPM.CustomerCode)
            .subscribe((response: ServiceResponse) => {
                console.log("[response] GetSingleDeclarationPaymentPMandDefaultExplain: ", response);
                var result = response.Result;
                if (!AppTool.IsNullOrEmpty(result)) {
                    this.paymentPM = result;
                }
                this.loadPaymentCompleted();
            });
        //dontPerformCheckEnabled = false;
        //}
    }
    loadPaymentCompleted() {

        this.FillGridsData();

        // create new entity if there is no payment
        if (AppTool.IsNullOrEmpty(this.paymentPM)) {
            this.paymentPM = new DeclarationPaymentPM();
        }

        this.IsDisplayOnlyAutomaticPayment = (this.DeclarationPM.AvailabilityDate != null && this.DeclarationPM.AvailabilityDate.toString() != '0001-01-01T00:00:00' && !this.paymentPM.AutomaticPayment);
        if (!AppTool.IsNullOrEmpty(this.paymentPM) && AppTool.IsNullOrEmpty(this.paymentPM.DeclarationId)) {
            this.paymentPM.DeclarationId = this.DeclarationPM.Id;
            this.paymentPM.Tenant = this.DeclarationPM.Tenant;

            this.entityCreated = true;
        }

        this.PostSendCreditToGetBank();

        if (this.paymentPM.FuturePaymentDateTime) {
            //this.FuturePaymentTime = new Date(Date.parse(this.paymentPM.FuturePaymentDateTime + "")); // sometimes this variable contains string value of date, so convert it to date
            this.FuturePaymentTime = DateTool.GetDateParts(this.paymentPM.FuturePaymentDateTime).DateObject;


            var datetimeParts = DateTool.GetDateParts(this.paymentPM.FuturePaymentDateTime);
            var stringOfYear = AppTool.PadLeft("" + datetimeParts.Year, 4, '0');
            var stringOfMonth = AppTool.PadLeft("" + datetimeParts.Month, 2, '0');
            var stringOfDay = AppTool.PadLeft("" + datetimeParts.Day, 2, '0');
            var stringOfHours = AppTool.PadLeft("" + datetimeParts.Hours, 2, '0');
            var stringOfHours12 = AppTool.PadLeft("" + datetimeParts.Hours12, 2, '0');
            var stringOfMinutes = AppTool.PadLeft("" + datetimeParts.Minutes, 2, '0');
            var stringOfSeconds = AppTool.PadLeft("" + datetimeParts.Seconds, 2, '0');
            var stringOfMilliseconds = AppTool.PadLeft("" + datetimeParts.Milliseconds, 3, '0');
            var stringDatetime = stringOfYear + "-" + stringOfMonth + "-" + stringOfDay + " " + stringOfHours + ":" + stringOfMinutes + "";

            //var p = datetime.formatUTC("yyyy.MM.dd T HH:mm");
            console.log("Future DateTime:", stringDatetime);

        }

        //init PaymentDate only if empty task-34833 ///////// commented by Task 36380 below
        //if (AppTool.IsNullOrEmpty(this.paymentPM.PaymentDate)) {
        //    //this.paymentPM.PaymentDate = new Date();
        //    this.paymentPM.PaymentDate = DateTool.GetCurrentDateTimeAsUtc();
        //}

        // update payment date if the payment is opened
        //this.initDates();
        //

        //else if (this.paymentPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert) // moran 3.1.17 - AMI-58876
        //{
        //    if (context.DeclarationPaymentPMs.Contains(this.paymentPM)) {
        //        context.DeclarationPaymentPMs.Detach(this.paymentPM);
        //        context.DeclarationPaymentPMs.Add(this.paymentPM);

        //        entityCreated = true;
        //    }
        //}

        // PayWithProtest Default
        if (!AppTool.IsNullOrEmpty(this.paymentPM.CustomsAgentExplanationDefault)) {
            this._PayWithProtest_Default = this.paymentPM.CustomsAgentExplanationDefault;
            if (AppTool.IsNullOrEmpty(this.paymentPM.DeclarationPaymentProtests) || this.paymentPM.DeclarationPaymentProtests.length == 0) {
                this.NewProtestMethod();
            }
            else if (AppTool.IsNullOrEmpty(this.paymentPM.DeclarationPaymentProtests[0].CustomsAgentExplanation)) {
                this.paymentPM.DeclarationPaymentProtests[0].CustomsAgentExplanation = this._PayWithProtest_Default;
            }
        }


        this.CheckRequireds();

        //this.CurrentEntity = this.paymentPM;

        this.RefreshScreen();

        //if (AppTool.IsNullOrEmpty(this.paymentPM.DeclarationPaymentMethods) || this.paymentPM.DeclarationPaymentMethods.length == 0) {

        //    /*
        //    Auto Filling screen data - check field Default “CGG_PAYHAND_FIL” in Unifreight  , if TRUE - WI 32966
        //    PaymentMethodType - 1(Masab)
        //    PaymentMethodAmount - Total Tax (same as double click on the field)
        //    */


        //    //this.OldAutoFillPaymentScreen();
        //    this.AutoFillPaymentScreenByDefault();

        //}



    }

    private PostSendCreditToGetBank() {

        if (this.DeclarationPM.PaymentDate) {
            return;
        }

        let objecttable: ObjectTablePM = window.ObjectTables.filter(d => d.Name == "Customs.Declaration")[0];
        let searchParams = new CustomFileCreditRequestParams();
        {
            searchParams.Tenant = SessionLocator.Tenant;
            searchParams.AppicationId = this.DeclarationPM.Id;//this.entityParent.DeclarationId;
            searchParams.LoggingEnabled = true;
            searchParams.LoggingEntityId = this.DeclarationPM.Id; //entityParent.DeclarationId;
            searchParams.LoggingObjectTableId = objecttable.Id;
            searchParams.LoggingUserId = SessionLocator.LoggedUserId;
            searchParams.RequestName = "Send Credit to Get Bank Request";
            searchParams.ResponseName = "Get Credit to Get Bank Response";
            searchParams.Mode = "GetBank";
        }

        searchParams.RequestVIA = SendRequestVIA.Default;
        var myIIGGeneralMessagesService = new IIGGeneralMessagesService();
        SessionLocator.SelectedSession.StartBusyIndicator("");

        myIIGGeneralMessagesService.PostCustomFileCredit(searchParams)
            .subscribe((myServiceResponse: ServiceResponse) => {

                let customFileCreditResponseData: CustomFileCreditResponseData = myServiceResponse.Result as CustomFileCreditResponseData;
                var newDate = DateTool.GetCurrentDateTimeAsUtc();
                var currentDate: Date = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate(), newDate.getHours(), newDate.getMinutes(), 0); // last of today
                var paymentDate: Date = DateTool.GetDateFromDate(this.PaymentDate);

                if (customFileCreditResponseData.PaymentDateTime == null) {
                    customFileCreditResponseData.PaymentDateTime = currentDate;
                }
                var paymentDateTime: Date = DateTool.GetDateFromDate(customFileCreditResponseData.PaymentDateTime);

                if (paymentDateTime <= currentDate) {
                    if (paymentDate > currentDate) {
                        this.PaymentDate = paymentDate;
                    }
                    else {
                        this.PaymentDate = customFileCreditResponseData.PaymentDateTime;
                    }
                    this.FuturePaymentDateTime = null;
                    this.FuturePaymentTime = null;
                }
                else {
                    this.PaymentDate = currentDate;
                    this.FuturePaymentDateTime = customFileCreditResponseData.PaymentDateTime;
                    this.FuturePaymentTime = DateTool.GetDateParts(customFileCreditResponseData.PaymentDateTime).DateObject;
                }
                if (!AppTool.IsNullOrEmpty(customFileCreditResponseData.BankCode)) {
                    var customBankListService: CustomBankListService = new CustomBankListService();
                    customBankListService.getAll().subscribe((response: ServiceResponse) => {
                        let allCustomBankList: CustomBankList[] = response.Result;
                        let bank: CustomBankList = allCustomBankList.filter(d => d.InternalCode == customFileCreditResponseData.BankCode && !d.InActive)[0];
                        if (!AppTool.IsNullOrEmpty(bank)) {
                            this.GetCreditInternalBankId = bank.Id;
                            //if (this.PaymentMethodsList && this.PaymentMethodsList.Collection) {
                            //    this.JustAutoFillPaymentScreen();
                            //}
                            if (AppTool.IsNullOrEmpty(this.paymentPM.DeclarationPaymentMethods) || this.paymentPM.DeclarationPaymentMethods.length == 0) {
                                this.AutoFillPaymentScreenByDefault();
                            }
                        }
                    });
                }
                else if (AppTool.IsNullOrEmpty(this.paymentPM.DeclarationPaymentMethods) || this.paymentPM.DeclarationPaymentMethods.length == 0) {
                    this.AutoFillPaymentScreenByDefault();
                }

                SessionLocator.SelectedSession.StopBusyIndicator();
            });
    }

    FillSignData() {

        // fill user  if the declaration is not payed
        if (!this.IsDisplayOnly) {

            this.userListService
                //.getSingleFromCache(this.DeclarationPM.SignedByUserId)
                .getSingle(this.DeclarationPM.SignedByUserId)
                .subscribe((response: ServiceResponse) => {
                    var user: UserList = response.Result;
                    console.log("[Response] userListService.getSingleFromCache: ", user);

                    if (!AppTool.IsNullOrEmpty(user)) {
                        if (user.PersonalId == this.DeclarationPM.SignerPersonalId) {
                            this.paymentPM.CreatedByUserId = this.DeclarationPM.SignedByUserId;
                            this.CreatedByUserId = this.DeclarationPM.SignedByUserId;


                        }
                    }
                });


        }

        //fill SignatoryIdentification if the declaration is not payed
        if (!this.IsDisplayOnly && this.DeclarationPM.SignerPersonalId) { //if payed -> its display only
            this.paymentPM.SignatoryIdentification = this.DeclarationPM.SignerPersonalId;
        }
        //Only for Courier - Task 54622
        if (AppTool.IsNullOrEmpty(this.paymentPM.SignatoryIdentification) && this.DeclarationPM.IsCourierDeclaration) {
            //if (this.DeclarationPM.Consignments != null && this.DeclarationPM.Consignments.length > 0) {
            //this.paymentPM.SignatoryIdentification = this.DeclarationPM.Consignments[0].SecondCargoID;
            this.customsSettingListService.getAll().subscribe((response: ServiceResponse) => {
                var list = response.Result;
                console.log("[response/customsSettingListService.getAll]", list);
                if (!AppTool.IsNullOrEmpty(list)) {
                    var customsSetting = list[0];
                    this.paymentPM.SignatoryIdentification = customsSetting.CustomsAgentId;
                    this.SignatoryIdentification = customsSetting.CustomsAgentId;
                }
            });
        }
    }
    AutoFillPaymentScreenByDefault() {
        this.NewMethodMethod();
        if (this.sumBtl != null && this.sumBtl > 0) {
            this.JustAutoFillPaymentScreen();
        }
        else {

            if (!AppTool.IsNullOrEmpty(this.GetCreditInternalBankId)) {
                if (this.PaymentMethodsList && this.PaymentMethodsList.Collection) {
                    this.JustAutoFillPaymentScreen();
                    return;
                }
            }

            this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CIM_PAYCASH_FIL", "NON", this.DeclarationPM.CustomerCode, SessionLocator.Tenant)
                .subscribe((response: ServiceResponse) => {
                    let obj = response.Result;
                    if (obj) {
                        let DefaultValue = obj['DefaultValue'];
                        if (!AppTool.IsNullOrEmpty(DefaultValue)) {
                            if (this.PaymentMethodsList && this.PaymentMethodsList.Collection) {
                                this.JustAutoFillPaymentScreenCash(DefaultValue);
                            }
                        }
                        else {
                            this.AutoFillPaymentScreen();
                        }
                    }
                });


        }


    }
    AutoFillPaymentScreen() {
        this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_PAYHAND_FIL", "NON", "NON", SessionLocator.Tenant)
            .subscribe(
                (response: ServiceResponse) => {

                    let obj = response.Result;
                    if (obj) {
                        let DefaultValue = obj['DefaultValue'];

                        if (DefaultValue == "A") {//==AutoFillPaymentScreen //if (customsSetting.AutoFillPaymentScreen) {
                            if (this.PaymentMethodsList && this.PaymentMethodsList.Collection) {
                                var MinAndMax;
                                this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_PAY_AMT_RNG", "NON", "NON", SessionLocator.Tenant)
                                    .subscribe(
                                        (response: ServiceResponse) => {
                                            let obj = response.Result;
                                            if (obj) {
                                                var CGG_PAY_AMT_RNGDefault: string = obj['DefaultValue'];
                                                if (CGG_PAY_AMT_RNGDefault != "" && CGG_PAY_AMT_RNGDefault != null) {
                                                    let MinAndMax = CGG_PAY_AMT_RNGDefault.split("-");
                                                    let min = parseFloat(MinAndMax[0].replace(/,/g, ''));
                                                    let max = parseFloat(MinAndMax[1].replace(/,/g, ''));
                                                    if (min < this.TotalTax && max > this.TotalTax) {
                                                        this.BetweenMinAndMax = true;
                                                    }
                                                }
                                            }
                                            this.JustAutoFillPaymentScreen();
                                        });

                            }
                        }

                    }
                });
    }

   
    JustAutoFillPaymentScreen() {

        if (this.sumBtl != null && this.sumBtl > 0) {
            for (let method of this.PaymentMethodsList.Collection) {
                method.Amount = this.DeclarationPM.TotalTax - this.sumBtl;
                method.MethodTypeCode = "1";
                this.paymentMethodTypeListService.getSingleFromCache("1").subscribe((response: ServiceResponse) => {
                    method.MethodTypeName = response.Result.LocalName;
                });
            }
            this.NewMethodMethod(true);
            this.PaymentMethodsList.Collection[this.PaymentMethodsList.Collection.length - 1].Amount = this.sumBtl;


            //     this.PaymentMethodsList.Collection.push(method);
         
        }
        else {
            for (let method of this.PaymentMethodsList.Collection) {
                method.Amount = this.DeclarationPM.TotalTax;
                method.MethodTypeCode = "1";
                this.paymentMethodTypeListService.getSingleFromCache("1").subscribe((response: ServiceResponse) => {
                    method.MethodTypeName = response.Result.LocalName;
                });
            }
        }
    }


    JustAutoFillPaymentScreenCash(defaultValue: string) {

        if (this.sumBtl != null && this.sumBtl > 0) {
            for (let method of this.PaymentMethodsList.Collection) {
                method.Amount = this.DeclarationPM.TotalTax - this.sumBtl;
                method.MethodTypeCode = "2";
                this.paymentMethodTypeListService.getSingleFromCache("2").subscribe((response: ServiceResponse) => {
                    method.MethodTypeName = response.Result.LocalName;
                });
                method.PayerActivityTypeCode = defaultValue;
                this.customerActivityTypeListService.getSingleFromCache(defaultValue).subscribe((response: ServiceResponse) => {
                    method.PayerActivityTypeName = response.Result.LocalName;
                });
            }
            this.NewMethodMethod();
            this.PaymentMethodsList.Collection[this.PaymentMethodsList.Collection.length - 1].Amount = this.sumBtl;

        }
        else {
            for (let method of this.PaymentMethodsList.Collection) {
                method.Amount = this.DeclarationPM.TotalTax;
                method.MethodTypeCode = "2";
                this.paymentMethodTypeListService.getSingleFromCache("2").subscribe((response: ServiceResponse) => {
                    method.MethodTypeName = response.Result.LocalName;
                });
                method.PayerActivityTypeCode = defaultValue;
                this.customerActivityTypeListService.getSingleFromCache(defaultValue).subscribe((response: ServiceResponse) => {
                    method.PayerActivityTypeName = response.Result.LocalName;
                });
            }
        }
    }

    OldAutoFillPaymentScreen() {
        this.customsSettingListService.getAll().subscribe((response: ServiceResponse) => {
            var list = response.Result;
            console.log("[response/customsSettingListService.getAll]", list);
            if (!AppTool.IsNullOrEmpty(list)) {
                var customsSetting = list[0];
                this.NewMethodMethod();

                if (!AppTool.IsNullOrEmpty(customsSetting)) {
                    if (customsSetting.AutoFillPaymentScreen) {

                        this.JustAutoFillPaymentScreen();

                    }
                }
            }
        });
    }
    CheckRequireds() {

    }
    CheckTotals() {
        var totalTax = 0;
        if (this.DeclarationPM.TotalTax == null) {
            totalTax = 0;
        }
        else {
            totalTax = this.DeclarationPM.TotalTax;
        }
        // this.DeclarationPM.TotalTax = 0;
        if (this.DeclarationPM.DeclarationTaxes.length > 0) {

            var sum = 0.0;
            this.DeclarationPM.DeclarationTaxes.forEach((el) => { sum += el.TotalAmount; });

            if (totalTax != sum) {
                this.IsDisplayOnly = true;
                this.OkButtonEnabled = false;
                //MessageBorderVisibility = Visibility.Visible;
                this.ErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.DifferentTotals");
            }
        }

        else {
            if (totalTax > 0) {
                this.IsDisplayOnly = true;
                this.OkButtonEnabled = false;
                //MessageBorderVisibility = Visibility.Visible;
                this.ErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.DifferentTotals");
            }
        }
    }
    NewProtestMethod() {
        this.NewProtestClicked();
    }
    NewMethodMethod(isBtl = false) {
        this.AddPaymentMethodClicked(isBtl);
    }
    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
    }
    GetDeclarationStatusColor(statusCode: string) {
        var color = "#45494A";

        if (!AppTool.IsNullOrEmpty(statusCode)) {

            switch (statusCode) {
                //case "הוגש, מםושר להתרה":
                //case  "הותר":
                //case "הותר ויצם מםתר בפיקוח המכס":
                //case "טיוטה תקינה, ממתין להגשה":
                case "3":
                case "7":
                case "8":
                case "13":
                    {
                        color = "#009161"; // green
                        break;
                    }

                //case "הוגש, ממתין לםישור םילוץ התרה":
                //case "הוגש, ממתין להחלטת המכס":
                //case "הוגש, ממתין להתרה":
                //case "טיוטה הוגשה לתםריך עתידי":
                //case "טיוטה ממתינה לםישור םילוץ הגשה":
                //case "ממתין לםישור פיצול":
                //case "ממתין לםישור בקשת םחסנה":
                case "4":
                case "5":
                case "6":
                case "10":
                case "11":
                case "15":
                case "21":
                    {
                        color = "#F37021"; // orange
                        break;
                    }

                //case "סטטוס לם ידוע":
                //case "בוטל":
                //case "הוגש, הצהרה שגויה":
                //case "טיוטה בוטלה":
                //case "טיוטה שגויה":
                //case "יש להגיש םת ההצהרה מחדש":
                //case "פוצל":
                //case "םסור ביבום":
                //case "הסחורה נתפסה":
                //case "הותר - הצהרה שגויה":
                //case "הותר ויצם מםתר בפיקוח מכס - הצהרה שגויה":
                case "0":
                case "1":
                case "2":
                case "9":
                case "12":
                case "14":
                case "16":
                case "17":
                case "18":
                case "19":
                case "20":
                    {
                        color = "#E53030"; // red
                        break;
                    }
            }
        }
        return color;
    }

    //#region Display only logic
    IsDisplayOnly: boolean = false;
    OkButtonEnabled: boolean = true;
    SendButtonEnabled: boolean = false;

    RefreshScreen() {
        var entityPM = this.DeclarationPM;

        if (entityPM.IsChanged) {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.ChangedDeclaration");
        }
        else {
            this.IsDisplayOnly = false;
            this.OkButtonEnabled = true;
            this.SendButtonEnabled = true;
        }

        if (!AppTool.IsNullOrEmpty(entityPM.PaymentDate)) {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.PaidDeclaration");
        }

        if (entityPM.DeclarationStatusTypeCode == "11") {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.WaitingApproval");
        }

        if (entityPM.DeclarationStatusTypeCode == "10") {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.FuturePayment");
        }

        var controller = SessionLocator.SelectedSession.CurrentEditComponent.EditComponentController;
        if (controller.InDisplayMode == true) {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = controller.InDisplayModeMessage;
        }

        if (entityPM.DeclarationStatusTypeCode == "14") {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = TextCodeTranslator.Translate("Customs.General.O.SubmitDeclarationAgain");
        }


        this.CheckTotals();

        //this.BuildMethods();
        //this.BuildProtest();

        this.DisplayOnlyCheck();


        if (entityPM.AutomaticPayment && this.ErrorMessage != TextCodeTranslator.Translate("Customs.General.O.InAutomaticPayment")) {
            this.OkButtonEnabled = true;
            this.DisplayAutomaticPayment = false;

        }

        this.SetScreenFieldsEditability();
    }

    public DrawMe: boolean = true;
    public ShowStorageStatusMessage: boolean;

    DisplayOnlyCheck() {
        var declarationDisplayOnly: boolean = false;
        this.DrawMe = true;
        this.ShowStorageStatusMessage = false;

        //get declaration display only
        declarationDisplayOnly = SessionLocator.SelectedSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.DeclarationPM.AmendmentMessage != null && this.DeclarationPM.AmendmentMessage != "") {
            {
                this.IsDisplayMessage = true;
                this.ErrorMessage = this.DeclarationPM.AmendmentMessage;
                if (this.DeclarationPM.IsAmendmentDisplayOnly) this.IsDisplayOnly = this.DeclarationPM.IsAmendmentDisplayOnly;
            }
        }

        else if (declarationDisplayOnly) {
            this.IsDisplayOnly = true;
            this.ErrorMessage = "לתצוגה בלבד - " + SessionLocator.SelectedSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;

            this.SetScreenFieldsEditability();
            DeclarationEventManager.DisplayModeChanged.emit(declarationDisplayOnly);

            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
        }
        else if (this.DeclarationPM.StorageStatusCode && !this.ErrorMessage) {
            this.ShowStorageStatusMessage = true;
            this.ErrorMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.DeclarationPM.StorageStatusName;
            this.IsDisplayOnly = false;


        }


        //if is not display only => its not payed , so fill sign data
        if (!this.IsDisplayOnly)
            this.FillSignData();

        //FuturePaymentTime editablity
        if (this.IsDisplayOnly) {
            this.UIProperties.SetEnabled("FuturePaymentTime", this.ObjectTableName, false);
        } else {
            this.UIProperties.SetEnabled("FuturePaymentTime", this.ObjectTableName, true);
        }


        //
        // if declaration display only checks changed
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.DeclarationPM).subscribe((response: any) => {
            var displayOnlyCheckResult: DisplayOnlyCheckResult = response.Result;

            var declarationDisplayOnly2 = displayOnlyCheckResult.IsDisplayOnly ? true : false;
            if (this.DeclarationPM.AmendmentMessage != null && this.DeclarationPM.AmendmentMessage != "") {
                {
                    this.IsDisplayMessage = true;

                    this.ErrorMessage = this.DeclarationPM.AmendmentMessage;
                    if (this.DeclarationPM.IsAmendmentDisplayOnly) this.IsDisplayOnly = this.DeclarationPM.IsAmendmentDisplayOnly;
                }
            }

            else if (declarationDisplayOnly2) {
                this.IsDisplayOnly = true;
                this.ErrorMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;

                this.IsDisplayOnly = true;
                this.OkButtonEnabled = false;
                this.SendButtonEnabled = false;
            }
            else if (this.DeclarationPM.StorageStatusCode && !this.ErrorMessage) {
                this.ShowStorageStatusMessage = true;
                this.ErrorMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.DeclarationPM.StorageStatusName;
                this.IsDisplayOnly = false;
            }

            //if is not display only => its not payed , so fill sign data
            if (!this.IsDisplayOnly)
                this.FillSignData();

            // set payment date after display only check finshed
            this.initDates();


            this.SetScreenFieldsEditability();
            DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);


            //FuturePaymentTime editablity
            if (this.IsDisplayOnly) {
                this.UIProperties.SetEnabled("FuturePaymentTime_timepicker", this.ObjectTableName, false);
            } else {
                this.UIProperties.SetEnabled("FuturePaymentTime_timepicker", this.ObjectTableName, true);
            }

        });



    }




    SetScreenFieldsEditability() {
        var enabled = !this.IsDisplayOnly;
        this.UIProperties.SetEnabled("PaymentDate", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ProcessADescription", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("IsProcessA", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("SignatoryIdentification", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("CreatedByUserId", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("FuturePaymentDateTime", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("FuturePaymentTime", this.ObjectTableName, enabled);
    }

    //#endregion

    //#region Payment Method
    PaymentMethodMessage: string = "";
    IsPaymentMethodMessageVisible: boolean = false;
    newLine: boolean = false;

    AddPaymentMethodClicked(isBtl: boolean) {

        this.newLine = true;
        var line = 0;
        var seq = 0;
        var method: DeclarationPaymentMethodPM = null;

        if (this.paymentPM.DeclarationPaymentMethods.length == 1 && !isBtl) {
            method = this.paymentPM.DeclarationPaymentMethods.find(d => d.PayerActivityTypeCode == null || d.MethodTypeCode == null || d.Amount == null);
        }
        if (!AppTool.IsNullOrEmpty(method)) {
            this.IsPaymentMethodMessageVisible = true;
            this.PaymentMethodMessage = TextCodeTranslator.Translate("Customs.Declaration.O.PaymentMethodFields");
        }
        else {
            this.IsPaymentMethodMessageVisible = false;
            this.PaymentMethodMessage = "";
            if (this.paymentPM.DeclarationPaymentMethods.length > 0) {

                var line = this.paymentPM.DeclarationPaymentMethods.reduce(function (prev, current) { return (prev.Line > current.Line) ? prev : current }).Line;
                var seq = this.paymentPM.DeclarationPaymentMethods.reduce(function (prev, current) { return (prev.SequenceNumeric > current.SequenceNumeric) ? prev : current }).SequenceNumeric;

            }

            line++;
            seq++;

            var item = new DeclarationPaymentMethodPM(this.paymentPM);
            item.Tenant = SessionLocator.Tenant;
            item.DeclarationId = this.DeclarationPM.Id;
            item.Line = line;
            item.SequenceNumeric = seq;
            this.paymentPM.AddDeclarationPaymentMethod(item);

            var itemModel = new PaymentMethodModel(item, this);
            this.PaymentMethodsList.Insert(itemModel);
        }

        this.BuildMethods();

        //RefreshScreenEvent myEvent = SessionLocator.CurrentAssemblyLocator.EventAggregator.GetEvent<RefreshScreenEvent>();
        //myEvent.Publish(new RefreshScreenEventArgs("DeclarationPaymentMethods"));
        //myEvent.Publish(new RefreshScreenEventArgs("rowadded-paymentmethods"));
    }
    RemovePaymentMethodClicked(item: PaymentMethodModel) {

        if (this.IsDisplayOnly) return;

        if (!AppTool.IsNullOrEmpty(item)) {
            this.PaymentMethodsList.Remove(item);
            this.paymentPM.RemoveDeclarationPaymentMethod(item.methodPM);
        }
    }

    BuildMethods() {

    }

    TotalAmount: number = 0;
    //public get TotalAmount() {
    //    if (!AppTool.IsNullOrEmpty(this.PaymentMethodsList)) {
    //        var sum = 0.0;
    //        this.PaymentMethodsList.Collection.forEach((method: PaymentMethodModel) => {
    //            sum += AppTool.IsNullOrEmpty(method.Amount) ? 0 : method.Amount;
    //        });
    //        return sum;
    //    } else {
    //        return 0;
    //    }
    //}
    //public set TotalAmount(value) {
    //    this.totalAmount = value;
    //}

    CalculateTotalAmount() {
        var total = 0;
        if (!AppTool.IsNullOrEmpty(this.PaymentMethodsList)) {
            this.PaymentMethodsList.Collection.forEach((method: PaymentMethodModel) => {
                total += AppTool.IsNullOrEmpty(method.Amount) ? 0 : method.Amount;
            });
        }

        this.TotalAmount = total;
    }

    //#endregion

    //#region Payment Protest

    NewProtestClicked() {
        if (this.IsDisplayOnly) return;

        var line = 0;

        if (this.paymentPM.DeclarationPaymentProtests.length > 0) {
            var line = this.paymentPM.DeclarationPaymentMethods.reduce(function (prev, current) { return (prev.Line > current.Line) ? prev : current }).Line;
        }

        line++;

        var protest = new DeclarationPaymentProtestPM(this.paymentPM);
        protest.Tenant = SessionLocator.Tenant;
        protest.DeclarationId = this.DeclarationPM.Id;
        protest.Line = line;
        protest.CustomsAgentExplanation = this._PayWithProtest_Default;

        if (!this.paymentPM.DeclarationPaymentProtests.includes(protest)) {
            this.paymentPM.DeclarationPaymentProtests.push(protest);
        }

        var item = new PaymentProtestModel(protest, this);
        this.PaymentProtestsList.Insert(item);


        this.BuildProtest();
    }

    DeleteProtestClicked(item: PaymentProtestModel) {
        if (item) {

            this.paymentPM.RemoveDeclarationPaymentProtest(item.protestPM);

            this.PaymentProtestsList.Remove(item);

        }
    }

    BuildProtest() {

    }

    //#endregion

    //#region Submit Payment
    saving: boolean = false;
    instructionCancelled: boolean = false;

    FuturePaymentDateTimeOnBlur(event) {
        // WI 32593
        this.IsFuturePaymentDateValid(null);


    }
    FuturePaymentTimeOnBlur(event) {
        // WI 32593
        this.IsFuturePaymentDateValid(event);


    }
    IsFuturePaymentDateValid(event) {
        if (this.AutomaticPayment && event != null) {
            var myMessageWindow = new MessageWindow
            myMessageWindow.Show("לא ניתן לבצע תשלום בזמינות עם תאריך תשלום עתידי");//TextCodeTranslator.Translate("")
            this.FuturePaymentDateTime = null;
            this.paymentPM.FuturePaymentDateTime = null;
            this.FuturePaymentTime = null;
            return false;
        }
        if (this.FuturePaymentDateTime && !this.AutomaticPayment) {

            var newDate = new Date();
            var currentDate = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate(), 0, 0, 0);

            if (this.FuturePaymentDateTime < currentDate) {
                this.UIProperties.SetValidity("FuturePaymentDateTime", "Customs.DeclarationPayment", false, TextCodeTranslator.Translate("Customs.Declaration.O.futuredatecantbepast"));
                return false;
            } else {
                this.UIProperties.SetValidity("FuturePaymentDateTime", "Customs.DeclarationPayment", true, "");
                return true;
            }

        }
        else // no date entered
        {
            this.UIProperties.SetValidity("FuturePaymentDateTime", "Customs.DeclarationPayment", true, "");
            return true;
        }
    }

    PaymentDateTimeOnBlur(event) {
        this.IsPaymentDateValid();
    }
    IsPaymentDateValid() {

        if (this.PaymentDate) {

            //var newDate = new Date();
            var newDate = DateTool.GetCurrentDateTimeAsUtc();
            var currentDate = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate(), 0, 0, 0); // last of today

            if (this.PaymentDate < currentDate) {
                this.UIProperties.SetValidity("PaymentDate", "Customs.DeclarationPayment", false, "לא ניתן להזין תאריך בעבר");
                return false;
            } else {
                this.UIProperties.SetValidity("PaymentDate", "Customs.DeclarationPayment", true, "");
                return true;
            }

        }
        else // no date entered
        {
            this.UIProperties.SetValidity("PaymentDate", "Customs.DeclarationPayment", true, "");
            return true;
        }
    }

    OkButtonClicked() {
        if (!this.FuturePaymentTime && this.FuturePaymentDateTime) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("הזן זמן עתידי"); // Please enter a future time
            return;
        }

        var isFuturePaymentDateValid = this.IsFuturePaymentDateValid(null); // WI 32593
        var isPaymentDateValid = this.IsPaymentDateValid();
        var isBlockTime = false;
        if (!isFuturePaymentDateValid || !isPaymentDateValid) {

            this.ValidationErrorsList = [];
            if (!isFuturePaymentDateValid)
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Declaration.O.futuredatecantbepast"));
            if (!isPaymentDateValid)
                this.ValidationErrorsList.push("לא ניתן להזין תאריך בעבר");

        }
        else {
            this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_PAY_BLK_RNG", "NON", "NON", SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
                let obj = response.Result;
                if (obj) {
                    let timeCompany = obj['DefaultValue'];
                    //timeCompany = "12:00 - 13:00";
                    this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CIM_PAY_BLK_RNG", "NON", this.DeclarationPM.CustomerCode, SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
                        let obj = response.Result;
                        if (obj) {
                            let timeCustomer = obj['DefaultValue'];
                            //   timeCustomer = "20:00 - 22:00";
                            if (AppTool.IsNullOrEmpty(timeCompany) && AppTool.IsNullOrEmpty(timeCustomer)) {
                                isBlockTime = false;
                            }
                            else {

                                if (!this.AutomaticPayment) {
                                    if (!AppTool.IsNullOrEmpty(this.FuturePaymentDateTime)) {
                                        if (!AppTool.IsNullOrEmpty(timeCompany)) {
                                            if (this.CheckIdDateBetween2Times(timeCompany, this.FuturePaymentDateTime)) {
                                                this.ValidationErrorsList.push("לא ניתן להגיש תשלום בשעות שהוזנו , לפי הגדרה ברמת חברה");
                                                isBlockTime = true;
                                            }

                                        }
                                        if (!AppTool.IsNullOrEmpty(timeCustomer)) {

                                            if (this.CheckIdDateBetween2Times(timeCustomer, this.FuturePaymentDateTime)) {
                                                this.ValidationErrorsList.push("לא ניתן להגיש תשלום בשעות שהוזנו , לפי הגדרה ברמת לקוח");
                                                isBlockTime = true;
                                            }
                                        }
                                    }

                                    else {
                                        if (!AppTool.IsNullOrEmpty(timeCompany)) {

                                            if (this.CheckIdDateBetween2Times(timeCompany, this.PaymentDate)) {
                                                this.ValidationErrorsList.push("לא ניתן להגיש תשלום בשעות שהוזנו , לפי הגדרה ברמת חברה");
                                                isBlockTime = true;
                                            }
                                        }
                                        if (!AppTool.IsNullOrEmpty(timeCustomer)) {

                                            if (this.CheckIdDateBetween2Times(timeCustomer, this.PaymentDate)) {
                                                this.ValidationErrorsList.push("לא ניתן להגיש תשלום בשעות שהוזנו , לפי הגדרה ברמת לקוח");
                                                isBlockTime = true;
                                            }
                                        }
                                    }

                                }
                            }


                            if (!isBlockTime) {
                                this.ValidationErrorsList = [];
                                this.ActivateUnifreightInstructionOK();
                            }

                        }




                    });
                }
            });
        }



        //var isBlockTime: boolean;

        //isBlockTime = this.CheckIfBlockTime();




    }



    CheckIdDateBetween2Times(times: any, date1: Date) {

        if (times == null) return false;
        var startTime = times.split("-")[0];
        var endTime = times.split("-")[1];

        if (startTime == null || endTime == null) return false;
        var date = new Date(date1.getFullYear(), date1.getMonth(), date1.getDate(), date1.getUTCHours(), date1.getUTCMinutes(), 0);

        var startDate = new Date(date.getTime());
        startDate.setHours(startTime.split(":")[0]);
        startDate.setMinutes(startTime.split(":")[1]);

        var endDate = new Date(date.getTime());
        endDate.setHours(endTime.split(":")[0]);
        endDate.setMinutes(endTime.split(":")[1]);



        return startDate < date && endDate > date
    }

    ActivateUnifreightInstructionOK() {
        //if (!AppTool.IsNullOrEmpty(this.DeclarationPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        if (AmitalGatewayUtil.Instance.IsDeclarationInUse(this.DeclarationPM.CustomFileNo, this.DeclarationPM.IsConvertedDeclaration, this.DeclarationPM.IsConnectedToUnifreight)) {
            SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.UnifreightInstSentMehes"));

            var myStoreViewUnifreightInstructionController
                = new UnifreightController(this.DeclarationPM, "Logitude.Customs.ViewModels.DeclarationPayment.DeclarationPaymentTabViewModel.MyStoreViewUnifreightInstructionController");

            myStoreViewUnifreightInstructionController.
                //UnifreightCallbackCompleted += (sender, e) => {
                GetPromise().then((e) => {
                    if (e.UnifreightResponseStatus) {
                        this.SubmitChanges();
                    }
                    else {
                        this.instructionCancelled = true;
                        SessionLocator.SelectedSession.StopBusyIndicator();
                    }
                    //myStoreViewUnifreightInstructionController.DisposeUnifreightMassaging();

                });
            SessionLocator.SelectedSession.StartBusyIndicator("");
            myStoreViewUnifreightInstructionController.SendRequestInstructionToUnifreightAsync("PAYHAND_STORE");
        }
        else {
            this.SubmitChanges();
        }
    }
    SubmitChanges() {
        //ActualOk
        if (this.entityCreated) {
            this.declarationPaymentPMService.insert(this.paymentPM).subscribe((response: ServiceResponse) => {
                var result = response.Result;
                console.log("[Response] declarationPaymentPMService.insert ", result);
                if (!AppTool.IsNullOrEmpty(result)) {

                    this.SubmitCompleted(response);

                    this.paymentPM = result;
                }
                this.entityCreated = false;
            });

        } else {
            this.declarationPaymentPMService.update(this.paymentPM).subscribe((response: ServiceResponse) => {
                var result = response.Result;
                console.log("[Response] declarationPaymentPMService.insert ", result);
                //if (!AppTool.IsNullOrEmpty(result)) {
                //    this.SubmitCompleted(response);

                //}
                this.SubmitCompleted(response);
            });
        }

    }

    SubmitCompleted(response: ServiceResponse) {
        if (!response.HasError && !this.instructionCancelled) {
            this.saving = true;
            this.RefreshDeclaration();
            SessionLocator.SelectedSession.CloseCurrentWindow();
        }
        else {

            var errors = [];
            Validator.TryValidateObject(this.DeclarationPM, "Customs.Declaration", errors);
            for (var item of this.paymentPM.DeclarationPaymentMethods) {
                Validator.TryValidateObject(item, "Customs.DeclarationPaymentMethod", errors);
            }

            for (var el of this.paymentPM.DeclarationPaymentProtests) {
                Validator.TryValidateObject(el, "Customs.DeclarationPaymentProtest", errors);
            }

            if (errors.length > 0) {
                this.ValidationErrorsList = errors;
            }
        }
    }
    RefreshDeclaration() {
        this.declarationService.get(this.paymentPM.DeclarationId).subscribe((res: ServiceResponse) => {
            this.DeclarationPM = res.Result;
        });
    }
    //#endregion

    //#region Send Payment
    customSendOptions: CustomSendOptionsArgs;
    Option: string;
    // Before send
    SendButtonClicked(event) {
        if (event.TestCase) {

            let windowArgs = { "SincroScreen": "SincroSendDeclarationPayment" };

            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 400;
            logWindow.Title = "תרחשי DeclarationPayment";
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(res => {
                    if (!AppTool.IsNullOrEmpty(res) && res == "Ok") {
                        this._TestCase = new TestCase();
                        this._TestCase.Code = comp._ScenarioCode;
                        this._TestCase.Param1 = comp.Param1;
                        this._TestCase.Param2 = comp.Param2;
                        this.SendButtonClickedStart(event);

                    }
                });
            });

            logWindow.Show('./CustomsModules/CustomsControls/Components/TestCase/SendDeclarationTastCaseComponent');
            ///this.StopMyBusyIndicator();///this.CurrentSession.CurrentEditComponent.StopBusyIndicator();

            return;

        }
        this._TestCase = null;
        this.SendButtonClickedStart(event);
    }

    SendButtonClickedStart(event) {
        if (this._CourierWorksheet != null && this._CourierWorksheet.CourierPendingReasonErrorPlace == "1" /*=="בתשלום"*/) {
            var myMessageWindow = new MessageWindow
            myMessageWindow.Show(/*"לם ניתן לבצע הגשת תשלום כםשר יש השהייה מסוג עצירת תשלום. "*/
                TextCodeTranslator.Translate("Customs.CourierMaster.M.PaymentPendingHold"));
            return;
        }
        this.customSendOptions = event;
        this.Option = event.Option;

        //#region Future Payment date validation
        if (!this.FuturePaymentTime && this.FuturePaymentDateTime) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("הזן זמן עתידי"); // Please enter a future time
            return;
        }
        //#endregion

        //#region Validate dates
        var isFuturePaymentDateValid = this.IsFuturePaymentDateValid(null); // WI 32593
        var isPaymentDateValid = this.IsPaymentDateValid();
        if (isFuturePaymentDateValid && isPaymentDateValid) {
            this.ValidationErrorsList = [];
        }
        else {
            this.ValidationErrorsList = [];
            if (!isFuturePaymentDateValid)
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Declaration.O.futuredatecantbepast"));
            if (!isPaymentDateValid)
                this.ValidationErrorsList.push("לא ניתן להזין תאריך בעבר");
        }
        //#endregion

        if (this.ValidationErrorsList.length > 0) return;

        var isBlockTime = false;

        this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_PAY_BLK_RNG", "NON", "NON", SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
            let obj = response.Result;
            if (obj) {
                let timeCompany = obj['DefaultValue'];
                //timeCompany = "12:00 - 13:00";
                this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CIM_PAY_BLK_RNG", "NON", this.DeclarationPM.CustomerCode, SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
                    let obj = response.Result;
                    if (obj) {
                        let timeCustomer = obj['DefaultValue'];
                        //   timeCustomer = "20:00 - 22:00";
                        if (AppTool.IsNullOrEmpty(timeCompany) && AppTool.IsNullOrEmpty(timeCustomer)) {
                            isBlockTime = false;
                        }
                        else {

                            if (!this.AutomaticPayment) {
                                if (!AppTool.IsNullOrEmpty(this.FuturePaymentDateTime)) {
                                    if (!AppTool.IsNullOrEmpty(timeCompany)) {
                                        if (this.CheckIdDateBetween2Times(timeCompany, this.FuturePaymentDateTime)) {
                                            this.ValidationErrorsList.push("לא ניתן להגיש תשלום בשעות שהוזנו , לפי הגדרה ברמת חברה");
                                            isBlockTime = true;
                                        }

                                    }
                                    if (!AppTool.IsNullOrEmpty(timeCustomer)) {

                                        if (this.CheckIdDateBetween2Times(timeCustomer, this.FuturePaymentDateTime)) {
                                            this.ValidationErrorsList.push("לא ניתן להגיש תשלום בשעות שהוזנו , לפי הגדרה ברמת לקוח");
                                            isBlockTime = true;
                                        }
                                    }
                                }

                                else {
                                    if (!AppTool.IsNullOrEmpty(timeCompany)) {

                                        if (this.CheckIdDateBetween2Times(timeCompany, this.PaymentDate)) {
                                            this.ValidationErrorsList.push("לא ניתן להגיש תשלום בשעות שהוזנו , לפי הגדרה ברמת חברה");
                                            isBlockTime = true;
                                        }
                                    }
                                    if (!AppTool.IsNullOrEmpty(timeCustomer)) {

                                        if (this.CheckIdDateBetween2Times(timeCustomer, this.PaymentDate)) {
                                            this.ValidationErrorsList.push("לא ניתן להגיש תשלום בשעות שהוזנו , לפי הגדרה ברמת לקוח");
                                            isBlockTime = true;
                                        }
                                    }
                                }

                            }
                        }


                        if (!isBlockTime) {
                            // Validate payment date with future date
                            if (this.FuturePaymentDateTime && this.PaymentDate) {

                                this.PaymentDate = new Date(Date.parse(this.PaymentDate + "")); // sometimes this variable contains string value of date, so convert it to date
                                this.FuturePaymentDateTime = new Date(Date.parse(this.FuturePaymentDateTime + "")); // sometimes this variable contains string value of date, so convert it to date

                                var paymentDate = new Date(this.PaymentDate.getFullYear(), this.PaymentDate.getMonth(), this.PaymentDate.getDate(), 0, 0, 0);
                                var futurePaymentDateTime = new Date(this.FuturePaymentDateTime.getFullYear(), this.FuturePaymentDateTime.getMonth(), this.FuturePaymentDateTime.getDate(), 0, 0, 0);

                                if (futurePaymentDateTime > paymentDate) {
                                    //valid
                                    var confirmWindow = new ConfirmWindow();
                                    confirmWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                                    confirmWindow.ShowCancelButton = false;
                                    confirmWindow.ShowNoButton = true;
                                    confirmWindow.WindowClosed.subscribe((event: any) => {
                                        if (confirmWindow.No) {
                                            console.log("[!] Send payment canceled");
                                            return;
                                        }
                                        else if (confirmWindow.Yes) {
                                            this.SendMethodStep1();
                                        }

                                    });
                                    confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.paymentDateSmallerThanFuture"));

                                }
                                else {
                                    this.SendMethodStep1();
                                }
                            }
                            else {
                                this.SendMethodStep1();
                            }

                        }

                    }




                });
            }
        });







    }

    SendMethodStep1() {
        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));

        if (this.entityCreated) {
            this.declarationPaymentPMService.insert(this.paymentPM).subscribe((response: ServiceResponse) => {
                var result = response.Result;
                console.log("[Response] declarationPaymentPMService.insert ", result);
                this.paymentPM = result;
                this.FillGridsData();

                if (!AppTool.IsNullOrEmpty(result)) {
                    this.CheckRequiredFields();
                } else {
                    SessionLocator.SelectedSession.StopBusyIndicator();
                }
            });
            this.entityCreated = false;
        } else {
            this.declarationPaymentPMService.update(this.paymentPM).subscribe((response: ServiceResponse) => {
                var result = response.Result;
                console.log("[Response] declarationPaymentPMService.insert ", result);
                this.paymentPM = result;
                this.FillGridsData();

                if (!AppTool.IsNullOrEmpty(result)) {
                    this.CheckRequiredFields();
                } else {
                    SessionLocator.SelectedSession.StopBusyIndicator();
                }

            });
        }
    }

    CheckRequiredFields() {
        this.declarationWebService.GetAllRequiredFieldsForDeclarationPayment(this.DeclarationPM.Id).subscribe((res: ServiceResponse) => {
            var customsRequiredFieldErrors: CustomsRequiredFieldErrors = res.Result;
            console.log("[response] GetAllRequiredFieldsForDeclarationPayment: ", customsRequiredFieldErrors);

            if (!AppTool.IsNullOrEmpty(customsRequiredFieldErrors)) {

                SessionLocator.SelectedSession.StopBusyIndicator();
                if (customsRequiredFieldErrors.RequiredFields.length == 0) {

                    //If Last Declaration was NOT Signed
                    //if (!this.DeclarationPM.IsSignedVersion) {  ///If IsCourierDeclaration= false, Check if Last Declaration Signed (IsSignedVersion.Declaration = True) , if NOT   - WI 18211
                    if (!this.DeclarationPM.IsCourierDeclaration && !this.DeclarationPM.IsSignedVersion) {

                        this.CheckBeforeSendPaymentOrder();
                    }
                    else {
                        this.SendMethod();
                    }

                } else {
                    //this.ValidationErrorsList = [];
                    //customsRequiredFieldErrors.RequiredFields.forEach(el => {
                    //    this.ValidationErrorsList.push(el.CustomMessageError);
                    //});
                    this.FillValidationErrors(TextCodeTranslator.Translate("Customs.General.O.RequiredFields"), customsRequiredFieldErrors);
                }
            } else {
                SessionLocator.SelectedSession.StopBusyIndicator();
            }

        });
    }
    CheckBeforeSendPaymentOrder() {

        this.customsSettingListService.getAll().subscribe((response: ServiceResponse) => {
            var list = response.Result;
            console.log("[response/customsSettingListService.getAll]", list);
            if (!AppTool.IsNullOrEmpty(list)) {
                var customsSetting = list[0];


                SessionLocator.SelectedSession.StopBusyIndicator();

                var listCustomsAgentId: string[] = ["550221105", "511487241"];
                if (SessionLocator.LoggedUserPM.IsCustomerCare
                    || (listCustomsAgentId.includes(customsSetting.CustomsAgentId) && new Date() < new Date(2016, 7, 24))) {

                    //Anat Friz unable to sign +2Month
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                    confirmWindow.ShowCancelButton = false;
                    confirmWindow.ShowNoButton = true;
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            this.SendMethod();
                        }
                    });
                    confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.IsSignedVersionErrorForCustomerCare"));

                } else {
                    var msg = new MessageWindow();
                    msg.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                    msg.Show(TextCodeTranslator.Translate("Customs.Declaration.O.IsSignedVersionError"));
                }


            }
        });

    }

    // Send
    SendMethod() {

        var totalAmount = AppTool.Round(this.TotalAmount, 2);
        var totalTax = AppTool.Round(this.TotalTax, 2);

        if (AppTool.IsNullOrEmpty(totalTax)) {
            totalTax = 0;
        }
        if (totalAmount != totalTax) {
            var msg = new MessageWindow();
            msg.Show(TextCodeTranslator.Translate("Customs.Declaration.O.Totalmustbeequaltototaltax"));
            // error not sending
        }
        else {

            //if (AppTool.IsNullOrEmpty(this.DeclarationPM.CustomFileNo) || !AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            if (!AmitalGatewayUtil.Instance.IsDeclarationInUse(this.DeclarationPM.CustomFileNo, this.DeclarationPM.IsConvertedDeclaration, this.DeclarationPM.IsConnectedToUnifreight)) {
                this.ActualSend();
            }
            else {
                SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.UnifreightInstSentMehes"));

                var myStoreViewUnifreightInstructionController = new UnifreightController(
                    this.DeclarationPM,
                    "Logitude.Customs.ViewModels.DeclarationPayment.DeclarationPaymentTabViewModel.MyStoreViewUnifreightInstructionController");
                myStoreViewUnifreightInstructionController.GetPromise()
                    //myStoreViewUnifreightInstructionController.UnifreightCallbackCompleted += (sender, e) => {
                    .then((e) => {
                        if (e.UnifreightResponseStatus) {
                            this.ActualSend();
                        }
                        else {
                            SessionLocator.SelectedSession.StopBusyIndicator();

                        }
                    });
                SessionLocator.SelectedSession.StartBusyIndicator("");
                myStoreViewUnifreightInstructionController.SendRequestInstructionToUnifreightAsync("PAYHAND_STORE");

            }


        }
    }
    ActualSend() {

        if (this.customSendOptions == null) {
            console.log("[!] no send options!!");
            return;
        }

        var ObjectTable = window.ObjectTables.filter(x => x.Name === "Customs.Declaration")[0];

        var params = new CustomFileCreditRequestParams();
        params.Tenant = SessionLocator.Tenant;
        params.AppicationId = this.paymentPM.DeclarationId;
        //params.TestCase = SelectedTest;
        params.LoggingEnabled = true;
        params.LoggingEntityId = this.DeclarationPM.Id;
        params.LoggingObjectTableId = ObjectTable.Id;
        params.LoggingUserId = SessionLocator.LoggedUserId;
        params.RequestName = "send declaration payment request";
        params.ResponseName = "send declaration payment response";
        params.Mode = "Check";
        params.RequestVIA = this.customSendOptions.RequestVIA;
        params.ForcePersonalSign = this.customSendOptions.ForcePersonalSign;
        params.TestCase = this._TestCase;
        let splitRequest = true;
        if (splitRequest) {
            this.CheckCustomFileCreditThenSendPayment(params);
            return;
        }

        //var myCustomMessageProgressHelper = new CustomMessageProgressHelper();
        //myCustomMessageProgressHelper.BasicResponse = true;
        //myCustomMessageProgressHelper.StartProgress(params.PBId, 5, true);


        //this.declarationMessagesService.PostSendPaymentWithCheckCustomFileCredit(params)
        //    .subscribe((myServiceResponse: ServiceResponse) => {
        //        myCustomMessageProgressHelper.MessageArrived = true;
        //        SessionLocator.SelectedSession.StopBusyIndicator();
        //        var result: CustomFileCreditResponseData = myServiceResponse.Result;
        //        if (!AppTool.IsNullOrEmpty(result)) {
        //            var mess :string = this.AnalyzeResponseMessageSendPaymentWithCheckCustomFileCredit(result);

        //            SessionLocator.SelectedSession.StopBusyIndicator();
        //            if (!AppTool.IsNullOrEmpty(mess)) {
        //                let messWindow = new MessageWindow();

        //                SessionLocator.SelectedSession.StopBusyIndicator();
        //                messWindow.Show(mess);
        //                messWindow.WindowClosed.subscribe((event: any) => {


        //                    if (mess.toLowerCase().includes("succeeded") || mess.toLowerCase().includes("בהצלחה") ||
        //                        this._IsCloseScreen == true) // Mirit 20/07/15 Task-14344 - add successfully (Hebrew) // Mirit 24/11/15 Task 18440- add IsCloseScreen
        //                    {
        //                        this.RefreshDeclaration();
        //                        SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
        //                        if (SessionLocator.SelectedSession.CurrentWindow != null) SessionLocator.SelectedSession.CloseCurrentWindow()
        //                    }
        //                });

        //            }

        //        }
        //    });

    }
    //CheckCustomFileCreditThenSendPayment(params: CustomFileCreditRequestParams) {
    //    var myCustomMessageProgressHelper = new CustomMessageProgressHelper();
    //    myCustomMessageProgressHelper.BasicResponse = true;
    //    myCustomMessageProgressHelper.StartProgress(params.PBId, 5, true);

    //    this.declarationMessagesService.PostCheckCustomFileCreditOnly(params)
    //        .subscribe((myServiceResponse: ServiceResponse) => {
    //            myCustomMessageProgressHelper.MessageArrived = true;
    //            SessionLocator.SelectedSession.StopBusyIndicator();
    //            let customFileCreditResponseData: CustomFileCreditResponseData = myServiceResponse.Result;

    //            if (!AppTool.IsNullOrEmpty(customFileCreditResponseData)) {
    //                if (customFileCreditResponseData.IsTRansGove) {
    //                    var confirmWindow = new ConfirmWindow();
    //                    confirmWindow.Show(customFileCreditResponseData.UserMessage);
    //                    SessionLocator.SelectedSession.StopBusyIndicator();
    //                    confirmWindow.WindowClosed.subscribe((event: any) => {
    //                        if (confirmWindow.Yes) {
    //                            this.ActualSendToTransfer();
    //                        }
    //                    });

    //                    return;
    //                    //////////////////////////////////////////////////////////////////////////
    //                }
    //                CustomMessageProgressComponent.ShowProgressBar
    //                    //(PBId: string, Title: string, OnSuccessCloseWin: boolean
    //                    //    , OnSuccessCloseWinMethod?: (response: any) => boolean)

    //                    (params.PBId, "תחילת שליחה למכס- הגשת תשלום", false)
    //                    .then(res => {

    //                        SessionLocator.SelectedSession.StopBusyIndicator();//// let it be ...
    //                        let myPaymentResponseData: CustomFileCreditResponseData = res;

    //                        this.RefreshDeclaration();
    //                        SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();

    //                        if (myPaymentResponseData.HasException || !myPaymentResponseData.Succeeded) {
    //                            //let mess = myPaymentResponseData.UserMessage || "Server return Error (Witout message????!!?!)";
    //                            //if (!AppTool.IsNullOrEmpty(mess)) {
    //                            //    let messWindow = new MessageWindow();
    //                            //    messWindow.Show(mess);
    //                            //    messWindow.WindowClosed.subscribe((event: any) => {

    //                            //        this.RefreshDeclaration();
    //                            //        SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
    //                            //        //if (SessionLocator.SelectedSession.CurrentWindow != null) SessionLocator.SelectedSession.CloseCurrentWindow()

    //                            //    });

    //                            //}

    //                        } else {

    //                            if (SessionLocator.SelectedSession.CurrentWindow != null) SessionLocator.SelectedSession.CloseCurrentWindow()
    //                        }
    //                    })
    //                    .catch(err => {
    //                        err = err || "PostSendPaymentOnly return Error (Without message????!!?!)";
    //                        let messWindow = new MessageWindow();
    //                        messWindow.Show(err);

    //                    });


    //                this.declarationMessagesService.PostSendPaymentOnly(params)
    //                    .subscribe(res1 => {
    //                    });



    //            }
    //        });

    //}
    CheckCustomFileCreditThenSendPayment(params: CustomFileCreditRequestParams) {
        var myCustomMessageProgressHelper = new CustomMessageProgressHelper();
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(params.PBId, 5, true);

        this.declarationMessagesService.PostCheckCustomFileCreditOnly(params)
            .subscribe((myServiceResponse: ServiceResponse) => {
                myCustomMessageProgressHelper.MessageArrived = true;
                SessionLocator.SelectedSession.StopBusyIndicator();
                let customFileCreditResponseData: CustomFileCreditResponseData = myServiceResponse.Result;

                if (!AppTool.IsNullOrEmpty(customFileCreditResponseData)) {
                    if (customFileCreditResponseData.IsTRansGove) {
                        var confirmWindow = new ConfirmWindow();
                        confirmWindow.Show(customFileCreditResponseData.UserMessage);
                        SessionLocator.SelectedSession.StopBusyIndicator();
                        confirmWindow.WindowClosed.subscribe((event: any) => {
                            if (confirmWindow.Yes) {
                                this.ActualSendToTransfer();
                            }
                        });
                        return;
                    }
                    if (customFileCreditResponseData.IsReTRansGove) {
                        var confirmWindow = new ConfirmWindow();
                        //confirmWindow.Width = 400;
                        confirmWindow.Show(customFileCreditResponseData.UserMessage);
                        SessionLocator.SelectedSession.StopBusyIndicator();
                        confirmWindow.WindowClosed.subscribe((event: any) => {
                            if (confirmWindow.Yes) {
                                this.ActualSendToReTransfer();
                            }
                        });
                        return;
                    }
                    if (customFileCreditResponseData.HasException) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                        messageWindow.Width = 250;
                        messageWindow.Height = 150;
                        messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                        messageWindow.Show(customFileCreditResponseData.UserMessage);
                        return;
                        //////////////////////////////////////////////////////////////////////////
                    }


                    if (AmitalGatewayUtil.Instance.IsDeclarationInUse(this.DeclarationPM.CustomFileNo, this.DeclarationPM.IsConvertedDeclaration, this.DeclarationPM.IsConnectedToUnifreight)) {
                        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.UnifreightInstSentMehes"));

                        var myStoreViewUnifreightInstructionController = new UnifreightController(
                            this.DeclarationPM,
                            "Logitude.Customs.ViewModels.DeclarationPayment.DeclarationPaymentTabViewModel.MyStoreViewUnifreightInstructionController");
                        myStoreViewUnifreightInstructionController.GetPromise()
                            //myStoreViewUnifreightInstructionController.UnifreightCallbackCompleted += (sender, e) => {
                            .then((e) => {
                                if (e.UnifreightResponseStatus) {
                                    this.Send2755(params);
                                }
                                else {
                                    SessionLocator.SelectedSession.StopBusyIndicator();

                                }
                            });
                        SessionLocator.SelectedSession.StartBusyIndicator("");
                        myStoreViewUnifreightInstructionController.SendRequestInstructionToUnifreightAsync("PAYHAND_SEND");


                    } else {
                        this.Send2755(params);
                    }

                }
            });

    }
    private Send2755(params: CustomFileCreditRequestParams) {
        let myShowProgressBarParams = new ShowProgressBarParams();
        myShowProgressBarParams.OnCloseCustomMessageProgressComponentMethod =
            (response: any) => {
                let myPaymentResponseData: CustomFileCreditResponseData = response;
                if (myPaymentResponseData) {
                    if (myPaymentResponseData.HasException || !myPaymentResponseData.Succeeded) {
                        //do not close Win !!
                    }
                    else {
                        //if OK then  close Win !!
                        if (SessionLocator.SelectedSession.CurrentWindow != null)
                            SessionLocator.SelectedSession.CloseCurrentWindow();
                    }
                }
            };
        CustomMessageProgressComponent.ShowProgressBar(params.PBId, "תחילת שליחה למכס- הגשת תשלום", false, myShowProgressBarParams).then(res => {
            var ResponseData = res; // this solution to fix the paid declaration not showing a yellow message.
            if (ResponseData && ResponseData.ContinueProcessInBackground) {
                SessionLocator.SelectedSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
            }
            else if (this.Option == 'WB' || this.Option == 'D') { // work around itzik shall fix the undefined problem.
                SessionLocator.SelectedSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
            }
            SessionLocator.SelectedSession.StopBusyIndicator(); //// let it be ...
            let myPaymentResponseData: CustomFileCreditResponseData = res;
            this.RefreshDeclaration();
            SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
        })
            .catch(err => {
                err = err || "PostSendPaymentOnly return Error (Without message????!!?!)";
                let messWindow = new MessageWindow();
                messWindow.Show(err);
                messWindow.WindowClosed.subscribe(() => {
                    SessionLocator.SelectedSession.CloseCurrentWindow();
                });
            });
        if (this.AutomaticPayment != 1)
            this.declarationMessagesService.PostSendPaymentOnly(params)
                .subscribe(res1 => {
                });
        else
            SessionLocator.SelectedSession.CloseCurrentWindow();

    }

    ActualSendToTransfer() {

        if (this.customSendOptions == null) {
            console.log("[!] no send options!!");
            return;
        }

        var ObjectTable = window.ObjectTables.filter(x => x.Name === "Customs.Declaration")[0];

        var params = new CustomFileCreditRequestParams();
        params.Tenant = SessionLocator.Tenant;
        params.AppicationId = this.paymentPM.DeclarationId;
        //params.TestCase = SelectedTest;
        params.LoggingEnabled = true;
        params.LoggingEntityId = this.DeclarationPM.Id;
        params.LoggingObjectTableId = ObjectTable.Id;
        params.LoggingUserId = SessionLocator.LoggedUserId;
        params.RequestName = "Send Transfer Request",
            params.ResponseName = "Get Transfer Response",
            params.Mode = "Transfer",
            params.RequestVIA = this.customSendOptions.RequestVIA;
        params.ForcePersonalSign = this.customSendOptions.ForcePersonalSign;

        //CustomMessageProgressComponent.ShowProgressBar(params.PBId, "שליחת בקשת העברה לגובה", false).then((res) => {
        //    console.log("[Send] Response/ShowProgressBar : ", res);

        //}).catch((err) => {
        //    this.ValidationErrorsList = [];
        //    this.ValidationErrorsList.push(err);
        //});
        var myCustomMessageProgressHelper = new CustomMessageProgressHelper();
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(params.PBId, 5, true);


        this.declarationMessagesService.PostSendTransferRequest(params)
            .subscribe((myServiceResponse: ServiceResponse) => {
                myCustomMessageProgressHelper.MessageArrived = true;
                SessionLocator.SelectedSession.StopBusyIndicator();

                var result: CustomFileCreditResponseData = myServiceResponse.Result;
                if (!AppTool.IsNullOrEmpty(CustomMessageProgressComponent.CurrCustomMessageProgressHelper)) {
                    CustomMessageProgressComponent.CurrCustomMessageProgressHelper.MessageArrived = true;
                }
                SessionLocator.SelectedSession.StopBusyIndicator();
                this.AnalyzeActualSendToTransfer(result);
            });

    }
    AnalyzeActualSendToTransfer(result: CustomFileCreditResponseData) {
        if (!AppTool.IsNullOrEmpty(result)) {
            var mess = this.AnalyzeResponseMessage(result);

            if (!AppTool.IsNullOrEmpty(mess)) {
                //_CustomMassagingProgressService.ShowResponseMessage(mess);
                //_CustomMassagingProgressService.WindowClosed += (canIContinueEventArgs) => {

                let confirmWindow = new MessageWindow();

                confirmWindow.Show(mess);
                confirmWindow.WindowClosed.subscribe((event: any) => {

                    if (mess.toLowerCase().includes("succeeded") || mess.toLowerCase().includes("בהצלחה") || mess.toLowerCase().includes("נפתחה רשומה בתיקים לאישור") || this._IsCloseScreen == true) // Mirit 20/07/15 Task-14344 - add successfully (Hebrew) // Mirit 24/11/15 Task 18440- add IsCloseScreen
                    {
                        this.RefreshDeclaration();
                        if (SessionLocator.SelectedSession.CurrentWindow != null) {
                            SessionLocator.SelectedSession.CloseCurrentWindow();; // moran 17.8.16 - AMI-57900 - add not null check
                        }
                    }

                });


                //    _CustomMassagingProgressService.Dispose();
                //};
            }

        }
    }
    ActualSendToReTransfer() {

        if (this.customSendOptions == null) {
            console.log("[!] no send options!!");
            return;
        }

        var ObjectTable = window.ObjectTables.filter(x => x.Name === "Customs.Declaration")[0];

        var params = new CustomFileCreditRequestParams();

        params.Tenant = SessionLocator.Tenant;
        params.AppicationId = this.paymentPM.DeclarationId;
        //params.TestCase = SelectedTest;
        params.LoggingEnabled = true;
        params.LoggingEntityId = this.DeclarationPM.Id;
        params.LoggingObjectTableId = ObjectTable.Id;
        params.LoggingUserId = SessionLocator.LoggedUserId;
        params.RequestName = "Send ReTransfer Request";
        params.ResponseName = "Get ReTransfer Response";
        params.Mode = "ReTransfer";
        params.RequestVIA = this.customSendOptions.RequestVIA;
        params.ForcePersonalSign = this.customSendOptions.ForcePersonalSign;

        CustomMessageProgressComponent.ShowProgressBar(params.PBId, "שליחת בקשת העברה חוזרת לגובה", true).then((res) => {
            console.log("[Send] Response/ShowProgressBar : ", res);
        }).catch((err) => {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push(err);
        });

        this.declarationMessagesService.PostSendTransferRequest(params)
            .subscribe((myServiceResponse: ServiceResponse) => {

                var result: CustomFileCreditResponseData = myServiceResponse.Result;

                if (!AppTool.IsNullOrEmpty(result)) {
                    SessionLocator.SelectedSession.StopBusyIndicator();
                    var mess = this.AnalyzeResponseMessageForsendToReTransfer(result);
                    if (!AppTool.IsNullOrEmpty(mess)) {
                        let messWindow = new MessageWindow();
                        messWindow.Show(mess);
                        messWindow.WindowClosed.subscribe((event: any) => {
                            if (mess.toLowerCase().includes("succeeded") || mess.toLowerCase().includes("בהצלחה") || this._IsCloseScreen == true) {
                                this.RefreshDeclaration();
                                if (SessionLocator.SelectedSession.CurrentWindow != null) SessionLocator.SelectedSession.CloseCurrentWindow();
                            }
                        });
                        //    _CustomMassagingProgressService.Dispose();
                        //};
                    }

                }
            });

    }

    // Response analyze
    FillValidationErrors(title: string, errors: CustomsRequiredFieldErrors) {

        var RequiredFieldsList = [];

        // 1- build validation errors
        for (var error of errors.RequiredFields) {
            if (!AppTool.IsNullOrEmpty(error.CustomMessageError)) {
                RequiredFieldsList.push(TextCodeTranslator.Translate(error.CustomMessageError));
            }
            else {
                var ObjectTable = window.ObjectTables.filter(x => x.Name === error.TableName)[0];
                var field = window.ObjectFields.filter(d => d.ObjectTableId === ObjectTable.Id && d.FieldName === error.FieldName)[0];

                if (error.TableName.includes("Customs.SupplierInvoiceItem") && !AppTool.IsNullOrEmpty(error.EntityReference2)) {
                    error.EntityReference = error.EntityReference + " (חשבון " + error.EntityReference2 + ")";
                }
                var requiredField = TextCodeTranslator.GetRequiredFieldForTableMessageTranslation("Customs.General.O.FieldForTableIsRequired", field.FullNameTextCodeCode, error.TableName, error.EntityReference);

                RequiredFieldsList.push(requiredField);
            }

        }



        // 2- open window
        var windowArgs: any = {};
        windowArgs.Errors = RequiredFieldsList;
        windowArgs.ComponentHeight = '323px';
        var windowTitle = title;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        //logWindow.WindowClosed.subscribe(($event: any) => this.OnAddEditWindowClosed($event));

        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
    }
    _IsCloseScreen: boolean = false;
    AnalyzeResponseMessageSendPaymentWithCheckCustomFileCredit(responseData: CustomFileCreditResponseData) {
        var message = "";
        if (responseData != null) {
            if (responseData.IsTRansGove) {
                //_CustomMassagingProgressService.CloseWin();
                //_CustomMassagingProgressService.Dispose();
                //_CustomMassagingProgressService = null;

                var confirmWindow = new ConfirmWindow();
                confirmWindow.Show(responseData.UserMessage);
                SessionLocator.SelectedSession.StopBusyIndicator();
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.ActualSendToTransfer();
                    }
                });

                return null;
            }
            else {
                message = responseData.UserMessage;
                if (AppTool.IsNullOrEmpty((responseData.UserMessage))) {
                    if (!responseData.HasException && responseData.Succeeded) {
                        //message = "Send Payment Succeeded";
                        message = TextCodeTranslator.Translate("Customs.Declaration.O.SendPaymentSucceeded");

                    }
                    else {
                        //message = "Send Payment Failed";
                        message = TextCodeTranslator.Translate("Customs.Declaration.O.SendPaymentFailed");
                    }

                }
                if (!responseData.HasException && responseData.Succeeded) {
                    this._IsCloseScreen = true;
                }
            }
        }
        else {
            //message = "Service returned a null response!";
            message = TextCodeTranslator.Translate("Customs.Declaration.O.Servicereturnedanullresponse");

        }
        return message;
    }
    AnalyzeResponseMessage(responseData: CustomFileCreditResponseData) {
        var message = "";
        if (responseData != null) {
            if (responseData.CreditStatus == "1") // moran 16.8.16 - AMI-57900
            {
                //if (_CustomMassagingProgressService != null) {
                //    _CustomMassagingProgressService.CloseWin();
                //    _CustomMassagingProgressService.Dispose();
                //    _CustomMassagingProgressService = null;
                //}

                var confirmWindow = new ConfirmWindow();
                SessionLocator.SelectedSession.StopBusyIndicator();
                confirmWindow.Show(responseData.UserMessage);
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.ActualSendToReTransfer();
                    }
                });
                return null;
            }
            else {
                message = responseData.UserMessage;
                if (AppTool.IsNullOrEmpty(responseData.UserMessage)) {
                    if (!responseData.HasException && responseData.Succeeded) {
                        //message = "Send Transfer Request Succeeded";
                        message = TextCodeTranslator.Translate("Customs.Declaration.O.SendTransferRequestSucceeded");
                    }

                    else {
                        //message = "Send Transfer Request Failed";
                        message = TextCodeTranslator.Translate("Customs.Declaration.O.SendTransferRequestFailed");
                    }
                }
                if (!responseData.HasException && responseData.Succeeded) {
                    this._IsCloseScreen = true;
                }
            }
        }
        else {
            //   message = "Service returned a null response!";
            message = TextCodeTranslator.Translate("Customs.Declaration.O.Servicereturnedanullresponse");
        }

        return message;
    }
    AnalyzeResponseMessageForsendToReTransfer(responseData: CustomFileCreditResponseData) {
        var message = "";
        if (responseData != null) {
            message = responseData.UserMessage;
            if (AppTool.IsNullOrEmpty(responseData.UserMessage)) {
                if (!responseData.HasException && responseData.Succeeded) {
                    //message = "Send ReTransfer Request Succeeded";
                    message = TextCodeTranslator.Translate("Customs.Declaration.O.SendReTransferRequestSucceeded");

                }
                else {
                    //message = "Send ReTransfer Request Failed";
                    message = TextCodeTranslator.Translate("Customs.Declaration.O.SendReTransferRequestFailed");

                }
            }
            if (!responseData.HasException && responseData.Succeeded) {
                this._IsCloseScreen = true;
            }

        }
        else {
            //message = "Service returned a null response!";
            message = TextCodeTranslator.Translate("Customs.Declaration.O.Servicereturnedanullresponse");

        }
        return message;
    }
    //#endregion 

    OpenTaxScreen() {



        var windowArgs: any = {};
        var certificates: any[] = [];

        var logWindow = new LogitudeWindow();
        logWindow.Height = 700;
        logWindow.Width = 1000;
        logWindow.ShowCloseButton = true;
        windowArgs.EntityPM = this.DeclarationPM;
        logWindow.WindowArgs = windowArgs;
        //  logWindow.WindowClosed.subscribe(($event: any) => this.SelectionCompleted($event));

        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Taxes/DeclarationTaxesTabComponent');

    }


    initDates() {
        //Task 36380: Update Payment Date & Time when Entering Payment screen
        if (!this.IsDisplayOnly) {
            var currentDate: Date = DateTool.GetCurrentDateTimeAsUtc();
            var paymentDate: Date = DateTool.GetDateFromDate(this.PaymentDate);
            if (paymentDate <= currentDate) {
                this.paymentPM.PaymentDate = DateTool.GetCurrentDateTimeAsUtc();
            }
        }
    }
     

}

export class PaymentMethodModel extends BaseComponent {
    public ObjectTableName = "Customs.DeclarationPaymentMethod";
    public DataContext = this;
    _BanksList: CustomBankList[] = [];
    public get BanksList() { return this._BanksList; }
    public set BanksList(val) {
        this._BanksList = val;
        this.LoggIt("PaymentMethodModel.SetBanksList");
    }
    LoggIt(stack: string) {
        if (!this.parent._2LogBankList) {
            return;
        }
        let errorLogPM: ErrorLogPM = new ErrorLogPM();
        errorLogPM.Id = this.parent.ClientBankListLogUntilDateyyyyMMdd;
        errorLogPM.StackTrace = stack;
        errorLogPM.Tenant = SessionLocator.Tenant;
        errorLogPM.UserName = SessionLocator.LoggedUserId + "/" + SessionLocator.LoggedUserPM.Email;
        errorLogPM.LogDate = DateTool.GetCurrentDateTimeAsUtc();

        let myBankList = this._BanksList.map(r => JSON.stringify({
            'InternalBankId': r.Id, 'LocalName': r.LocalName, 'EnglishName': r.EnglishName
        }));

        errorLogPM.Exception = "BanksList:";
        errorLogPM.Exception += JSON.stringify(myBankList);
        errorLogPM.Exception += "\r\n";
        errorLogPM.Exception += "SelectedBank:";
        errorLogPM.Exception += JSON.stringify(this.SelectedBank);
        errorLogPM.Exception += "\r\n";
        errorLogPM.Exception += "DeclarationPaymentMethodPM:"
        errorLogPM.Exception += JSON.stringify({ 'DeclarationId': this.methodPM.DeclarationId, 'Line': this.methodPM.Line, 'SequenceNumeric': this.methodPM.SequenceNumeric });

        this.parent._ErrorLogPMFileLoggerService.insert(errorLogPM)
            .subscribe((response: ServiceResponse) => { });

    }
    BankIsNull: boolean;
    agentBanks: CustomBankList[] = [];
    IsAmountButtonVisibile: boolean = false;
    customBankListService: CustomBankListService = new CustomBankListService();
    customsSettingListService: CustomsSettingListService = new CustomsSettingListService;
    _CustomsSettingExtendedListService: CustomsSettingExtendedListService = new CustomsSettingExtendedListService();
    customsSettingExtendedListService: CustomsSettingExtendedListService = new CustomsSettingExtendedListService();
    customBankCardExtendedPMService: CustomBankCardExtendedPMService = new CustomBankCardExtendedPMService();
    constructor(public methodPM: DeclarationPaymentMethodPM, public parent: DeclarationPaymentComponent) {
        super();
        this.BankIsNull = false;
        if (methodPM.MethodTypeCode == "1") {
            this.BanksList = [];
            if (!AppTool.IsNullOrEmpty(methodPM.InternalBankId)) {
                this.customBankListService.getSingle(methodPM.InternalBankId).subscribe((response: ServiceResponse) => {
                    if (response) {
                        this.SelectedBank = response.Result;
                    }
                });

            }
            else {
                this.LoadBanks();
            }
        }

        if (parent.PaymentMethodsList.Length == 1) {
            this.IsAmountButtonVisibile = true;
        }
    
    }

    LoadBanks() {
            this.parent.declarationWebService.GetCustomBanksForCard(this.parent.DeclarationPM.CustomerId).subscribe((response: ServiceResponse) => {
                    let BlockAgentBankForMasabDefaultValue = "";
                    var result = response.Result.filter(d => !d.InActive);
                    console.log("[Response] GetCustomBanksForCard: ", result);
                    if (!AppTool.IsNullOrEmpty(result)) {
                        //this.BanksList = result;
                        this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_BLOCK_BANK", "NON", "NON", SessionLocator.Tenant)
                            .subscribe(
                                (responseDefault: ServiceResponse) => {
                                    let obj = responseDefault.Result;
                                    if (obj) {
                                        BlockAgentBankForMasabDefaultValue = obj['DefaultValue'];

                                    }

                                    this.customsSettingExtendedListService.GetSettingByTenant().subscribe((response: ServiceResponse) => {
                                        var list = response.Result;

                                        if (!AppTool.IsNullOrEmpty(list)) {
                                            var customsSetting = list;
                                            this.BanksList = result;
                                            if (result.length == 0) {
                                                this.customBankListService.getAllFromCache().subscribe((response: ServiceResponse) => {  // window.CustomBanks.filter(d => d.PayerTypeCode == "3" && !d.InActive)[0].Id; //CustomBankDataProvider.GetCachedList<CustomBankList>().Where(d => d.PayerTypeCode == "3" && !d.InActive).ToList();
                                                    if (response) {
                                                        if (!response.HasError) {

                                                            this.agentBanks = response.Result.filter(d => d.PayerTypeCode == "3" && !d.InActive);
                                                            if (this.agentBanks.length == 1) {
                                                                this.InternalBankId = this.agentBanks[0].Id;
                                                                if (!this.BankIsNull) {
                                                                    this.SelectedBank = this.agentBanks[0];
                                                                    this.BanksList = this.agentBanks;
                                                                }
                                                            }
                                                            else {
                                                                if (customsSetting != null && customsSetting.IsConnectedToUniFreight) {
                                                                    if (!AppTool.IsNullOrEmpty(this.parent.GetCreditInternalBankId)) {
                                                                        this.InternalBankId = this.parent.GetCreditInternalBankId;
                                                                    }
                                                                    this.SendCreditToGetBank();
                                                                }
                                                                this.BanksList = this.agentBanks;
                                                                if (customsSetting != null && customsSetting.IsConnectedToUniFreight) {
                                                                    //   GetCustomBankDefaultForCard();
                                                                }
                                                            }
                                                        }
                                                    }
                                                });
                                            }
                                            else {
                                                if (customsSetting != null) {
                                                    //this.InternalBankId = null;
                                                    if (!AppTool.IsNullOrEmpty(this.parent.GetCreditInternalBankId)) {
                                                        this.InternalBankId = this.parent.GetCreditInternalBankId;
                                                    }
                                                    //Check GDFDATA - “CGG_BLOCK_BANK” , in case “Y” -   don't allow user to choose a bank that is not connected to the Customer -
                                                    //if (customsSetting.BlockAgentBankForMasab) {

                                                    //case: block Agent banks
                                                    if (BlockAgentBankForMasabDefaultValue == "Y") {
                                                        //if (result.length == 1)
                                                        //{
                                                        //    if (this.InternalBankId == null) {
                                                        //        this.InternalBankId = this.BanksList[0].Id;
                                                        //    }
                                                        //    var bank: CustomBankList = this.BanksList.filter(d => d.Id == this.InternalBankId)[0];
                                                        //    this.SelectedBank = bank;

                                                        //}
                                                        //else if (result.length > 1)
                                                        //{
                                                        //    if (this.InternalBankId != null) {
                                                        //        var bank: CustomBankList = this.BanksList.filter(d => d.Id == this.InternalBankId)[0];
                                                        //        this.SelectedBank = bank;
                                                        //    }

                                                        //}

                                                        this.customBankListService.getAll().subscribe((response: ServiceResponse) => {
                                                            if (response) {
                                                                if (!response.HasError) {
                                                                    var agentBanks = [];
                                                                    agentBanks = response.Result.filter(d => d.PayerTypeCode == "3" && !d.InActive);
                                                                    agentBanks = agentBanks.concat(this.BanksList);

                                                                    if (result.length == 1) {
                                                                        if (AppTool.IsNullOrEmpty(this.parent.GetCreditInternalBankId)) {
                                                                            this.InternalBankId = this.BanksList[0].Id;
                                                                        }
                                                                        var bank: CustomBankList = agentBanks.filter(d => d.Id == this.InternalBankId)[0];
                                                                        this.SelectedBank = bank;
                                                                    }
                                                                }
                                                            }
                                                        });
                                                    }
                                                    //
                                                    //case: do not block Agent banks
                                                    else {
                                                        //if no banks
                                                        if (result.length == 0) {
                                                            this.customBankListService.getAll().subscribe((response: ServiceResponse) => {
                                                                if (response) {
                                                                    if (!response.HasError) {
                                                                        this.agentBanks = response.Result.filter(d => d.PayerTypeCode == "3" && !d.InActive);
                                                                        if (this.agentBanks.length > 0) {
                                                                            this.BanksList = this.agentBanks;
                                                                            if (this.agentBanks.length == 1) {
                                                                                if (this.InternalBankId == null) {
                                                                                    this.InternalBankId = this.BanksList[0].Id;
                                                                                }
                                                                            }
                                                                            else {
                                                                                if (this.InternalBankId != null) {
                                                                                    var bank: CustomBankList = this.BanksList.filter(d => d.Id == this.InternalBankId)[0];
                                                                                    this.SelectedBank = bank;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            });
                                                        }
                                                        //one bank
                                                        else if (result.length == 1) {
                                                            if (this.InternalBankId == null) {
                                                                this.InternalBankId = this.BanksList[0].Id;
                                                            }
                                                            this.customBankListService.getAll().subscribe((response: ServiceResponse) => {
                                                                if (response) {
                                                                    if (!response.HasError) {
                                                                        var agentBanks = [];
                                                                        agentBanks = response.Result.filter(d => d.PayerTypeCode == "3" && !d.InActive);
                                                                        this.BanksList = this.BanksList.concat(agentBanks);
                                                                        if (this.InternalBankId != null) {
                                                                            var bank: CustomBankList = this.BanksList.filter(d => d.Id == this.InternalBankId)[0];
                                                                            this.SelectedBank = bank;
                                                                        }
                                                                    }
                                                                }
                                                            });
                                                        }

                                                        //two or more & dont block agent
                                                        else if (result.length > 1) {
                                                            var agentBanks = [];
                                                            //fill connected banks
                                                            var connectedBanks = result.filter(d => !d.InActive);

                                                            //fill agent
                                                            this.customBankListService.getAll().subscribe((response: ServiceResponse) => {
                                                                if (response) {
                                                                    if (!response.HasError) {
                                                                        agentBanks = response.Result.filter(d => d.PayerTypeCode == "3" && !d.InActive);
                                                                        //fill the LOV
                                                                        this.BanksList = connectedBanks.concat(agentBanks);

                                                                        //select bank
                                                                        if (this.InternalBankId != null) {
                                                                            var bank: CustomBankList = this.BanksList.filter(d => d.Id == this.InternalBankId)[0];
                                                                            this.SelectedBank = bank;
                                                                        }
                                                                    }
                                                                }
                                                            });

                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    });
                                });
                    }
                
            });
        
    }


    private SendCreditToGetBank() {
        if (!AppTool.IsNullOrEmpty(this.InternalBankId)) return;

        let objecttable: ObjectTablePM = window.ObjectTables.filter(d => d.Name == "Customs.Declaration")[0];
        let searchParams = new CustomFileCreditRequestParams();
        {
            searchParams.Tenant = SessionLocator.Tenant;
            searchParams.AppicationId = this.parent.DeclarationPM.Id;//this.entityParent.DeclarationId;
            searchParams.LoggingEnabled = true;
            searchParams.LoggingEntityId = this.parent.DeclarationPM.Id; //entityParent.DeclarationId;
            searchParams.LoggingObjectTableId = objecttable.Id;
            searchParams.LoggingUserId = SessionLocator.LoggedUserId;
            searchParams.RequestName = "Send Credit to Get Bank Request";
            searchParams.ResponseName = "Get Credit to Get Bank Response";
            searchParams.Mode = "GetBank";
        }

        searchParams.RequestVIA = SendRequestVIA.Default;
        //searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
        //searchParams.RequestVIA = SendRequestVIA.WebServiceBatch;
        var myIIGGeneralMessagesService = new IIGGeneralMessagesService();
        SessionLocator.SelectedSession.StartBusyIndicator("");
        this.customBankListService.getAllFromCache().subscribe((response: ServiceResponse) => {
            let allCustomBankList: CustomBankList[] = response.Result;

            myIIGGeneralMessagesService.PostCustomFileCredit(searchParams)
                .subscribe((myServiceResponse: ServiceResponse) => {
                    this.InternalBankId = "";

                    let customFileCreditResponseData: CustomFileCreditResponseData = myServiceResponse.Result as CustomFileCreditResponseData;
                    let mess = this.AnalyzeResponseMessage(allCustomBankList, customFileCreditResponseData);
                    if (AppTool.IsNullOrEmpty(this.InternalBankId) && !AppTool.IsNullOrEmpty(this.parent.DeclarationPM.CustomerCode)) {
                        myIIGGeneralMessagesService.GetDefBankForCustomer(this.parent.DeclarationPM.CustomerCode, this.parent.DeclarationPM.Tenant)
                            .subscribe((myServiceResponse: ServiceResponse) => {
                                let myCIM_AGENT_BANK: string = myServiceResponse.Result;
                                //if (!AppTool.IsNullOrEmpty(myCIM_AGENT_BANK)) {
                                //    let bank: CustomBankList = this.BanksList.filter(d => d.InternalCode == myCIM_AGENT_BANK && !d.InActive)[0];
                                //    //this.parent.SelectedBankIndex = banksList.IndexOf(bank);
                                //    //FirePropertyChanged("banksList");
                                //    //FirePropertyChanged("SelectedBankIndex");
                                //    this.InternalBankId = bank.Id;
                                //}
                                this.SetInternalBankId(allCustomBankList, myCIM_AGENT_BANK);
                                SessionLocator.SelectedSession.StopBusyIndicator();
                            });


                    } else {
                        SessionLocator.SelectedSession.StopBusyIndicator();
                    }

                    //this.ResponseData = myServiceResponse.Result;
                    //this.OnMassageDisplayMethod();
                });
        });
    }
    SetInternalBankId(allCustomBankList: CustomBankList[], BankCode: string) {

        if (!AppTool.IsNullOrEmpty(BankCode)) {
            let bank: CustomBankList = allCustomBankList
                .filter(d => d.InternalCode == BankCode && !d.InActive)[0];
            //.filter(d => d.BankCode == BankCode && !d.InActive)[0];

            if (!AppTool.IsNullOrEmpty(bank)) {
                this.InternalBankId = bank.Id;
            }
        }
    }
    private AnalyzeResponseMessage(allCustomBankList: CustomBankList[], responseData: CustomFileCreditResponseData) {
        let message = "";

        if (responseData != null) {
            //if (!AppTool.IsNullOrEmpty(responseData.BankCode)) {
            //    let bank: CustomBankList = allCustomBankList.filter(d => d.InternalCode == responseData.BankCode && !d.InActive)[0];
            //    //this.parent.SelectedBankIndex = banksList.IndexOf(bank);
            //    //FirePropertyChanged("banksList");
            //    //FirePropertyChanged("SelectedBankIndex");
            //    if (!AppTool.IsNullOrEmpty(bank)) {
            //        this.InternalBankId = bank.Id;
            //    }
            //}
            this.SetInternalBankId(allCustomBankList, responseData.BankCode);
            //if (!AppTool.IsNullOrEmpty(responseData.PaymentDate)) {
            //    //this.parent.PaymentDate = GetUnifreightFormatedDate(responseData.PaymentDate, "responseData.PaymentDate");
            //    this.parent.PaymentDate = responseData.PaymentDateTime;

            //    //RefreshScreenEvent myEvent = SessionLocator.CurrentAssemblyLocator.EventAggregator.GetEvent<RefreshScreenEvent>();
            //    //myEvent.Publish(new RefreshScreenEventArgs("DeclarationPaymentMethodPaymentDate"));
            //}
            let test = false;
            if (test) {
                this.parent.PaymentDate = DateTool.AddDays(new Date(), -5);
            }
            if (responseData.CreditStatus == "1") {
                return null;
            }
            else {
                message = responseData.UserMessage;
                if (AppTool.IsNullOrEmpty(responseData.UserMessage)) {
                    if (!responseData.HasException && responseData.Succeeded) {
                        message = "Send Get Bank Request Succeeded";
                    }
                    else {
                        message = "Send Get Bank Request Failed";
                    }
                }
                if (!responseData.HasException && responseData.Succeeded) {

                }
            }
        }
        else {
            //message = "Service returned a null response!";
            message = TextCodeTranslator.Translate("Customs.Declaration.O.Servicereturnedanullresponse");
        }

        return message;
    }


    //#region Properties

    selectedBank: CustomBankList;
    get SelectedBank() { return this.selectedBank; }
    set SelectedBank(value: CustomBankList) {
        if (this.selectedBank != value) {
            this.selectedBank = value;

            if (!AppTool.IsNullOrEmpty(value)) {
                this.InternalBankId = value.Id;
                this.InternalBankName = value.LocalName;
                if (AppTool.IsNullOrEmpty(value.LocalName)) {
                    this.InternalBankName = value.EnglishName;
                }
            } else {
                this.InternalBankId = null;
                this.InternalBankName = null;
            }

            this.LoggIt("SelectedBankChanged!!");
        }
    }

    get SequenceNumeric() { return this.methodPM.SequenceNumeric; }
    set SequenceNumeric(value: number) {
        if (this.methodPM.SequenceNumeric != value) {
            this.methodPM.SequenceNumeric = value;
        }
    }

    customerActivityType: CustomerActivityTypePM;
    get CustomerActivityType() { return this.customerActivityType; }
    set CustomerActivityType(value: CustomerActivityTypePM) {

        if (this.customerActivityType != value) {
            this.customerActivityType = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.PayerActivityTypeName = value.LocalName;


        } else {
            this.PayerActivityTypeName = null;
            this.PayerActivityTypeCode = null;
        }
    }
    get MethodTypeCode() { return this.methodPM.MethodTypeCode; }
    set MethodTypeCode(value: string) {
        if (this.methodPM.MethodTypeCode != value) {
            this.methodPM.MethodTypeCode = value;
            if (value == "1") {
                this.BanksList = [];
                this.LoadBanks();
            }
            else {
                this.InternalBankId = null;
                //  this.BankDetails = null;
                this.PayerActivityTypeCode = null;
                this.PayerActivityTypeName = null;
                this.BanksList = [];
                this.SelectedBank = null;
            }
        }
    }

    get MethodTypeName() { return this.methodPM.MethodTypeName; }
    set MethodTypeName(value: string) {
        if (this.methodPM.MethodTypeName != value) {
            this.methodPM.MethodTypeName = value;
        }
    }

    get Amount() { return this.methodPM.Amount; }
    set Amount(value: number) {
        if (this.methodPM.Amount != value) {
            this.methodPM.Amount = value;
            this.parent.CalculateTotalAmount();
        }
    }

    get BankCode() { return this.methodPM.BankCode; }
    set BankCode(value: string) {
        if (this.methodPM.BankCode != value) {
            this.methodPM.BankCode = value;
        }
    }

    get InternalBankId() { return this.methodPM.InternalBankId; }
    set InternalBankId(value: string) {
        if (this.methodPM.InternalBankId != value) {
            this.methodPM.InternalBankId = value;

                this.customBankListService.getAllFromCache().subscribe((response: ServiceResponse) => {
                    if (response) {
                        if (!response.HasError) {
                            if (!this.BankIsNull) {
                                var customBank: CustomBankList = response.Result.filter(d => d.Id == value)[0];

                                if (customBank == null && this.BanksList != null) {
                                    customBank = this.BanksList.filter(d => d.Id == value)[0];
                                }
                                if (customBank != null) {
                                    this.InternalBankName = customBank.LocalName != null ? customBank.LocalName : customBank.EnglishName;
                                    if (customBank.PayerTypeCode == "0") {

                                        this.customBankCardExtendedPMService.GetSingleCustomBanksCard(value, this.parent.DeclarationPM.CustomerId).subscribe((response: ServiceResponse) => {

                                            if (response) {
                                                if (!response.HasError) {
                                                    var bankCard: CustomBanksCardPM = response.Result;
                                                    if (bankCard != null) {
                                                        this.methodPM.BankCode = customBank.BankCode;

                                                        this.methodPM.BranchCode = customBank.BranchCode;
                                                        this.methodPM.CustomsBranchId = customBank.CustomsBranchId;
                                                        this.methodPM.AccountNumber = customBank.AccountNumber;
                                                        this.methodPM.PayerActivityTypeCode = customBank.PayerTypeCode;
                                                        this.PayerActivityTypeName = customBank.PayerTypeName;


                                                    }
                                                    else {

                                                        var messageWindow = new MessageWindow();

                                                        messageWindow.Show(TextCodeTranslator.Translate("Customs.CustomBank.O.BankNotConnectedToCustomer"));



                                                        this.InternalBankId = null;
                                                        this.methodPM.InternalBankId = null;
                                                        this.InternalBankName = null;
                                                        this.SelectedBank = null;
                                                        this.PayerActivityTypeCode = null;
                                                        this.PayerActivityTypeName = null;
                                                    }
                                                }
                                            }

                                        });

                                    }
                                    else {
                                        this.methodPM.BankCode = customBank.BankCode;
                                        this.methodPM.BranchCode = customBank.BranchCode;
                                        this.methodPM.CustomsBranchId = customBank.CustomsBranchId;
                                        this.methodPM.AccountNumber = customBank.AccountNumber;
                                        this.methodPM.PayerActivityTypeCode = customBank.PayerTypeCode;
                                        this.PayerActivityTypeName = customBank.PayerTypeName;


                                    }


                                }

                                if (customBank == null) {
                                    this.methodPM.BankCode = null;
                                    this.methodPM.BranchCode = null;
                                    this.methodPM.CustomsBranchId = null;
                                    this.methodPM.AccountNumber = null;
                                    this.methodPM.PayerActivityTypeCode = null;



                                }
                                if (this.parent.BetweenMinAndMax && this.methodPM.PayerActivityTypeCode == "3") {
                                    this.BankIsNull = true;
                                    this.methodPM.MethodTypeCode = "2";
                                    this.methodPM.PayerActivityTypeCode = "3";
                                    this.methodPM.Amount = this.parent.DeclarationPM.TotalTax ;
                                    this.InternalBankName = null;
                                    this.InternalBankId = null;
                                    this.parent.paymentMethodTypeListService.getSingleFromCache("2").subscribe((response: ServiceResponse) => {
                                        this.methodPM.MethodTypeName = response.Result.LocalName;
                                    });
                                    this.parent.customerActivityTypeListService.getSingleFromCache("3").subscribe((response: ServiceResponse) => {
                                        this.methodPM.PayerActivityTypeName = response.Result.LocalName;
                                    });
                                }
                            }
                        }
                    }



                });
            }
        

    }

    get InternalBankName() { return this.methodPM.InternalBankName; }
    set InternalBankName(value: string) {
        if (this.methodPM.InternalBankName != value) {
            this.methodPM.InternalBankName = value;
        }
    }

    get PayerActivityTypeCode() { return this.methodPM.PayerActivityTypeCode; }
    set PayerActivityTypeCode(value: string) {
        if (this.methodPM.PayerActivityTypeCode != value) {
            this.methodPM.PayerActivityTypeCode = value;
        }
    }

    get PayerActivityTypeName() { return this.methodPM.PayerActivityTypeName; }
    set PayerActivityTypeName(value: string) {
        if (this.methodPM.PayerActivityTypeName != value) {
            this.methodPM.PayerActivityTypeName = value;
        }
    }

    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }

    OnDblClick() {
        if (!AppTool.IsNullOrEmpty(this.MethodTypeCode) && this.parent.PaymentMethodsList.Length == 1) {
            if (AppTool.IsNullOrEmpty(this.Amount)) {
                this.Amount = this.parent.TotalTax;
            }
        }
    }

    ComboBoxClicked() {
        this.LoadBanks();
    }
}

export class PaymentProtestModel extends BaseComponent {
    public ObjectTableName = "Customs.DeclarationPaymentProtest";
    public DataContext = this;
    constructor(public protestPM: DeclarationPaymentProtestPM, public parent: DeclarationPaymentComponent) {
        super();
        //if (!AppTool.IsNullOrEmpty(this.protestPM.InvoiceItemClassificationCode) && this.protestPM.GoodsItemLineNumber != null) {
        //    this.GoodsItemLineNumber = this.protestPM.GoodsItemLineNumber + "-" + this.protestPM.InvoiceItemClassificationCode;
        //}
        //else if (AppTool.IsNullOrEmpty(this.protestPM.GoodsItemLineNumber)) {
        //    this.GoodsItemLineNumber = this.protestPM.InvoiceItemClassificationCode;
        //}
        //else if (AppTool.IsNullOrEmpty(this.protestPM.InvoiceItemClassificationCode)) {
        //this.GoodsItemLineNumber = this.protestPM.GoodsItemLineNumber + "";
        //this.GoodsItemClassification = this.protestPM.InvoiceItemClassificationCode;
        // }
    }

    //#region Properties

    get ProtestTypeCode() { return this.protestPM.ProtestTypeCode; }
    set ProtestTypeCode(value: string) {
        if (this.protestPM.ProtestTypeCode != value) {
            this.protestPM.ProtestTypeCode = value;
        }
    }

    get ProtestTypeName() { return this.protestPM.ProtestTypeName; }
    set ProtestTypeName(value: string) {
        if (this.protestPM.ProtestTypeName != value) {
            this.protestPM.ProtestTypeName = value;
        }
    }

    get CustomsAgentExplanation() { return this.protestPM.CustomsAgentExplanation; }
    set CustomsAgentExplanation(value: string) {
        if (this.protestPM.CustomsAgentExplanation != value) {
            this.protestPM.CustomsAgentExplanation = value;
        }
    }

    get InvoiceNumber() { return this.protestPM.InvoiceNumber; }
    set InvoiceNumber(value: string) {
        if (this.protestPM.InvoiceNumber != value) {
            this.protestPM.InvoiceNumber = value;
        }
    }

    get InvoiceCounterKey() { return this.protestPM.InvoiceCounterKey; }
    set InvoiceCounterKey(value: number) {
        if (this.protestPM.InvoiceCounterKey != value) {
            this.protestPM.InvoiceCounterKey = value;
        }
    }



    get GoodsItemClassification() { return this.protestPM.GoodsItemClassification; }
    set GoodsItemClassification(value: string) {
        if (this.protestPM.GoodsItemClassification != value) {
            this.protestPM.GoodsItemClassification = value;
        }
    }

    get GoodsItemLineNumber() { return this.protestPM.GoodsItemLineNumber; }
    set GoodsItemLineNumber(value: number) {
        if (this.protestPM.GoodsItemLineNumber != value) {
            this.protestPM.GoodsItemLineNumber = value;
        }
    }

    get AmountInDispute() { return this.protestPM.AmountInDispute; }
    set AmountInDispute(value: number) {
        if (this.protestPM.AmountInDispute != value) {
            this.protestPM.AmountInDispute = value;
        }
    }


    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }


    EditProtestClicked(item: PaymentProtestModel) {

        if (!AppTool.IsNullOrEmpty(item) && !this.parent.IsDisplayOnly) {
            var windowArgs: any = {};
            var certificates: any[] = [];
            windowArgs.DeclarationPM = this.parent.DeclarationPM;
            windowArgs.Parent = this.parent;
            windowArgs.Protest = this.protestPM;
            var logWindow = new LogitudeWindow();
            logWindow.Height = 700;
            logWindow.Width = 1000;
            logWindow.ShowCloseButton = true;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(($event: any) => this.SelectionCompleted($event));

            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationPayment/SupplierInvoiceSelectionComponent');
        }
    }

    SelectionCompleted(msg: string) {
        if (msg == "ok") {

            var paymentProtestType: string = null;
            var customsAgentExplanation: string = null;
            var protestTypeCode: string = null;
            var exsists: any = null;
            var editedProtest: DeclarationPaymentProtestPM = null;
            if (this.parent.PaymentProtestsList.Length > 0) {
                paymentProtestType = this.parent.PaymentProtestsList.Collection[0].ProtestTypeName;
                protestTypeCode = this.parent.PaymentProtestsList.Collection[0].ProtestTypeCode;

                customsAgentExplanation = this.parent.PaymentProtestsList.Collection[0].CustomsAgentExplanation;
            }
            if (AppTool.IsNullOrEmpty(customsAgentExplanation)) customsAgentExplanation = this.parent._PayWithProtest_Default;

            if (this.parent.SelectedInvoices.Collection.length > 0) {

                var protests: DeclarationPaymentProtestPM[] = this.parent.paymentPM.DeclarationPaymentProtests;
                for (let protest of protests) {
                    if (protest.InvoiceNumber != null) {
                        exsists = this.parent.SelectedInvoices.Collection.filter(d => d.InvoiceNumber == protest.InvoiceNumber && d.InvoiceCounterKey == protest.InvoiceCounterKey)[0];

                        if (!exsists) {
                            editedProtest = this.parent.PaymentProtestsList.Collection.filter(d => d.DeclarationId == protest.DeclarationId && d.Line == protest.Line)[0];



                            if (editedProtest != null) {
                                this.parent.paymentPM.RemoveDeclarationPaymentProtest(editedProtest);
                                this.parent.PaymentProtestsList.Remove(editedProtest);
                            }
                        }
                    }


                    if (protest.GoodsItemLineNumber != null) {
                        exsists = this.parent.SelectedInvoiceItems.Collection.filter(a => a.SequenceNumeric == protest.GoodsItemLineNumber && a.ClassificationCode == protest.InvoiceItemClassificationCode)[0];

                        if (!exsists) {
                            editedProtest = this.parent.PaymentProtestsList.Collection.filter(a => a.DeclarationId == protest.DeclarationId && a.Line == protest.Line)[0];


                            if (editedProtest != null) {
                                this.parent.paymentPM.RemoveDeclarationPaymentProtest(editedProtest);
                                this.parent.PaymentProtestsList.Remove(editedProtest);
                            }

                        }
                    }
                }



                for (let invoice of this.parent.SelectedInvoices.Collection) {

                    exsists = this.parent.PaymentProtestsList.Collection.filter(a => a.InvoiceNumber == invoice.InvoiceNumber && a.DeclarationId == invoice.DeclarationId && a.InvoiceCounterKey == invoice.InvoiceCounterKey)[0];//(from a in declarationPaymentPM.DeclarationPaymentProtests


                    if (!exsists) {
                        if (!AppTool.IsNullOrEmpty(invoice.InvoiceNumber)) {
                            var line: number = 0;

                            //if (this.parent.PaymentProtestsList.Collection.length > 0) {
                            //    var items = this.parent.paymentPM.DeclarationPaymentProtests.sort((a, b) => { return (a.Line === b.Line) ? 0 : (a.Line < b.Line) ? -1 : 1 });
                            //    if (items.length == 0) line = 0;
                            //    else {
                            //        line = items[this.parent.paymentPM.DeclarationPaymentProtests.length - 1].Line;
                            //    }


                            //}

                            //line += 1;

                            //   var item: DeclarationPaymentProtestPM = new DeclarationPaymentProtestPM(this.parent.paymentPM);

                            //    item.Tenant = this.protestPM.Tenant,
                            // item.DeclarationId = this.parent.DeclarationPM.Id;
                            if (AppTool.IsNullOrEmpty(this.protestPM.Line)) { ///itzik :???
                                this.protestPM.Line = line;
                            }

                            this.protestPM.InvoiceNumber = invoice.InvoiceNumber;
                            this.protestPM.InvoiceCounterKey = invoice.InvoiceCounterKey;
                            this.protestPM.ProtestTypeName = paymentProtestType,
                                this.protestPM.CustomsAgentExplanation = customsAgentExplanation,
                                this.protestPM.ProtestTypeCode = protestTypeCode


                            //        if (!this.parent.paymentPM.DeclarationPaymentProtests.includes(item)) {
                            //            this.parent.paymentPM.DeclarationPaymentProtests.push(item);
                            //            this.parent.PaymentProtestsList.Insert(new PaymentProtestModel(item, this.parent));
                            //}

                        }
                    }

                }


            }
            if (this.parent.SelectedInvoiceItems.Collection.length > 0) {
                for (let invoiceItem of this.parent.SelectedInvoiceItems.Collection) {



                    var invoice = this.parent.SelectedInvoices.Collection.filter(d => d.DeclarationId == invoiceItem.DeclarationId && d.InvoiceCounterKey == invoiceItem.CounterKey)[0];

                    if (invoice != null) {
                        exsists = this.parent.PaymentProtestsList.Collection.filter(a => a.GoodsItemLineNumber == invoiceItem.SequenceNumeric && a.InvoiceItemClassificationCode == invoiceItem.ClassificationCode && a.DeclarationId == invoice.DeclarationId && a.InvoiceNumber == invoice.InvoiceNumber)[0];// (from a in declarationPaymentPM.DeclarationPaymentProtests


                        if (!exsists) {
                            if (!AppTool.IsNullOrEmpty(invoice.InvoiceNumber)) {
                                var line: number = 0;


                                //if (this.parent.paymentPM.DeclarationPaymentProtests.length > 0) {
                                //    var items = this.parent.paymentPM.DeclarationPaymentProtests.sort((a, b) => { return (a.Line === b.Line) ? 0 : (a.Line < b.Line) ? -1 : 1 });
                                //    if (items.length == 0) line = 0;
                                //    else {
                                //        line = items[this.parent.paymentPM.DeclarationPaymentProtests.length - 1].Line;
                                //    }
                                //}

                                //line += 1;


                                //  var item: DeclarationPaymentProtestPM = new DeclarationPaymentProtestPM(this.parent.paymentPM);

                                // item.Tenant = this.protestPM.Tenant;// TenantContext.Current.Id,
                                //  item.DeclarationId = this.parent.DeclarationPM.Id;// entityPM.Id,
                                // item.Line = line;
                                this.protestPM.GoodsItemLineNumber = invoiceItem.SequenceNumeric;
                                // this.protestPM.InvoiceNumber = invoice.InvoiceNumber;
                                this.protestPM.ProtestTypeName = paymentProtestType;
                                this.protestPM.AddedByInvoice = true;
                                this.protestPM.InvoiceItemClassificationCode = invoiceItem.ClassificationCode;
                                this.protestPM.CustomsAgentExplanation = customsAgentExplanation;
                                this.protestPM.ProtestTypeCode = protestTypeCode;
                                this.protestPM.GoodsItemClassification = invoiceItem.ClassificationCode;
                                //if (!AppTool.IsNullOrEmpty(this.protestPM.InvoiceItemClassificationCode) && this.protestPM.GoodsItemLineNumber != null) {
                                //    this.GoodsItemLineNumber = this.protestPM.GoodsItemLineNumber.toString();// + "-" + this.protestPM.InvoiceItemClassificationCode;
                                //}
                                //else if (AppTool.IsNullOrEmpty(this.protestPM.GoodsItemLineNumber)) {
                                //    this.GoodsItemLineNumber = this.protestPM.InvoiceItemClassificationCode;
                                //}
                                //else if (AppTool.IsNullOrEmpty(this.protestPM.InvoiceItemClassificationCode)) {


                                //this.GoodsItemLineNumber = this.protestPM.GoodsItemLineNumber + "";///Severity	Code	Description	Project	File	Line	Suppression State                                                                                                                                                                                                         Error	TS2322	Type 'string' is not assignable to type 'number'.TypeScript Virtual Projects	C: \LW\.\Customsmodules\Customsdeclarationmodules\Declarationothers\Components\DeclarationPayment\DeclarationPaymentComponent.ts	2278	Active
                                this.GoodsItemLineNumber = this.protestPM.GoodsItemLineNumber;
                                this.GoodsItemClassification = this.protestPM.InvoiceItemClassificationCode;
                                // }

                                //if (!this.parent.paymentPM.DeclarationPaymentProtests.includes(item)) {
                                //    this.parent.paymentPM.DeclarationPaymentProtests.push(item);
                                //    this.parent.PaymentProtestsList.Insert(new PaymentProtestModel(item, this.parent));
                                //}

                            }
                        }
                    }
                }
            }



            else if (this.parent.SelectedInvoices.Collection.length == 0) {


                if (this.parent.PaymentProtestsList.Collection.length > 0) {
                    for (let protest of this.parent.PaymentProtestsList.Collection) {

                        if (protest.InvoiceNumber != null) {
                            this.parent.paymentPM.RemoveDeclarationPaymentProtest(protest);

                            this.parent.PaymentProtestsList.Remove(protest);
                        }


                    }
                }




            }

            //    this.parent.FillGridsData();




        }
    }

}
