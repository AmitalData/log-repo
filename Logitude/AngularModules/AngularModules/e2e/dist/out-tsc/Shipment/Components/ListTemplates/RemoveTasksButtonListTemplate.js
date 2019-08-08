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
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var RemoveTasksButtonListTemplate = /** @class */ (function () {
    function RemoveTasksButtonListTemplate(CD) {
        this.CD = CD;
        this.ConnectBtn = "Connect";
        this.Width = 57;
        this.ShowButtons = true;
        this.HasSharedDocs = true;
        this.ShowRenewButtons = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        //if (SessionLocator.PrivateLableSettings) {
        //    this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        //    this.Width = 80;
        //    this.ConnectBtn = SessionLocator.PrivateLableSettings.PrivateLabelShortName + " Connect";
        //}
    }
    RemoveTasksButtonListTemplate.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.ShowRenewButtons = (this.rowData['IsDepositionRequired'] == true);
        //if (SessionLocator.PrivateLableSettings) {
        //    this.ShowButtons = this.rowData['StatusName'].toLowerCase() == "in progress" ? false : true;
        //    if (SessionLocator.PrivateLableSettings) {
        //        this._documentsFilingExtendedPMService.IsEntityHasSharedDocs(this.rowData['Id'], SessionLocator.Tenant).subscribe(res => {
        //            if (res.Result == false) {
        //                this.HasSharedDocs = false;
        //            }
        //        });
        //    }
        //}
        //this.fieldName = fieldName;
        //var myService: WebFreightDomainService = new WebFreightDomainService();
        //if (rowData['PartnerLogoId']){
        //    myService.getHypridPartnerLogo(rowData['PartnerLogoId']).subscribe(myResult => {
        //        this.Source = "data:image/JPEG;base64," + myResult;
        //        this.CD.detectChanges(); 
        //    });
        //}
    };
    RemoveTasksButtonListTemplate.prototype.RemoveTasksButtonClicked = function () {
        var _this = this;
        this.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = "Confirm Deletion";
        confirmWindow.Show("Are you sure you want to cancel tasks for this shipment ?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.CurrentSession.StartBusyIndicator("Loading ..");
                _this._ShipmentPMService.RemoveShipmentTasks(_this.rowData.Id).subscribe(function (myResult) {
                    if (!myResult.HasError) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                        _this.CurrentSession.FireEvent({ Name: 'CustomReloadShipments' });
                    }
                });
            }
            else {
            }
        });
    };
    RemoveTasksButtonListTemplate.prototype.EditButtonClicked = function () {
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
    RemoveTasksButtonListTemplate.prototype.RenewButtonClicked = function () {
        var _this = this;
        var newWindow = new LogitudeWindow_1.LogitudeWindow();
        newWindow.Width = 600;
        newWindow.Height = 230;
        var forwarderShipmentNumber = this.rowData['ForwarderShipmentNumber'];
        if (!forwarderShipmentNumber)
            forwarderShipmentNumber = "";
        newWindow.Title = "נדרש תצהיר עבור תיק עמילות" + " " + forwarderShipmentNumber;
        var windowArgs = {};
        if (this.rowData) {
            windowArgs.ShipmentId = this.rowData['Id'];
            windowArgs.ImporterDepositionRequestDetails = this.rowData['ImporterDepositionRequestDetails'];
            windowArgs.ForwarderShipmentNumber = this.rowData['ForwarderShipmentNumber'];
            windowArgs.DirectionId = this.rowData['DirectionId'];
            windowArgs.ForwarderPartnerId = this.rowData['ForwarderPartnerId'];
        }
        //windowArgs.EntityPm = myResult.Result
        newWindow.WindowArgs = windowArgs;
        newWindow.RTL = true;
        newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/DepositionRequestComponent');
        newWindow.WindowClosed.subscribe(function ($event) {
            _this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
            if ($event == "DepositionRequest") {
                _this.CurrentSession.FireEvent({ Name: 'CustomReloadShipments' });
            }
        });
    };
    RemoveTasksButtonListTemplate = __decorate([
        core_1.Component({
            template: "<table>\n                <tr style=\"height:1px;\"> \n                    <td>\n                        <div style=\"height:30px;\">\n\n                            <button class=\"Button\" (click)=\"EditButtonClicked()\" style=\"width:40px;float: right;margin:4px;\">Edit</button>\n                            <button class=\"Button\" (click)=\"RemoveTasksButtonClicked()\" style=\"width:85px;float: right;margin:4px;\">Remove Tasks</button>\n                            <button class=\"RedButton\" *ngIf=\"ShowRenewButtons == true\"  (click)=\"RenewButtonClicked()\" style=\"width:50px;float: right;margin:4px;\">Renew</button>\n                        </div>\n                    </td>\n                    \n                </tr>\n            </table>\n            "
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], RemoveTasksButtonListTemplate);
    return RemoveTasksButtonListTemplate;
}());
exports.RemoveTasksButtonListTemplate = RemoveTasksButtonListTemplate;
//# sourceMappingURL=RemoveTasksButtonListTemplate.js.map