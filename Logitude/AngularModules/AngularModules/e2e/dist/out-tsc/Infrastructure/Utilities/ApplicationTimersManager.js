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
var ErrorsLogPMService_1 = require("../../Infrastructure/Services/ExtendedPMs/ErrorsLogPMService");
var LogitudeApplicationService_1 = require("../../Infrastructure/Services/WebServices/LogitudeApplicationService");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var Rx_1 = require("rxjs/Rx");
var SessionLocator_1 = require("../Utilities/SessionLocator");
var CachedDataManager_1 = require("../Utilities/CachedDataManager");
var SessionInfo_1 = require("../Utilities/SessionInfo");
var Tools_1 = require("../Tools");
var UserLastLoginPMService_1 = require("../../Common/Services/StandardPMs/UserLastLoginPMService");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var PerformanceLogService_1 = require("../../Infrastructure/Services/ExtendedPMs/PerformanceLogService");
var ObjectsLocator_1 = require("../Locators/ObjectsLocator");
var ApplicationTimersManager = /** @class */ (function () {
    function ApplicationTimersManager() {
        this.SignoutCompleted = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsUserUnlock = false;
        this.IsUpgradingEnd = false;
        this.logService = new ErrorsLogPMService_1.ErrorsLogPMService();
        this.logitudeApplicationService = new LogitudeApplicationService_1.LogitudeApplicationService();
        this.userLastLoginPMService = new UserLastLoginPMService_1.UserLastLoginPMService();
        this.performanceLogService = new PerformanceLogService_1.PerformanceLogService();
        //this.signalRGeneralService = new SignalRGeneralService();
    }
    ApplicationTimersManager.prototype.StartApplicationTimers = function () {
        var _this = this;
        //this.signalRChannelService = new SignalRChannelService();
        //SessionLocator.SignalRChannelService = this.signalRChannelService;
        SessionLocator_1.SessionLocator.TimersSubscribtions.push(this.getTimer(30000).subscribe(function (res) {
            _this.AddErrorLogs();
            //console.log('The response is received.');
        }));
        if (ObjectsLocator_1.ObjectsLocator != null && ObjectsLocator_1.ObjectsLocator.GlobalSetting != null && ObjectsLocator_1.ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
            console.log("WorkEnvironment is customs! Suppress this.AddPeformanceLogs();");
        }
        else {
            SessionLocator_1.SessionLocator.TimersSubscribtions.push(this.getTimer(30000).subscribe(function (res) {
                _this.AddPeformanceLogs();
            }));
        }
        SessionLocator_1.SessionLocator.TimersSubscribtions.push(this.getTimer(60000).subscribe(function (res) {
            _this.CheckIsupgradingSystem();
        }));
        SessionLocator_1.SessionLocator.TimersSubscribtions.push(this.getTimer(30000).subscribe(function (res) {
            _this.CheckApplicationLocalStorage();
        }));
        SessionLocator_1.SessionLocator.TimersSubscribtions.push(this.getTimer(30000).subscribe(function (res) {
            _this.CheckUserLastLogin();
        }));
        SessionLocator_1.SessionLocator.TimersSubscribtions.push(this.getTimer(120000).subscribe(function (res) {
            _this.CheckUserValidity();
        }));
        SessionLocator_1.SessionLocator.TimersSubscribtions.push(this.getTimer(60000).subscribe(function (res) {
            CachedDataManager_1.CachedDataManager.CheckSystemMetadataLastUpdate().subscribe(function (reponse) {
                console.log("------------- SystemMetadataLastUpdate has been checked by timer! ---------------");
            });
        }));
        if (SessionLocator_1.SessionLocator.UseCachedData) {
            SessionLocator_1.SessionLocator.TimersSubscribtions.push(this.getTimer(30000).subscribe(function (res) {
                CachedDataManager_1.CachedDataManager.CheckCachedTableLastUpdateDate().subscribe(function (reponse) {
                    console.log("cached tables checked by timer!");
                });
            }));
            //Observable.Interval(TimeSpan.FromSeconds(1.0));
            //var timerId = setTimeout(this.AddErrorLogs, 2000)  
            // setInterval(SaveErrorLogs, delay);//60000
            //this.signalRGeneralService.messageReceived.subscribe((ms: ChannelEvent) => {
            //    debugger;
            //});
            //var channelName: string = "Tenant" + SessionInfo.LoggedUserTenant + "Channel";
            //this.signalRChannelService.subscribeChannel(channelName).subscribe(
            //    (ev: any) => {
            //        console.log('Application Manager :signalR event received:' + channelName);
            //        if (ev.EventName === "CachedTableUpdate") {
            //                CachedDataManager.RefreshTableData(ev.EventParameter);
            //        }
            //    },
            //    (error: any) => {
            //        console.warn("Attempt to join channel failed!", error);
            //    }
            //)
        }
    };
    ApplicationTimersManager.prototype.getTimer = function (period) {
        return Rx_1.Observable.interval(period).timeInterval();
    };
    ApplicationTimersManager.prototype.CheckApplicationLocalStorage = function () {
        var _this = this;
        try {
            var isMac = navigator.platform.toUpperCase().indexOf('MAC') >= 0; //MAC//WIN32
            if (window.localStorage.length === 0 && !isMac) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Width = 450;
                messageWindow.Title = "Application Storage Deleted";
                messageWindow.Height = 190;
                messageWindow.Show("Your application storage has been deleted. Please login again.");
                messageWindow.WindowClosed.subscribe(function ($event) {
                    _this.OnSignoutClicked();
                });
            }
        }
        catch (e) {
            console.error(e);
        }
    };
    ApplicationTimersManager.prototype.CheckUserLastLogin = function () {
        var _this = this;
        this.userLastLoginPMService.GetUserLastLogin(SessionInfo_1.SessionInfo.LoggedUserPM.Id, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (response) {
            if (!response.HasError && response.Result) {
                var lastloginPM = response.Result;
                var computerId = SessionLocator_1.SessionLocator.GetComputerIdFromStorage();
                if (!Tools_1.AppTool.IsNullOrEmpty(computerId) && lastloginPM.ComputerId != computerId && !ObjectsLocator_1.ObjectsLocator.GlobalSetting.SameUserLoginEnabled) {
                    if (!_this.IsUserUnlock) { //
                        _this.IsUserUnlock = true;
                        if (_this.CurrentSession) {
                            _this.CurrentSession.StopBusyIndicator();
                            if (_this.CurrentSession.CurrentWindow) {
                                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            }
                        }
                        var args = "";
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.IsOverAll = true;
                        logWindow.Title = "";
                        logWindow.WindowArgs = args;
                        logWindow.Width = 1000;
                        logWindow.Height = 250;
                        logWindow.IsHideWindowMargin = true;
                        logWindow.IsHideHeader = true;
                        SessionLocator_1.SessionLocator.HomeComponent.ShowLockIndicator = true;
                        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/UserUnlockComponent/UserUnlockComponent');
                        logWindow.WindowClosed.subscribe(function ($event) {
                            SessionLocator_1.SessionLocator.HomeComponent.ShowLockIndicator = false;
                            _this.IsUserUnlock = false;
                        });
                    }
                }
            }
        });
    };
    ApplicationTimersManager.prototype.CheckIsupgradingSystem = function () {
        var _this = this;
        try {
            if (!this.IsUpgradingEnd) {
                this.logitudeApplicationService.GetCheckIsupgradingSystem().subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            if (myResult == true && !_this.IsUpgradingEnd) {
                                _this.IsUpgradingEnd = true;
                                var messageWindow = new MessageWindow_1.MessageWindow();
                                messageWindow.IsOverAll = true;
                                messageWindow.Width = 450;
                                messageWindow.Title = "Logitude Message";
                                messageWindow.Height = 190;
                                messageWindow.Show("The site is upgrading right now and you will be logged out , sorry for disturbing you!");
                                messageWindow.WindowClosed.subscribe(function ($event) {
                                    _this.OnSignoutClicked();
                                });
                            }
                        }
                    }
                }, function (error) {
                    console.error("Check Isupgrading System: ", error);
                });
            }
        }
        catch (e) {
            console.error(e);
        }
    };
    ApplicationTimersManager.prototype.CheckUserValidity = function () {
        var _this = this;
        try {
            this.logitudeApplicationService.GetCurrenctUserValidity().subscribe(function (res) {
                var response = res;
                if (!response.HasError) {
                    var myResult = response.Result;
                    if (myResult) {
                        if (myResult.IsValid == false) {
                            var isShowMessage = true;
                            if (myResult.ErrorCode == "AUTH") {
                                SessionLocator_1.SessionLocator.HomeComponent.SignoutClicked();
                            }
                            else {
                                var errorMessage = "This user is not authenticated!";
                                if (myResult.ErrorCode == "SUSR") {
                                    errorMessage = "Another user has logged!";
                                }
                                if (myResult.ErrorCode == "SUPG") {
                                    errorMessage = "The site is upgrading right now and you will be logged out , sorry for disturbing you!";
                                    if (_this.IsUpgradingEnd) {
                                        isShowMessage = false;
                                    }
                                    else {
                                        _this.IsUpgradingEnd = true;
                                    }
                                }
                                if (isShowMessage) {
                                    var messageWindow = new MessageWindow_1.MessageWindow();
                                    messageWindow.Width = 450;
                                    messageWindow.Title = "Logitude Message";
                                    messageWindow.Height = 190;
                                    messageWindow.Show(errorMessage);
                                    messageWindow.WindowClosed.subscribe(function ($event) {
                                        _this.OnSignoutClicked();
                                    });
                                }
                            }
                        }
                        SessionInfo_1.SessionInfo.DocumentDownloadToken = myResult.DocumentDownloadToken;
                    }
                }
            }, function (error) {
                console.error("Check User Validity: ", error);
            });
        }
        catch (e) {
            console.error(e);
        }
    };
    ApplicationTimersManager.prototype.OnSignoutClicked = function () {
        SessionLocator_1.SessionLocator.HomeComponent.SignoutClicked();
    };
    ApplicationTimersManager.prototype.AddErrorLogs = function () {
        try {
            for (var key in sessionStorage) {
                if (key.indexOf("ErrorLogs") != -1) {
                    var logJson = window.sessionStorage.getItem(key);
                    var errorLog = JSON.parse(logJson);
                    this.logService.insert(errorLog).subscribe(function (response) {
                        window.sessionStorage.removeItem(["ErrorLogs", response.Result.Id]);
                    }, function (error) {
                        console.error("Adding ErrorLog Timer: ", error);
                    });
                }
            }
        }
        catch (e) {
            console.error(e);
        }
    };
    ApplicationTimersManager.prototype.AddPeformanceLogs = function () {
        try {
            var AllLogsList = [];
            for (var key in sessionStorage) {
                if (key.indexOf("PerformanceLogs") != -1) {
                    var logJson = window.sessionStorage.getItem(key);
                    var performanceLog = JSON.parse(logJson);
                    performanceLog.LogDateTimeLocal = new Date();
                    AllLogsList.push(performanceLog);
                    //PerformanceLogs,5d816163-030d-4d76-a5ef-c19a43951e0b
                    //this.performanceLogService.insert(performanceLog).subscribe(response => {
                    //    window.sessionStorage.removeItem(["PerformanceLogs", response.Result.Id]);
                    //}, error => {
                    //    console.error("Adding Performance Log Timer: ", error);
                    //});
                }
            }
            //if (logsList.length <= 20) {
            var tobeAddedLogsList = [];
            var count = 0;
            for (var lk in AllLogsList) {
                tobeAddedLogsList.push(AllLogsList[lk]);
                window.sessionStorage.removeItem(["PerformanceLogs", AllLogsList[lk].Id]);
                count++;
                if (count == 20)
                    break;
            }
            if (tobeAddedLogsList.length > 0) {
                this.performanceLogService.insertLogsList(tobeAddedLogsList).subscribe(function (response) {
                }, function (error) {
                    console.error("Adding Performance Log Timer: ", error);
                });
            }
        }
        catch (e) {
            console.error(e);
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ApplicationTimersManager.prototype, "SignoutCompleted", void 0);
    ApplicationTimersManager = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ApplicationTimersManager);
    return ApplicationTimersManager;
}());
exports.ApplicationTimersManager = ApplicationTimersManager;
//# sourceMappingURL=ApplicationTimersManager.js.map