import {Component, ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ShipmentPMService} from '../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {DocumentsFilingExtendedPMService} from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';

@Component({

    template: `<table>
                <tr style="height:1px;"> 
                    <td>
                        <div style="height:30px;">

                            <button class="Button" (click)="EditButtonClicked()" style="width:40px;float: right;margin:4px;">Edit</button>
                            <button class="Button" (click)="RemoveTasksButtonClicked()" style="width:85px;float: right;margin:4px;">Remove Tasks</button>
                            <button class="RedButton" *ngIf="ShowRenewButtons == true"  (click)="RenewButtonClicked()" style="width:50px;float: right;margin:4px;">Renew</button>
                        </div>
                    </td>
                    
                </tr>
            </table>
            `
})

export class RemoveTasksButtonListTemplate {
 
    public rowData: any;
    public fieldName: any;
    public Source: any;
    public ConnectBtn: string = "Connect";
    public Width: number = 57;
    public ShowButtons: boolean = true;
    public HasSharedDocs: boolean = true;
    public _ShipmentPMService: ShipmentPMService;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    public ShowRenewButtons: boolean = false;
    constructor(private CD: ChangeDetectorRef) {
        this._ShipmentPMService = new ShipmentPMService();
        //if (SessionLocator.PrivateLableSettings) {
        //    this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        //    this.Width = 80;
        //    this.ConnectBtn = SessionLocator.PrivateLableSettings.PrivateLabelShortName + " Connect";
        //}
    }

    setVariables(rowData: any, fieldName: string) {
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
    } 

    RemoveTasksButtonClicked() {
        SessionLocator.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Confirm Deletion";
        confirmWindow.Show("Are you sure you want to cancel tasks for this shipment ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                SessionLocator.CurrentSession.StartBusyIndicator("Loading ..")
                this._ShipmentPMService.RemoveShipmentTasks(this.rowData.Id).subscribe(myResult => {
                    if (!myResult.HasError) {
                        SessionLocator.CurrentSession.StopBusyIndicator();
                        SessionLocator.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                        SessionLocator.CurrentSession.FireEvent({ Name: 'CustomReloadShipments' });
                        
                    }
                });
            }

            else {

            }
        });
       
    }

    EditButtonClicked() {
        SessionLocator.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        SessionLocator.CurrentSession.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.get(this.rowData.Id).subscribe(myResult => {
            if (!myResult.HasError) {
                SessionLocator.CurrentSession.StopBusyIndicator();
                var newWindow = new LogitudeWindow();
                newWindow.Width = 600;
                newWindow.Height = 150;
                newWindow.Title = "Edit Shipment";
                var windowArgs: any = {};
                windowArgs.IsNew = false;
                windowArgs.EntityPm = myResult.Result
                newWindow.WindowArgs = windowArgs;
                //newWindow.Add(control);
                //if (SessionLocator.PrivateLableSettings) {
                //    newWindow.Show('./Shipment/Components/Logbox/AddEditPrivateLabelShipmentComponent');
                //}
                //else {
                newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/EditLogBoxShipmentComponent');
                //}
                newWindow.WindowClosed.subscribe(($event: any) => {
                    SessionLocator.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                    if ($event == "MyShipmentAdded") {
                        SessionLocator.CurrentSession.FireEvent({ Name: 'CustomReloadShipments' });
                    }
                });
            }
        });
    }

    RenewButtonClicked() {
        var newWindow = new LogitudeWindow();
        newWindow.Width = 600;
        newWindow.Height = 230;

        var forwarderShipmentNumber = this.rowData['ForwarderShipmentNumber'];
        if (!forwarderShipmentNumber) forwarderShipmentNumber = "";

        newWindow.Title = "נדרש תצהיר עבור תיק עמילות" + " " + forwarderShipmentNumber;
        var windowArgs: any = {};
        
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
        newWindow.WindowClosed.subscribe(($event: any) => {
            SessionLocator.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
            if ($event == "DepositionRequest") {
                SessionLocator.CurrentSession.FireEvent({ Name: 'CustomReloadShipments' });
            }
        });

    }

}
