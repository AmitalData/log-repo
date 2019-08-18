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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var CustomsCollateralListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsCollateralListService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var ConstraintDetailWithCollateralComponent = /** @class */ (function (_super) {
    __extends(ConstraintDetailWithCollateralComponent, _super);
    function ConstraintDetailWithCollateralComponent(EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.DeclarationConstraint";
        _this.IsDisplayOnly = false;
        _this.IsAgentObjectionButtonVisibile = false;
        _this.gridHeight = 53;
        _this.Conditionslist = [];
        //Services
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this.customsCollateralListService = new CustomsCollateralListService_1.CustomsCollateralListService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region Properties
        _this.totalAmount = 0.0;
        _this.fieldName = null;
        return _this;
    }
    ConstraintDetailWithCollateralComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            console.log("[Args] ", args);
            this.ConstraintPM = args.ConstraintPM;
            this.DeclarationError = args.DeclarationError;
            this.IsAgentObjectionButtonVisibile = args.IsAgentObjectionButtonVisibile;
            this.LoadCollateralFromConstraint();
        }
    };
    Object.defineProperty(ConstraintDetailWithCollateralComponent.prototype, "TotalAmount", {
        get: function () {
            var _this = this;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.customsCollateralPM)) {
                this.totalAmount = 0.0;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.customsCollateralPM.CustomsCollateralsConditions)) {
                    this.customsCollateralPM.CustomsCollateralsConditions.forEach(function (item) {
                        _this.totalAmount += item.RequestedAmount;
                    });
                }
            }
            return this.totalAmount;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintDetailWithCollateralComponent.prototype, "FieldName", {
        get: function () {
            var field = "";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.DeclarationError.FieldNameTextCode)) {
                field = TextCodeTranslator_1.TextCodeTranslator.Translate(this.DeclarationError.FieldNameTextCode);
            }
            return field;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    ConstraintDetailWithCollateralComponent.prototype.LoadCollateralFromConstraint = function () {
        var _this = this;
        this.declarationWebService.GetSingleCustomsCollateral(this.ConstraintPM.CustomsCollateralId).subscribe(function (response) {
            var res = response.Result;
            console.log("[Response] GetSingleCustomsCollateral", res);
            if (!Tools_1.AppTool.IsNullOrEmpty(res)) {
                _this.customsCollateralPM = res;
                _this.Conditionslist = [];
                _this.Conditionslist = _this.customsCollateralPM.CustomsCollateralsConditions;
                if (_this.Conditionslist.length > 1) {
                    var itemsHeight = _this.Conditionslist.length * 25;
                    _this.gridHeight += itemsHeight - 25; //25 for the first item
                }
            }
        });
    };
    ConstraintDetailWithCollateralComponent.prototype.AgentButtonClicked = function () {
        var window = new LogitudeWindow_1.LogitudeWindow();
        window.Width = 500;
        window.Height = 500;
        window.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsCollateral.O.AgentObjection");
        window.WindowArgs = {
            DeclarationError: this.DeclarationError,
            ConstraintPM: this.ConstraintPM,
        };
        window.Show("./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/CustomsAnswers/AgentObjectionComponent");
    };
    ConstraintDetailWithCollateralComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ConstraintDetailWithCollateralComponent.prototype.ColleteralButtonClicked = function () {
        var _this = this;
        var windowArgs = {};
        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe(function (response) {
            windowArgs.CurrentEntity = _this.customsCollateralPM;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 710;
            logWindow.WindowArgs = windowArgs;
            logWindow.ShowCloseButton = true;
            logWindow.IsHideHeader = true;
            logWindow.Show('./CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent');
            logWindow.WindowClosed.subscribe(function ($event1) {
            });
        });
    };
    ConstraintDetailWithCollateralComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ConstraintDetailWithCollateralComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], ConstraintDetailWithCollateralComponent);
    return ConstraintDetailWithCollateralComponent;
}(BaseComponent_1.BaseComponent));
exports.ConstraintDetailWithCollateralComponent = ConstraintDetailWithCollateralComponent;
//# sourceMappingURL=ConstraintDetailWithCollateralComponent.js.map