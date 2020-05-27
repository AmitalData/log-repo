"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Args_1 = require("../../../../Common/Args");
var FBLStockFieldComponent = /** @class */ (function () {
    function FBLStockFieldComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsDataReady = false;
        this.IsFromStockVisible = false;
        this.IsFromStockEnabled = false;
        this.IsReturnStockVisible = false;
        this.IsReturnStockEnabled = false;
        this.HideCoumns = false;
        this.isGetFromStock = false;
    }
    FBLStockFieldComponent.prototype.SetUIProperties_StockButton = function () {
        var setting = SessionLocator_1.SessionLocator.TenantSettings.filter(function (s) { return s.SettingCode == "HAWBCounterO_E_D"; })[0];
        if (this.DataContext.TransportModeId == "O" && (this.DataContext.DirectionId == "E" || this.DataContext.DirectionId == "D") && this.DataContext.ShipmentLevelCode != "C" && setting != null && setting.SettingValue == "Stock") {
            var isFromStockVisible = false;
            var isReturnStockVisible = false;
            var isFromStockEnabled = true;
            var isReturnStockEnabled = true;
            if (this.DataContext.FBLIsFromStock || this.DataContext.FBLTakenFromStock) {
                isReturnStockVisible = true;
                isFromStockEnabled = false;
            }
            isFromStockVisible = !isReturnStockVisible;
            this.IsFromStockVisible = isFromStockVisible;
            this.IsFromStockEnabled = isFromStockEnabled;
            this.IsReturnStockVisible = isReturnStockVisible;
            this.IsReturnStockEnabled = isReturnStockEnabled;
            this.DataContext.UIProperties.SetEnabled("House", this.ObjectTableName, isFromStockEnabled);
            this.HideCoumns = true;
        }
        else {
            this.IsFromStockVisible = false;
            this.IsReturnStockVisible = false;
        }
    };
    FBLStockFieldComponent.prototype.Run = function (args) {
        this.DataContext = args['DataContext'];
        this.ObjectField = args['ObjectField'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsNewEntityCall = args['IsNewEntityCall'];
        this.SetUIProperties_StockButton();
        this.IsDataReady = true;
    };
    FBLStockFieldComponent.prototype.GetFBLStockClicked = function () {
        var _this = this;
        var windowArgs = new Args_1.GetStackWindowArgs();
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 750;
        logWindow.Height = 450;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = "Select FBL Number";
        logWindow.Show('./ShipmentModules/ShipmentStock/Components/FBLStock/FBLStackSelectionComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                if (windowArgs.SelectedFBLStock != null) {
                    var stackNumber = windowArgs.SelectedFBLStock.Number;
                    _this.myOldFBLStockNumber = _this.DataContext.FBLStockNumber;
                    _this.DataContext.FBLTakenFromStock = true;
                    _this.DataContext.FBLStockNumber = stackNumber.toString();
                    _this.isGetFromStock = true;
                    _this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                        if (isSaveSuccess) {
                            _this.DataContext = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        }
                        else {
                            if (_this.isGetFromStock) {
                                _this.DataContext.FBLTakenFromStock = false;
                                _this.DataContext.FBLStockNumber = _this.myOldFBLStockNumber;
                                _this.isGetFromStock = false;
                            }
                        }
                        _this.SetUIProperties_StockButton();
                    });
                    _this.CurrentSession.CurrentEditComponent.SaveChanges();
                }
            }
        });
    };
    FBLStockFieldComponent.prototype.ReturnFBLStockClicked = function () {
        var _this = this;
        if (this.DataContext.FBLIsFromStock || this.DataContext.FBLTakenFromStock) {
            //this.SetMAWBAirline();
            this.DataContext.FBLReturnedToStock = true;
            this.DataContext.FBLStockNumber = this.DataContext.House;
            this.isGetFromStock = false;
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.DataContext = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
                else {
                    //if (this.isGetFromStock) {
                    //    this.DataContext.FBLTakenFromStock = false;
                    //    this.DataContext.FBLStockNumber = this.myOldFBLStockNumber;
                    //    this.isGetFromStock = false;
                    //}
                }
                _this.SetUIProperties_StockButton();
            });
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    };
    FBLStockFieldComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'FBLStockFieldComponent',
            templateUrl: './FBLStockFieldComponent.html',
        })
    ], FBLStockFieldComponent);
    return FBLStockFieldComponent;
}());
exports.FBLStockFieldComponent = FBLStockFieldComponent;
//# sourceMappingURL=FBLStockFieldComponent.js.map