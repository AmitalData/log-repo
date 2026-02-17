declare var window: any;
import {Component, ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ShipmentPMService} from '../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {DocumentsFilingExtendedPMService} from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {DocumentsFilingPMService} from '../../../Common/Services/StandardPMs/DocumentsFilingPMService'
import {AppTool} from '../../../Infrastructure/Tools';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';

@Component({

    template: `<table>
                <tr style="height:1px;"> 
                    <td>
                        <div style="height:30px;"> 
                            <button class="Button" (click)="ChooseButtonClicked()" style="width:80px;float: right;margin:4px;">Choose</button>
                        </div>
                    </td> 
                </tr>
            </table>
            `
})

export class ConnectButtonsListTemplate {

    public rowData: any;
    public fieldName: any;
    public SourceId: any;
    public Source: any;
    public CurrentPM: any;
    public _ShipmentPMService: ShipmentPMService;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    public _documentsFilingPMService: DocumentsFilingPMService;
    SourceEntity: any;
    Subscribed: any = false;
    HasSharedDocs: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        this._ShipmentPMService = new ShipmentPMService();
        this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        this._documentsFilingPMService = new DocumentsFilingPMService();
        //this.CurrentSession.SessionEvent.subscribe(($event: any) => { 
        //    if ($event.Name == "SourceEntity" && $event.EntityId == this.rowData["Id"]) { 
        //        this.SourceEntity = $event.Entity;
        //        this.CompleteChoosingEntity();
        //    }
        //}); 
    }

    setVariables(rowData: any, fieldName: string) {
        this.SourceId = fieldName;
        this.rowData = rowData;
    }

    ChooseButtonClicked() {

        this.CurrentSession.StartBusyIndicator("Loading ..");
        this._ShipmentPMService.get(this.SourceId).subscribe(myResult => {
            if (!myResult.HasError) {
                this.SourceEntity = myResult.Result;
                this._ShipmentPMService.get(this.rowData['Id']).subscribe(myResult1 => {
                    this.CurrentPM = myResult1.Result;
                    this.CurrentSession.StopBusyIndicator();
                    if (this.CurrentPM.ForwarderPartnerId != this.SourceEntity.ForwarderPartnerId) {
                        var msg = new MessageWindow();
                        msg.Show("You can't connect to shipment with different partner.");
                    }
                    else {
                        this.CompleteChoosingEntity();
                    }
                });
            }
        });

    }

    CompleteChoosingEntity() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.Title = "Confirm";
        confirmWindow.YesButtonText = "Connect";
        confirmWindow.NoButtonText = "Cancel";
        confirmWindow.Show("Importer Shipment " + this.SourceEntity.ShipmentNumber + " (Order " + this.SourceEntity.CustomerReference1 + " )" + " will be connected to forwarder shipment " + this.rowData['ForwarderShipmentNumber']);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.StartBusyIndicator("Loading ..")
                var ObjectTable = window.ObjectTables.filter(x => x.Name === "Shipment")[0];
                this._documentsFilingExtendedPMService.GetLogBoxConnectedDocs(this.SourceEntity.Id, this.rowData['Id'],ObjectTable.Id,SessionLocator.Tenant).subscribe(res => {
                    if (AppTool.IsNullOrEmpty(this.CurrentPM.ShipperName)) {
                        this.CurrentPM.ShipperName = this.SourceEntity.ShipperName;
                    }
                    if (AppTool.IsNullOrEmpty(this.CurrentPM.CustomerReference1)) {
                        this.CurrentPM.CustomerReference1 = this.SourceEntity.CustomerReference1;
                        this.CurrentPM.ShipperReference1 = this.SourceEntity.CustomerReference1;

                    }
                    else if (AppTool.IsNullOrEmpty(this.CurrentPM.CustomerReference2)) {
                        this.CurrentPM.CustomerReference2 = this.SourceEntity.CustomerReference2;
                        this.CurrentPM.ShipperReference2 = this.SourceEntity.CustomerReference2;
                    }

                    this._ShipmentPMService.update(this.CurrentPM).subscribe(myResult => {
                        this._ShipmentPMService.get(this.SourceEntity.Id).subscribe(myResult => {
                            if (!myResult.HasError) {
                                myResult.Result.IsCancelled = true;
                                this._ShipmentPMService.update(myResult.Result).subscribe(myResult => {
                                    this.CurrentSession.StopBusyIndicator();
                                    this.CurrentSession.FireEvent({ Name: 'ReloadShipments' });
                                    this.CurrentSession.CloseCurrentWindow();
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

                }, error => {
                    //var dd: Response = error;
                    this.CurrentSession.StopBusyIndicator();
                });
            }

            else {

            }
        });
    }

}
