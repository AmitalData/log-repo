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
var Tools_1 = require("../../../../Infrastructure/Tools");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_2 = require("../../../../Shipment/Tools");
var PortListService_1 = require("../../../../Common/Services/StandardLists/PortListService");
var AddEditHouseRoutingComponent = /** @class */ (function (_super) {
    __extends(AddEditHouseRoutingComponent, _super);
    function AddEditHouseRoutingComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.TransportModeId = null;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.FromTextCodeLabel = null;
        _this.ToTextCodeLabel = null;
        _this.IsEditingEnabled = false;
        _this.myPortListService = new PortListService_1.PortListService();
        return _this;
    }
    AddEditHouseRoutingComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.FatherComponent = args['FatherComponent'];
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.SetLabels();
        this.SetUIProperties();
        this.Clone();
    };
    AddEditHouseRoutingComponent.prototype.SetLabels = function () {
        switch (this.TransportModeId) {
            case "A": {
                this.FromTextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Gateway");
                this.ToTextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Destination");
                break;
            }
            case "O": {
                this.FromTextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.LoadingPort");
                this.ToTextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.DischargePort");
                break;
            }
            case "I": {
                this.FromTextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.From");
                this.ToTextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.To");
                break;
            }
        }
    };
    AddEditHouseRoutingComponent.prototype.SetUIProperties = function () {
        var isEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageToPortId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("MainCarriageToPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageToPortId) ? true : false);
        this.IsEditingEnabled = isEditingEnabled;
    };
    Object.defineProperty(AddEditHouseRoutingComponent.prototype, "MainCarriageFromPortId", {
        get: function () { return this.EntityPM.MainCarriageFromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageFromPortId != value) {
                this.EntityPM.MainCarriageFromPortId = value;
                this.EntityPM.FromPortId = value;
                this.SetUIProperties();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, null);
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            Tools_2.RoutingHelper.MainCarriageFromPortChanged(_this.EntityPM, list);
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditHouseRoutingComponent.prototype, "MainCarriageToPortId", {
        get: function () { return this.EntityPM.MainCarriageToPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageToPortId != value) {
                this.EntityPM.MainCarriageToPortId = value;
                this.EntityPM.ToPortId = value;
                this.SetUIProperties();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.FinalDestinationPortChanged(this.EntityPM, null);
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            Tools_2.RoutingHelper.FinalDestinationPortChanged(_this.EntityPM, list);
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditHouseRoutingComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditHouseRoutingComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
            errors.push(msg.replace("%FieldName", this.FromTextCodeLabel));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageToPortId)) {
            errors.push(msg.replace("%FieldName", this.ToTextCodeLabel));
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.FatherComponent.BuildItemsCollection();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditHouseRoutingComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this);
        this.myCloner.AddField('FromPortId');
        this.myCloner.AddField('ToPortId');
        this.myCloner.AddEntity(this.EntityPM);
    };
    AddEditHouseRoutingComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditHouseRoutingComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditHouseRoutingComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditHouseRoutingComponent);
    return AddEditHouseRoutingComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditHouseRoutingComponent = AddEditHouseRoutingComponent;
//# sourceMappingURL=AddEditHouseRoutingComponent.js.map