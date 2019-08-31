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
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ShipmentPMService_1 = require("../../../Shipment/Services/StandardPMs/ShipmentPMService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var DocumentsFilingExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var Tools_1 = require("../../../Infrastructure/Tools");
var EditShipmentButtonListTemplate = /** @class */ (function () {
    function EditShipmentButtonListTemplate(CD) {
        this.CD = CD;
        this.ConnectBtn = "Connect";
        this.Width = 57;
        this.ShowButtons = true;
        this.HasSharedDocs = true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
            this.Width = 80;
            this.ConnectBtn = SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName + " Connect";
        }
    }
    EditShipmentButtonListTemplate.prototype.setVariables = function (rowData, fieldName) {
        var _this = this;
        this.rowData = rowData;
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            this.ShowButtons = this.rowData['StatusName'].toLowerCase() == "in progress" ? false : true;
            if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
                this._documentsFilingExtendedPMService.IsEntityHasSharedDocs(this.rowData['Id'], SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                    if (res.Result == false) {
                        _this.HasSharedDocs = false;
                    }
                });
            }
        }
        this.fieldName = fieldName;
        if (Tools_1.AppTool.IsNullOrEmpty(this.rowData['ForwarderShipmentNumber']) && this.fieldName == "EditShipmentButtonListTemplate" + "All Shipments") {
            this.ShowButtons = false;
        }
        //var myService: WebFreightDomainService = new WebFreightDomainService();
        //if (rowData['PartnerLogoId']){
        //    myService.getHypridPartnerLogo(rowData['PartnerLogoId']).subscribe(myResult => {
        //        this.Source = "data:image/JPEG;base64," + myResult;
        //        this.CD.detectChanges(); 
        //    });
        //}
    };
    EditShipmentButtonListTemplate.prototype.EditButtonClicked = function () {
        var _this = this;
        this.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.get(this.rowData.Id).subscribe(function (myResult) {
            if (!myResult.HasError) {
                _this.CurrentSession.StopBusyIndicator();
                var newWindow = new LogitudeWindow_1.LogitudeWindow();
                newWindow.Width = 600;
                newWindow.Height = 150;
                newWindow.Title = "Edit Shipment";
                var windowArgs = {};
                windowArgs.IsNew = false;
                windowArgs.EntityPm = myResult.Result;
                newWindow.WindowArgs = windowArgs;
                //newWindow.Add(control);
                //if (SessionLocator.PrivateLableSettings) {
                //    newWindow.Show('./Shipment/Components/Logbox/AddEditPrivateLabelShipmentComponent');
                //}
                //else {
                newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/EditLogBoxShipmentComponent');
                //}
                newWindow.WindowClosed.subscribe(function ($event) {
                    _this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                    if ($event == "MyShipmentAdded") {
                        _this.CurrentSession.FireEvent({ Name: 'CustomReloadShipments' });
                    }
                });
            }
        });
    };
    EditShipmentButtonListTemplate = __decorate([
        core_1.Component({
            template: "<table *ngIf=\"ShowButtons == true\">\n                <tr style=\"height:1px;\"> \n                    <td>\n                        <div style=\"height:30px;\"> \n                            <button class=\"Button\" (click)=\"EditButtonClicked()\" style=\"width:57px;margin:4px;\">Edit</button>\n                        </div>\n                    </td>\n                </tr>\n            </table>\n            "
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], EditShipmentButtonListTemplate);
    return EditShipmentButtonListTemplate;
}());
exports.EditShipmentButtonListTemplate = EditShipmentButtonListTemplate;
//# sourceMappingURL=EditShipmentButtonListTemplate.js.map