"use strict";
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
var Tools_1 = require("../../../Infrastructure/Tools");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var ResponseDataBase_1 = require("../../../Customs/DataContract/ResponseData/ResponseDataBase");
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
//////////////////
/// CustomMessageProgressComponent  is below 
var CustomMessageProgressComponent = /** @class */ (function () {
    function CustomMessageProgressComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    CustomMessageProgressComponent_1 = CustomMessageProgressComponent;
    CustomMessageProgressComponent.ShowProgressBar = function (PBId, Title, OnSuccessCloseWin
    //, OnSuccessCloseWinMethod?: (response: any) => boolean
    , myShowProgressBarParams) {
        var _this = this;
        var alreadyDone = false;
        return new Promise(function (resolve, reject) {
            _this.StaticCurrentSession.StartBusyIndicator("");
            var currCustomMessageProgressHelper = new CustomMessageProgressHelper();
            CustomMessageProgressComponent_1.CurrCustomMessageProgressHelper = currCustomMessageProgressHelper;
            currCustomMessageProgressHelper.StartProgress(PBId, 3, OnSuccessCloseWin);
            currCustomMessageProgressHelper.OnMessageArrived.subscribe(function () {
                //currCustomMessageProgressHelper.OnMessageArrived.unsubscribe();
                if (alreadyDone)
                    return;
                alreadyDone = true;
                try {
                    _this.StaticCurrentSession.StopBusyIndicator();
                    var response = currCustomMessageProgressHelper.ResponseData;
                    resolve(response);
                    var success = false;
                    var continueProcessInBackground = false;
                    if (currCustomMessageProgressHelper.ResponseData) {
                        if (currCustomMessageProgressHelper.ResponseData.Succeeded && !currCustomMessageProgressHelper.ResponseData.HasException) {
                            ////xxxxxxxxxxxxx
                            success = true;
                        }
                        if (currCustomMessageProgressHelper.ResponseData.ContinueProcessInBackground) {
                            continueProcessInBackground = true;
                        }
                    }
                    if (OnSuccessCloseWin) {
                        if (!continueProcessInBackground && success) {
                            //this.CurrentSession.StopBusyIndicator();
                            //currCustomMessageProgressComponent.ngOnDestroy();
                            return;
                        }
                    }
                    _this.StaticCurrentSession.StopBusyIndicator();
                    if (myShowProgressBarParams) {
                        if (myShowProgressBarParams.OnSuccessAnalyzeCloseWinMethod) {
                            var successAnalyze = myShowProgressBarParams.OnSuccessAnalyzeCloseWinMethod(currCustomMessageProgressHelper.ResponseData);
                            if (successAnalyze) {
                                return;
                            }
                        }
                    }
                    var myOnCloseCustomMessageProgressComponentMethod_1;
                    if (myShowProgressBarParams) {
                        myOnCloseCustomMessageProgressComponentMethod_1 = myShowProgressBarParams.OnCloseCustomMessageProgressComponentMethod;
                    }
                    //CustomMessageProgressComponent.ShowCustomMessageProgressComponent(Title, currCustomMessageProgressHelper._Message,
                    //    currCustomMessageProgressHelper.ResponseData, myOnCloseCustomMessageProgressComponentMethod
                    //);
                    CustomMessageProgressComponent_1.ShowCustomMessageProgressComponent(Title, currCustomMessageProgressHelper._Message, function () {
                        if (myOnCloseCustomMessageProgressComponentMethod_1) {
                            myOnCloseCustomMessageProgressComponentMethod_1(currCustomMessageProgressHelper.ResponseData);
                        }
                    }
                    //currCustomMessageProgressHelper.ResponseData, myOnCloseCustomMessageProgressComponentMethod
                    );
                }
                finally {
                    _this.StaticCurrentSession.StopBusyIndicator();
                    currCustomMessageProgressHelper.ngOnDestroy();
                }
            });
        });
    };
    CustomMessageProgressComponent.ShowCustomMessageProgressComponent = function (title, mess, 
    //    response: any, OnCloseCustomMessageProgressComponentMethod?: (response: any) => void
    OnCloseCustomMessageProgressComponentMethod) {
        var customMassagingProgressWindow = new LogitudeWindow_1.LogitudeWindow();
        customMassagingProgressWindow.Height = 400;
        customMassagingProgressWindow.Width = 600;
        customMassagingProgressWindow.ShowCloseButton = true; //.CloseButton.IsEnabled = false;
        customMassagingProgressWindow.Title = title;
        customMassagingProgressWindow.Show('./CustomsModules/CustomsControls/Components/CustomMessageProgressComponent');
        customMassagingProgressWindow.ComponentLoaded
            .subscribe(function (customMessageProgressComponent) {
            customMessageProgressComponent._Message = mess; //currCustomMessageProgressHelper._Message;
        });
        customMassagingProgressWindow.WindowClosed.subscribe(function (anyString) {
            ///
            if (OnCloseCustomMessageProgressComponentMethod) {
                OnCloseCustomMessageProgressComponentMethod();
            }
        });
    };
    CustomMessageProgressComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    var CustomMessageProgressComponent_1;
    CustomMessageProgressComponent.StaticCurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    CustomMessageProgressComponent.CurrCustomMessageProgressHelper = null;
    CustomMessageProgressComponent = CustomMessageProgressComponent_1 = __decorate([
        core_1.Component({
            selector: 'custom-message-progress',
            moduleId: module.id,
            templateUrl: './CustomMessageProgressComponent.html',
        })
    ], CustomMessageProgressComponent);
    return CustomMessageProgressComponent;
}());
exports.CustomMessageProgressComponent = CustomMessageProgressComponent;
var ShowProgressBarParams = /** @class */ (function () {
    function ShowProgressBarParams() {
    }
    return ShowProgressBarParams;
}());
exports.ShowProgressBarParams = ShowProgressBarParams;
var CustomMessageProgressHelper = /** @class */ (function () {
    function CustomMessageProgressHelper() {
        this.OnMessageArrived = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._LastUpdateCurrentStageLine = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this._Disposed = false;
        this._ServerCalc = true;
        this._ContinueInBackgroundMess = "המסר נבנה בהצלחה וישלח בתהליך רקע";
        this._DispatcherTimerIntervalStart = 99; // 300;
        this._DispatcherTimerInterval = 99; // 400;
        this.BasicResponse = false;
    }
    CustomMessageProgressHelper.prototype.StartProgress = function (PBId, timeOutInMinutes, OnSuccessCloseWin) {
        this._OnSuccessCloseWin = OnSuccessCloseWin;
        this._PBId = PBId;
        this._TimeOutInMinutes = 3;
        this._TimeOutInMinutes = timeOutInMinutes;
        this.DoWork();
    };
    CustomMessageProgressHelper.prototype.StopTimer = function () {
        if (this._DispatcherTimer) {
            clearTimeout(this._DispatcherTimer);
        }
    };
    CustomMessageProgressHelper.prototype.GetformatedLine = function (val) {
        if (!Tools_1.AppTool.IsNullOrEmpty(val) && val.startsWith("<?xml")) {
            var toStop = false;
            ;
            var res = this.AnalyzeResponseMessage(val);
            if (res.toStop) {
                return null;
            }
            return res.messageFromServer;
        }
        else {
            return val;
        }
    };
    CustomMessageProgressHelper.prototype.AnalyzeResponseMessage = function (messageFromServer) {
        var toStop = true;
        try {
            this.ResponseData = JSON.parse(messageFromServer);
        }
        catch (err) {
            ///Maybe Onlt mesaage like : No Sign For Id ...
        }
        if (this.ResponseData != null) {
            if (this.ResponseData.HasException) {
                messageFromServer = this.ResponseData.UserMessage;
            }
            else if (this.ResponseData.ContinueProcessInBackground) {
                messageFromServer = this._ContinueInBackgroundMess; // ("המסר נבנה בהצלחה וישלח בתהליך רקע");
            }
            else if (this.ResponseData.Succeeded) {
                messageFromServer = "בוצע בהצלחה";
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ResponseData.UserMessage)) {
                    messageFromServer = this.ResponseData.UserMessage;
                }
            }
        }
        toStop = true;
        this.StopAndShowMessage(messageFromServer);
        return { 'messageFromServer': messageFromServer, 'toStop': toStop };
    };
    CustomMessageProgressHelper.prototype.DoWork = function () {
        var _this = this;
        clearTimeout(this._DispatcherTimer);
        //if (DateTime.Now.Subtract(_LastUpdateCurrentStageLine) > TimeSpan.FromMinutes(TimeOutInMinutes))
        if (Tools_1.DateTool.AddMinute(this._LastUpdateCurrentStageLine, this._TimeOutInMinutes) < Tools_1.DateTool.GetCurrentDateTimeAsUtc()) {
            this.CurrentSession.StopBusyIndicator();
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show("הבקשה נתקלה בחוסר מענה , האם להמשיך להמתין לתשובה ?");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this._LastUpdateCurrentStageLine = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                    _this._TimeOutInMinutes = 1;
                    _this.DoWork();
                }
                else {
                    _this.StopAndShowMessage("הבקשה נתקלה בחוסר מענה");
                }
            });
            return;
        }
        this.GetClientProgressBarIndicatorCurrentStage();
    };
    CustomMessageProgressHelper.prototype.GetClientProgressBarIndicatorCurrentStage = function () {
        var _this = this;
        var myIIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        var sub = myIIGGeneralMessagesService
            .GetClientProgressBarIndicatorCurrentStage(SessionLocator_1.SessionLocator.Tenant, this._PBId, this.BasicResponse).subscribe(function (serviceResponse) {
            sub.unsubscribe();
            _this.ClientProgressBarIndicatorCurrentStageCompleted(serviceResponse);
        });
    };
    CustomMessageProgressHelper.prototype.ClientProgressBarIndicatorCurrentStageCompleted = function (serviceResponse) {
        var _this = this;
        if (this.MessageArrived)
            return;
        try {
            this._WebServiceClientHasError = (serviceResponse.HasError);
            var e = serviceResponse.Result;
            var continueInBackground = e.continueInBackground;
            var Error1 = serviceResponse.ErrorsArray.map(function (err) { return err; }).join(', ');
            ;
            //e.stopMeNow
            var responseDataXml = e.responseDataXml;
            if (this.BasicResponse) {
                this.CurrentStageLine = responseDataXml;
                return;
            }
            var customsRequestStep = e.ProgressStage;
            var mess = "";
            mess = ResponseDataBase_1.ResponseDataBase.GetCustomsRequestStepText(customsRequestStep);
            if (continueInBackground) {
                console.log("ContinueInBackground !!");
                this.CurrentStageLine = this._ContinueInBackgroundMess; // "המסר נבנה בהצלחה וישלח בתהליך רקע" + mess;
                if (!Tools_1.AppTool.IsNullOrEmpty(responseDataXml)) {
                    this.CurrentStageLine = responseDataXml;
                }
                this.ResponseData = {
                    //ProgressStage: "????",
                    ///ContinueProcessInBackgroundMessage: "Dummy From CustomMessageProgressComponent",
                    HasException: false,
                    UserMessage: "",
                    ContinueProcessInBackground: true,
                    Succeeded: true,
                    CustomsRequestsSheetId: "ContinueProcessInBackground DummyResponsdata!! - From CustomMessageProgressComponent",
                    CorrelationId: "",
                };
                this.StopAndShowMessage(this.CurrentStageLine);
                //var res = this.AnalyzeResponseMessage(responseDataXml);//
                //var res = this.AnalyzeResponseMessage(responseDataXml);
                this.MessageArrived = true;
                return;
            }
            if (e.stopMeNow) {
                if (Tools_1.AppTool.IsNullOrEmpty(responseDataXml)) {
                    this.CurrentStageLine = this._ContinueInBackgroundMess; //"המסר נבנה בהצלחה וישלח בתהליך רקע" + mess;
                    this.StopAndShowMessage(this.CurrentStageLine);
                }
                else {
                    var o = false;
                    var res = this.AnalyzeResponseMessage(responseDataXml);
                }
                this.MessageArrived = true;
                return;
            }
            this.CurrentStageLine = mess;
        }
        catch (err) {
        }
        finally {
            if (!this.MessageArrived) {
                if (this._DispatcherTimerInterval < 2000) {
                    this._DispatcherTimerInterval = this._DispatcherTimerInterval + 50;
                }
                this._DispatcherTimerIntervalStart;
                this._DispatcherTimer = setTimeout(function () { return _this.DoWork(); }, this._DispatcherTimerInterval);
            }
        }
    };
    CustomMessageProgressHelper.prototype.StopAndShowMessage = function (mess) {
        this.CurrentSession.StopBusyIndicator();
        this.StopTimer(); // _DispatcherTimer.Tick -= _DispatcherTimer_Tick;
        this._Message = mess;
        this.CurrentStageLine = null;
        this.MessageArrived = true;
    };
    Object.defineProperty(CustomMessageProgressHelper.prototype, "CurrentStageLine", {
        get: function () { return this._CurrentStageLine; },
        set: function (value) {
            if (this._CurrentStageLine == value)
                return;
            this._CurrentStageLine = value;
            this.CurrentSession.StartBusyIndicator(this._CurrentStageLine);
            this._LastUpdateCurrentStageLine = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            this._DispatcherTimerInterval = this._DispatcherTimerIntervalStart;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomMessageProgressHelper.prototype, "MessageArrived", {
        get: function () { return this._MessageArrived; },
        set: function (value) {
            if (value) {
                this.OnMessageArrived.emit();
            }
            if (this._MessageArrived == value)
                return;
            this._MessageArrived = value;
        },
        enumerable: true,
        configurable: true
    });
    CustomMessageProgressHelper.prototype.ngOnDestroy = function () {
        this.StopTimer();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CustomMessageProgressHelper.prototype, "OnMessageArrived", void 0);
    return CustomMessageProgressHelper;
}());
exports.CustomMessageProgressHelper = CustomMessageProgressHelper;
//////////////////
/////////////////////////////////////
//# sourceMappingURL=CustomMessageProgressComponent.js.map