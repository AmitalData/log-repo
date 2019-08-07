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
var UserExtendedListService_1 = require("../../../Common/Services/ExtendedLists/UserExtendedListService");
var UserLicensePM_1 = require("../../../Common/EntityPMs/UserLicensePM");
var Tools_1 = require("../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var UserExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/UserExtendedPMService");
var LicensesManagementComponent = /** @class */ (function () {
    function LicensesManagementComponent() {
        var _this = this;
        this.SearchFieldChangeEvent = new core_1.EventEmitter();
        this.Columns = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ListenEvent = null;
        this.DataSource = {
            pageSize: 100,
            rowCount: null,
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        this.DataLoaded = false;
        this.ValidationErrorsList = [];
        this.Listen();
    }
    LicensesManagementComponent.prototype.Listen = function () {
        var _this = this;
        this.ListenEvent = this.CurrentSession.PseventRowSelectEvent.subscribe(function (res) {
            if (res.Name == "Add") {
                _this.Add(res.User, res.PackageCode);
            }
            if (res.Name == "Remove") {
                _this.Remove(res.User, res.PackageCode);
            }
        });
    };
    LicensesManagementComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.ListenEvent);
        this.ListenEvent = null;
    };
    LicensesManagementComponent.prototype.SetWindowArgs = function (args) {
        this.AllPackages = args.AllPackages;
        this.dirtyItem = null;
        this.InitColumns();
        this.LoadUserLicenses();
    };
    LicensesManagementComponent.prototype.InitColumns = function () {
        this.Columns = [];
        this.Columns.push({
            FieldName: "EnglishName",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Name',
            ServerSideSortable: true,
            Styles: { width: '200px' },
        });
        this.Columns.push({
            FieldName: "Email",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            Display: 'Email',
            Styles: { width: '200px' },
        });
    };
    LicensesManagementComponent.prototype.GetRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = 100;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchFields)) {
            filters.Filter1Name = "SearchFields";
            filters.Filter1Value = this.SearchFields;
            filters.Filter1Operator = "Contains";
        }
        var service = new UserExtendedListService_1.UserExtendedListService();
        return new Promise(function (resolve, reject) { resolve(service.GetCustomDataByFilters(filters)); });
    };
    LicensesManagementComponent.prototype.LoadUserLicenses = function () {
        var _this = this;
        var userExtendedPMService = new UserExtendedPMService_1.UserExtendedPMService();
        userExtendedPMService.GetUserLicenses().subscribe(function (myResult) {
            if (myResult == null) {
                _this.LicensesManagmentsList = [];
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.AllUserLicenses = myResponse.Result;
                    _this.BuildHeaders();
                    _this.BuildAdditionalColumns();
                }
            }
        });
    };
    LicensesManagementComponent.prototype.BuildHeaders = function () {
        var _this = this;
        this.LicensesManagmentsList = [];
        var index = 0;
        SessionLocator_1.SessionLocator.TenantManagementJS.TenantManagementLicenses.sort(function (a, b) { return (a.PackageCode === b.PackageCode) ? 0 : (a.PackageCode < b.PackageCode) ? -1 : 1; }).forEach(function (item) {
            index++;
            if (index <= 10) {
                var myPackageName = "";
                var myPackageCode = null;
                var list = _this.AllPackages.filter(function (d) { return d.Code == item.PackageCode; })[0];
                if (list != null) {
                    myPackageName = list.Name;
                    myPackageCode = list.Code;
                }
                var usersCount = _this.AllUserLicenses.filter(function (d) { return d.PackageCode == item.PackageCode; }).length;
                var numberOfUsers = Tools_1.AppTool.IsNullOrZero(item.NumberOfUsers) ? 0 : item.NumberOfUsers;
                var foreground = Tools_1.FontTool.Black;
                if (usersCount > numberOfUsers) {
                    foreground = Tools_1.FontTool.Red;
                }
                var newItem = new LicensesManagementDataItem();
                newItem.Header = usersCount + "/" + numberOfUsers;
                newItem.Color = foreground;
                newItem.UsersCount = usersCount;
                newItem.NumberOfUsers = numberOfUsers;
                _this.LicensesManagmentsList.push(newItem);
            }
        });
    };
    LicensesManagementComponent.prototype.BuildAdditionalColumns = function () {
        var _this = this;
        this.DataLoaded = false;
        var index = 0;
        SessionLocator_1.SessionLocator.TenantManagementJS.TenantManagementLicenses.sort(function (a, b) { return (a.PackageCode === b.PackageCode) ? 0 : (a.PackageCode < b.PackageCode) ? -1 : 1; }).forEach(function (item) {
            index++;
            if (index <= 10) {
                var myPackageName = "";
                var myPackageCode = null;
                var list = _this.AllPackages.filter(function (d) { return d.Code == item.PackageCode; })[0];
                if (list != null) {
                    myPackageName = list.Name;
                    myPackageCode = list.Code;
                }
                _this.Columns.push({
                    FieldName: myPackageCode + "," + index,
                    DataTypeCode: 'Boolean',
                    Display: myPackageName,
                    IsCustomTemplate: true,
                    ServerSideSortable: true,
                    Styles: { width: '100px' },
                    HtmlListComponentName: 'ColumnCheckBoxComponent',
                    HtmlListComponentUrl: './InfrastructureModules/InfrastructureUser/Components/ColumnCheckBoxComponent',
                });
            }
        });
        this.DataLoaded = true;
    };
    LicensesManagementComponent.prototype.SearchTextChanged = function (searchText) {
        this.SearchFields = searchText;
        this.SearchFieldChangeEvent.emit(this.SearchFields);
    };
    LicensesManagementComponent.prototype.Add = function (user, myPackageCode) {
        this.dirtyItem = null;
        var itemPM = new UserLicensePM_1.UserLicensePM();
        itemPM.UserId = user.Id;
        itemPM.PackageCode = myPackageCode;
        itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        itemPM.Email = user.Email;
        if (this.AllUserLicenses.filter(function (d) { return d.UserId == user.Id && d.PackageCode == myPackageCode; }).length == 0) {
            this.AllUserLicenses.push(itemPM);
            this.dirtyItem = itemPM;
            this.Save(myPackageCode);
        }
    };
    LicensesManagementComponent.prototype.Remove = function (user, myPackageCode) {
        this.dirtyItem = null;
        var itemPM = this.AllUserLicenses.filter(function (d) { return d.UserId == user.Id && d.PackageCode == myPackageCode; })[0];
        if (itemPM != null) {
            var index = this.AllUserLicenses.indexOf(itemPM);
            if (index > -1) {
                this.AllUserLicenses.splice(index, 1);
                if (!Tools_1.AppTool.IsNullOrEmpty(itemPM.Id)) {
                    this.dirtyItem = itemPM;
                }
                this.Save(myPackageCode);
            }
        }
    };
    LicensesManagementComponent.prototype.Save = function (myPackageCode) {
        var _this = this;
        var errors = [];
        var userLicenses = this.AllUserLicenses.filter(function (d) { return d.PackageCode == myPackageCode; });
        var tenantLicenses = SessionLocator_1.SessionLocator.TenantManagementJS.TenantManagementLicenses.filter(function (d) { return d.PackageCode == myPackageCode; })[0];
        var usersCount = userLicenses.length;
        var numberOfUsers = tenantLicenses.NumberOfUsers;
        if (usersCount > numberOfUsers) {
            errors.push("Some Packages have exceeded the allowed number of users");
        }
        this.BuildHeaders();
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0 && this.dirtyItem != null) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show("You will need to logout and login again for the changes to take place");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.RunSave();
                }
            });
        }
    };
    LicensesManagementComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    LicensesManagementComponent.prototype.RunSave = function () {
        var _this = this;
        if (this.dirtyItem != null) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myServiceHelper = new CommonDomainService_1.UserLicenseUpdateHelper();
            myServiceHelper.Tenant = SessionLocator_1.SessionLocator.Tenant;
            myServiceHelper.Items.push(this.dirtyItem);
            var generalService = new CommonDomainService_1.CommonDomainService();
            generalService.UpdateUserLicense(myServiceHelper).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    var itemPM = _this.AllUserLicenses.filter(function (d) { return d.UserId == _this.dirtyItem.UserId && d.PackageCode == _this.dirtyItem.PackageCode; })[0];
                    if (itemPM != null) {
                        var index = _this.AllUserLicenses.indexOf(itemPM);
                        if (index > -1) {
                            _this.AllUserLicenses.filter(function (d) { return d.UserId == _this.dirtyItem.UserId && d.PackageCode == _this.dirtyItem.PackageCode; })[0].Id = myResponse.Result.Items[0].Id;
                        }
                    }
                    _this.dirtyItem = null;
                    //this.LoadUserLicenses();
                    _this.BuildHeaders();
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LicensesManagementComponent.prototype, "SearchFieldChangeEvent", void 0);
    LicensesManagementComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './LicensesManagementComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], LicensesManagementComponent);
    return LicensesManagementComponent;
}());
exports.LicensesManagementComponent = LicensesManagementComponent;
var LicensesManagementDataItem = /** @class */ (function () {
    function LicensesManagementDataItem() {
    }
    return LicensesManagementDataItem;
}());
exports.LicensesManagementDataItem = LicensesManagementDataItem;
//# sourceMappingURL=LicensesManagementComponent.js.map