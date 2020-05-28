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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EditCustomerAdditionalServiceComponent = /** @class */ (function (_super) {
    __extends(EditCustomerAdditionalServiceComponent, _super);
    function EditCustomerAdditionalServiceComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.ObjectTableName = "Customer";
        _this.DataContext = _this;
        _this.ShowRadioButtons = true;
        _this.CustomerAdditionalServiceRadio = "CustomerAdditionalServiceRadio_";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CustomerAdditionalServiceRadio += _this.CurrentSession.GetNewId("RadioButton");
        return _this;
    }
    Object.defineProperty(EditCustomerAdditionalServiceComponent.prototype, "Potential", {
        get: function () {
            if (this.EntityPM != null) {
                return this.EntityPM.Potential;
            }
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (value != this.EntityPM.Potential)
                    this.EntityPM.Potential = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditCustomerAdditionalServiceComponent.prototype, "Notes", {
        get: function () {
            if (this.EntityPM != null)
                return this.EntityPM.Notes;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (value != this.EntityPM.Notes)
                    this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditCustomerAdditionalServiceComponent.prototype, "InUse", {
        get: function () {
            if (this.EntityPM != null) {
                return this.EntityPM.InUse;
            }
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (value != this.EntityPM.InUse)
                    this.EntityPM.InUse = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    EditCustomerAdditionalServiceComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args;
    };
    EditCustomerAdditionalServiceComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    EditCustomerAdditionalServiceComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    };
    EditCustomerAdditionalServiceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditCustomerAdditionalServiceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditCustomerAdditionalServiceComponent);
    return EditCustomerAdditionalServiceComponent;
}(BaseComponent_1.BaseComponent));
exports.EditCustomerAdditionalServiceComponent = EditCustomerAdditionalServiceComponent;
//# sourceMappingURL=EditCustomerAdditionalServiceComponent.js.map