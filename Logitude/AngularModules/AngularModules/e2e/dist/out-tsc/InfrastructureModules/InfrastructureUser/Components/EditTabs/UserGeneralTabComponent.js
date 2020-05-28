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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var TenantLoginPolicyListService_1 = require("../../../../Common/Services/StandardLists/TenantLoginPolicyListService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var UserExtendedListService_1 = require("../../../../Common/Services/ExtendedLists/UserExtendedListService");
var UserGeneralTabComponent = /** @class */ (function (_super) {
    __extends(UserGeneralTabComponent, _super);
    function UserGeneralTabComponent(entityArgs, TenantLoginPolicyListService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.TenantLoginPolicyListService = TenantLoginPolicyListService;
        _this.ObjectTableName = "User";
        _this.DataContext = _this;
        _this.TechnologyList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsShowFactorAuthenticationEnabled = false;
        _this.IsEditingEnabled = false;
        _this.IsPersonalIdVisible = false;
        _this.IsExpirationDateVisible = false;
        _this.IsSalesmanVisible = false;
        _this.IsLicencedUserVisible = false;
        _this.IsShowContactInMobileVisiable = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.BuildTechnologyList();
        _this.SetUIProperties();
        _this.Listen();
        _this.CheckSecurityPolicySettingToShowPhone();
        return _this;
    }
    UserGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.SetUIProperties();
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.SetUIProperties();
                    }
                });
            }
        }
    };
    UserGeneralTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    UserGeneralTabComponent.prototype.BuildTechnologyList = function () {
        var _this = this;
        this.TechnologyList = [];
        this.TechnologyList.push(new CodeNameClass_1.CodeNameClass("AG", "Angular"));
        this.TechnologyList.push(new CodeNameClass_1.CodeNameClass("SL", "SilverLight"));
        this.TechnologyList.push(new CodeNameClass_1.CodeNameClass("DE", "Default"));
        this.TechnologyList.push(new CodeNameClass_1.CodeNameClass("PR", "Prompt"));
        if (this.Technology) {
            this.SelectedTechnology = this.TechnologyList.filter(function (d) { return d.Code == _this.EntityPM.Technology; })[0];
        }
        else {
            this.SelectedTechnology = this.TechnologyList[0];
        }
    };
    UserGeneralTabComponent.prototype.TechnologyValueChanged = function (item) {
        this.SelectedTechnology = item;
        if (item == null) {
            this.Technology = null;
        }
        else {
            this.Technology = item.Code;
        }
    };
    UserGeneralTabComponent.prototype.CheckSecurityPolicySettingToShowPhone = function () {
        var _this = this;
        this.TenantLoginPolicyListService.getSingle(SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                var result = pmResponse.Result;
                if (result.IsEnabledForSpecificUsers && result.LoginPolicyCode != "DISABLED") {
                    _this.IsShowFactorAuthenticationEnabled = true;
                }
            }
        });
    };
    UserGeneralTabComponent.prototype.SetUIProperties = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "PERSONALID")) {
            this.IsPersonalIdVisible = true;
        }
        if (SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.IsExpirationDateVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "ROLES")) {
            this.IsSalesmanVisible = true;
        }
        if (SessionLocator_1.SessionLocator.TenantManagementJS.ManageLicencesPerUser) {
            this.IsLicencedUserVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "MOBILE")) {
            this.IsShowContactInMobileVisiable = true;
        }
        var isEditingEnabled = true;
        if (SessionLocator_1.SessionLocator.Tenant == 65) {
            if (!SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                isEditingEnabled = false;
            }
        }
        this.IsEditingEnabled = isEditingEnabled;
        this.UIProperties.SetEnabled("Email", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PersonalId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("DepartmentId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("BusinessUnitId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ExpirationDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("Technology", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("IsSalesman", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InActive", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("LicencedUser", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("IsShowContactDetailsInTheMobileApp", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ShowLocalNameInLOV", this.ObjectTableName, isEditingEnabled);
    };
    Object.defineProperty(UserGeneralTabComponent.prototype, "Email", {
        get: function () { return this.EntityPM.Email; },
        set: function (value) {
            if (this.EntityPM.Email != value) {
                this.EntityPM.Email = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "PersonalId", {
        get: function () { return this.EntityPM.PersonalId; },
        set: function (value) {
            if (this.EntityPM.PersonalId != value) {
                this.EntityPM.PersonalId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "DepartmentId", {
        get: function () { return this.EntityPM.DepartmentId; },
        set: function (value) {
            if (this.EntityPM.DepartmentId != value) {
                this.EntityPM.DepartmentId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "BranchId", {
        get: function () { return this.EntityPM.BranchId; },
        set: function (value) {
            if (this.EntityPM.BranchId != value) {
                this.EntityPM.BranchId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "BusinessUnitId", {
        get: function () { return this.EntityPM.BusinessUnitId; },
        set: function (value) {
            if (this.EntityPM.BusinessUnitId != value) {
                this.EntityPM.BusinessUnitId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "ExpirationDate", {
        get: function () { return this.EntityPM.ExpirationDate; },
        set: function (value) {
            if (this.EntityPM.ExpirationDate != value) {
                this.EntityPM.ExpirationDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "Technology", {
        get: function () { return this.EntityPM.Technology; },
        set: function (value) {
            if (this.EntityPM.Technology != value) {
                this.EntityPM.Technology = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "IsSalesman", {
        get: function () { return this.EntityPM.IsSalesman; },
        set: function (value) {
            if (this.EntityPM.IsSalesman != value) {
                this.EntityPM.IsSalesman = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.InActive != value) {
                var isDirty = this.EntityPM.IsDirty;
                this.EntityPM.InActive = value;
                if (SessionLocator_1.SessionLocator.TenantManagementJS.IsMultiPackage) {
                    var service = new UserExtendedListService_1.UserExtendedListService();
                    service.GetUserLicensesCountForUser(this.EntityPM.Id).subscribe(function (myResult) {
                        var myResponse = myResult;
                        if (!myResponse.HasError) {
                            var count = myResponse.Result;
                            if (count > 0) {
                                if (value) {
                                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                                    confirmWindow.Show("Setting this user as inactive will disconnect its licensese");
                                    confirmWindow.WindowClosed.subscribe(function (event) {
                                        if (confirmWindow.Yes) {
                                        }
                                        else if (confirmWindow.No) {
                                            _this.InActive = false;
                                            if (!isDirty) {
                                                _this.EntityPM.IsDirty = false;
                                            }
                                        }
                                    });
                                }
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "LicencedUser", {
        get: function () { return this.EntityPM.LicencedUser; },
        set: function (value) {
            if (this.EntityPM.LicencedUser != value) {
                this.EntityPM.LicencedUser = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "IsShowContactDetailsInTheMobileApp", {
        get: function () { return this.EntityPM.IsShowContactDetailsInTheMobileApp; },
        set: function (value) {
            if (this.EntityPM.IsShowContactDetailsInTheMobileApp != value) {
                this.EntityPM.IsShowContactDetailsInTheMobileApp = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "Mobile", {
        get: function () { return this.EntityPM.Mobile; },
        set: function (value) {
            if (this.EntityPM.Mobile != value) {
                this.EntityPM.Mobile = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "IsTwoFactorAuthenticationEnabled", {
        get: function () { return this.EntityPM.IsTwoFactorAuthenticationEnabled; },
        set: function (value) {
            if (this.EntityPM.IsTwoFactorAuthenticationEnabled != value) {
                this.EntityPM.IsTwoFactorAuthenticationEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserGeneralTabComponent.prototype, "ShowLocalNameInLOV", {
        get: function () { return this.EntityPM.ShowLocalNameInLOV; },
        set: function (value) {
            if (this.EntityPM.ShowLocalNameInLOV != value) {
                this.EntityPM.ShowLocalNameInLOV = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    UserGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './UserGeneralTabComponent.html',
            providers: [TenantLoginPolicyListService_1.TenantLoginPolicyListService],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, TenantLoginPolicyListService_1.TenantLoginPolicyListService])
    ], UserGeneralTabComponent);
    return UserGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.UserGeneralTabComponent = UserGeneralTabComponent;
//# sourceMappingURL=UserGeneralTabComponent.js.map