"use strict";
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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FieldTemplateComponent = /** @class */ (function () {
    function FieldTemplateComponent() {
        this.Entity = null;
        this.FieldName = null;
        this.FieldValue = null;
        this.ObjectTableName = null;
        this.ShipmentLevelCode = null;
        this.IsHouseIconVisible = true;
        this.IsHouseNotConnected = false;
        this.SpotlightDataTemplate = null;
        this.IsSpotLightTemplate = false;
        this.IsHeaderScreenTemplate = false;
        this.localCurrency = "(" + SessionLocator_1.SessionLocator.LocalCurrencyCode + ")";
        this.ProfitCurrency = "(" + SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode + ")";
        this.Retries = 0;
        this.FWBStatusSource = null;
        this.FWBStatusTooltip = null;
        this.FHLStatusSource = null;
        this.FHLStatusTooltip = null;
        this.CargonautFWBStatusSource = null;
        this.CargonautFWBStatusTooltip = null;
        this.CargonautFHLStatusSource = null;
        this.CargonautFHLStatusTooltip = null;
    }
    FieldTemplateComponent.prototype.Run = function (args) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];
        if (this.Entity != null) {
            if (this.FieldName != null) {
                this.FieldValue = this.Entity[this.FieldName];
                this.ShipmentLevelCode = this.Entity.ShipmentLevelCode;
                if (this.Entity.ShipmentLevelCode == 'H' && Tools_1.AppTool.IsNullOrEmpty(this.Entity.MasterShipmentDataId)) {
                    this.IsHouseNotConnected = true;
                }
                if (this.IsHeaderScreenTemplate) {
                    if (!this.IsHouseNotConnected) {
                        this.IsHouseIconVisible = false;
                    }
                }
                switch (this.FieldName) {
                    case "FWBStatusName": {
                        this.SetFWBStatusSource();
                        break;
                    }
                    case "FHLStatusName": {
                        this.SetFHLStatusSource();
                        break;
                    }
                    case "CargonautFWBStatusName": {
                        this.SetCargonautFWBStatusSource();
                        break;
                    }
                    case "CargonautFHLStatusName": {
                        this.SetCargonautFHLStatusSource();
                        break;
                    }
                }
            }
            if (this.IsSpotLightTemplate) {
                this.RunComponent();
            }
        }
    };
    FieldTemplateComponent.prototype.RunComponent = function () {
        var _this = this;
        if (this.SpotLightViewContainerRef) {
            this.SpotLightViewContainerRef.clear();
            var myComponentPath = "./Shipment/Components/Spotlight/ShipmentSpotlightComponent";
            SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, this.SpotLightViewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.Run(_this.Entity.Id);
            });
        }
        else {
            this.RunComponentTimer();
        }
    };
    FieldTemplateComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    FieldTemplateComponent.prototype.SetFWBStatusSource = function () {
        this.FWBStatusSource = "./Images/Icons/AWB/" + this.Entity['FWBStatusCode'] + ".png";
        this.FWBStatusTooltip = this.ConvertTooltip(this.Entity['FWBStatusName']);
    };
    FieldTemplateComponent.prototype.SetFHLStatusSource = function () {
        this.FHLStatusSource = "./Images/Icons/AWB/" + this.Entity['FHLStatusCode'] + ".png";
        this.FHLStatusTooltip = this.ConvertTooltip(this.Entity['FHLStatusName']);
    };
    FieldTemplateComponent.prototype.SetCargonautFWBStatusSource = function () {
        this.CargonautFWBStatusSource = "./Images/Icons/AWB/" + this.Entity['CargonautFWBStatusCode'] + ".png";
        this.CargonautFWBStatusTooltip = this.ConvertTooltip(this.Entity['CargonautFWBStatusName']);
    };
    FieldTemplateComponent.prototype.SetCargonautFHLStatusSource = function () {
        this.CargonautFHLStatusSource = "./Images/Icons/AWB/" + this.Entity['CargonautFHLStatusCode'] + ".png";
        this.CargonautFHLStatusTooltip = this.ConvertTooltip(this.Entity['CargonautFHLStatusName']);
    };
    FieldTemplateComponent.prototype.ConvertTooltip = function (tooltip) {
        var myResult = tooltip;
        if (myResult) {
            myResult.replace("Error", "FNA error by airline");
            myResult.replace("Accepted", "Accepted by airline");
        }
        return myResult;
    };
    __decorate([
        core_1.ViewChild('SpotLight', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], FieldTemplateComponent.prototype, "SpotLightViewContainerRef", void 0);
    FieldTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FieldTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], FieldTemplateComponent);
    return FieldTemplateComponent;
}());
exports.FieldTemplateComponent = FieldTemplateComponent;
//# sourceMappingURL=FieldTemplateComponent.js.map