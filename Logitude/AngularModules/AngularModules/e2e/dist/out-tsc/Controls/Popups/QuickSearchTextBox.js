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
var Tools_1 = require("../../Infrastructure/Tools");
var EntityListService_1 = require("../../Infrastructure/Services/EntityListService");
var ApiQueryFilters_1 = require("../../Infrastructure/DataContracts/ApiQueryFilters");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var PartnersDomainService_1 = require("../../Common/Services/PartnersDomainService");
var ShipmentDomainService_1 = require("../../Shipment/Services/ShipmentDomainService");
var ObjectsLocator_1 = require("../../Infrastructure/Locators/ObjectsLocator");
var idgeneratorpipe_1 = require("../pipes/idgeneratorpipe");
var QuickSearchTextBox = /** @class */ (function () {
    function QuickSearchTextBox(entityListService) {
        this.entityListService = entityListService;
        this.Area = null;
        this.Watermark = null;
        this.ObjectTableName = null;
        this.Filters = null;
        this.ItemHeight = 25;
        this.IsDeleteIconVisible = false;
        this.DropDownHeight = 75;
        this.DropDownWidth = 300;
        this.IsDisabled = false;
        this.AWBMessagesCCSTypeCode = null;
        this.HideBox = false;
        this.HideIcons = false;
        this.IsControlFocused = false;
        this.IsQuickSearch = true;
        this.IsQuickSearchOpened = false;
        this.IsQuickSearchLoading = false;
        this.IsQuickSearchNoResult = false;
        this.IsSearchIconVisible = false;
        this.ShowViewAll = false;
        this.IsIconsVisible = true;
        this.DataLoaded = new core_1.EventEmitter();
        this.DataLoadedCount = new core_1.EventEmitter();
        this.ViewAllClicked = new core_1.EventEmitter();
        this.OnTextChanged = new core_1.EventEmitter();
        this.displayText = null;
        this.isItemSelected = false;
        this.searchText = null;
        this.SetControlDisplay();
    }
    QuickSearchTextBox.prototype.ngOnInit = function () {
        var pipe = new idgeneratorpipe_1.IdGeneratorPipe();
        this.NewId = pipe.transform(this.ObjectTableName + '_Search');
        this.SetWatermark();
        this.SetAPIFilters();
    };
    QuickSearchTextBox.prototype.SetWatermark = function () {
        if (this.Watermark == null) {
            var myResult = "Search...";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ObjectTableName)) {
                var textCode = this.ObjectTableName + ".F.SearchFields";
                var translated = TextCodeTranslator_1.TextCodeTranslator.Translate(textCode);
                if (!Tools_1.AppTool.IsNullOrEmpty(translated)) {
                    myResult = translated;
                }
            }
            this.Watermark = myResult;
        }
    };
    QuickSearchTextBox.prototype.SetAPIFilters = function () {
        if (this.Filters == null) {
            this.Filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        this.Filters.PageIndex = 0;
        this.Filters.PageSize = 10;
        this.Filters.GetCount = true;
    };
    QuickSearchTextBox.prototype.OnFucos = function () {
        this.IsControlFocused = true;
        this.SetControlDisplay();
    };
    QuickSearchTextBox.prototype.OnLostFucos = function () {
        if (this.Area != "Ticket.Shipment" && this.Area != "FilingInbox" && this.Area != "FilingInboxHouses") {
            this.SearchText = null;
        }
        if (!this.IsItemSelected) {
            this.SearchText = null;
        }
        this.IsControlFocused = false;
        this.SetControlDisplay();
    };
    QuickSearchTextBox.prototype.SetControlDisplay = function () {
        var isSearchIconVisible = false;
        var isDeleteIconVisible = false;
        var isQuickSearchOpened = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            if (!this.IsControlFocused) {
                isSearchIconVisible = true;
            }
            isQuickSearchOpened = false;
        }
        else {
            isDeleteIconVisible = true;
            if (this.IsQuickSearch) {
                if (this.HideBox) {
                    isQuickSearchOpened = true;
                }
                else {
                    if (this.IsControlFocused) {
                        isQuickSearchOpened = true;
                    }
                    else {
                        isQuickSearchOpened = false;
                    }
                }
            }
        }
        this.IsSearchIconVisible = isSearchIconVisible;
        this.IsDeleteIconVisible = isDeleteIconVisible;
        this.IsQuickSearchOpened = isQuickSearchOpened;
    };
    QuickSearchTextBox.prototype.DeleteClicked = function () {
        this.SearchText = null;
    };
    Object.defineProperty(QuickSearchTextBox.prototype, "DisplayText", {
        get: function () { return this.displayText; },
        set: function (value) {
            if (this.displayText != value) {
                this.displayText = value;
                this.searchText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuickSearchTextBox.prototype, "IsItemSelected", {
        get: function () { return this.isItemSelected; },
        set: function (value) {
            if (this.isItemSelected != value) {
                this.isItemSelected = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuickSearchTextBox.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            var _this = this;
            if (this.searchText != newValue) {
                this.searchText = newValue;
                if (this.Area == "FilingInbox" || this.Area == "FilingInboxHouses") {
                    this.IsItemSelected = false;
                }
                this.OnTextChanged.emit(false);
                this.SetControlDisplay();
                if (this.timerToken) {
                    clearTimeout(this.timerToken);
                }
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.OnDataLoaded();
                }
                else {
                    this.timerToken = setTimeout(function () { return _this.LoadData(); }, 400);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    QuickSearchTextBox.prototype.LoadData = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Area)) {
            switch (this.Area) {
                case "CRM.Customers": {
                    this.IsQuickSearchLoading = true;
                    this.IsQuickSearchNoResult = false;
                    if (this.myPartnersDomainService == null) {
                        this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
                    }
                    this.myPartnersDomainService.GetCustomersQuickSearch(this.searchText).subscribe(function (myResponse) {
                        _this.OnDataLoaded(myResponse.Result);
                    });
                    break;
                }
                case "TenantManagement.Airline": {
                    this.IsQuickSearchLoading = true;
                    this.IsQuickSearchNoResult = false;
                    this.Filters.PageIndex = 0;
                    this.Filters.PageSize = 8;
                    this.Filters.addAdditionalFilter("IsAllowedInAirlinesRestriction", false, null, null, "Equals", false, false, false, "boolean");
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                        this.Filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", true, true, false, "string");
                    }
                    if (this.myPartnersDomainService == null) {
                        this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
                    }
                    this.myPartnersDomainService.GetAirlinesByFiltersAndTenant(this.Filters, 0).subscribe(function (myResponse) {
                        _this.OnDataLoaded(myResponse.Result);
                    });
                    break;
                }
                case "FilingInbox": {
                    this.Filters = new ApiQueryFilters_1.ApiQueryFilters();
                    this.Filters.PageIndex = 0;
                    this.Filters.PageSize = 10;
                    this.Filters.GetCount = true;
                    this.IsQuickSearchLoading = true;
                    this.IsQuickSearchNoResult = false;
                    var item = this.Filters.AdditionalFilters.filter(function (f) { return f.FieldName == "SearchFields"; })[0];
                    if (item) {
                        var indexOfItem = this.Filters.AdditionalFilters.indexOf(item);
                        this.Filters.AdditionalFilters.splice(indexOfItem, 1);
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                        this.Filters.Filter10Name = "SearchFields";
                        this.Filters.Filter10Value = this.SearchText;
                        this.Filters.Filter10Operator = "Contains";
                    }
                    if (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Master") {
                        if (this.myShipmentDomainService == null) {
                            this.myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
                        }
                        if (this.ObjectTableName == "Shipment" && (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LogoCode == "L.O.B")) {
                        }
                        else {
                            if (this.ObjectTableName == "Master") {
                                this.ObjectTableName = "Shipment";
                                this.Filters.addAdditionalFilter("ShipmentLevelCode", "D,C", null, null, "InList", true, true, false, "string");
                            }
                            else {
                                this.Filters.addAdditionalFilter("ShipmentLevelCode", "D,H", null, null, "InList", true, true, false, "string");
                            }
                        }
                        //this.myShipmentDomainService.GetShipmentFullTextSearch(this.Filters).subscribe((myResponse: ServiceResponse) => {
                        //    if (!myResponse.HasError) {
                        //        this.OnDataLoaded(myResponse.Result);
                        //    }
                        //});
                    }
                    var loadPromise = this.entityListService.getByFilters(this.ObjectTableName, this.Filters);
                    loadPromise.then(function (res) {
                        res.subscribe(function (resp) {
                            _this.OnDataLoaded(resp.Result);
                        });
                    });
                    break;
                }
                case "Ticket.Shipment": {
                    this.Filters = new ApiQueryFilters_1.ApiQueryFilters();
                    this.Filters.PageIndex = 0;
                    this.Filters.PageSize = 10;
                    this.Filters.GetCount = true;
                    this.IsQuickSearchLoading = true;
                    this.IsQuickSearchNoResult = false;
                    var item = this.Filters.AdditionalFilters.filter(function (f) { return f.FieldName == "SearchFields"; })[0];
                    if (item) {
                        var indexOfItem = this.Filters.AdditionalFilters.indexOf(item);
                        this.Filters.AdditionalFilters.splice(indexOfItem, 1);
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                        this.Filters.Filter10Name = "SearchFields";
                        this.Filters.Filter10Value = this.SearchText;
                        this.Filters.Filter10Operator = "Contains";
                    }
                    if (this.ObjectTableName == "Shipment") {
                        if (this.myShipmentDomainService == null) {
                            this.myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
                        }
                        this.myShipmentDomainService.GetShipmentFullTextSearch(this.Filters).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                _this.OnDataLoaded(myResponse.Result);
                            }
                        });
                    }
                    else {
                        var loadPromise = this.entityListService.getByFilters(this.ObjectTableName, this.Filters);
                        loadPromise.then(function (res) {
                            res.subscribe(function (resp) {
                                _this.OnDataLoaded(resp.Result);
                            });
                        });
                    }
                    break;
                }
                default:
                    {
                        this.LoadSeachData();
                        break;
                    }
            }
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.ObjectTableName)) {
            this.LoadSeachData();
        }
    };
    QuickSearchTextBox.prototype.LoadSeachData = function () {
        var _this = this;
        this.IsQuickSearchLoading = true;
        this.IsQuickSearchNoResult = false;
        var item = this.Filters.AdditionalFilters.filter(function (f) { return f.FieldName == "SearchFields"; })[0];
        if (item) {
            var indexOfItem = this.Filters.AdditionalFilters.indexOf(item);
            this.Filters.AdditionalFilters.splice(indexOfItem, 1);
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            //this.Filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
            this.Filters.Filter10Name = "SearchFields";
            this.Filters.Filter10Value = this.SearchText;
            this.Filters.Filter10Operator = "Contains";
        }
        var loadPromise = this.entityListService.getByFilters(this.ObjectTableName, this.Filters);
        loadPromise.then(function (res) {
            res.subscribe(function (resp) {
                _this.OnDataLoaded(resp.Result);
            });
        });
    };
    QuickSearchTextBox.prototype.OnDataLoaded = function (items) {
        if (items === void 0) { items = []; }
        var itemsCount = items.length;
        this.IsQuickSearchLoading = false;
        this.IsQuickSearchNoResult = itemsCount == 0 ? true : false;
        this.SetDropDownheight(itemsCount);
        if (itemsCount > 10) {
            var myItems = [];
            items.forEach(function (item) {
                if (myItems.length < 10) {
                    myItems.push(item);
                }
            });
            this.DataLoaded.emit(myItems);
        }
        else {
            this.DataLoaded.emit(items);
        }
        this.DataLoadedCount.emit(itemsCount);
    };
    QuickSearchTextBox.prototype.SetDropDownheight = function (itemsCount) {
        var myHeight = 75;
        if (itemsCount > 0) {
            if (itemsCount > 10) {
                itemsCount = 10;
            }
            var itemsHeight = ((itemsCount * this.ItemHeight) + 2);
            if (itemsHeight > myHeight) {
                myHeight = itemsHeight;
            }
            if (itemsCount > 10) {
                myHeight = myHeight + 5;
            }
        }
        if (this.ShowViewAll) {
            myHeight += 20;
        }
        this.DropDownHeight = myHeight;
    };
    QuickSearchTextBox.prototype.OnViewAllClicked = function () {
        this.ViewAllClicked.emit(true);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], QuickSearchTextBox.prototype, "DataLoaded", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], QuickSearchTextBox.prototype, "DataLoadedCount", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], QuickSearchTextBox.prototype, "ViewAllClicked", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], QuickSearchTextBox.prototype, "OnTextChanged", void 0);
    QuickSearchTextBox = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './QuickSearchTextBox.html',
            selector: "QuickSearchTextBox",
            inputs: ['Watermark', 'ObjectTableName', 'Filters', 'DropDownWidth', 'ItemHeight', 'Area', 'IsDisabled', 'AWBMessagesCCSTypeCode', 'ShowViewAll', 'DisplayText', 'IsIconsVisible', 'IsItemSelected'],
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService])
    ], QuickSearchTextBox);
    return QuickSearchTextBox;
}());
exports.QuickSearchTextBox = QuickSearchTextBox;
//# sourceMappingURL=QuickSearchTextBox.js.map