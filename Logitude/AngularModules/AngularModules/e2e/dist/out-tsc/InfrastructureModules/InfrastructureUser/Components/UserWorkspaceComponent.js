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
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var UserExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/UserExtendedPMService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var Args_1 = require("../../../Infrastructure/Args");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var PackageListService_1 = require("../../../Common/Services/StandardLists/PackageListService");
var Tools_1 = require("../../../Infrastructure/Tools");
var Args_2 = require("../../../Infrastructure/Args");
var UserWorkspaceComponent = /** @class */ (function () {
    function UserWorkspaceComponent(_userExtendedPMService) {
        this._userExtendedPMService = _userExtendedPMService;
        this.SearchText = "Search names /positions";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.BackButtonText = "Maintenance";
        this.IsDemoTenant = false;
        this.IsLicensesManagmentSystem = false;
        this.ReloadUserQueries = new core_1.EventEmitter();
        this.AllUsersQueryVisibility = false;
        this.ActiveUsersQueryVisibility = false;
        this.InactiveUsersQueryVisibility = false;
        this.ActiveNotLicensedQueryVisibility = false;
        this.MyViewsQueryVisibility = false;
        this.IsLicensesVisibile = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
    }
    UserWorkspaceComponent.prototype.ngOnInit = function () {
    };
    UserWorkspaceComponent.prototype.Run = function (args) {
        if (args) {
            this.BackButtonText = args.BackButtonText;
        }
        if (SessionLocator_1.SessionLocator.Tenant == 65 && !SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.IsDemoTenant = true;
        }
        if (!this.IsDemoTenant) {
            this.InitCounts();
            this.CheckVisibilityProperties();
            this.InitLicensesManagment();
            this.LoadAllData();
        }
    };
    UserWorkspaceComponent.prototype.InitCounts = function () {
        this.LicensesCount = "0";
        this.AllUsersCount = "0";
        this.ActiveUsersCount = "0";
        this.InactiveUsersCount = "0";
        this.ActiveLicensedCount = "0";
        this.ActiveNotLicensedCount = "0";
    };
    UserWorkspaceComponent.prototype.InitLicensesManagment = function () {
        var isLicensesManagmentSystem = false;
        if (SessionLocator_1.SessionLocator.TenantManagementJS.IsMultiPackage) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "User.Feature.LicensesManagment")) {
                isLicensesManagmentSystem = true;
            }
        }
        this.IsLicensesManagmentSystem = isLicensesManagmentSystem;
        if (this.IsLicensesManagmentSystem) {
            this.LoadUserLicenses();
        }
    };
    UserWorkspaceComponent.prototype.LoadUserLicenses = function () {
        var _this = this;
        this._userExtendedPMService.GetUserLicenses().subscribe(function (myResult) {
            if (myResult == null) {
                _this.LicensesManagmentsList = [];
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.allUserLicenses = myResponse.Result;
                    _this.BuildLicensesManagmentCounts();
                    _this.CheckUserLicenseExclamationMark();
                }
            }
        });
    };
    UserWorkspaceComponent.prototype.BuildLicensesManagmentCounts = function () {
        var _this = this;
        this.LicensesManagmentsList = [];
        var service = new PackageListService_1.PackageListService();
        service.getAllFromCache().subscribe(function (result) {
            _this.allPackages = result.Result;
            _this.FillLicensesManagmentsList();
        });
    };
    UserWorkspaceComponent.prototype.FillLicensesManagmentsList = function () {
        var _this = this;
        var index = 0;
        SessionLocator_1.SessionLocator.TenantManagementJS.TenantManagementLicenses.sort(function (a, b) { return (a.PackageCode === b.PackageCode) ? 0 : (a.PackageCode < b.PackageCode) ? -1 : 1; }).forEach(function (item) {
            index++;
            if (index <= 10) {
                var myPackageName = "";
                var myPackage = _this.allPackages.filter(function (d) { return d.Code == item.PackageCode; })[0];
                if (myPackage != null) {
                    myPackageName = myPackage.Name;
                }
                var usersCount = _this.allUserLicenses.filter(function (d) { return d.PackageCode == item.PackageCode; }).length;
                var numberOfUsers = Tools_1.AppTool.IsNullOrZero(item.NumberOfUsers) ? 0 : item.NumberOfUsers;
                var myCountText = usersCount + "/" + numberOfUsers;
                var newItem = new LicensesManagementDataItem();
                newItem.PackageCode = item.PackageCode;
                newItem.PackageName = myPackageName;
                newItem.CountText = myCountText;
                _this.LicensesManagmentsList.push(newItem);
            }
        });
    };
    UserWorkspaceComponent.prototype.CheckUserLicenseExclamationMark = function () {
        var _this = this;
        this.ShowUserLicenseExclamationMark = false;
        if (this.allUserLicenses != null) {
            SessionLocator_1.SessionLocator.TenantManagementJS.TenantManagementLicenses.forEach(function (item) {
                var usersCount = _this.allUserLicenses.filter(function (d) { return d.PackageCode == item.PackageCode; }).length;
                var numberOfUsers = Tools_1.AppTool.IsNullOrZero(item.NumberOfUsers) ? 0 : item.NumberOfUsers;
                if (usersCount < numberOfUsers) {
                    _this.ShowUserLicenseExclamationMark = true;
                    //break;
                }
            });
        }
    };
    UserWorkspaceComponent.prototype.LicensesManagementClicked = function () {
        var _this = this;
        var args = new Args_2.UserLicenseArgs();
        args.AllPackages = this.allPackages;
        args.AllUserLicenses = this.allUserLicenses;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 780;
        logitudeWindow.Title = "Licenses Management";
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureUser/Components/LicensesManagementComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            _this.LoadUserLicenses();
        });
    };
    UserWorkspaceComponent.prototype.RefreshBackButtonEnabled = function () {
        if (this.isLoadDataSummaryCompleted && this.isLoadRecentUsersCompleted) {
            this.IsBackButtonEnabled = true;
        }
    };
    UserWorkspaceComponent.prototype.LoadAllData = function () {
        this.IsBackButtonEnabled = false;
        this.LoadDataSummary();
        this.LoadRecentUsers();
        this.BuildCustomQueriesList();
    };
    UserWorkspaceComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllData();
    };
    UserWorkspaceComponent.prototype.LoadDataSummary = function () {
        var _this = this;
        this.InitCounts();
        this.isLoadDataSummaryCompleted = false;
        this._userExtendedPMService.GetUsersWorkspaceSummary(SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.LicensesCount = myResult.LicensesCount > 1000 ? "1000+" : myResult.LicensesCount.toString();
                    _this.AllUsersCount = myResult.AllUsersCount > 1000 ? "1000+" : myResult.AllUsersCount.toString();
                    _this.ActiveUsersCount = myResult.ActiveUsersCount > 1000 ? "1000+" : myResult.ActiveUsersCount.toString();
                    _this.InactiveUsersCount = myResult.InactiveUsersCount > 1000 ? "1000+" : myResult.InactiveUsersCount.toString();
                    _this.ActiveLicensedCount = myResult.ActiveLicensedCount > 1000 ? "1000+" : myResult.ActiveLicensedCount.toString();
                    _this.ActiveNotLicensedCount = myResult.ActiveNotLicensedCount > 1000 ? "1000+" : myResult.ActiveNotLicensedCount.toString();
                    _this.isLoadDataSummaryCompleted = true;
                    _this.RefreshBackButtonEnabled();
                }
            }
        });
    };
    UserWorkspaceComponent.prototype.LoadRecentUsers = function () {
        var _this = this;
        this.RecentUserLists = [];
        this.isLoadRecentUsersCompleted = false;
        this._userExtendedPMService.GetUsersWorkspaceRecentLogins(SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.RecentUserLists = myResult;
                    _this.RecentUserLists.forEach(function (item) {
                        item.IPSiteUri = "http://www.infosniper.net/index.php?ip_address=" + item.IP;
                    });
                    _this.SelectedUserViewModel = _this.RecentUserLists[0];
                    _this.isLoadRecentUsersCompleted = true;
                    _this.RefreshBackButtonEnabled();
                }
            }
        });
    };
    UserWorkspaceComponent.prototype.BuildCustomQueriesList = function () {
        this.ReloadUserQueries.emit();
    };
    UserWorkspaceComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllData();
    };
    UserWorkspaceComponent.prototype.CheckVisibilityProperties = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "User.Query.AllUsers")) {
            this.AllUsersQueryVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "User.Query.ActiveUsers")) {
            this.ActiveUsersQueryVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "User.Query.InactiveUsers")) {
            this.InactiveUsersQueryVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "User.Query.ActiveNotLicensed")) {
            if (SessionLocator_1.SessionLocator.TenantManagementJS.ManageLicencesPerUser) {
                this.ActiveNotLicensedQueryVisibility = true;
            }
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES")) {
            this.MyViewsQueryVisibility = true;
        }
        if (SessionLocator_1.SessionLocator.TenantManagementJS.ManageLicencesPerUser) {
            this.IsLicensesVisibile = true;
        }
    };
    UserWorkspaceComponent.prototype.ViewUserQuery = function (myCode, nameTextCodeCode) {
        var _this = this;
        if (nameTextCodeCode === void 0) { nameTextCodeCode = null; }
        var backButtonTitle = "User Work Space";
        var queryCode = "";
        var displayTitle = "";
        if (myCode != null) {
            if (nameTextCodeCode) {
                queryCode = myCode;
                displayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate(nameTextCodeCode);
            }
            else {
                switch (myCode) {
                    case "ACTV":
                        {
                            queryCode = "ActiveUsers";
                            displayTitle = "Active Users";
                            break;
                        }
                    case "NTLC":
                        {
                            queryCode = "ActiveNotLicensed";
                            displayTitle = "Active Not Licensed";
                            break;
                        }
                    case "INAC":
                        {
                            queryCode = "InactiveUsers";
                            displayTitle = "Inactive Users";
                            break;
                        }
                    case "All":
                        {
                            queryCode = "AllUsers";
                            displayTitle = "All Users";
                            break;
                        }
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = "User";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            var objectTablePM = window.ObjectTables.filter(function (d) { return d.Name == "User"; })[0];
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/ListComponent/ListComponent", this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllData(); });
            });
        }
    };
    UserWorkspaceComponent.prototype.BackButtonClicked = function () {
        if (this.ComponentRef != null) {
            this.ComponentRef.destroy();
            this.ComponentRef = null;
        }
    };
    UserWorkspaceComponent.prototype.EditUser = function (selectedItem) {
        var _this = this;
        if (selectedItem) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: selectedItem.Id, ObjectTableName: 'User', BackButtonLabel: 'Users Workspace' });
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllData(); });
            });
        }
    };
    UserWorkspaceComponent.prototype.ShowHistory = function (selectedItem) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 500;
        logWindow.Title = "User Login History";
        logWindow.DataContext = selectedItem;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/UserLoginHistoryComponent');
    };
    UserWorkspaceComponent.prototype.SupportManagement = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 300;
        logWindow.Height = 260;
        logWindow.Title = "Support Management";
        logWindow.DataContext = this;
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/SupportManagementComponent');
    };
    UserWorkspaceComponent.prototype.ResetUserPassword = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 250;
        logWindow.Title = "Reset User Password";
        logWindow.DataContext = this;
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/ResetUserPasswordComponent');
    };
    UserWorkspaceComponent.prototype.ViewNewUserWindow = function () {
        this._entityResourceService.getEntityResourceByTableName("User", 0).subscribe(function (response) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 965;
            logWindow.Height = 600;
            logWindow.Title = "New User";
            logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/NewUserComponent');
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], UserWorkspaceComponent.prototype, "ReloadUserQueries", void 0);
    UserWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'UserWorkspace',
            templateUrl: './UserWorkspaceComponent.html',
            providers: [UserExtendedPMService_1.UserExtendedPMService]
        }),
        __metadata("design:paramtypes", [UserExtendedPMService_1.UserExtendedPMService])
    ], UserWorkspaceComponent);
    return UserWorkspaceComponent;
}());
exports.UserWorkspaceComponent = UserWorkspaceComponent;
var LicensesManagementDataItem = /** @class */ (function () {
    function LicensesManagementDataItem() {
    }
    return LicensesManagementDataItem;
}());
exports.LicensesManagementDataItem = LicensesManagementDataItem;
//# sourceMappingURL=UserWorkspaceComponent.js.map