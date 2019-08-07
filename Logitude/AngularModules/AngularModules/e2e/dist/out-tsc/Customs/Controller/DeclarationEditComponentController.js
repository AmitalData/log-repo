"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AmitalGatewayUtil_1 = require("../../Infrastructure/Utilities/AmitalGatewayUtil");
var Tools_1 = require("../../Infrastructure/Tools");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var MenuButtonsEvents_1 = require("../../Infrastructure/Utilities/events/MenuButtonsEvents");
var DeclarationEditComponentController = /** @class */ (function () {
    function DeclarationEditComponentController() {
        this.MustRefresh = null;
        this.MustRefreshMessage = null;
        this.IsInBatchRequest = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._ControllerOn = false;
        this.HaveSaved = false;
        this.ToCancell = false;
        this.InDisplayMode = false;
        this.CustomsAnswersShowManifest = false;
        this.ShowDeclarationClassificationComponentTAB = false;
    }
    DeclarationEditComponentController.prototype.OnFirstTimeAfterSingleDataLoaded = function (CurrentEntity) {
        var _this = this;
        this._CurrentEntity = CurrentEntity;
        this._ControllerOn = true;
        return new Promise(function (resolve, reject) {
            if (!AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                _this._ControllerOn = false;
                resolve(_this._ControllerOn);
                return;
            }
            //alert(SessionLocator.AllSessions.length);
            //let myEditTab: SessionTabItem = this.Tabs[1];
            if (SessionLocator_1.SessionLocator.AllSessions.length == 2 &&
                SessionLocator_1.SessionLocator.AllSessions[0] != _this.CurrentSession) {
                //myEditTab no need to Check !!!
                _this._ControllerOn = false;
                resolve(_this._ControllerOn);
                return;
            }
            //if (!(this._CurrentEntity.IsConvertedDeclaration || this._CurrentEntity.IsConnectedToUnifreight)) {
            if (!(_this._CurrentEntity.IsConnectedToUnifreight)) {
                _this._ControllerOn = false;
                resolve(_this._ControllerOn);
                return;
            }
            _this.RaiseCFIFILMLockReturnCFIFILMAlreadyLock(resolve);
        });
    };
    DeclarationEditComponentController.prototype.RaiseCFIFILMLockReturnCFIFILMAlreadyLock = function (resolve) {
        var _this = this;
        var sub = AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(function (myUnifreightMessageM) {
            if (myUnifreightMessageM.LogitudeViewModel == "DeclarationEditComponentController" &&
                (myUnifreightMessageM.LogitudeEntity == "Customs.Declaration" || myUnifreightMessageM.LogitudeEntity == "Declaration") &&
                myUnifreightMessageM.LogitudeEntityNumber == _this._CurrentEntity.Id) {
                sub.unsubscribe();
                //let ResponseInstructionCancel = false;
                //let listRes = myUnifreightMessageM.Response.filter(itm =>
                //    itm[0] == AmitalGatewayUtil.Instance.DeclarationMessaging.ResponseInstructionCancel);
                //if (listRes.length > -1 && !AppTool.IsNullOrEmpty(listRes[0])) {
                //    if (!AppTool.IsNullOrEmpty(listRes[0][1])) {
                //        ResponseInstructionCancel = (listRes[0][1].toLowerCase() == 'true');
                //    }
                //}
                var ResponseInstructionCancel = _this.GetBoolean(myUnifreightMessageM, AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging.ResponseInstructionCancel);
                if (ResponseInstructionCancel) {
                    _this.ToCancell = true;
                    resolve(_this._ControllerOn);
                    //this.CurrentSession.RealCloseCurrentEditComponent();
                    return;
                }
                _this._InDisplayModeCFIFILMLockMMessage = "";
                var IsAlreadyLock = _this.GetBoolean(myUnifreightMessageM, AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging.ResponseCFIFILMAlreadyLockKey);
                if (IsAlreadyLock) {
                    _this._InDisplayModeCFIFILMLockMMessage = "ההצהרה נעולה";
                    _this._UnifaceExclusiveAlreadyLocked = _this.InDisplayMode = true;
                    var responseCFIFILMAlreadyLockMessgae = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(myUnifreightMessageM, AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging.ResponseCFIFILMAlreadyLockMessgae);
                    if (!Tools_1.AppTool.IsNullOrEmpty(responseCFIFILMAlreadyLockMessgae)) {
                        _this._InDisplayModeCFIFILMLockMMessage = responseCFIFILMAlreadyLockMessgae;
                    }
                    resolve(_this._ControllerOn);
                    //**********to lock menu buttons and save button --- mohammad bug 30289***************************//
                    var args = new MenuButtonsEvents_1.MenuButtonsStateChangedEventArgs();
                    args.MenuButtonsStates = {};
                    args.MenuButtonsStates["SendDeclaration"] = true;
                    MenuButtonsEvents_1.MenuButtonsEvents.MenuButtonsStateChanged.emit(args);
                    if (_this.CurrentSession.CurrentEditComponent) {
                        _this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = true;
                    }
                    //**************************************************************************//
                    //this.CurrentSession.RealCloseCurrentEditComponent();
                    return;
                }
                _this._UnifaceExclusiveAlreadyLocked = _this.InDisplayMode = false;
                _this._InDisplayModeCFIFILMLockMMessage = "";
                resolve(_this._ControllerOn);
            }
        });
        AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging
            .RaiseCFIFILMLockReturnCFIFILMAlreadyLock(this._CurrentEntity.CustomFileNo, this._CurrentEntity.Id, 
        //this.GetType().Name
        "DeclarationEditComponentController");
    };
    DeclarationEditComponentController.prototype.ForceCheckIfLockWhileReload = function () {
        this._UnifaceExclusiveAlreadyLocked = true;
    };
    DeclarationEditComponentController.prototype.OnReloadEntityPM = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            if (!_this._ControllerOn) {
                resolve();
                return;
            }
            if (!_this._UnifaceExclusiveAlreadyLocked) { ////if true then force Check is loc in every reload
                resolve();
                return;
            }
            _this.RaiseCFIFILMLockReturnCFIFILMAlreadyLock(resolve);
        });
    };
    DeclarationEditComponentController.prototype.OnCloseEditControl = function () {
        if (!this._ControllerOn) {
            return;
        }
        if (!this._UnifaceExclusiveAlreadyLocked
        ///&& this._CurrentEntity.IsConvertedDeclaration != true /// yuval +im - not need 
        ) //Yuval Chalup 25.11.2015 TASK-17450 (Add _CurrentEntity.IsConvertedDeclaration != true)
         {
            AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseUnlockCFIFILEM(this._CurrentEntity.CustomFileNo, this._CurrentEntity.Id, this.HaveSaved);
        }
    };
    DeclarationEditComponentController.prototype.GetBoolean = function (myUnifreightMessageM, theKey) {
        var myBool = false;
        myBool = (AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(myUnifreightMessageM, theKey).toLowerCase() == 'true');
        return myBool;
    };
    Object.defineProperty(DeclarationEditComponentController.prototype, "InDisplayModeMessage", {
        get: function () { return this._InDisplayModeCFIFILMLockMMessage; },
        set: function (value) { this._InDisplayModeCFIFILMLockMMessage = value; },
        enumerable: true,
        configurable: true
    });
    DeclarationEditComponentController.prototype.UnifaceStartAsLock = function (lockMess) {
        this.InDisplayMode = true;
        this.InDisplayModeMessage = lockMess;
        this._ControllerOn = true;
        this._UnifaceExclusiveAlreadyLocked = true;
    };
    DeclarationEditComponentController.prototype.ResetMustRefresh = function () {
        this.MustRefresh = null;
        this.MustRefreshMessage = null;
    };
    DeclarationEditComponentController.prototype.IsDisabled = function (itemTabCode) {
        if (itemTabCode != "DCCF") {
            return false;
        }
        if (this.ShowDeclarationClassificationComponentTAB) {
            return false;
        }
        return false;
    };
    return DeclarationEditComponentController;
}());
exports.DeclarationEditComponentController = DeclarationEditComponentController;
//# sourceMappingURL=DeclarationEditComponentController.js.map