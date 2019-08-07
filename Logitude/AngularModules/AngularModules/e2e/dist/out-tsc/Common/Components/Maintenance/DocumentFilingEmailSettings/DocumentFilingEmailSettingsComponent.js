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
var InfrastructureDomainService_1 = require("../../../../Infrastructure/Services/InfrastructureDomainService");
var DocumentFilingEmailSettingsComponent = /** @class */ (function (_super) {
    __extends(DocumentFilingEmailSettingsComponent, _super);
    function DocumentFilingEmailSettingsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isDocumentFilingByEmailEnabled = false;
        return _this;
    }
    Object.defineProperty(DocumentFilingEmailSettingsComponent.prototype, "IsDocumentFilingByEmailEnabled", {
        get: function () {
            return this.isDocumentFilingByEmailEnabled;
        },
        set: function (value) {
            if (this.isDocumentFilingByEmailEnabled != value) {
                this.isDocumentFilingByEmailEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    DocumentFilingEmailSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DocumentFilingEmailSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var myService = new InfrastructureDomainService_1.InfrastructureDomainService();
        myService.UpdateTenantSettings(this.IsDocumentFilingByEmailEnabled).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                _this.CurrentSession.CloseCurrentWindow();
            }
        });
    };
    DocumentFilingEmailSettingsComponent = __decorate([
        core_1.Component({
            selector: 'DocumentFilingEmailSettingsComponent',
            moduleId: module.id,
            templateUrl: './DocumentFilingEmailSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DocumentFilingEmailSettingsComponent);
    return DocumentFilingEmailSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentFilingEmailSettingsComponent = DocumentFilingEmailSettingsComponent;
//# sourceMappingURL=DocumentFilingEmailSettingsComponent.js.map