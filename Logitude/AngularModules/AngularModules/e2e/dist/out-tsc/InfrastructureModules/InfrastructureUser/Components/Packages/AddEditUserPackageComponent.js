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
var Tools_1 = require("../../../../Infrastructure/Tools");
var PackagePM_1 = require("../../../../Common/EntityPMs/PackagePM");
var PackageConnectedPackagePM_1 = require("../../../../Common/EntityPMs/PackageConnectedPackagePM");
var PackagePMService_1 = require("../../../../Common/Services/StandardPMs/PackagePMService");
var PackageListService_1 = require("../../../../Common/Services/StandardLists/PackageListService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var AddEditUserPackageComponent = /** @class */ (function (_super) {
    __extends(AddEditUserPackageComponent, _super);
    function AddEditUserPackageComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Package";
        _this.ValidationErrorsList = [];
        _this.ItemsSource = [];
        _this.IsResourcesReady = false;
        _this.IsNewMode = false;
        _this.IsEditMode = false;
        _this.allExistingPackagesCodes = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsGridViewVisible = false;
        _this.AllCloners = [];
        return _this;
    }
    AddEditUserPackageComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args['PackagePM'];
        if (!this.EntityPM) {
            this.IsNewMode = true;
            this.EntityPM = new PackagePM_1.PackagePM();
        }
        else {
            this.IsEditMode = true;
        }
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.IsResourcesReady = true;
            _this.SetUIProperties();
            _this.BuildItemsSource();
            if (_this.IsEditMode) {
                _this.Clone();
            }
        });
    };
    AddEditUserPackageComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, this.IsNewMode);
        this.UIProperties.SetEnabled("FeaturePackageTypeCode", this.ObjectTableName, this.IsNewMode);
        this.IsGridViewVisible = this.FeaturePackageTypeCode == "PK" || this.FeaturePackageTypeCode == "AD" ? true : false;
    };
    Object.defineProperty(AddEditUserPackageComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (value) {
            if (this.EntityPM.Code != value) {
                this.EntityPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditUserPackageComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value) {
                this.EntityPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditUserPackageComponent.prototype, "FeaturePackageTypeCode", {
        get: function () { return this.EntityPM.FeaturePackageTypeCode; },
        set: function (value) {
            if (this.EntityPM.FeaturePackageTypeCode != value) {
                this.EntityPM.FeaturePackageTypeCode = value;
                this.IsGridViewVisible = this.FeaturePackageTypeCode == "PK" || this.FeaturePackageTypeCode == "AD" ? true : false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditUserPackageComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value) {
                this.EntityPM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditUserPackageComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.EntityPM.ConnectedPackages.forEach(function (itemConnected) {
            _this.ItemsSource.push(new ConnectedPackageItemClass(itemConnected, true));
        });
        var myService = new PackageListService_1.PackageListService();
        myService.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var allPackages = myResponse.Result;
                allPackages = allPackages.sort(function (a, b) { return a.Name.toLowerCase() == b.Name.toLowerCase() ? 0 : a.Name.toLowerCase() < b.Name.toLowerCase() ? -1 : 1; });
                allPackages.forEach(function (item) {
                    _this.allExistingPackagesCodes.push(item.Code.toLowerCase());
                    if (item.FeaturePackageTypeCode == "BS" && item.InActive == false) {
                        if (_this.EntityPM.ConnectedPackages.filter(function (f) { return f.ConnectedPackageCode == item.Code; }).length == 0) {
                            var itemPM = new PackageConnectedPackagePM_1.PackageConnectedPackagePM(null);
                            itemPM.PackageCode = _this.Code;
                            itemPM.ConnectedPackageCode = item.Code;
                            itemPM.ConnectedPackageName = item.Name;
                            _this.ItemsSource.push(new ConnectedPackageItemClass(itemPM, false));
                        }
                    }
                });
            }
        });
    };
    AddEditUserPackageComponent.prototype.CancelButtonClicked = function () {
        if (this.IsEditMode) {
            this.RejectChanges();
        }
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditUserPackageComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.IsNewMode) {
            if (this.Code != null) {
                var isExists = Tools_1.ArrayTool.Contains(this.allExistingPackagesCodes, this.Code.toLowerCase());
                if (isExists) {
                    errors.push("Package with same code already exists");
                }
            }
        }
        if (this.FeaturePackageTypeCode != null) {
            if (this.FeaturePackageTypeCode.toUpperCase() == "PK" || this.FeaturePackageTypeCode.toUpperCase() == "AD") {
                var allCheckedItems = this.ItemsSource.filter(function (d) { return d.IsChecked; });
                if (allCheckedItems.length == 0) {
                    errors.push("You must select 1 package at least");
                }
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            if (this.FeaturePackageTypeCode.toUpperCase() == "BS") {
                this.EntityPM.ConnectedPackages = [];
            }
            else {
                this.ItemsSource.forEach(function (item) {
                    if (item.EntityPM.PackageCode != _this.EntityPM.Code) {
                        item.EntityPM.PackageCode = _this.EntityPM.Code;
                    }
                    if (item.IsChecked) {
                        _this.EntityPM.AddPackageConnectedPackagePM(item.EntityPM);
                    }
                    else {
                        _this.EntityPM.RemovePackageConnectedPackagePM(item.EntityPM);
                    }
                });
            }
            var myService = new PackagePMService_1.PackagePMService();
            if (this.IsNewMode) {
                myService.insert(this.EntityPM).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                });
            }
            else {
                myService.update(this.EntityPM).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                });
            }
        }
    };
    AddEditUserPackageComponent.prototype.Clone = function () {
        var _this = this;
        this.myCloner = new Cloner_1.Cloner(this);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('InActive');
        this.myCloner.AddField('FeaturePackageTypeCode');
        this.myCloner.AddEntity(this.EntityPM);
        this.ItemsSource.forEach(function (item) {
            var itemCloner = new Cloner_1.Cloner(item);
            itemCloner.AddField('IsChecked');
            itemCloner.AddEntity(item.EntityPM);
            _this.AllCloners.push(itemCloner);
        });
    };
    AddEditUserPackageComponent.prototype.RejectChanges = function () {
        this.AllCloners.forEach(function (myCloner) {
            myCloner.RejectChanges();
        });
        this.myCloner.RejectChanges();
    };
    AddEditUserPackageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditUserPackageComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], AddEditUserPackageComponent);
    return AddEditUserPackageComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditUserPackageComponent = AddEditUserPackageComponent;
var ConnectedPackageItemClass = /** @class */ (function () {
    function ConnectedPackageItemClass(item, isConnected) {
        this.isChecked = false;
        this.EntityPM = item;
        this.isChecked = isConnected;
    }
    Object.defineProperty(ConnectedPackageItemClass.prototype, "Name", {
        get: function () { return this.EntityPM.ConnectedPackageName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConnectedPackageItemClass.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            if (this.isChecked != value) {
                this.isChecked = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return ConnectedPackageItemClass;
}());
exports.ConnectedPackageItemClass = ConnectedPackageItemClass;
//# sourceMappingURL=AddEditUserPackageComponent.js.map