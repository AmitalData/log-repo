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
var BusinessUnitPM_1 = require("../../../EntityPMs/BusinessUnitPM");
var BusinessUnitPMService_1 = require("../../../Services/StandardPMs/BusinessUnitPMService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var NewBusinessUnitComponent = /** @class */ (function (_super) {
    __extends(NewBusinessUnitComponent, _super);
    function NewBusinessUnitComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "BusinessUnit";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new BusinessUnitPM_1.BusinessUnitPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.SetUIProperties();
        return _this;
    }
    NewBusinessUnitComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("ParentId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.ParentId));
    };
    Object.defineProperty(NewBusinessUnitComponent.prototype, "Name", {
        //Properties
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value) {
                this.EntityPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessUnitComponent.prototype, "ParentId", {
        get: function () { return this.EntityPM.ParentId; },
        set: function (value) {
            if (this.EntityPM.ParentId != value) {
                this.EntityPM.ParentId = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands 
    NewBusinessUnitComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewBusinessUnitComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.ParentId)) {
            errors.push(msg.replace("%FieldName", "Parent"));
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var service = new BusinessUnitPMService_1.BusinessUnitPMService();
            service.insert(this.EntityPM).subscribe(function (myResult) {
                if (myResult) {
                    if (!myResult.HasError) {
                        _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        _this.ValidationErrorsList = myResult.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
    };
    NewBusinessUnitComponent = __decorate([
        core_1.Component({
            selector: 'NewBusinessUnitComponent',
            moduleId: module.id,
            templateUrl: './NewBusinessUnitComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewBusinessUnitComponent);
    return NewBusinessUnitComponent;
}(BaseComponent_1.BaseComponent));
exports.NewBusinessUnitComponent = NewBusinessUnitComponent;
//# sourceMappingURL=NewBusinessUnitComponent.js.map