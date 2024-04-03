import {Component, ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ShipmentPMService} from '../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {DocumentsFilingExtendedPMService} from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { AppTool } from '../../../Infrastructure/Tools';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';

@Component({

    template: `<table *ngIf="ShowButtons == true">
                <tr style="height:1px;"> 
                    <td>
                        <div style="height:30px;"> 
                            <button class="Button" (click)="EditButtonClicked()" style="width:57px;margin:4px;">Edit</button>
                        </div>
                    </td>
                </tr>
            </table>
            `
})

export class EditShipmentButtonListTemplate {
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
    public Width: number = 57;
    public ShowButtons: boolean = true;
    public HasSharedDocs: boolean = true;
    public _ShipmentPMService: ShipmentPMService;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        this._ShipmentPMService = new ShipmentPMService();
        if (SessionLocator.PrivateLableSettings) {
            this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
            this.Width = 80;
            this.ConnectBtn = SessionLocator.PrivateLableSettings.PrivateLabelShortName + " Connect";
        }
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData; 
        if (SessionLocator.PrivateLableSettings) {
            this.ShowButtons = this.rowData['StatusName'].toLowerCase() == "in progress" ? false : true;
            if (SessionLocator.PrivateLableSettings) {
                this._documentsFilingExtendedPMService.IsEntityHasSharedDocs(this.rowData['Id'], SessionLocator.Tenant).subscribe((res:any) => {
                    if (res.Result == false) {
                        this.HasSharedDocs = false;
                    }
                });
            }
        }
        this.fieldName = fieldName;
        if (AppTool.IsNullOrEmpty(this.rowData['ForwarderShipmentNumber']) && this.fieldName == "EditShipmentButtonListTemplate" + "All Shipments") {
            this.ShowButtons = false;
        }

        //var myService: WebFreightDomainService = new WebFreightDomainService();
        //if (rowData['PartnerLogoId']){
        //    myService.getHypridPartnerLogo(rowData['PartnerLogoId']).subscribe((myResult:any) => {
        //        this.Source = "data:image/JPEG;base64," + myResult;
        //        this.CD.detectChanges(); 
        //    });
        //}
    } 

    EditButtonClicked() {
        this.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.get(this.rowData.Id).subscribe((myResult:any) => {
            if (!myResult.HasError) {
                MixPanelLocator.Action({ ProjectName:"LogBox", ActionName: "Edit Shipment" });
                this.CurrentSession.StopBusyIndicator();
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
                    this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                    if ($event == "MyShipmentAdded") {
                        this.CurrentSession.FireEvent({ Name: 'CustomReloadShipments' });
                    }
                });
            }
        });
    }

}
