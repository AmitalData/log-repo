import {Component, ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ShipmentPMService} from '../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {DocumentsFilingExtendedPMService} from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {ShipmentAdditionalCloudDataService} from '../../../Shipment/Services/Others/ShipmentAdditionalCloudDataService';
import {AppTool} from '../../../Infrastructure/Tools';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({

    template: `<table>
                <tr style="height:1px;"> 
                    <td>
                        <div style="height:30px;">
                            <button class="Button" (click)="EditButtonClicked()" style="width:57px;margin:4px;float: right">Edit</button>
                            <button *ngIf="ShowRemoveButton == true" class="Button" (click)="RemoveTasksButtonClicked()" style="width:85px;float: right;margin:4px;">Remove Tasks</button>
                             <button *ngIf="ShowButtons == true" class="Button" (click)="ApproveButtonClicked()" style="width:122px;;float: right;margin:4px;">Declaration Approval</button>
                            <button  *ngIf="ShowRenewButtons == true" class="RedButton"   (click)="RenewButtonClicked()" style="width:50px;float: right;margin:4px;">Renew</button>
                        </div>
                    </td>
                </tr>
            </table>
            `
})

export class ApprovePaymentButtonListTemplate {
    //
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
    public ShowRemoveButton: boolean = true;
    public HasSharedDocs: boolean = true;
    public _ShipmentPMService: ShipmentPMService;
    public _ShipmentAdditionalCloudDataService: ShipmentAdditionalCloudDataService;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    public ShowRenewButtons: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    Language: string = 'HB';
    constructor(private CD: ChangeDetectorRef) {
        this._ShipmentPMService = new ShipmentPMService();
        this._ShipmentAdditionalCloudDataService = new ShipmentAdditionalCloudDataService();
        this.Language = SessionLocator.TenantPM.Language;
        //if (SessionLocator.PrivateLableSettings) {
        //    this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        //    this.Width = 80;
        //    this.ConnectBtn = SessionLocator.PrivateLableSettings.PrivateLabelShortName + " Connect";
        //}
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData; 
        this.ShowRenewButtons = (this.rowData['IsDepositionRequired'] == true);
        if (SessionLocator.PrivateLableSettings) {
            this.ShowButtons = (this.rowData['IsImporterApprovalRequried'] == true);// && AppTool.IsNullOrEmpty(this.rowData['ApprovedByUserName'])
            this.ShowRemoveButton = (this.rowData['IsDigitalSignRequired'] == true || this.rowData['IsRequestedDocuments'] == true || this.rowData['IsDepositionRequired'] == true);
            //if (SessionLocator.PrivateLableSettings) {
            //    this._documentsFilingExtendedPMService.IsEntityHasSharedDocs(this.rowData['Id'], SessionLocator.Tenant).subscribe(res => {
            //        if (res.Result == false) {
            //            this.HasSharedDocs = false;
            //        }
            //    });
            //}
        }
        //this.fieldName = fieldName;
        //var myService: WebFreightDomainService = new WebFreightDomainService();
        //if (rowData['PartnerLogoId']){
        //    myService.getHypridPartnerLogo(rowData['PartnerLogoId']).subscribe(myResult => {
        //        this.Source = "data:image/JPEG;base64," + myResult;
        //        this.CD.detectChanges(); 
        //    });
        //}
    } 
    ApproveButtonClicked() {
        this.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        //this.CurrentSession.SessionEvent.emit("DisableBusyIndicator");
        //this.CurrentSession.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.get(this.rowData.Id).subscribe(myResult => {
            if (!myResult.HasError) {
                this._ShipmentAdditionalCloudDataService.get(this.rowData.Id).subscribe(AdditionalResult => {
                    //this.CurrentSession.StopBusyIndicator();
                    this._entityResourceService.getEntityResourceByTableName("Shipment").subscribe(response1 => {
                        var newWindow = new LogitudeWindow();
                        newWindow.Width = 705;
                        newWindow.Height = 700;
                        if (this.Language == 'HB') {
                            newWindow.RTL = true;
                        }
                        else {
                            newWindow.RTL = false;
                        }
                        //newWindow.CustomTitleIcon = "data:image/JPEG;base64," + SessionLocator.PrivateLableSettings.SmallLogo;
                        newWindow.Title = TextCodeTranslator.Translate("Shipment.O.PLApprovalWindowTitle");//"אישור היבואן להגשת הצהרת יבוא למכס";
                        var windowArgs: any = {};
                        //windowArgs.IsNew = false;
                        windowArgs.EntityPm = myResult.Result
                        windowArgs.AdditionalData = AdditionalResult.Result
                        newWindow.WindowArgs = windowArgs;
                        //newWindow.Add(control); 
                        newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/PrivateLabelApprovePaymentComponent');
                        newWindow.WindowClosed.subscribe(($event: any) => {
                            this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                            //if ($event == "MyShipmentAdded") {
                            //    this.CurrentSession.FireEvent({ Name: 'ReloadShipments' });
                            //}
                        });
                    });
                });
                
            }
        });
    }

    RemoveTasksButtonClicked() {
        this.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Confirm Deletion";
        confirmWindow.Show("Are you sure you want to cancel tasks for this shipment ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.StartBusyIndicator("Loading ..")
                this._ShipmentPMService.RemoveShipmentTasks(this.rowData.Id).subscribe(myResult => {
                    if (!myResult.HasError) {
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                        this.CurrentSession.FireEvent({ Name: 'CustomReloadShipments' });

                    }
                });
            }

            else {

            }
        });

    }

    RenewButtonClicked() {
        var newWindow = new LogitudeWindow();
        newWindow.Width = 600;
        newWindow.Height = 230;
        var forwarderShipmentNumber = this.rowData['ForwarderShipmentNumber'];
        if (AppTool.IsNullOrEmpty(forwarderShipmentNumber)) forwarderShipmentNumber = "";

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
            this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
            if ($event == "DepositionRequest") {
                this.CurrentSession.FireEvent({ Name: 'CustomReloadShipments' });
            }
        });

    }

    EditButtonClicked() {
        this.CurrentSession.PseventRowSelectEvent.emit("PreventLogBoxSelect");
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.get(this.rowData.Id).subscribe(myResult => {
            if (!myResult.HasError) {
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
