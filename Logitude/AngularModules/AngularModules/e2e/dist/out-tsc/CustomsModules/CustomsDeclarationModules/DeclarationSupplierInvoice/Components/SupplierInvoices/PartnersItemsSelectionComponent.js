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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var CustomsSettingListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsSettingListService");
var CustomsVendorListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsVendorListService");
var PartnersItemsSelectionComponent = /** @class */ (function (_super) {
    __extends(PartnersItemsSelectionComponent, _super);
    function PartnersItemsSelectionComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        _this.searchText = "";
        _this.ValidationErrorsList = [];
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        //Services
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService;
        _this.customsVendorListService = new CustomsVendorListService_1.CustomsVendorListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsConnectedToUniFreight = false;
        _this.SelectedRow = null;
        return _this;
    }
    PartnersItemsSelectionComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.invoicePM = args.invoicePM;
            this.customerCode = args.customerCode;
            this.searchText = args.searchText;
            // Load screen data
            this.customsSettingListService.getAll().subscribe(function (response) {
                var list = response.Result;
                console.log("[response/customsSettingListService.getAll]", list);
                if (!Tools_1.AppTool.IsNullOrEmpty(list)) {
                    var customsSettingList = list[0];
                    var isConnectedToUniFreight = false;
                    isConnectedToUniFreight = customsSettingList.IsConnectedToUniFreight;
                    _this.IsConnectedToUniFreight = isConnectedToUniFreight;
                    if (isConnectedToUniFreight) {
                        _this.LoadGTBITEMS();
                    }
                    else {
                        _this.LoadCustomsPartnersItems();
                    }
                }
            });
        }
    };
    PartnersItemsSelectionComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    PartnersItemsSelectionComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit(this.SelectedRow);
    };
    PartnersItemsSelectionComponent.prototype.Search = function (text) {
        var _this = this;
        this.searchText = text;
        if (this.IsConnectedToUniFreight) {
            var tkn = setTimeout(function () {
                if (!_this.searchText || _this.searchText.length != 1) {
                    _this.LoadGTBITEMS();
                }
            }, 200);
        }
        else {
            var data = this.originalData;
            if (!Tools_1.AppTool.IsNullOrEmpty(text) && !Tools_1.AppTool.IsNullOrEmpty(data)) {
                var tkn = setTimeout(function () {
                    var filteredData = data.filter(function (el) {
                        var searchField = el["SearchFields"] == undefined ? "" : el["SearchFields"];
                        if (searchField.toLowerCase().includes(text.trim().toLowerCase()))
                            return true;
                        else
                            return false;
                    });
                    _this.ItemsSource.Clear();
                    _this.ItemsSource.InsertCollection(filteredData);
                }, 200);
                //} else {
                //    var tkn = setTimeout(() => {
                //        this.ItemsSource.Clear();
                //        this.ItemsSource.InsertCollection(data);
                //    }, 202);
                //
            }
        }
    };
    PartnersItemsSelectionComponent.prototype.LoadCustomsPartnersItems = function () {
        var _this = this;
        this.declarationWebService.GetCustomsPartnersItemsForSelection(this.invoicePM.VendorId, null)
            .subscribe(function (response) {
            var res = response.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(res)) {
                _this.ItemsSource.Clear();
                _this.ItemsSource.InsertCollection(res);
                _this.originalData = res;
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.searchText))
                    _this.Search(_this.searchText);
            }
        });
    };
    PartnersItemsSelectionComponent.prototype.LoadGTBITEMS = function () {
        //if (AppTool.IsNullOrEmpty(this.invoicePM.VendorId)) {
        //    this.vendorNumber = "NULL";
        //}
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.invoicePM.VendorId) && Tools_1.AppTool.IsNullOrEmpty(this.vendorNumber)) {
            this.customsVendorListService.getSingle(this.invoicePM.VendorId)
                .subscribe(function (customsVendorList) {
                if (customsVendorList) {
                    _this.vendorNumber = customsVendorList.Result.VendorNumber;
                    _this.GetGITITEMPartnersItemList();
                }
            });
        }
        else {
            this.GetGITITEMPartnersItemList();
        }
    };
    PartnersItemsSelectionComponent.prototype.GetGITITEMPartnersItemList = function () {
        var _this = this;
        this.declarationWebService.GetGITITEMPartnersItemList(this.vendorNumber, this.customerCode, this.searchText, 30, true)
            .subscribe(function (response) {
            var res = response.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(res)) {
                _this.ItemsSource.Clear();
                _this.ItemsSource.InsertCollection(res);
            }
        });
    };
    PartnersItemsSelectionComponent.prototype.OnRowSelected = function (item) {
        this.SelectedRow = item;
    };
    PartnersItemsSelectionComponent.prototype.OnRowDoubleClick = function (item) {
        this.SelectedRow = item;
        this.CurrentSession.CloseCurrentWindowEmit(this.SelectedRow);
    };
    PartnersItemsSelectionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PartnersItemsSelectionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], PartnersItemsSelectionComponent);
    return PartnersItemsSelectionComponent;
}(BaseComponent_1.BaseComponent));
exports.PartnersItemsSelectionComponent = PartnersItemsSelectionComponent;
//# sourceMappingURL=PartnersItemsSelectionComponent.js.map