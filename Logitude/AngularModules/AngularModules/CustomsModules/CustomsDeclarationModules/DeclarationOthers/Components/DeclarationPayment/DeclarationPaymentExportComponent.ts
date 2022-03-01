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
import { UnifreightController, UnifreightInstructionController } from '../../../../../Customs/Controller/UnifreightController';
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

    templateUrl: './DeclarationPaymentExportComponent.html',
    providers: [DeclarationExtendedListService]
})

export class DeclarationPaymentExportComponent extends BaseComponent implements OnInit {
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
    SelectedInvoiceItems: ObservableCollection;
    SelectedInvoices: ObservableCollection;
    IsDisplayOnlyAutomaticPayment: boolean;
    BetweenMinAndMax: boolean = false;
    sumBtl: any = 0.0;
    _ErrorLogPMFileLoggerService: ErrorLogPMFileLoggerService;
    _2LogBankList: boolean = false;
    IsDisplayMessage: boolean;
    DisplayAutomaticPayment: boolean = true;
    ClientBankListLogUntilDateyyyyMMdd = "20180820.ClientBankListLogUntilDateyyyyMMdd";
    _CourierWorksheet: DeclarationCourierStatusList;
    _TestCase: TestCase;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public declarationExtendedListService: DeclarationExtendedListService) {
        super();
        this.SelectedInvoiceItems = new ObservableCollection([]);
        this.SelectedInvoices = new ObservableCollection([]);
        this._ErrorLogPMFileLoggerService = new ErrorLogPMFileLoggerService();
        this._ErrorLogPMFileLoggerService.get(this.ClientBankListLogUntilDateyyyyMMdd)
            .subscribe((response: ServiceResponse) => {
                this._2LogBankList = response.Result.IsLogInOn;
            });
    }

    ngOnInit() {


    }

    isAutoFill: boolean = false;
    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {

            this.entityResourceService.getEntityResourceByTableName("Customs.PaymentMethodType").subscribe((response: any) => {
                this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                    this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPayment").subscribe((response: any) => {
                        this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPaymentMethod").subscribe((response: any) => {
                            this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPaymentProtest").subscribe((response: any) => {
                                this.entityResourceService.getEntityResourceByTableName("Customs.CustomBank").subscribe((response: any) => {
                                    this.DeclarationPM = args.EntityPM;
                                    this.LoadPayment();
                                    this.CheckRequrierdFieldsForSend();
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
        this.declarationWebService.GetSingleDeclarationPaymentPMandDefaultExplain(this.DeclarationPM.Id, this.DeclarationPM.CustomerCode)
            .subscribe((response: ServiceResponse) => {
                console.log("[response] GetSingleDeclarationPaymentPMandDefaultExplain: ", response);
                var result = response.Result;
                if (!AppTool.IsNullOrEmpty(result)) {
                    this.paymentPM = result;
                }
                this.loadPaymentCompleted();
            });
    }
    loadPaymentCompleted() {

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

                }
                else {
                    this.PaymentDate = currentDate;
                }
                if (!AppTool.IsNullOrEmpty(customFileCreditResponseData.BankCode)) {
                    var customBankListService: CustomBankListService = new CustomBankListService();
                    customBankListService.getAll().subscribe((response: ServiceResponse) => {
                        let allCustomBankList: CustomBankList[] = response.Result;
                        let bank: CustomBankList = allCustomBankList.filter(d => d.InternalCode == customFileCreditResponseData.BankCode && !d.InActive)[0];
                        if (!AppTool.IsNullOrEmpty(bank)) {
                            this.GetCreditInternalBankId = bank.Id;
                        }
                    });
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
    //AutoFillPaymentScreenByDefault() {
    //    this.NewMethodMethod();
    //    if (this.sumBtl != null && this.sumBtl > 0) {
    //        this.JustAutoFillPaymentScreen();
    //    }
    //    else {

    //        if (!AppTool.IsNullOrEmpty(this.GetCreditInternalBankId)) {
    //            if (this.PaymentMethodsList && this.PaymentMethodsList.Collection) {
    //                this.JustAutoFillPaymentScreen();
    //                return;
    //            }
    //        }

    //        this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CIM_PAYCASH_FIL", "NON", this.DeclarationPM.CustomerCode, SessionLocator.Tenant)
    //            .subscribe((response: ServiceResponse) => {
    //                let obj = response.Result;
    //                if (obj) {
    //                    let DefaultValue = obj['DefaultValue'];
    //                    if (!AppTool.IsNullOrEmpty(DefaultValue)) {
    //                        if (this.PaymentMethodsList && this.PaymentMethodsList.Collection) {
    //                            this.JustAutoFillPaymentScreenCash(DefaultValue);
    //                        }
    //                    }
    //                    else {
    //                        this.AutoFillPaymentScreen();
    //                    }
    //                }
    //            });


    //    }


    //}
    //AutoFillPaymentScreen() {
    //    if (this.isAutoFill)
    //        this.JustAutoFillPaymentScreen();
    //    //this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_PAYHAND_FIL", "NON", "NON", SessionLocator.Tenant)
    //    //    .subscribe(
    //    //        (response: ServiceResponse) => {

    //    //            let obj = response.Result;
    //    //            if (obj) {
    //    //                let DefaultValue = obj['DefaultValue'];

    //    //                if (DefaultValue == "A") {//==AutoFillPaymentScreen //if (customsSetting.AutoFillPaymentScreen) {
    //    //                    if (this.PaymentMethodsList && this.PaymentMethodsList.Collection) {
    //    //                        var MinAndMax;
    //    //                        this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_PAY_AMT_RNG", "NON", "NON", SessionLocator.Tenant)
    //    //                            .subscribe(
    //    //                                (response: ServiceResponse) => {
    //    //                                    let obj = response.Result;
    //    //                                    if (obj) {
    //    //                                        var CGG_PAY_AMT_RNGDefault: string = obj['DefaultValue'];
    //    //                                        if (CGG_PAY_AMT_RNGDefault != "" && CGG_PAY_AMT_RNGDefault != null) {
    //    //                                            let MinAndMax = CGG_PAY_AMT_RNGDefault.split("-");
    //    //                                            let min = parseFloat(MinAndMax[0].replace(/,/g, ''));
    //    //                                            let max = parseFloat(MinAndMax[1].replace(/,/g, ''));
    //    //                                            if (min < this.TotalTax && max > this.TotalTax) {
    //    //                                                this.BetweenMinAndMax = true;
    //    //                                            }
    //    //                                        }
    //    //                                    }
    //    //                                    this.JustAutoFillPaymentScreen();
    //    //                                });

    //    //                    }
    //    //                }

    //    //            }
    //    //        });
    //}


    //JustAutoFillPaymentScreen() {

    //    if (this.sumBtl != null && this.sumBtl > 0) {
    //        for (let method of this.PaymentMethodsList.Collection) {
    //            method.Amount = this.DeclarationPM.TotalTax - this.sumBtl;
    //            method.MethodTypeCode = "1";
    //            this.paymentMethodTypeListService.getSingleFromCache("1").subscribe((response: ServiceResponse) => {
    //                method.MethodTypeName = response.Result.LocalName;
    //            });
    //        }
    //        this.NewMethodMethod(true);
    //        this.PaymentMethodsList.Collection[this.PaymentMethodsList.Collection.length - 1].Amount = this.sumBtl;


    //        //     this.PaymentMethodsList.Collection.push(method);

    //    }
    //    else {
    //        for (let method of this.PaymentMethodsList.Collection) {
    //            method.Amount = this.DeclarationPM.TotalTax;
    //            method.MethodTypeCode = "1";
    //            this.paymentMethodTypeListService.getSingleFromCache("1").subscribe((response: ServiceResponse) => {
    //                method.MethodTypeName = response.Result.LocalName;
    //            });
    //        }

    //        if (!AppTool.IsNullOrEmpty(this.paymentMethodModelMax)) {
    //            this.paymentMethodModelMax.Amount = this.DeclarationPM.TotalTax;
    //            this.paymentMethodModelMax.MethodTypeCode = "1";
    //            this.paymentMethodTypeListService.getSingleFromCache("1").subscribe((response: ServiceResponse) => {
    //                this.paymentMethodModelMax.MethodTypeName = response.Result.LocalName;
    //            });
    //        }
    //    }
    //}


    //JustAutoFillPaymentScreenCash(defaultValue: string) {

    //    if (this.sumBtl != null && this.sumBtl > 0) {
    //        for (let method of this.PaymentMethodsList.Collection) {
    //            method.Amount = this.DeclarationPM.TotalTax - this.sumBtl;
    //            method.MethodTypeCode = "2";
    //            this.paymentMethodTypeListService.getSingleFromCache("2").subscribe((response: ServiceResponse) => {
    //                method.MethodTypeName = response.Result.LocalName;
    //            });
    //            method.PayerActivityTypeCode = defaultValue;
    //            this.customerActivityTypeListService.getSingleFromCache(defaultValue).subscribe((response: ServiceResponse) => {
    //                method.PayerActivityTypeName = response.Result.LocalName;
    //            });
    //        }
    //        this.NewMethodMethod();
    //        this.PaymentMethodsList.Collection[this.PaymentMethodsList.Collection.length - 1].Amount = this.sumBtl;

    //    }
    //    else {
    //        for (let method of this.PaymentMethodsList.Collection) {
    //            method.Amount = this.DeclarationPM.TotalTax;
    //            method.MethodTypeCode = "2";
    //            this.paymentMethodTypeListService.getSingleFromCache("2").subscribe((response: ServiceResponse) => {
    //                method.MethodTypeName = response.Result.LocalName;
    //            });
    //            method.PayerActivityTypeCode = defaultValue;
    //            this.customerActivityTypeListService.getSingleFromCache(defaultValue).subscribe((response: ServiceResponse) => {
    //                method.PayerActivityTypeName = response.Result.LocalName;
    //            });
    //        }

    //        if (!AppTool.IsNullOrEmpty(this.paymentMethodModelMax)) {
    //            this.paymentMethodModelMax.Amount = this.DeclarationPM.TotalTax;
    //            this.paymentMethodModelMax.MethodTypeCode = "2";
    //            this.paymentMethodTypeListService.getSingleFromCache("2").subscribe((response: ServiceResponse) => {
    //                this.paymentMethodModelMax.MethodTypeName = response.Result.LocalName;
    //            });
    //            this.paymentMethodModelMax.PayerActivityTypeCode = defaultValue;
    //            this.customerActivityTypeListService.getSingleFromCache(defaultValue).subscribe((response: ServiceResponse) => {
    //                this.paymentMethodModelMax.PayerActivityTypeName = response.Result.LocalName;
    //            });
    //        }
    //    }
    //}

    //OldAutoFillPaymentScreen() {
    //    this.customsSettingListService.getAll().subscribe((response: ServiceResponse) => {
    //        var list = response.Result;
    //        console.log("[response/customsSettingListService.getAll]", list);
    //        if (!AppTool.IsNullOrEmpty(list)) {
    //            var customsSetting = list[0];
    //            this.NewMethodMethod();

    //            if (!AppTool.IsNullOrEmpty(customsSetting)) {
    //                if (customsSetting.AutoFillPaymentScreen) {

    //                    this.JustAutoFillPaymentScreen();

    //                }
    //            }
    //        }
    //    });
    //}
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
            this.ErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.ChangedDeclarationExport");
        }
        else {
            this.IsDisplayOnly = false;
            this.OkButtonEnabled = true;
            this.SendButtonEnabled = true;
        }

        if (!AppTool.IsNullOrEmpty(entityPM.IsSubmitDeclaration)) {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.PaidDeclarationExport");
        }

        if (entityPM.DeclarationStatusTypeCode == "11") {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.WaitingApproval");
        }

        /*if (entityPM.DeclarationStatusTypeCode == "10") {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.FuturePayment");
        }*/

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
    }

    //#endregion

    //#region Payment Method
    PaymentMethodMessage: string = "";
    IsPaymentMethodMessageVisible: boolean = false;
    newLine: boolean = false;

    BuildMethods() {

    }

    TotalAmount: number = 0;


    BuildProtest() {

    }

    //#endregion

    //#region Submit Payment
    saving: boolean = false;
    instructionCancelled: boolean = false;

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

        var isPaymentDateValid = this.IsPaymentDateValid();
        var isBlockTime = false;
        if (!isPaymentDateValid) {

            this.ValidationErrorsList = [];

            if (!isPaymentDateValid)
                this.ValidationErrorsList.push("לא ניתן להזין תאריך בעבר");

        }
        else {
            /*this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_PAY_BLK_RNG", "NON", "NON", SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
                let obj = response.Result;
               if (obj) {
                   let timeCompany = obj['DefaultValue'];
                    timeCompany = "12:00 - 13:00";
                    this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CIM_PAY_BLK_RNG", "NON", this.DeclarationPM.CustomerCode, SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
                        let obj = response.Result;
                        if (obj) {
                            let timeCustomer = obj['DefaultValue'];
                            //   timeCustomer = "20:00 - 22:00";
                            if (AppTool.IsNullOrEmpty(timeCompany) && AppTool.IsNullOrEmpty(timeCustomer)) {
                                isBlockTime = false;
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

                            if (!isBlockTime) {
                                this.ValidationErrorsList = [];
                                this.ActivateUnifreightInstructionOK();
                            }

                        }

                    });
                }
            });*/
            this.ValidationErrorsList = [];
            this.ActivateUnifreightInstructionOK();
        }

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

        if (this.ValidationErrorsList.length > 0) return;

        var isPaymentDateValid = this.IsPaymentDateValid();
        if (isPaymentDateValid) {
            this.ValidationErrorsList = [];
        }
        else {
            this.ValidationErrorsList = [];
            if (!isPaymentDateValid)
                this.ValidationErrorsList.push("לא ניתן להזין תאריך בעבר");
        }

        if (this.ValidationErrorsList.length > 0) return;

        var isBlockTime = false;


        /* this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_PAY_BLK_RNG", "NON", "NON", SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
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
 
                         if (!isBlockTime) {
                             // Validate payment date with future date
 
                             this.PaymentDate = new Date(Date.parse(this.PaymentDate + "")); // sometimes this variable contains string value of date, so convert it to date
 
                             var paymentDate = new Date(this.PaymentDate.getFullYear(), this.PaymentDate.getMonth(), this.PaymentDate.getDate(), 0, 0, 0);
                             this.SendMethodStep1();
                         }
                         else {
                             this.SendMethodStep1();
                         }
                     }
                 });
             }
         });*/
        this.PaymentDate = new Date(Date.parse(this.PaymentDate + "")); // sometimes this variable contains string value of date, so convert it to date
        var paymentDate = new Date(this.PaymentDate.getFullYear(), this.PaymentDate.getMonth(), this.PaymentDate.getDate(), 0, 0, 0);
        this.SendMethodStep1();

    }

    SendMethodStep1() {
        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));

        if (this.entityCreated) {
            this.declarationPaymentPMService.insert(this.paymentPM).subscribe((response: ServiceResponse) => {
                var result = response.Result;
                console.log("[Response] declarationPaymentPMService.insert ", result);
                this.paymentPM = result;
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
                    if (FeatureLocator.HasFeaturePermession("Customs.Declaration", "EscapeSign")) {
                        this.SendMethod();
                    }
                    else {
                        //If Last Declaration was NOT Signed
                        //if (!this.DeclarationPM.IsSignedVersion) {  ///If IsCourierDeclaration= false, Check if Last Declaration Signed (IsSignedVersion.Declaration = True) , if NOT   - WI 18211
                        if (!this.DeclarationPM.IsCourierDeclaration && !this.DeclarationPM.IsSignedVersion) {

                            this.CheckBeforeSendPaymentOrder();
                        }
                        else {
                            this.SendMethod();
                        }
                    }
                } else {
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
            if (this.DeclarationPM.IsCourierDeclaration) {
                this.OnlySendPayment(params);
            }
            else {
         this.CheckCustomFileCreditThenSendPayment(params);
            }
            return;
        }

    }
    CheckCustomFileCreditThenSendPayment(params: CustomFileCreditRequestParams) {
        var myCustomMessageProgressHelper = new CustomMessageProgressHelper(this.CurrentSession);
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(params.PBId, 5, true);

        //this.declarationMessagesService.PostCheckCustomFileCreditOnly(params)
        //    .subscribe((myServiceResponse: ServiceResponse) => {
        //        myCustomMessageProgressHelper.MessageArrived = true;
        //        SessionLocator.SelectedSession.StopBusyIndicator();
        //        let customFileCreditResponseData: CustomFileCreditResponseData = myServiceResponse.Result;

        //        if (!AppTool.IsNullOrEmpty(customFileCreditResponseData)) {
        //            if (customFileCreditResponseData.IsTRansGove) {
        //                var confirmWindow = new ConfirmWindow();
        //                confirmWindow.Show(customFileCreditResponseData.UserMessage);
        //                SessionLocator.SelectedSession.StopBusyIndicator();
        //                confirmWindow.WindowClosed.subscribe((event: any) => {
        //                    if (confirmWindow.Yes) {
        //                        //this.ActualSendToTransfer();
        //                        this.InstructionActualSendToTransfer()
        //                    }
        //                });
        //                return;
        //            }
        //            if (customFileCreditResponseData.IsReTRansGove) {
        //                var confirmWindow = new ConfirmWindow();
        //                //confirmWindow.Width = 400;
        //                confirmWindow.Show(customFileCreditResponseData.UserMessage);
        //                SessionLocator.SelectedSession.StopBusyIndicator();
        //                confirmWindow.WindowClosed.subscribe((event: any) => {
        //                    if (confirmWindow.Yes) {
        //                        this.ActualSendToReTransfer();
        //                    }
        //                });
        //                return;
        //            }
        //            if (customFileCreditResponseData.HasException) {
        //                var messageWindow = new MessageWindow();
        //                messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
        //                messageWindow.Width = 250;
        //                messageWindow.Height = 150;
        //                messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        //                messageWindow.Show(customFileCreditResponseData.UserMessage);
        //                return;
        //                //////////////////////////////////////////////////////////////////////////
        //            }


               

        //        }
        //    });


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

    OnlySendPayment(params: CustomFileCreditRequestParams) {

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
        CustomMessageProgressComponent.ShowProgressBar(this.CurrentSession, params.PBId, "תחילת שליחה למכס- הגשת תשלום", false, myShowProgressBarParams).then(res => {
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
        if (this.DeclarationPM.DeclarationTypeCode == "2") {
            this.declarationMessagesService.PostSendExportPaymentOnly(params)
                .subscribe(res1 => {
                });
        } else {
            this.declarationMessagesService.PostSendPaymentOnly(params)
                .subscribe(res1 => {
                });
        }

        SessionLocator.SelectedSession.CloseCurrentWindow();

    }

    InstructionActualSendToTransfer() {
        let myUnifreightInstructionController = new UnifreightInstructionController(this.DeclarationPM, "COLLECT_TRANSFER");
        myUnifreightInstructionController
            .ShowInstruction(
                () => {
                    console.log("Instruction return - continue TransferToCollectorMethod");
                    this.ActualSendToTransfer();
                },
                () => { console.log("Instruction return - do not continue 2 TransferToCollectorMethod!!"); }
            );
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

        //CustomMessageProgressComponent.ShowProgressBar(this.CurrentSession,params.PBId, "שליחת בקשת העברה לגובה", false).then((res) => {
        //    console.log("[Send] Response/ShowProgressBar : ", res);

        //}).catch((err) => {
        //    this.ValidationErrorsList = [];
        //    this.ValidationErrorsList.push(err);
        //});
        var myCustomMessageProgressHelper = new CustomMessageProgressHelper(this.CurrentSession);
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

        CustomMessageProgressComponent.ShowProgressBar(this.CurrentSession, params.PBId, "שליחת בקשת העברה חוזרת לגובה", true).then((res) => {
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
                        //this.ActualSendToTransfer();
                        this.InstructionActualSendToTransfer();
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


