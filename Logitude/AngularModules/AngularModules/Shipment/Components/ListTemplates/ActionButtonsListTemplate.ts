import {Component, ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ShipmentPMService} from '../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {DocumentsFilingExtendedPMService} from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';

@Component({

    template: `<table *ngIf="ShowButtons == true">
                <tr style="height:1px;"> 
                    <td>
                        <div style="height:30px;">
                            <button class="RedButton" (click)="CancelButtonClicked()" [style.width.px]="Width1" style="float: right;margin:4px;">Cancel</button>
                            <button *ngIf="!IsPrivateLabel || IsDSV" class="Button" (click)="ConnectButtonClicked()" [style.width.px]="Width" style="float: right;margin:4px;">{{ConnectBtn}}</button>
                            <button class="Button" (click)="EditButtonClicked()" [style.width.px]="Width1" style="float: right;margin:4px;">Edit</button>
                        </div>
                    </td>
                </tr>
            </table>
            `
})

export class ActionButtonsListTemplate {
    //<button class="Button" *ngIf="this.HasSharedDocs == false" title="No documents were shared with agent yet." [style.width.px]="Width" style="float: right;margin:4px; opacity:0.5;">{{ConnectBtn}}</button>
    // <td *ngIf="this.HasSharedDocs == false" >
    //<div style="height:30px;" >
    //    <span style="color:red;" > No documents were shared with agent yet.</span>
    //        < /div>
    //        < /td>
    public rowData: any;
    public fieldName: any;
    public Source: any;
    public ConnectBtn: string = "Connect";
    public Width1: number = SessionLocator.PrivateLableSettings ? 70 : 57;
    public Width: number = 57;
    public ShowButtons: boolean = true;
    public HasSharedDocs: boolean = true;
    public _ShipmentPMService: ShipmentPMService;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsDSV: boolean = false;
    public IsPrivateLabel: boolean = false; 
    constructor(private CD: ChangeDetectorRef) {
        this._ShipmentPMService = new ShipmentPMService();
        if (SessionLocator.PrivateLableSettings) {
            this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
            this.Width = 80;
            this.IsPrivateLabel = true; 
            this.IsDSV = SessionLocator.PrivateLableSettings.PrivateLabelDomain.toLowerCase().indexOf("dsv") > -1; 
            this.ConnectBtn = SessionLocator.PrivateLableSettings.PrivateLabelShortName + " Connect";
        }
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.ShowButtons = this.rowData['StatusName'].toLowerCase() == "in progress" ? false : true;
        if (SessionLocator.PrivateLableSettings) {
            this._documentsFilingExtendedPMService.IsEntityHasSharedDocs(this.rowData['Id'], SessionLocator.Tenant).subscribe((res: any) => {
                if (res.Result == false) {
                    this.HasSharedDocs = false;
                }
            });
        }

     

        //this.fieldName = fieldName;
        //var myService: WebFreightDomainService = new WebFreightDomainService();
        //if (rowData['PartnerLogoId']){
        //    myService.getHypridPartnerLogo(rowData['PartnerLogoId']).subscribe((myResult:any) => {
        //        this.Source = "data:image/JPEG;base64," + myResult;
        //        this.CD.detectChanges(); 
        //    });
        //}
    }

    CancelButtonClicked() {
        //this.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.get(this.rowData.Id).subscribe((myResult:any) => {
            if (!myResult.HasError) {
                this.CurrentSession.StopBusyIndicator();
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Title = "Confirm Deletion";
                confirmWindow.Show("Are you sure you want to cancel this Shipment ?");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.CurrentSession.StartBusyIndicator("Loading ..")
                        myResult.Result.IsCancelled = true;
                        this._ShipmentPMService.update(myResult.Result).subscribe((myResult:any) => {
                            this.CurrentSession.StopBusyIndicator();
                            this.CurrentSession.FireEvent({ Name: 'ReloadShipments' });
                            this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                        });
                    }

                    else {

                    }
                });
            }
        });
    }

    ConnectButtonClicked() {
        //this.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.get(this.rowData.Id).subscribe((myResult:any) => {
            if (!myResult.HasError) {
                if (SessionLocator.PrivateLableSettings) {
                    this._documentsFilingExtendedPMService.IsEntityHasSharedDocs(this.rowData['Id'], SessionLocator.Tenant).subscribe((res:any) => {
                        if (res.Result == false) {
                            this.HasSharedDocs = false;
                        }
                        else {
                            this.HasSharedDocs = true;
                        }
                        this.CurrentSession.StopBusyIndicator();
                        var newWindow = new LogitudeWindow();
                        newWindow.Width = 1050;
                        newWindow.Height = 700;
                        if (SessionLocator.PrivateLableSettings) {
                            newWindow.Title = "Connect/Create new shipment in " + SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                        }
                        else {
                            newWindow.Title = "Connect To Agent Shipment";
                        }

                        var windowArgs: any = {};
                        windowArgs.SourceEntity = myResult.Result;//this.rowData;
                        windowArgs.HasSharedDocs = this.HasSharedDocs;
                        newWindow.WindowArgs = windowArgs;
                        newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/ForwarderShipmentsComponent');
                        newWindow.WindowClosed.subscribe(($event: any) => {
                            this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                        });
                    });
                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                    var newWindow = new LogitudeWindow();
                    newWindow.Width = 1050;
                    newWindow.Height = 700;
                    if (SessionLocator.PrivateLableSettings) {
                        newWindow.Title = "Connect/Create new shipment in " + SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                    }
                    else {
                        newWindow.Title = "Connect To Agent Shipment";
                    }

                    var windowArgs: any = {};
                    windowArgs.SourceEntity = myResult.Result;//this.rowData;
                    windowArgs.HasSharedDocs = this.HasSharedDocs;
                    newWindow.WindowArgs = windowArgs;
                    newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/ForwarderShipmentsComponent');
                    newWindow.WindowClosed.subscribe(($event: any) => {
                        this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                    });
                }
            }
        });
        
    }

    EditButtonClicked() {

        let isExportAirShipment = this.rowData['DirectionId']== 'E';
        if (isExportAirShipment) return; 
        this.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        this.CurrentSession.StartBusyIndicator("Loading ..."); 
        this._ShipmentPMService.get(this.rowData.Id).subscribe((myResult:any) => {
            if (!myResult.HasError) {
                this.CurrentSession.StopBusyIndicator();  
                var newWindow = new LogitudeWindow();
                newWindow.Width = 600;
                newWindow.Height = 350;
                newWindow.Title = "Edit Shipment";
                var windowArgs: any = {};
                windowArgs.IsNew = false;
                windowArgs.EntityPM = myResult.Result 
                newWindow.WindowArgs = windowArgs;
                if ((SessionLocator.PrivateLableSettings)) {
                    newWindow.Height = this.IsDSV ?  376 : 420;
                    newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditPrivateLabelShipmentComponent');
                }
                else {
                    newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditImporterShipmentComponent');
                }
                newWindow.WindowClosed.subscribe(($event: any) => {
                    this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                    if ($event == "MyShipmentAdded") {
                        this.CurrentSession.FireEvent({ Name: 'ReloadShipments' });
                    }
                });
            }
        });
    }
}
