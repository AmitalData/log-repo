"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ServiceResponse_1 = require("../DataContracts/ServiceResponse");
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var Tools_1 = require("../Tools");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var Guid_1 = require("../Utilities/Guid");
var SessionInfo_1 = require("../Utilities/SessionInfo");
var SessionLocator_1 = require("../Utilities/SessionLocator");
var LogitudeErrorHandler_1 = require("../Utilities/LogitudeErrorHandler");
var LoginService_1 = require("../Services/LoginService");
var ServiceHelper = /** @class */ (function () {
    function ServiceHelper() {
    }
    ServiceHelper.HandleServiceError = function (error) {
        var response;
        response = new ServiceResponse_1.ServiceResponse();
        response.HasError = true;
        if (error instanceof http_1.Response) {
            //var mm = error.json();
            if (error.status == 400) {
                var apiException = error.json();
                if (apiException.ErrorType == "Exception" || apiException.ErrorType == "ModelStateError" || apiException.ErrorType == "DbEntityValidationException" || apiException.ErrorType == "ApplicationException") {
                    var errorMessage = apiException.ShortErrorMessage;
                    if (apiException.ShortErrorMessage) {
                        errorMessage = apiException.ShortErrorMessage;
                        // response.ErrorsArray.push(apiException.ShortErrorMessage);
                    }
                    else {
                        errorMessage = apiException.ExceptionMessage;
                        //response.ErrorsArray.push(apiException.ExceptionMessage);
                    }
                    if (errorMessage) {
                        if (errorMessage.indexOf('session expiration') == -1) {
                            if (errorMessage.indexOf(';') != -1) {
                                var errArray = errorMessage.split(';');
                                for (var k in errArray) {
                                    response.ErrorsArray.push(errArray[k]);
                                }
                            }
                            else {
                                response.ErrorsArray.push(errorMessage);
                            }
                        }
                    }
                }
                else if (apiException.ErrorType == "AutenticationException") {
                    if (!SessionLocator_1.SessionLocator.IsSiguOut) {
                        SessionLocator_1.SessionLocator.HomeComponent.SignoutClicked();
                    }
                }
                // Ayman: please leave this commented
                else if (apiException.ErrorType == "OptimisticConcurrencyException") {
                    //var message: string = TextCodeTranslator.Translate("General.M.CantUpdateRecord");
                    response.ErrorsArray.push(apiException.ShortErrorMessage);
                }
                else {
                    ServiceHelper.LogServiceError(apiException.ShortErrorMessage, apiException.ErrorMessage);
                }
            }
            else {
                if (error.status == 0) {
                    ServiceHelper.LogServiceError("There seems to be an Internet Connection Problem", "net::ERR_CONNECTION_REFUSED", false); //("net::ERR_CONNECTION_REFUSED", "net::ERR_CONNECTION_REFUSED");
                }
                else if (error.status == 500) {
                    try {
                        var errorObject = JSON.parse(error["_body"]);
                        ServiceHelper.LogServiceError(errorObject.Message + " " + errorObject.ExceptionMessage, errorObject.StackTrace);
                    }
                    catch (e) { }
                    //ServiceHelper._LogitudeErrorHandler.handleError(error);
                }
            }
        }
        else {
            var exceptionmessage = error.message + '\n' + error.stack;
            response.ErrorsArray.push(exceptionmessage);
            ServiceHelper._LogitudeErrorHandler.handleError(error);
        }
        return Rx_1.Observable.of(response);
    };
    ServiceHelper.HandleTimerServiceError = function (error) {
        var response;
        response = new ServiceResponse_1.ServiceResponse();
        response.HasError = true;
        if (error instanceof http_1.Response) {
            //var mm = error.json();
            if (error.status == 400) {
                var apiException = error.json();
                if (apiException.ErrorType == "Exception" || apiException.ErrorType == "ModelStateError" || apiException.ErrorType == "DbEntityValidationException" || apiException.ErrorType == "ApplicationException") {
                    var errorMessage = apiException.ShortErrorMessage;
                    if (apiException.ShortErrorMessage) {
                        errorMessage = apiException.ShortErrorMessage;
                    }
                    else {
                        errorMessage = apiException.ExceptionMessage;
                    }
                    if (errorMessage) {
                        if (errorMessage.indexOf('session expiration') == -1) {
                            if (errorMessage.indexOf(';') != -1) {
                                var errArray = errorMessage.split(';');
                                for (var k in errArray) {
                                    response.ErrorsArray.push(errArray[k]);
                                }
                            }
                            else {
                                response.ErrorsArray.push(errorMessage);
                            }
                        }
                    }
                }
                else if (apiException.ErrorType == "AutenticationException") {
                    if (!SessionLocator_1.SessionLocator.IsSiguOut) {
                        if (errorMessage != "") {
                            SessionLocator_1.SessionLocator.HomeComponent.SignoutClicked();
                        }
                    }
                }
                else {
                    console.error(apiException.ShortErrorMessage);
                    //ServiceHelper.LogServiceError(apiException.ShortErrorMessage, apiException.ErrorMessage);
                }
            }
            else {
                if (error.status == 0) {
                    console.error("net::ERR_CONNECTION_REFUSED");
                    //ServiceHelper.LogServiceError("net::ERR_CONNECTION_REFUSED", "net::ERR_CONNECTION_REFUSED");
                }
                else {
                }
            }
        }
        else {
            var exceptionmessage = error.message + '\n' + error.stack;
            response.ErrorsArray.push(exceptionmessage);
        }
        return Rx_1.Observable.of(response);
    };
    ServiceHelper.LogServiceError = function (exception, stackTrace, logException) {
        var _this = this;
        if (logException === void 0) { logException = true; }
        try {
            if (exception) {
                if (this.CurrentSession) {
                    this.CurrentSession.StopBusyIndicator();
                    if (!this.CurrentSession.IsShowErrorWindow) {
                        this.CurrentSession.IsShowErrorWindow = true;
                        var mywindow = new MessageWindow_1.MessageWindow();
                        mywindow.Show(exception);
                        mywindow.WindowClosed.subscribe(function ($event) {
                            _this.CurrentSession.IsShowErrorWindow = false;
                            if (exception) {
                                if (exception.indexOf("Internet Connection Problem") > -1) {
                                    var loginService = new LoginService_1.LoginService();
                                    loginService.GetDocumentDownloadToken().subscribe(function (myResult) {
                                        if (myResult) {
                                            SessionInfo_1.SessionInfo.DocumentDownloadToken = myResult;
                                        }
                                    });
                                }
                            }
                        });
                    }
                }
            }
            if (exception && stackTrace && logException === true) {
                var errorLog = new ErrorLogPM();
                errorLog.Id = Guid_1.Guid.newGuid();
                errorLog.ClientDate = new Date();
                errorLog.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                errorLog.Tier = "Client";
                errorLog.UserName = SessionInfo_1.SessionInfo.LoggedUserEmail;
                errorLog.Exception = exception;
                errorLog.StackTrace = stackTrace;
                window.sessionStorage.setItem(["ErrorLogs", errorLog.Id], JSON.stringify(errorLog));
                console.error(exception);
            }
        }
        catch (e) {
            console.error(e);
        }
    };
    ServiceHelper.CloneObject = function (sourceObject, targetObject) {
        if (targetObject === void 0) { targetObject = null; }
        if (!targetObject) {
            targetObject = {};
        }
        var jsonPMKeys = Object.keys(sourceObject);
        for (var key in jsonPMKeys) {
            //if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
            //    continue;
            //}
            var property = jsonPMKeys[key];
            targetObject[property] = sourceObject[property];
        }
        return targetObject;
    };
    ServiceHelper.DeepClone = function (currentObject) {
        var newObj = currentObject;
        if (currentObject && typeof currentObject === "object") {
            if (Object.prototype.toString.call(currentObject) === "[object Array]") {
                newObj = [];
            }
            else {
                newObj = Object.create(currentObject);
            }
            for (var i in currentObject) {
                newObj[i] = this.DeepClone(currentObject[i]);
            }
        }
        return newObj;
    };
    ServiceHelper.CloneEntityPM = function (currentObject) {
        var newObj = currentObject;
        if (currentObject && typeof currentObject === "object") {
            if (Object.prototype.toString.call(currentObject) === "[object Array]") {
                newObj = [];
            }
            else {
                newObj = Object.create(currentObject);
            }
            for (var i in currentObject) {
                if (typeof (currentObject[i]) === "object") {
                    continue;
                }
                else {
                    newObj[i] = this.DeepClone(currentObject[i]);
                }
            }
        }
        newObj.IsDirty = currentObject.IsDirty;
        currentObject.MyClone = newObj;
        return newObj;
    };
    ServiceHelper.RejectEntityPMChanges = function (currentObject) {
        var oldObj = currentObject.MyClone;
        if (oldObj) {
            var isDirty = currentObject.MyClone.IsDirty;
            if (oldObj && typeof oldObj === "object") {
                oldObj.MyClone = null;
                for (var i in oldObj) {
                    if (typeof (oldObj[i]) === "object") {
                        continue;
                    }
                    else {
                        currentObject[i] = this.DeepClone(oldObj[i]);
                    }
                }
            }
            currentObject.IsDirty = isDirty;
        }
        return currentObject;
    };
    ServiceHelper.base64ToBufferConvertor = function (str) {
        str = window.atob(str); // creates a ASCII string
        var buffer = new ArrayBuffer(str.length), view = new Uint8Array(buffer);
        for (var i = 0; i < str.length; i++) {
            view[i] = str.charCodeAt(i);
        }
        return buffer;
    };
    ServiceHelper.GetLogitudeURL = function () {
        return Tools_1.AppTool.GetLogitudeURL();
    };
    ServiceHelper.GetDateString = function (givenDate) {
        var myResult = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(givenDate)) {
            var DateParts = Tools_1.DateTool.GetDateParts(givenDate);
            myResult = DateParts.Year + ":" + DateParts.Month + ":" + DateParts.Day + ":" + DateParts.Hours + ":" + DateParts.Minutes + ":" + DateParts.Seconds;
        }
        return myResult;
    };
    ServiceHelper.MapJsonToEntity = function (jsonList, classType) {
        var entityList;
        entityList = new classType();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    ServiceHelper.MapJsonToArrayofEntities = function (jsonArray, classType) {
        var mappedArray = [];
        for (var key in jsonArray) {
            var entity = jsonArray[key];
            mappedArray.push(ServiceHelper.MapJsonToEntity(entity, classType));
        }
        return mappedArray;
    };
    ServiceHelper.GetLoggedUserToken = function () {
        return SessionInfo_1.SessionInfo.Token;
    };
    ServiceHelper.GetLDocumentDownloadToken = function () {
        return SessionInfo_1.SessionInfo.DocumentDownloadToken;
    };
    ServiceHelper.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    ServiceHelper._LogitudeErrorHandler = new LogitudeErrorHandler_1.LogitudeErrorHandler();
    return ServiceHelper;
}());
exports.ServiceHelper = ServiceHelper;
var ErrorLogPM = /** @class */ (function () {
    function ErrorLogPM() {
    }
    return ErrorLogPM;
}());
//# sourceMappingURL=ServiceHelper.js.map