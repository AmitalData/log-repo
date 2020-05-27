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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var AWBDescriptionOfGoodsListService_1 = require("../../../../Common/Services/StandardLists/AWBDescriptionOfGoodsListService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ChooseDescriptionOfGoodsComponent = /** @class */ (function (_super) {
    __extends(ChooseDescriptionOfGoodsComponent, _super);
    function ChooseDescriptionOfGoodsComponent() {
        var _this = _super.call(this) || this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.searchTextValue = null;
        _this.Count = 0;
        return _this;
    }
    ChooseDescriptionOfGoodsComponent.prototype.SetWindowArgs = function (entityPM) {
        this.EntityPM = entityPM;
        this.LoadData();
    };
    Object.defineProperty(ChooseDescriptionOfGoodsComponent.prototype, "SearchTextValue", {
        get: function () { return this.searchTextValue; },
        set: function (newValue) {
            if (this.searchTextValue != newValue) {
                this.searchTextValue = newValue;
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    ChooseDescriptionOfGoodsComponent.prototype.LoadData = function () {
        var _this = this;
        if (this.ItemsSource == null) {
            this.ItemsSource = new Array();
        }
        else {
            this.ItemsSource = [];
        }
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.Filter1Name = "AirlineCode";
        filters.Filter1Value = this.EntityPM.MainCarriageCarrierCode;
        filters.Filter1Operator = "Equals";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchTextValue)) {
            filters.Filter2Name = "SearchFields";
            filters.Filter2Value = this.SearchTextValue;
            filters.Filter2Operator = "Contains";
        }
        var myService = new AWBDescriptionOfGoodsListService_1.AWBDescriptionOfGoodsListService();
        myService.getByFilters(filters).subscribe(function (myResult) {
            if (myResult == null) {
                _this.ItemsSource = [];
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.ItemsSource = myResponse.Result;
                }
            }
            _this.Count = _this.ItemsSource.length;
        });
    };
    ChooseDescriptionOfGoodsComponent.prototype.Selecting = function (item) {
        if (item == null) {
            this.EntityPM.DescriptionOfGoodsId = null;
            this.EntityPM.DescriptionOfGoods = null;
            this.EntityPM.DescriptionOfGoodsService = null;
            this.EntityPM.IsTemperatureSensitive = false;
        }
        else {
            this.EntityPM.DescriptionOfGoodsId = item.Id;
            this.EntityPM.DescriptionOfGoods = item.ShortDescriptionOfGoods;
            this.EntityPM.DescriptionOfGoodsService = item.Service;
            this.EntityPM.IsTemperatureSensitive = item.IsTemperatureSensitive;
        }
        this.CloseButtonClicked();
    };
    ChooseDescriptionOfGoodsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ChooseDescriptionOfGoodsComponent = __decorate([
        core_1.Component({
            selector: 'ChooseDescriptionOfGoodsComponent',
            moduleId: module.id,
            templateUrl: './ChooseDescriptionOfGoodsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ChooseDescriptionOfGoodsComponent);
    return ChooseDescriptionOfGoodsComponent;
}(BaseComponent_1.BaseComponent));
exports.ChooseDescriptionOfGoodsComponent = ChooseDescriptionOfGoodsComponent;
//# sourceMappingURL=ChooseDescriptionOfGoodsComponent.js.map