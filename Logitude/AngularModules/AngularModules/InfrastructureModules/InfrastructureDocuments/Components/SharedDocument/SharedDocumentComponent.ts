
declare var window: any;
import {Component, OnInit, EventEmitter}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentTypePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ShareDocument} from '../../../../Common/DataContracts/ShipmentShareDocumentsData';
import {ShipmentShareDocumentsData} from '../../../../Common/DataContracts/ShipmentShareDocumentsData';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {AgentSharedDocumentExtendedService} from '../../../../Common/Services/ExtendedPMs/AgentSharedDocumentExtendedService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {AttachmentsList} from '../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SharedDocumentHelper} from '../../../../Infrastructure/Helpers/SharedDocumentHelper';
import { GeneralEmailSender } from '../../../../Infrastructure/Helpers/GeneralEmailSender';

@Component({
    
    selector: 'SharedDocumentComponent',
    templateUrl: './SharedDocumentComponent.html',
    providers: [DocumentTypePMExtendedService , AgentSharedDocumentExtendedService],
})

export class SharedDocumentComponent implements OnInit {
    EntityPM: ShipmentPM;
    AgentName: string = "";
    Master: string = "";
    ShipmentNumber: string;
    ShareDocumentSelected: ShareDocument;
    ShipmentShareDocumentsDataLists: ShipmentShareDocumentsData[];
    ObjectTableId: string;
    IsShowMessageNoDocument: boolean = false;
    IsShareDocumentsViaEmail: boolean = false;
    AttachmentsLists: AttachmentsList[];
    OnCloseSharedWithAgentsEvent = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    ShareDocumentsViaEmailDocumentTypeCode = "SDVE"; 
    public documentTypePMExtendedService: DocumentTypePMExtendedService = new DocumentTypePMExtendedService();

    constructor(public _documentTypePMExtendedService: DocumentTypePMExtendedService, public _agentSharedDocumentExtendedService: AgentSharedDocumentExtendedService) {


    }

    ngOnInit(

    ) {


    }


    Mode: string = "";
    OkButtonLable: string;
    SetWindowArgs(args: any) {
        this.IsShareDocumentsViaEmail = args.ShareDocumentsViaEmail;
        this.Mode = args.Mode;
        this.EntityPM = args.EntityPM;
        this.ObjectTableId = window.ObjectTables.filter(d => d.Name == "Shipment")[0].Id;
        if (this.EntityPM) {
            this.AgentName = this.EntityPM.AgentName;
            this.Master = this.EntityPM.LongMaster;
            this.ShipmentNumber = this.EntityPM.ShipmentNumber;
            this.LoadData();

        }

        this.OkButtonLable = (this.Mode == "Attachment" || this.IsShareDocumentsViaEmail) ? "OK" : "Share";
        if (this.Mode == "Attachment") {
            this.OnCloseSharedWithAgentsEvent = args.OnCloseSharedWithAgentsEvent;
            this.AttachmentsLists = [];
        }



    }

  

    LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ShipmentShareDocumentsDataLists = [];
        this._documentTypePMExtendedService.GetShareDocumentByObjectTableAndEntityIdAndshipmentLevel(this.EntityPM.Id, this.EntityPM.AgentId, this.EntityPM.ShipmentNumber, this.ObjectTableId, this.EntityPM.ShipmentLevelCode, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                var myList = pmResponse.Result;
                this.ShipmentShareDocumentsDataLists = myList;
                if (this.ShipmentShareDocumentsDataLists.length == 0) {
                    this.IsShowMessageNoDocument = true;
                } else {

                    this.ShipmentShareDocumentsDataLists = this.ShipmentShareDocumentsDataLists.filter(d => d.ShareDocuments && d.ShareDocuments.length > 0);
                }

                if (this.ShipmentShareDocumentsDataLists) {
                    this.ShipmentShareDocumentsDataLists.forEach((item) => {
                        if (item.ShareDocuments && !AppTool.IsNullOrEmpty(item.AgentSharedManifestRef)) {
                            item.ShareDocuments.forEach((doc) => {
                                doc.Included = doc.IsReady;

                            });

                            this.SortItemSource(item.ShareDocuments);
                        }
                       


                    });
                }
                    
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }


    OnmMouseOver(item: ShareDocument) {

        this.ShipmentShareDocumentsDataLists.forEach((item) => {
            item.ShareDocuments.forEach((shareDocument) => {
                shareDocument.VisibleUploadButton = false;
                shareDocument.VisibleBliudDocumentButton = false;
                shareDocument.VisibleViewButton = false;
            });

        });

        item.VisibleViewButton = true;
        if (item.DirectionCode == "I") {
            item.VisibleUploadButton = true;
        }
        else {

            item.VisibleBliudDocumentButton = true;
        }
        if (this.ShareDocumentSelected) {

            this.ShareDocumentSelected.VisibleViewButton = true; 

            if (this.ShareDocumentSelected.DirectionCode == "I") { this.ShareDocumentSelected.VisibleUploadButton = true; }
            else { this.ShareDocumentSelected.VisibleBliudDocumentButton = true; }
        }



    }
    OnmMouseleave(item: ShareDocument) {
        this.ShipmentShareDocumentsDataLists.forEach((item) => {
            item.ShareDocuments.forEach((shareDocument) => {
                shareDocument.VisibleUploadButton = false;
                shareDocument.VisibleBliudDocumentButton = false;
                shareDocument.VisibleViewButton = false;
            });

        });

        if (this.ShareDocumentSelected) {
            this.ShareDocumentSelected.VisibleViewButton = true;

            if (this.ShareDocumentSelected.DirectionCode == "I") { this.ShareDocumentSelected.VisibleUploadButton = true; }
            else { this.ShareDocumentSelected.VisibleBliudDocumentButton = true; }
        }

    }


    ViewButtonClick(item: ShareDocument) {
        if (!AppTool.IsNullOrEmpty(item.SecurityId)) {
            if (item.DirectionCode == "I") {
                DownloadManager.DownloadPage("", item.SecurityId);
            } else   DownloadManager.DownloadPage(item.DocumentId, item.SecurityId);
           

        } else {
            var messageWindow: MessageWindow = new MessageWindow();

            if (item.DirectionCode == "I") {
                messageWindow.Show("Please upload a document first");
            } else  messageWindow.Show("Please Bliud a document first");
        }
    }


    sharedDocumentHelper: SharedDocumentHelper;
    BuildButtonClick(item: ShareDocument) {
        if (!this.sharedDocumentHelper || (this.sharedDocumentHelper && !this.sharedDocumentHelper.IsBuildDocumentRunning)) {
            this.sharedDocumentHelper = new SharedDocumentHelper();
            this.sharedDocumentHelper.BuildDocument(item);
        }
    }

    UploadButtonClick(item: ShareDocument, shipmentShareDocumentsData: ShipmentShareDocumentsData) {

        if (!this.sharedDocumentHelper || (this.sharedDocumentHelper && !this.sharedDocumentHelper.IsUploadDocumentRunning)) {
            this.sharedDocumentHelper = new SharedDocumentHelper();
            this.sharedDocumentHelper.UploadButton(item, shipmentShareDocumentsData, this);
        }
    }
   

    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    }

    SelectedShipmentShareDocumentsDataLists: ShipmentShareDocumentsData[] = []; 

    private GetShareDocumentsViaEmailAction() {
        var attachmentsList = this.GetSelectedAttachmentsList(); 
        this.ShowSendControlBasedDocumentType(attachmentsList); 
    }

    private ShowSendControlBasedDocumentType(attachmentsList: AttachmentsList[]) {
        this.documentTypePMExtendedService.GetDoesDocumentTypeCodeExist(this.ShareDocumentsViaEmailDocumentTypeCode, SessionLocator.Tenant).subscribe((res: any) => {
            var serviceResponse: ServiceResponse = res;
            if (!serviceResponse.HasError && serviceResponse.Result == false) {
                this.ShowValidationMessage("Contact your administrator");
            }
            if (!serviceResponse.HasError && serviceResponse.Result == true) {
                this.ShowSendControl(attachmentsList);
            }
        });
    }

    private ShowValidationMessage(messsage: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(messsage);
    }

    private ShowSendControl(attachmentsList: AttachmentsList[]) {
        var generalEmailSender = new GeneralEmailSender("Shipment", this.ShareDocumentsViaEmailDocumentTypeCode, this.EntityPM.Id, this.EntityPM.ShipmentNumber, this.EntityPM.CustomerId, "", "", "", attachmentsList, "", this.EntityPM, false, "QEMO");
        generalEmailSender.ShowFullSendControll();
    }

    private GetSelectedAttachmentsList() {
        var attachmentsList = new Array<AttachmentsList>();
        this.ShipmentShareDocumentsDataLists.forEach((item) => {
            if (!AppTool.IsNullOrEmpty(item.AgentSharedManifestRef) && item.ShareDocuments.filter(d => d.Included == true).length > 0) { 
                this.BuildShareDocumentAtttachmentsList(item, attachmentsList);
            }
        });
        return attachmentsList;
    }


    private BuildShareDocumentAtttachmentsList(item: ShipmentShareDocumentsData, attachmentsList: AttachmentsList[]) {
        item.ShareDocuments.filter(d => d.Included).forEach((doc) => {
            var item = new AttachmentsList();
            item.Id = doc.DocumentId;
            item.DocumentFilingId = doc.DocumentsFilingId;
            item.Tenant = SessionLocator.Tenant;
            item.FileSize = doc.FileSize;
            item.FileExtension = doc.Extension;
            item.DocumentTypeCopyNameWithDocumentTypeName = doc.DocumentTypeName;
            item.ShowRemoveLink = true;
            attachmentsList.push(item);
        });
    }

    SaveButtonClicked() {

        if (this.IsShareDocumentsViaEmail) {
            this.GetShareDocumentsViaEmailAction();  
             return; 
         }
         
            this.SelectedShipmentShareDocumentsDataLists = [];
            if (this.ShipmentShareDocumentsDataLists) {
                this.ShipmentShareDocumentsDataLists.forEach((item) => {
                    if (!AppTool.IsNullOrEmpty(item.AgentSharedManifestRef) && item.ShareDocuments.filter(d => d.Included == true).length > 0) {
                        var shipmentShareDocumentsData: ShipmentShareDocumentsData = new ShipmentShareDocumentsData();
                        shipmentShareDocumentsData.AgentId = item.AgentId;
                        shipmentShareDocumentsData.AgentSharedManifestRef = item.AgentSharedManifestRef;
                        shipmentShareDocumentsData.TenantAgent = item.TenantAgent;
                        shipmentShareDocumentsData.EntityId = item.EntityId;
                        shipmentShareDocumentsData.ShipmentLevelCode = item.ShipmentLevelCode;
                        shipmentShareDocumentsData.ShipmentNumber = item.ShipmentNumber;
                        shipmentShareDocumentsData.ShareDocuments = item.ShareDocuments.filter(d => d.Included);

                        this.SelectedShipmentShareDocumentsDataLists.push(shipmentShareDocumentsData);
                    }
                });

                if (this.Mode == "Attachment") {
                    this.AttachmentsLists = new Array<AttachmentsList>();

                    this.SelectedShipmentShareDocumentsDataLists.forEach((item) => {
                        item.ShareDocuments.forEach((doc) => {
                            var item = new AttachmentsList();
                            item.Id = doc.DocumentId;
                            item.DocumentFilingId = doc.DocumentsFilingId;
                            item.Tenant = SessionLocator.Tenant;
                            item.FileSize = doc.FileSize;
                            item.FileExtension = doc.Extension;
                            item.DocumentTypeCopyNameWithDocumentTypeName = doc.DocumentTypeName;
                            item.ShowRemoveLink = true;
                            this.AttachmentsLists.push(item);
                        });


                    });

                    this.OnCloseSharedWithAgentsEvent.emit(this.AttachmentsLists);
                    this.CloseButtonClicked();

                }
                else {

                    if (this.SelectedShipmentShareDocumentsDataLists.length > 0) {

                        this.CurrentSession.StartBusyIndicator("Sharing Documnents...");
                        this._agentSharedDocumentExtendedService.PostSharedDocuments(this.SelectedShipmentShareDocumentsDataLists, this.EntityPM.Id).subscribe((res:any) => {
                            var pmResponse: ServiceResponse = res;
                            var messageWindow: MessageWindow = new MessageWindow();
                            messageWindow.Title = "Share Document";
                            if (!pmResponse.HasError) {
                                ServiceLocator.SendTotangoUserActivity("Agents Shared Logistics", "Share Documents");
                                messageWindow.Show("Selected documents were shared successfully.");
                                this.CloseButtonClicked();
                            }
                            else {
                                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray[0]) {
                                    messageWindow.Show(pmResponse.ErrorsArray[0].toString());
                                }
                            }
                            this.CurrentSession.StopBusyIndicator();
                        });

                    }
                    else {

                        var messageWindow: MessageWindow = new MessageWindow();
                        messageWindow.Title = "Share Document";
                        messageWindow.Show("Please select at least one document.");
                    }
                }

            }

            else {
                if (this.Mode == "Attachment") {
                    this.OnCloseSharedWithAgentsEvent.emit(this.AttachmentsLists);
                    this.CloseButtonClicked();

                } else {
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Title = "Share Document";
                    messageWindow.Show("Please definition at least one document.");
                }

            }



        }
 
    SortItemSource(ItemsSource: any) {

        ItemsSource.sort((a, b) => {
            if (a.DocumentTypeName.toLowerCase() < b.DocumentTypeName.toLowerCase()) {
                return -1;
            }
            else if (a.DocumentTypeName.toLowerCase() > b.DocumentTypeName.toLowerCase()) {
                return 1;
            }
            else {

                return 0;
            }
        });
        return ItemsSource;
    }
}



