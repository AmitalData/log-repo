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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var InfrastructureDomainService_1 = require("../../../../Infrastructure/Services/InfrastructureDomainService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var EditRoleFeaturesComponent = /** @class */ (function () {
    function EditRoleFeaturesComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.DataContext = this;
        this.ObjectTableName = "Role";
        this.ValidationErrorsList = [];
        this.IsResourcesReady = false;
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];
        this.ItemsSource1Hidden = false;
        this.ItemsSource2Hidden = false;
        this.MenusList = [];
        this.OthersList = [];
        this.IsCustomRole = false;
        this.IsEventsButtonVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsEditingEnabled = false;
        this.IsOkButtonVisible = false;
        this.CancelButtonTextCode = "General.B.Close";
        this.IsShowNewFeaturesButtonVisible = false;
        this.mySearchText = null;
        this.allFeatures = [];
        this.allFeaturesItems = [];
        this.myDomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "FeaturesChanges")) {
            this.IsEventsButtonVisible = true;
        }
    }
    EditRoleFeaturesComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args['RolePM'];
        this.IsCustomRole = this.EntityPM.IsCustomRole;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.IsResourcesReady = true;
            _this.SetUIProperties();
            _this.LoadFeatures();
        });
    };
    EditRoleFeaturesComponent.prototype.SetUIProperties = function () {
        if (SessionLocator_1.SessionLocator.Tenant == 0) {
            this.IsEditingEnabled = true;
            this.IsShowNewFeaturesButtonVisible = true;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ParentRoleId)) {
            if (this.EntityPM.Tenant == SessionLocator_1.SessionLocator.Tenant) {
                this.IsEditingEnabled = true;
            }
        }
        if (SessionLocator_1.SessionLocator.Tenant == 0 || this.IsCustomRole) {
            this.IsOkButtonVisible = true;
            this.CancelButtonTextCode = "General.B.Cancel";
        }
    };
    EditRoleFeaturesComponent.prototype.SearchTextChanged = function (text) {
        this.mySearchText = text;
        this.BuildCollections();
    };
    EditRoleFeaturesComponent.prototype.LoadFeatures = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myDomainService.GetSelectedAndUnselectedRoleFeatures(this.EntityPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var loadedFeatures = myResponse.Result;
                if (SessionLocator_1.SessionLocator.Tenant == 0) {
                    _this.allFeatures = loadedFeatures;
                }
                else {
                    if (_this.IsCustomRole) {
                        loadedFeatures.forEach(function (item) {
                            if (FeatureLocator_1.FeatureLocator.HasFeaturePermessionByObjectTableId(item.ObjectTableId, item.Code)) {
                                _this.allFeatures.push(item);
                            }
                        });
                    }
                    else {
                        _this.allFeatures = loadedFeatures;
                    }
                }
                _this.allFeatures.forEach(function (itemFeature) {
                    _this.allFeaturesItems.push(new RoleFeatureClass(itemFeature, _this));
                });
            }
            _this.BuildCollections();
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    EditRoleFeaturesComponent.prototype.BuildCollections = function () {
        this.BuildTablesLists();
        this.BuildMenusList();
        this.BuildOthersList();
    };
    EditRoleFeaturesComponent.prototype.BuildTablesLists = function () {
        var _this = this;
        var items = window.ObjectTables.filter(function (d) { return d.IsMain == true && d.EnableSecurity == true && d.IsClosed == false && d.IsComposition == false; });
        if (!Tools_1.AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(function (f) { return f.Name != null && f.Name.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1; });
        }
        items = items.sort(function (a, b) { return a.Name.toLowerCase() == b.Name.toLowerCase() ? 0 : a.Name.toLowerCase() < b.Name.toLowerCase() ? -1 : 1; });
        var allTablesItems = [];
        items.forEach(function (item) {
            if (allTablesItems.filter(function (f) { return f.ObjectTableId == item.Id; }).length == 0) {
                if (_this.allFeatures.filter(function (d) { return d.ObjectTableId == item.Id && d.FeatureTypeCode == "MODL"; }).length > 0) {
                    allTablesItems.push(new TableRoleFeatureClass(item, _this.allFeatures.filter(function (d) { return d.ObjectTableId == item.Id; }), _this));
                }
            }
        });
        this.ItemsSource1 = allTablesItems.filter(function (f) { return f.ObjectTableTypeCode != "MD"; });
        this.ItemsSource2 = allTablesItems.filter(function (f) { return f.ObjectTableTypeCode == "MD"; });
    };
    EditRoleFeaturesComponent.prototype.BuildMenusList = function () {
        var _this = this;
        var items = [];
        items = this.allFeaturesItems.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "MENU"; });
        items = items.sort(function (a, b) { return a.Code.toLowerCase() == b.Code.toLowerCase() ? 0 : a.Code.toLowerCase() < b.Code.toLowerCase() ? -1 : 1; });
        if (!Tools_1.AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(function (f) { return f.Name != null && f.Name.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1; });
        }
        this.MenusList = items;
    };
    EditRoleFeaturesComponent.prototype.BuildOthersList = function () {
        var _this = this;
        var items = [];
        items = this.allFeaturesItems.filter(function (d) { return d.FeatureTypeCode == "OTH"; });
        items = items.sort(function (a, b) { return a.Code.toLowerCase() == b.Code.toLowerCase() ? 0 : a.Code.toLowerCase() < b.Code.toLowerCase() ? -1 : 1; });
        if (!Tools_1.AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(function (f) { return f.Name != null && f.Name.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1; });
        }
        this.OthersList = items;
    };
    EditRoleFeaturesComponent.prototype.ShowNewFeaturesClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New Features";
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/ShowNewFeaturesComponent');
    };
    EditRoleFeaturesComponent.prototype.ShowEventsClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Role Events";
        logWindow.WindowArgs = this.EntityPM.Id;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/RoleFeaturesEventsComponent');
    };
    EditRoleFeaturesComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EditRoleFeaturesComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var items = this.allFeatures.filter(function (f) { return f.IsDirty == true; });
        if (items.length == 0) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            if (this.IsCustomRole) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Show("The user who will be assigned this role will need to logout and login so the changes will take place");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.Save(items);
                    }
                });
            }
            else {
                this.Save(items);
            }
        }
    };
    EditRoleFeaturesComponent.prototype.Save = function (items) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var myServiceHelper = new InfrastructureDomainService_1.FeaturesUpdateHelper();
        myServiceHelper.Tenant = SessionLocator_1.SessionLocator.Tenant;
        myServiceHelper.RoleId = this.EntityPM.Id;
        myServiceHelper.Items = items;
        this.myDomainService.UpdateFeatures(myServiceHelper).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.myDomainService.GetAllowedFeaturesForLoggedUser().subscribe(function (myResponse1) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (myResponse1.HasError) {
                        _this.ValidationErrorsList = myResponse1.ErrorsArray;
                    }
                    else {
                        _this.CurrentSession.CloseCurrentWindowEmit("Ok");
                    }
                });
            }
        });
    };
    EditRoleFeaturesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditRoleFeaturesComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], EditRoleFeaturesComponent);
    return EditRoleFeaturesComponent;
}());
exports.EditRoleFeaturesComponent = EditRoleFeaturesComponent;
var RoleFeatureClass = /** @class */ (function () {
    function RoleFeatureClass(entityPM, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.Code = null;
        this.Name = null;
        this.FeatureTypeCode = null;
        this.IsEditingEnabled = false;
        this.IsCustomRoleFeature = false;
        this.OldAccessLevelCode = "NO";
        this.Feature = entityPM;
        this.Code = entityPM.Code;
        this.Name = entityPM.TranslatedName;
        this.FeatureTypeCode = entityPM.FeatureTypeCode.toUpperCase();
        this.IsEditingEnabled = fatherComponent.IsEditingEnabled;
        this.IsCustomRoleFeature = entityPM.IsCustomRoleFeature;
        if (this.Feature.AccessLevelCode) {
            this.OldAccessLevelCode = this.Feature.AccessLevelCode;
        }
    }
    Object.defineProperty(RoleFeatureClass.prototype, "AccessLevelCode", {
        get: function () { return this.Feature.AccessLevelCode; },
        set: function (value) {
            if (this.Feature.AccessLevelCode != value) {
                this.Feature.AccessLevelCode = value;
                this.Feature.RoleId = this.fatherComponent.EntityPM.Id;
                if (value == this.OldAccessLevelCode) {
                    this.Feature.IsDirty = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return RoleFeatureClass;
}());
exports.RoleFeatureClass = RoleFeatureClass;
var TableRoleFeatureClass = /** @class */ (function () {
    function TableRoleFeatureClass(objectTablePM, myFeatures, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.Name = null;
        this.ObjectTableId = null;
        this.ObjectTableTypeCode = null;
        this.ObjectTableTypeName = null;
        this.ModuleFeature = null;
        this.ReadFeature = null;
        this.UpdateFeature = null;
        this.AddNewFeature = null;
        this.AreasFeatures = [];
        this.ActionsFeatures = [];
        this.QueriesFeatures = [];
        this.IsCustomRoleFeature_Read = false;
        this.IsCustomRoleFeature_Update = false;
        this.IsCustomRoleFeature_AddNew = false;
        this.OldModuleFeatureAccessLevelCode = "NO";
        this.OldReadFeatureAccessLevelCode = "NO";
        this.OldUpdateFeatureAccessLevelCode = "NO";
        this.OldAddNewFeatureAccessLevelCode = "NO";
        this.IsEditingEnabled = false;
        this.IsUpdateFeatureEnabled = false;
        this.IsAddNewFeatureEnabled = false;
        this.Name = TextCodeTranslator_1.TextCodeTranslator.Translate(objectTablePM.Name);
        this.ObjectTableId = objectTablePM.Id;
        this.ObjectTableTypeCode = objectTablePM.ObjectTableTypeCode;
        this.ObjectTableTypeName = this.ObjectTableTypeCode == "MD" ? "Master Data" : "Buisness Records";
        this.ModuleFeature = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "MODL"; })[0];
        this.ReadFeature = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "READ"; })[0];
        this.UpdateFeature = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "UPDT"; })[0];
        this.AddNewFeature = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "NEW"; })[0];
        this.AreasFeatures = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "AREA"; });
        this.ActionsFeatures = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "ACT"; });
        this.QueriesFeatures = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "QUER"; });
        if (this.ModuleFeature) {
            if (this.ModuleFeature.AccessLevelCode) {
                this.OldModuleFeatureAccessLevelCode = this.ModuleFeature.AccessLevelCode;
            }
        }
        if (this.ReadFeature) {
            this.IsCustomRoleFeature_Read = this.ReadFeature.IsCustomRoleFeature;
            if (this.ReadFeature.AccessLevelCode) {
                this.OldReadFeatureAccessLevelCode = this.ReadFeature.AccessLevelCode;
            }
        }
        if (this.UpdateFeature) {
            this.IsCustomRoleFeature_Update = this.UpdateFeature.IsCustomRoleFeature;
            if (this.UpdateFeature.AccessLevelCode) {
                this.OldUpdateFeatureAccessLevelCode = this.UpdateFeature.AccessLevelCode;
            }
        }
        if (this.AddNewFeature) {
            this.IsCustomRoleFeature_AddNew = this.AddNewFeature.IsCustomRoleFeature;
            if (this.AddNewFeature.AccessLevelCode) {
                this.OldAddNewFeatureAccessLevelCode = this.AddNewFeature.AccessLevelCode;
            }
        }
        this.SetHyperlinks();
        this.SetUIProperties();
    }
    TableRoleFeatureClass.prototype.SetHyperlinks = function () {
        this.AreasLinkText = this.AreasFeatures.filter(function (d) { return d.AccessLevelCode == "OR"; }).length + "/" + this.AreasFeatures.length;
        this.ActionsLinkText = this.ActionsFeatures.filter(function (d) { return d.AccessLevelCode == "OR"; }).length + "/" + this.ActionsFeatures.length;
        this.QueriesLinkText = this.QueriesFeatures.filter(function (d) { return d.AccessLevelCode == "OR"; }).length + "/" + this.QueriesFeatures.length;
        this.AreasLinkColor = this.AreasFeatures.filter(function (d) { return d.AccessLevelCode != "OR"; }).length > 0 ? "#1E4AC4" : "#009161";
        this.ActionsLinkColor = this.ActionsFeatures.filter(function (d) { return d.AccessLevelCode != "OR"; }).length > 0 ? "#1E4AC4" : "#009161";
        this.QueriesLinkColor = this.QueriesFeatures.filter(function (d) { return d.AccessLevelCode != "OR"; }).length > 0 ? "#1E4AC4" : "#009161";
    };
    TableRoleFeatureClass.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        var isUpdateFeatureEnabled = true;
        var isAddNewFeatureEnabled = true;
        if (!this.IsEditingEnabled) {
            isUpdateFeatureEnabled = false;
            isAddNewFeatureEnabled = false;
        }
        else {
            if (this.ReadFeature == null) {
                isUpdateFeatureEnabled = false;
                isAddNewFeatureEnabled = false;
            }
            else if (this.ReadFeature.AccessLevelCode == "NO" || Tools_1.AppTool.IsNullOrEmpty(this.ReadFeature.AccessLevelCode)) {
                isUpdateFeatureEnabled = false;
                isAddNewFeatureEnabled = false;
            }
            else {
                if (this.AddNewFeature == null) {
                    isUpdateFeatureEnabled = false;
                }
                else if (this.AddNewFeature.AccessLevelCode != "NO" && !Tools_1.AppTool.IsNullOrEmpty(this.AddNewFeature.AccessLevelCode)) {
                    isUpdateFeatureEnabled = false;
                }
            }
        }
        this.IsUpdateFeatureEnabled = isUpdateFeatureEnabled;
        this.IsAddNewFeatureEnabled = isAddNewFeatureEnabled;
    };
    Object.defineProperty(TableRoleFeatureClass.prototype, "AccessLevelCode_Read", {
        // Access Level
        get: function () { return this.ReadFeature == null ? null : this.ReadFeature.AccessLevelCode; },
        set: function (value) {
            if (this.ReadFeature) {
                if (this.ReadFeature.AccessLevelCode != value) {
                    this.ReadFeature.AccessLevelCode = value;
                    this.ReadFeature.RoleId = this.fatherComponent.EntityPM.Id;
                    this.SetUIProperties();
                    if (value == this.OldReadFeatureAccessLevelCode) {
                        this.ReadFeature.IsDirty = false;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TableRoleFeatureClass.prototype, "AccessLevelCode_Update", {
        get: function () { return this.UpdateFeature == null ? null : this.UpdateFeature.AccessLevelCode; },
        set: function (value) {
            if (this.UpdateFeature) {
                if (this.UpdateFeature.AccessLevelCode != value) {
                    this.UpdateFeature.AccessLevelCode = value;
                    this.UpdateFeature.RoleId = this.fatherComponent.EntityPM.Id;
                    if (value == this.OldUpdateFeatureAccessLevelCode) {
                        this.UpdateFeature.IsDirty = false;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TableRoleFeatureClass.prototype, "AccessLevelCode_AddNew", {
        get: function () { return this.AddNewFeature == null ? null : this.AddNewFeature.AccessLevelCode; },
        set: function (value) {
            if (this.AddNewFeature) {
                if (this.AddNewFeature.AccessLevelCode != value) {
                    this.AddNewFeature.AccessLevelCode = value;
                    this.AddNewFeature.RoleId = this.fatherComponent.EntityPM.Id;
                    if (value != "NO" && !Tools_1.AppTool.IsNullOrEmpty(value)) {
                        this.AccessLevelCode_Update = value;
                    }
                    this.SetUIProperties();
                    if (value == this.OldAddNewFeatureAccessLevelCode) {
                        this.AddNewFeature.IsDirty = false;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    TableRoleFeatureClass.prototype.HyperlinkClicked = function (typeCode) {
        var _this = this;
        var myFeatures = [];
        var myFeaturesItems = [];
        switch (typeCode) {
            case "Areas": {
                myFeatures = this.AreasFeatures;
                break;
            }
            case "Actions": {
                myFeatures = this.ActionsFeatures;
                break;
            }
            case "Queries": {
                myFeatures = this.QueriesFeatures;
                break;
            }
        }
        myFeatures.forEach(function (item) {
            myFeaturesItems.push(new RoleFeatureClass(item, _this.fatherComponent));
        });
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Edit " + typeCode + " Features";
        logWindow.WindowArgs = { Items: myFeaturesItems };
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/EditFeaturesRoleLinkComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.SetHyperlinks();
            }
        });
    };
    return TableRoleFeatureClass;
}());
exports.TableRoleFeatureClass = TableRoleFeatureClass;
//# sourceMappingURL=EditRoleFeaturesComponent.js.map