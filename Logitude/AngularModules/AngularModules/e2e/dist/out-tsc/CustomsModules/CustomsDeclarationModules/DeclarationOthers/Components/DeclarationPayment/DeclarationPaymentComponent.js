"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var DeclarationPaymentMethodPM_1 = require("../../../../../Customs/EntityPMs/DeclarationPaymentMethodPM");
var DeclarationPaymentProtestPM_1 = require("../../../../../Customs/EntityPMs/DeclarationPaymentProtestPM");
var DeclarationPaymentPM_1 = require("../../../../../Customs/EntityPMs/DeclarationPaymentPM");
var DeclarationEventManager_1 = require("../../../../../Customs/Utilities/DeclarationEventManager");
var DeclarationDisplayOnlyChecks_1 = require("../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks");
var AmitalGatewayUtil_1 = require("../../../../../Infrastructure/Utilities/AmitalGatewayUtil");
var UnifreightController_1 = require("../../../../../Customs/Controller/UnifreightController");
var CustomMessageProgressComponent_1 = require("../../../../CustomsControls/Components/CustomMessageProgressComponent");
var CustomFileCreditRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/CustomFileCreditRequestParams");
var RequestParamsBase_1 = require("../../../../../Customs/DataContract/RequestParams/RequestParamsBase");
var CustomsRequiredFieldListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService");
// Services
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var UserListService_1 = require("../../../../../Common/Services/StandardLists/UserListService");
var CustomsSettingListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsSettingListService");
var CustomBankListService_1 = require("../../../../../Customs/Services/StandardLists/CustomBankListService");
var CustomBankCardExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/CustomBankCardExtendedPMService");
var PaymentMethodTypeListService_1 = require("../../../../../Customs/Services/StandardLists/PaymentMethodTypeListService");
var CustomerActivityTypeListService_1 = require("../../../../../Customs/Services/StandardLists/CustomerActivityTypeListService");
var IIGGeneralMessagesService_1 = require("../../../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var DeclarationMessagesService_1 = require("../../../../../Customs/Services/WebServices/DeclarationMessagesService");
var DeclarationPaymentPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationPaymentPMService");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var DeclarationPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var CustomsSettingExtendedListService_1 = require("../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService");
var ErrorLogPMFileLoggerService_1 = require("../../../../../Infrastructure/Services/ExtendedPMs/ErrorLogPMFileLoggerService");
var ErrorLogPM_1 = require("../../../../../Infrastructure/EntityPMs/ErrorLogPM");
var DeclarationPaymentComponent = /** @class */ (function (_super) {
    __extends(DeclarationPaymentComponent, _super);
    function DeclarationPaymentComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.DeclarationPayment"; //Customs.Declaration";
        _this.ValidationErrorsList = [];
        _this.ErrorMessage = "";
        _this.entityCreated = false;
        //Services
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService();
        _this.declarationService = new DeclarationPMService_1.DeclarationPMService();
        _this.declarationPaymentPMService = new DeclarationPaymentPMService_1.DeclarationPaymentPMService();
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.userListService = new UserListService_1.UserListService();
        _this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService;
        _this.paymentMethodTypeListService = new PaymentMethodTypeListService_1.PaymentMethodTypeListService();
        _this.customerActivityTypeListService = new CustomerActivityTypeListService_1.CustomerActivityTypeListService();
        _this.declarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        _this._CustomsSettingExtendedListService = new CustomsSettingExtendedListService_1.CustomsSettingExtendedListService();
        _this._2LogBankList = false;
        _this.ClientBankListLogUntilDateyyyyMMdd = "20180820.ClientBankListLogUntilDateyyyyMMdd";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region Display only logic
        _this.IsDisplayOnly = false;
        _this.OkButtonEnabled = true;
        _this.SendButtonEnabled = false;
        _this.DrawMe = true;
        //#endregion
        //#region Payment Method
        _this.PaymentMethodMessage = "";
        _this.IsPaymentMethodMessageVisible = false;
        _this.newLine = false;
        _this.TotalAmount = 0;
        //#endregion
        //#region Submit Payment
        _this.saving = false;
        _this.instructionCancelled = false;
        _this._IsCloseScreen = false;
        _this.PaymentMethodsList = new ObservableCollection_1.ObservableCollection([]);
        _this.PaymentProtestsList = new ObservableCollection_1.ObservableCollection([]);
        _this.SelectedInvoiceItems = new ObservableCollection_1.ObservableCollection([]);
        _this.SelectedInvoices = new ObservableCollection_1.ObservableCollection([]);
        _this._ErrorLogPMFileLoggerService = new ErrorLogPMFileLoggerService_1.ErrorLogPMFileLoggerService();
        _this._ErrorLogPMFileLoggerService.get(_this.ClientBankListLogUntilDateyyyyMMdd)
            .subscribe(function (response) {
            _this._2LogBankList = response.Result.IsLogInOn;
        });
        return _this;
    }
    DeclarationPaymentComponent.prototype.ngOnInit = function () {
    };
    DeclarationPaymentComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.entityResourceService.getEntityResourceByTableName("Customs.PaymentMethodType").subscribe(function (response) {
                _this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                    _this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPayment").subscribe(function (response) {
                        _this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPaymentMethod").subscribe(function (response) {
                            _this.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPaymentProtest").subscribe(function (response) {
                                _this.entityResourceService.getEntityResourceByTableName("Customs.CustomBank").subscribe(function (response) {
                                    _this.DeclarationPM = args.EntityPM;
                                    _this.LoadPayment();
                                    _this.CheckRequrierdFieldsForSend();
                                });
                            });
                        });
                    });
                });
            });
        }
    };
    DeclarationPaymentComponent.prototype.CheckRequrierdFieldsForSend = function () {
        var _this = this;
        var customsRequiredFieldListService = new CustomsRequiredFieldListService_1.CustomsRequiredFieldListService();
        var table = window.ObjectTables.filter(function (d) { return d.Name == 'Customs.DeclarationPayment'; })[0];
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");
        customsRequiredFieldListService.getAllFromCache(filters).subscribe(function (response) {
            var requiredFields = response.Result;
            requiredFields.forEach(function (field) {
                var objectField = window.ObjectFields.filter(function (d) { return d.Id == field.ObjectfieldId; })[0];
                _this.UIProperties.SetWarning(objectField.FieldName, 'Customs.DeclarationPayment', true);
            });
        });
    };
    DeclarationPaymentComponent.prototype.FillGridsData = function () {
        // PaymentMethods List
        this.PaymentMethodsList = new ObservableCollection_1.ObservableCollection([]);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.paymentPM)) {
            for (var _i = 0, _a = this.paymentPM.DeclarationPaymentMethods; _i < _a.length; _i++) {
                var item = _a[_i];
                this.PaymentMethodsList.Insert(new PaymentMethodModel(item, this));
            }
        }
        // Protests List
        this.PaymentProtestsList = new ObservableCollection_1.ObservableCollection([]);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.paymentPM)) {
            for (var _b = 0, _c = this.paymentPM.DeclarationPaymentProtests; _b < _c.length; _b++) {
                var item = _c[_b];
                this.PaymentProtestsList.Insert(new PaymentProtestModel(item, this));
            }
        }
        this.CalculateTotalAmount();
    };
    Object.defineProperty(DeclarationPaymentComponent.prototype, "PaymentDate", {
        //#region Properties
        get: function () {
            return this.paymentPM.PaymentDate;
        },
        set: function (newValue) {
            this.paymentPM.PaymentDate = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationPaymentComponent.prototype, "TotalTax", {
        get: function () { return this.DeclarationPM.TotalTax; },
        set: function (newValue) {
            this.DeclarationPM.TotalTax = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationPaymentComponent.prototype, "ProcessADescription", {
        get: function () { return this.paymentPM.ProcessADescription; },
        set: function (newValue) {
            this.paymentPM.ProcessADescription = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationPaymentComponent.prototype, "IsProcessA", {
        get: function () { return this.paymentPM.IsProcessA == null ? false : this.paymentPM.IsProcessA; },
        set: function (newValue) {
            this.paymentPM.IsProcessA = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationPaymentComponent.prototype, "SignatoryIdentification", {
        get: function () { return this.paymentPM.SignatoryIdentification; },
        set: function (newValue) {
            this.paymentPM.SignatoryIdentification = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationPaymentComponent.prototype, "CreatedByUserId", {
        get: function () { return this.paymentPM.CreatedByUserId; },
        set: function (newValue) {
            this.paymentPM.CreatedByUserId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationPaymentComponent.prototype, "FuturePaymentDateTime", {
        get: function () { return this.paymentPM.FuturePaymentDateTime; },
        set: function (newValue) {
            this.paymentPM.FuturePaymentDateTime = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationPaymentComponent.prototype, "FuturePaymentTime", {
        get: function () { return this._FuturePaymentTime; },
        set: function (newValue) {
            if (newValue) {
                var date = this.FuturePaymentDateTime;
                if (this.paymentPM.FuturePaymentDateTime && typeof (this.paymentPM.FuturePaymentDateTime) == 'string') {
                    date = this.GetDateFromString(this.paymentPM.FuturePaymentDateTime);
                }
                // var date = new Date(Date.parse(this.paymentPM.FuturePaymentDateTime + "")); // sometimes this variable contains string value of date, so convert it to date
                //else
                //    var date = this.GetTodaysDate();// new Date();
                var datetime = this.GetDate(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), newValue.getUTCHours(), newValue.getUTCMinutes(), newValue.getUTCSeconds()); //new Date(date.getFullYear(), date.getMonth(), date.getDate(), newValue.getHours(), newValue.getMinutes(), newValue.getSeconds());
                this.FuturePaymentDateTime = datetime;
                this._FuturePaymentTime = datetime;
            }
            else {
                this._FuturePaymentTime = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationPaymentComponent.prototype, "GetCreditInternalBankId", {
        get: function () { return this._GetCreditInternalBankId; },
        set: function (value) {
            this._GetCreditInternalBankId = value;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    DeclarationPaymentComponent.prototype.GetTodaysDate = function () {
        var today = new Date();
        today.setUTCFullYear(today.getFullYear());
        today.setUTCMonth(today.getMonth());
        today.setUTCDate(today.getDate());
        today.setUTCHours(today.getHours());
        today.setUTCMinutes(today.getMinutes());
        today.setUTCSeconds(today.getSeconds());
        today.setUTCMilliseconds(0);
        return today;
    };
    DeclarationPaymentComponent.prototype.GetDateFromString = function (datestring) {
        //2016/08/14 05:00:00
        //2016-08-14T05:00:00
        //2016/08/14 05:00:00 PM
        //console.log("this is the date string that arrived " + datestring);
        var dateAndTime;
        var suffix;
        if (datestring.indexOf('T') > -1) {
            dateAndTime = datestring.split('T');
        }
        else {
            dateAndTime = datestring.split(' ');
        }
        var dateArray;
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
        var timeArray;
        var hour = 0;
        var minute = 0;
        var second = 0;
        if (dateAndTime.length >= 2) {
            if (dateAndTime[1].indexOf('.') > -1) {
                timeArray = dateAndTime[1].split('.')[0].split(':');
            }
            else {
                timeArray = dateAndTime[1].split(':');
            }
            var hour = this.GetTimeFor24Mode(Number(timeArray[0]), suffix);
            var minute = Number(timeArray[1]);
            var second = Number(timeArray[2].substring(0, 2));
        }
        var year = Number(dateArray[0]);
        var month = Number(dateArray[1]) - 1;
        var day = Number(dateArray[2]);
        var date = this.GetDate(year, month, day, hour, minute, second);
        return date;
    };
    DeclarationPaymentComponent.prototype.GetTimeFor24Mode = function (hours, suffix) {
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
    };
    DeclarationPaymentComponent.prototype.GetDate = function (year, month, day, hour, minute, second) {
        var date = new Date();
        date.setUTCDate(1);
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(hour);
        date.setUTCMinutes(minute);
        date.setUTCSeconds(second);
        date.setUTCMilliseconds(0);
        return date;
    };
    DeclarationPaymentComponent.prototype.LoadPayment = function () {
        var _this = this;
        //if (!dontPerformCheckEnabled) {
        //    CheckEnable();
        //}
        //else {
        this.declarationWebService.GetSingleDeclarationPaymentPMandDefaultExplain(this.DeclarationPM.Id, this.DeclarationPM.CustomerCode)
            .subscribe(function (response) {
            console.log("[response] GetSingleDeclarationPaymentPMandDefaultExplain: ", response);
            var result = response.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                _this.paymentPM = result;
            }
            _this.loadPaymentCompleted();
        });
        //dontPerformCheckEnabled = false;
        //}
    };
    DeclarationPaymentComponent.prototype.loadPaymentCompleted = function () {
        this.FillGridsData();
        // create new entity if there is no payment
        if (Tools_1.AppTool.IsNullOrEmpty(this.paymentPM)) {
            this.paymentPM = new DeclarationPaymentPM_1.DeclarationPaymentPM();
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.paymentPM) && Tools_1.AppTool.IsNullOrEmpty(this.paymentPM.DeclarationId)) {
            this.paymentPM.DeclarationId = this.DeclarationPM.Id;
            this.paymentPM.Tenant = this.DeclarationPM.Tenant;
            this.entityCreated = true;
        }
        this.PostSendCreditToGetBank();
        if (this.paymentPM.FuturePaymentDateTime) {
            //this.FuturePaymentTime = new Date(Date.parse(this.paymentPM.FuturePaymentDateTime + "")); // sometimes this variable contains string value of date, so convert it to date
            this.FuturePaymentTime = Tools_1.DateTool.GetDateParts(this.paymentPM.FuturePaymentDateTime).DateObject;
            var datetimeParts = Tools_1.DateTool.GetDateParts(this.paymentPM.FuturePaymentDateTime);
            var stringOfYear = Tools_1.AppTool.PadLeft("" + datetimeParts.Year, 4, '0');
            var stringOfMonth = Tools_1.AppTool.PadLeft("" + datetimeParts.Month, 2, '0');
            var stringOfDay = Tools_1.AppTool.PadLeft("" + datetimeParts.Day, 2, '0');
            var stringOfHours = Tools_1.AppTool.PadLeft("" + datetimeParts.Hours, 2, '0');
            var stringOfHours12 = Tools_1.AppTool.PadLeft("" + datetimeParts.Hours12, 2, '0');
            var stringOfMinutes = Tools_1.AppTool.PadLeft("" + datetimeParts.Minutes, 2, '0');
            var stringOfSeconds = Tools_1.AppTool.PadLeft("" + datetimeParts.Seconds, 2, '0');
            var stringOfMilliseconds = Tools_1.AppTool.PadLeft("" + datetimeParts.Milliseconds, 3, '0');
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
        if (!Tools_1.AppTool.IsNullOrEmpty(this.paymentPM.CustomsAgentExplanationDefault)) {
            this._PayWithProtest_Default = this.paymentPM.CustomsAgentExplanationDefault;
            if (Tools_1.AppTool.IsNullOrEmpty(this.paymentPM.DeclarationPaymentProtests) || this.paymentPM.DeclarationPaymentProtests.length == 0) {
                this.NewProtestMethod();
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(this.paymentPM.DeclarationPaymentProtests[0].CustomsAgentExplanation)) {
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
    };
    DeclarationPaymentComponent.prototype.PostSendCreditToGetBank = function () {
        var _this = this;
        if (this.DeclarationPM.PaymentDate) {
            return;
        }
        var objecttable = window.ObjectTables.filter(function (d) { return d.Name == "Customs.Declaration"; })[0];
        var searchParams = new CustomFileCreditRequestParams_1.CustomFileCreditRequestParams();
        {
            searchParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
            searchParams.AppicationId = this.DeclarationPM.Id; //this.entityParent.DeclarationId;
            searchParams.LoggingEnabled = true;
            searchParams.LoggingEntityId = this.DeclarationPM.Id; //entityParent.DeclarationId;
            searchParams.LoggingObjectTableId = objecttable.Id;
            searchParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            searchParams.RequestName = "Send Credit to Get Bank Request";
            searchParams.ResponseName = "Get Credit to Get Bank Response";
            searchParams.Mode = "GetBank";
        }
        searchParams.RequestVIA = RequestParamsBase_1.SendRequestVIA.Default;
        var myIIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        this.CurrentSession.StartBusyIndicator("");
        myIIGGeneralMessagesService.PostCustomFileCredit(searchParams)
            .subscribe(function (myServiceResponse) {
            var customFileCreditResponseData = myServiceResponse.Result;
            var newDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            var currentDate = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate(), newDate.getHours(), newDate.getMinutes(), 0); // last of today
            var paymentDateTime = Tools_1.DateTool.GetDateFromDate(customFileCreditResponseData.PaymentDateTime);
            if (customFileCreditResponseData.PaymentDateTime == null) {
                customFileCreditResponseData.PaymentDateTime = currentDate;
            }
            if (paymentDateTime <= currentDate) {
                _this.PaymentDate = customFileCreditResponseData.PaymentDateTime;
                _this.FuturePaymentDateTime = null;
                _this.FuturePaymentTime = null;
            }
            else {
                _this.PaymentDate = currentDate;
                _this.FuturePaymentDateTime = customFileCreditResponseData.PaymentDateTime;
                _this.FuturePaymentTime = Tools_1.DateTool.GetDateParts(customFileCreditResponseData.PaymentDateTime).DateObject;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(customFileCreditResponseData.BankCode)) {
                var customBankListService = new CustomBankListService_1.CustomBankListService();
                customBankListService.getAll().subscribe(function (response) {
                    var allCustomBankList = response.Result;
                    var bank = allCustomBankList.filter(function (d) { return d.InternalCode == customFileCreditResponseData.BankCode && !d.InActive; })[0];
                    if (!Tools_1.AppTool.IsNullOrEmpty(bank)) {
                        _this.GetCreditInternalBankId = bank.Id;
                        //if (this.PaymentMethodsList && this.PaymentMethodsList.Collection) {
                        //    this.JustAutoFillPaymentScreen();
                        //}
                        if (Tools_1.AppTool.IsNullOrEmpty(_this.paymentPM.DeclarationPaymentMethods) || _this.paymentPM.DeclarationPaymentMethods.length == 0) {
                            _this.AutoFillPaymentScreenByDefault();
                        }
                    }
                });
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(_this.paymentPM.DeclarationPaymentMethods) || _this.paymentPM.DeclarationPaymentMethods.length == 0) {
                _this.AutoFillPaymentScreenByDefault();
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    DeclarationPaymentComponent.prototype.FillSignData = function () {
        var _this = this;
        // fill user  if the declaration is not payed
        if (!this.IsDisplayOnly) {
            this.userListService
                //.getSingleFromCache(this.DeclarationPM.SignedByUserId)
                .getSingle(this.DeclarationPM.SignedByUserId)
                .subscribe(function (response) {
                var user = response.Result;
                console.log("[Response] userListService.getSingleFromCache: ", user);
                if (!Tools_1.AppTool.IsNullOrEmpty(user)) {
                    if (user.PersonalId == _this.DeclarationPM.SignerPersonalId) {
                        _this.paymentPM.CreatedByUserId = _this.DeclarationPM.SignedByUserId;
                        _this.CreatedByUserId = _this.DeclarationPM.SignedByUserId;
                    }
                }
            });
        }
        //fill SignatoryIdentification if the declaration is not payed
        if (!this.IsDisplayOnly && this.DeclarationPM.SignerPersonalId) { //if payed -> its display only
            this.paymentPM.SignatoryIdentification = this.DeclarationPM.SignerPersonalId;
        }
    };
    DeclarationPaymentComponent.prototype.AutoFillPaymentScreenByDefault = function () {
        var _this = this;
        this.NewMethodMethod();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.GetCreditInternalBankId)) {
            if (this.PaymentMethodsList && this.PaymentMethodsList.Collection) {
                this.JustAutoFillPaymentScreen();
                return;
            }
        }
        this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CIM_PAYCASH_FIL", "NON", this.DeclarationPM.CustomerCode, SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (response) {
            var obj = response.Result;
            if (obj) {
                var DefaultValue = obj['DefaultValue'];
                if (DefaultValue == "Y") {
                    if (_this.PaymentMethodsList && _this.PaymentMethodsList.Collection) {
                        _this.JustAutoFillPaymentScreenCash();
                    }
                }
                else {
                    _this.AutoFillPaymentScreen();
                }
            }
        });
    };
    DeclarationPaymentComponent.prototype.AutoFillPaymentScreen = function () {
        var _this = this;
        this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_PAYHAND_FIL", "NON", "NON", SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (response) {
            var obj = response.Result;
            if (obj) {
                var DefaultValue = obj['DefaultValue'];
                if (DefaultValue == "A") { //==AutoFillPaymentScreen //if (customsSetting.AutoFillPaymentScreen) {
                    if (_this.PaymentMethodsList && _this.PaymentMethodsList.Collection) {
                        _this.JustAutoFillPaymentScreen();
                    }
                }
            }
        });
    };
    DeclarationPaymentComponent.prototype.JustAutoFillPaymentScreen = function () {
        var _loop_1 = function (method) {
            method.Amount = this_1.DeclarationPM.TotalTax;
            method.MethodTypeCode = "1";
            this_1.paymentMethodTypeListService.getSingleFromCache("1").subscribe(function (response) {
                method.MethodTypeName = response.Result.LocalName;
            });
        };
        var this_1 = this;
        for (var _i = 0, _a = this.PaymentMethodsList.Collection; _i < _a.length; _i++) {
            var method = _a[_i];
            _loop_1(method);
        }
    };
    DeclarationPaymentComponent.prototype.JustAutoFillPaymentScreenCash = function () {
        var _loop_2 = function (method) {
            method.Amount = this_2.DeclarationPM.TotalTax;
            method.MethodTypeCode = "2";
            this_2.paymentMethodTypeListService.getSingleFromCache("2").subscribe(function (response) {
                method.MethodTypeName = response.Result.LocalName;
            });
            method.PayerActivityTypeCode = "0";
            this_2.customerActivityTypeListService.getSingleFromCache("0").subscribe(function (response) {
                method.PayerActivityTypeName = response.Result.LocalName;
            });
        };
        var this_2 = this;
        for (var _i = 0, _a = this.PaymentMethodsList.Collection; _i < _a.length; _i++) {
            var method = _a[_i];
            _loop_2(method);
        }
    };
    DeclarationPaymentComponent.prototype.OldAutoFillPaymentScreen = function () {
        var _this = this;
        this.customsSettingListService.getAll().subscribe(function (response) {
            var list = response.Result;
            console.log("[response/customsSettingListService.getAll]", list);
            if (!Tools_1.AppTool.IsNullOrEmpty(list)) {
                var customsSetting = list[0];
                _this.NewMethodMethod();
                if (!Tools_1.AppTool.IsNullOrEmpty(customsSetting)) {
                    if (customsSetting.AutoFillPaymentScreen) {
                        _this.JustAutoFillPaymentScreen();
                    }
                }
            }
        });
    };
    DeclarationPaymentComponent.prototype.CheckRequireds = function () {
    };
    DeclarationPaymentComponent.prototype.CheckTotals = function () {
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
            this.DeclarationPM.DeclarationTaxes.forEach(function (el) { sum += el.TotalAmount; });
            if (totalTax != sum) {
                this.IsDisplayOnly = true;
                this.OkButtonEnabled = false;
                //MessageBorderVisibility = Visibility.Visible;
                this.ErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DifferentTotals");
            }
        }
        else {
            if (totalTax > 0) {
                this.IsDisplayOnly = true;
                this.OkButtonEnabled = false;
                //MessageBorderVisibility = Visibility.Visible;
                this.ErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DifferentTotals");
            }
        }
    };
    DeclarationPaymentComponent.prototype.NewProtestMethod = function () {
        this.NewProtestClicked();
    };
    DeclarationPaymentComponent.prototype.NewMethodMethod = function () {
        this.AddPaymentMethodClicked();
    };
    DeclarationPaymentComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    DeclarationPaymentComponent.prototype.GetDeclarationStatusColor = function (statusCode) {
        var color = "#45494A";
        if (!Tools_1.AppTool.IsNullOrEmpty(statusCode)) {
            switch (statusCode) {
                //case "הוגש, מאושר להתרה":
                //case  "הותר":
                //case "הותר ויצא מאתר בפיקוח המכס":
                //case "טיוטה תקינה, ממתין להגשה":
                case "3":
                case "7":
                case "8":
                case "13":
                    {
                        color = "#009161"; // green
                        break;
                    }
                //case "הוגש, ממתין לאישור אילוץ התרה":
                //case "הוגש, ממתין להחלטת המכס":
                //case "הוגש, ממתין להתרה":
                //case "טיוטה הוגשה לתאריך עתידי":
                //case "טיוטה ממתינה לאישור אילוץ הגשה":
                //case "ממתין לאישור פיצול":
                //case "ממתין לאישור בקשת אחסנה":
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
                //case "סטטוס לא ידוע":
                //case "בוטל":
                //case "הוגש, הצהרה שגויה":
                //case "טיוטה בוטלה":
                //case "טיוטה שגויה":
                //case "יש להגיש את ההצהרה מחדש":
                //case "פוצל":
                //case "אסור ביבוא":
                //case "הסחורה נתפסה":
                //case "הותר - הצהרה שגויה":
                //case "הותר ויצא מאתר בפיקוח מכס - הצהרה שגויה":
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
    };
    DeclarationPaymentComponent.prototype.RefreshScreen = function () {
        var entityPM = this.DeclarationPM;
        if (entityPM.IsChanged) {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ChangedDeclaration");
        }
        else {
            this.IsDisplayOnly = false;
            this.OkButtonEnabled = true;
            this.SendButtonEnabled = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.PaymentDate)) {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.PaidDeclaration");
        }
        if (entityPM.DeclarationStatusTypeCode == "11") {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.WaitingApproval");
        }
        if (entityPM.DeclarationStatusTypeCode == "10") {
            this.IsDisplayOnly = true;
            this.OkButtonEnabled = false;
            this.SendButtonEnabled = false;
            this.ErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.FuturePayment");
        }
        var controller = this.CurrentSession.CurrentEditComponent.EditComponentController;
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
            this.ErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.SubmitDeclarationAgain");
        }
        this.CheckTotals();
        //this.BuildMethods();
        //this.BuildProtest();
        this.DisplayOnlyCheck();
        this.SetScreenFieldsEditability();
    };
    DeclarationPaymentComponent.prototype.DisplayOnlyCheck = function () {
        var _this = this;
        var declarationDisplayOnly = false;
        this.DrawMe = true;
        this.ShowStorageStatusMessage = false;
        //get declaration display only
        declarationDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (declarationDisplayOnly) {
            this.IsDisplayOnly = true;
            this.ErrorMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            this.SetScreenFieldsEditability();
            DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.emit(declarationDisplayOnly);
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
        }
        else {
            this.UIProperties.SetEnabled("FuturePaymentTime", this.ObjectTableName, true);
        }
        //
        // if declaration display only checks changed
        var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.DeclarationPM).subscribe(function (response) {
            var displayOnlyCheckResult = response.Result;
            var declarationDisplayOnly2 = displayOnlyCheckResult.IsDisplayOnly ? true : false;
            if (declarationDisplayOnly2) {
                _this.IsDisplayOnly = true;
                _this.ErrorMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
                _this.IsDisplayOnly = true;
                _this.OkButtonEnabled = false;
                _this.SendButtonEnabled = false;
            }
            else if (_this.DeclarationPM.StorageStatusCode && !_this.ErrorMessage) {
                _this.ShowStorageStatusMessage = true;
                _this.ErrorMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + _this.DeclarationPM.StorageStatusName;
                _this.IsDisplayOnly = false;
            }
            //if is not display only => its not payed , so fill sign data
            if (!_this.IsDisplayOnly)
                _this.FillSignData();
            // set payment date after display only check finshed
            _this.initDates();
            _this.SetScreenFieldsEditability();
            DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.emit(_this.IsDisplayOnly);
            //FuturePaymentTime editablity
            if (_this.IsDisplayOnly) {
                _this.UIProperties.SetEnabled("FuturePaymentTime_timepicker", _this.ObjectTableName, false);
            }
            else {
                _this.UIProperties.SetEnabled("FuturePaymentTime_timepicker", _this.ObjectTableName, true);
            }
        });
    };
    DeclarationPaymentComponent.prototype.SetScreenFieldsEditability = function () {
        var enabled = !this.IsDisplayOnly;
        this.UIProperties.SetEnabled("PaymentDate", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("ProcessADescription", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("IsProcessA", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("SignatoryIdentification", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("CreatedByUserId", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("FuturePaymentDateTime", this.ObjectTableName, enabled);
        this.UIProperties.SetEnabled("FuturePaymentTime", this.ObjectTableName, enabled);
    };
    DeclarationPaymentComponent.prototype.AddPaymentMethodClicked = function () {
        this.newLine = true;
        var line = 0;
        var seq = 0;
        var method = null;
        if (this.paymentPM.DeclarationPaymentMethods.length == 1) {
            method = this.paymentPM.DeclarationPaymentMethods.find(function (d) { return d.PayerActivityTypeCode == null || d.MethodTypeCode == null || d.Amount == null; });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(method)) {
            this.IsPaymentMethodMessageVisible = true;
            this.PaymentMethodMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.PaymentMethodFields");
        }
        else {
            this.IsPaymentMethodMessageVisible = false;
            this.PaymentMethodMessage = "";
            if (this.paymentPM.DeclarationPaymentMethods.length > 0) {
                var line = this.paymentPM.DeclarationPaymentMethods.reduce(function (prev, current) { return (prev.Line > current.Line) ? prev : current; }).Line;
                var seq = this.paymentPM.DeclarationPaymentMethods.reduce(function (prev, current) { return (prev.SequenceNumeric > current.SequenceNumeric) ? prev : current; }).SequenceNumeric;
            }
            line++;
            seq++;
            var item = new DeclarationPaymentMethodPM_1.DeclarationPaymentMethodPM(this.paymentPM);
            item.Tenant = SessionLocator_1.SessionLocator.Tenant;
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
    };
    DeclarationPaymentComponent.prototype.RemovePaymentMethodClicked = function (item) {
        if (this.IsDisplayOnly)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.PaymentMethodsList.Remove(item);
            this.paymentPM.RemoveDeclarationPaymentMethod(item.methodPM);
        }
    };
    DeclarationPaymentComponent.prototype.BuildMethods = function () {
    };
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
    DeclarationPaymentComponent.prototype.CalculateTotalAmount = function () {
        var total = 0;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PaymentMethodsList)) {
            this.PaymentMethodsList.Collection.forEach(function (method) {
                total += Tools_1.AppTool.IsNullOrEmpty(method.Amount) ? 0 : method.Amount;
            });
        }
        this.TotalAmount = total;
    };
    //#endregion
    //#region Payment Protest
    DeclarationPaymentComponent.prototype.NewProtestClicked = function () {
        if (this.IsDisplayOnly)
            return;
        var line = 0;
        if (this.paymentPM.DeclarationPaymentProtests.length > 0) {
            var line = this.paymentPM.DeclarationPaymentMethods.reduce(function (prev, current) { return (prev.Line > current.Line) ? prev : current; }).Line;
        }
        line++;
        var protest = new DeclarationPaymentProtestPM_1.DeclarationPaymentProtestPM(this.paymentPM);
        protest.Tenant = SessionLocator_1.SessionLocator.Tenant;
        protest.DeclarationId = this.DeclarationPM.Id;
        protest.Line = line;
        protest.CustomsAgentExplanation = this._PayWithProtest_Default;
        if (!this.paymentPM.DeclarationPaymentProtests.includes(protest)) {
            this.paymentPM.DeclarationPaymentProtests.push(protest);
        }
        var item = new PaymentProtestModel(protest, this);
        this.PaymentProtestsList.Insert(item);
        this.BuildProtest();
    };
    DeclarationPaymentComponent.prototype.DeleteProtestClicked = function (item) {
        if (item) {
            this.paymentPM.RemoveDeclarationPaymentProtest(item.protestPM);
            this.PaymentProtestsList.Remove(item);
        }
    };
    DeclarationPaymentComponent.prototype.BuildProtest = function () {
    };
    DeclarationPaymentComponent.prototype.FuturePaymentDateTimeOnBlur = function (event) {
        // WI 32593
        this.IsFuturePaymentDateValid();
    };
    DeclarationPaymentComponent.prototype.IsFuturePaymentDateValid = function () {
        if (this.FuturePaymentDateTime) {
            var newDate = new Date();
            var currentDate = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate(), 0, 0, 0);
            if (this.FuturePaymentDateTime < currentDate) {
                this.UIProperties.SetValidity("FuturePaymentDateTime", "Customs.DeclarationPayment", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.futuredatecantbepast"));
                return false;
            }
            else {
                this.UIProperties.SetValidity("FuturePaymentDateTime", "Customs.DeclarationPayment", true, "");
                return true;
            }
        }
        else // no date entered
         {
            this.UIProperties.SetValidity("FuturePaymentDateTime", "Customs.DeclarationPayment", true, "");
            return true;
        }
    };
    DeclarationPaymentComponent.prototype.PaymentDateTimeOnBlur = function (event) {
        this.IsPaymentDateValid();
    };
    DeclarationPaymentComponent.prototype.IsPaymentDateValid = function () {
        if (this.PaymentDate) {
            //var newDate = new Date();
            var newDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            var currentDate = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate(), 0, 0, 0); // last of today
            if (this.PaymentDate < currentDate) {
                this.UIProperties.SetValidity("PaymentDate", "Customs.DeclarationPayment", false, "לא ניתן להזין תאריך בעבר");
                return false;
            }
            else {
                this.UIProperties.SetValidity("PaymentDate", "Customs.DeclarationPayment", true, "");
                return true;
            }
        }
        else // no date entered
         {
            this.UIProperties.SetValidity("PaymentDate", "Customs.DeclarationPayment", true, "");
            return true;
        }
    };
    DeclarationPaymentComponent.prototype.OkButtonClicked = function () {
        if (!this.FuturePaymentTime && this.FuturePaymentDateTime) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("הזן זמן עתידי"); // Please enter a future time
            return;
        }
        var isFuturePaymentDateValid = this.IsFuturePaymentDateValid(); // WI 32593
        var isPaymentDateValid = this.IsPaymentDateValid();
        if (isFuturePaymentDateValid && isPaymentDateValid) {
            this.ValidationErrorsList = [];
            this.ActivateUnifreightInstructionOK();
        }
        else {
            this.ValidationErrorsList = [];
            if (!isFuturePaymentDateValid)
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.futuredatecantbepast"));
            if (!isPaymentDateValid)
                this.ValidationErrorsList.push("לא ניתן להזין תאריך בעבר");
        }
    };
    DeclarationPaymentComponent.prototype.ActivateUnifreightInstructionOK = function () {
        var _this = this;
        //if (!AppTool.IsNullOrEmpty(this.DeclarationPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsDeclarationInUse(this.DeclarationPM.CustomFileNo, this.DeclarationPM.IsConvertedDeclaration, this.DeclarationPM.IsConnectedToUnifreight)) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.UnifreightInstSentMehes"));
            var myStoreViewUnifreightInstructionController = new UnifreightController_1.UnifreightController(this.DeclarationPM, "Logitude.Customs.ViewModels.DeclarationPayment.DeclarationPaymentTabViewModel.MyStoreViewUnifreightInstructionController");
            myStoreViewUnifreightInstructionController.
                //UnifreightCallbackCompleted += (sender, e) => {
                GetPromise().then(function (e) {
                if (e.UnifreightResponseStatus) {
                    _this.SubmitChanges();
                }
                else {
                    _this.instructionCancelled = true;
                    _this.CurrentSession.StopBusyIndicator();
                }
                //myStoreViewUnifreightInstructionController.DisposeUnifreightMassaging();
            });
            myStoreViewUnifreightInstructionController.SendRequestInstructionToUnifreightAsync("PAYHAND_STORE");
        }
        else {
            this.SubmitChanges();
        }
    };
    DeclarationPaymentComponent.prototype.SubmitChanges = function () {
        var _this = this;
        //ActualOk
        if (this.entityCreated) {
            this.declarationPaymentPMService.insert(this.paymentPM).subscribe(function (response) {
                var result = response.Result;
                console.log("[Response] declarationPaymentPMService.insert ", result);
                if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                    _this.SubmitCompleted(response);
                    _this.paymentPM = result;
                }
                _this.entityCreated = false;
            });
        }
        else {
            this.declarationPaymentPMService.update(this.paymentPM).subscribe(function (response) {
                var result = response.Result;
                console.log("[Response] declarationPaymentPMService.insert ", result);
                //if (!AppTool.IsNullOrEmpty(result)) {
                //    this.SubmitCompleted(response);
                //}
                _this.SubmitCompleted(response);
            });
        }
    };
    DeclarationPaymentComponent.prototype.SubmitCompleted = function (response) {
        if (!response.HasError && !this.instructionCancelled) {
            this.saving = true;
            this.RefreshDeclaration();
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            var errors = [];
            Validator_1.Validator.TryValidateObject(this.DeclarationPM, "Customs.Declaration", errors);
            for (var _i = 0, _a = this.paymentPM.DeclarationPaymentMethods; _i < _a.length; _i++) {
                var item = _a[_i];
                Validator_1.Validator.TryValidateObject(item, "Customs.DeclarationPaymentMethod", errors);
            }
            for (var _b = 0, _c = this.paymentPM.DeclarationPaymentProtests; _b < _c.length; _b++) {
                var el = _c[_b];
                Validator_1.Validator.TryValidateObject(el, "Customs.DeclarationPaymentProtest", errors);
            }
            if (errors.length > 0) {
                this.ValidationErrorsList = errors;
            }
        }
    };
    DeclarationPaymentComponent.prototype.RefreshDeclaration = function () {
        var _this = this;
        this.declarationService.get(this.paymentPM.DeclarationId).subscribe(function (res) {
            _this.DeclarationPM = res.Result;
        });
    };
    // Before send
    DeclarationPaymentComponent.prototype.SendButtonClicked = function (event) {
        var _this = this;
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
        var isFuturePaymentDateValid = this.IsFuturePaymentDateValid(); // WI 32593
        var isPaymentDateValid = this.IsPaymentDateValid();
        if (isFuturePaymentDateValid && isPaymentDateValid) {
            this.ValidationErrorsList = [];
        }
        else {
            this.ValidationErrorsList = [];
            if (!isFuturePaymentDateValid)
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.futuredatecantbepast"));
            if (!isPaymentDateValid)
                this.ValidationErrorsList.push("לא ניתן להזין תאריך בעבר");
        }
        //#endregion
        if (this.ValidationErrorsList.length > 0)
            return;
        // Validate payment date with future date
        if (this.FuturePaymentDateTime && this.PaymentDate) {
            this.PaymentDate = new Date(Date.parse(this.PaymentDate + "")); // sometimes this variable contains string value of date, so convert it to date
            this.FuturePaymentDateTime = new Date(Date.parse(this.FuturePaymentDateTime + "")); // sometimes this variable contains string value of date, so convert it to date
            var paymentDate = new Date(this.PaymentDate.getFullYear(), this.PaymentDate.getMonth(), this.PaymentDate.getDate(), 0, 0, 0);
            var futurePaymentDateTime = new Date(this.FuturePaymentDateTime.getFullYear(), this.FuturePaymentDateTime.getMonth(), this.FuturePaymentDateTime.getDate(), 0, 0, 0);
            if (futurePaymentDateTime > paymentDate) {
                //valid
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                confirmWindow.ShowCancelButton = false;
                confirmWindow.ShowNoButton = true;
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.No) {
                        console.log("[!] Send payment canceled");
                        return;
                    }
                    else if (confirmWindow.Yes) {
                        _this.SendMethodStep1();
                    }
                });
                confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.paymentDateSmallerThanFuture"));
            }
            else {
                this.SendMethodStep1();
            }
        }
        else {
            this.SendMethodStep1();
        }
    };
    DeclarationPaymentComponent.prototype.SendMethodStep1 = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Loading"));
        if (this.entityCreated) {
            this.declarationPaymentPMService.insert(this.paymentPM).subscribe(function (response) {
                var result = response.Result;
                console.log("[Response] declarationPaymentPMService.insert ", result);
                _this.paymentPM = result;
                _this.FillGridsData();
                if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                    _this.CheckRequiredFields();
                }
                else {
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
            this.entityCreated = false;
        }
        else {
            this.declarationPaymentPMService.update(this.paymentPM).subscribe(function (response) {
                var result = response.Result;
                console.log("[Response] declarationPaymentPMService.insert ", result);
                _this.paymentPM = result;
                _this.FillGridsData();
                if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                    _this.CheckRequiredFields();
                }
                else {
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    DeclarationPaymentComponent.prototype.CheckRequiredFields = function () {
        var _this = this;
        this.declarationWebService.GetAllRequiredFieldsForDeclarationPayment(this.DeclarationPM.Id).subscribe(function (res) {
            var customsRequiredFieldErrors = res.Result;
            console.log("[response] GetAllRequiredFieldsForDeclarationPayment: ", customsRequiredFieldErrors);
            if (!Tools_1.AppTool.IsNullOrEmpty(customsRequiredFieldErrors)) {
                _this.CurrentSession.StopBusyIndicator();
                if (customsRequiredFieldErrors.RequiredFields.length == 0) {
                    //If Last Declaration was NOT Signed
                    //if (!this.DeclarationPM.IsSignedVersion) {  ///If IsCourierDeclaration= false, Check if Last Declaration Signed (IsSignedVersion.Declaration = True) , if NOT   - WI 18211
                    if (!_this.DeclarationPM.IsCourierDeclaration && !_this.DeclarationPM.IsSignedVersion) {
                        _this.CheckBeforeSendPaymentOrder();
                    }
                    else {
                        _this.SendMethod();
                    }
                }
                else {
                    //this.ValidationErrorsList = [];
                    //customsRequiredFieldErrors.RequiredFields.forEach(el => {
                    //    this.ValidationErrorsList.push(el.CustomMessageError);
                    //});
                    _this.FillValidationErrors(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.RequiredFields"), customsRequiredFieldErrors);
                }
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    DeclarationPaymentComponent.prototype.CheckBeforeSendPaymentOrder = function () {
        var _this = this;
        this.customsSettingListService.getAll().subscribe(function (response) {
            var list = response.Result;
            console.log("[response/customsSettingListService.getAll]", list);
            if (!Tools_1.AppTool.IsNullOrEmpty(list)) {
                var customsSetting = list[0];
                _this.CurrentSession.StopBusyIndicator();
                var listCustomsAgentId = ["550221105", "511487241"];
                if (SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare
                    || (listCustomsAgentId.includes(customsSetting.CustomsAgentId) && new Date() < new Date(2016, 7, 24))) {
                    //Anat Friz unable to sign +2Month
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                    confirmWindow.ShowCancelButton = false;
                    confirmWindow.ShowNoButton = true;
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.SendMethod();
                        }
                    });
                    confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.IsSignedVersionErrorForCustomerCare"));
                }
                else {
                    var msg = new MessageWindow_1.MessageWindow();
                    msg.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                    msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.IsSignedVersionError"));
                }
            }
        });
    };
    // Send
    DeclarationPaymentComponent.prototype.SendMethod = function () {
        var _this = this;
        var totalAmount = Tools_1.AppTool.Round(this.TotalAmount, 2);
        var totalTax = Tools_1.AppTool.Round(this.TotalTax, 2);
        if (Tools_1.AppTool.IsNullOrEmpty(totalTax)) {
            totalTax = 0;
        }
        if (totalAmount != totalTax) {
            var msg = new MessageWindow_1.MessageWindow();
            msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Totalmustbeequaltototaltax"));
            // error not sending
        }
        else {
            //if (AppTool.IsNullOrEmpty(this.DeclarationPM.CustomFileNo) || !AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            if (!AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.IsDeclarationInUse(this.DeclarationPM.CustomFileNo, this.DeclarationPM.IsConvertedDeclaration, this.DeclarationPM.IsConnectedToUnifreight)) {
                this.ActualSend();
            }
            else {
                this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.UnifreightInstSentMehes"));
                var myStoreViewUnifreightInstructionController = new UnifreightController_1.UnifreightController(this.DeclarationPM, "Logitude.Customs.ViewModels.DeclarationPayment.DeclarationPaymentTabViewModel.MyStoreViewUnifreightInstructionController");
                myStoreViewUnifreightInstructionController.GetPromise()
                    //myStoreViewUnifreightInstructionController.UnifreightCallbackCompleted += (sender, e) => {
                    .then(function (e) {
                    if (e.UnifreightResponseStatus) {
                        _this.ActualSend();
                    }
                    else {
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
                this.CurrentSession.StartBusyIndicator("");
                myStoreViewUnifreightInstructionController.SendRequestInstructionToUnifreightAsync("PAYHAND_STORE");
            }
        }
    };
    DeclarationPaymentComponent.prototype.ActualSend = function () {
        if (this.customSendOptions == null) {
            console.log("[!] no send options!!");
            return;
        }
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Customs.Declaration"; })[0];
        var params = new CustomFileCreditRequestParams_1.CustomFileCreditRequestParams();
        params.Tenant = SessionLocator_1.SessionLocator.Tenant;
        params.AppicationId = this.paymentPM.DeclarationId;
        //params.TestCase = SelectedTest;
        params.LoggingEnabled = true;
        params.LoggingEntityId = this.DeclarationPM.Id;
        params.LoggingObjectTableId = ObjectTable.Id;
        params.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        params.RequestName = "send declaration payment request";
        params.ResponseName = "send declaration payment response";
        params.Mode = "Check";
        params.RequestVIA = this.customSendOptions.RequestVIA;
        params.ForcePersonalSign = this.customSendOptions.ForcePersonalSign;
        var splitRequest = true;
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
        //        this.CurrentSession.StopBusyIndicator();
        //        var result: CustomFileCreditResponseData = myServiceResponse.Result;
        //        if (!AppTool.IsNullOrEmpty(result)) {
        //            var mess :string = this.AnalyzeResponseMessageSendPaymentWithCheckCustomFileCredit(result);
        //            this.CurrentSession.StopBusyIndicator();
        //            if (!AppTool.IsNullOrEmpty(mess)) {
        //                let messWindow = new MessageWindow();
        //                this.CurrentSession.StopBusyIndicator();
        //                messWindow.Show(mess);
        //                messWindow.WindowClosed.subscribe((event: any) => {
        //                    if (mess.toLowerCase().includes("succeeded") || mess.toLowerCase().includes("בהצלחה") ||
        //                        this._IsCloseScreen == true) // Mirit 20/07/15 Task-14344 - add successfully (Hebrew) // Mirit 24/11/15 Task 18440- add IsCloseScreen
        //                    {
        //                        this.RefreshDeclaration();
        //                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        //                        if (this.CurrentSession.CurrentWindow != null) this.CurrentSession.CloseCurrentWindow()
        //                    }
        //                });
        //            }
        //        }
        //    });
    };
    //CheckCustomFileCreditThenSendPayment(params: CustomFileCreditRequestParams) {
    //    var myCustomMessageProgressHelper = new CustomMessageProgressHelper();
    //    myCustomMessageProgressHelper.BasicResponse = true;
    //    myCustomMessageProgressHelper.StartProgress(params.PBId, 5, true);
    //    this.declarationMessagesService.PostCheckCustomFileCreditOnly(params)
    //        .subscribe((myServiceResponse: ServiceResponse) => {
    //            myCustomMessageProgressHelper.MessageArrived = true;
    //            this.CurrentSession.StopBusyIndicator();
    //            let customFileCreditResponseData: CustomFileCreditResponseData = myServiceResponse.Result;
    //            if (!AppTool.IsNullOrEmpty(customFileCreditResponseData)) {
    //                if (customFileCreditResponseData.IsTRansGove) {
    //                    var confirmWindow = new ConfirmWindow();
    //                    confirmWindow.Show(customFileCreditResponseData.UserMessage);
    //                    this.CurrentSession.StopBusyIndicator();
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
    //                        this.CurrentSession.StopBusyIndicator();//// let it be ...
    //                        let myPaymentResponseData: CustomFileCreditResponseData = res;
    //                        this.RefreshDeclaration();
    //                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    //                        if (myPaymentResponseData.HasException || !myPaymentResponseData.Succeeded) {
    //                            //let mess = myPaymentResponseData.UserMessage || "Server return Error (Witout message????!!?!)";
    //                            //if (!AppTool.IsNullOrEmpty(mess)) {
    //                            //    let messWindow = new MessageWindow();
    //                            //    messWindow.Show(mess);
    //                            //    messWindow.WindowClosed.subscribe((event: any) => {
    //                            //        this.RefreshDeclaration();
    //                            //        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    //                            //        //if (this.CurrentSession.CurrentWindow != null) this.CurrentSession.CloseCurrentWindow()
    //                            //    });
    //                            //}
    //                        } else {
    //                            if (this.CurrentSession.CurrentWindow != null) this.CurrentSession.CloseCurrentWindow()
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
    DeclarationPaymentComponent.prototype.CheckCustomFileCreditThenSendPayment = function (params) {
        var _this = this;
        var myCustomMessageProgressHelper = new CustomMessageProgressComponent_1.CustomMessageProgressHelper();
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(params.PBId, 5, true);
        this.declarationMessagesService.PostCheckCustomFileCreditOnly(params)
            .subscribe(function (myServiceResponse) {
            myCustomMessageProgressHelper.MessageArrived = true;
            _this.CurrentSession.StopBusyIndicator();
            var customFileCreditResponseData = myServiceResponse.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(customFileCreditResponseData)) {
                if (customFileCreditResponseData.IsTRansGove) {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Show(customFileCreditResponseData.UserMessage);
                    _this.CurrentSession.StopBusyIndicator();
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.ActualSendToTransfer();
                        }
                    });
                    return;
                }
                if (customFileCreditResponseData.IsReTRansGove) {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    //confirmWindow.Width = 400;
                    confirmWindow.Show(customFileCreditResponseData.UserMessage);
                    _this.CurrentSession.StopBusyIndicator();
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.ActualSendToReTransfer();
                        }
                    });
                    return;
                }
                if (customFileCreditResponseData.HasException) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                    messageWindow.Width = 250;
                    messageWindow.Height = 150;
                    messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                    messageWindow.Show(customFileCreditResponseData.UserMessage);
                    return;
                    //////////////////////////////////////////////////////////////////////////
                }
                var myShowProgressBarParams = new CustomMessageProgressComponent_1.ShowProgressBarParams();
                myShowProgressBarParams.OnCloseCustomMessageProgressComponentMethod =
                    function (response) {
                        var myPaymentResponseData = response;
                        if (myPaymentResponseData) {
                            if (myPaymentResponseData.HasException || !myPaymentResponseData.Succeeded) {
                                //do not close Win !!
                            }
                            else {
                                //if OK then  close Win !!
                                if (_this.CurrentSession.CurrentWindow != null)
                                    _this.CurrentSession.CloseCurrentWindow();
                            }
                        }
                    };
                CustomMessageProgressComponent_1.CustomMessageProgressComponent.ShowProgressBar(params.PBId, "תחילת שליחה למכס- הגשת תשלום", false, myShowProgressBarParams).then(function (res) {
                    var ResponseData = res; // this solution to fix the paid declaration not showing a yellow message.
                    if (ResponseData && ResponseData.ContinueProcessInBackground) {
                        _this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                    }
                    else if (_this.Option == 'WB' || _this.Option == 'D') { // work around itzik shall fix the undefined problem.
                        _this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                    }
                    _this.CurrentSession.StopBusyIndicator(); //// let it be ...
                    var myPaymentResponseData = res;
                    _this.RefreshDeclaration();
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                })
                    .catch(function (err) {
                    err = err || "PostSendPaymentOnly return Error (Without message????!!?!)";
                    var messWindow = new MessageWindow_1.MessageWindow();
                    messWindow.Show(err);
                    messWindow.WindowClosed.subscribe(function () {
                        _this.CurrentSession.CloseCurrentWindow();
                    });
                });
                _this.declarationMessagesService.PostSendPaymentOnly(params)
                    .subscribe(function (res1) {
                });
            }
        });
    };
    DeclarationPaymentComponent.prototype.ActualSendToTransfer = function () {
        var _this = this;
        if (this.customSendOptions == null) {
            console.log("[!] no send options!!");
            return;
        }
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Customs.Declaration"; })[0];
        var params = new CustomFileCreditRequestParams_1.CustomFileCreditRequestParams();
        params.Tenant = SessionLocator_1.SessionLocator.Tenant;
        params.AppicationId = this.paymentPM.DeclarationId;
        //params.TestCase = SelectedTest;
        params.LoggingEnabled = true;
        params.LoggingEntityId = this.DeclarationPM.Id;
        params.LoggingObjectTableId = ObjectTable.Id;
        params.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
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
        var myCustomMessageProgressHelper = new CustomMessageProgressComponent_1.CustomMessageProgressHelper();
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(params.PBId, 5, true);
        this.declarationMessagesService.PostSendTransferRequest(params)
            .subscribe(function (myServiceResponse) {
            myCustomMessageProgressHelper.MessageArrived = true;
            _this.CurrentSession.StopBusyIndicator();
            var result = myServiceResponse.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(CustomMessageProgressComponent_1.CustomMessageProgressComponent.CurrCustomMessageProgressHelper)) {
                CustomMessageProgressComponent_1.CustomMessageProgressComponent.CurrCustomMessageProgressHelper.MessageArrived = true;
            }
            _this.CurrentSession.StopBusyIndicator();
            _this.AnalyzeActualSendToTransfer(result);
        });
    };
    DeclarationPaymentComponent.prototype.AnalyzeActualSendToTransfer = function (result) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
            var mess = this.AnalyzeResponseMessage(result);
            if (!Tools_1.AppTool.IsNullOrEmpty(mess)) {
                //_CustomMassagingProgressService.ShowResponseMessage(mess);
                //_CustomMassagingProgressService.WindowClosed += (canIContinueEventArgs) => {
                var confirmWindow = new MessageWindow_1.MessageWindow();
                confirmWindow.Show(mess);
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (mess.toLowerCase().includes("succeeded") || mess.toLowerCase().includes("בהצלחה") || mess.toLowerCase().includes("נפתחה רשומה בתיקים לאישור") || _this._IsCloseScreen == true) // Mirit 20/07/15 Task-14344 - add successfully (Hebrew) // Mirit 24/11/15 Task 18440- add IsCloseScreen
                     {
                        _this.RefreshDeclaration();
                        if (_this.CurrentSession.CurrentWindow != null) {
                            _this.CurrentSession.CloseCurrentWindow();
                            ; // moran 17.8.16 - AMI-57900 - add not null check
                        }
                    }
                });
                //    _CustomMassagingProgressService.Dispose();
                //};
            }
        }
    };
    DeclarationPaymentComponent.prototype.ActualSendToReTransfer = function () {
        var _this = this;
        if (this.customSendOptions == null) {
            console.log("[!] no send options!!");
            return;
        }
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Customs.Declaration"; })[0];
        var params = new CustomFileCreditRequestParams_1.CustomFileCreditRequestParams();
        params.Tenant = SessionLocator_1.SessionLocator.Tenant;
        params.AppicationId = this.paymentPM.DeclarationId;
        //params.TestCase = SelectedTest;
        params.LoggingEnabled = true;
        params.LoggingEntityId = this.DeclarationPM.Id;
        params.LoggingObjectTableId = ObjectTable.Id;
        params.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        params.RequestName = "Send ReTransfer Request";
        params.ResponseName = "Get ReTransfer Response";
        params.Mode = "ReTransfer";
        params.RequestVIA = this.customSendOptions.RequestVIA;
        params.ForcePersonalSign = this.customSendOptions.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent.ShowProgressBar(params.PBId, "שליחת בקשת העברה חוזרת לגובה", true).then(function (res) {
            console.log("[Send] Response/ShowProgressBar : ", res);
        }).catch(function (err) {
            _this.ValidationErrorsList = [];
            _this.ValidationErrorsList.push(err);
        });
        this.declarationMessagesService.PostSendTransferRequest(params)
            .subscribe(function (myServiceResponse) {
            var result = myServiceResponse.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                _this.CurrentSession.StopBusyIndicator();
                var mess = _this.AnalyzeResponseMessageForsendToReTransfer(result);
                if (!Tools_1.AppTool.IsNullOrEmpty(mess)) {
                    var messWindow = new MessageWindow_1.MessageWindow();
                    messWindow.Show(mess);
                    messWindow.WindowClosed.subscribe(function (event) {
                        if (mess.toLowerCase().includes("succeeded") || mess.toLowerCase().includes("בהצלחה") || _this._IsCloseScreen == true) {
                            _this.RefreshDeclaration();
                            if (_this.CurrentSession.CurrentWindow != null)
                                _this.CurrentSession.CloseCurrentWindow();
                        }
                    });
                    //    _CustomMassagingProgressService.Dispose();
                    //};
                }
            }
        });
    };
    // Response analyze
    DeclarationPaymentComponent.prototype.FillValidationErrors = function (title, errors) {
        var RequiredFieldsList = [];
        // 1- build validation errors
        for (var _i = 0, _a = errors.RequiredFields; _i < _a.length; _i++) {
            var error = _a[_i];
            if (!Tools_1.AppTool.IsNullOrEmpty(error.CustomMessageError)) {
                RequiredFieldsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate(error.CustomMessageError));
            }
            else {
                var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === error.TableName; })[0];
                var field = window.ObjectFields.filter(function (d) { return d.ObjectTableId === ObjectTable.Id && d.FieldName === error.FieldName; })[0];
                if (error.TableName.includes("Customs.SupplierInvoiceItem") && !Tools_1.AppTool.IsNullOrEmpty(error.EntityReference2)) {
                    error.EntityReference = error.EntityReference + " (חשבון " + error.EntityReference2 + ")";
                }
                var requiredField = TextCodeTranslator_1.TextCodeTranslator.GetRequiredFieldForTableMessageTranslation("Customs.General.O.FieldForTableIsRequired", field.FullNameTextCodeCode, error.TableName, error.EntityReference);
                RequiredFieldsList.push(requiredField);
            }
        }
        // 2- open window
        var windowArgs = {};
        windowArgs.Errors = RequiredFieldsList;
        windowArgs.ComponentHeight = '323px';
        var windowTitle = title;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        //logWindow.WindowClosed.subscribe(($event: any) => this.OnAddEditWindowClosed($event));
        logWindow.Show('./CustomsModules/CustomControls/Components/CustomsErrorsComponent');
    };
    DeclarationPaymentComponent.prototype.AnalyzeResponseMessageSendPaymentWithCheckCustomFileCredit = function (responseData) {
        var _this = this;
        var message = "";
        if (responseData != null) {
            if (responseData.IsTRansGove) {
                //_CustomMassagingProgressService.CloseWin();
                //_CustomMassagingProgressService.Dispose();
                //_CustomMassagingProgressService = null;
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Show(responseData.UserMessage);
                this.CurrentSession.StopBusyIndicator();
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.ActualSendToTransfer();
                    }
                });
                return null;
            }
            else {
                message = responseData.UserMessage;
                if (Tools_1.AppTool.IsNullOrEmpty((responseData.UserMessage))) {
                    if (!responseData.HasException && responseData.Succeeded) {
                        //message = "Send Payment Succeeded";
                        message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.SendPaymentSucceeded");
                    }
                    else {
                        //message = "Send Payment Failed";
                        message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.SendPaymentFailed");
                    }
                }
                if (!responseData.HasException && responseData.Succeeded) {
                    this._IsCloseScreen = true;
                }
            }
        }
        else {
            //message = "Service returned a null response!";
            message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Servicereturnedanullresponse");
        }
        return message;
    };
    DeclarationPaymentComponent.prototype.AnalyzeResponseMessage = function (responseData) {
        var _this = this;
        var message = "";
        if (responseData != null) {
            if (responseData.CreditStatus == "1") // moran 16.8.16 - AMI-57900
             {
                //if (_CustomMassagingProgressService != null) {
                //    _CustomMassagingProgressService.CloseWin();
                //    _CustomMassagingProgressService.Dispose();
                //    _CustomMassagingProgressService = null;
                //}
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                this.CurrentSession.StopBusyIndicator();
                confirmWindow.Show(responseData.UserMessage);
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.ActualSendToReTransfer();
                    }
                });
                return null;
            }
            else {
                message = responseData.UserMessage;
                if (Tools_1.AppTool.IsNullOrEmpty(responseData.UserMessage)) {
                    if (!responseData.HasException && responseData.Succeeded) {
                        //message = "Send Transfer Request Succeeded";
                        message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.SendTransferRequestSucceeded");
                    }
                    else {
                        //message = "Send Transfer Request Failed";
                        message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.SendTransferRequestFailed");
                    }
                }
                if (!responseData.HasException && responseData.Succeeded) {
                    this._IsCloseScreen = true;
                }
            }
        }
        else {
            //   message = "Service returned a null response!";
            message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Servicereturnedanullresponse");
        }
        return message;
    };
    DeclarationPaymentComponent.prototype.AnalyzeResponseMessageForsendToReTransfer = function (responseData) {
        var message = "";
        if (responseData != null) {
            message = responseData.UserMessage;
            if (Tools_1.AppTool.IsNullOrEmpty(responseData.UserMessage)) {
                if (!responseData.HasException && responseData.Succeeded) {
                    //message = "Send ReTransfer Request Succeeded";
                    message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.SendReTransferRequestSucceeded");
                }
                else {
                    //message = "Send ReTransfer Request Failed";
                    message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.SendReTransferRequestFailed");
                }
            }
            if (!responseData.HasException && responseData.Succeeded) {
                this._IsCloseScreen = true;
            }
        }
        else {
            //message = "Service returned a null response!";
            message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Servicereturnedanullresponse");
        }
        return message;
    };
    //#endregion 
    DeclarationPaymentComponent.prototype.OpenTaxScreen = function () {
        var windowArgs = {};
        var certificates = [];
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Height = 700;
        logWindow.Width = 1000;
        logWindow.ShowCloseButton = true;
        windowArgs.EntityPM = this.DeclarationPM;
        logWindow.WindowArgs = windowArgs;
        //  logWindow.WindowClosed.subscribe(($event: any) => this.SelectionCompleted($event));
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Taxes/DeclarationTaxesTabComponent');
    };
    DeclarationPaymentComponent.prototype.initDates = function () {
        //Task 36380: Update Payment Date & Time when Entering Payment screen
        if (!this.IsDisplayOnly) {
            this.paymentPM.PaymentDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        }
    };
    DeclarationPaymentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeclarationPaymentComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DeclarationPaymentComponent);
    return DeclarationPaymentComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationPaymentComponent = DeclarationPaymentComponent;
var PaymentMethodModel = /** @class */ (function (_super) {
    __extends(PaymentMethodModel, _super);
    function PaymentMethodModel(methodPM, parent) {
        var _this = _super.call(this) || this;
        _this.methodPM = methodPM;
        _this.parent = parent;
        _this.ObjectTableName = "Customs.DeclarationPaymentMethod";
        _this.DataContext = _this;
        _this._BanksList = [];
        _this.agentBanks = [];
        _this.IsAmountButtonVisibile = false;
        _this.customBankListService = new CustomBankListService_1.CustomBankListService();
        _this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService;
        _this._CustomsSettingExtendedListService = new CustomsSettingExtendedListService_1.CustomsSettingExtendedListService();
        _this.customsSettingExtendedListService = new CustomsSettingExtendedListService_1.CustomsSettingExtendedListService();
        _this.customBankCardExtendedPMService = new CustomBankCardExtendedPMService_1.CustomBankCardExtendedPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (methodPM.MethodTypeCode == "1") {
            _this.BanksList = [];
            if (!Tools_1.AppTool.IsNullOrEmpty(methodPM.InternalBankId)) {
                _this.customBankListService.getSingle(methodPM.InternalBankId).subscribe(function (response) {
                    if (response) {
                        _this.SelectedBank = response.Result;
                    }
                });
            }
            _this.LoadBanks();
        }
        if (parent.PaymentMethodsList.Length == 1) {
            _this.IsAmountButtonVisibile = true;
        }
        return _this;
    }
    Object.defineProperty(PaymentMethodModel.prototype, "BanksList", {
        get: function () { return this._BanksList; },
        set: function (val) {
            this._BanksList = val;
            this.LoggIt("PaymentMethodModel.SetBanksList");
        },
        enumerable: true,
        configurable: true
    });
    PaymentMethodModel.prototype.LoggIt = function (stack) {
        if (!this.parent._2LogBankList) {
            return;
        }
        var errorLogPM = new ErrorLogPM_1.ErrorLogPM();
        errorLogPM.Id = this.parent.ClientBankListLogUntilDateyyyyMMdd;
        errorLogPM.StackTrace = stack;
        errorLogPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        errorLogPM.UserName = SessionLocator_1.SessionLocator.LoggedUserId + "/" + SessionLocator_1.SessionLocator.LoggedUserPM.Email;
        errorLogPM.LogDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        var myBankList = this._BanksList.map(function (r) { return JSON.stringify({
            'InternalBankId': r.Id, 'LocalName': r.LocalName, 'EnglishName': r.EnglishName
        }); });
        errorLogPM.Exception = "BanksList:";
        errorLogPM.Exception += JSON.stringify(myBankList);
        errorLogPM.Exception += "\r\n";
        errorLogPM.Exception += "SelectedBank:";
        errorLogPM.Exception += JSON.stringify(this.SelectedBank);
        errorLogPM.Exception += "\r\n";
        errorLogPM.Exception += "DeclarationPaymentMethodPM:";
        errorLogPM.Exception += JSON.stringify({ 'DeclarationId': this.methodPM.DeclarationId, 'Line': this.methodPM.Line, 'SequenceNumeric': this.methodPM.SequenceNumeric });
        this.parent._ErrorLogPMFileLoggerService.insert(errorLogPM)
            .subscribe(function (r) { });
    };
    PaymentMethodModel.prototype.LoadBanks = function () {
        var _this = this;
        this.parent.declarationWebService.GetCustomBanksForCard(this.parent.DeclarationPM.CustomerId).subscribe(function (response) {
            var BlockAgentBankForMasabDefaultValue = "";
            var result = response.Result.filter(function (d) { return !d.InActive; });
            console.log("[Response] GetCustomBanksForCard: ", result);
            if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                //this.BanksList = result;
                _this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_BLOCK_BANK", "NON", "NON", SessionLocator_1.SessionLocator.Tenant)
                    .subscribe(function (responseDefault) {
                    var obj = responseDefault.Result;
                    if (obj) {
                        BlockAgentBankForMasabDefaultValue = obj['DefaultValue'];
                    }
                    _this.customsSettingExtendedListService.GetSettingByTenant().subscribe(function (response) {
                        var list = response.Result;
                        if (!Tools_1.AppTool.IsNullOrEmpty(list)) {
                            var customsSetting = list;
                            _this.BanksList = result;
                            if (result.length == 0) {
                                _this.customBankListService.getAllFromCache().subscribe(function (response) {
                                    if (response) {
                                        if (!response.HasError) {
                                            _this.agentBanks = response.Result.filter(function (d) { return d.PayerTypeCode == "3" && !d.InActive; });
                                            if (_this.agentBanks.length == 1) {
                                                _this.InternalBankId = _this.agentBanks[0].Id;
                                                _this.SelectedBank = _this.agentBanks[0];
                                                _this.BanksList = _this.agentBanks;
                                            }
                                            else {
                                                if (customsSetting != null && customsSetting.IsConnectedToUniFreight) {
                                                    _this.SendCreditToGetBank();
                                                }
                                                _this.BanksList = _this.agentBanks;
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
                                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.parent.GetCreditInternalBankId)) {
                                        _this.InternalBankId = _this.parent.GetCreditInternalBankId;
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
                                        _this.customBankListService.getAll().subscribe(function (response) {
                                            if (response) {
                                                if (!response.HasError) {
                                                    var agentBanks = [];
                                                    agentBanks = response.Result.filter(function (d) { return d.PayerTypeCode == "3" && !d.InActive; });
                                                    agentBanks = agentBanks.concat(_this.BanksList);
                                                    if (result.length == 1) {
                                                        if (Tools_1.AppTool.IsNullOrEmpty(_this.parent.GetCreditInternalBankId)) {
                                                            _this.InternalBankId = _this.BanksList[0].Id;
                                                        }
                                                        var bank = agentBanks.filter(function (d) { return d.Id == _this.InternalBankId; })[0];
                                                        _this.SelectedBank = bank;
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
                                            _this.customBankListService.getAll().subscribe(function (response) {
                                                if (response) {
                                                    if (!response.HasError) {
                                                        _this.agentBanks = response.Result.filter(function (d) { return d.PayerTypeCode == "3" && !d.InActive; });
                                                        if (_this.agentBanks.length > 0) {
                                                            _this.BanksList = _this.agentBanks;
                                                            if (_this.agentBanks.length == 1) {
                                                                if (_this.InternalBankId == null) {
                                                                    _this.InternalBankId = _this.BanksList[0].Id;
                                                                }
                                                            }
                                                            else {
                                                                if (_this.InternalBankId != null) {
                                                                    var bank = _this.BanksList.filter(function (d) { return d.Id == _this.InternalBankId; })[0];
                                                                    _this.SelectedBank = bank;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            });
                                        }
                                        //one bank
                                        else if (result.length == 1) {
                                            if (_this.InternalBankId == null) {
                                                _this.InternalBankId = _this.BanksList[0].Id;
                                            }
                                            _this.customBankListService.getAll().subscribe(function (response) {
                                                if (response) {
                                                    if (!response.HasError) {
                                                        var agentBanks = [];
                                                        agentBanks = response.Result.filter(function (d) { return d.PayerTypeCode == "3" && !d.InActive; });
                                                        _this.BanksList = _this.BanksList.concat(agentBanks);
                                                        if (_this.InternalBankId != null) {
                                                            var bank = _this.BanksList.filter(function (d) { return d.Id == _this.InternalBankId; })[0];
                                                            _this.SelectedBank = bank;
                                                        }
                                                    }
                                                }
                                            });
                                        }
                                        //two or more & dont block agent
                                        else if (result.length > 1) {
                                            var agentBanks = [];
                                            //fill connected banks
                                            var connectedBanks = result.filter(function (d) { return !d.InActive; });
                                            //fill agent
                                            _this.customBankListService.getAll().subscribe(function (response) {
                                                if (response) {
                                                    if (!response.HasError) {
                                                        agentBanks = response.Result.filter(function (d) { return d.PayerTypeCode == "3" && !d.InActive; });
                                                        //fill the LOV
                                                        _this.BanksList = connectedBanks.concat(agentBanks);
                                                        //select bank
                                                        if (_this.InternalBankId != null) {
                                                            var bank = _this.BanksList.filter(function (d) { return d.Id == _this.InternalBankId; })[0];
                                                            _this.SelectedBank = bank;
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
    };
    PaymentMethodModel.prototype.SendCreditToGetBank = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.InternalBankId))
            return;
        var objecttable = window.ObjectTables.filter(function (d) { return d.Name == "Customs.Declaration"; })[0];
        var searchParams = new CustomFileCreditRequestParams_1.CustomFileCreditRequestParams();
        {
            searchParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
            searchParams.AppicationId = this.parent.DeclarationPM.Id; //this.entityParent.DeclarationId;
            searchParams.LoggingEnabled = true;
            searchParams.LoggingEntityId = this.parent.DeclarationPM.Id; //entityParent.DeclarationId;
            searchParams.LoggingObjectTableId = objecttable.Id;
            searchParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            searchParams.RequestName = "Send Credit to Get Bank Request";
            searchParams.ResponseName = "Get Credit to Get Bank Response";
            searchParams.Mode = "GetBank";
        }
        searchParams.RequestVIA = RequestParamsBase_1.SendRequestVIA.Default;
        //searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
        //searchParams.RequestVIA = SendRequestVIA.WebServiceBatch;
        var myIIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        this.CurrentSession.StartBusyIndicator("");
        this.customBankListService.getAllFromCache().subscribe(function (response) {
            var allCustomBankList = response.Result;
            myIIGGeneralMessagesService.PostCustomFileCredit(searchParams)
                .subscribe(function (myServiceResponse) {
                _this.InternalBankId = "";
                var customFileCreditResponseData = myServiceResponse.Result;
                var mess = _this.AnalyzeResponseMessage(allCustomBankList, customFileCreditResponseData);
                if (Tools_1.AppTool.IsNullOrEmpty(_this.InternalBankId) && !Tools_1.AppTool.IsNullOrEmpty(_this.parent.DeclarationPM.CustomerCode)) {
                    myIIGGeneralMessagesService.GetDefBankForCustomer(_this.parent.DeclarationPM.CustomerCode, _this.parent.DeclarationPM.Tenant)
                        .subscribe(function (myServiceResponse) {
                        var myCIM_AGENT_BANK = myServiceResponse.Result;
                        //if (!AppTool.IsNullOrEmpty(myCIM_AGENT_BANK)) {
                        //    let bank: CustomBankList = this.BanksList.filter(d => d.InternalCode == myCIM_AGENT_BANK && !d.InActive)[0];
                        //    //this.parent.SelectedBankIndex = banksList.IndexOf(bank);
                        //    //FirePropertyChanged("banksList");
                        //    //FirePropertyChanged("SelectedBankIndex");
                        //    this.InternalBankId = bank.Id;
                        //}
                        _this.SetInternalBankId(allCustomBankList, myCIM_AGENT_BANK);
                        _this.CurrentSession.StopBusyIndicator();
                    });
                }
                else {
                    _this.CurrentSession.StopBusyIndicator();
                }
                //this.ResponseData = myServiceResponse.Result;
                //this.OnMassageDisplayMethod();
            });
        });
    };
    PaymentMethodModel.prototype.SetInternalBankId = function (allCustomBankList, BankCode) {
        if (!Tools_1.AppTool.IsNullOrEmpty(BankCode)) {
            var bank = allCustomBankList
                .filter(function (d) { return d.InternalCode == BankCode && !d.InActive; })[0];
            //.filter(d => d.BankCode == BankCode && !d.InActive)[0];
            if (!Tools_1.AppTool.IsNullOrEmpty(bank)) {
                this.InternalBankId = bank.Id;
            }
        }
    };
    PaymentMethodModel.prototype.AnalyzeResponseMessage = function (allCustomBankList, responseData) {
        var message = "";
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
            var test = false;
            if (test) {
                this.parent.PaymentDate = Tools_1.DateTool.AddDays(new Date(), -5);
            }
            if (responseData.CreditStatus == "1") {
                return null;
            }
            else {
                message = responseData.UserMessage;
                if (Tools_1.AppTool.IsNullOrEmpty(responseData.UserMessage)) {
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
            message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Servicereturnedanullresponse");
        }
        return message;
    };
    Object.defineProperty(PaymentMethodModel.prototype, "SelectedBank", {
        get: function () { return this.selectedBank; },
        set: function (value) {
            if (this.selectedBank != value) {
                this.selectedBank = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.InternalBankId = value.Id;
                    this.InternalBankName = value.LocalName;
                    if (Tools_1.AppTool.IsNullOrEmpty(value.LocalName)) {
                        this.InternalBankName = value.EnglishName;
                    }
                }
                else {
                    this.InternalBankId = null;
                    this.InternalBankName = null;
                }
                this.LoggIt("SelectedBankChanged!!");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "SequenceNumeric", {
        get: function () { return this.methodPM.SequenceNumeric; },
        set: function (value) {
            if (this.methodPM.SequenceNumeric != value) {
                this.methodPM.SequenceNumeric = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "CustomerActivityType", {
        get: function () { return this.customerActivityType; },
        set: function (value) {
            if (this.customerActivityType != value) {
                this.customerActivityType = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.PayerActivityTypeName = value.LocalName;
            }
            else {
                this.PayerActivityTypeName = null;
                this.PayerActivityTypeCode = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "MethodTypeCode", {
        get: function () { return this.methodPM.MethodTypeCode; },
        set: function (value) {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "MethodTypeName", {
        get: function () { return this.methodPM.MethodTypeName; },
        set: function (value) {
            if (this.methodPM.MethodTypeName != value) {
                this.methodPM.MethodTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "Amount", {
        get: function () { return this.methodPM.Amount; },
        set: function (value) {
            if (this.methodPM.Amount != value) {
                this.methodPM.Amount = value;
                this.parent.CalculateTotalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "BankCode", {
        get: function () { return this.methodPM.BankCode; },
        set: function (value) {
            if (this.methodPM.BankCode != value) {
                this.methodPM.BankCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "InternalBankId", {
        get: function () { return this.methodPM.InternalBankId; },
        set: function (value) {
            var _this = this;
            if (this.methodPM.InternalBankId != value) {
                this.methodPM.InternalBankId = value;
                this.customBankListService.getAllFromCache().subscribe(function (response) {
                    if (response) {
                        if (!response.HasError) {
                            var customBank = response.Result.filter(function (d) { return d.Id == value; })[0];
                            if (customBank == null && _this.BanksList != null) {
                                customBank = _this.BanksList.filter(function (d) { return d.Id == value; })[0];
                            }
                            if (customBank != null) {
                                _this.InternalBankName = customBank.LocalName != null ? customBank.LocalName : customBank.EnglishName;
                                if (customBank.PayerTypeCode == "0") {
                                    _this.customBankCardExtendedPMService.GetSingleCustomBanksCard(value, _this.parent.DeclarationPM.CustomerId).subscribe(function (response) {
                                        if (response) {
                                            if (!response.HasError) {
                                                var bankCard = response.Result;
                                                if (bankCard != null) {
                                                    _this.methodPM.BankCode = customBank.BankCode;
                                                    _this.methodPM.BranchCode = customBank.BranchCode;
                                                    _this.methodPM.CustomsBranchId = customBank.CustomsBranchId;
                                                    _this.methodPM.AccountNumber = customBank.AccountNumber;
                                                    _this.methodPM.PayerActivityTypeCode = customBank.PayerTypeCode;
                                                    _this.PayerActivityTypeName = customBank.PayerTypeName;
                                                }
                                                else {
                                                    var messageWindow = new MessageWindow_1.MessageWindow();
                                                    messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomBank.O.BankNotConnectedToCustomer"));
                                                    _this.InternalBankId = null;
                                                    _this.methodPM.InternalBankId = null;
                                                    _this.InternalBankName = null;
                                                    _this.SelectedBank = null;
                                                    _this.PayerActivityTypeCode = null;
                                                    _this.PayerActivityTypeName = null;
                                                }
                                            }
                                        }
                                    });
                                }
                                else {
                                    _this.methodPM.BankCode = customBank.BankCode;
                                    _this.methodPM.BranchCode = customBank.BranchCode;
                                    _this.methodPM.CustomsBranchId = customBank.CustomsBranchId;
                                    _this.methodPM.AccountNumber = customBank.AccountNumber;
                                    _this.methodPM.PayerActivityTypeCode = customBank.PayerTypeCode;
                                    _this.PayerActivityTypeName = customBank.PayerTypeName;
                                }
                            }
                            if (customBank == null) {
                                _this.methodPM.BankCode = null;
                                _this.methodPM.BranchCode = null;
                                _this.methodPM.CustomsBranchId = null;
                                _this.methodPM.AccountNumber = null;
                                _this.methodPM.PayerActivityTypeCode = null;
                            }
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "InternalBankName", {
        get: function () { return this.methodPM.InternalBankName; },
        set: function (value) {
            if (this.methodPM.InternalBankName != value) {
                this.methodPM.InternalBankName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "PayerActivityTypeCode", {
        get: function () { return this.methodPM.PayerActivityTypeCode; },
        set: function (value) {
            if (this.methodPM.PayerActivityTypeCode != value) {
                this.methodPM.PayerActivityTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "PayerActivityTypeName", {
        get: function () { return this.methodPM.PayerActivityTypeName; },
        set: function (value) {
            if (this.methodPM.PayerActivityTypeName != value) {
                this.methodPM.PayerActivityTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    PaymentMethodModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    PaymentMethodModel.prototype.OnDblClick = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MethodTypeCode) && this.parent.PaymentMethodsList.Length == 1) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.Amount)) {
                this.Amount = this.parent.TotalTax;
            }
        }
    };
    PaymentMethodModel.prototype.ComboBoxClicked = function () {
        this.LoadBanks();
    };
    return PaymentMethodModel;
}(BaseComponent_1.BaseComponent));
exports.PaymentMethodModel = PaymentMethodModel;
var PaymentProtestModel = /** @class */ (function (_super) {
    __extends(PaymentProtestModel, _super);
    function PaymentProtestModel(protestPM, parent) {
        var _this = _super.call(this) || this;
        _this.protestPM = protestPM;
        _this.parent = parent;
        _this.ObjectTableName = "Customs.DeclarationPaymentProtest";
        _this.DataContext = _this;
        return _this;
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
    Object.defineProperty(PaymentProtestModel.prototype, "ProtestTypeCode", {
        //#region Properties
        get: function () { return this.protestPM.ProtestTypeCode; },
        set: function (value) {
            if (this.protestPM.ProtestTypeCode != value) {
                this.protestPM.ProtestTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentProtestModel.prototype, "ProtestTypeName", {
        get: function () { return this.protestPM.ProtestTypeName; },
        set: function (value) {
            if (this.protestPM.ProtestTypeName != value) {
                this.protestPM.ProtestTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentProtestModel.prototype, "CustomsAgentExplanation", {
        get: function () { return this.protestPM.CustomsAgentExplanation; },
        set: function (value) {
            if (this.protestPM.CustomsAgentExplanation != value) {
                this.protestPM.CustomsAgentExplanation = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentProtestModel.prototype, "InvoiceNumber", {
        get: function () { return this.protestPM.InvoiceNumber; },
        set: function (value) {
            if (this.protestPM.InvoiceNumber != value) {
                this.protestPM.InvoiceNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentProtestModel.prototype, "InvoiceCounterKey", {
        get: function () { return this.protestPM.InvoiceCounterKey; },
        set: function (value) {
            if (this.protestPM.InvoiceCounterKey != value) {
                this.protestPM.InvoiceCounterKey = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentProtestModel.prototype, "GoodsItemClassification", {
        get: function () { return this.protestPM.GoodsItemClassification; },
        set: function (value) {
            if (this.protestPM.GoodsItemClassification != value) {
                this.protestPM.GoodsItemClassification = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentProtestModel.prototype, "GoodsItemLineNumber", {
        get: function () { return this.protestPM.GoodsItemLineNumber; },
        set: function (value) {
            if (this.protestPM.GoodsItemLineNumber != value) {
                this.protestPM.GoodsItemLineNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentProtestModel.prototype, "AmountInDispute", {
        get: function () { return this.protestPM.AmountInDispute; },
        set: function (value) {
            if (this.protestPM.AmountInDispute != value) {
                this.protestPM.AmountInDispute = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    PaymentProtestModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    PaymentProtestModel.prototype.EditProtestClicked = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item) && !this.parent.IsDisplayOnly) {
            var windowArgs = {};
            var certificates = [];
            windowArgs.DeclarationPM = this.parent.DeclarationPM;
            windowArgs.Parent = this.parent;
            windowArgs.Protest = this.protestPM;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Height = 700;
            logWindow.Width = 1000;
            logWindow.ShowCloseButton = true;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.SelectionCompleted($event); });
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationPayment/SupplierInvoiceSelectionComponent');
        }
    };
    PaymentProtestModel.prototype.SelectionCompleted = function (msg) {
        if (msg == "ok") {
            var paymentProtestType = null;
            var customsAgentExplanation = null;
            var protestTypeCode = null;
            var exsists = null;
            var editedProtest = null;
            if (this.parent.PaymentProtestsList.Length > 0) {
                paymentProtestType = this.parent.PaymentProtestsList.Collection[0].ProtestTypeName;
                protestTypeCode = this.parent.PaymentProtestsList.Collection[0].ProtestTypeCode;
                customsAgentExplanation = this.parent.PaymentProtestsList.Collection[0].CustomsAgentExplanation;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(customsAgentExplanation))
                customsAgentExplanation = this.parent._PayWithProtest_Default;
            if (this.parent.SelectedInvoices.Collection.length > 0) {
                var protests = this.parent.paymentPM.DeclarationPaymentProtests;
                var _loop_3 = function (protest) {
                    if (protest.InvoiceNumber != null) {
                        exsists = this_3.parent.SelectedInvoices.Collection.filter(function (d) { return d.InvoiceNumber == protest.InvoiceNumber && d.InvoiceCounterKey == protest.InvoiceCounterKey; })[0];
                        if (!exsists) {
                            editedProtest = this_3.parent.PaymentProtestsList.Collection.filter(function (d) { return d.DeclarationId == protest.DeclarationId && d.Line == protest.Line; })[0];
                            if (editedProtest != null) {
                                this_3.parent.paymentPM.RemoveDeclarationPaymentProtest(editedProtest);
                                this_3.parent.PaymentProtestsList.Remove(editedProtest);
                            }
                        }
                    }
                    if (protest.GoodsItemLineNumber != null) {
                        exsists = this_3.parent.SelectedInvoiceItems.Collection.filter(function (a) { return a.SequenceNumeric == protest.GoodsItemLineNumber && a.ClassificationCode == protest.InvoiceItemClassificationCode; })[0];
                        if (!exsists) {
                            editedProtest = this_3.parent.PaymentProtestsList.Collection.filter(function (a) { return a.DeclarationId == protest.DeclarationId && a.Line == protest.Line; })[0];
                            if (editedProtest != null) {
                                this_3.parent.paymentPM.RemoveDeclarationPaymentProtest(editedProtest);
                                this_3.parent.PaymentProtestsList.Remove(editedProtest);
                            }
                        }
                    }
                };
                var this_3 = this;
                for (var _i = 0, protests_1 = protests; _i < protests_1.length; _i++) {
                    var protest = protests_1[_i];
                    _loop_3(protest);
                }
                var _loop_4 = function (invoice_1) {
                    exsists = this_4.parent.PaymentProtestsList.Collection.filter(function (a) { return a.InvoiceNumber == invoice_1.InvoiceNumber && a.DeclarationId == invoice_1.DeclarationId && a.InvoiceCounterKey == invoice_1.InvoiceCounterKey; })[0]; //(from a in declarationPaymentPM.DeclarationPaymentProtests
                    if (!exsists) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(invoice_1.InvoiceNumber)) {
                            line = 0;
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
                            this_4.protestPM.Line = line;
                            this_4.protestPM.InvoiceNumber = invoice_1.InvoiceNumber;
                            this_4.protestPM.InvoiceCounterKey = invoice_1.InvoiceCounterKey;
                            this_4.protestPM.ProtestTypeName = paymentProtestType,
                                this_4.protestPM.CustomsAgentExplanation = customsAgentExplanation,
                                this_4.protestPM.ProtestTypeCode = protestTypeCode;
                            //        if (!this.parent.paymentPM.DeclarationPaymentProtests.includes(item)) {
                            //            this.parent.paymentPM.DeclarationPaymentProtests.push(item);
                            //            this.parent.PaymentProtestsList.Insert(new PaymentProtestModel(item, this.parent));
                            //}
                        }
                    }
                };
                var this_4 = this, line;
                for (var _a = 0, _b = this.parent.SelectedInvoices.Collection; _a < _b.length; _a++) {
                    var invoice_1 = _b[_a];
                    _loop_4(invoice_1);
                }
            }
            if (this.parent.SelectedInvoiceItems.Collection.length > 0) {
                var _loop_5 = function (invoiceItem) {
                    invoice = this_5.parent.SelectedInvoices.Collection.filter(function (d) { return d.DeclarationId == invoiceItem.DeclarationId && d.InvoiceCounterKey == invoiceItem.CounterKey; })[0];
                    if (invoice != null) {
                        exsists = this_5.parent.PaymentProtestsList.Collection.filter(function (a) { return a.GoodsItemLineNumber == invoiceItem.SequenceNumeric && a.InvoiceItemClassificationCode == invoiceItem.ClassificationCode && a.DeclarationId == invoice.DeclarationId && a.InvoiceNumber == invoice.InvoiceNumber; })[0]; // (from a in declarationPaymentPM.DeclarationPaymentProtests
                        if (!exsists) {
                            if (!Tools_1.AppTool.IsNullOrEmpty(invoice.InvoiceNumber)) {
                                line = 0;
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
                                this_5.protestPM.GoodsItemLineNumber = invoiceItem.SequenceNumeric;
                                // this.protestPM.InvoiceNumber = invoice.InvoiceNumber;
                                this_5.protestPM.ProtestTypeName = paymentProtestType;
                                this_5.protestPM.AddedByInvoice = true;
                                this_5.protestPM.InvoiceItemClassificationCode = invoiceItem.ClassificationCode;
                                this_5.protestPM.CustomsAgentExplanation = customsAgentExplanation;
                                this_5.protestPM.ProtestTypeCode = protestTypeCode;
                                this_5.protestPM.GoodsItemClassification = invoiceItem.ClassificationCode;
                                //if (!AppTool.IsNullOrEmpty(this.protestPM.InvoiceItemClassificationCode) && this.protestPM.GoodsItemLineNumber != null) {
                                //    this.GoodsItemLineNumber = this.protestPM.GoodsItemLineNumber.toString();// + "-" + this.protestPM.InvoiceItemClassificationCode;
                                //}
                                //else if (AppTool.IsNullOrEmpty(this.protestPM.GoodsItemLineNumber)) {
                                //    this.GoodsItemLineNumber = this.protestPM.InvoiceItemClassificationCode;
                                //}
                                //else if (AppTool.IsNullOrEmpty(this.protestPM.InvoiceItemClassificationCode)) {
                                //this.GoodsItemLineNumber = this.protestPM.GoodsItemLineNumber + "";///Severity	Code	Description	Project	File	Line	Suppression State                                                                                                                                                                                                         Error	TS2322	Type 'string' is not assignable to type 'number'.TypeScript Virtual Projects	C: \LW\.\Customsmodules\Customsdeclarationmodules\Declarationothers\Components\DeclarationPayment\DeclarationPaymentComponent.ts	2278	Active
                                this_5.GoodsItemLineNumber = this_5.protestPM.GoodsItemLineNumber;
                                this_5.GoodsItemClassification = this_5.protestPM.InvoiceItemClassificationCode;
                                // }
                                //if (!this.parent.paymentPM.DeclarationPaymentProtests.includes(item)) {
                                //    this.parent.paymentPM.DeclarationPaymentProtests.push(item);
                                //    this.parent.PaymentProtestsList.Insert(new PaymentProtestModel(item, this.parent));
                                //}
                            }
                        }
                    }
                };
                var this_5 = this, invoice, line;
                for (var _c = 0, _d = this.parent.SelectedInvoiceItems.Collection; _c < _d.length; _c++) {
                    var invoiceItem = _d[_c];
                    _loop_5(invoiceItem);
                }
            }
            else if (this.parent.SelectedInvoices.Collection.length == 0) {
                if (this.parent.PaymentProtestsList.Collection.length > 0) {
                    for (var _e = 0, _f = this.parent.PaymentProtestsList.Collection; _e < _f.length; _e++) {
                        var protest = _f[_e];
                        if (protest.InvoiceNumber != null) {
                            this.parent.paymentPM.RemoveDeclarationPaymentProtest(protest);
                            this.parent.PaymentProtestsList.Remove(protest);
                        }
                    }
                }
            }
            //    this.parent.FillGridsData();
        }
    };
    return PaymentProtestModel;
}(BaseComponent_1.BaseComponent));
exports.PaymentProtestModel = PaymentProtestModel;
//# sourceMappingURL=DeclarationPaymentComponent.js.map