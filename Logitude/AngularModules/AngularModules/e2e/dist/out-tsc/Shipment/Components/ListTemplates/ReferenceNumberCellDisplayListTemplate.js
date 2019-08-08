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
var WebFreightDomainService_1 = require("../../../Infrastructure/Services/WebFreightDomainService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ReferenceNumberCellDisplayListTemplate = /** @class */ (function () {
    function ReferenceNumberCellDisplayListTemplate(CD) {
        this.CD = CD;
        this.Source = "";
        this.ShowImg = true;
        this.TransportModSRC = '';
        this.DirectionSRC = '';
        //public Imgs: Logosdictionary[];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ToggleIsExportShipments = false;
        if (!this.CurrentSession.Imgs) {
            this.CurrentSession.Imgs = [];
        }
        var FeatureToggle = SessionLocator_1.SessionLocator.FeatureToggles.filter(function (d) { return d.ToggleCode == "LEX" && d.TenantNumber == SessionLocator_1.SessionLocator.Tenant; })[0];
        if (FeatureToggle) {
            this.ToggleIsExportShipments = true;
        }
    }
    ReferenceNumberCellDisplayListTemplate.prototype.setVariables = function (rowData, fieldName) {
        var _this = this;
        this.rowData = rowData;
        this.fieldName = fieldName;
        if (this.rowData['TransportModeId']) {
            this.TransportModSRC = './Images/TransportModes/' + rowData['TransportModeId'] + '.png';
        }
        if (this.rowData['DirectionId']) {
            this.DirectionSRC = './Images/Directions/' + rowData['DirectionId'] + '.png';
        }
        var myService = new WebFreightDomainService_1.WebFreightDomainService();
        if (!SessionLocator_1.SessionLocator.PrivateLableSettings) {
            this.ShowImg = true;
            if (this.CurrentSession.Imgs.filter(function (a) { return a.LogoId == rowData['PartnerLogoId']; }).length > 0) {
                this.Source = this.CurrentSession.Imgs.filter(function (a) { return a.LogoId == rowData['PartnerLogoId']; })[0].Src;
                var isDestroyed = this.CD['destroyed'];
                if (!isDestroyed) {
                    this.CD.detectChanges();
                }
            }
            else {
                if (rowData['PartnerLogoId']) {
                    myService.getHypridPartnerLogo(rowData['PartnerLogoId']).subscribe(function (myResult) {
                        if (myResult) {
                            _this.Source = "data:image/JPEG;base64," + myResult;
                            if (_this.CurrentSession.Imgs.filter(function (a) { return a.LogoId == rowData['PartnerLogoId']; }).length == 0) {
                                _this.CurrentSession.Imgs.push(new Logosdictionary(rowData['PartnerLogoId'], _this.Source));
                            }
                            var isDestroyed = _this.CD['destroyed'];
                            if (!isDestroyed) {
                                _this.CD.detectChanges();
                            }
                        }
                    });
                }
            }
        }
        else {
            this.ShowImg = false;
        }
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    ReferenceNumberCellDisplayListTemplate = __decorate([
        core_1.Component({
            template: "<table>\n                <tr>\n                <td>\n                       <div *ngIf=\"ShowImg\" style=\"width: 80px;height:35px;text-indent: 10px; overflow: hidden; text-overflow: ellipsis;float:left;margin-right: 5px;margin-top: -8px;\">\n                       <img *ngIf=\"Source\" style=\"width: 70px;max-height:35px;\" src=\"{{Source}}\"  title=\"{{rowData ? rowData['PartnerName'] : ''}}\" />\n                       </div>\n                </td>\n                <td *ngIf=\"ToggleIsExportShipments\">\n                       <div style=\"text-indent: 10px; overflow: hidden; text-overflow: ellipsis;float:left;\">\n                        <img width=\"18\" height=\"15\" style=\"vertical-align: middle;margin-left: -7px;\" [src]=\"DirectionSRC\" title=\"{{rowData ? rowData['DirectionName']:''}}\" />\n                       </div>\n                </td>\n                <td>\n                       <div style=\"text-indent: 10px; overflow: hidden; text-overflow: ellipsis;float:left;\">\n                        <img width=\"18\" height=\"15\" style=\"vertical-align: middle;margin-left: -7px;\" [src]=\"TransportModSRC\" title=\"{{rowData ? rowData['TransportModeName']:''}}\" />\n                       </div>\n                </td> \n                <td style=\"width:90px;\">\n                        <div style=\"text-indent: 10px; overflow: hidden; text-overflow: ellipsis;float:left; position: absolute;top: 0;bottom: 0;left: 0;right: 0;\">\n                        <span style=\"text-overflow: ellipsis;overflow: hidden;white-space: nowrap;\" *ngIf=\"fieldName == 'My Shipments'\">{{rowData ? rowData['CustomerReference1']:''}}</span>\n                        <span style=\"text-overflow: ellipsis;overflow: hidden;white-space: nowrap;\" *ngIf=\"fieldName != 'My Shipments'\">{{rowData ? rowData['ForwarderShipmentNumber']:''}}</span>\n                        </div>\n                </td>\n                </tr>\n              </table>\n            "
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ReferenceNumberCellDisplayListTemplate);
    return ReferenceNumberCellDisplayListTemplate;
}());
exports.ReferenceNumberCellDisplayListTemplate = ReferenceNumberCellDisplayListTemplate;
var Logosdictionary = /** @class */ (function () {
    function Logosdictionary(logoId, src) {
        this.LogoId = logoId;
        this.Src = src;
    }
    return Logosdictionary;
}());
exports.Logosdictionary = Logosdictionary;
//# sourceMappingURL=ReferenceNumberCellDisplayListTemplate.js.map