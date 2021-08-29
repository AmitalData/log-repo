declare var System: any, window: any;
import {Component, Output, EventEmitter, OnInit, AfterViewInit} from '@angular/core';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';

import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {IconButton} from '../../../../Controls/IconButton';
import {DocumentsFilingExtendedPMService} from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {GroupByPipe} from '../../../../Infrastructure/Pipes/GroupByPipe';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {DocumentExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentExtendedService';
import {ImageLibraryService} from '../../../../Common/Services/Others/ImageLibraryService';
import {DocumentsFilingPMService} from '../../../../Common/Services/StandardPMs/DocumentsFilingPMService'
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ExportDocumentService} from '../../../../Common/Services/DocumentServices/ExportDocumentService';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {GeneralEmailSender} from '../../../../Infrastructure/Helpers/GeneralEmailSender';
import {AttachmentsList} from '../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList';
import {LogBoxSignatureClientService} from '../../../../Shipment/Services/Others/LogBoxSignatureClientService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import {HybridPartnerPMService} from '../../../../Common/Services/StandardPMs/HybridPartnerPMService';
import {EntityStatusExtendedListService} from '../../../../Infrastructure/Services/ExtendedLists/EntityStatusExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'LogBoxDocuments',
    
    templateUrl: './LogBoxDocumentsComponent.html',
    //providers: [ EntityListService, DocumentsFilingExtendedPMService],
    inputs: ['ShipmentSelectedEvent', 'OnImporterShipmentsFilterChanged', 'SearchText'],
    //pipes: [CountryFlagPipe, AttatchmentIconPipe]
})

export class LogBoxDocumentsComponent extends BaseComponent implements OnInit, AfterViewInit {
    public ShipmentSelectedEvent: EventEmitter<any>;
    public OnImporterShipmentsFilterChanged: EventEmitter<any>;
    public _LogBoxSignatureClientService: LogBoxSignatureClientService;
    _HybridPartnerPMService: HybridPartnerPMService;
    SelectedShipment: any;
    ShipmentPM: any;
    SelectedTabCode: string;
    externalDocs: any[];
    SignReqDocs: any[];
    externalRequestedDocs: any[];
    SharedDocs: any[] = [];
    Style: any = {};
    IsRecentSelected: boolean = false;
    ShowCancelled: boolean = false;
    RequestedCount: number = 0;
    SignRequiredCount: number = 0;
    StopLoading: boolean = false;
    @Output() ArchiveDone = new EventEmitter();
    DataContext: LogBoxDocumentsComponent = this;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public _documentExtendedService: DocumentExtendedService;
    _ImageLibraryService: ImageLibraryService;
    public _documentsFilingPMService: DocumentsFilingPMService;
    public _exportDocumentService: ExportDocumentService;
    public _ShipmentPMService: ShipmentPMService;
    public _EntityStatusExtendedListService: EntityStatusExtendedListService;
    SearchText: string;
    LogBoxFileLabel: string = "LogBox File";
    AgentLable: string = "Agent";
    private messageWindow: MessageWindow = new MessageWindow();
    RefreshTimer: any;
    private CurrentSession = SessionLocator.SelectedSession;

    public MainCarriageTA: any = "";
    public MainCarriageTD: any = "";
    public MainCarriageTALabel: string = "";
    public MainCarriageTDLabel: string = "";

    public hasExportShipmentToggle: boolean = false;

    public hasDocumentTypeHighlightColor = SessionLocator.PrivateLableSettings ? (SessionLocator.PrivateLableSettings.DocumentTypeHighlightColor == null ? false : true) : false; 
    public privateLabelClass = {
        background: SessionLocator.PrivateLableSettings ? SessionLocator.PrivateLableSettings.DocumentTypeHighlightColor : "",
        border: SessionLocator.PrivateLableSettings ? "1px solid #" + SessionLocator.PrivateLableSettings.DocumentTypeHighlightColor: "",
    } 

    public FromPortCountryCode: string;
    public   FromPortCode: string;

    public ToPortCountryCode: string;
    public ToPortCode: string;
    constructor(public http: HttpClient, public serviceArgs: ServiceArgs, private _entityListService: EntityListService) {
        super();
        this.serviceArgs.http = this.http;
        this.SelectedTabCode = "CAT";
        this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        this._documentExtendedService = new DocumentExtendedService();
        this._ImageLibraryService = new ImageLibraryService();
        this._documentsFilingPMService = new DocumentsFilingPMService();
        this._exportDocumentService = new ExportDocumentService();
        this._ShipmentPMService = new ShipmentPMService();
        this._LogBoxSignatureClientService = new LogBoxSignatureClientService();
        this._HybridPartnerPMService = new HybridPartnerPMService();
        this._EntityStatusExtendedListService = new EntityStatusExtendedListService();
        this.CurrentSession.SessionEvent.subscribe(($event: any) => {
            if ($event.Name == "DisableBusyIndicator") {
                this.StopBusyIndicator();
            }
        });

        this.checkExportShipmentToggle();
    }

    checkExportShipmentToggle() { 
        let ExportShipmentFeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "PLE")[0];
        if (ExportShipmentFeatureToggle) {
            this.hasExportShipmentToggle = true; 
        }
    }
    SetPortFields() {

        if (this.SelectedShipment) {

            this.FromPortCountryCode = this.SelectedShipment.MainCarriageFromPortCountryCode ? this.SelectedShipment.MainCarriageFromPortCountryCode : this.SelectedShipment.FromPortCountryCode;
            this.ToPortCountryCode = this.SelectedShipment.MainCarriageToPortCountryCode ? this.SelectedShipment.MainCarriageToPortCountryCode : this.SelectedShipment.ToPortCountryCode;

            this.FromPortCode = (this.SelectedShipment.MainCarriageFromPortCode && this.SelectedShipment.MainCarriageFromPortCode != '---') ? this.SelectedShipment.MainCarriageFromPortCode : this.SelectedShipment.FromPortCode;
            this.ToPortCode = (this.SelectedShipment.MainCarriageToPortCode && this.SelectedShipment.MainCarriageToPortCode != '---') ? this.SelectedShipment.MainCarriageToPortCode : this.SelectedShipment.ToPortCode;

            if (!this.FromPortCode) this.FromPortCode = "";
            if (!this.ToPortCode) this.ToPortCode = "";

        }
    }

    ClearPortFields() {

        this.FromPortCountryCode = "";
        this.ToPortCountryCode = "";
        this.FromPortCode = "";
        this.ToPortCode = "";
    }

    IsPrivateLabel: boolean = false;
    AllowSendingDocsToAgent: boolean = false;
    DocsSentToAgent: boolean = false;
    //ForwardingPartner: any;
    ngOnInit() {
        if (SessionLocator.PrivateLableSettings) {
            this.LogBoxFileLabel = SessionLocator.PrivateLableSettings.PrivateLabelShortName + " File";
            this.AgentLable = SessionLocator.PrivateLableSettings.PrivateLabelShortName;
            this.IsPrivateLabel = true;
        }
        else {
            this.IsPrivateLabel = false;
        }
        this.ShipmentSelectedEvent.subscribe((res) => {
            //this.CurrentSession.StartBusyIndicator("Loading ...");//
          this.StopLoading = false;
            if (res == null) {
                this.StopLoading = true;
                this.externalDocs = [];
                this.SignReqDocs = [];
                this.externalRequestedDocs = [];
                this.AllHeader = "By Category (0)";
                this.RequestedCount = 0;
                this.SignRequiredCount = 0;
                this.SelectedShipment = null;
                this.ClearPortFields();
            return;
          }
          
            //this.DisableAddDocumentButton = true;
            this.StartBusyIndicator("Loading ...");
            var div = document.getElementById("DocsTab");
            this.Style = { "max-height": div.clientHeight };
            this.SelectedShipment = res;

            this.SetPortFields();
            //if (this.RefreshTimer) {
            //    clearTimeout(this.RefreshTimer);
            //}
            //this.RefreshTimer = setInterval(() => this.ReloadDocuments(false), 5000);//setTimeout(() => this.CheckIfSignDone(EntityPm.Id), 2000);
            this._ShipmentPMService.get(this.SelectedShipment.Id).subscribe((myResult: any) => {
              if (!myResult.HasError) {
                if (this.StopLoading) {
                  this.StopBusyIndicator();
                }
                else {
                  this.ShipmentPM = myResult.Result;
                  this.ShipmentTypeId = myResult.Result.ShipmentTypeId;
                  this.DocsSentToAgent = myResult.Result.DocsSentToAgent;
                    this.SetMainCarriageDates();
                  this._EntityStatusExtendedListService.getSingle("INPS").subscribe((Status: ServiceResponse) => {
                    if (Status.Result && (myResult.Result.StatusId == Status.Result.Id)) {
                      this.DisableAddDocumentButton = true;
                    }
                    else {
                      this.DisableAddDocumentButton = false;
                    }
                    this.ReloadDocuments(true);
                  });
                  this._HybridPartnerPMService.get(myResult.Result.ForwarderPartnerId).subscribe((theResult: any) => {
                      if (!theResult.HasError && theResult.Result) {
                      this.AllowSendingDocsToAgent = theResult.Result.AllowSendingDocsToAgent;
                    }
                  });
                  //this._HybridPartnerPMService.get(myResult.Result.ForwardingPartnerId).subscribe(theResult => {
                  //    if (!theResult.HasError) {
                  //        this.ForwardingPartner = theResult.Result;
                  //    }
                  //});
                }
              }
              else {
                this.ReloadDocuments(true);
              }
            });
          

            //this.ReloadDocuments(true);

        });
        this.OnImporterShipmentsFilterChanged.subscribe((res) => {
            this.OnImporterShipmentsFilterChangedMethod(res);
        });
    }
    SetMainCarriageDates() { 
        this.SetMainCarriageArrivalDate(); 
        this.SetMainCarriageDepartureDate(); 
    }
    
    private SetMainCarriageArrivalDate() {
        if (this.ShipmentPM.MainCarriageATA != null) {
            this.MainCarriageTA = this.ShipmentPM.MainCarriageATA;
            this.MainCarriageTALabel = 'ATA:';
        } else if (this.ShipmentPM.MainCarriageETA != null) {
            this.MainCarriageTA = this.ShipmentPM.MainCarriageETA;
            this.MainCarriageTALabel = 'ETA:';
        } else {
            this.MainCarriageTA = "";
            this.MainCarriageTALabel = "";
        }
    }

    private SetMainCarriageDepartureDate() {
        if (this.ShipmentPM.MainCarriageATD != null) {
            this.MainCarriageTD = this.ShipmentPM.MainCarriageATD;
            this.MainCarriageTDLabel = 'ATD:';
        } else if (this.ShipmentPM.MainCarriageETD != null) {
            this.MainCarriageTD = this.ShipmentPM.MainCarriageETD;
            this.MainCarriageTDLabel = 'ETD:';
        } else {
            this.MainCarriageTD = '';
            this.MainCarriageTDLabel = '';
        }
    }

    ngAfterViewInit() {

    }
    private shipmentTypeId: string = null;
    public get ShipmentTypeId() { return this.shipmentTypeId }
    public set ShipmentTypeId(newValue: string) { this.shipmentTypeId = newValue; }

    private showDeleted: boolean = false;
    public get ShowDeleted() { return this.showDeleted }
    public set ShowDeleted(newValue: boolean) { this.showDeleted = newValue; this.StartBusyIndicator("Loading .."); this.ReloadDocuments(); }

    private textChanged: string = "";
    get TextChanged() {
        if (this.SelectedShipment != null && this.SelectedShipment.TransportModeId == "A") {
            if (AppTool.IsNullOrEmpty(this.SelectedShipment.House)) {
                return this.textChanged = "Master:";
            }
            else {
                return this.textChanged = "Hawb:";
            }

        }
        else if (this.SelectedShipment != null && this.SelectedShipment.TransportModeId == "O") {
            return this.textChanged = "Ocean BL:";
        }
        else {
            return this.textChanged = "Transport Document #:";
        }
    }
    set TextChanged(newValue: string) {
        if (this.textChanged != newValue) {
            this.textChanged = newValue;
        }
    }

    private valueChanged: string = "All";
    get ValueChanged() {
        if (this.SelectedShipment != null && this.SelectedShipment.TransportModeId == "A") {
            if (AppTool.IsNullOrEmpty(this.SelectedShipment.House)) {
                return this.SelectedShipment.Master;
            }
            else {
                return this.SelectedShipment.House;
            }
        }
        else if (this.SelectedShipment != null && this.SelectedShipment.TransportModeId == "O") {
            return this.SelectedShipment.Master;
        }
        else {
            if (this.SelectedShipment != null) {
                return this.SelectedShipment.CarrierTransportDocumentNumber;
            }
            return "";
        }
    }
    set ValueChanged(newValue: string) {
        if (this.valueChanged != newValue) {
            this.valueChanged = newValue;
        }
    }

    //    get ShareWithForwarder() { 
    //            return "";

    //    }
    //    public bool ShareWithForwarder
    //        {
    //    get
    //    {
    //        return (!IsSharedWithForwarder && !string.IsNullOrEmpty(ShipmentEntityList.ForwarderShipmentNumber));
    //    }
    //}
    Timers: { [Id: string]: any; } = {};
    TimerStartDate: Date;
    SetDigitallySigned(EntityPm) {//id 
        if (this.RefreshTimer) {
            clearTimeout(this.RefreshTimer);
        }
        this.TimerStartDate = DateTool.GetCurrentDateTimeAsUtc();
        this.RefreshTimer = setInterval(() => this.ReloadDocuments(false), 5000);//setTimeout(() => this.CheckIfSignDone(EntityPm.Id), 2000);
        this.IsDeleteClicked = true;

        if (!FeatureLocator.HasFeaturePermession("General", "LBDS")) {
            var window = new ConfirmWindow();
            window.Width = 450;
            window.Height = 190;
            window.Title = "You have no permession";
            window.YesButtonText = "Ok";
            window.ShowNoButton = false;
            window.Show("Your package doesn't include this module..");
        }

        else {
            //item.IsCustomReference == true
            if (EntityPm.FileExtension.toLowerCase() == "pdf") {//&& FeatureLocator.HasFeaturePermession("General", "LBDS")
                if (EntityPm.IsCustomReference == true) {
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Title = "Confirm Deletion";
                    confirmWindow.Width = 450;
                    confirmWindow.Height = 190;
                    confirmWindow.YesButtonText = "Ok";
                    confirmWindow.NoButtonText = "Cancel";
                    confirmWindow.Show("Document was already sent to customs and cannot be updated , we will create a copy of them for the customs agent.");
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            this.RunSignBusyIndicator(true, EntityPm.Id);
                            //var element = document.getElementById('BusyIndecator' + EntityPm.Id);
                            //if (element) {
                            //    element.style.display = 'block';
                            //}
                            this.IsDeleteClicked = true;
                            EntityPm.DontAddToQueue = true;
                            EntityPm.SignRequestByUserEmail = SessionLocator.LoggedUserPM.Email;
                            EntityPm.SignDueDate = DateTool.GetCurrentDateTimeAsUtc();
                            var CurrMin = EntityPm.SignDueDate.getMinutes() + 5;
                            EntityPm.SignDueDate.setMinutes(CurrMin);
                            EntityPm.CancellSignRequest = false;
                            //this._documentsFilingPMService.update(EntityPm).subscribe((myResult:any) => {
                            this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPm).subscribe((Result:any) => {
                                ServiceLocator.SendTotangoUserActivity("LogBox", "Sign Document");
                                if (Result.Result != null && Result.Result.HasError) {
                                    this.RunSignBusyIndicator(false, EntityPm.Id);
                                    this.messageWindow.Width = 300;
                                    this.messageWindow.Height = 150;
                                    this.messageWindow.Title = "Warning !";
                                    this.messageWindow.Message = Result.Result.ErrorsArray[0];
                                    this.messageWindow.Show(this.messageWindow.Message);
                                }
                                else if (Result.Result == null) {
                                    this.messageWindow.Width = 300;
                                    this.messageWindow.Height = 150;
                                    this.messageWindow.Title = "Warning !";
                                    this.messageWindow.Message = "Please make sure that cloud sign app installed to your computer.";
                                    this.messageWindow.Show(this.messageWindow.Message);
                                }
                                else {

                                    //if (this.Timers[EntityPm.Id]) {
                                    //    clearTimeout(this.Timers[EntityPm.Id]);
                                    //}
                                    //this.Timers[EntityPm.Id] = setInterval(() => this.CheckIfSignDone(EntityPm.Id), 5000);//setTimeout(() => this.CheckIfSignDone(EntityPm.Id), 2000);
                                }

                            });
                        }
                    });
                }
                else {
                    this.RunSignBusyIndicator(true, EntityPm.Id);
                    //var element = document.getElementById('BusyIndecator' + EntityPm.Id);
                    //if (element) {
                    //    element.style.display = 'block';
                    //}
                    this.IsDeleteClicked = true;
                    EntityPm.DontAddToQueue = true;
                    EntityPm.SignRequestByUserEmail = SessionLocator.LoggedUserPM.Email;
                    EntityPm.SignDueDate = DateTool.GetCurrentDateTimeAsUtc();
                    var CurrMin = EntityPm.SignDueDate.getMinutes() + 5;
                    EntityPm.SignDueDate.setMinutes(CurrMin);
                    EntityPm.CancellSignRequest = false;
                    //this._documentsFilingPMService.update(EntityPm).subscribe((myResult:any) => {
                    this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPm).subscribe((Result:any) => {
                        ServiceLocator.SendTotangoUserActivity("LogBox", "Sign Document");
                        if (Result.Result != null && Result.Result.HasError) {
                            this.RunSignBusyIndicator(false, EntityPm.Id);
                            this.messageWindow.Width = 300;
                            this.messageWindow.Height = 150;
                            this.messageWindow.Title = "Warning !";
                            this.messageWindow.Message = Result.Result.ErrorsArray[0];
                            this.messageWindow.Show(this.messageWindow.Message);
                        }
                        else if (Result.Result == null) {
                            this.messageWindow.Width = 300;
                            this.messageWindow.Height = 150;
                            this.messageWindow.Title = "Warning !";
                            this.messageWindow.Message = "Please make sure that cloud sign app installed to your computer.";
                            this.messageWindow.Show(this.messageWindow.Message);
                        }
                        else {

                            //if (this.Timers[EntityPm.Id]) {
                            //    clearTimeout(this.Timers[EntityPm.Id]);
                            //}
                            //this.Timers[EntityPm.Id] = setInterval(() => this.CheckIfSignDone(EntityPm.Id), 5000);//setTimeout(() => this.CheckIfSignDone(EntityPm.Id), 2000);
                        }

                    });
                } 
            }
            else {
                var window = new ConfirmWindow();
                window.Width = 450;
                window.Height = 190;
                window.Title = "Warning !";
                window.YesButtonText = "Ok";
                window.ShowNoButton = false;
                window.Show("You can only sign PDF files ..");
            }
        }
    }

    CheckIfSignDone(DocId: string) {
        this._documentsFilingPMService.get(DocId).subscribe((res:any) => {
            var pmResponse: any = res;
            if (pmResponse != null && !pmResponse.HasError) {
                var currentdocument = pmResponse.Result;
                if (currentdocument && currentdocument.IsDigitallySigned == true && currentdocument.SignRequestByUserEmail == null) {
                    //this.Timers[currentdocument.Id] = null;
                    // if (this.Timers[currentdocument.Id]) {
                    //    clearTimeout(this.Timers[currentdocument.Id]);
                    //}
                    //var element = document.getElementById('BusyIndecator' + currentdocument.Id);
                    //if (element) {
                    //    element.style.display = 'none';
                    //    this.ReloadDocuments();
                    this.SendSignedDocumentToAgent(currentdocument);
                    //}
                }
                //else if (currentdocument && AppTool.IsNullOrEmpty(currentdocument.SignRequestByUserEmail)) {
                //    if (this.Timers[currentdocument.Id]) {
                //        clearTimeout(this.Timers[currentdocument.Id]);
                //    }
                //    var element = document.getElementById('BusyIndecator' + currentdocument.Id);
                //    if (element) {
                //        element.style.display = 'none';
                //        this.ReloadDocuments();
                //        this.SendSignedDocumentToAgent(currentdocument);
                //    }
                //    var window = new ConfirmWindow();
                //    window.Width = 450;
                //    window.Height = 190;
                //    window.Title = "Warning !";
                //    window.YesButtonText = "Ok";
                //    window.ShowNoButton = false;
                //    window.Show("There is no physical document for this file , it might be corrupted ..");
                //}
                else {
                    //if (this.Timers[currentdocument.Id]) {
                    //    clearTimeout(this.Timers[currentdocument.Id]);
                    //}
                }
            }
        });
    }

    SendSignedDocumentToAgent(Document: any) {
        if (Document.IsSharedWithCustomer == true) {

            Document.DontAddToQueue = false;
            Document.ForwarderDocumentId = null;
            this._documentsFilingPMService.update(Document).subscribe((myResult:any) => {

            });
        }
    }
    CancelSignProcess(EntityPM) {
        this.IsDeleteClicked = true;
        EntityPM.DontAddToQueue = true;
        EntityPM.SignRequestByUserEmail = null;
        EntityPM.CancellSignRequest = true;
        //this._documentsFilingPMService.update(EntityPm).subscribe((myResult:any) => {
        this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPM).subscribe((Result:any) => {
            this.RunSignBusyIndicator(false, EntityPM.Id);
        });
    }

    RunSignBusyIndicator(IsStart: boolean, Id: string) {
        if (IsStart == true) {
            var element = document.getElementById('BusyIndecator' + Id);
            if (element) {
                element.style.display = 'block';
            }
        }
        else {
            var element = document.getElementById('BusyIndecator' + Id);
            if (element) {
                element.style.display = 'none';
            }
            this.ReloadDocuments();
        }
    }

    AllHeader: string = "By Category (0)";
    IsRequestedSelected: boolean = false;
    OnImporterShipmentsFilterChangedMethod(res) {
        var IsRecentSelected = res.IsRecentSelected;
        this.IsRequestedSelected = res.IsRequestedSelected;
        if (IsRecentSelected) {
            this.AllHeader = "By Date (0)";
            this.IsRecentSelected = true;
        }
        else {
            this.AllHeader = "By Category (0)";
            this.IsRecentSelected = false;
        }
        if (this.IsRequestedSelected && (this.SelectedShipment ? (this.SelectedShipment.IsRequestedDocuments == true) : true)) {
            this.SelectedTabCode = 'REQ';
        }
        else {
            this.SelectedTabCode = 'CAT';
        }
        //if (this.IsRequestedSelected && this.SignReqPureDocs.length > 0) {

        //    this.SelectedTabCode = 'SREQ';
        //}
        this.externalDocs = [];
        this.SignReqDocs = [];
        this.externalRequestedDocs = [];
        this.RequestedCount = 0;
        this.SignRequiredCount = 0;
    }

    AddDocumentClick() {
        this._entityResourceService.getEntityResourceByTableName("DocumentsFiling").subscribe((response1:any) => {
            var windowArgs: any = {};
            windowArgs.SelectedShipment = this.SelectedShipment;
            windowArgs.IsNewDocument = true;

            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.WindowArgs = windowArgs;
            logitudeWindow.Width = 960;
            logitudeWindow.Height = 620;
            logitudeWindow.Title = "";
          logitudeWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditImporterDocumentComponent');
            logitudeWindow.WindowClosed.subscribe(($event: any) => {
                this.ReloadDocuments();
            });
        });
    }

    DocumentClicked(Document) {

        if (this.IsDeleteClicked) {
            this.IsDeleteClicked = false;
        }
        else {
            this._entityResourceService.getEntityResourceByTableName("DocumentsFiling").subscribe((response1:any) => {
                var windowArgs: any = {};
                windowArgs.SelectedShipment = this.SelectedShipment;
                windowArgs.IsNewDocument = false;
                windowArgs.EntityPm = Document;
                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.WindowArgs = windowArgs;
                logitudeWindow.Width = 960;
                logitudeWindow.Height = 620;
                logitudeWindow.Title = "";
              logitudeWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditImporterDocumentComponent');
                logitudeWindow.WindowClosed.subscribe(($event: any) => {
                    this.ReloadDocuments();
                });
            });
        }

    }
    IsDeleteClicked: boolean = false;
    DeleteDocumentClicked(item) {
        if (this.DisableAddDocumentButton == true) {
            return;
        }
        this.IsDeleteClicked = true;
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Confirm Deletion";
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.YesButtonText = "Delete";
        confirmWindow.NoButtonText = "Cancel";
        confirmWindow.Show("Are you sure you want to delete this document ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.StartBusyIndicator("Loading ..")
                item.IsDeleted = true;
                item.HasFile = false;
                item.FileSize = null;
                item.FileExtension = null;
                item.FileName = null;
                item.DocumentId = null;
                this._documentsFilingPMService.update(item).subscribe((myResult:any) => {
                    this.ReloadDocuments();
                    //this.StopBusyIndicator();
                });
            }

            else {

            }
        });
        //alert(item.Id);
    }
    DeleteDocumentFile(item) {
        if (this.DisableAddDocumentButton == true) {
            return;
        }
        this.IsDeleteClicked = true;
        if (item.IsCustomReference) {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "document was sent to custom !";
            this.messageWindow.Show("The document was sent to custom, therefore, it can't be deleted.");
        }
        else {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Title = "Confirm Deletion";
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.YesButtonText = "Delete";
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.Show("Are you sure you want to delete this document ?");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.StartBusyIndicator("Loading ..")
                    item.IsDeleted = true;
                    item.DontDeleteRealFile = true;
                    //item.HasFile = false;
                    //item.FileSize = null;
                    //item.FileExtension = null;
                    //item.FileName = null;
                    //item.DocumentId = null;
                    this._documentsFilingPMService.update(item).subscribe((myResult:any) => {
                        this.ReloadDocuments();
                        //this.StopBusyIndicator();
                    });
                }

                else {

                }
            });
        }
        
    }


    EmailSender: GeneralEmailSender;
    SendDocumentFile(item) {
        this.IsDeleteClicked = true;
        var attachment = new AttachmentsList();
        attachment.Tenant = SessionLocator.Tenant;
        attachment.DocumentTypeCopyNameWithDocumentTypeName = item.FileName;
        attachment.FileSize = item.FileSize;
        attachment.ShowRemoveLink = true;
        attachment.Id = item.DocumentId;
        attachment.FileExtension = item.FileExtension;


        var attachmentsList = new Array<AttachmentsList>();
        attachmentsList.push(attachment);

        if (!this.EmailSender || (this.EmailSender && !this.EmailSender.LoadingSendingComponent)) {
            this.EmailSender = new GeneralEmailSender("Shipment", item.DocumentTypeCode, this.SelectedShipment.Id, this.SelectedShipment.ShipmentNumber, null, null, item.Id, item.Description, attachmentsList);
            this.EmailSender.SendMessage();
        }



    }
    DownloadDocumentFile(item) {
        this.IsDeleteClicked = true;
        ServiceLocator.SendTotangoUserActivity("LogBox", "Document Viewed");
        //this._ImageLibraryService.DownloadFile(item.DocumentId, item.FileExtension, item.Folder, SessionLocator.Tenant).subscribe((res:any) => {
            var EntityNumber = "";
            if (this.SelectedShipment != null) {
                if (this.SelectedShipment.ForwarderShipmentNumber == null) {
                    EntityNumber = this.SelectedShipment.ShipmentNumber;
                }
                else {
                    EntityNumber = this.SelectedShipment.ForwarderShipmentNumber;
                }
            }
            var documentName = item.DocumentId + "*" + item.DocumentTypeCode + "-" + (!AppTool.IsNullOrEmpty(EntityNumber) ? EntityNumber : item.EntityId) + "-" + item.Code;// +"." + CurrentDocument.Extension;

            DownloadManager.DownloadPage(documentName);
        //});

    }

    ShareWithAgent(EntityPm) {
        if (this.SelectedShipment.StatusName == "In Progress") {
            return;
        }

        this.IsDeleteClicked = true;
        if (EntityPm.IsSharedWithCustomer || EntityPm.IsSharedWithForwarder) {
            return;
        }

        ServiceLocator.SendTotangoUserActivity("LogBox", "Share Document With Agent");
        EntityPm.IsSharedWithForwarder = true;
        EntityPm.DontAddToQueue = AppTool.IsNullOrEmpty(this.SelectedShipment.ForwarderShipmentNumber) ? true : false;

        this.UpdateDocumentPM(EntityPm);
    }

    UpdateDocumentPM(EntityPm: any) {
        this.StartBusyIndicator("Loading ...");
        this._documentsFilingPMService.update(EntityPm).subscribe((myResult: any) => {
            this.StopBusyIndicator();
            this.ReloadDocuments();
        });
    }

    RefreshBtnClick() {
        this.ReloadDocuments();
    }
    archiveButtonText: string = "Archive";
    public get ArchiveButtonText() {
        this.archiveButtonText = (this.SelectedShipment && this.SelectedShipment.IsOperationalClosed == true ? "Undo Archive" : "Archive");
        return this.archiveButtonText;
    }
    public set ArchiveButtonText(newValue: string) { this.archiveButtonText = newValue; }

    ArchiveClicked() {
        //this.CurrentSession.StartBusyIndicator("Saving ...");
        this.StartBusyIndicator("Saving ...");
        this._ShipmentPMService.get(this.SelectedShipment.Id).subscribe((myResult:any) => {
            if (!myResult.HasError) {
                myResult.Result.IsImporterShipment = true;
                if (this.ArchiveButtonText == "Archive") {
                    myResult.Result.IsOperationalClosed = true;
                    this.SelectedShipment.IsOperationalClosed = true;
                }
                else {
                    myResult.Result.IsOperationalClosed = false;
                    this.SelectedShipment.IsOperationalClosed = false;
                }
                var action = "A";
                myResult.Result.GrossWeightUnitCode = "KG";
                myResult.Result.DimensionsUnitCode = "Cm";
                myResult.Result.ChargeableWeightUnitCode = "KG";
                myResult.Result.VolumeUnitCode = "CBF";
                this._ShipmentPMService.update(myResult.Result).subscribe((myResult:any) => {
                    if (!myResult.HasError) {
                        ServiceLocator.SendTotangoUserActivity("LogBox", "Shipment Archived");
                        if (myResult.Result.IsOperationalClosed == true) {
                            this.ArchiveButtonText == "Undo Archive";
                            action = "A";
                        }
                        else {
                            this.ArchiveButtonText == "Archive";
                            action = "U";
                        }
                        //this.CurrentSession.StopBusyIndicator();
                        this.StopBusyIndicator();
                        //this.CurrentSession.FireEvent({ Name: "ReloadPublicShipments" });
                        this.ArchiveDone.emit({ Ship: this.SelectedShipment, Action: action });
                    }
                    else {
                        //this.CurrentSession.StopBusyIndicator();
                        this.StopBusyIndicator();
                    }

                });
            }
            else {
                //this.ValidationErrorsList = myResult.ErrorsArray;
                //this.CurrentSession.StopBusyIndicator();
                this.StopBusyIndicator();
            }
        });
    }

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public StartBusyIndicator(myText: string) {
        this.BusyIndicatorText = myText;
        this.ShowBusyIndicator = true;
    }

    public StopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }
    SignReqPureDocs: any[] = [];
    DeletedDocsCount: number = 0;
    AllUnDeletedDocsCount: number = 0;
    ReloadDocuments(ChangeTab: boolean = false) {
        var MyDate: Date = DateTool.GetCurrentDateTimeAsUtc();// new Date();
        if (this.TimerStartDate && (MyDate.getMinutes() > (this.TimerStartDate.getMinutes() + 5))) {
            if (this.RefreshTimer) {
                clearTimeout(this.RefreshTimer);
            }
        }
        else if (this.TimerStartDate && (MyDate.getMinutes() > (this.TimerStartDate.getMinutes() + 1))) {
            if (this.RefreshTimer) {
                clearTimeout(this.RefreshTimer);
            }
            //this.TimerStartDate = DateTool.GetCurrentDateTimeAsUtc();
            this.RefreshTimer = setInterval(() => this.ReloadDocuments(false), 2000);
        }
        var ObjectTable = window.ObjectTables.filter(x => x.Name === "Shipment")[0];
        if (ChangeTab == true) {
            if (this.IsRequestedSelected && this.SelectedShipment.IsRequestedDocuments == true) {
                this.SelectedTabCode = 'REQ';
            }
            else if (this.IsRequestedSelected && this.SelectedShipment.IsDigitalSignRequired == true) {
                this.SelectedTabCode = 'SREQ'; 
            } 
            else {
                this.SelectedTabCode = 'CAT';
            }
        }
        this._documentsFilingExtendedPMService.getAllDocumentsFilingsByEntityIdAndObjectTable(this.SelectedShipment.Id, ObjectTable.Id, "I", SessionLocator.Tenant).subscribe((res:any) => {
            var Result = [];
            var ResultSignReq = [];
            this.DeletedDocsCount = res.Result.filter(a => a.IsDeleted == true).length;
            this.AllUnDeletedDocsCount = res.Result.filter(a => a.IsDeleted == false).length;
            if (!this.ShowDeleted) {
                if (res.Result){
                    Result = res.Result.filter(a => a.IsDeleted == false);
                    ResultSignReq = res.Result.filter(a => a.IsDeleted == false && a.IsDigitalSignRequired == true);
                }
            }
            else {
                Result = res.Result;
                ResultSignReq = res.Result.filter(a => a.IsDigitalSignRequired == true);
            }
            this.SharedDocs = res.Result.filter(a => a.IsDeleted == false && a.IsSharedWithForwarder == true);
            var temp = [];
            var tempSignReq = [];
            this.SignReqPureDocs = ResultSignReq;
            if (this.IsRecentSelected) {
                temp = new GroupByPipe().transform(Result, "CreateDateWords");
                tempSignReq = new GroupByPipe().transform(ResultSignReq, "CreateDateWords");
                this.AllHeader = "By Date";
            }
            else {
                temp = new GroupByPipe().transform(Result, "DocumentCategoryName");
                tempSignReq = new GroupByPipe().transform(ResultSignReq, "DocumentCategoryName");
                this.AllHeader = "By Category";
            }

            this.externalDocs = temp;
            this.SignReqDocs = tempSignReq;//.filter(a => a.IsDigitalSignRequired);
            var tempArray = [];
            var Count = 0;
            this.externalDocs.forEach((docin) => {

                Count += docin.value.length;
            });
            var SignReqCount = 0;
            this.SignReqDocs.forEach((docin) => {

                SignReqCount += docin.value.length;
            });
            var TempArr = [];
            if (this.IsRecentSelected) {
                var Today = this.externalDocs.filter(a => a.key == "Today");
                var LastDays = this.externalDocs.filter(a => a.key == "Last 7 Days");
                var AllLeft = this.externalDocs.filter(a => a.key != "Last 7 Days" && a.key != "Today");
                var TodaySign = this.SignReqDocs.filter(a => a.key == "Today");
                var LastDaysSign = this.SignReqDocs.filter(a => a.key == "Last 7 Days");
                var AllLeftSign = this.SignReqDocs.filter(a => a.key != "Last 7 Days" && a.key != "Today");
                //AllLeft.forEach((docin) => {
                //    TempArr.push(docin);
                //});
                //AllLeft.forEach((docin) => {
                //    TempArr.push(docin);
                //});
                //AllLeft.forEach((docin) => {
                //    TempArr.push(docin);
                //});
                this.externalDocs = (Today.concat(LastDays)).concat(AllLeft);//this.externalDocs.sort(a => a.key == "Today" ? 1 : a.key == "Last 7 Days" ? 2 : 3);
                this.SignReqDocs = (TodaySign.concat(LastDaysSign)).concat(AllLeftSign);//this.externalDocs.sort(a => a.key == "Today" ? 1 : a.key == "Last 7 Days" ? 2 : 3);
            }
            else {
                //if (this.externalDocs.length > 2) {
                //    if (docin.key == "Operational Documents") {
                //        tempArray[0] = docin;
                //    }
                //    else if (docin.key == "Accounting Documents") {
                //        tempArray[1] = docin;
                //    }
                //    else {
                //        tempArray[2] = docin;
                //    }
                //}
                //else if()
                var OperationalDocuments = this.externalDocs.filter(a => a.key == "Operational Documents");
                var AccountingDocuments = this.externalDocs.filter(a => a.key == "Accounting Documents");
                var AllLeft = this.externalDocs.filter(a => a.key != "Operational Documents" && a.key != "Accounting Documents");
                var OperationalDocumentsSign = this.SignReqDocs.filter(a => a.key == "Operational Documents");
                var AccountingDocumentsSign = this.SignReqDocs.filter(a => a.key == "Accounting Documents");
                var AllLeftSign = this.SignReqDocs.filter(a => a.key != "Operational Documents" && a.key != "Accounting Documents");
                this.externalDocs = (OperationalDocuments.concat(AccountingDocuments)).concat(AllLeft);//this.externalDocs.sort(a => { return (a.key == "Operational Documents" ? 1 : a.key == "Accounting Documents" ? 2 : 3) });
                this.SignReqDocs = (OperationalDocumentsSign.concat(AccountingDocumentsSign)).concat(AllLeftSign);//this.externalDocs.sort(a => { return (a.key == "Operational Documents" ? 1 : a.key == "Accounting Documents" ? 2 : 3) });
            }
            if (this.IsRecentSelected) {
                this.AllHeader = "By Date ( " + Count + " )";
            }
            else {
                this.AllHeader = "By Category ( " + Count + " )";
            }
            this.SignRequiredCount = SignReqCount;
            if (ChangeTab == true && this.IsRequestedSelected && this.SignReqPureDocs.length > 0) {

                this.SelectedTabCode = 'SREQ';
            }
            //this.CurrentSession.StopBusyIndicator();
            this.StopBusyIndicator();

        }, error => {
            var dd: Response = error;
            //this.CurrentSession.StopBusyIndicator();
            this.StopBusyIndicator();
        });
        this._documentsFilingExtendedPMService.getRequestedDocumentsFilingsByEntityIdAndObjectTable(this.SelectedShipment.Id, ObjectTable.Id, "I", SessionLocator.Tenant).subscribe((res:any) => {
            var Count = 0;
            var Result = [];
            //this.DeletedDocsCount = res.Result.filter(a => a.IsDeleted == true).length;
            if (!this.ShowDeleted) {
                if (res.Result) {
                    Result = res.Result.filter(a => a.IsDeleted == false);
                }
            }
            else {
                Result = res.Result;
            }
            var temp = [];
            if (this.IsRecentSelected) {
                temp = new GroupByPipe().transform(Result, "CreateDateWords");
            }
            else {
                temp = new GroupByPipe().transform(Result, "DocumentCategoryName");
            }

            this.externalRequestedDocs = temp;
            this.externalRequestedDocs.forEach((docin) => {
                Count += docin.value.length;
            });
            this.RequestedCount = Count;

        }, error => {
            var dd: Response = error;
            });

       
    }

    DownloadAllClicked() {
        var windowArgs: any = {};
        var OTable = window.ObjectTables.filter(a => a.Name == "Shipment")[0];
        windowArgs.ObjectTableId = OTable.Id;
        windowArgs.ShipmentId = this.SelectedShipment.Id;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Width = 570;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = "Exporting All Documents To ZIP File";
      logitudeWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/DownloadAllFilesComponent');

    }
    DisableAddDocumentButton: boolean = true;

    ShareDocumentsClick() {
        if (this.SharedDocs.length <= 0) {
            this.ShowNoSharedDocumentsWindow();
            return;
        }

        var window = new ConfirmWindow();
        this.ShowConfirmSharingDocumentsWindow(window);
        window.WindowClosed.subscribe((event: any) => {
            this.OnClosedConfirmSharingDocumentsWindow(window);
        });
    }

    ShowNoSharedDocumentsWindow() {
        this.messageWindow.Width = 300;
        this.messageWindow.Height = 150;
        this.messageWindow.Title = "No Shared Documents !";
        this.messageWindow.Show("There are no shared documents, please share the related documents before.");
    }

    ShowConfirmSharingDocumentsWindow(window: ConfirmWindow) {
        window.Title = "Confirm sharing";
        window.Width = 450;
        window.Height = 190;
        window.YesButtonText = "Ok";
        window.NoButtonText = "Cancel";
        let windowMessage = "The shared documents will be send to the agent .";
        if (this.SharedDocs.length == this.AllUnDeletedDocsCount) {
            window.Show(windowMessage);
        }
        else {
            window.ShowWarningImage = true;
            windowMessage += "You are about to share the envelope but not all the documents are marked as shared with agent, are you sure?";
            window.Show(windowMessage);
        }
    }

    OnClosedConfirmSharingDocumentsWindow(window: ConfirmWindow) {
        if (window.Yes) {
            this.StartSharingDocumentsWithAgent();
        }
        else {
        }
    }

    StartSharingDocumentsWithAgent() {
        var SharedDocsIds = [];
        this.CurrentSession.StartBusyIndicator("Sharing ...");
        this.SharedDocs.forEach((docin) => {
            SharedDocsIds.push(docin.Id);
        });
        this.ShareDocumentsWithAgent(SharedDocsIds);
    }

    ShareDocumentsWithAgent(SharedDocsIds: any[]) {
        this._documentsFilingExtendedPMService.ShareDocumentsWithAgent(SharedDocsIds).subscribe((myResult: any) => {
            if (myResult.HasError) {
                return;
            }

            this.DocsSentToAgent = true;
            this._EntityStatusExtendedListService.getSingle("INPS").subscribe((Status: ServiceResponse) => {
                if (Status.Result) {
                    this.UpdateSelectedShipmentPM(Status);
                }
                else {
                    this.ShowNoStatusInProgressWindow();
                }
            });
        });
    }

    UpdateSelectedShipmentPM(Status: ServiceResponse) {
        this._ShipmentPMService.get(this.SelectedShipment.Id).subscribe((myShipmentResult: any) => {
            if (!myShipmentResult.HasError) {
                this.ShipmentPM = myShipmentResult.Result;
                this.ShipmentPM.StatusId = Status.Result.Id;
                this.ShipmentPM.ShipperReference1 = this.ShipmentPM.CustomerReference1;
                this.ShipmentPM.ShipperReference2 = this.ShipmentPM.CustomerReference2;
                this.ShipmentPM.ShipperId = this.ShipmentPM.CustomerId;
                this.ShipmentPM.DontAddToForwarderQueue = true;
                this._ShipmentPMService.update(this.ShipmentPM).subscribe((myResult: any) => {
                    if (!myResult.HasError) {
                        this.DisableAddDocumentButton = true;
                        this.CurrentSession.SessionEvent.emit({ Name: "ReloadShipments" });
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        });
    }

    ShowNoStatusInProgressWindow() {
        this.CurrentSession.StopBusyIndicator();
        this.messageWindow.Width = 300;
        this.messageWindow.Height = 150;
        this.messageWindow.Title = "No Status In progress !";
        this.messageWindow.Show("There are no Status In progress.");
    }

    SignAllClick() {
        if (!FeatureLocator.HasFeaturePermession("General", "LBDS")) {
            var window = new ConfirmWindow();
            window.Width = 450;
            window.Height = 190;
            window.Title = "You have no permession";
            window.YesButtonText = "Ok";
            window.ShowNoButton = false;
            window.Show("Your package doesn't include this module..");
        }

        else { 
            var SentToCustomesDocs = this.SignReqPureDocs.filter(a => a.IsCustomReference == true);
            if (SentToCustomesDocs.length > 0) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Title = "Confirm Deletion";
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.YesButtonText = "Ok";
                confirmWindow.NoButtonText = "Cancel";
                confirmWindow.Show("Some Documents you are trying to sign was already sent to customs and cannot be updated , we will create a copy of them for the customs agent.");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.SignAllLogic();
                    }
                });
            }
            else {
                this.SignAllLogic();
            }
           
        } 
    }

    SignAllLogic() {
        var Ids = [];
        this.SignReqPureDocs.forEach((docin) => {
            if (docin.IsDigitallySigned == false) {
                Ids.push(docin.Id);
            }
        });
        this._LogBoxSignatureClientService.GetMultiSignRequestReceived(Ids).subscribe((Result:any) => {
            //ServiceLocator.SendTotangoUserActivity("LogBox", "Sign Document");
            if (Result.Result != null && Result.Result.HasError) {
                //this.RunSignBusyIndicator(false, EntityPm.Id);
                this.messageWindow.Width = 300;
                this.messageWindow.Height = 150;
                this.messageWindow.Title = "Warning !";
                this.messageWindow.Message = Result.Result.ErrorsArray[0];
                this.messageWindow.Show(this.messageWindow.Message);
            }
            else if (Result.Result == null) {
                this.messageWindow.Width = 300;
                this.messageWindow.Height = 150;
                this.messageWindow.Title = "Warning !";
                this.messageWindow.Message = "Please make sure that cloud sign app installed to your computer.";
                this.messageWindow.Show(this.messageWindow.Message);
            }
            else {
                if (this.RefreshTimer) {
                    clearTimeout(this.RefreshTimer);
                }
                this.TimerStartDate = DateTool.GetCurrentDateTimeAsUtc();
                this.ReloadDocuments();
                this.RefreshTimer = setInterval(() => this.ReloadDocuments(false), 5000);

                //if (this.Timers[EntityPm.Id]) {
                //    clearTimeout(this.Timers[EntityPm.Id]);
                //}
                //this.Timers[EntityPm.Id] = setInterval(() => this.CheckIfSignDone(EntityPm.Id), 5000);//setTimeout(() => this.CheckIfSignDone(EntityPm.Id), 2000);
            }

        });
    }

    OnPackagesClick(Title) {
        var windowArgs: any = {};
        windowArgs.ShipmentPM = this.ShipmentPM;
        windowArgs.Title = Title;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Width = 690;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = Title;
      logitudeWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/LogBoxPackagesComponent');
    }
}
