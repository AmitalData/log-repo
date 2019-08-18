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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var VatTypePercentagePM_1 = require("../../../EntityPMs/VatTypePercentagePM");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var VatTypePercentagesTabComponent = /** @class */ (function (_super) {
    __extends(VatTypePercentagesTabComponent, _super);
    function VatTypePercentagesTabComponent(args, entityResourceService) {
        var _this = _super.call(this) || this;
        _this.args = args;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.IsNewEntity = true;
        _this.ObjectTableName = "VatType";
        _this.ItemsSource = [];
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.selectedItem = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.isWindowOpened = false;
        _this.entityResourceService.getEntityResourceByTableName("VatType").subscribe(function (res) {
            _this.entityResourceService.getEntityResourceByTableName("VatTypePercentage").subscribe(function (res2) {
                _this.IsResourcesReady = true;
                _this.EntityPM = args.EntityPM;
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) {
                    _this.IsNewEntity = false;
                    _this.Listen();
                }
                _this.SetUIProperties();
                _this.BuildItemsSource();
            });
        });
        return _this;
    }
    Object.defineProperty(VatTypePercentagesTabComponent.prototype, "IsMultiPercentage", {
        get: function () { return this.EntityPM.IsMultiPercentage; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VatTypePercentagesTabComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (value) {
            if (value != this.selectedItem) {
                this.selectedItem = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    VatTypePercentagesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.BuildItemsSource();
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.BuildItemsSource();
                    }
                });
            }
        }
    };
    VatTypePercentagesTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    VatTypePercentagesTabComponent.prototype.SetUIProperties = function () {
    };
    VatTypePercentagesTabComponent.prototype.BuildItemsSource = function () {
        var items = this.EntityPM.VatTypePercentages;
        items.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.FromDate) === Tools_1.DateTool.GetDateFromDate(b.FromDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.FromDate) > Tools_1.DateTool.GetDateFromDate(b.FromDate)) ? -1 : 1; });
        this.ItemsSource = items;
        this.SelectedItem = null;
    };
    VatTypePercentagesTabComponent.prototype.AddPercentage = function () {
        var itemPM = new VatTypePercentagePM_1.VatTypePercentagePM(null);
        itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        itemPM.VatTypeId = this.EntityPM.Id;
        itemPM.FromDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        var title = TextCodeTranslator_1.TextCodeTranslator.Translate("VatTypePercentage.O.AddVatTypePercentage");
        this.RunAddEditWindow(itemPM, title, true);
    };
    VatTypePercentagesTabComponent.prototype.EditPercentage = function (itemPM) {
        var title = TextCodeTranslator_1.TextCodeTranslator.Translate("VatTypePercentage.O.EditVatTypePercentage");
        this.RunAddEditWindow(itemPM, title, false);
    };
    VatTypePercentagesTabComponent.prototype.RunAddEditWindow = function (itemPM, windowTitle, isNewEntity) {
        var _this = this;
        if (!this.isWindowOpened) {
            this.isWindowOpened = true;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = { EntityPM: itemPM, VatTypePM: this.EntityPM, IsNewEntity: isNewEntity };
            logWindow.Show('./Common/Components/Maintenance/VatType/NewVatTypePercentageComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                _this.isWindowOpened = false;
                _this.BuildItemsSource();
            });
        }
    };
    VatTypePercentagesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VatTypePercentagesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], VatTypePercentagesTabComponent);
    return VatTypePercentagesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.VatTypePercentagesTabComponent = VatTypePercentagesTabComponent;
//# sourceMappingURL=VatTypePercentagesTabComponent.js.map