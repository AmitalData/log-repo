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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FBLStockExtenedPMService_1 = require("../../../../Shipment/Services/ExtendedPMs/FBLStockExtenedPMService");
var FBLStockMainComponent = /** @class */ (function () {
    function FBLStockMainComponent(entityArgs) {
        var _this = this;
        this.entityArgs = entityArgs;
        this.ObjectTableName = "FBLStock";
        this.ItemsCount = 0;
        this.ItemsSource = [];
        this.IsVisibile = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Commands
        this.SelectedItem = null;
        this._entityResourceService.getEntityResourceByTableName("FBLStock", 0).subscribe(function (response) {
            _this.IsVisibile = true;
            //this.EntityPM = entityArgs.EntityPM;
            _this.FBLStockExtenedPMService = new FBLStockExtenedPMService_1.FBLStockExtenedPMService();
            _this.LoadData();
        });
    }
    FBLStockMainComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.SelectedItem = null;
        this.FBLStockExtenedPMService.GetAllFBLStockPMsByTenant(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.BuildItemsSource(myResponse.Result);
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    FBLStockMainComponent.prototype.BuildItemsSource = function (items) {
        var itemsCount = 0;
        var itemsSource = [];
        if (items != null) {
            itemsCount = items.length;
            itemsSource = items;
        }
        this.ItemsCount = itemsCount;
        this.ItemsSource = itemsSource;
    };
    Object.defineProperty(FBLStockMainComponent.prototype, "IsAddEnabled", {
        get: function () {
            var myResult = true;
            //if (this.EntityPM.CheckDigit) {
            //    myResult = true;
            //}
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FBLStockMainComponent.prototype, "IsRemoveEnabled", {
        get: function () {
            var myResult = false;
            if (this.SelectedItem != null) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    FBLStockMainComponent.prototype.AddStockClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("FBLStock.O.NewFBLNumbers");
        logWindow.WindowArgs = { FBLStocksList: this.ItemsSource };
        logWindow.Show('./ShipmentModules/ShipmentStock/Components/FBLStock/NewFBLStockComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s == "OK") {
                _this.LoadData();
            }
        });
    };
    //AssignClicked() {
    //    var logWindow = new LogitudeWindow();
    //    logWindow.Title = "Assign to shipper";
    //    logWindow.WindowArgs = { AirlineId: this.EntityPM.Id, IsCustomerMode: false };
    //    logWindow.Show('./Common/Components/Partners/AWBStock/AssignComponent');
    //    logWindow.WindowClosed.subscribe(s => {
    //        if (s == "OK") {
    //            this.LoadData();
    //        }
    //    });
    //}
    FBLStockMainComponent.prototype.RemoveClicked = function () {
        this.Remove(false);
    };
    FBLStockMainComponent.prototype.RemoveSeriesClicked = function () {
        this.Remove(true);
    };
    FBLStockMainComponent.prototype.Remove = function (isDeletingSeries) {
        var _this = this;
        if (this.SelectedItem != null) {
            if (isDeletingSeries == true && this.SelectedItem.InsertionDate == null) {
                var win = new MessageWindow_1.MessageWindow();
                win.Show("This stock does not have an insertion date");
            }
            else {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("FBLStock.O.RemovingStock");
                confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Delete");
                confirmWindow.NoButtonText = "Cancel";
                var message = TextCodeTranslator_1.TextCodeTranslator.Translate("FBLStock.M.DeleteStockNumber");
                if (isDeletingSeries) {
                    var myGetDateFormats = Tools_1.DateTool.GetDateFormats(this.SelectedItem.InsertionDate);
                    message = TextCodeTranslator_1.TextCodeTranslator.Translate("FBLStock.M.DeleteStockSeries");
                    message = message + "\n" + "[" + myGetDateFormats.ShortDateString + "] at [" + myGetDateFormats.ShortTimeString + "] ?";
                }
                else {
                    message = message + " [" + this.SelectedItem.Number + "] ?";
                }
                confirmWindow.Show(message);
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.CurrentSession.StartBusyIndicatorSaving();
                        _this.FBLStockExtenedPMService.DeleteFBLStocksOperation(_this.SelectedItem.Id, isDeletingSeries).subscribe(function (myResponse) {
                            _this.CurrentSession.StopBusyIndicator();
                            if (myResponse != null) {
                                if (myResponse.HasError) {
                                    var win = new MessageWindow_1.MessageWindow();
                                    win.Show("Remove Stock Failed: " + myResponse.ErrorsArray[0]);
                                }
                                else {
                                    _this.LoadData();
                                }
                            }
                        });
                    }
                });
            }
        }
    };
    FBLStockMainComponent.prototype.CloseClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    FBLStockMainComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FBLStockMainComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], FBLStockMainComponent);
    return FBLStockMainComponent;
}());
exports.FBLStockMainComponent = FBLStockMainComponent;
//# sourceMappingURL=FBLStockMainComponent.js.map