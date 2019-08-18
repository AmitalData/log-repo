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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../../Shipment/Tools");
var Cloner_1 = require("../../../../../Infrastructure/Utilities/Cloner");
var AdvancedCommentsComponent = /** @class */ (function (_super) {
    __extends(AdvancedCommentsComponent, _super);
    function AdvancedCommentsComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Shipment";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsEditingEnabled = false;
        return _this;
    }
    AdvancedCommentsComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.EntityPM = windowArgs;
        this.Clone();
        this.SetUIProperties();
    };
    AdvancedCommentsComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_1.ShipmentTool.IsEditingEnabled(this.EntityPM);
    };
    Object.defineProperty(AdvancedCommentsComponent.prototype, "AWBComments", {
        get: function () { return this.EntityPM.AWBComments; },
        set: function (value) {
            if (this.EntityPM.AWBComments != value) {
                this.EntityPM.AWBComments = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdvancedCommentsComponent.prototype, "AWBPrintingComments", {
        get: function () { return this.EntityPM.AWBPrintingComments; },
        set: function (value) {
            if (this.EntityPM.AWBPrintingComments != value) {
                this.EntityPM.AWBPrintingComments = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AdvancedCommentsComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AdvancedCommentsComponent.prototype.OkButtonClicked = function () {
        this.ValidationErrorsList = [];
        var minMaxMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.MinMax");
        minMaxMessage = minMaxMessage.replace("%Minlength", "0");
        if (this.AWBComments != null && this.AWBComments.length > 195) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "195");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AWBComments.Short")));
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    };
    AdvancedCommentsComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('AWBComments');
        this.myCloner.AddField('AWBPrintingComments');
        this.myCloner.AddEntity(this.EntityPM);
    };
    AdvancedCommentsComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AdvancedCommentsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AdvancedCommentsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AdvancedCommentsComponent);
    return AdvancedCommentsComponent;
}(BaseComponent_1.BaseComponent));
exports.AdvancedCommentsComponent = AdvancedCommentsComponent;
//# sourceMappingURL=AdvancedCommentsComponent.js.map