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
var PackagePMService_1 = require("../../../../Common/Services/StandardPMs/PackagePMService");
var InfrastructureDomainService_1 = require("../../../../Infrastructure/Services/InfrastructureDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var ExcelExportService_1 = require("../../../../Common/Services/Others/ExcelExportService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var ImageParameter_1 = require("../../../../Infrastructure/DataContracts/ImageParameter");
var UserPackagesComponent = /** @class */ (function () {
    function UserPackagesComponent() {
        this.ItemsSource = [];
        this.loadedDataList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.mySearchText = null;
        this.showInactive = false;
        this.filterTypeCode = "AL";
        this.ImportFeaturesFileHtmlId = Guid_1.Guid.NewRandomString();
        this.EntityPMService = new PackagePMService_1.PackagePMService();
        this.DomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
    }
    UserPackagesComponent.prototype.ngOnInit = function () {
        this.LoadData(true);
    };
    UserPackagesComponent.prototype.LoadData = function (startBusyIndicator) {
        var _this = this;
        if (startBusyIndicator) {
            this.CurrentSession.StartBusyIndicatorLoading();
        }
        this.DomainService.GetPackagesBMs().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.loadedDataList = myResponse.Result;
            }
            _this.BuildItemsSource();
            if (startBusyIndicator) {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    UserPackagesComponent.prototype.SearchTextChanged = function (text) {
        this.mySearchText = text;
        this.BuildItemsSource();
    };
    Object.defineProperty(UserPackagesComponent.prototype, "ShowInactive", {
        get: function () { return this.showInactive; },
        set: function (value) {
            if (this.showInactive != value) {
                this.showInactive = value;
                this.BuildItemsSource();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPackagesComponent.prototype, "FilterTypeCode", {
        get: function () { return this.filterTypeCode; },
        set: function (value) {
            if (this.filterTypeCode != value) {
                this.filterTypeCode = value;
                this.BuildItemsSource();
            }
        },
        enumerable: true,
        configurable: true
    });
    UserPackagesComponent.prototype.ExportFeaturesToCSVFile = function () {
        var service = new ExcelExportService_1.ExcelExportService();
        service.ExportFeaturesToCSVFile().subscribe(function (res) {
            if (!res.HasError) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Show("Export?");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        var defaultname = "package features" + "_" + new Date().toLocaleDateString();
                        var data = base64ToArrayBuffer(res.Result);
                        saveByteArray(defaultname, data, ".csv");
                    }
                });
            }
        });
    };
    UserPackagesComponent.prototype.ImportFeaturesToFile = function () {
        document.getElementById(this.ImportFeaturesFileHtmlId).click();
    };
    UserPackagesComponent.prototype.ImportFeaturesFile = function (event) {
        var file = UploadLogoFile(this.ImportFeaturesFileHtmlId);
        if (file && file.name && file.name.toLowerCase().indexOf("csv") != -1) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.ArrayBufferToBase64(file, this);
        }
    };
    UserPackagesComponent.prototype.ImportFeatures = function (data) {
        var _this = this;
        var service = new ExcelExportService_1.ExcelExportService();
        var file = new ImageParameter_1.ImageParameter();
        file.Base64String = data;
        service.ImportFeaturePackages(file).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var wind = new MessageWindow_1.MessageWindow();
            wind.Show("Import completed successfully");
        });
    };
    UserPackagesComponent.prototype.ArrayBufferToBase64 = function (file, viewmode) {
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
    UserPackagesComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        var items = this.loadedDataList;
        if (this.FilterTypeCode != "AL") {
            items = items.filter(function (f) { return f.FeaturePackageTypeCode == _this.FilterTypeCode; });
        }
        if (!this.ShowInactive) {
            items = items.filter(function (f) { return f.InActive == false; });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.mySearchText)) {
            items = items.filter(function (f) { return f.Name != null && f.Name.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1; });
        }
        items = items.sort(function (a, b) { return a.Name.toLowerCase() == b.Name.toLowerCase() ? 0 : a.Name.toLowerCase() < b.Name.toLowerCase() ? -1 : 1; });
        items.filter(function (f) { return f.FeaturePackageTypeCode == "AD"; }).forEach(function (item) {
            _this.ItemsSource.push(new UserPackageItemClass(item, _this));
        });
        items.filter(function (f) { return f.FeaturePackageTypeCode == "BS"; }).forEach(function (item) {
            _this.ItemsSource.push(new UserPackageItemClass(item, _this));
        });
        items.filter(function (f) { return f.FeaturePackageTypeCode == "PK"; }).forEach(function (item) {
            _this.ItemsSource.push(new UserPackageItemClass(item, _this));
        });
    };
    UserPackagesComponent.prototype.NewPackageClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Add New Package";
        logWindow.WindowArgs = { PackagePM: null };
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    if (comp.FeaturePackageTypeCode == "BS") {
                        _this.LoadData(false);
                        var item = new UserPackageItemClass(comp.EntityPM, _this);
                        _this.EditPackageClicked(item);
                    }
                    else {
                        _this.LoadData(true);
                    }
                }
            });
        });
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Packages/AddEditUserPackageComponent');
    };
    UserPackagesComponent.prototype.EditPackageClicked = function (item) {
        var _this = this;
        if (item.EntityPM.FeaturePackageTypeCode == "BS") {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.IsFillScreen = true;
            logWindow.Title = "Edit " + item.Name + " Package Features";
            logWindow.WindowArgs = { PackageCode: item.Code };
            logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Packages/EditPackageFeaturesComponent');
        }
        else {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Edit " + item.Name + " Package";
            logWindow.WindowArgs = { PackagePM: item.EntityPM };
            logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Packages/AddEditUserPackageComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.LoadData(true);
                }
            });
        }
    };
    UserPackagesComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    UserPackagesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './UserPackagesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], UserPackagesComponent);
    return UserPackagesComponent;
}());
exports.UserPackagesComponent = UserPackagesComponent;
var UserPackageItemClass = /** @class */ (function () {
    function UserPackageItemClass(item, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.EntityPM = item;
    }
    Object.defineProperty(UserPackageItemClass.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPackageItemClass.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPackageItemClass.prototype, "Type", {
        get: function () { return this.EntityPM.FeaturePackageTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UserPackageItemClass.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value) {
                this.EntityPM.InActive = value;
                this.fatherComponent.EntityPMService.update(this.EntityPM).subscribe(function (myResponse) {
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    return UserPackageItemClass;
}());
exports.UserPackageItemClass = UserPackageItemClass;
//# sourceMappingURL=UserPackagesComponent.js.map