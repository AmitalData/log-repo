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
var RolePM_1 = require("../../../../Common/EntityPMs/RolePM");
var UserRolesPM_1 = require("../../../../Common/EntityPMs/UserRolesPM");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var ExcelExportService_1 = require("../../../../Common/Services/Others/ExcelExportService");
var RoleExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/RoleExtendedPMService");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ImageParameter_1 = require("../../../../Infrastructure/DataContracts/ImageParameter");
var UserRolesTabComponent = /** @class */ (function (_super) {
    __extends(UserRolesTabComponent, _super);
    function UserRolesTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "User";
        _this.DataContext = _this;
        _this.ObsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsEditingEnabled = false;
        _this.IsExportButtonVisible = false;
        _this.IsNewRoleButtonVisible = false;
        _this.ImportFeaturesFileHtmlId = Guid_1.Guid.NewRandomString();
        _this.isEditingRoleRequested = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.InitializeServices();
        _this.SetUIProperties();
        _this.LoadUserRoles();
        _this.Listen();
        return _this;
    }
    UserRolesTabComponent.prototype.InitializeServices = function () {
        this.excelExportService = new ExcelExportService_1.ExcelExportService();
        this.roleExtendedPMService = new RoleExtendedPMService_1.RoleExtendedPMService();
    };
    UserRolesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.SetUIProperties();
                        if (_this.isEditingRoleRequested) {
                            if (_this.editedRole) {
                                _this.EditRole(_this.editedRole);
                            }
                        }
                    }
                    _this.isEditingRoleRequested = false;
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.SetUIProperties();
                    }
                    _this.isEditingRoleRequested = false;
                });
            }
        }
    };
    UserRolesTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    UserRolesTabComponent.prototype.SetUIProperties = function () {
        var isEditingEnabled = true;
        if (SessionLocator_1.SessionLocator.Tenant == 65) {
            if (!SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                isEditingEnabled = false;
            }
            else if (this.EntityPM.Tenant == 0 && !this.EntityPM.IsDistributor) {
                isEditingEnabled = false;
            }
        }
        else {
            if (SessionLocator_1.SessionLocator.Tenant != 0) {
                if (this.EntityPM.Tenant == 0 && !this.EntityPM.IsDistributor) {
                    isEditingEnabled = false;
                }
            }
        }
        this.IsEditingEnabled = isEditingEnabled;
        if (SessionLocator_1.SessionLocator.Tenant == 0) {
            this.IsExportButtonVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "User.Feature.CustomRoles")) {
            this.IsNewRoleButtonVisible = true;
        }
    };
    UserRolesTabComponent.prototype.ExportFeaturesToFileButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Initializing...");
        this.excelExportService.ExportRoleFeaturesToCSVFile().subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                var myResult = myResponse.Result;
                if (myResult) {
                    var defaultname = "Role features" + "_" + new Date().toLocaleDateString();
                    var data = base64ToArrayBuffer(myResult);
                    saveByteArray("defaultname", data, ".csv");
                    // this.ShowMessage("Export completed successfully");
                    //                    SaveFileDialog dlg = new SaveFileDialog();
                    //                    dlg.Filter = "Microsoft Excel (*.csv)|*.csv";
                    //                    dlg.DefaultExt = "csv";
                    //                    string defaultname = "Role features" + "_" + DateTime.Now.ToShortDateString();
                    //                    dlg.DefaultFileName = defaultname.Replace("/", "-");
                    //                    if ((bool)dlg.ShowDialog())
                    //        {
                    //            Stream fs = (Stream)dlg.OpenFile();
                    //            fs.Write(fileBytes, 0, fileBytes.Length);
                    //            fs.Close();
                    //        }
                    //                else
                    //                {
                    //    return;
                }
            }
        });
    };
    UserRolesTabComponent.prototype.ImportFeaturesToFileuttonClicked = function () {
        document.getElementById(this.ImportFeaturesFileHtmlId).click();
    };
    UserRolesTabComponent.prototype.ImportFeaturesFile = function (event) {
        var file = UploadLogoFile(this.ImportFeaturesFileHtmlId);
        //   && (file.type == "image/png" || file.type == "image/Png")
        if (file && file.name && file.name.toLowerCase().indexOf("csv") != -1) {
            this.ArrayBufferToBase64(file, this);
        }
    };
    UserRolesTabComponent.prototype.ImportFeatures = function (data) {
        var _this = this;
        var service = new ExcelExportService_1.ExcelExportService();
        var file = new ImageParameter_1.ImageParameter();
        file.Base64String = data;
        service.ImportRoleFeatures(file).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var wind = new MessageWindow_1.MessageWindow();
            wind.Show("Import completed successfully");
        });
    };
    UserRolesTabComponent.prototype.ArrayBufferToBase64 = function (file, viewmode) {
        if (file) {
            var reader = new FileReader();
            var reader = new FileReader();
            reader.onload = function (e) {
                var binary = '';
                var result = ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                viewmode.ImportFeatures(window.btoa(binary));
            };
            reader.onerror = function (e) {
                SessionLocator_1.SessionLocator.SelectedSession.StopBusyIndicator();
                var wind = new MessageWindow_1.MessageWindow();
                wind.Show("Error Importing file");
            };
            reader.readAsArrayBuffer(file);
        }
    };
    UserRolesTabComponent.prototype.LoadUserRoles = function (StartBusyIndicator) {
        var _this = this;
        if (StartBusyIndicator === void 0) { StartBusyIndicator = true; }
        if (StartBusyIndicator) {
            this.CurrentSession.StartBusyIndicatorLoading();
        }
        this.roleExtendedPMService.GetRolesForUser(this.EntityPM.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
            _this.ObsList = [];
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                var allRoles = myResponse.Result;
                if (allRoles) {
                    allRoles.filter(function (f) { return f.Exists == true; }).forEach(function (item) {
                        _this.AddRoleItem(item);
                    });
                    allRoles.filter(function (f) { return f.Exists == false; }).forEach(function (item) {
                        _this.AddRoleItem(item);
                    });
                }
            }
        });
    };
    UserRolesTabComponent.prototype.AddRoleItem = function (item) {
        switch (item.Code) {
            case "DIST":
            case "CUCA":
                {
                    if (SessionLocator_1.SessionLocator.Tenant == 0) {
                        this.ObsList.push(new UserRolesItemClass(item, this.EntityPM, this));
                    }
                    break;
                }
            default:
                {
                    if (item.IsCustomRole) {
                        if (item.Tenant == SessionLocator_1.SessionLocator.Tenant) {
                            this.ObsList.push(new UserRolesItemClass(item, this.EntityPM, this));
                        }
                    }
                    else {
                        this.ObsList.push(new UserRolesItemClass(item, this.EntityPM, this));
                    }
                    break;
                }
        }
    };
    UserRolesTabComponent.prototype.NewRoleButtonClicked = function () {
        var _this = this;
        var myCustomRolePM = new RolePM_1.RolePM();
        myCustomRolePM.Added = true;
        myCustomRolePM.Exists = true;
        myCustomRolePM.Removed = false;
        myCustomRolePM.UserId = this.EntityPM.Id;
        myCustomRolePM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        myCustomRolePM.IsCustomRole = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New Custom Role";
        logWindow.WindowArgs = { RolePM: myCustomRolePM, IsNew: true };
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/NewRoleComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.LoadUserRoles(false);
                    _this.EditRole(comp.EntityPM);
                }
            });
        });
    };
    UserRolesTabComponent.prototype.EditRolePropertiesClicked = function (item) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Edit Custom Role";
        logWindow.WindowArgs = { RolePM: item.EntityPM, IsNew: false };
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/NewRoleComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.LoadUserRoles(false);
                }
            });
        });
    };
    UserRolesTabComponent.prototype.EditRoleButtonClicked = function (item) {
        this.editedRole = item.EntityPM;
        this.isEditingRoleRequested = true;
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    };
    UserRolesTabComponent.prototype.EditRole = function (myRole) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.Title = "Edit " + myRole.Name + " Role";
        logWindow.WindowArgs = { RolePM: myRole };
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/EditRoleFeaturesComponent');
    };
    UserRolesTabComponent.prototype.UpdateRoles = function () {
        var _this = this;
        this.EntityPM.UserRolesNamesList = "";
        this.ObsList.filter(function (a) { return a.IsActive; }).forEach(function (item) {
            _this.EntityPM.UserRolesNamesList += item.Name + ",";
        });
    };
    UserRolesTabComponent.prototype.test = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.UserRolesNamesList_db)) {
            this.EntityPM.UserRolesNamesList_db = "";
            this.ObsList.filter(function (a) { return a.IsActive; }).forEach(function (item) {
                _this.EntityPM.UserRolesNamesList_db += item.Name + ",";
            });
        }
    };
    UserRolesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './UserRolesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], UserRolesTabComponent);
    return UserRolesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.UserRolesTabComponent = UserRolesTabComponent;
var UserRolesItemClass = /** @class */ (function () {
    function UserRolesItemClass(entityPM, myUserPM, father) {
        this.myUserPM = myUserPM;
        this.father = father;
        this.isNewUserMode = false;
        this.isActive = false;
        this.EntityPM = entityPM;
        this.UserPM = myUserPM;
        this.isActive = entityPM.Exists;
        if (myUserPM == null) {
            this.isNewUserMode = true;
        }
        else if (myUserPM.Id == null) {
            this.isNewUserMode = true;
        }
    }
    Object.defineProperty(UserRolesItemClass.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserRolesItemClass.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserRolesItemClass.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserRolesItemClass.prototype, "Id", {
        get: function () { return this.EntityPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserRolesItemClass.prototype, "IsCustomRole", {
        get: function () { return this.EntityPM.IsCustomRole; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserRolesItemClass.prototype, "IsActive", {
        get: function () { return this.isActive; },
        set: function (value) {
            var _this = this;
            var isUserDirty = this.UserPM.IsDirty;
            if (!this.isNewUserMode) {
                this.father.test();
            }
            if (this.isActive != value) {
                this.isActive = value;
                this.UserPM.IsDirty = true;
                this.EntityPM.UserId = this.UserPM.Id;
                if (!this.UserPM.RolePMLists) {
                    this.UserPM.RolePMLists = [];
                }
                var index = this.UserPM.RolePMLists.indexOf(this.EntityPM);
                if (index > -1) {
                    this.UserPM.RolePMLists.splice(index, 1);
                }
                if (this.isNewUserMode) {
                    this.EntityPM.UserId = null;
                }
                if (value) {
                    if (this.EntityPM.IsCustomRole) {
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.Show("The user who will be assigned this role will need to logout and login so the changes will take place");
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                                _this.EntityPM.Exists = true;
                                _this.EntityPM.Added = true;
                                _this.EntityPM.Removed = false;
                                if (_this.isNewUserMode) {
                                    var userRole = new UserRolesPM_1.UserRolesPM(null);
                                    userRole.Added = _this.EntityPM.Added;
                                    userRole.Exists = _this.EntityPM.Exists;
                                    userRole.Id = _this.EntityPM.Id;
                                    userRole.Name = _this.EntityPM.Name;
                                    userRole.Removed = _this.EntityPM.Removed;
                                    userRole.Tenant = _this.EntityPM.Tenant;
                                    _this.UserPM.AddUserRolesPM(userRole);
                                }
                            }
                            else {
                                _this.isActive = false;
                                if (!isUserDirty) {
                                    _this.UserPM.IsDirty = false;
                                }
                            }
                        });
                    }
                    else {
                        this.EntityPM.Exists = true;
                        this.EntityPM.Added = true;
                        this.EntityPM.Removed = false;
                        if (this.isNewUserMode) {
                            var userRole = new UserRolesPM_1.UserRolesPM(null);
                            userRole.Added = this.EntityPM.Added;
                            userRole.Exists = this.EntityPM.Exists;
                            userRole.Id = this.EntityPM.Id;
                            userRole.Name = this.EntityPM.Name;
                            userRole.Removed = this.EntityPM.Removed;
                            userRole.Tenant = this.EntityPM.Tenant;
                            this.UserPM.AddUserRolesPM(userRole);
                        }
                    }
                }
                else {
                    this.EntityPM.Exists = false;
                    this.EntityPM.Added = false;
                    this.EntityPM.Removed = true;
                    if (this.isNewUserMode) {
                        var userRole = this.UserPM.Roles.filter(function (r) { return r.Id == _this.EntityPM.Id; })[0];
                        if (userRole) {
                            this.UserPM.RemoveUserRolesPM(userRole);
                        }
                    }
                }
                this.UserPM.RolePMLists.push(this.EntityPM);
                if (!this.isNewUserMode) {
                    this.father.UpdateRoles();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return UserRolesItemClass;
}());
exports.UserRolesItemClass = UserRolesItemClass;
//# sourceMappingURL=UserRolesTabComponent.js.map