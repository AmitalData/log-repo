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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var ShipmentPackageHarmonizePM_1 = require("../../../../Shipment/EntityPMs/ShipmentPackageHarmonizePM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var AddEditPackageHarmonizeComponent = /** @class */ (function () {
    function AddEditPackageHarmonizeComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.ShipmentPM = null;
        this.ItemsSource = [];
        this.ObjectTableName = "ShipmentPackageHarmonize";
        this.IsEditingEnabled = true;
        this.IsVisibile = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isPackageDirty = false;
        this.isShipmentDirty = false;
    }
    AddEditPackageHarmonizeComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(function (response) {
            if (args) {
                _this.IsEditingEnabled = args['IsEditingEnabled'];
                _this.EntityPM = args['PackagePM'];
                _this.ShipmentPM = args['ShipmentPM'];
                _this.isPackageDirty = _this.EntityPM.IsDirty;
                _this.isShipmentDirty = _this.ShipmentPM.IsDirty;
                _this.EntityPM.ShipmentPackageHarmonizes.forEach(function (item) {
                    _this.ItemsSource.push(new HarmonizeItemClass(item));
                });
                _this.Clone();
            }
            _this.IsVisibile = true;
        });
    };
    AddEditPackageHarmonizeComponent.prototype.AddButtonClicked = function () {
        var item = new ShipmentPackageHarmonizePM_1.ShipmentPackageHarmonizePM(this.EntityPM);
        this.ItemsSource.push(new HarmonizeItemClass(item));
    };
    AddEditPackageHarmonizeComponent.prototype.DeleteItem = function (item) {
        if (item) {
            var index = this.ItemsSource.indexOf(item);
            if (index > -1) {
                this.ItemsSource.splice(index, 1);
            }
        }
    };
    AddEditPackageHarmonizeComponent.prototype.CancelButtonClicked = function () {
        this.ItemsSource.forEach(function (item) {
            if (item.Harmonize != item.OldValue) {
                item.Harmonize = item.OldValue;
            }
        });
        this.EntityPM.IsDirty = this.isPackageDirty;
        this.ShipmentPM.IsDirty = this.isShipmentDirty;
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPackageHarmonizeComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        this.ItemsSource.forEach(function (item) {
            Validator_1.Validator.TryValidateObject(item.EntityPM, _this.ObjectTableName, errors);
        });
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            var allItemsPM = [];
            this.ItemsSource.forEach(function (item) {
                var index = _this.EntityPM.ShipmentPackageHarmonizes.indexOf(item.EntityPM);
                if (index > -1) {
                    var itemPM = _this.EntityPM.ShipmentPackageHarmonizes[index];
                    if (itemPM) {
                        if (itemPM.Harmonize != item.Harmonize) {
                            itemPM.Harmonize = item.Harmonize;
                        }
                    }
                }
                else {
                    _this.EntityPM.ShipmentPackageHarmonizes.push(item.EntityPM);
                }
                allItemsPM.push(item.EntityPM);
            });
            for (var i = this.EntityPM.ShipmentPackageHarmonizes.length - 1; i >= 0; i--) {
                var index = allItemsPM.indexOf(this.EntityPM.ShipmentPackageHarmonizes[i]);
                if (index == -1) {
                    var item = this.EntityPM.ShipmentPackageHarmonizes[i];
                    this.EntityPM.RemoveShipmentPackageHarmonizePM(item);
                }
            }
            this.EntityPM.IsMultiHarmonize = this.EntityPM.ShipmentPackageHarmonizes.length > 0 ? true : false;
            if (this.EntityPM.IsMultiHarmonize) {
                if (this.EntityPM.Harmonize) {
                    this.EntityPM.Harmonize = null;
                }
            }
            this.CurrentSession.CloseCurrentWindowEmit("Ok");
        }
    };
    AddEditPackageHarmonizeComponent.prototype.Clone = function () {
    };
    AddEditPackageHarmonizeComponent.prototype.RejectChanges = function () {
        //this.DataContext.ResetPackageItems();
        //this.myCloner.RejectChanges();
    };
    AddEditPackageHarmonizeComponent.prototype.ChooseHarmonizeClicked = function (item) {
        if (item) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural("HarmonizeCode") + " Search";
            logitudeWindow.WindowArgs = { Entity: item, FieldName: 'Harmonize' };
            logitudeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Windows/Harmonizes/HarmonizesComponent");
            logitudeWindow.WindowClosed.subscribe(function (s) {
            });
        }
    };
    AddEditPackageHarmonizeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditPackageHarmonizeComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], AddEditPackageHarmonizeComponent);
    return AddEditPackageHarmonizeComponent;
}());
exports.AddEditPackageHarmonizeComponent = AddEditPackageHarmonizeComponent;
var HarmonizeItemClass = /** @class */ (function (_super) {
    __extends(HarmonizeItemClass, _super);
    function HarmonizeItemClass(item) {
        var _this = _super.call(this) || this;
        _this.Id = null;
        _this.ObjectTableName = "ShipmentPackageHarmonize";
        _this.OldValue = null;
        _this.Id = item.Id;
        _this.EntityPM = item;
        _this.OldValue = item.Harmonize;
        return _this;
    }
    Object.defineProperty(HarmonizeItemClass.prototype, "Harmonize", {
        get: function () { return this.EntityPM.Harmonize; },
        set: function (value) {
            if (this.EntityPM.Harmonize != value) {
                this.EntityPM.Harmonize = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return HarmonizeItemClass;
}(BaseComponent_1.BaseComponent));
//# sourceMappingURL=AddEditPackageHarmonizeComponent.js.map