"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var DeclarationValidator_1 = require("../Validators/DeclarationValidator");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var Rx_1 = require("rxjs/Rx");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var http_1 = require("@angular/http");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../Infrastructure/Tools");
var DeclarationDisplayOnlyChecks = /** @class */ (function () {
    function DeclarationDisplayOnlyChecks() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CustomsRequestSheetExtended';
        this.http = ServiceHelper_1.ServiceHelper.Http;
    }
    DeclarationDisplayOnlyChecks.prototype.DeclarationViewDisplayOnlyChecks = function (entityPM) {
        var _this = this;
        var editComponentNeedsRefresh = null;
        if (this.CurrentSession.CurrentEditComponent) {
            editComponentNeedsRefresh = this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefresh;
        }
        //if (!editComponentNeedsRefresh) {
        this.entityPM = entityPM;
        var declarationValidator = new DeclarationValidator_1.DeclarationValidator();
        declarationValidator.SetEntityPM(entityPM);
        var serviceResponse;
        serviceResponse = new ServiceResponse_1.ServiceResponse();
        if (this.entityPM.IsCancelled) {
            return Rx_1.Observable.defer(function () {
                // the declaration is cancelled 
                var message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Cancelled");
                //for menu buttons
                clearTimeout(_this.timerToken);
                //this.timerToken = setTimeout(() => {
                //    var args: MenuButtonsStateChangedEventArgs = new MenuButtonsStateChangedEventArgs();
                //    args.MenuButtonsStates = {};
                //    args.MenuButtonsStates["SendDeclaration"] = true;
                //    MenuButtonsEvents.MenuButtonsStateChanged.emit(args);
                //}, 100);
                //save button
                if (_this.CurrentSession.CurrentEditComponent) {
                    _this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = true;
                }
                serviceResponse.Result = new DisplayOnlyCheckResult(true, message);
                return Rx_1.Observable.of(serviceResponse);
            });
        }
        //other declaration checks
        declarationValidator.DeclarationViewDisplayOnlyChecks();
        if (declarationValidator.ValidationErrorMessageCodes.length > 0) {
            return Rx_1.Observable.defer(function () {
                var message = TextCodeTranslator_1.TextCodeTranslator.Translate(declarationValidator.ValidationErrorMessageCodes[0]);
                serviceResponse.Result = new DisplayOnlyCheckResult(true, message);
                //for menu buttons
                //this.timerToken = setTimeout(() => {
                //    var args: MenuButtonsStateChangedEventArgs = new MenuButtonsStateChangedEventArgs();
                //    args.MenuButtonsStates = {};
                //    args.MenuButtonsStates["SendDeclaration"] = true;
                //    MenuButtonsEvents.MenuButtonsStateChanged.emit(args);
                //}, 100);
                //save button
                if (_this.CurrentSession.CurrentEditComponent) {
                    _this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = true;
                }
                return Rx_1.Observable.of(serviceResponse);
            });
        }
        if (editComponentNeedsRefresh && this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefreshMessage != null) {
            return Rx_1.Observable.defer(function () {
                var text = _this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefreshMessage;
                serviceResponse.Result = new DisplayOnlyCheckResult(true, text);
                return Rx_1.Observable.of(serviceResponse);
            });
        }
        // Request sheets in progress check
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this.http.get(_this.apiUrl + '/GetRequestInProgress/?' + 'tenant=' + entityPM.Tenant + '&interfaceTypeCode= 2750' + '&objectTableId1=' + "" + '&entityId1=' + "" + '&objectTableId2=' + "" + '&entityId2=' + "" + '&customFileNo=' + entityPM.CustomFileNo + '&displayOnlyMode= true', { headers: authHeader })
                .map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                var requestSheets = response.json();
                if ((requestSheets == null || requestSheets.length == 0) && !editComponentNeedsRefresh) {
                    if (_this.CurrentSession.CurrentEditComponent) {
                        _this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = false;
                        _this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = false;
                    }
                    serviceResponse.Result = new DisplayOnlyCheckResult(false, "");
                    return serviceResponse;
                }
                else if ((requestSheets[0].InterfaceTypeCode == null || requestSheets[0].InterfaceTypeCode == undefined) && !editComponentNeedsRefresh) { //DUMMY From ITZIK
                    if (_this.CurrentSession.CurrentEditComponent) {
                        _this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = false;
                        _this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = false;
                    }
                    serviceResponse.Result = new DisplayOnlyCheckResult(false, "");
                    return serviceResponse;
                    //if (this.entityPM.ConcurrencyGUID != requestSheets[0].MainEntityConcurrencyGUID) {
                    //    //this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe
                    //    //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    //}
                }
                else {
                    if (_this.CurrentSession.CurrentEditComponent) {
                        _this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = true;
                        if (_this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest) {
                            _this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefresh = true;
                            editComponentNeedsRefresh = _this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefresh;
                        }
                    }
                    var returnDefualt = function () {
                        var RequestInProgressInterfaceTypeName = requestSheets[0].InterfaceTypeName;
                        var text = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.RequestInProgress");
                        text = text.replace('{0}', RequestInProgressInterfaceTypeName);
                        if (editComponentNeedsRefresh == true) {
                            _this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefreshMessage = text;
                        }
                        serviceResponse.Result = new DisplayOnlyCheckResult(true, text);
                        return serviceResponse;
                    };
                    if (requestSheets[0].InterfaceTypeCode == "2755" && requestSheets[0].FutureSendDateTime) {
                        var myFutureSendDateTime;
                        myFutureSendDateTime = new Date(requestSheets[0].FutureSendDateTime);
                        if (myFutureSendDateTime.valueOf() > Date.now().valueOf()) {
                            var datetimeParts = Tools_1.DateTool.GetDateParts(myFutureSendDateTime);
                            var stringOfYear = Tools_1.AppTool.PadLeft("" + datetimeParts.Year, 4, '0');
                            var stringOfMonth = Tools_1.AppTool.PadLeft("" + datetimeParts.Month, 2, '0');
                            var stringOfDay = Tools_1.AppTool.PadLeft("" + datetimeParts.Day, 2, '0');
                            var stringOfHours = Tools_1.AppTool.PadLeft("" + datetimeParts.Hours, 2, '0');
                            var stringOfHours12 = Tools_1.AppTool.PadLeft("" + datetimeParts.Hours12, 2, '0');
                            var stringOfMinutes = Tools_1.AppTool.PadLeft("" + datetimeParts.Minutes, 2, '0');
                            var stringOfSeconds = Tools_1.AppTool.PadLeft("" + datetimeParts.Seconds, 2, '0');
                            var stringOfMilliseconds = Tools_1.AppTool.PadLeft("" + datetimeParts.Milliseconds, 3, '0');
                            var stringDatetime = stringOfYear + "-" + stringOfMonth + "-" + stringOfDay + " " + stringOfHours + ":" + stringOfMinutes + "";
                            //`לתצוגה בלבד - הוגדרה בקשה מתוזמנת לתאריך ${paymentPM.FuturePaymentDateTime}`;
                            //`לתצוגה בלבד - הוגדרה בקשה מתוזמנת לתאריך ${requestSheets[0].FutureSendDateTime} - לא ניתן להמשיך עד לסיום טיפול או ביטול הבקשה`;
                            var text = " \u05D4\u05D5\u05D2\u05D3\u05E8\u05D4 \u05D1\u05E7\u05E9\u05D4 \u05DE\u05EA\u05D5\u05D6\u05DE\u05E0\u05EA \u05DC\u05EA\u05D0\u05E8\u05D9\u05DA " + stringDatetime + " - \u05DC\u05D0 \u05E0\u05D9\u05EA\u05DF \u05DC\u05D4\u05DE\u05E9\u05D9\u05DA \u05E2\u05D3 \u05DC\u05E1\u05D9\u05D5\u05DD \u05D8\u05D9\u05E4\u05D5\u05DC \u05D0\u05D5 \u05D1\u05D9\u05D8\u05D5\u05DC \u05D4\u05D1\u05E7\u05E9\u05D4";
                            if (editComponentNeedsRefresh == true) {
                                _this.CurrentSession.CurrentEditComponent.EditComponentController.MustRefreshMessage = text;
                            }
                            serviceResponse.Result = new DisplayOnlyCheckResult(true, text);
                            return serviceResponse;
                        }
                        return returnDefualt();
                    }
                    else {
                        return returnDefualt();
                    }
                }
                //return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
        //}
    };
    DeclarationDisplayOnlyChecks.prototype.GetRequestByInterfaceTypeCode = function (entityPM) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this.http.get(_this.apiUrl + '/GetRequestByInterfaceTypeCode/?' + 'tenant=' + entityPM.Tenant + '&interfaceTypeCode=2755' + '&objectTableId1=' + "" + '&entityId1=' + "" + '&customFileNo=' + entityPM.CustomFileNo, { headers: authHeader })
                .map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                var requestSheets = response.json();
                serviceResponse.Result = requestSheets;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationDisplayOnlyChecks.prototype.GetAnyRequest = function (interfaceTypeCode, customFileNo, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this.http.get(_this.apiUrl + '/GetAnyRequest/?' + 'tenant=' + tenant + '&interfaceTypeCode=' + interfaceTypeCode + '&customFileNo=' + customFileNo, { headers: authHeader })
                .map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                var requestSheets = response.json();
                serviceResponse.Result = requestSheets;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationDisplayOnlyChecks.prototype.CheckIfRequestInProgress = function (interfaceTypeCode, customFileNo, tenant, displayOnlyMode) {
        var _this = this;
        if (displayOnlyMode === void 0) { displayOnlyMode = true; }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this.http.get(_this.apiUrl + '/GetRequestInProgress/?' + 'tenant=' + tenant + '&interfaceTypeCode=' + interfaceTypeCode + '&objectTableId1=' + "" + '&entityId1=' + "" + '&objectTableId2=' + "" + '&entityId2=' + "" + '&customFileNo=' + customFileNo + '&displayOnlyMode=' + displayOnlyMode, { headers: authHeader })
                .map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                var requestSheets = response.json();
                serviceResponse.Result = requestSheets;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationDisplayOnlyChecks.prototype.CheckIfGeneralRequestInProgress = function (interfaceTypeCode, customFileNo, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this.http.get(_this.apiUrl + '/GetGeneralRequestInProgress/?' + 'tenant=' + tenant + '&interfaceTypeCode=' + interfaceTypeCode + '&objectTableId1=' + "" + '&entityId1=' + "" + '&objectTableId2=' + "" + '&entityId2=' + "" + '&customFileNo=' + customFileNo, { headers: authHeader })
                .map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                var requestSheets = response.json();
                serviceResponse.Result = requestSheets;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    return DeclarationDisplayOnlyChecks;
}());
exports.DeclarationDisplayOnlyChecks = DeclarationDisplayOnlyChecks;
var DisplayOnlyCheckResult = /** @class */ (function () {
    function DisplayOnlyCheckResult(IsDisplayOnly, DisplayOnlyMessage) {
        this.IsDisplayOnly = IsDisplayOnly;
        this.DisplayOnlyMessage = DisplayOnlyMessage;
    }
    return DisplayOnlyCheckResult;
}());
exports.DisplayOnlyCheckResult = DisplayOnlyCheckResult;
//# sourceMappingURL=DeclarationDisplayOnlyChecks.js.map