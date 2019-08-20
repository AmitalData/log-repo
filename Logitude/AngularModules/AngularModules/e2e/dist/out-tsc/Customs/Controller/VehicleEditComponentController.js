"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AmitalGatewayUtil_1 = require("../../Infrastructure/Utilities/AmitalGatewayUtil");
var Tools_1 = require("../../Infrastructure/Tools");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var MenuButtonsEvents_1 = require("../../Infrastructure/Utilities/events/MenuButtonsEvents");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var VehicleEditComponentController = /** @class */ (function () {
    function VehicleEditComponentController() {
        this.MustRefresh = null;
        this.MustRefreshMessage = null;
        this.IsInBatchRequest = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._ControllerOn = false;
        this.HaveSaved = false;
        this.ToCancell = false;
        this.InDisplayMode = false;
    }
    VehicleEditComponentController.prototype.OnFirstTimeAfterSingleDataLoaded = function (CurrentEntity) {
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
            //if (!(this._CurrentEntity.IsConvertedVehicle || this._CurrentEntity.IsConnectedToUnifreight)) {
            _this.RaiseLockIIGEntReturnEntityAlreadyLock(resolve);
        });
    };
    VehicleEditComponentController.prototype.RaiseLockIIGEntReturnEntityAlreadyLock = function (resolve) {
        var _this = this;
        var sub = AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(function (myUnifreightMessageM) {
            if (myUnifreightMessageM.LogitudeViewModel == "VehicleEditComponentController" &&
                (myUnifreightMessageM.LogitudeEntity == "Customs.Vehicle" || myUnifreightMessageM.LogitudeEntity == "Vehicle") &&
                myUnifreightMessageM.LogitudeEntityNumber == _this._CurrentEntity.Id) {
                sub.unsubscribe();
                //let ResponseInstructionCancel = this.GetBoolean(myUnifreightMessageM, AmitalGatewayUtil.Instance.VehicleMessaging.ResponseInstructionCancel)
                //if (ResponseInstructionCancel) {
                //    this.ToCancell = true;
                //    resolve(this._ControllerOn);
                //    //this.CurrentSession.RealCloseCurrentEditComponent();
                //    return;
                //}
                _this._InDisplayModeCFIFILMLockMMessage = "";
                var IsAlreadyLock = _this.GetBoolean(myUnifreightMessageM, AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.GeneralMessaging.ResponseEntityAlreadyLockKey);
                if (IsAlreadyLock) {
                    _this._InDisplayModeCFIFILMLockMMessage = "הרכב בשימוש במסוף אחר";
                    _this._UnifaceExclusiveAlreadyLocked = _this.InDisplayMode = true;
                    var responseLockMessgae = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(myUnifreightMessageM, AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.GeneralMessaging.ResponseEntityAlreadyLockMessage);
                    if (!Tools_1.AppTool.IsNullOrEmpty(responseLockMessgae)) {
                        _this._InDisplayModeCFIFILMLockMMessage = responseLockMessgae;
                    }
                    //this.ToCancell = true;///messageAndExit
                    //**********to lock menu buttons and save button --- mohammad bug 30289***************************//
                    var args = new MenuButtonsEvents_1.MenuButtonsStateChangedEventArgs();
                    args.MenuButtonsStates = {};
                    args.MenuButtonsStates["SendVehicle"] = true;
                    args.MenuButtonsStates["DeleteVehicle"] = true;
                    MenuButtonsEvents_1.MenuButtonsEvents.MenuButtonsStateChanged.emit(args);
                    if (_this.CurrentSession.CurrentEditComponent) {
                        _this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = true;
                    }
                    //**************************************************************************//
                    _this.CurrentSession.StopBusyIndicator();
                    var message = new MessageWindow_1.MessageWindow();
                    message.Width = 350;
                    message.Height = 180;
                    message.Title = "הרכב בשימוש במסוף אחר";
                    message.Show(_this._InDisplayModeCFIFILMLockMMessage);
                    message.WindowClosed.subscribe(function (res) {
                        _this.ToCancell = true;
                        resolve(_this._ControllerOn);
                    });
                    //resolve(this._ControllerOn); 
                    return;
                }
                _this._UnifaceExclusiveAlreadyLocked = _this.InDisplayMode = false;
                _this._InDisplayModeCFIFILMLockMMessage = "";
                resolve(_this._ControllerOn);
            }
        });
        AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.GeneralMessaging
            .RaiseLockIIGEntityReturnEntityAlreadyLock("Vehicle", this._CurrentEntity.Id, "VehicleEditComponentController");
    };
    VehicleEditComponentController.prototype.OnReloadEntityPM = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            if (!_this._ControllerOn) {
                resolve();
                return;
            }
            if (!_this._UnifaceExclusiveAlreadyLocked) {
                resolve();
                return;
            }
            _this.RaiseLockIIGEntReturnEntityAlreadyLock(resolve);
        });
    };
    VehicleEditComponentController.prototype.OnCloseEditControl = function () {
        if (!this._ControllerOn) {
            return;
        }
        if (!this._UnifaceExclusiveAlreadyLocked) {
            AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.GeneralMessaging.RaiseUnlockIIGEntity(this._CurrentEntity.Id, this._CurrentEntity.Id, this.HaveSaved);
        }
    };
    VehicleEditComponentController.prototype.GetBoolean = function (myUnifreightMessageM, theKey) {
        var myBool = false;
        myBool = (AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(myUnifreightMessageM, theKey).toLowerCase() == 'true');
        return myBool;
    };
    Object.defineProperty(VehicleEditComponentController.prototype, "InDisplayModeMessage", {
        get: function () { return this._InDisplayModeCFIFILMLockMMessage; },
        set: function (value) { this._InDisplayModeCFIFILMLockMMessage = value; },
        enumerable: true,
        configurable: true
    });
    VehicleEditComponentController.prototype.UnifaceStartAsLock = function (lockMess) {
        this.InDisplayMode = true;
        this.InDisplayModeMessage = lockMess;
        this._ControllerOn = true;
        this._UnifaceExclusiveAlreadyLocked = true;
    };
    VehicleEditComponentController.prototype.ResetMustRefresh = function () {
        this.MustRefresh = null;
        this.MustRefreshMessage = null;
    };
    VehicleEditComponentController.prototype.IsDisabled = function (itemTabCode) {
        return false;
    };
    return VehicleEditComponentController;
}());
exports.VehicleEditComponentController = VehicleEditComponentController;
//# sourceMappingURL=VehicleEditComponentController.js.map