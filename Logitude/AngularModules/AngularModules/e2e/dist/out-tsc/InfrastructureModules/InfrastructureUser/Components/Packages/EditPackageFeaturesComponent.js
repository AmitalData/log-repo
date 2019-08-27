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
var InfrastructureDomainService_1 = require("../../../../Infrastructure/Services/InfrastructureDomainService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EditPackageFeaturesComponent = /** @class */ (function () {
    function EditPackageFeaturesComponent() {
        this.ValidationErrorsList = [];
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];
        this.ItemsSource1Hidden = false;
        this.ItemsSource2Hidden = false;
        this.MenusList = [];
        this.OthersList = [];
        this.SettingsList = [];
        this.IsEventsButtonVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsEditingEnabled = false;
        this.mySearchText = null;
        this.allFeatures = [];
        this.allFeaturesItems = [];
        this.myDomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "FeaturesChanges")) {
            this.IsEventsButtonVisible = true;
        }
    }
    EditPackageFeaturesComponent.prototype.SetWindowArgs = function (args) {
        this.PackageCode = args['PackageCode'];
        this.SetUIProperties();
        this.LoadFeatures();
    };
    EditPackageFeaturesComponent.prototype.SetUIProperties = function () {
        if (SessionLocator_1.SessionLocator.Tenant == 0) {
            this.IsEditingEnabled = true;
        }
    };
    EditPackageFeaturesComponent.prototype.SearchTextChanged = function (text) {
        this.mySearchText = text;
        this.BuildCollections();
    };
    EditPackageFeaturesComponent.prototype.LoadFeatures = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myDomainService.GetSelectedAndUnselectedPackageFeatures(this.PackageCode).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.allFeatures = myResponse.Result;
                _this.allFeatures.forEach(function (itemFeature) {
                    _this.allFeaturesItems.push(new PackageFeatureClass(itemFeature, _this));
                });
            }
            _this.BuildCollections();
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    EditPackageFeaturesComponent.prototype.BuildCollections = function () {
        this.BuildTablesLists();
        this.BuildMenusList();
        this.BuildOthersList();
        this.BuildSettingsList();
    };
    EditPackageFeaturesComponent.prototype.BuildTablesLists = function () {
        var _this = this;
        var items = window.ObjectTables.filter(function (d) { return d.IsMain == true && d.IsClosed == false && d.IsComposition == false; });
        if (!Tools_1.AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(function (f) { return f.Name != null && f.Name.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1; });
        }
        items = items.sort(function (a, b) { return a.Name.toLowerCase() == b.Name.toLowerCase() ? 0 : a.Name.toLowerCase() < b.Name.toLowerCase() ? -1 : 1; });
        var allTablesItems = [];
        items.forEach(function (item) {
            if (allTablesItems.filter(function (f) { return f.ObjectTableId == item.Id; }).length == 0) {
                if (_this.allFeatures.filter(function (d) { return d.ObjectTableId == item.Id && d.FeatureTypeCode == "MODL"; }).length > 0) {
                    allTablesItems.push(new TablePackageFeatureClass(item, _this.allFeatures.filter(function (d) { return d.ObjectTableId == item.Id; }), _this));
                    ;
                }
            }
        });
        this.ItemsSource1 = allTablesItems.filter(function (f) { return f.ObjectTableTypeCode != "MD"; });
        this.ItemsSource2 = allTablesItems.filter(function (f) { return f.ObjectTableTypeCode == "MD"; });
    };
    EditPackageFeaturesComponent.prototype.BuildMenusList = function () {
        var _this = this;
        var items = [];
        items = this.allFeaturesItems.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "MENU"; });
        items = items.sort(function (a, b) { return a.Code.toLowerCase() == b.Code.toLowerCase() ? 0 : a.Code.toLowerCase() < b.Code.toLowerCase() ? -1 : 1; });
        if (!Tools_1.AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(function (f) { return f.Name != null && f.Name.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1; });
        }
        this.MenusList = items;
    };
    EditPackageFeaturesComponent.prototype.BuildOthersList = function () {
        var _this = this;
        var items = [];
        items = this.allFeaturesItems.filter(function (d) { return d.FeatureTypeCode == "OTH"; });
        items = items.sort(function (a, b) { return a.Code.toLowerCase() == b.Code.toLowerCase() ? 0 : a.Code.toLowerCase() < b.Code.toLowerCase() ? -1 : 1; });
        if (!Tools_1.AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(function (f) { return f.Name != null && f.Name.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1; });
        }
        this.OthersList = items;
    };
    EditPackageFeaturesComponent.prototype.BuildSettingsList = function () {
        var _this = this;
        var items = [];
        items = this.allFeaturesItems.filter(function (d) { return d.FeatureTypeCode == "SET"; });
        items = items.sort(function (a, b) { return a.Code.toLowerCase() == b.Code.toLowerCase() ? 0 : a.Code.toLowerCase() < b.Code.toLowerCase() ? -1 : 1; });
        if (!Tools_1.AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(function (f) { return f.Name != null && f.Name.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1; });
        }
        this.SettingsList = items;
    };
    EditPackageFeaturesComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EditPackageFeaturesComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var items = this.allFeatures.filter(function (f) { return f.IsDirty == true; });
        if (items.length == 0) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myServiceHelper = new InfrastructureDomainService_1.FeaturesUpdateHelper();
            myServiceHelper.Tenant = SessionLocator_1.SessionLocator.Tenant;
            myServiceHelper.PackageCode = this.PackageCode;
            myServiceHelper.Items = items;
            this.myDomainService.UpdateFeatures(myServiceHelper).subscribe(function (myResponse) {
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
        }
    };
    EditPackageFeaturesComponent.prototype.ShowEventsClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Package Events";
        logWindow.WindowArgs = this.PackageCode;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Packages/PackageFeaturesEventsComponent');
    };
    EditPackageFeaturesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditPackageFeaturesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditPackageFeaturesComponent);
    return EditPackageFeaturesComponent;
}());
exports.EditPackageFeaturesComponent = EditPackageFeaturesComponent;
var PackageFeatureClass = /** @class */ (function () {
    function PackageFeatureClass(entityPM, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.Code = null;
        this.Name = null;
        this.FeatureTypeCode = null;
        this.IsEditingEnabled = false;
        this.OldIsActive = false;
        this.isActive = false;
        this.Feature = entityPM;
        this.Code = entityPM.Code;
        this.Name = entityPM.TranslatedName;
        this.FeatureTypeCode = entityPM.FeatureTypeCode.toUpperCase();
        this.IsEditingEnabled = fatherComponent.IsEditingEnabled;
        this.isActive = entityPM.Exists;
        this.OldIsActive = entityPM.Exists;
    }
    Object.defineProperty(PackageFeatureClass.prototype, "IsActive", {
        get: function () { return this.isActive; },
        set: function (value) {
            if (this.isActive != value) {
                this.isActive = value;
                if (value == true) {
                    this.Feature.Exists = true;
                    this.Feature.IsAdded = true;
                    this.Feature.IsRemoved = false;
                    this.Feature.PackageCode = this.fatherComponent.PackageCode;
                }
                else {
                    this.Feature.Exists = false;
                    this.Feature.IsAdded = false;
                    this.Feature.IsRemoved = true;
                    this.Feature.PackageCode = this.fatherComponent.PackageCode;
                }
                if (value == this.OldIsActive) {
                    this.Feature.IsDirty = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return PackageFeatureClass;
}());
exports.PackageFeatureClass = PackageFeatureClass;
var TablePackageFeatureClass = /** @class */ (function () {
    function TablePackageFeatureClass(objectTablePM, myFeatures, fatherComponent) {
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
        this.OldIsActive_ModuleFeature = false;
        this.OldIsActive_ReadFeature = false;
        this.OldIsActive_UpdateFeature = false;
        this.OldIsActive_AddNewFeature = false;
        this.IsEditingEnabled = false;
        this.IsEditingEnabled_Read = false;
        this.IsEditingEnabled_Update = false;
        this.IsEditingEnabled_AddNew = false;
        this.IsModuleEnabled = false;
        this.isActive_ModuleFeature = false;
        this.isActive_ReadFeature = false;
        this.isActive_UpdateFeature = false;
        this.isActive_AddNewFeature = false;
        this.Name = TextCodeTranslator_1.TextCodeTranslator.Translate(objectTablePM.Name);
        this.ObjectTableId = objectTablePM.Id;
        this.ObjectTableTypeCode = objectTablePM.ObjectTableTypeCode;
        this.ObjectTableTypeName = this.ObjectTableTypeCode == "MD" ? "Master Data" : "Buisness Records";
        this.ModuleFeature = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "MODL"; })[0];
        this.ReadFeature = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "READ"; })[0];
        this.UpdateFeature = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "UPDT"; })[0];
        this.AddNewFeature = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "NEW"; })[0];
        this.AreasFeatures = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "AREA" && d.Packagable == true; });
        this.ActionsFeatures = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "ACT" && d.Packagable == true; });
        this.QueriesFeatures = myFeatures.filter(function (d) { return d.FeatureTypeCode.toUpperCase() == "QUER"; });
        if (this.ModuleFeature) {
            this.isActive_ModuleFeature = this.ModuleFeature.Exists;
            this.OldIsActive_ModuleFeature = this.ModuleFeature.Exists;
        }
        if (this.ReadFeature) {
            this.isActive_ReadFeature = this.ReadFeature.Exists;
            this.OldIsActive_ReadFeature = this.ReadFeature.Exists;
        }
        if (this.UpdateFeature) {
            this.isActive_UpdateFeature = this.UpdateFeature.Exists;
            this.OldIsActive_UpdateFeature = this.UpdateFeature.Exists;
        }
        if (this.AddNewFeature) {
            this.isActive_AddNewFeature = this.AddNewFeature.Exists;
            this.OldIsActive_AddNewFeature = this.AddNewFeature.Exists;
        }
        this.SetHyperlinks();
        this.SetUIProperties();
    }
    TablePackageFeatureClass.prototype.SetHyperlinks = function () {
        this.AreasLinkText = this.AreasFeatures.filter(function (d) { return d.Exists == true; }).length + "/" + this.AreasFeatures.length;
        this.ActionsLinkText = this.ActionsFeatures.filter(function (d) { return d.Exists == true; }).length + "/" + this.ActionsFeatures.length;
        this.QueriesLinkText = this.QueriesFeatures.filter(function (d) { return d.Exists == true; }).length + "/" + this.QueriesFeatures.length;
        this.AreasLinkColor = this.AreasFeatures.filter(function (d) { return d.Exists != true; }).length > 0 ? "#1E4AC4" : "#009161";
        this.ActionsLinkColor = this.ActionsFeatures.filter(function (d) { return d.Exists != true; }).length > 0 ? "#1E4AC4" : "#009161";
        this.QueriesLinkColor = this.QueriesFeatures.filter(function (d) { return d.Exists != true; }).length > 0 ? "#1E4AC4" : "#009161";
    };
    TablePackageFeatureClass.prototype.SetUIProperties = function () {
        var isEditingEnabled = true;
        var isEditingEnabled_Read = true;
        var isEditingEnabled_Update = true;
        var isEditingEnabled_AddNew = true;
        var isModuleEnabled = true;
        if (SessionLocator_1.SessionLocator.Tenant != 0) {
            isEditingEnabled = false;
            isEditingEnabled_Read = false;
            isEditingEnabled_Update = false;
            isEditingEnabled_AddNew = false;
        }
        if (this.ModuleFeature == null) {
            isEditingEnabled = false;
            isEditingEnabled_Update = false;
            isEditingEnabled_AddNew = false;
        }
        if (this.IsActive_ModuleFeature == false) {
            isEditingEnabled_Read = false;
            isEditingEnabled_Update = false;
            isEditingEnabled_AddNew = false;
            isModuleEnabled = false;
        }
        if (this.IsActive_ReadFeature == false) {
            isEditingEnabled_AddNew = false;
        }
        if (this.IsActive_AddNewFeature == true) {
            isEditingEnabled_Update = false;
        }
        this.IsEditingEnabled = isEditingEnabled;
        this.IsEditingEnabled_Read = isEditingEnabled_Read;
        this.IsEditingEnabled_Update = isEditingEnabled_Update;
        this.IsEditingEnabled_AddNew = isEditingEnabled_AddNew;
        this.IsModuleEnabled = isModuleEnabled;
    };
    Object.defineProperty(TablePackageFeatureClass.prototype, "IsActive_ModuleFeature", {
        get: function () { return this.isActive_ModuleFeature; },
        set: function (value) {
            if (this.isActive_ModuleFeature != value) {
                this.isActive_ModuleFeature = value;
                this.SetUIProperties();
                if (this.ModuleFeature) {
                    if (value == true) {
                        this.ModuleFeature.Exists = true;
                        this.ModuleFeature.IsAdded = true;
                        this.ModuleFeature.IsRemoved = false;
                        this.ModuleFeature.PackageCode = this.fatherComponent.PackageCode;
                    }
                    else {
                        this.ModuleFeature.Exists = false;
                        this.ModuleFeature.IsAdded = false;
                        this.ModuleFeature.IsRemoved = true;
                        this.ModuleFeature.PackageCode = this.fatherComponent.PackageCode;
                    }
                    if (value == this.OldIsActive_ModuleFeature) {
                        this.ModuleFeature.IsDirty = false;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TablePackageFeatureClass.prototype, "IsActive_ReadFeature", {
        get: function () { return this.isActive_ReadFeature; },
        set: function (value) {
            if (this.isActive_ReadFeature != value) {
                this.isActive_ReadFeature = value;
                this.SetUIProperties();
                if (this.ReadFeature) {
                    if (value == true) {
                        this.ReadFeature.Exists = true;
                        this.ReadFeature.IsAdded = true;
                        this.ReadFeature.IsRemoved = false;
                        this.ReadFeature.PackageCode = this.fatherComponent.PackageCode;
                    }
                    else {
                        this.ReadFeature.Exists = false;
                        this.ReadFeature.IsAdded = false;
                        this.ReadFeature.IsRemoved = true;
                        this.ReadFeature.PackageCode = this.fatherComponent.PackageCode;
                    }
                    if (value == this.OldIsActive_ReadFeature) {
                        this.ReadFeature.IsDirty = false;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TablePackageFeatureClass.prototype, "IsActive_UpdateFeature", {
        get: function () { return this.isActive_UpdateFeature; },
        set: function (value) {
            if (this.isActive_UpdateFeature != value) {
                this.isActive_UpdateFeature = value;
                if (this.UpdateFeature) {
                    if (value == true) {
                        this.UpdateFeature.Exists = true;
                        this.UpdateFeature.IsAdded = true;
                        this.UpdateFeature.IsRemoved = false;
                        this.UpdateFeature.PackageCode = this.fatherComponent.PackageCode;
                    }
                    else {
                        this.UpdateFeature.Exists = false;
                        this.UpdateFeature.IsAdded = false;
                        this.UpdateFeature.IsRemoved = true;
                        this.UpdateFeature.PackageCode = this.fatherComponent.PackageCode;
                    }
                    if (value == this.OldIsActive_UpdateFeature) {
                        this.UpdateFeature.IsDirty = false;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TablePackageFeatureClass.prototype, "IsActive_AddNewFeature", {
        get: function () { return this.isActive_AddNewFeature; },
        set: function (value) {
            if (this.isActive_AddNewFeature != value) {
                this.isActive_AddNewFeature = value;
                this.SetUIProperties();
                if (this.AddNewFeature) {
                    if (value == true) {
                        this.AddNewFeature.Exists = true;
                        this.AddNewFeature.IsAdded = true;
                        this.AddNewFeature.IsRemoved = false;
                        this.AddNewFeature.PackageCode = this.fatherComponent.PackageCode;
                    }
                    else {
                        this.AddNewFeature.Exists = false;
                        this.AddNewFeature.IsAdded = false;
                        this.AddNewFeature.IsRemoved = true;
                        this.AddNewFeature.PackageCode = this.fatherComponent.PackageCode;
                    }
                    if (value == this.OldIsActive_AddNewFeature) {
                        this.AddNewFeature.IsDirty = false;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    TablePackageFeatureClass.prototype.HyperlinkClicked = function (typeCode) {
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
            myFeaturesItems.push(new PackageFeatureClass(item, _this.fatherComponent));
        });
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Edit " + typeCode + " Features";
        logWindow.WindowArgs = { Items: myFeaturesItems };
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Packages/EditFeaturesPackageLinkComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.SetHyperlinks();
            }
        });
    };
    return TablePackageFeatureClass;
}());
exports.TablePackageFeatureClass = TablePackageFeatureClass;
//# sourceMappingURL=EditPackageFeaturesComponent.js.map