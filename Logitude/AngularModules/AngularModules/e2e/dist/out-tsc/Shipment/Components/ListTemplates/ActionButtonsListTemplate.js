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
var DocumentsFilingExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var ActionButtonsListTemplate = /** @class */ (function () {
    function ActionButtonsListTemplate(CD) {
        this.CD = CD;
        this.ConnectBtn = "Connect";
        this.Width1 = SessionLocator_1.SessionLocator.PrivateLableSettings ? 70 : 57;
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
    ActionButtonsListTemplate.prototype.setVariables = function (rowData, fieldName) {
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
        //this.fieldName = fieldName;
        //var myService: WebFreightDomainService = new WebFreightDomainService();
        //if (rowData['PartnerLogoId']){
        //    myService.getHypridPartnerLogo(rowData['PartnerLogoId']).subscribe(myResult => {
        //        this.Source = "data:image/JPEG;base64," + myResult;
        //        this.CD.detectChanges(); 
        //    });
        //}
    };
    ActionButtonsListTemplate.prototype.CancelButtonClicked = function () {
        var _this = this;
        //this.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.get(this.rowData.Id).subscribe(function (myResult) {
            if (!myResult.HasError) {
                _this.CurrentSession.StopBusyIndicator();
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Title = "Confirm Deletion";
                confirmWindow.Show("Are you sure you want to cancel this Shipment ?");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.CurrentSession.StartBusyIndicator("Loading ..");
                        myResult.Result.IsCancelled = true;
                        _this._ShipmentPMService.update(myResult.Result).subscribe(function (myResult) {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.CurrentSession.FireEvent({ Name: 'ReloadShipments' });
                            _this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                        });
                    }
                    else {
                    }
                });
            }
        });
    };
    ActionButtonsListTemplate.prototype.ConnectButtonClicked = function () {
        var _this = this;
        //this.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.get(this.rowData.Id).subscribe(function (myResult) {
            if (!myResult.HasError) {
                if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
                    _this._documentsFilingExtendedPMService.IsEntityHasSharedDocs(_this.rowData['Id'], SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                        if (res.Result == false) {
                            _this.HasSharedDocs = false;
                        }
                        else {
                            _this.HasSharedDocs = true;
                        }
                        _this.CurrentSession.StopBusyIndicator();
                        var newWindow = new LogitudeWindow_1.LogitudeWindow();
                        newWindow.Width = 1050;
                        newWindow.Height = 700;
                        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
                            newWindow.Title = "Connect/Create new shipment in " + SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                        }
                        else {
                            newWindow.Title = "Connect To Agent Shipment";
                        }
                        var windowArgs = {};
                        windowArgs.SourceEntity = myResult.Result; //this.rowData;
                        windowArgs.HasSharedDocs = _this.HasSharedDocs;
                        newWindow.WindowArgs = windowArgs;
                        newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/ForwarderShipmentsComponent');
                        newWindow.WindowClosed.subscribe(function ($event) {
                            _this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                        });
                    });
                }
                else {
                    _this.CurrentSession.StopBusyIndicator();
                    var newWindow = new LogitudeWindow_1.LogitudeWindow();
                    newWindow.Width = 1050;
                    newWindow.Height = 700;
                    if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
                        newWindow.Title = "Connect/Create new shipment in " + SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                    }
                    else {
                        newWindow.Title = "Connect To Agent Shipment";
                    }
                    var windowArgs = {};
                    windowArgs.SourceEntity = myResult.Result; //this.rowData;
                    windowArgs.HasSharedDocs = _this.HasSharedDocs;
                    newWindow.WindowArgs = windowArgs;
                    newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/ForwarderShipmentsComponent');
                    newWindow.WindowClosed.subscribe(function ($event) {
                        _this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                    });
                }
            }
        });
    };
    ActionButtonsListTemplate.prototype.EditButtonClicked = function () {
        var _this = this;
        this.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.get(this.rowData.Id).subscribe(function (myResult) {
            if (!myResult.HasError) {
                _this.CurrentSession.StopBusyIndicator();
                var newWindow = new LogitudeWindow_1.LogitudeWindow();
                newWindow.Width = 600;
                newWindow.Height = 350;
                newWindow.Title = "Edit Shipment";
                var windowArgs = {};
                windowArgs.IsNew = false;
                windowArgs.EntityPM = myResult.Result;
                newWindow.WindowArgs = windowArgs;
                //newWindow.Add(control);
                if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
                    newWindow.Height = 376;
                    newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditPrivateLabelShipmentComponent');
                }
                else {
                    newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditImporterShipmentComponent');
                }
                newWindow.WindowClosed.subscribe(function ($event) {
                    _this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                    if ($event == "MyShipmentAdded") {
                        _this.CurrentSession.FireEvent({ Name: 'ReloadShipments' });
                    }
                });
            }
        });
    };
    ActionButtonsListTemplate = __decorate([
        core_1.Component({
            template: "<table *ngIf=\"ShowButtons == true\">\n                <tr style=\"height:1px;\"> \n                    <td>\n                        <div style=\"height:30px;\">\n                            <button class=\"RedButton\" (click)=\"CancelButtonClicked()\" [style.width.px]=\"Width1\" style=\"float: right;margin:4px;\">Cancel</button>\n                            <button class=\"Button\" (click)=\"ConnectButtonClicked()\" [style.width.px]=\"Width\" style=\"float: right;margin:4px;\">{{ConnectBtn}}</button>\n                            <button class=\"Button\" (click)=\"EditButtonClicked()\" [style.width.px]=\"Width1\" style=\"float: right;margin:4px;\">Edit</button>\n                        </div>\n                    </td>\n                </tr>\n            </table>\n            "
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ActionButtonsListTemplate);
    return ActionButtonsListTemplate;
}());
exports.ActionButtonsListTemplate = ActionButtonsListTemplate;
//# sourceMappingURL=ActionButtonsListTemplate.js.map