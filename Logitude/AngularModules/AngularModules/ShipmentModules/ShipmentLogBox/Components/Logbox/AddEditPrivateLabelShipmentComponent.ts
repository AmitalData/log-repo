
declare var window: any; 
import { Component, Output, EventEmitter, OnInit, AfterViewInit } from '@angular/core'; 
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator'; 
import { ShipmentDomainService, ImporterQueriesDataCounts } from '../../../../Shipment/Services/ShipmentDomainService';
import { AppTool, DateTool, FormatTool } from '../../../../Infrastructure/Tools';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityStatusListService } from '../../../../Infrastructure/Services/StandardLists/EntityStatusListService';
import { BranchListService } from '../../../../Common/Services/StandardLists/BranchListService';
import { PackageTypeListService } from '../../../../Common/Services/StandardLists/PackageTypeListService';
import { DepartmentListService } from '../../../../Common/Services/StandardLists/DepartmentListService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { ShipmentPMService } from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import { PortExtendedPMService } from '../../../../Common/Services/ExtendedPMs/PortExtendedPMService';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ServiceLocator } from '../../../../Infrastructure/Locators/ServiceLocator';
import { DocumentsFilingExtendedPMService } from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { LogBoxSignatureClientService } from '../../../../Shipment/Services/Others/LogBoxSignatureClientService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { EntityStatusExtendedListService } from '../../../../Infrastructure/Services/ExtendedLists/EntityStatusExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({

    templateUrl: './AddEditPrivateLabelShipmentComponent.html',
    selector: 'AddEditPrivateLabelShipment',
    //providers: [Http, ServiceArgs, EntityListService]
})

export class AddEditPrivateLabelShipmentComponent extends BaseComponent implements OnInit, AfterViewInit {
    private myShipmentDomainService: ShipmentDomainService;
    EntityPM: ShipmentPM = new ShipmentPM();
    DataContext: AddEditPrivateLabelShipmentComponent = this;
    ValidationErrorsList: any[];
    WarningErrorsList: any[];
    IsNew: boolean = true;
    public IsDSVTenant: boolean = false;
    public _EntityStatusListService: EntityStatusListService;
    public _DepartmentListService: DepartmentListService;
    public _BranchListService: BranchListService;
    public _PackageTypeListService: PackageTypeListService;
    public _PortExtendedPMService: PortExtendedPMService;
    public _ShipmentPMService: ShipmentPMService;
    public PLShortName: string = "";
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    public _LogBoxSignatureClientService: LogBoxSignatureClientService;

    private messageWindow: MessageWindow = new MessageWindow();
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    public TransportationTypes: any[];// = [new TransportationTypes("Ashdod", "O", "ASH", "IL"), new TransportationTypes("Haifa", "O", "HFA", "IL"), new TransportationTypes("Eilat", "O", "ETH", "IL")];
    public FilterId_A: string;
    public FilterId_O: string;
    public FilterId_I: string;
    public CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        if (SessionLocator.PrivateLableSettings) {
            this.PLShortName = SessionLocator.PrivateLableSettings.PrivateLabelShortName;
        }

        this.IsDSVTenant =  SessionLocator.PrivateLableSettings.PrivateLabelDomain.toLowerCase().indexOf("dsv") > -1;

        if (this.CurrentSession == null) {
            this.FilterId_A = "TransportFilter_A_-1_-1";
            this.FilterId_O = "TransportFilter_O_-1_-1";
            this.FilterId_I = "TransportFilter_I_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("TransportsFilter");
            this.FilterId_A = "TransportFilter_A_" + idIndex;
            this.FilterId_O = "TransportFilter_O_" + idIndex;
            this.FilterId_I = "TransportFilter_I_" + idIndex;
        }
        this.ValidationErrorsList = [];
        this.myShipmentDomainService = new ShipmentDomainService();
        this._EntityStatusListService = new EntityStatusListService();
        this._DepartmentListService = new DepartmentListService();
        this._BranchListService = new BranchListService();
        this._ShipmentPMService = new ShipmentPMService();
        this._PortExtendedPMService = new PortExtendedPMService();
        this._PackageTypeListService = new PackageTypeListService();
        this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        this._LogBoxSignatureClientService = new LogBoxSignatureClientService();



    }
    //
    RefreshTimer: any;

    documentsFilings: any[] = [];
    LoadDocumentsFilings() {
        if (this.EntityPM && this.EntityPM.Id) {
            this.documentsFilings = [];
            var objectTable = window.ObjectTables.filter(x => x.Name === "Shipment")[0];
            this._documentsFilingExtendedPMService.getAllDocumentsFilingsByEntityIdAndObjectTable(this.EntityPM.Id, objectTable.Id, "I", SessionLocator.Tenant).subscribe((res: any) => {
                this.documentsFilings = res.Result.filter(a => a.IsDeleted == false);

            });
        }
    }


    LoadDocumentsFilingById(id: string) {
        this._documentsFilingExtendedPMService.getDocumentsFilingsById(id).subscribe((res: any) => {
            if (!this.documentsFilings) this.documentsFilings = [];
            this.documentsFilings.push(res.Result);

        });
    }

    DownloadDocumentFile(item) {
        ServiceLocator.SendTotangoUserActivity("LogBox", "Document Viewed");
        DownloadManager.DownloadPage(item.DocumentId);


    }

    ConnectDocumentsFilings(entityId: string) {

        this.documentsFilings.filter(d => !d.EntityId).forEach((documentsFiling) => {
            documentsFiling.EntityId = entityId;
            this._documentsFilingExtendedPMService.update(documentsFiling).subscribe((res: any) => {
            });
        });
    }




    EntityProgressStatusId: string;
    ConnectShipment(entity: any) {

        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");

        new EntityStatusExtendedListService().getSingle("INPS").subscribe((Status: ServiceResponse) => {
            this.EntityPM.StatusId = Status.Result.Id;
            this._ShipmentPMService.update(this.EntityPM).subscribe((myResult: any) => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();

                if (!myResult.HasError) {
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    this.CurrentSession.SessionEvent.emit({ Name: "ReloadShipments" });
                    this.CurrentSession.CurrentWindow.Close("");
                }
                else {
                    this.ValidationErrorsList = myResult.ErrorsArray;
                }
            });
        });
    }

    IsAddDocumentButtonClick: boolean = false;
    AddDocumentClick() {
        if (!this.IsAddDocumentButtonClick) {
            this.IsAddDocumentButtonClick = true;

            this._entityResourceService.getEntityResourceByTableName("DocumentsFiling").subscribe((response1: any) => {
                var windowArgs: any = {};
                windowArgs.SelectedShipment = this.EntityPM;
                windowArgs.IsNewDocument = true;
                windowArgs.ShareAsDefault = true;
                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.WindowArgs = windowArgs;
                logitudeWindow.Width = 960;
                logitudeWindow.Height = 620;
                logitudeWindow.Title = "";
                logitudeWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditImporterDocumentComponent');
                logitudeWindow.WindowClosed.subscribe(($event: any) => {
                    this.IsAddDocumentButtonClick = false;
                    if ($event) {
                        if (this.EntityPM && this.EntityPM.Id) {
                            this.LoadDocumentsFilings();
                        }
                        else {
                            this.LoadDocumentsFilingById($event);
                        }
                    }
                });
            });
        }
    }


    Timers: { [Id: string]: any; } = {};
    TimerStartDate: Date;
    SetDigitallySigned(EntityPm) {//id 
        if (this.RefreshTimer) {
            clearTimeout(this.RefreshTimer);
        }
        this.TimerStartDate = DateTool.GetCurrentDateTimeAsUtc();
        this.RefreshTimer = setInterval(() => this.LoadDocumentsFilings(), 5000);

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
            if (EntityPm.FileExtension.toLowerCase() == "pdf") {
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
                            this.CurrentSession.CurrentWindow.StartBusyIndicator("Signing in progress..");
                            EntityPm.DontAddToQueue = true;
                            EntityPm.SignRequestByUserEmail = SessionLocator.LoggedUserPM.Email;
                            EntityPm.SignDueDate = DateTool.GetCurrentDateTimeAsUtc();
                            var CurrMin = EntityPm.SignDueDate.getMinutes() + 5;
                            EntityPm.SignDueDate.setMinutes(CurrMin);
                            EntityPm.CancellSignRequest = false;
                            this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPm).subscribe((Result: any) => {
                                ServiceLocator.SendTotangoUserActivity("LogBox", "Sign Document");
                                if (Result.Result != null && Result.Result.HasError) {
                                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
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


                            });
                        }
                    });
                }
                else {
                    this.CurrentSession.CurrentWindow.StartBusyIndicator("Signing in progress..");
                    EntityPm.DontAddToQueue = true;
                    EntityPm.SignRequestByUserEmail = SessionLocator.LoggedUserPM.Email;
                    EntityPm.SignDueDate = DateTool.GetCurrentDateTimeAsUtc();
                    var CurrMin = EntityPm.SignDueDate.getMinutes() + 5;
                    EntityPm.SignDueDate.setMinutes(CurrMin);
                    EntityPm.CancellSignRequest = false;
                    this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPm).subscribe((Result: any) => {
                        ServiceLocator.SendTotangoUserActivity("LogBox", "Sign Document");
                        if (Result.Result != null && Result.Result.HasError) {
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
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








    transportItemClicked(itemValue: string) {
        if (this.TransportModeId != itemValue) {
            this.TransportModeId = itemValue;
            if (this.TransportModeId == "O") {
                this.TransportationTypes = [new TransportationTypes("Ashdod", "O", "ASH", "IL"), new TransportationTypes("Haifa", "O", "HFA", "IL"), new TransportationTypes("Eilat", "O", "ETH", "IL")];
                this.onTransportationTypeChange(new TransportationTypes("Haifa", "O", "HFA", "IL"));
            }
            else if (this.TransportModeId == "I") {
                this.TransportationTypes = [new TransportationTypes("Nitzana", "I", "NZN", "IL"), new TransportationTypes("Arava", "I", "ARV", "IL"), new TransportationTypes("Alenbi", "I", "ALN", "IL"), new TransportationTypes("Jordan", "I", "JOR", "IL")];
                this.onTransportationTypeChange(new TransportationTypes("Nitzana", "I", "NZN", "IL"));
            }
            else if (this.TransportModeId == "A") {
                this.TransportationTypes = [new TransportationTypes("Tel-Aviv", "A", "TLV", "IL")];
                this.onTransportationTypeChange(new TransportationTypes("Tel-Aviv", "A", "TLV", "IL"));
            }
        }
    }
    transportItemMouseOver(itemValue: string) {
        if (this.TransportModeId != itemValue) {
            var img_A = document.getElementById(this.FilterId_A);
            var img_O = document.getElementById(this.FilterId_O);
            var img_I = document.getElementById(this.FilterId_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I.png");
                    //img_I.style.top = "1px";
                    break;
                }
            }
        }
    }
    transportItemMouseLeave(itemValue: string) {
        if (this.TransportModeId != itemValue) {
            var img_A = document.getElementById(this.FilterId_A);
            var img_O = document.getElementById(this.FilterId_O);
            var img_I = document.getElementById(this.FilterId_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A_g.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O_g.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I_g.png");
                    break;
                }
            }
        }
    }
    ApplySelectedStyle() {
        var img_A = document.getElementById(this.FilterId_A);
        var img_O = document.getElementById(this.FilterId_O);
        var img_I = document.getElementById(this.FilterId_I);

        if (img_A) {
            img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A_g.png");
        }
        if (img_O) {
            img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O_g.png");
        }
        if (img_I) {
            img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I_G.png");
        }
        if (img_A) {
            switch (this.TransportModeId) {
                case "A": {
                    img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A_w.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O_w.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I_w.png");
                    break;
                }
            }
        }
    }
    private selectedTransportationTypes: TransportationTypes;
    public get SelectedTransportationTypes() {
        return this.selectedTransportationTypes;
    }
    public set SelectedTransportationTypes(newValue: TransportationTypes) {
        if (newValue) {
            this.selectedTransportationTypes = newValue;
            this.TransportModeId = newValue.TransporationType;
            this.ToPortId = newValue.ToPortCode;
        }

    }
    onTransportationTypeChange($event) {
        this.SelectedTransportationTypes = $event;
    }
    ngOnInit() {

    }
    ngAfterViewInit() {

    }
    UnAssignedPackageTypeId: string = '';
    SetWindowArgs(args: any) {
        //this.ShipmentList = args.SelectedShipment;
        this.SetCustomsShipmentArgs(args);
    }

    public SetCustomsShipmentArgs(args: any) {
        this.IsNew = args.IsNew;
        this._PackageTypeListService.getAll().subscribe((myResult: any) => {
            if (!myResult.HasError) {
                this.UnAssignedPackageTypeId = myResult.Result.filter(a => a.Tenant == SessionLocator.Tenant && a.Code == '---')[0].Id;
            }
            else {
                this.ValidationErrorsList = myResult.ErrorsArray;
            }
        });
        if (this.IsNew) {
            this.TransportModeId = "O";
            this.TransportationTypes = [new TransportationTypes("Ashdod", "O", "ASH", "IL"), new TransportationTypes("Haifa", "O", "HFA", "IL"), new TransportationTypes("Eilat", "O", "ETH", "IL")];
            this.SelectedTransportationTypes = this.TransportationTypes[1];

            this._EntityStatusListService.getAll().subscribe((myResult: any) => {
                if (!myResult.HasError) {

                    this.StatusId = myResult.Result.filter(a => a.Code == "OPOP")[0]?.Id;
                    this.EntityProgressStatusId = myResult.Result.filter(a => a.Code == "INPS")[0]?.Id;
                }
                else {
                    this.ValidationErrorsList = myResult.ErrorsArray;
                }
            });
            this._DepartmentListService.getAll().subscribe((myResult: any) => {
                if (!myResult.HasError) {
                    this.DepartmentId = myResult.Result.filter(a => a.Tenant == SessionLocator.Tenant)[0].Id;
                }
                else {
                    this.ValidationErrorsList = myResult.ErrorsArray;
                }
            });
            this._BranchListService.getAll().subscribe((myResult: any) => {
                if (!myResult.HasError) {
                    this.BranchId = myResult.Result.filter(a => a.Tenant == SessionLocator.Tenant)[0].Id;
                }
                else {
                    this.ValidationErrorsList = myResult.ErrorsArray;
                }
            });

        }
        if (args.EntityPM) {
            this.EntityPM = args.EntityPM;
            //if (!this.IsNew) {
            if (this.EntityPM.TransportModeId == "O") {
                this.TransportationTypes = [new TransportationTypes("Ashdod", "O", "ASH", "IL"), new TransportationTypes("Haifa", "O", "HFA", "IL"), new TransportationTypes("Eilat", "O", "ETH", "IL")];
                //this.SelectedTransportationTypes = this.TransportationTypes[1];
            }
            else if (this.EntityPM.TransportModeId == "I") {
                this.TransportationTypes = [new TransportationTypes("Nitzana", "I", "NZN", "IL"), new TransportationTypes("Arava", "I", "ARV", "IL"), new TransportationTypes("Alenbi", "I", "ALN", "IL"), new TransportationTypes("Jordan", "I", "JOR", "IL")];

            }
            else if (this.EntityPM.TransportModeId == "A") {
                this.TransportationTypes = [new TransportationTypes("Tel-Aviv", "A", "TLV", "IL")];
                this.onTransportationTypeChange(new TransportationTypes("Tel-Aviv", "A", "TLV", "IL"));
            }
            // Keep it true until Sprint D
            if (AppTool.IsNullOrEmpty(this.EntityPM.ShipmentAddtionalDataXML)) {
                this.PLForwarding = false;
            }
            else {
                this.PLForwarding = true;
            }
            ///////////////////////////////
            //var isLcl = this.IsLCLEntity(this.EntityPM);
            //if (isLcl == true) {
            //    this.PackageType = "LCL";
            //}
            //else if (this.EntityPM){
            //    this.PackageType = "FCL";
            //}
            //}
            this.SelectedTransportationTypes = new TransportationTypes("Ashdod", "O", "ASH", "IL");
            if (this.EntityPM.MainCarriageToPortCode == "ASH") {
                this.SelectedTransportationTypes = new TransportationTypes("Ashdod", "O", "ASH", "IL");
            }
            else if (this.EntityPM.MainCarriageToPortCode == "HFA") {
                this.SelectedTransportationTypes = new TransportationTypes("Haifa", "O", "HFA", "IL");
            }
            else if (this.EntityPM.MainCarriageToPortCode == "ETH") {
                this.SelectedTransportationTypes = new TransportationTypes("Eilat", "O", "ETH", "IL");
            }
            else if (this.EntityPM.MainCarriageToPortCode == "NZN") {
                this.SelectedTransportationTypes = new TransportationTypes("Nitzana", "I", "NZN", "IL");
            }
            else if (this.EntityPM.MainCarriageToPortCode == "ARV") {
                this.SelectedTransportationTypes = new TransportationTypes("Arava", "I", "ARV", "IL");
            }
            else if (this.EntityPM.MainCarriageToPortCode == "ALN") {
                this.SelectedTransportationTypes = new TransportationTypes("Alenbi", "I", "ALN", "IL");
            }
            else if (this.EntityPM.MainCarriageToPortCode == "JOR") {
                this.SelectedTransportationTypes = new TransportationTypes("Jordan", "I", "JOR", "IL");
            }
            else {
                this.SelectedTransportationTypes = new TransportationTypes("Tel-Aviv", "A", "TLV", "IL");
            }

            if (this.EntityPM.ShipmentAddtionalDataXML == "<PLForwarding>true</PLForwarding>") {
                this.PLForwarding = true;
            }
            else {
                this.PLForwarding = false;
            }


            this.LoadDocumentsFilings();
        }
    }

    PLForwardingClicked(PLF) {
        if (PLF == "Yes") {
            this.PLForwarding = true;
        }
        else {
            this.PLForwarding = false;
        }
    }

    PackageTypeClicked(Type) {
        if (Type == "OTHER") {
            this.PackageType = null;
        }
        else {
            this.PackageType = Type;
        }
    }

    // private packageType: string;
    public get PackageType() { return this.EntityPM.ShipmentTypeId }
    public set PackageType(newValue: string) { this.EntityPM.ShipmentTypeId = newValue; }

    private pLForwarding: boolean = false;
    public get PLForwarding() {
        return this.pLForwarding
    }
    public set PLForwarding(newValue: boolean) { this.pLForwarding = newValue; }

    public get ShipperName() { return this.EntityPM.ShipperName }
    public set ShipperName(newValue: string) { this.EntityPM.ShipperName = newValue; }

    public get CustomerReference1() { return this.EntityPM.CustomerReference1 }
    public set CustomerReference1(newValue: string) { this.EntityPM.CustomerReference1 = newValue; }

    public get CustomerReference2() { return this.EntityPM.CustomerReference2 }
    public set CustomerReference2(newValue: string) { this.EntityPM.CustomerReference2 = newValue; }

    public get CustomerId() { return this.EntityPM.CustomerId }
    public set CustomerId(newValue: string) { this.EntityPM.CustomerId = newValue; }

    public get StatusDate() { return this.EntityPM.StatusDate }
    public set StatusDate(newValue: any) { this.EntityPM.StatusDate = newValue; }

    public get StatusId() { return this.EntityPM.StatusId }
    public set StatusId(newValue: string) { this.EntityPM.StatusId = newValue; }

    public get ForwarderPartnerId() { return this.EntityPM.ForwarderPartnerId }
    public set ForwarderPartnerId(newValue: string) { this.EntityPM.ForwarderPartnerId = newValue; }

    public get DepartmentId() { return this.EntityPM.DepartmentId }
    public set DepartmentId(newValue: string) { this.EntityPM.DepartmentId = newValue; }

    public get BranchId() { return this.EntityPM.BranchId }
    public set BranchId(newValue: string) { this.EntityPM.BranchId = newValue; }

    public get ShipmentLevelCode() { return this.EntityPM.ShipmentLevelCode }
    public set ShipmentLevelCode(newValue: string) { this.EntityPM.ShipmentLevelCode = newValue; }

    public get DirectionId() { return this.EntityPM.DirectionId }
    public set DirectionId(newValue: string) { this.EntityPM.DirectionId = newValue; }

    public get TransportModeId() { return this.EntityPM.TransportModeId }
    public set TransportModeId(newValue: string) { this.EntityPM.TransportModeId = newValue; this.ApplySelectedStyle(); }

    public get ToPortId() { return this.EntityPM.ToPortId }
    public set ToPortId(newValue: string) { this.EntityPM.ToPortId = newValue; }

    public get OtherPrepaidCollectId() { return this.EntityPM.OtherPrepaidCollectId }
    public set OtherPrepaidCollectId(newValue: string) { this.EntityPM.OtherPrepaidCollectId = newValue; }

    public get FreightPrepaidCollectId() { return this.EntityPM.FreightPrepaidCollectId }
    public set FreightPrepaidCollectId(newValue: string) { this.EntityPM.FreightPrepaidCollectId = newValue; }

    public get FromPortId() { return this.EntityPM.FromPortId }
    public set FromPortId(newValue: string) { this.EntityPM.FromPortId = newValue; }

    public get PackagesQuantity() { return this.EntityPM.PackagesQuantity }
    public set PackagesQuantity(newValue: number) { this.EntityPM.PackagesQuantity = +(newValue); }

    public get GrossWeight() { return this.EntityPM.GrossWeight }
    public set GrossWeight(newValue: number) { this.EntityPM.GrossWeight = +(newValue); }

    public get Master() { return this.EntityPM.Master }
    public set Master(newValue: any) { this.EntityPM.Master = newValue; }

    public get House() { return this.EntityPM.House }
    public set House(newValue: string) { this.EntityPM.House = newValue; }

    public get Remarks() { return this.EntityPM.Notes }
    public set Remarks(newValue: string) { this.EntityPM.Notes = newValue; }



    private containerNumber: string;
    public get ContainerNumber() {
        if (this.EntityPM.ShipmentPackages && this.EntityPM.ShipmentPackages.length > 0) {
            this.containerNumber = this.EntityPM.ShipmentPackages[0].ContainerNumber;
        }
        return this.containerNumber;
    }
    public set ContainerNumber(newValue: string) {
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.EntityPM.ShipmentPackages = [];
            this.containerNumber = newValue;
        }
        if (this.EntityPM.ShipmentPackages && this.EntityPM.ShipmentPackages.length == 0 && !AppTool.IsNullOrEmpty(newValue)) {
            this.EntityPM.ShipmentPackages = [];
            var MyPackage = new ShipmentPackagePM(this.EntityPM);
            MyPackage.ContainerNumber = newValue;
            MyPackage.Weight = this.GrossWeight;
            MyPackage.PackageTypeId = this.UnAssignedPackageTypeId;
            MyPackage.Quantity = this.PackagesQuantity;
            this.EntityPM.ShipmentPackages.push(MyPackage);
        }
        else if (this.EntityPM.ShipmentPackages.length > 0) {
            this.EntityPM.ShipmentPackages[0].ContainerNumber = newValue;
            this.EntityPM.ShipmentPackages[0].Weight = this.GrossWeight;
            this.EntityPM.ShipmentPackages[0].PackageTypeId = this.UnAssignedPackageTypeId;
            this.EntityPM.ShipmentPackages[0].Quantity = this.PackagesQuantity;
        }
        //this.ValidateContainerNumber(newValue);
    }



    ValidateContainerNumber(input: string) {
        //this.ValidationErrorsList = [];
        this.ValidationErrorsList = [];
        var error = FormatTool.ValidateContainerNumber(input);

        if (!AppTool.IsNullOrEmpty(error)) {
            this.ValidationErrorsList.push(error);
        }
    }



    SaveChanges() {
        if (this.isSaveClicked == true) {
            return;
        }
        this.isSaveClicked = true;
        this.ValidationErrorsList = [];


        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (!AppTool.IsNullOrEmpty(this.ContainerNumber)) {
            var error = FormatTool.ValidateContainerNumber(this.ContainerNumber);

            if (!AppTool.IsNullOrEmpty(error)) {
                this.ValidationErrorsList.push(error);
            }
        }
        if (!this.SelectedTransportationTypes) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "TransportationTypes"));
        }

        if (AppTool.IsNullOrEmpty(this.CustomerReference1)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "OrderNumber"));
        }


        if (this.IsDSVTenant) {

            if (this.CustomerReference2 && this.CustomerReference2.length > 30) {
                this.ValidationErrorsList.push("My Reference can't be more than 30 characters");
            }
            if (!AppTool.IsNullOrEmpty(this.PackagesQuantity) && this.isInt(this.PackagesQuantity) == false) {
                this.ValidationErrorsList.push("Quantity Must be integer.");
            }

            if ((typeof this.PackagesQuantity != 'number' || this.PackagesQuantity.toString() == "NaN") && this.PackagesQuantity != null) {
                this.ValidationErrorsList.push("Quantity must be numaric value");
            }
            if ((typeof this.GrossWeight != 'number' || this.GrossWeight.toString() == "NaN") && this.GrossWeight != null) {
                this.ValidationErrorsList.push("Weight must be numaric value");
            }
            if (!AppTool.IsNullOrEmpty(this.ContainerNumber) && (AppTool.IsNullOrEmpty(this.PackagesQuantity) || AppTool.IsNullOrEmpty(this.GrossWeight))) {
                this.ValidationErrorsList.push("Weight and Quantity are required");
            }
            else {
                if (this.EntityPM.ShipmentPackages.length > 0) {
                    //this.EntityPM.ShipmentPackages[0].ContainerNumber = newValue;
                    this.EntityPM.ShipmentPackages[0].Weight = this.GrossWeight;
                    this.EntityPM.ShipmentPackages[0].PackageTypeId = this.UnAssignedPackageTypeId;
                    this.EntityPM.ShipmentPackages[0].Quantity = this.PackagesQuantity;
                }
            }
        }

        else {
            if (this.ShipperName && this.ShipperName.length > 50) {
                this.ValidationErrorsList.push("Supplier Name can't be more than 50 characters");
            }

            if (!this.documentsFilings || this.documentsFilings.filter(d => d.IsSharedWithForwarder == true).length == 0) {
                this.ValidationErrorsList.push("You should have at least one document shared with agent");
            }
        }

        if (this.ValidationErrorsList.length == 0) {
            this._PortExtendedPMService.getSinglePort(this.SelectedTransportationTypes.ToPortCode, this.SelectedTransportationTypes.CountryCode, SessionLocator.Tenant).subscribe((myResult: any) => {
                if (myResult.Result) {
                    this.ToPortId = myResult.Result.Id;
                    if (AppTool.IsNullOrEmpty(this.EntityPM.FromPortId)) {
                        this._PortExtendedPMService.getSinglePort("---", "IL", SessionLocator.Tenant).subscribe((Result: any) => {
                            this.FromPortId = Result.Result.Id;
                            if (this.IsNew) {
                                this._ShipmentPMService.GetByCustomerReference1(this.CustomerReference1, false).subscribe((myResult: any) => {
                                    if (myResult.Result) {
                                        var confirmWindow = new ConfirmWindow();
                                        confirmWindow.Title = "Warning !";
                                        confirmWindow.Width = 300;
                                        confirmWindow.Height = 150;
                                        confirmWindow.YesButtonText = "Continue";
                                        confirmWindow.NoButtonText = "Cancel";
                                        confirmWindow.Show("There are already shipments with the same order number");
                                        confirmWindow.WindowClosed.subscribe((event: any) => {
                                            if (confirmWindow.Yes) {
                                                this.SaveData();
                                            }
                                            else {
                                                this.isSaveClicked = false;
                                                //this.LoadImporterShipments(true);
                                            }
                                        });
                                    }
                                    else {
                                        this.SaveData();
                                    }
                                });
                            }
                            else {
                                this._ShipmentPMService.GetByCustomerReference1ForUpdate(this.CustomerReference1, this.EntityPM.Id, false).subscribe((myResult: any) => {
                                    if (myResult.Result) {
                                        var confirmWindow = new ConfirmWindow();
                                        confirmWindow.Title = "Warning !";
                                        confirmWindow.Width = 300;
                                        confirmWindow.Height = 150;
                                        confirmWindow.YesButtonText = "Continue";
                                        confirmWindow.NoButtonText = "Cancel";
                                        confirmWindow.Show("There are already shipments with the same order number");
                                        confirmWindow.WindowClosed.subscribe((event: any) => {
                                            if (confirmWindow.Yes) {
                                                this.SaveData();
                                            }
                                            else {
                                                this.isSaveClicked = false;
                                                //this.LoadImporterShipments(true);
                                            }
                                        });
                                    }
                                    else {
                                        this.SaveData();
                                    }
                                });
                            }

                        });
                    }
                    else {
                        if (this.IsNew == true) {
                            this._ShipmentPMService.GetByCustomerReference1(this.CustomerReference1, false).subscribe((myResult: any) => {
                                if (myResult.Result) {
                                    var confirmWindow = new ConfirmWindow();
                                    confirmWindow.Title = "Warning !";
                                    confirmWindow.Width = 300;
                                    confirmWindow.Height = 150;
                                    confirmWindow.YesButtonText = "Continue";
                                    confirmWindow.NoButtonText = "Cancel";
                                    confirmWindow.Show("There are already shipments with the same order number");
                                    confirmWindow.WindowClosed.subscribe((event: any) => {
                                        if (confirmWindow.Yes) {
                                            this.SaveData();
                                        }
                                        else {
                                            this.isSaveClicked = false;
                                            //this.LoadImporterShipments(true);
                                        }
                                    });
                                }
                                else {
                                    this.SaveData();
                                }
                            });
                        }
                        else {
                            this._ShipmentPMService.GetByCustomerReference1ForUpdate(this.CustomerReference1, this.EntityPM.Id, false).subscribe((myResult: any) => {
                                if (myResult.Result) {
                                    var confirmWindow = new ConfirmWindow();
                                    confirmWindow.Title = "Warning !";
                                    confirmWindow.Width = 300;
                                    confirmWindow.Height = 150;
                                    confirmWindow.YesButtonText = "Continue";
                                    confirmWindow.NoButtonText = "Cancel";
                                    confirmWindow.Show("There are already shipments with the same order number");
                                    confirmWindow.WindowClosed.subscribe((event: any) => {
                                        if (confirmWindow.Yes) {
                                            this.SaveData();
                                        }
                                        else {
                                            this.isSaveClicked = false;
                                            //this.LoadImporterShipments(true);
                                        }
                                    });
                                }
                                else {
                                    this.SaveData();
                                }
                            });
                        }
                    }
                }
                else {
                    this.ValidationErrorsList = [];

                    //var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

                    //if (!this.SelectedTransportationTypes) {
                    this.ValidationErrorsList.push("Transportation Type Port is not defined in your tenant.");
                    this.isSaveClicked = false;
                    //}
                    //this.SaveData();
                }
            });
        }
        else {
            this.isSaveClicked = false;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }

    }
    isInt(n) {
        return n % 1 === 0;
    }
    isSaveClicked: boolean = false;
    SaveData() {
        this.ValidationErrorsList = [];

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (!this.SelectedTransportationTypes) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "TransportationTypes"));
        }

        if (AppTool.IsNullOrEmpty(this.CustomerReference1)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "OrderNumber"));
        }

        if (this.IsDSVTenant) {
            if (!AppTool.IsNullOrEmpty(this.PackagesQuantity) && this.isInt(this.PackagesQuantity) == false) {
                this.ValidationErrorsList.push("Quantity Must be integer.");
            }
            //if (AppTool.IsNullOrEmpty(this.CustomerReference2)) {
            //    this.ValidationErrorsList.push(msg.replace("%FieldName", "My Reference"));
            //}
            var isMasterAllDigits = this.Master != null ? /^\d+$/.test(this.Master) : true;//typeof this.Master;
            if (!isMasterAllDigits) {
                this.ValidationErrorsList.push("Master Field must be all digits");
            }
            if ((typeof this.PackagesQuantity != 'number' || this.PackagesQuantity.toString() == "NaN") && this.PackagesQuantity != null) {
                this.ValidationErrorsList.push("Quantity must be numaric value");
            }
            //Master Field must be all digits
            if ((typeof this.GrossWeight != 'number' || this.GrossWeight.toString() == "NaN") && this.GrossWeight != null) {
                this.ValidationErrorsList.push("Weight must be numaric value");
            }
            if (!AppTool.IsNullOrEmpty(this.ContainerNumber) && (AppTool.IsNullOrEmpty(this.PackagesQuantity) || AppTool.IsNullOrEmpty(this.GrossWeight))) {
                this.ValidationErrorsList.push("Weight and Quantity are required");
            }
            else {
                if (this.EntityPM.ShipmentPackages.length > 0) {
                    this.EntityPM.ShipmentPackages[0].Weight = this.GrossWeight;
                    this.EntityPM.ShipmentPackages[0].PackageTypeId = this.UnAssignedPackageTypeId;
                    this.EntityPM.ShipmentPackages[0].Quantity = this.PackagesQuantity;
                }
            }
        }

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.isSaveClicked = false;
            this.EntityPM.IsImporterShipment = true;
            this.EntityPM.MainCarriageFromPortId = this.EntityPM.FromPortId;
            this.EntityPM.MainCarriageToPortId = this.EntityPM.ToPortId;
            this.EntityPM.GrossWeightUnitCode = "KG";
            this.EntityPM.DimensionsUnitCode = "Cm";
            this.EntityPM.ChargeableWeightUnitCode = "KG";
            this.EntityPM.VolumeUnitCode = "CBF";
            if (this.IsNew) {
                this.EntityPM.FreightPrepaidCollectId = "C";
                this.EntityPM.ShipmentCustomerTypeCode = "SHI";
                this.EntityPM.OtherPrepaidCollectId = "C";
                this.EntityPM.DirectionId = "C";
                this.EntityPM.ShipmentLevelCode = "A";
                this.EntityPM.OrderIsDangerouseGoods = false;
                this.EntityPM.StatusDate = DateTool.GetCurrentDateTimeAsUtc(); 
                this.EntityPM.CustomerId =SessionLocator.TenantPM.CustomerId;
                this.EntityPM.CustomerName = SessionLocator.TenantPM.CustomerId;
                this.EntityPM.ConsigneeId = SessionLocator.TenantPM.CustomerId;
                this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
                this.EntityPM.NewConcurrencyGUID = Guid.newGuid();
                this.EntityPM.Tenant = SessionLocator.Tenant;

                this.EntityPM.NumberOfContainers = this.PackagesQuantity;
                //}
                //else {
                this.EntityPM.NumberOfPackages = this.PackagesQuantity;
                //}

                if (SessionLocator.PrivateLableSettings) {
                    this.EntityPM.ForwarderPartnerId = SessionLocator.PrivateLableSettings.HybridPartnerId;
                }
                if (this.PLForwarding == true) {
                    this.EntityPM.ShipmentAddtionalDataXML = "<PLForwarding>true</PLForwarding>";
                }
                else {
                    this.EntityPM.ShipmentAddtionalDataXML = "<PLForwarding>false</PLForwarding>";
                }

                this.EntityPM.StatusId = !this.IsDSVTenant ? this.EntityProgressStatusId : this.EntityPM.StatusId;
                this.EntityPM.DocumentFilingIds = "";
                this.documentsFilings.forEach((item) => {
                    this.EntityPM.DocumentFilingIds += (item.Id + ",");
                });

                this._ShipmentPMService.insert(this.EntityPM).subscribe((myResult: any) => {
                    if (!myResult.HasError) {
                        ServiceLocator.SendTotangoUserActivity("LogBox", "New Shipment");
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        if (!this.IsDSVTenant) {
                            //  this.ConnectDocumentsFilings(myResult.Result.Id);
                            this.CurrentSession.SessionEvent.emit({ Name: "ReloadShipments" });
                            this.CurrentSession.CurrentWindow.Close("");

                        } else this.CurrentSession.CloseCurrentWindowEmit("MyShipmentAdded");

                    }
                    else {
                        this.ValidationErrorsList = myResult.ErrorsArray;
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                });
            }
            else {
                this.EntityPM.NumberOfContainers = this.PackagesQuantity;

                this.EntityPM.NumberOfPackages = this.PackagesQuantity;

                this.EntityPM.ForwarderPartnerId = SessionLocator.PrivateLableSettings.HybridPartnerId;

                if (this.PLForwarding == true) {
                    this.EntityPM.ShipmentAddtionalDataXML = "<PLForwarding>true</PLForwarding>";
                }
                else {
                    this.EntityPM.ShipmentAddtionalDataXML = "<PLForwarding>false</PLForwarding>";
                }
                this._ShipmentPMService.update(this.EntityPM).subscribe((myResult: any) => {
                    if (!myResult.HasError) {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindowEmit("MyShipmentAdded");
                    }
                    else {
                        //this.ValidationErrorsList = myResult.ErrorsArray;
                        this.ValidationErrorsList = myResult.ErrorsArray;//.push("There Are Validation Errors.");
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                });
            }
            //ShipmentContext.SubmitChanges().Completed += AddEditImporterShipmentViewModel_Completed;

        }
        else {
            this.isSaveClicked = false;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    }

    IsLCLEntity(Entity: any) {
        var myResult = false;
        if (Entity.TransportModeId == "A") {
            myResult = true;
        }

        else if (Entity.TransportModeId == "O" && Entity.ShipmentTypeId == "LCLD") {
            myResult = true;
        }

        else if (Entity.TransportModeId == "I" && Entity.ShipmentTypeId == "LTL") {
            myResult = true;
        }
        return myResult;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    itemMouseOver(itemValue: string) {
        //if (this.SelectedValue != itemValue) {
        //    var img_A = document.getElementById("TransportFilter_A");
        //    var img_O = document.getElementById("TransportFilter_O");
        //    var img_I = document.getElementById("TransportFilter_I");

        //    switch (itemValue) {
        //        case "A": {
        //            img_A.setAttribute("src", "./Images/TransportModes/A.png");
        //            break;
        //        }

        //        case "O": {
        //            img_O.setAttribute("src", "./Images/TransportModes/O.png");
        //            break;
        //        }

        //        case "I": {
        //            img_I.setAttribute("src", "./Images/TransportModes/I.png");
        //            //img_I.style.top = "1px";
        //            break;
        //        }
        //    }
        //}
    }
    itemMouseLeave(itemValue: string) {
        //if (this.SelectedValue != itemValue) {
        //    var img_A = document.getElementById("TransportFilter_A");
        //    var img_O = document.getElementById("TransportFilter_O");
        //    var img_I = document.getElementById("TransportFilter_I");

        //    switch (itemValue) {
        //        case "A": {
        //            img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
        //            break;
        //        }

        //        case "O": {
        //            img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
        //            break;
        //        }

        //        case "I": {
        //            img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
        //            break;
        //        }
        //    }
        //}
    }

}

export class TransportationTypes {
    constructor(private name: string, private transporationType: string, private toPortCode: string, CountryCode: string) {
        this.Name = name;
        this.TransporationType = transporationType;
        this.ToPortCode = toPortCode;
        this.CountryCode = CountryCode;
    }
    Name: string;
    TransporationType: string;
    ToPortCode: string;
    CountryCode: string;
}
