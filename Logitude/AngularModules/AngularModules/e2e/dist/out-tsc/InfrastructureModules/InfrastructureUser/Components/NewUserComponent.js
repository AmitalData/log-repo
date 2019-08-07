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
var UserRolesTabComponent_1 = require("./EditTabs/UserRolesTabComponent");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var UserPM_1 = require("../../../Common/EntityPMs/UserPM");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ClassLevelValidator_1 = require("../../../Infrastructure/Validators/ClassLevelValidator");
var Tools_1 = require("../../../Infrastructure/Tools");
var PasswordChangeService_1 = require("../../../Common/Services/Others/PasswordChangeService");
var UserPMService_1 = require("../../../Common/Services/StandardPMs/UserPMService");
var RoleExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/RoleExtendedPMService");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var forms_1 = require("@angular/forms");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var NewUserComponent = /** @class */ (function (_super) {
    __extends(NewUserComponent, _super);
    function NewUserComponent(fb, _passwordChangeService, _roleExtendedPMService) {
        var _this = _super.call(this) || this;
        _this._passwordChangeService = _passwordChangeService;
        _this._roleExtendedPMService = _roleExtendedPMService;
        _this.ReTypePassword = "";
        _this.ObjectFieldHelp = "";
        //IsFreelancerVisible: boolean = false;
        _this.IsCurrentUserFreelancer = false;
        _this.CanSave = true;
        _this.NewUserPM = new UserPM_1.UserPM();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (_this.userPMService == null) {
            _this.userPMService = new UserPMService_1.UserPMService();
        }
        _this.myForm = fb.group({});
        _this.validator = new ClassLevelValidator_1.ClassLevelValidator();
        _this.NewUserPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.NewUserPM.BusinessUnitId = SessionLocator_1.SessionLocator.Tenant.toString();
        _this.IsCurrentUserFreelancer = SessionLocator_1.SessionLocator.LoggedUserPM.IsFreelancer;
        //if (ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
        if (ObjectsLocator_1.ObjectsLocator != null && ObjectsLocator_1.ObjectsLocator.GlobalSetting != null && ObjectsLocator_1.ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
            if (_this.IsCurrentUserFreelancer) {
                //this.NewUserPM.IsFreelancer = true;
                //this.IsFreelancerVisible = false;
            }
            else {
                //this.IsFreelancerVisible = true;
            }
        }
        return _this;
    }
    NewUserComponent.prototype.ngOnInit = function () {
        this.Run();
    };
    NewUserComponent.prototype.Run = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "ROLES"))
            this.RolesAreaVisibility = true;
        this.IsScreenEnabled = true;
        if (SessionInfo_1.SessionInfo.LoggedUserTenant == 65) {
            this.DemoTenantMessageVisibility = true;
            this.IsScreenEnabled = false;
            if (SessionInfo_1.SessionInfo.LoggedUserPM.IsCustomerCare) {
                this.IsScreenEnabled = true;
                this.IsPasswordDisable = false;
                this.DemoTenantMessageVisibility = false;
            }
            var date = Tools_1.DateTool.GetCurrentDateAsUtc();
            var month = date.getUTCMonth();
            var year = date.getUTCFullYear();
            var day = date.getUTCDate();
            var result = new Date();
            result.setUTCFullYear(year);
            result.setUTCMonth(month);
            result.setUTCDate(day + 7);
            this.NewUserPM.ExpirationDate = result;
        }
        this.SetUiProperties_Visibility();
        this.SetUiProperties_IsEnabled();
        this.LoadUserRolesMethod();
        var feature = FeatureLocator_1.FeatureLocator.Features.filter(function (x) { return x.Code === "PERSONALID"; })[0];
        if (feature == null)
            this.PersonalIdVisible = false;
        else
            this.PersonalIdVisible = true;
    };
    NewUserComponent.prototype.LoadUserRolesMethod = function () {
        var _this = this;
        this._roleExtendedPMService.GetRolesForUser(null, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.allRoles = myResult;
                    _this.BuildObsList();
                }
            }
        });
    };
    NewUserComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        var Password = "";
        if (this.CanSave) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = this.validator.Validate("User", this.NewUserPM);
            if (this.NewUserPM.Password) {
                Password = this.NewUserPM.Password.trim();
            }
            else {
                this.ValidationErrorsList.push("Please fill the password field!");
            }
            if (this.ReTypePassword) {
                this.ReTypePassword = this.ReTypePassword.trim();
            }
            if (this.ObsList.filter(function (d) { return d.IsActive; }).length == 0) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.AddRoleToUser"));
            }
            if (Password != this.ReTypePassword) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.CurrentPasswordDoesntMatchYourInput"));
            }
            if (!Tools_1.FormatTool.IsEmail(this.NewUserPM.Email)) {
                this.ValidationErrorsList.push("Invalid email format!");
            }
            if (this.ValidationErrorsList.length == 0) {
                this.ValidationErrorsList = [];
                this.NewUserPM.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                this.NewUserPM.Technology = "AG";
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
                this.userPMService.insert(this.NewUserPM).subscribe(function (myResult) {
                    if (myResult) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        if (myResult.HasError) {
                            myResult.ErrorsArray.forEach(function (item) {
                                _this.ValidationErrorsList.push(item);
                            });
                        }
                        else {
                            var newUserPM = myResult.Result;
                            _this.CurrentSession.CloseCurrentWindowEmit(newUserPM.Id);
                        }
                    }
                }, function (error) {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    var dd = error;
                    console.log(dd.text);
                });
            }
        }
    };
    NewUserComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewUserComponent.prototype.EmailTextChangedMethod = function (s) {
        if (this.ValidationErrorsList != null) {
            var err = this.ValidationErrorsList.filter(function (e) { return e == TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.ContactExistInCurrentTenant"); })[0];
            if (err != null) {
                this.ValidationErrorsList = [];
            }
        }
    };
    NewUserComponent.prototype.EmailKeyUpMethod = function (email) {
        var _this = this;
        this.IsPasswordDisable = false;
        this.Info1Text = "";
        this.Info1Visibility = false;
        this.CanSave = true;
        this.NewUserPM.Password = this.ReTypePassword = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(email)) {
            email = email.trim();
            if (email.indexOf('.') > 0 && email.indexOf('@') > 0) {
                this._passwordChangeService.CheckIfUserIsExists(email, SessionLocator_1.SessionLocator.Tenant, false, false).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            var result = myResult.split('@');
                            if (result[0] == "True") {
                                _this.ValidationErrorsList = [];
                                _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.ContactExistInCurrentTenant"));
                                _this.CanSave = false;
                                _this.IsPasswordDisable = false;
                            }
                            else {
                                if (_this.ValidationErrorsList) {
                                    var index = _this.ValidationErrorsList.indexOf(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.ContactExistInCurrentTenant"));
                                    if (index != -1)
                                        _this.ValidationErrorsList.splice(index, 1);
                                }
                                if (result[1] == "True") {
                                    _this.IsPasswordDisable = true;
                                    _this.NewUserPM.Password = _this.ReTypePassword = "123";
                                    _this.Info1Text = "This email already has a password";
                                    _this.Info1Visibility = true;
                                }
                                else {
                                    _this.IsPasswordDisable = false;
                                    _this.Info1Text = "";
                                    _this.Info1Visibility = false;
                                    _this.CanSave = true;
                                }
                            }
                        }
                    }
                });
            }
            else {
                this.CanSave = true;
            }
        }
        else {
            this.CanSave = true;
        }
    };
    NewUserComponent.prototype.SetUiProperties_Visibility = function () {
        if (SessionLocator_1.SessionLocator.TenantManagementJS.ManageLicencesPerUser)
            this.LicencedUserVisible = true;
        else
            this.LicencedUserVisible = false;
        if (SessionInfo_1.SessionInfo.LoggedUserTenant == 65 && SessionInfo_1.SessionInfo.LoggedUserPM.IsCustomerCare)
            this.ExpirationDateVisible = true;
        else
            this.ExpirationDateVisible = false;
        if (SessionInfo_1.SessionInfo.LoggedUserTenant != 0) {
            this.IsDistributorVisible = false;
            this.DistributorCodeVisible = false;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "ROLES"))
            this.IsSalesmanVisible = true;
        else
            this.IsSalesmanVisible = false;
    };
    NewUserComponent.prototype.SetUiProperties_IsEnabled = function () {
        this.NewUserPM.UIProperties.SetEnabled("Email", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("EnglishName", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("LocalName", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("PersonalId", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("ExpirationDate", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("IsSalesman", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("LicencedUser", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("IsDistributor", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("DistributorCode", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("DepartmentId", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("BranchId", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("BusinessUnitId", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("Notes", "User", this.IsScreenEnabled);
    };
    NewUserComponent.prototype.EditRole = function (myRole) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.Title = "Edit " + myRole.Name + " Role";
        logWindow.WindowArgs = { RolePM: myRole };
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/EditRoleFeaturesComponent');
    };
    NewUserComponent.prototype.BuildObsList = function () {
        var _this = this;
        this.ObsList = new Array();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "ROLES")) {
            var isEditingAllowed = this.IsScreenEnabled;
            this.allRoles.forEach(function (item) {
                switch (item.Code) {
                    case "DIST":
                    case "CUCA":
                        {
                            if (SessionInfo_1.SessionInfo.LoggedUserTenant == 0) {
                                _this.ObsList.push(new UserRolesTabComponent_1.UserRolesItemClass(item, _this.NewUserPM, _this));
                            }
                            break;
                        }
                    default:
                        {
                            if (item.IsCustomRole) {
                                if (item.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant) {
                                    _this.ObsList.push(new UserRolesTabComponent_1.UserRolesItemClass(item, _this.NewUserPM, _this));
                                }
                            }
                            else {
                                _this.ObsList.push(new UserRolesTabComponent_1.UserRolesItemClass(item, _this.NewUserPM, _this));
                            }
                            break;
                        }
                }
            });
        }
        else {
            var adminRole = this.allRoles.filter(function (d) { return d.Code == "ADMN"; })[0];
            if (adminRole) {
                var adminItem = new UserRolesTabComponent_1.UserRolesItemClass(adminRole, this.NewUserPM, this);
                adminItem.IsActive = true;
                this.ObsList.push(adminItem);
            }
        }
    };
    NewUserComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'NewUser',
            templateUrl: './NewUserComponent.html',
            providers: [PasswordChangeService_1.PasswordChangeService, UserPMService_1.UserPMService, RoleExtendedPMService_1.RoleExtendedPMService]
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder, PasswordChangeService_1.PasswordChangeService, RoleExtendedPMService_1.RoleExtendedPMService])
    ], NewUserComponent);
    return NewUserComponent;
}(BaseComponent_1.BaseComponent));
exports.NewUserComponent = NewUserComponent;
//# sourceMappingURL=NewUserComponent.js.map