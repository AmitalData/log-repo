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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ReportFliter_1 = require("../../Components/Filters/ReportFliter");
var QueryFilterItem_1 = require("../../Components/Filters/QueryFilterItem");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ReportsDomainService_1 = require("../../Services/ReportsDomainService");
var UserListService_1 = require("../../../Common/Services/StandardLists/UserListService");
var CodeNameClass_1 = require("./CodeNameClass");
var CustomerAdditionalServicesFilterComponent = /** @class */ (function (_super) {
    __extends(CustomerAdditionalServicesFilterComponent, _super);
    function CustomerAdditionalServicesFilterComponent() {
        var _this = _super.call(this) || this;
        _this.listOfValuesUserId = null;
        _this.CustomerId = null;
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SalesmanUserId = SessionLocator_1.SessionLocator.LoggedUserPM.Id;
        _this.BusinessUnitId = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId;
        _this.IsVisibale = false;
        _this.IsAll = true;
        _this.IsInUse = false;
        _this.IsPotential = false;
        _this.SelectedItem = "All";
        _this.IsAllId = "_IsAllId" + _this.CurrentSession.GetNewId("_IsAllId");
        _this.IsPotentialId = "_IsPotentialId" + _this.CurrentSession.GetNewId("_IsPotentialId");
        _this.IsInUseId = "_IsInUseId" + _this.CurrentSession.GetNewId("_IsInUseId");
        _this.TypeRadio = "_TypeRadio" + _this.CurrentSession.GetNewId("_TypeRadio");
        return _this;
    }
    CustomerAdditionalServicesFilterComponent.prototype.BuildCustomerStatusFilters = function () {
        this.CustomerStatusList = [];
        this.CustomerStatusList.push(new CodeNameClass_1.CodeNameClass("ALL", "All"));
        this.CustomerStatusList.push(new CodeNameClass_1.CodeNameClass("ACT", "Active"));
        this.CustomerStatusList.push(new CodeNameClass_1.CodeNameClass("POT", "Potential"));
        this.selectedCustomerStatus = this.CustomerStatusList.filter(function (d) { return d.Code == "ALL"; })[0];
    };
    Object.defineProperty(CustomerAdditionalServicesFilterComponent.prototype, "SelectedCustomerStatus", {
        get: function () { return this.selectedCustomerStatus; },
        set: function (value) {
            if (this.selectedCustomerStatus != value) {
                this.selectedCustomerStatus = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerAdditionalServicesFilterComponent.prototype, "ListOfValuesUserId", {
        get: function () {
            if (this.listOfValuesUserId != null) {
                return this.listOfValuesUserId;
            }
            else {
                return null;
            }
        },
        set: function (value) {
            var _this = this;
            this.listOfValuesUserId = value;
            if (value == "" || value == null) {
                this.SalesmanUserId = null;
                this.BusinessUnitId = null;
            }
            else {
                this.UsersCachedList.forEach(function (i) {
                    if (i.Id == value + "") {
                        _this.SalesmanUserId = i.Id;
                        _this.BusinessUnitId = i.BusinessUnitId;
                        return;
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerAdditionalServicesFilterComponent.prototype.IsListOfValuesVisible = function () {
        var visible = false;
        if (this.SelectedItemBusniessFilterd != null) {
            if (this.SelectedItemBusniessFilterd.Code == "A") {
                visible = true;
            }
            else {
                visible = false;
            }
        }
        else {
            return visible;
        }
    };
    Object.defineProperty(CustomerAdditionalServicesFilterComponent.prototype, "SelectedItemBusniessFilterd", {
        get: function () {
            return this.selectedItemBusniessFilterd;
        },
        set: function (value) {
            this.selectedItemBusniessFilterd = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerAdditionalServicesFilterComponent.prototype, "ServiceType", {
        get: function () {
            if (this.IsAll)
                return "All";
            else if (this.IsInUse)
                return "In Use";
            else
                return "Potential";
        },
        enumerable: true,
        configurable: true
    });
    CustomerAdditionalServicesFilterComponent.prototype.CheckUsersEnabled = function () {
        if (this.SelectedItemBusniessFilterd != null)
            if (this.SelectedItemBusniessFilterd.Code == 'M')
                return true;
        if (this.UsersFilterList != null)
            if (this.UsersFilterList.length == 0)
                return true;
        return false;
    };
    CustomerAdditionalServicesFilterComponent.prototype.SelectedBusniessUnitChanged = function (item1) {
        this.SelectedItemBusniessFilterd = item1;
        this.UsersFilterList = [];
        if (this.SelectedItemBusniessFilterd != null) {
            switch (this.SelectedItemBusniessFilterd.Code) {
                case "M": {
                    var item = new CodeNameClass_1.CodeNameClass();
                    item.Code = SessionLocator_1.SessionLocator.LoggedUserPM.Id;
                    item.Name = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                    this.UsersFilterList.push(item);
                    this.ListOfValuesUserId = this.TenantPM.Id;
                    this.SalesmanUserId = SessionLocator_1.SessionLocator.LoggedUserPM.Id;
                    this.BusinessUnitId = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId;
                    break;
                }
                case "A": {
                    var item = new CodeNameClass_1.CodeNameClass();
                    item.Code = "A";
                    item.Name = "All Owners";
                    this.UsersFilterList.push(item);
                    this.SalesmanUserId = null;
                    this.ListOfValuesUserId = null;
                    break;
                }
                default: {
                    var item = new CodeNameClass_1.CodeNameClass();
                    item.Code = "A";
                    item.Name = "All " + this.SelectedItemBusniessFilterd.Name + " Owners";
                    this.UsersFilterList.push(item);
                    this.SalesmanUserId = null;
                    this.ListOfValuesUserId = null;
                    this.getFromCachedList(this.UsersCachedList);
                    break;
                }
            }
        }
    };
    CustomerAdditionalServicesFilterComponent.prototype.getFromCachedList = function (myResult) {
        var _this = this;
        this.UsersCachedList = myResult;
        myResult.forEach(function (i) {
            if (i.BusinessUnitId == _this.SelectedItemBusniessFilterd.Code + "") {
                var item = new CodeNameClass_1.CodeNameClass();
                item.Code = i.Id;
                item.Name = i.EnglishName;
                _this.UsersFilterList.push(item);
            }
        });
        if (this.UsersFilterList.length > 0) {
            this.UsersFilterList.sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
        }
    };
    CustomerAdditionalServicesFilterComponent.prototype.BuildBusinessUnitFilterList = function (myResult) {
        var _this = this;
        var item = new CodeNameClass_1.CodeNameClass();
        item.Code = "M";
        item.Name = "My Records";
        this.BusniessItemSource.push(item);
        myResult.forEach(function (i) {
            if (i.Id != _this.TenantPM.Id + "") {
                var item = new CodeNameClass_1.CodeNameClass();
                item.Code = i.Id;
                item.Name = i.Name;
                _this.BusniessItemSource.push(item);
            }
        });
        item = new CodeNameClass_1.CodeNameClass();
        item.Code = "A";
        item.Name = "All Records";
        this.BusniessItemSource.push(item);
        this.SelectedBusniessUnitChanged(this.BusniessItemSource[0]);
    };
    CustomerAdditionalServicesFilterComponent.prototype.fillcombo = function (arr) {
        var _this = this;
        this.FilterdAdditionalService = [];
        arr.forEach(function (i) {
            if (!i.InActive) {
                var item = new CodeNameClass_1.CodeNameClass();
                item.Code = i.Id;
                item.Name = i.Name;
                item.Checked = false;
                _this.FilterdAdditionalService.push(i);
            }
            else {
            }
        });
        this.FilterdAdditionalService.sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
    };
    CustomerAdditionalServicesFilterComponent.prototype.IsAllClicked = function () {
        this.IsAll = true;
        this.IsPotential = false;
        this.IsInUse = false;
    };
    CustomerAdditionalServicesFilterComponent.prototype.IsPotentialClicked = function () {
        this.IsAll = false;
        this.IsPotential = true;
        this.IsInUse = false;
    };
    CustomerAdditionalServicesFilterComponent.prototype.IsInUseClicked = function () {
        this.IsAll = false;
        this.IsPotential = false;
        this.IsInUse = true;
    };
    CustomerAdditionalServicesFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        var _this = this;
        this.ReportsPreview = myReportsPreview;
        this.userListService = new UserListService_1.UserListService();
        this.reportDomainService = new ReportsDomainService_1.ReportsDomainService();
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.BuildCustomerStatusFilters();
        this.reportDomainService.GetBusinessUnitLists(this.TenantPM.Id).subscribe(function (myResult) {
            _this.BusniessItemSource = new Array();
            _this.BuildBusinessUnitFilterList(myResult);
        });
        this.reportDomainService.GetAdditionalServicesByTenant(this.TenantPM.Id).subscribe(function (myResult) {
            _this.fillcombo(myResult);
        });
        this.userListService.getAllFromCache().subscribe(function (myResult) {
            _this.UsersCachedList = myResult.Result;
        });
    };
    CustomerAdditionalServicesFilterComponent.prototype.ngOnInit = function () {
    };
    CustomerAdditionalServicesFilterComponent.prototype.EditedItemSource = function (newSource) {
        this.FilterdAdditionalService = newSource;
    };
    CustomerAdditionalServicesFilterComponent.prototype.SelectedAdditionalServiceChanged = function (item) {
        this.SelectedItem = item;
    };
    CustomerAdditionalServicesFilterComponent.prototype.RunReport = function (isloading) {
        this.ValidationErrorsList = [];
        if (!this.SelectedCustomerStatus) {
            this.ValidationErrorsList.push("Customer field is required");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array();
            var myAdditionalServices = "";
            if (this.SelectedItem == "All") {
                myAdditionalServices = "All";
            }
            else {
                this.FilterdAdditionalService.forEach(function (i) {
                    if (i.Checked) {
                        myAdditionalServices += i.Id + ",";
                    }
                });
            }
            if (this.SelectedCustomerStatus.Code == "ALL") {
                this.SelectedCustomerStatus.Code = "";
            }
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "AdditionalServices";
            this.queryFilterItem.FieldValue = myAdditionalServices;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "BusinessUnitId";
            this.queryFilterItem.FieldValue = this.BusinessUnitId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "SalesmanUserId";
            this.queryFilterItem.FieldValue = this.SalesmanUserId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ServiceType";
            this.queryFilterItem.FieldValue = this.ServiceType;
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerStatus";
            this.queryFilterItem.FieldValue = this.SelectedCustomerStatus.Code;
            this.queryFilterItems.push(this.queryFilterItem);
            this.reportFliter = new ReportFliter_1.ReportFliter();
            this.reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    };
    CustomerAdditionalServicesFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CustomerAdditionalServicesFilterComponent',
            templateUrl: './CustomerAdditionalServicesFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], CustomerAdditionalServicesFilterComponent);
    return CustomerAdditionalServicesFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerAdditionalServicesFilterComponent = CustomerAdditionalServicesFilterComponent;
//# sourceMappingURL=CustomerAdditionalServicesFilterComponent.js.map