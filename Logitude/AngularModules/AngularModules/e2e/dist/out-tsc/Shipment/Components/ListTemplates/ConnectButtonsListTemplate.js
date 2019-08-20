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
var ShipmentPMService_1 = require("../../../Shipment/Services/StandardPMs/ShipmentPMService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var DocumentsFilingExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var DocumentsFilingPMService_1 = require("../../../Common/Services/StandardPMs/DocumentsFilingPMService");
var Tools_1 = require("../../../Infrastructure/Tools");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ConnectButtonsListTemplate = /** @class */ (function () {
    function ConnectButtonsListTemplate(CD) {
        this.CD = CD;
        this.Subscribed = false;
        this.HasSharedDocs = true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        this._documentsFilingPMService = new DocumentsFilingPMService_1.DocumentsFilingPMService();
        //this.CurrentSession.SessionEvent.subscribe(($event: any) => { 
        //    if ($event.Name == "SourceEntity" && $event.EntityId == this.rowData["Id"]) { 
        //        this.SourceEntity = $event.Entity;
        //        this.CompleteChoosingEntity();
        //    }
        //}); 
    }
    ConnectButtonsListTemplate.prototype.setVariables = function (rowData, fieldName) {
        this.SourceId = fieldName;
        this.rowData = rowData;
    };
    ConnectButtonsListTemplate.prototype.ChooseButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Loading ..");
        this._ShipmentPMService.get(this.SourceId).subscribe(function (myResult) {
            if (!myResult.HasError) {
                _this.SourceEntity = myResult.Result;
                _this._ShipmentPMService.get(_this.rowData['Id']).subscribe(function (myResult1) {
                    _this.CurrentPM = myResult1.Result;
                    _this.CurrentSession.StopBusyIndicator();
                    if (_this.CurrentPM.ForwarderPartnerId != _this.SourceEntity.ForwarderPartnerId) {
                        var msg = new MessageWindow_1.MessageWindow();
                        msg.Show("You can't connect to shipment with different partner.");
                    }
                    else {
                        _this.CompleteChoosingEntity();
                    }
                });
            }
        });
    };
    ConnectButtonsListTemplate.prototype.CompleteChoosingEntity = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.Title = "Confirm";
        confirmWindow.YesButtonText = "Connect";
        confirmWindow.NoButtonText = "Cancel";
        confirmWindow.Show("Importer Shipment " + this.SourceEntity.ShipmentNumber + " (Order " + this.SourceEntity.CustomerReference1 + " )" + " will be connected to forwarder shipment " + this.rowData['ForwarderShipmentNumber']);
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.CurrentSession.StartBusyIndicator("Loading ..");
                var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Shipment"; })[0];
                _this._documentsFilingExtendedPMService.GetLogBoxConnectedDocs(_this.SourceEntity.Id, _this.rowData['Id'], ObjectTable.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.CurrentPM.ShipperName)) {
                        _this.CurrentPM.ShipperName = _this.SourceEntity.ShipperName;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.CurrentPM.CustomerReference1)) {
                        _this.CurrentPM.CustomerReference1 = _this.SourceEntity.CustomerReference1;
                        _this.CurrentPM.ShipperReference1 = _this.SourceEntity.CustomerReference1;
                    }
                    else if (Tools_1.AppTool.IsNullOrEmpty(_this.CurrentPM.CustomerReference2)) {
                        _this.CurrentPM.CustomerReference2 = _this.SourceEntity.CustomerReference2;
                        _this.CurrentPM.ShipperReference2 = _this.SourceEntity.CustomerReference2;
                    }
                    _this._ShipmentPMService.update(_this.CurrentPM).subscribe(function (myResult) {
                        _this._ShipmentPMService.get(_this.SourceEntity.Id).subscribe(function (myResult) {
                            if (!myResult.HasError) {
                                myResult.Result.IsCancelled = true;
                                _this._ShipmentPMService.update(myResult.Result).subscribe(function (myResult) {
                                    _this.CurrentSession.StopBusyIndicator();
                                    _this.CurrentSession.FireEvent({ Name: 'ReloadShipments' });
                                    _this.CurrentSession.CloseCurrentWindow();
                                });
                            }
                        });
                    });
                    //var count = 0;
                    //res.Result.forEach((docin) => {
                    //    count++;
                    //    docin.EntityId = this.rowData['Id'];
                    //    this._documentsFilingPMService.update(docin).subscribe(myResult => {
                    //        if (count == res.Result.length) {
                    //            if (AppTool.IsNullOrEmpty(this.CurrentPM.ShipperName)) {
                    //                this.CurrentPM.ShipperName = this.SourceEntity.ShipperName;
                    //            }
                    //            if (AppTool.IsNullOrEmpty(this.CurrentPM.CustomerReference1)) {
                    //                this.CurrentPM.CustomerReference1 = this.SourceEntity.CustomerReference1;
                    //                this.CurrentPM.ShipperReference1 = this.SourceEntity.CustomerReference1;
                    //            }
                    //            else if (AppTool.IsNullOrEmpty(this.CurrentPM.CustomerReference2)) {
                    //                this.CurrentPM.CustomerReference2 = this.SourceEntity.CustomerReference2;
                    //                this.CurrentPM.ShipperReference2 = this.SourceEntity.CustomerReference2;
                    //            }
                    //            this._ShipmentPMService.update(this.CurrentPM).subscribe(myResult => {
                    //                this._ShipmentPMService.get(this.SourceEntity.Id).subscribe(myResult => {
                    //                    if (!myResult.HasError) {
                    //                        myResult.Result.IsCancelled = true;
                    //                        this._ShipmentPMService.update(myResult.Result).subscribe(myResult => {
                    //                            this.CurrentSession.StopBusyIndicator();
                    //                            this.CurrentSession.FireEvent({ Name: 'ReloadShipments' });
                    //                            this.CurrentSession.CloseCurrentWindow();
                    //                        });
                    //                    }
                    //                });
                    //            });
                    //        }
                    //    });
                    //});
                    //if (res.Result.length == 0) {
                    //    if (AppTool.IsNullOrEmpty(this.CurrentPM.ShipperName)) {
                    //        this.CurrentPM.ShipperName = this.SourceEntity.ShipperName;
                    //    }
                    //    if (AppTool.IsNullOrEmpty(this.CurrentPM.CustomerReference1)) {
                    //        this.CurrentPM.CustomerReference1 = this.SourceEntity.CustomerReference1;
                    //    }
                    //    else if (AppTool.IsNullOrEmpty(this.CurrentPM.CustomerReference2)) {
                    //        this.CurrentPM.CustomerReference2 = this.SourceEntity.CustomerReference2;
                    //    }
                    //    this._ShipmentPMService.update(this.CurrentPM).subscribe(myResult => {
                    //        this._ShipmentPMService.get(this.SourceEntity.Id).subscribe(myResult => {
                    //            if (!myResult.HasError) {
                    //                myResult.Result.IsCancelled = true;
                    //                this._ShipmentPMService.update(myResult.Result).subscribe(myResult => {
                    //                    this.CurrentSession.StopBusyIndicator();
                    //                    this.CurrentSession.FireEvent({ Name: 'ReloadShipments' });
                    //                    this.CurrentSession.CloseCurrentWindow();
                    //                });
                    //            }
                    //        });
                    //    });
                    //}
                }, function (error) {
                    //var dd: Response = error;
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
            else {
            }
        });
    };
    ConnectButtonsListTemplate = __decorate([
        core_1.Component({
            template: "<table>\n                <tr style=\"height:1px;\"> \n                    <td>\n                        <div style=\"height:30px;\"> \n                            <button class=\"Button\" (click)=\"ChooseButtonClicked()\" style=\"width:80px;float: right;margin:4px;\">Choose</button>\n                        </div>\n                    </td> \n                </tr>\n            </table>\n            "
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ConnectButtonsListTemplate);
    return ConnectButtonsListTemplate;
}());
exports.ConnectButtonsListTemplate = ConnectButtonsListTemplate;
//# sourceMappingURL=ConnectButtonsListTemplate.js.map