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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var AWBOCIPM_1 = require("../../../../../Shipment/EntityPMs/AWBOCIPM");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var Tools_1 = require("../../../../../Shipment/Tools");
var Tools_2 = require("../../../../../Infrastructure/Tools");
var OCITabComponent = /** @class */ (function (_super) {
    __extends(OCITabComponent, _super);
    function OCITabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsEditingEnabled = false;
        return _this;
    }
    OCITabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.BuildData();
        this.SetUIProperties();
    };
    OCITabComponent.prototype.RefreshTab = function () {
    };
    OCITabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.BuildData();
                    _this.SetUIProperties();
                }
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.BuildData();
                    _this.SetUIProperties();
                }
            });
        }
    };
    OCITabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_1.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.ItemsSource.forEach(function (item) {
            item.SetUIProperties();
        });
    };
    OCITabComponent.prototype.BuildData = function () {
        var _this = this;
        if (this.ItemsSource == null) {
            this.ItemsSource = new Array();
        }
        else {
            this.ItemsSource = [];
        }
        var list = new Array();
        this.EntityPM.AWBOCIPMs.forEach(function (item) {
            list.push(item);
        });
        if (list.length < 5) {
            for (var i = list.length; i < 5; i++) {
                var item = new AWBOCIPM_1.AWBOCIPM(null);
                item.Tenant = this.EntityPM.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                item.IsAWBWizardDefault = true;
                list.push(item);
            }
        }
        list.sort(function (a, b) { return (a === b) ? 0 : a ? -1 : 1; }).forEach(function (item) {
            _this.ItemsSource.push(new AWBWizardOCIItem(item, false, _this));
        });
    };
    OCITabComponent.prototype.Add = function () {
        var itemPM = new AWBOCIPM_1.AWBOCIPM(null);
        itemPM.ShipmentId = this.EntityPM.Id;
        itemPM.Tenant = this.EntityPM.Tenant;
        var itemViewModel = new AWBWizardOCIItem(itemPM, true, this);
        this.RunWindow(itemViewModel, "Add OCI line");
    };
    OCITabComponent.prototype.Edit = function (itemViewModel) {
        this.RunWindow(itemViewModel, "Edit OCI line");
    };
    OCITabComponent.prototype.Delete = function (itemViewModel) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show('Delete this line ?');
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var itemIndex = _this.ItemsSource.indexOf(itemViewModel);
                if (itemIndex > -1) {
                    _this.ItemsSource.splice(itemIndex, 1);
                }
                _this.EntityPM.RemoveOCI(itemViewModel.EntityPM);
            }
        });
    };
    OCITabComponent.prototype.RunWindow = function (item, windowTitle) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.DataContext = item;
        logWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/OCI/AddEditOCIComponent");
    };
    OCITabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'OCITabComponent',
            templateUrl: './OCITabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], OCITabComponent);
    return OCITabComponent;
}(BaseComponent_1.BaseComponent));
exports.OCITabComponent = OCITabComponent;
var AWBWizardOCIItem = /** @class */ (function (_super) {
    __extends(AWBWizardOCIItem, _super);
    function AWBWizardOCIItem(entityPM, isNew, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.DataContext = _this;
        _this.ObjectTableName = "AWBOCI";
        _this.IsNewEntity = false;
        _this.IsWindowMode = false;
        _this.IsEditingEnabled = false;
        _this.EntityPM = entityPM;
        _this.ShipmentPM = fatherComponent.EntityPM;
        _this.IsNewEntity = isNew;
        _this.SetUIProperties();
        return _this;
    }
    AWBWizardOCIItem.prototype.SetUIProperties = function () {
        var isFieldRequired = false;
        if (Tools_2.AppTool.IsNullOrEmpty(this.SupplementaryCustomsInfo)) {
            if (this.IsWindowMode) {
                isFieldRequired = true;
            }
            else {
                if (!Tools_2.AppTool.IsNullOrEmpty(this.CountryId) || !Tools_2.AppTool.IsNullOrEmpty(this.AWBInformationCode) || !Tools_2.AppTool.IsNullOrEmpty(this.AWBCustomsInformationCode)) {
                    isFieldRequired = true;
                }
            }
        }
        var isEditingEnabled = this.fatherComponent.IsEditingEnabled;
        this.UIProperties.SetRequired("SupplementaryCustomsInfo", this.ObjectTableName, isFieldRequired);
        this.UIProperties.SetEnabled('CountryId', this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled('AWBInformationCode', this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled('AWBCustomsInformationCode', this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled('SupplementaryCustomsInfo', this.ObjectTableName, isEditingEnabled);
        this.IsEditingEnabled = isEditingEnabled;
    };
    Object.defineProperty(AWBWizardOCIItem.prototype, "CountryId", {
        // Properties
        get: function () { return this.EntityPM.CountryId; },
        set: function (newValue) {
            if (this.EntityPM.CountryId != newValue) {
                this.EntityPM.CountryId = newValue;
                this.OnDataChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOCIItem.prototype, "AWBInformationCode", {
        get: function () { return this.EntityPM.AWBInformationCode; },
        set: function (newValue) {
            if (this.EntityPM.AWBInformationCode != newValue) {
                this.EntityPM.AWBInformationCode = newValue;
                this.OnDataChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOCIItem.prototype, "AWBCustomsInformationCode", {
        get: function () { return this.EntityPM.AWBCustomsInformationCode; },
        set: function (newValue) {
            if (this.EntityPM.AWBCustomsInformationCode != newValue) {
                this.EntityPM.AWBCustomsInformationCode = newValue;
                this.OnDataChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOCIItem.prototype, "SupplementaryCustomsInfo", {
        get: function () { return this.EntityPM.SupplementaryCustomsInfo; },
        set: function (newValue) {
            if (this.EntityPM.SupplementaryCustomsInfo != newValue) {
                this.EntityPM.SupplementaryCustomsInfo = newValue;
                this.OnDataChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBWizardOCIItem.prototype.OnDataChanged = function () {
        this.SetUIProperties();
        if (!this.IsWindowMode) {
            var itemIndex = this.fatherComponent.EntityPM.AWBOCIPMs.indexOf(this.EntityPM);
            if (!Tools_2.AppTool.IsNullOrEmpty(this.CountryId) || !Tools_2.AppTool.IsNullOrEmpty(this.AWBCustomsInformationCode) || !Tools_2.AppTool.IsNullOrEmpty(this.AWBInformationCode) || !Tools_2.AppTool.IsNullOrEmpty(this.SupplementaryCustomsInfo)) {
                if (itemIndex == -1) {
                    this.fatherComponent.EntityPM.AddOCI(this.EntityPM);
                }
            }
            else {
                if (itemIndex > -1) {
                    this.fatherComponent.EntityPM.RemoveOCI(this.EntityPM);
                }
            }
            this.SetUIProperties();
            this.fatherComponent.Wizard.ValidateScreen_OCI();
        }
    };
    return AWBWizardOCIItem;
}(BaseComponent_1.BaseComponent));
exports.AWBWizardOCIItem = AWBWizardOCIItem;
//# sourceMappingURL=OCITabComponent.js.map