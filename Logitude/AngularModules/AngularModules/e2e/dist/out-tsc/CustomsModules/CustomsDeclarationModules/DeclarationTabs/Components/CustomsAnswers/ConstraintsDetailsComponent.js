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
var ConstraintsDetailsComponent = /** @class */ (function (_super) {
    __extends(ConstraintsDetailsComponent, _super);
    function ConstraintsDetailsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.DeclarationConstraint";
        _this.IsDisplayOnly = false;
        _this.IsAgentObjectionButtonVisibile = false;
        _this.ApprovalDenaialTitle = "";
        //Services
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    ConstraintsDetailsComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.ConstraintPM = args.ConstraintPM;
            this.DeclarationError = args.DeclarationError;
            this.ApprovalDenaialTitle = args.ApprovalDenaialTitle;
            this.IsAgentObjectionButtonVisibile = args.IsAgentObjectionButtonVisibile;
        }
    };
    ConstraintsDetailsComponent.prototype.AgentButtonClicked = function () {
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
    ConstraintsDetailsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ConstraintsDetailsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ConstraintsDetailsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ConstraintsDetailsComponent);
    return ConstraintsDetailsComponent;
}(BaseComponent_1.BaseComponent));
exports.ConstraintsDetailsComponent = ConstraintsDetailsComponent;
//# sourceMappingURL=ConstraintsDetailsComponent.js.map