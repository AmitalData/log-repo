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
var AddEditAirlineAreaComponent_1 = require("./AddEditAirlineAreaComponent");
var ChoosePortComponent = /** @class */ (function (_super) {
    __extends(ChoosePortComponent, _super);
    function ChoosePortComponent() {
        var _this = _super.call(this) || this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DataContext = _this;
        _this.ObjectTableName = "AirlineAreasPort";
        _this.ValidationErrorsList = [];
        _this.port = null;
        return _this;
    }
    ChoosePortComponent.prototype.SetDataContext = function (dataContext) {
        this.ParentClass = dataContext;
    };
    Object.defineProperty(ChoosePortComponent.prototype, "PortId", {
        get: function () {
            return this.portId;
        },
        set: function (value) {
            if (this.portId != value) {
                this.portId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChoosePortComponent.prototype, "Port", {
        get: function () { return this.port; },
        set: function (newValue) {
            if (this.port != newValue) {
                this.port = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ChoosePortComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ChoosePortComponent.prototype.AddButtonClicked = function () {
        var _this = this;
        var errors = [];
        if (this.Port == null || this.PortId == null) {
            errors.push("Please Choose port");
        }
        else if (this.ParentClass.fatherComponent.ItemList.filter(function (d) { return d.Code == _this.Port.Code; }).length > 0) {
            errors.push("Port with the same code already added");
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            var newItem = new AddEditAirlineAreaComponent_1.DestinationClass(this.ParentClass.fatherComponent, this.Port, true);
            this.ParentClass.fatherComponent.ItemList.push(newItem);
            this.Port = null;
            this.PortId = null;
        }
    };
    ChoosePortComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ChoosePortComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ChoosePortComponent);
    return ChoosePortComponent;
}(BaseComponent_1.BaseComponent));
exports.ChoosePortComponent = ChoosePortComponent;
//# sourceMappingURL=ChoosePortComponent.js.map