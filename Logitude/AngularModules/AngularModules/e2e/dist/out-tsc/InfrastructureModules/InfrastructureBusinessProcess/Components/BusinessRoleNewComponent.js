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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BusinessRolePMService_1 = require("../../../Infrastructure/Services/StandardPMs/BusinessRolePMService");
var BusinessRolePM_1 = require("../../../Infrastructure/EntityPMs/BusinessRolePM");
var BusinessRolePMInitService_1 = require("../../../Infrastructure/EntityPMInitServices/BusinessRolePMInitService");
var BusinessRoleNewComponent = /** @class */ (function (_super) {
    __extends(BusinessRoleNewComponent, _super);
    function BusinessRoleNewComponent() {
        var _this = _super.call(this) || this;
        _this.Session = SessionLocator_1.SessionLocator.Tenant;
        _this.DataContext = _this;
        _this.ObjectTableName = "BusinessRole";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new BusinessRolePM_1.BusinessRolePM();
        BusinessRolePMInitService_1.BusinessRolePMInitService.InitValues(_this.EntityPM, true);
        return _this;
    }
    // Methods
    BusinessRoleNewComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            errors.push("Name field is required");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myService = new BusinessRolePMService_1.BusinessRolePMService();
            myService.insert(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit(_this.EntityPM.Id);
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    BusinessRoleNewComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    Object.defineProperty(BusinessRoleNewComponent.prototype, "Name", {
        // Properties
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value)
                this.EntityPM.Name = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessRoleNewComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value)
                this.EntityPM.LocalName = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessRoleNewComponent.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (value) {
            if (this.EntityPM.Description != value)
                this.EntityPM.Description = value;
        },
        enumerable: true,
        configurable: true
    });
    BusinessRoleNewComponent = __decorate([
        core_1.Component({
            selector: 'BusinessRoleNewComponent',
            moduleId: module.id,
            templateUrl: './BusinessRoleNewComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], BusinessRoleNewComponent);
    return BusinessRoleNewComponent;
}(BaseComponent_1.BaseComponent));
exports.BusinessRoleNewComponent = BusinessRoleNewComponent;
//# sourceMappingURL=BusinessRoleNewComponent.js.map