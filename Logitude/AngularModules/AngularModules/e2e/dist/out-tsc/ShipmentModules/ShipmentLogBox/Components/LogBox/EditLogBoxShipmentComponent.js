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
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ShipmentPM_1 = require("../../../../Shipment/EntityPMs/ShipmentPM");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var EditLogBoxShipmentComponent = /** @class */ (function (_super) {
    __extends(EditLogBoxShipmentComponent, _super);
    function EditLogBoxShipmentComponent(_entityListService) {
        var _this = _super.call(this) || this;
        _this._entityListService = _entityListService;
        _this.EntityPm = new ShipmentPM_1.ShipmentPM();
        _this.DataContext = _this;
        _this.IsPrivate = false;
        _this.PLShortName = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CustomerReferenceChanged = false;
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            _this.IsPrivate = true;
            _this.PLShortName = SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName;
        }
        _this.ValidationErrorsList = [];
        _this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        return _this;
    }
    EditLogBoxShipmentComponent.prototype.ngOnInit = function () {
    };
    EditLogBoxShipmentComponent.prototype.ngAfterViewInit = function () {
    };
    EditLogBoxShipmentComponent.prototype.SetWindowArgs = function (args) {
        if (args.EntityPm) {
            this.EntityPm = args.EntityPm;
        }
    };
    Object.defineProperty(EditLogBoxShipmentComponent.prototype, "CustomerReference2", {
        get: function () { return this.EntityPm.CustomerReference2; },
        set: function (newValue) {
            if (this.EntityPm.CustomerReference2 != newValue) {
                this.EntityPm.CustomerReference2 = newValue;
                this.CustomerReferenceChanged = true;
            }
            else {
                this.CustomerReferenceChanged = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditLogBoxShipmentComponent.prototype, "SendUpdatesToAgentEnabled", {
        get: function () { return this.EntityPm.SendUpdatesToAgentEnabled; },
        set: function (newValue) {
            if (this.EntityPm.SendUpdatesToAgentEnabled != newValue) {
                this.EntityPm.SendUpdatesToAgentEnabled = newValue;
                this.CustomerReferenceChanged = true;
            }
            else {
                this.CustomerReferenceChanged = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    EditLogBoxShipmentComponent.prototype.SaveChanges = function () {
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //if (AppTool.IsNullOrEmpty(this.CustomerReference2)) {
        //    this.ValidationErrorsList.push(msg.replace("%FieldName", "My Reference"));
        //}
        if (this.CustomerReferenceChanged == true) {
            this.SaveData();
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    EditLogBoxShipmentComponent.prototype.SaveData = function () {
        var _this = this;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.EntityPm.IsImporterShipment = true;
            this.EntityPm.MainCarriageFromPortId = this.EntityPm.FromPortId;
            this.EntityPm.MainCarriageToPortId = this.EntityPm.ToPortId;
            this.EntityPm.ShipperReference2 = this.EntityPm.CustomerReference2;
            this.EntityPm.ConsigneeReference2 = this.EntityPm.CustomerReference2;
            this.EntityPm.UpdateSendUpdatesToAgentEnabledField = true;
            this.EntityPm.GrossWeightUnitCode = "KG";
            this.EntityPm.DimensionsUnitCode = "Cm";
            this.EntityPm.ChargeableWeightUnitCode = "KG";
            this.EntityPm.VolumeUnitCode = "CBF";
            this._ShipmentPMService.update(this.EntityPm).subscribe(function (myResult) {
                if (!myResult.HasError) {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    //this.CurrentSession.CloseCurrentWindow();
                    //this.CurrentSession.CloseCurrentWindowEmit("CustomReloadShipments");
                    _this.CurrentSession.SessionEvent.emit({ Name: "CustomReloadShipments" });
                    _this.CurrentSession.CurrentWindow.Close("");
                }
                else {
                    //this.ValidationErrorsList = myResult.ErrorsArray;
                    _this.ValidationErrorsList = myResult.ErrorsArray; //.push("There Are Validation Errors.");
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                }
            });
        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    };
    EditLogBoxShipmentComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EditLogBoxShipmentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditLogBoxShipmentComponent.html',
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService])
    ], EditLogBoxShipmentComponent);
    return EditLogBoxShipmentComponent;
}(BaseComponent_1.BaseComponent));
exports.EditLogBoxShipmentComponent = EditLogBoxShipmentComponent;
//# sourceMappingURL=EditLogBoxShipmentComponent.js.map