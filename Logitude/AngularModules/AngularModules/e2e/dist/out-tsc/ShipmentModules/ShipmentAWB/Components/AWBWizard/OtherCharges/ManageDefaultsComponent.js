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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var ChargesTypeListService_1 = require("../../../../../Common/Services/StandardLists/ChargesTypeListService");
var CommonDomainService_1 = require("../../../../../Common/Services/CommonDomainService");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ManageDefaultsComponent = /** @class */ (function () {
    function ManageDefaultsComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.AllChargesList = [];
        this.AutoDisplaylist = [];
    }
    ManageDefaultsComponent.prototype.SetWindowArgs = function (ShipmentLevelCode) {
        this.ShipmentLevelCode = ShipmentLevelCode;
        this.ObjectTableName = this.ShipmentLevelCode == "C" ? "Consolidation" : "Shipment";
        this.CurrentSession.StartBusyIndicatorLoading();
        this.LoadData();
    };
    Object.defineProperty(ManageDefaultsComponent.prototype, "SearchText", {
        get: function () { return this.mySearchText; },
        set: function (newValue) {
            if (this.mySearchText != newValue) {
                this.mySearchText = newValue;
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    ManageDefaultsComponent.prototype.LoadData = function () {
        var _this = this;
        if (this.myService == null) {
            this.myService = new ChargesTypeListService_1.ChargesTypeListService();
        }
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 50;
        filters.SortBy = "Code";
        filters.SortDirection = "Descending";
        filters.Filter1Name = "IsAir";
        filters.Filter1Value = true;
        filters.Filter1Operator = "Equals";
        filters.Filter2Name = "InActive";
        filters.Filter2Value = false;
        filters.Filter2Operator = "Equals";
        filters.Filter3Name = "ChargesGroupCode";
        filters.Filter3Value = "FRT";
        filters.Filter3Operator = "NotEqual";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.mySearchText)) {
            filters.Filter4Name = "SearchFields";
            filters.Filter4Value = this.mySearchText;
            filters.Filter4Operator = "Contains";
        }
        this.AllChargesList = [];
        this.AutoDisplaylist = [];
        this.myService.getByFilters(filters).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    list.forEach(function (item) {
                        if (_this.ShipmentLevelCode == "C") {
                            if (item.IsAutoDisplayInConsolidation) {
                                _this.AutoDisplaylist.push(new AutoDisplayItemViewModel(item));
                            }
                            else {
                                _this.AllChargesList.push(new AutoDisplayItemViewModel(item));
                            }
                        }
                        else {
                            if (item.IsAutoDisplayInShipment) {
                                _this.AutoDisplaylist.push(new AutoDisplayItemViewModel(item));
                            }
                            else {
                                _this.AllChargesList.push(new AutoDisplayItemViewModel(item));
                            }
                        }
                    });
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    Object.defineProperty(ManageDefaultsComponent.prototype, "AllChargesHeader", {
        get: function () {
            var myResult = "All Charges";
            if (this.AllChargesList != null) {
                myResult += " (" + this.AllChargesList.length + ")";
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ManageDefaultsComponent.prototype, "AutoDisplayHeader", {
        get: function () {
            var myResult = "Auto Display";
            if (this.ShipmentLevelCode == "C") {
                myResult += " in Consolidation";
            }
            else {
                myResult += " in Shipment";
            }
            if (this.AllChargesList != null) {
                myResult += " (" + this.AutoDisplaylist.length + ")";
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    ManageDefaultsComponent.prototype.AddClicked = function () {
        var myChargeId;
        var item = this.SelectedAllChargesItem;
        if (item != null) {
            myChargeId = item.Id;
            var index1 = this.AllChargesList.indexOf(item);
            if (index1 > -1) {
                this.AllChargesList.splice(index1, 1);
            }
            var index2 = this.AutoDisplaylist.indexOf(item);
            if (index2 == -1) {
                this.AutoDisplaylist.push(item);
            }
        }
        this.SelectedAllChargesItem = null;
        this.InvokeEditChargeType(myChargeId, true);
    };
    ManageDefaultsComponent.prototype.RemoveClicked = function () {
        var myChargeId;
        var item = this.SelectedAutoDisplayItem;
        if (item != null) {
            myChargeId = item.Id;
            var index1 = this.AutoDisplaylist.indexOf(item);
            if (index1 > -1) {
                this.AutoDisplaylist.splice(index1, 1);
            }
            var index2 = this.AllChargesList.indexOf(item);
            if (index2 == -1) {
                this.AllChargesList.push(item);
            }
        }
        this.SelectedAutoDisplayItem = null;
        this.InvokeEditChargeType(myChargeId, false);
    };
    ManageDefaultsComponent.prototype.InvokeEditChargeType = function (myChargeId, isAutoDisplay) {
        var myPropertyTypeCode = "S";
        if (this.ShipmentLevelCode == "C") {
            myPropertyTypeCode = "C";
        }
        if (this.myDomainService == null) {
            this.myDomainService = new CommonDomainService_1.CommonDomainService();
        }
        this.myDomainService.InvokeUpdateAutoDisplay(myChargeId, myPropertyTypeCode, isAutoDisplay).subscribe(function (myResult) {
        });
    };
    ManageDefaultsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ManageDefaultsComponent.prototype.EditChargeType = function (item) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Edit Charges Type";
        logWindow.IsFillScreen = true;
        logWindow.ShowEditComponent(item.Id, "ChargesType");
    };
    ManageDefaultsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ManageDefaultsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ManageDefaultsComponent);
    return ManageDefaultsComponent;
}());
exports.ManageDefaultsComponent = ManageDefaultsComponent;
var AutoDisplayItemViewModel = /** @class */ (function () {
    function AutoDisplayItemViewModel(item) {
        this.item = item;
        this.Id = item.Id;
        this.Code = item.Code;
        this.Name = item.EnglishName;
    }
    return AutoDisplayItemViewModel;
}());
//# sourceMappingURL=ManageDefaultsComponent.js.map