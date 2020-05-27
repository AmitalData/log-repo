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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ReportFliter_1 = require("../../Components/Filters/ReportFliter");
var QueryFilterItem_1 = require("../../Components/Filters/QueryFilterItem");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var CodeNameClass_1 = require("../../../Infrastructure/DataContracts/CodeNameClass");
var Tools_1 = require("../../../Infrastructure/Tools");
var ProductTypeListService_1 = require("../../../Common/Services/StandardLists/ProductTypeListService");
var BusinessUnitListService_1 = require("../../../Common/Services/StandardLists/BusinessUnitListService");
var UserListService_1 = require("../../../Common/Services/StandardLists/UserListService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var ReportService_1 = require("../../../Common/Services/ExtendedLists/ReportService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var CustomerPotentialActualFilterComponent = /** @class */ (function (_super) {
    __extends(CustomerPotentialActualFilterComponent, _super);
    function CustomerPotentialActualFilterComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.AD_IsVisible = false;
        _this.AR_IsVisible = false;
        _this.AE_IsVisible = false;
        _this.AI_IsVisible = false;
        _this.CI_IsVisible = false;
        _this.DL_IsVisible = false;
        _this.ID_IsVisible = false;
        _this.IR_IsVisible = false;
        _this.IE_IsVisible = false;
        _this.II_IsVisible = false;
        _this.IN_IsVisible = false;
        _this.OD_IsVisible = false;
        _this.OR_IsVisible = false;
        _this.OE_IsVisible = false;
        _this.OI_IsVisible = false;
        _this.SelectedProdustTypeFilter = "All";
        _this.UsersFilterList = [];
        _this.BusinessUnitFilterList = [];
        _this.UpdateColumns();
        _this.BuildFilters();
        return _this;
    }
    CustomerPotentialActualFilterComponent.prototype.UpdateColumns = function () {
        var _this = this;
        var ad_IsVisible = false;
        var ar_IsVisible = false;
        var ae_IsVisible = false;
        var ai_IsVisible = false;
        var ci_IsVisible = false;
        var dl_IsVisible = false;
        var id_IsVisible = false;
        var ir_IsVisible = false;
        var ie_IsVisible = false;
        var ii_IsVisible = false;
        var in_IsVisible = false;
        var od_IsVisible = false;
        var or_IsVisible = false;
        var oe_IsVisible = false;
        var oi_IsVisible = false;
        var proeductTypeListService = new ProductTypeListService_1.ProductTypeListService();
        proeductTypeListService.getAllFromCache().subscribe(function (response) {
            var list = response.Result;
            list.filter(function (d) { return !d.InActive; }).forEach(function (item) {
                if (_this.SelectedProdustTypeFilter == "All") {
                    if (item.Code == "AD") {
                        ad_IsVisible = true;
                    }
                    else if (item.Code == "AR") {
                        ar_IsVisible = true;
                    }
                    else if (item.Code == "AE") {
                        ae_IsVisible = true;
                    }
                    else if (item.Code == "AI") {
                        ai_IsVisible = true;
                    }
                    else if (item.Code == "CI") {
                        ci_IsVisible = true;
                    }
                    else if (item.Code == "DL") {
                        dl_IsVisible = true;
                    }
                    else if (item.Code == "ID") {
                        id_IsVisible = true;
                    }
                    else if (item.Code == "IR") {
                        ir_IsVisible = true;
                    }
                    else if (item.Code == "IE") {
                        ie_IsVisible = true;
                    }
                    else if (item.Code == "II") {
                        ii_IsVisible = true;
                    }
                    else if (item.Code == "IN") {
                        in_IsVisible = true;
                    }
                    else if (item.Code == "OD") {
                        od_IsVisible = true;
                    }
                    else if (item.Code == "OR") {
                        or_IsVisible = true;
                    }
                    else if (item.Code == "OE") {
                        oe_IsVisible = true;
                    }
                    else if (item.Code == "OI") {
                        oi_IsVisible = true;
                    }
                }
                else {
                    if (_this.mySelectedProductsList.indexOf(item.Code) > -1) {
                        if (item.Code == "AD") {
                            ad_IsVisible = true;
                        }
                        else if (item.Code == "AR") {
                            ar_IsVisible = true;
                        }
                        else if (item.Code == "AE") {
                            ae_IsVisible = true;
                        }
                        else if (item.Code == "AI") {
                            ai_IsVisible = true;
                        }
                        else if (item.Code == "CI") {
                            ci_IsVisible = true;
                        }
                        else if (item.Code == "DL") {
                            dl_IsVisible = true;
                        }
                        else if (item.Code == "ID") {
                            id_IsVisible = true;
                        }
                        else if (item.Code == "IR") {
                            ir_IsVisible = true;
                        }
                        else if (item.Code == "IE") {
                            ie_IsVisible = true;
                        }
                        else if (item.Code == "II") {
                            ii_IsVisible = true;
                        }
                        else if (item.Code == "IN") {
                            in_IsVisible = true;
                        }
                        else if (item.Code == "OD") {
                            od_IsVisible = true;
                        }
                        else if (item.Code == "OR") {
                            or_IsVisible = true;
                        }
                        else if (item.Code == "OE") {
                            oe_IsVisible = true;
                        }
                        else if (item.Code == "OI") {
                            oi_IsVisible = true;
                        }
                    }
                }
            });
        });
        this.AD_IsVisible = ad_IsVisible;
        this.AR_IsVisible = ar_IsVisible;
        this.AE_IsVisible = ae_IsVisible;
        this.AI_IsVisible = ai_IsVisible;
        this.CI_IsVisible = ci_IsVisible;
        this.DL_IsVisible = dl_IsVisible;
        this.ID_IsVisible = id_IsVisible;
        this.IR_IsVisible = ir_IsVisible;
        this.IE_IsVisible = ie_IsVisible;
        this.II_IsVisible = ii_IsVisible;
        this.IN_IsVisible = in_IsVisible;
        this.OD_IsVisible = od_IsVisible;
        this.OR_IsVisible = or_IsVisible;
        this.OE_IsVisible = oe_IsVisible;
        this.OI_IsVisible = oi_IsVisible;
    };
    CustomerPotentialActualFilterComponent.prototype.BuildFilters = function () {
        this.BuildTimeRangeFilters();
        this.BuildProductTypesFilters();
        this.BuildBusinessUnitFilter();
        this.BuildViewByFilters();
        this.BuildProductsFilter();
    };
    CustomerPotentialActualFilterComponent.prototype.GetMonthName = function (monthNumber) {
        var monthName = null;
        switch (monthNumber) {
            case 1: {
                monthName = "January";
                break;
            }
            case 2: {
                monthName = "February";
                break;
            }
            case 3: {
                monthName = "March";
                break;
            }
            case 4: {
                monthName = "April";
                break;
            }
            case 5: {
                monthName = "May";
                break;
            }
            case 6: {
                monthName = "June";
                break;
            }
            case 7: {
                monthName = "July";
                break;
            }
            case 8: {
                monthName = "August";
                break;
            }
            case 9: {
                monthName = "September";
                break;
            }
            case 10: {
                monthName = "October";
                break;
            }
            case 11: {
                monthName = "November";
                break;
            }
            case 12: {
                monthName = "December";
                break;
            }
        }
        return monthName;
    };
    Object.defineProperty(CustomerPotentialActualFilterComponent.prototype, "CountryId", {
        get: function () { return this.countryId; },
        set: function (value) {
            if (this.countryId != value) {
                this.countryId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerPotentialActualFilterComponent.prototype.BuildTimeRangeFilters = function () {
        this.TimeRangeComboList = [];
        var lastMonth = new Date();
        this.TimeRangeComboList.push(new CodeNameClass_1.CodeNameClass("L1M", this.GetMonthName(lastMonth.getMonth()) + " " + lastMonth.getFullYear()));
        this.TimeRangeComboList.push(new CodeNameClass_1.CodeNameClass("L3M", "Average of last 3 months"));
        this.TimeRangeComboList.push(new CodeNameClass_1.CodeNameClass("L12M", "Average of last 12 months"));
        this.selectedTimeRangeFilter = this.TimeRangeComboList.filter(function (d) { return d.Code == "L1M"; })[0];
    };
    Object.defineProperty(CustomerPotentialActualFilterComponent.prototype, "SelectedTimeRangeFilter", {
        get: function () { return this.selectedTimeRangeFilter; },
        set: function (value) {
            if (this.selectedTimeRangeFilter != value) {
                this.selectedTimeRangeFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerPotentialActualFilterComponent.prototype.BuildProductTypesFilters = function () {
        var _this = this;
        this.ProductTypeComboList = [];
        var proeductTypeListService = new ProductTypeListService_1.ProductTypeListService();
        proeductTypeListService.getAllFromCache().subscribe(function (response) {
            var list = response.Result;
            list.filter(function (d) { return !d.InActive; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; }).forEach(function (item) {
                _this.ProductTypeComboList.push(new ProductTypeItemClass(item));
            });
        });
    };
    CustomerPotentialActualFilterComponent.prototype.EditedItemSource = function (newSource) {
        this.ProductTypeComboList = newSource;
    };
    CustomerPotentialActualFilterComponent.prototype.BuildBusinessUnitFilter = function () {
        var _this = this;
        var myBusinessUnitListService = new BusinessUnitListService_1.BusinessUnitListService();
        myBusinessUnitListService.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                _this.BusinessUnitFilterList = [];
                _this.BusinessUnitFilterList.push(new CodeNameClass_1.CodeNameClass("M", "My Records"));
                if (list) {
                    list.filter(function (d) { return d.Id != SessionLocator_1.SessionLocator.Tenant.toString(); }).forEach(function (item) {
                        _this.BusinessUnitFilterList.push(new CodeNameClass_1.CodeNameClass(item.Id, item.Name));
                    });
                }
                _this.BusinessUnitFilterList.push(new CodeNameClass_1.CodeNameClass("A", "All Records"));
                _this.selectedBusinessUnitFilter = _this.BusinessUnitFilterList.filter(function (d) { return d.Code == "M"; })[0];
                _this.GetSelectedBusinessUnitId();
                _this.BuildUsersFilters();
            }
        });
    };
    CustomerPotentialActualFilterComponent.prototype.GetSelectedBusinessUnitId = function () {
        var myResult = null;
        if (this.SelectedBusinessUnitFilter) {
            switch (this.SelectedBusinessUnitFilter.Code) {
                case "M": {
                    myResult = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId;
                    break;
                }
                case "A": {
                    myResult = null;
                    break;
                }
                default: {
                    myResult = this.SelectedBusinessUnitFilter.Code;
                    break;
                }
            }
        }
        this.BusinessUnitId = myResult;
    };
    CustomerPotentialActualFilterComponent.prototype.BuildUsersFilters = function () {
        var _this = this;
        this.UsersFilterList = [];
        if (this.SelectedBusinessUnitFilter == null) {
            this.selectedUserFilter = null;
            this.GetSelectedOwnerId();
        }
        else {
            switch (this.SelectedBusinessUnitFilter.Code) {
                case "M":
                    {
                        var item = new CodeNameClass_1.CodeNameClass(SessionLocator_1.SessionLocator.LoggedUserId, SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName);
                        this.UsersFilterList.push(item);
                        this.selectedUserFilter = item;
                        this.GetSelectedOwnerId();
                        break;
                    }
                case "A":
                    {
                        var item = new CodeNameClass_1.CodeNameClass("A", "All Owners");
                        this.UsersFilterList.push(item);
                        this.OwnerId = null;
                        this.listOfValuesUserId = null;
                        this.selectedUserFilter = null;
                        this.GetSelectedOwnerId();
                        break;
                    }
                default:
                    {
                        var item = new CodeNameClass_1.CodeNameClass("A", "All " + this.SelectedBusinessUnitFilter.Name + " Owners");
                        this.UsersFilterList.push(item);
                        var filters = new ApiQueryFilters_1.ApiQueryFilters();
                        filters.PageIndex = 0;
                        filters.PageSize = 100;
                        filters.Filter1Name = "BusinessUnitId";
                        filters.Filter1Value = this.BusinessUnitId;
                        filters.Filter1Operator = "Equals";
                        var myUserListService = new UserListService_1.UserListService();
                        myUserListService.getAllFromCache(filters).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var loadedUsers = myResponse.Result;
                                if (loadedUsers != null) {
                                    loadedUsers.forEach(function (list) {
                                        _this.UsersFilterList.push(new CodeNameClass_1.CodeNameClass(list.Id, list.EnglishName));
                                    });
                                }
                            }
                            _this.OwnerId = null;
                            _this.listOfValuesUserId = null;
                            _this.selectedUserFilter = item;
                            _this.listOfValuesUserId = _this.OwnerId;
                            if (!Tools_1.AppTool.IsNullOrEmpty(_this.OwnerId)) {
                                _this.selectedUserFilter = _this.UsersFilterList.filter(function (d) { return d.Code == _this.OwnerId; })[0];
                            }
                            if (_this.selectedUserFilter == null) {
                                _this.selectedUserFilter = _this.UsersFilterList[0];
                            }
                        });
                        break;
                    }
            }
        }
    };
    CustomerPotentialActualFilterComponent.prototype.GetSelectedOwnerId = function () {
        var myResult = null;
        this.listOfValuesUserId = null;
        if (this.SelectedUserFilter) {
            switch (this.SelectedUserFilter.Code) {
                case "A": {
                    this.listOfValuesUserId = null;
                    break;
                }
                default: {
                    myResult = this.SelectedUserFilter.Code;
                    this.listOfValuesUserId = myResult;
                    break;
                }
            }
        }
        this.OwnerId = myResult;
    };
    Object.defineProperty(CustomerPotentialActualFilterComponent.prototype, "SelectedBusinessUnitFilter", {
        get: function () { return this.selectedBusinessUnitFilter; },
        set: function (value) {
            if (this.selectedBusinessUnitFilter != value) {
                this.selectedBusinessUnitFilter = value;
                this.GetSelectedBusinessUnitId();
                this.BuildUsersFilters();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerPotentialActualFilterComponent.prototype, "SelectedUserFilter", {
        get: function () { return this.selectedUserFilter; },
        set: function (value) {
            if (this.selectedUserFilter != value) {
                this.selectedUserFilter = value;
                if (value == null) {
                    this.OwnerId = null;
                }
                else if (value.Code == "A") {
                    this.OwnerId = null;
                }
                else {
                    this.OwnerId = value.Code;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerPotentialActualFilterComponent.prototype, "ListOfValuesUserId", {
        get: function () { return this.listOfValuesUserId; },
        set: function (value) {
            if (this.listOfValuesUserId != value) {
                this.OwnerId = value;
                this.listOfValuesUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerPotentialActualFilterComponent.prototype.BuildViewByFilters = function () {
        this.ViewByComboList = [];
        this.ViewByComboList.push(new CodeNameClass_1.CodeNameClass("NSH", "No. Of Shipments"));
        this.ViewByComboList.push(new CodeNameClass_1.CodeNameClass("TEU", "TEU"));
        this.ViewByComboList.push(new CodeNameClass_1.CodeNameClass("CHW", "Chargeable Weight"));
        this.ViewByComboList.push(new CodeNameClass_1.CodeNameClass("REV", "Revenue"));
        this.selectedViewByFilter = this.ViewByComboList.filter(function (d) { return d.Code == "NSH"; })[0];
    };
    Object.defineProperty(CustomerPotentialActualFilterComponent.prototype, "SelectedViewByFilter", {
        get: function () { return this.selectedViewByFilter; },
        set: function (value) {
            if (this.selectedViewByFilter != value) {
                this.selectedViewByFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerPotentialActualFilterComponent.prototype.BuildProductsFilter = function () {
        this.ProductFilterList = [];
        this.ProductFilterList.push(new CodeNameClass_1.CodeNameClass("ALL", "All"));
        this.ProductFilterList.push(new CodeNameClass_1.CodeNameClass("POT", "Potential Only"));
        this.ProductFilterList.push(new CodeNameClass_1.CodeNameClass("ACT", "Actual Only"));
        this.ProductFilterList.push(new CodeNameClass_1.CodeNameClass("NON", "None"));
        this.selectedProductFilter = this.ProductFilterList.filter(function (d) { return d.Code == "ALL"; })[0];
    };
    Object.defineProperty(CustomerPotentialActualFilterComponent.prototype, "SelectedProductFilter", {
        get: function () { return this.selectedProductFilter; },
        set: function (value) {
            if (this.selectedProductFilter != value) {
                this.selectedProductFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerPotentialActualFilterComponent.prototype.RunReport = function () {
        this.ValidationErrorsList = [];
        if (this.SelectedProdustTypeFilter != "All") {
            if (this.ProductTypeComboList.filter(function (d) { return d.Checked; }).length == 0) {
                this.ValidationErrorsList.push("Please select product type");
            }
        }
        if (!this.SelectedTimeRangeFilter) {
            this.ValidationErrorsList.push("Time Range field is required");
        }
        if (!this.SelectedViewByFilter) {
            this.ValidationErrorsList.push("View by field is required");
        }
        if (!this.SelectedProductFilter) {
            this.ValidationErrorsList.push("Product field is required");
        }
        if (!this.SelectedBusinessUnitFilter) {
            this.ValidationErrorsList.push("Business unit field is required");
        }
        if (this.SelectedBusinessUnitFilter) {
            if (this.SelectedBusinessUnitFilter.Code != "A") {
                if (!this.SelectedUserFilter) {
                    this.ValidationErrorsList.push("User field is required");
                }
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array();
            var myProductTypes = "";
            if (this.SelectedProdustTypeFilter == "All") {
                myProductTypes = "All";
            }
            else {
                this.ProductTypeComboList.forEach(function (i) {
                    if (i.Checked) {
                        myProductTypes += i.Code + ",";
                    }
                });
            }
            var myProductsText = myProductTypes.trim();
            this.mySelectedProductsList = myProductsText.split(',');
            this.UpdateColumns();
            var queryFilterItem1 = new QueryFilterItem_1.QueryFilterItem();
            queryFilterItem1.DisplayInList = false;
            queryFilterItem1.FieldName = "DataTypeCode";
            queryFilterItem1.FieldValue = this.SelectedViewByFilter.Code;
            queryFilterItem1.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem1);
            var queryFilterItem2 = new QueryFilterItem_1.QueryFilterItem();
            queryFilterItem2.DisplayInList = false;
            queryFilterItem2.FieldName = "TimeRange";
            queryFilterItem2.FieldValue = this.SelectedTimeRangeFilter.Code;
            queryFilterItem2.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem2);
            var queryFilterItem3 = new QueryFilterItem_1.QueryFilterItem();
            queryFilterItem3.DisplayInList = false;
            queryFilterItem3.FieldName = "ProductsTypes";
            queryFilterItem3.FieldValue = myProductTypes;
            queryFilterItem3.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem3);
            var queryFilterItem4 = new QueryFilterItem_1.QueryFilterItem();
            queryFilterItem4.DisplayInList = false;
            queryFilterItem4.FieldName = "BusinessUnitId";
            queryFilterItem4.FieldValue = this.BusinessUnitId;
            queryFilterItem4.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem4);
            var queryFilterItem5 = new QueryFilterItem_1.QueryFilterItem();
            queryFilterItem5.DisplayInList = false;
            queryFilterItem5.FieldName = "OwnerId";
            queryFilterItem5.FieldValue = this.OwnerId;
            queryFilterItem5.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem5);
            var queryFilterItem6 = new QueryFilterItem_1.QueryFilterItem();
            queryFilterItem6.DisplayInList = false;
            queryFilterItem6.FieldName = "CountryId";
            queryFilterItem6.FieldValue = this.CountryId;
            queryFilterItem6.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem6);
            var queryFilterItem7 = new QueryFilterItem_1.QueryFilterItem();
            queryFilterItem7.DisplayInList = false;
            queryFilterItem7.FieldName = "ProductCode";
            queryFilterItem7.FieldValue = this.SelectedProductFilter.Code;
            queryFilterItem7.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem7);
            this.reportFliter = new ReportFliter_1.ReportFliter();
            this.reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = "CustomerPotentialActualFilterControl";
            this.reportFliter.ReportCode = "CUPA";
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            this.GenerateReport(this.reportFliter);
        }
    };
    CustomerPotentialActualFilterComponent.prototype.GenerateReport = function (filter) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var reportService = new ReportService_1.ReportService();
        reportService.GenerateReportForCustomerPotentialActual(filter).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show(myResponse.ErrorsArray[0]);
            }
            else {
                var myResult = myResponse.Result;
                if (myResult != null) {
                    _this.BuildItemsSource(myResult);
                }
            }
        });
    };
    CustomerPotentialActualFilterComponent.prototype.BuildItemsSource = function (myResult) {
        var _this = this;
        this.ItemsSource = [];
        myResult.Customers.forEach(function (i) {
            _this.ItemsSource.push(new CustomerSummary(i, _this));
        });
    };
    CustomerPotentialActualFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CustomerPotentialActualFilterComponent',
            templateUrl: './CustomerPotentialActualFilterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomerPotentialActualFilterComponent);
    return CustomerPotentialActualFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerPotentialActualFilterComponent = CustomerPotentialActualFilterComponent;
var ProductTypeItemClass = /** @class */ (function () {
    function ProductTypeItemClass(entityList) {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.entityList = entityList;
    }
    Object.defineProperty(ProductTypeItemClass.prototype, "Code", {
        get: function () { return this.entityList.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "Name", {
        get: function () { return this.entityList.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "Checked", {
        get: function () { return this.checked; },
        set: function (value) { this.checked = value; },
        enumerable: true,
        configurable: true
    });
    return ProductTypeItemClass;
}());
exports.ProductTypeItemClass = ProductTypeItemClass;
var CustomerSummary = /** @class */ (function () {
    function CustomerSummary(myEntity, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.entity = myEntity;
    }
    Object.defineProperty(CustomerSummary.prototype, "CustomerId", {
        get: function () { return this.entity.CustomerId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "CustomerName", {
        get: function () { return this.entity.CustomerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "PrimaryContactName", {
        get: function () { return this.entity.PrimaryContactName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "PrimaryContactEmail", {
        get: function () { return this.entity.PrimaryContactEmail; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "Salesman", {
        get: function () { return this.entity.Salesman; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "AD_POT", {
        get: function () { return this.entity.AD_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "AD_ACT", {
        get: function () { return this.entity.AD_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "AR_POT", {
        get: function () { return this.entity.AR_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "AR_ACT", {
        get: function () { return this.entity.AR_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "AE_POT", {
        get: function () { return this.entity.AE_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "AE_ACT", {
        get: function () { return this.entity.AE_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "AI_POT", {
        get: function () { return this.entity.AI_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "AI_ACT", {
        get: function () { return this.entity.AI_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "CI_POT", {
        get: function () { return this.entity.CI_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "CI_ACT", {
        get: function () { return this.entity.CI_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "DL_POT", {
        get: function () { return this.entity.DL_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "DL_ACT", {
        get: function () { return this.entity.DL_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "ID_POT", {
        get: function () { return this.entity.ID_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "ID_ACT", {
        get: function () { return this.entity.ID_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "IR_POT", {
        get: function () { return this.entity.IR_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "IR_ACT", {
        get: function () { return this.entity.IR_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "IE_POT", {
        get: function () { return this.entity.IE_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "IE_ACT", {
        get: function () { return this.entity.IE_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "II_POT", {
        get: function () { return this.entity.II_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "II_ACT", {
        get: function () { return this.entity.II_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "IN_POT", {
        get: function () { return this.entity.IN_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "IN_ACT", {
        get: function () { return this.entity.IN_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "OD_POT", {
        get: function () { return this.entity.OD_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "OD_ACT", {
        get: function () { return this.entity.OD_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "OR_POT", {
        get: function () { return this.entity.OR_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "OR_ACT", {
        get: function () { return this.entity.OR_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "OE_POT", {
        get: function () { return this.entity.OE_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "OE_ACT", {
        get: function () { return this.entity.OE_ACT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "OI_POT", {
        get: function () { return this.entity.OI_POT; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSummary.prototype, "OI_ACT", {
        get: function () { return this.entity.OI_ACT; },
        enumerable: true,
        configurable: true
    });
    CustomerSummary.prototype.ViewCustomerClicked = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this.CustomerId, ObjectTableName: "Customer", BackButtonLabel: "Reports" });
                var isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    if (isEditComponentSaved) {
                        _this.fatherComponent.RunReport();
                    }
                });
                cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
            });
        }
    };
    return CustomerSummary;
}());
exports.CustomerSummary = CustomerSummary;
//# sourceMappingURL=CustomerPotentialActualFilterComponent.js.map