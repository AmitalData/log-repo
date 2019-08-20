"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var http_1 = require("@angular/http");
var ServiceArgs_1 = require("../../../../Infrastructure/DataContracts/ServiceArgs");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var DocumentsFilingExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var GroupByPipe_1 = require("../../../../Infrastructure/Pipes/GroupByPipe");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var DocumentExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentExtendedService");
var ImageLibraryService_1 = require("../../../../Common/Services/Others/ImageLibraryService");
var DocumentsFilingPMService_1 = require("../../../../Common/Services/StandardPMs/DocumentsFilingPMService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ExportDocumentService_1 = require("../../../../Common/Services/DocumentServices/ExportDocumentService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var GeneralEmailSender_1 = require("../../../../Infrastructure/Helpers/GeneralEmailSender");
var AttachmentsList_1 = require("../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList");
var LogBoxSignatureClientService_1 = require("../../../../Shipment/Services/Others/LogBoxSignatureClientService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var HybridPartnerPMService_1 = require("../../../../Common/Services/StandardPMs/HybridPartnerPMService");
var EntityStatusExtendedListService_1 = require("../../../../Infrastructure/Services/ExtendedLists/EntityStatusExtendedListService");
var LogBoxDocumentsComponent = /** @class */ (function (_super) {
    __extends(LogBoxDocumentsComponent, _super);
    function LogBoxDocumentsComponent(http, serviceArgs, _entityListService) {
        var _this = _super.call(this) || this;
        _this.http = http;
        _this.serviceArgs = serviceArgs;
        _this._entityListService = _entityListService;
        _this.SharedDocs = [];
        _this.Style = {};
        _this.IsRecentSelected = false;
        _this.ShowCancelled = false;
        _this.RequestedCount = 0;
        _this.SignRequiredCount = 0;
        _this.ArchiveDone = new core_1.EventEmitter();
        _this.DataContext = _this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.LogBoxFileLabel = "LogBox File";
        _this.AgentLable = "Agent";
        _this.messageWindow = new MessageWindow_1.MessageWindow();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsPrivateLabel = false;
        _this.AllowSendingDocsToAgent = false;
        _this.DocsSentToAgent = false;
        _this.shipmentTypeId = null;
        _this.showDeleted = false;
        _this.textChanged = "";
        _this.valueChanged = "All";
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
        _this.Timers = {};
        _this.AllHeader = "By Category (0)";
        _this.IsRequestedSelected = false;
        _this.IsDeleteClicked = false;
        _this.archiveButtonText = "Archive";
        _this.BusyIndicatorText = null;
        _this.ShowBusyIndicator = false;
        _this.SignReqPureDocs = [];
        _this.DeletedDocsCount = 0;
        _this.DisableAddDocumentButton = false;
        _this.serviceArgs.http = _this.http;
        _this.SelectedTabCode = "CAT";
        _this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        _this._documentExtendedService = new DocumentExtendedService_1.DocumentExtendedService();
        _this._ImageLibraryService = new ImageLibraryService_1.ImageLibraryService();
        _this._documentsFilingPMService = new DocumentsFilingPMService_1.DocumentsFilingPMService();
        _this._exportDocumentService = new ExportDocumentService_1.ExportDocumentService();
        _this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        _this._LogBoxSignatureClientService = new LogBoxSignatureClientService_1.LogBoxSignatureClientService();
        _this._HybridPartnerPMService = new HybridPartnerPMService_1.HybridPartnerPMService();
        _this._EntityStatusExtendedListService = new EntityStatusExtendedListService_1.EntityStatusExtendedListService();
        _this.CurrentSession.SessionEvent.subscribe(function ($event) {
            if ($event.Name == "DisableBusyIndicator") {
                _this.StopBusyIndicator();
            }
        });
        return _this;
    }
    //ForwardingPartner: any;
    LogBoxDocumentsComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            this.LogBoxFileLabel = SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName + " File";
            this.AgentLable = SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName;
            this.IsPrivateLabel = true;
        }
        else {
            this.IsPrivateLabel = false;
        }
        this.ShipmentSelectedEvent.subscribe(function (res) {
            //this.CurrentSession.StartBusyIndicator("Loading ...");//
            if (res == null) {
                _this.externalDocs = [];
                _this.SignReqDocs = [];
                _this.externalRequestedDocs = [];
                _this.AllHeader = "By Category (0)";
                _this.RequestedCount = 0;
                _this.SignRequiredCount = 0;
                return;
            }
            _this.DisableAddDocumentButton = false;
            _this.StartBusyIndicator("Loading ...");
            var div = document.getElementById("DocsTab");
            _this.Style = { "max-height": div.clientHeight };
            _this.SelectedShipment = res;
            //if (this.RefreshTimer) {
            //    clearTimeout(this.RefreshTimer);
            //}
            //this.RefreshTimer = setInterval(() => this.ReloadDocuments(false), 5000);//setTimeout(() => this.CheckIfSignDone(EntityPm.Id), 2000);
            _this._ShipmentPMService.get(_this.SelectedShipment.Id).subscribe(function (myResult) {
                if (!myResult.HasError) {
                    _this.ShipmentPM = myResult.Result;
                    _this.ShipmentTypeId = myResult.Result.ShipmentTypeId;
                    _this.DocsSentToAgent = myResult.Result.DocsSentToAgent;
                    _this._EntityStatusExtendedListService.getSingle("INPS").subscribe(function (Status) {
                        if (Status.Result && (myResult.Result.StatusId == Status.Result.Id)) {
                            _this.DisableAddDocumentButton = true;
                        }
                    });
                    _this._HybridPartnerPMService.get(myResult.Result.ForwarderPartnerId).subscribe(function (theResult) {
                        if (!theResult.HasError) {
                            _this.AllowSendingDocsToAgent = theResult.Result.AllowSendingDocsToAgent;
                        }
                    });
                    //this._HybridPartnerPMService.get(myResult.Result.ForwardingPartnerId).subscribe(theResult => {
                    //    if (!theResult.HasError) {
                    //        this.ForwardingPartner = theResult.Result;
                    //    }
                    //});
                }
            });
            _this.ReloadDocuments(true);
        });
        this.OnImporterShipmentsFilterChanged.subscribe(function (res) {
            _this.OnImporterShipmentsFilterChangedMethod(res);
        });
    };
    LogBoxDocumentsComponent.prototype.ngAfterViewInit = function () {
    };
    Object.defineProperty(LogBoxDocumentsComponent.prototype, "ShipmentTypeId", {
        get: function () { return this.shipmentTypeId; },
        set: function (newValue) { this.shipmentTypeId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogBoxDocumentsComponent.prototype, "ShowDeleted", {
        get: function () { return this.showDeleted; },
        set: function (newValue) { this.showDeleted = newValue; this.StartBusyIndicator("Loading .."); this.ReloadDocuments(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogBoxDocumentsComponent.prototype, "TextChanged", {
        get: function () {
            if (this.SelectedShipment != null && this.SelectedShipment.TransportModeId == "A") {
                if (Tools_1.AppTool.IsNullOrEmpty(this.SelectedShipment.House)) {
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
        },
        set: function (newValue) {
            if (this.textChanged != newValue) {
                this.textChanged = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogBoxDocumentsComponent.prototype, "ValueChanged", {
        get: function () {
            if (this.SelectedShipment != null && this.SelectedShipment.TransportModeId == "A") {
                if (Tools_1.AppTool.IsNullOrEmpty(this.SelectedShipment.House)) {
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
        },
        set: function (newValue) {
            if (this.valueChanged != newValue) {
                this.valueChanged = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    LogBoxDocumentsComponent.prototype.SetDigitallySigned = function (EntityPm) {
        var _this = this;
        if (this.RefreshTimer) {
            clearTimeout(this.RefreshTimer);
        }
        this.TimerStartDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this.RefreshTimer = setInterval(function () { return _this.ReloadDocuments(false); }, 5000); //setTimeout(() => this.CheckIfSignDone(EntityPm.Id), 2000);
        this.IsDeleteClicked = true;
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "LBDS")) {
            var window = new ConfirmWindow_1.ConfirmWindow();
            window.Width = 450;
            window.Height = 190;
            window.Title = "You have no permession";
            window.YesButtonText = "Ok";
            window.ShowNoButton = false;
            window.Show("Your package doesn't include this module..");
        }
        else {
            //item.IsCustomReference == true
            if (EntityPm.FileExtension.toLowerCase() == "pdf") { //&& FeatureLocator.HasFeaturePermession("General", "LBDS")
                if (EntityPm.IsCustomReference == true) {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Title = "Confirm Deletion";
                    confirmWindow.Width = 450;
                    confirmWindow.Height = 190;
                    confirmWindow.YesButtonText = "Ok";
                    confirmWindow.NoButtonText = "Cancel";
                    confirmWindow.Show("Document was already sent to customs and cannot be updated , we will create a copy of them for the customs agent.");
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.RunSignBusyIndicator(true, EntityPm.Id);
                            //var element = document.getElementById('BusyIndecator' + EntityPm.Id);
                            //if (element) {
                            //    element.style.display = 'block';
                            //}
                            _this.IsDeleteClicked = true;
                            EntityPm.DontAddToQueue = true;
                            EntityPm.SignRequestByUserEmail = SessionLocator_1.SessionLocator.LoggedUserPM.Email;
                            EntityPm.SignDueDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                            var CurrMin = EntityPm.SignDueDate.getMinutes() + 5;
                            EntityPm.SignDueDate.setMinutes(CurrMin);
                            EntityPm.CancellSignRequest = false;
                            //this._documentsFilingPMService.update(EntityPm).subscribe(myResult => {
                            _this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPm).subscribe(function (Result) {
                                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "Sign Document");
                                if (Result.Result != null && Result.Result.HasError) {
                                    _this.RunSignBusyIndicator(false, EntityPm.Id);
                                    _this.messageWindow.Width = 300;
                                    _this.messageWindow.Height = 150;
                                    _this.messageWindow.Title = "Warning !";
                                    _this.messageWindow.Message = Result.Result.ErrorsArray[0];
                                    _this.messageWindow.Show(_this.messageWindow.Message);
                                }
                                else if (Result.Result == null) {
                                    _this.messageWindow.Width = 300;
                                    _this.messageWindow.Height = 150;
                                    _this.messageWindow.Title = "Warning !";
                                    _this.messageWindow.Message = "Please make sure that cloud sign app installed to your computer.";
                                    _this.messageWindow.Show(_this.messageWindow.Message);
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
                    EntityPm.SignRequestByUserEmail = SessionLocator_1.SessionLocator.LoggedUserPM.Email;
                    EntityPm.SignDueDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                    var CurrMin = EntityPm.SignDueDate.getMinutes() + 5;
                    EntityPm.SignDueDate.setMinutes(CurrMin);
                    EntityPm.CancellSignRequest = false;
                    //this._documentsFilingPMService.update(EntityPm).subscribe(myResult => {
                    this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPm).subscribe(function (Result) {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "Sign Document");
                        if (Result.Result != null && Result.Result.HasError) {
                            _this.RunSignBusyIndicator(false, EntityPm.Id);
                            _this.messageWindow.Width = 300;
                            _this.messageWindow.Height = 150;
                            _this.messageWindow.Title = "Warning !";
                            _this.messageWindow.Message = Result.Result.ErrorsArray[0];
                            _this.messageWindow.Show(_this.messageWindow.Message);
                        }
                        else if (Result.Result == null) {
                            _this.messageWindow.Width = 300;
                            _this.messageWindow.Height = 150;
                            _this.messageWindow.Title = "Warning !";
                            _this.messageWindow.Message = "Please make sure that cloud sign app installed to your computer.";
                            _this.messageWindow.Show(_this.messageWindow.Message);
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
                var window = new ConfirmWindow_1.ConfirmWindow();
                window.Width = 450;
                window.Height = 190;
                window.Title = "Warning !";
                window.YesButtonText = "Ok";
                window.ShowNoButton = false;
                window.Show("You can only sign PDF files ..");
            }
        }
    };
    LogBoxDocumentsComponent.prototype.CheckIfSignDone = function (DocId) {
        var _this = this;
        this._documentsFilingPMService.get(DocId).subscribe(function (res) {
            var pmResponse = res;
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
                    _this.SendSignedDocumentToAgent(currentdocument);
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
    };
    LogBoxDocumentsComponent.prototype.SendSignedDocumentToAgent = function (Document) {
        if (Document.IsSharedWithCustomer == true) {
            Document.DontAddToQueue = false;
            Document.ForwarderDocumentId = null;
            this._documentsFilingPMService.update(Document).subscribe(function (myResult) {
            });
        }
    };
    LogBoxDocumentsComponent.prototype.CancelSignProcess = function (EntityPM) {
        var _this = this;
        this.IsDeleteClicked = true;
        EntityPM.DontAddToQueue = true;
        EntityPM.SignRequestByUserEmail = null;
        EntityPM.CancellSignRequest = true;
        //this._documentsFilingPMService.update(EntityPm).subscribe(myResult => {
        this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPM).subscribe(function (Result) {
            _this.RunSignBusyIndicator(false, EntityPM.Id);
        });
    };
    LogBoxDocumentsComponent.prototype.RunSignBusyIndicator = function (IsStart, Id) {
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
    };
    LogBoxDocumentsComponent.prototype.OnImporterShipmentsFilterChangedMethod = function (res) {
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
    };
    LogBoxDocumentsComponent.prototype.AddDocumentClick = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("DocumentsFiling").subscribe(function (response1) {
            var windowArgs = {};
            windowArgs.SelectedShipment = _this.SelectedShipment;
            windowArgs.IsNewDocument = true;
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.WindowArgs = windowArgs;
            logitudeWindow.Width = 960;
            logitudeWindow.Height = 620;
            logitudeWindow.Title = "";
            logitudeWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditImporterDocumentComponent');
            logitudeWindow.WindowClosed.subscribe(function ($event) {
                _this.ReloadDocuments();
            });
        });
    };
    LogBoxDocumentsComponent.prototype.DocumentClicked = function (Document) {
        var _this = this;
        if (this.IsDeleteClicked) {
            this.IsDeleteClicked = false;
        }
        else {
            this._entityResourceService.getEntityResourceByTableName("DocumentsFiling").subscribe(function (response1) {
                var windowArgs = {};
                windowArgs.SelectedShipment = _this.SelectedShipment;
                windowArgs.IsNewDocument = false;
                windowArgs.EntityPm = Document;
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.WindowArgs = windowArgs;
                logitudeWindow.Width = 960;
                logitudeWindow.Height = 620;
                logitudeWindow.Title = "";
                logitudeWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditImporterDocumentComponent');
                logitudeWindow.WindowClosed.subscribe(function ($event) {
                    _this.ReloadDocuments();
                });
            });
        }
    };
    LogBoxDocumentsComponent.prototype.DeleteDocumentClicked = function (item) {
        var _this = this;
        this.IsDeleteClicked = true;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = "Confirm Deletion";
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.YesButtonText = "Delete";
        confirmWindow.NoButtonText = "Cancel";
        confirmWindow.Show("Are you sure you want to delete this document ?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.StartBusyIndicator("Loading ..");
                item.IsDeleted = true;
                item.HasFile = false;
                item.FileSize = null;
                item.FileExtension = null;
                item.FileName = null;
                item.DocumentId = null;
                _this._documentsFilingPMService.update(item).subscribe(function (myResult) {
                    _this.ReloadDocuments();
                    //this.StopBusyIndicator();
                });
            }
            else {
            }
        });
        //alert(item.Id);
    };
    LogBoxDocumentsComponent.prototype.DeleteDocumentFile = function (item) {
        var _this = this;
        this.IsDeleteClicked = true;
        if (item.IsCustomReference) {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "document was sent to custom !";
            this.messageWindow.Show("The document was sent to custom, therefore, it can't be deleted.");
        }
        else {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Title = "Confirm Deletion";
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.YesButtonText = "Delete";
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.Show("Are you sure you want to delete this document ?");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.StartBusyIndicator("Loading ..");
                    item.IsDeleted = true;
                    item.DontDeleteRealFile = true;
                    //item.HasFile = false;
                    //item.FileSize = null;
                    //item.FileExtension = null;
                    //item.FileName = null;
                    //item.DocumentId = null;
                    _this._documentsFilingPMService.update(item).subscribe(function (myResult) {
                        _this.ReloadDocuments();
                        //this.StopBusyIndicator();
                    });
                }
                else {
                }
            });
        }
    };
    LogBoxDocumentsComponent.prototype.SendDocumentFile = function (item) {
        this.IsDeleteClicked = true;
        var attachment = new AttachmentsList_1.AttachmentsList();
        attachment.Tenant = SessionLocator_1.SessionLocator.Tenant;
        attachment.DocumentTypeCopyNameWithDocumentTypeName = item.FileName;
        attachment.FileSize = item.FileSize;
        attachment.ShowRemoveLink = true;
        attachment.Id = item.DocumentId;
        attachment.FileExtension = item.FileExtension;
        var attachmentsList = new Array();
        attachmentsList.push(attachment);
        if (!this.EmailSender || (this.EmailSender && !this.EmailSender.LoadingSendingComponent)) {
            this.EmailSender = new GeneralEmailSender_1.GeneralEmailSender("Shipment", item.DocumentTypeCode, this.SelectedShipment.Id, this.SelectedShipment.ShipmentNumber, null, null, item.Id, item.Description, attachmentsList);
            this.EmailSender.SendMessage();
        }
    };
    LogBoxDocumentsComponent.prototype.DownloadDocumentFile = function (item) {
        this.IsDeleteClicked = true;
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "Document Viewed");
        //this._ImageLibraryService.DownloadFile(item.DocumentId, item.FileExtension, item.Folder, SessionLocator.Tenant).subscribe(res => {
        var EntityNumber = "";
        if (this.SelectedShipment != null) {
            if (this.SelectedShipment.ForwarderShipmentNumber == null) {
                EntityNumber = this.SelectedShipment.ShipmentNumber;
            }
            else {
                EntityNumber = this.SelectedShipment.ForwarderShipmentNumber;
            }
        }
        var documentName = item.DocumentId + "*" + item.DocumentTypeCode + "-" + (!Tools_1.AppTool.IsNullOrEmpty(EntityNumber) ? EntityNumber : item.EntityId) + "-" + item.Code; // +"." + CurrentDocument.Extension;
        DownloadManager_1.DownloadManager.DownloadPage(documentName);
        //});
    };
    LogBoxDocumentsComponent.prototype.ShareWithAgent = function (EntityPm) {
        var _this = this;
        this.IsDeleteClicked = true;
        if ((EntityPm.IsSharedWithCustomer == false && EntityPm.IsSharedWithForwarder == false)) {
            if (!EntityPm.IsSharedWithForwarder) {
                //var window = new ConfirmWindow();
                //window.Title = "Confirm sharing";
                //window.Width = 450;
                //window.Height = 190;
                //window.YesButtonText = "Ok";
                //window.NoButtonText = "Cancel";
                //window.Show("Are you sure you want to share this document with agent?");
                //window.WindowClosed.subscribe((event: any) => {
                //    if (window.Yes) {
                //    }
                //    else {
                //    }
                //});
                //this.CurrentSession.StartBusyIndicator("Loading ...");//
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "Share Document With Agent");
                this.StartBusyIndicator("Loading ...");
                if (EntityPm.IsSharedWithForwarder == true) {
                    EntityPm.IsSharedWithForwarder = false;
                    EntityPm.DontAddToQueue = true;
                    //BlueSharedWithAgentVisibility = Visibility.Visible;
                    //GraySharedWithAgentVisibility = Visibility.Collapsed;
                }
                //&& (!AppTool.IsNullOrEmpty(this.SelectedShipment.ForwarderShipmentNumber) || SessionLocator.PrivateLableSettings)
                else {
                    EntityPm.IsSharedWithForwarder = true;
                    if (Tools_1.AppTool.IsNullOrEmpty(this.SelectedShipment.ForwarderShipmentNumber)) {
                        EntityPm.DontAddToQueue = true;
                    }
                    else {
                        EntityPm.DontAddToQueue = false;
                    }
                    //BlueSharedWithAgentVisibility = Visibility.Collapsed;
                    //GraySharedWithAgentVisibility = Visibility.Visible;
                }
                this._documentsFilingPMService.update(EntityPm).subscribe(function (myResult) {
                    //this.CurrentSession.StopBusyIndicator();//
                    _this.StopBusyIndicator();
                    //this.IssharedWithAgentButtonEnabled = false;
                    _this.ReloadDocuments();
                    //this.StopBusyIndicator();
                });
                //if (!importerDocumentDataViewModel.EntityPM.IsSharedWithForwarder) {
                //    importerDocumentDataViewModel.EntityPM.DontAddToQueue = true;
                //}
            }
        }
    };
    LogBoxDocumentsComponent.prototype.RefreshBtnClick = function () {
        this.ReloadDocuments();
    };
    Object.defineProperty(LogBoxDocumentsComponent.prototype, "ArchiveButtonText", {
        get: function () {
            this.archiveButtonText = (this.SelectedShipment && this.SelectedShipment.IsOperationalClosed == true ? "Undo Archive" : "Archive");
            return this.archiveButtonText;
        },
        set: function (newValue) { this.archiveButtonText = newValue; },
        enumerable: true,
        configurable: true
    });
    LogBoxDocumentsComponent.prototype.ArchiveClicked = function () {
        var _this = this;
        //this.CurrentSession.StartBusyIndicator("Saving ...");
        this.StartBusyIndicator("Saving ...");
        this._ShipmentPMService.get(this.SelectedShipment.Id).subscribe(function (myResult) {
            if (!myResult.HasError) {
                myResult.Result.IsImporterShipment = true;
                if (_this.ArchiveButtonText == "Archive") {
                    myResult.Result.IsOperationalClosed = true;
                    _this.SelectedShipment.IsOperationalClosed = true;
                }
                else {
                    myResult.Result.IsOperationalClosed = false;
                    _this.SelectedShipment.IsOperationalClosed = false;
                }
                var action = "A";
                myResult.Result.GrossWeightUnitCode = "KG";
                myResult.Result.DimensionsUnitCode = "Cm";
                myResult.Result.ChargeableWeightUnitCode = "KG";
                myResult.Result.VolumeUnitCode = "CBF";
                _this._ShipmentPMService.update(myResult.Result).subscribe(function (myResult) {
                    if (!myResult.HasError) {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "Shipment Archived");
                        if (myResult.Result.IsOperationalClosed == true) {
                            _this.ArchiveButtonText == "Undo Archive";
                            action = "A";
                        }
                        else {
                            _this.ArchiveButtonText == "Archive";
                            action = "U";
                        }
                        //this.CurrentSession.StopBusyIndicator();
                        _this.StopBusyIndicator();
                        //this.CurrentSession.FireEvent({ Name: "ReloadPublicShipments" });
                        _this.ArchiveDone.emit({ Ship: _this.SelectedShipment, Action: action });
                    }
                    else {
                        //this.CurrentSession.StopBusyIndicator();
                        _this.StopBusyIndicator();
                    }
                });
            }
            else {
                //this.ValidationErrorsList = myResult.ErrorsArray;
                //this.CurrentSession.StopBusyIndicator();
                _this.StopBusyIndicator();
            }
        });
    };
    LogBoxDocumentsComponent.prototype.StartBusyIndicator = function (myText) {
        this.BusyIndicatorText = myText;
        this.ShowBusyIndicator = true;
    };
    LogBoxDocumentsComponent.prototype.StopBusyIndicator = function () {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    };
    LogBoxDocumentsComponent.prototype.ReloadDocuments = function (ChangeTab) {
        var _this = this;
        if (ChangeTab === void 0) { ChangeTab = false; }
        var MyDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc(); // new Date();
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
            this.RefreshTimer = setInterval(function () { return _this.ReloadDocuments(false); }, 2000);
        }
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Shipment"; })[0];
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
        this._documentsFilingExtendedPMService.getAllDocumentsFilingsByEntityIdAndObjectTable(this.SelectedShipment.Id, ObjectTable.Id, "I", SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var Result = [];
            var ResultSignReq = [];
            _this.DeletedDocsCount = res.Result.filter(function (a) { return a.IsDeleted == true; }).length;
            if (!_this.ShowDeleted) {
                if (res.Result) {
                    Result = res.Result.filter(function (a) { return a.IsDeleted == false; });
                    ResultSignReq = res.Result.filter(function (a) { return a.IsDeleted == false && a.IsDigitalSignRequired == true; });
                }
            }
            else {
                Result = res.Result;
                ResultSignReq = res.Result.filter(function (a) { return a.IsDigitalSignRequired == true; });
            }
            _this.SharedDocs = res.Result.filter(function (a) { return a.IsDeleted == false && a.IsSharedWithForwarder == true; });
            var temp = [];
            var tempSignReq = [];
            _this.SignReqPureDocs = ResultSignReq;
            if (_this.IsRecentSelected) {
                temp = new GroupByPipe_1.GroupByPipe().transform(Result, "CreateDateWords");
                tempSignReq = new GroupByPipe_1.GroupByPipe().transform(ResultSignReq, "CreateDateWords");
                _this.AllHeader = "By Date";
            }
            else {
                temp = new GroupByPipe_1.GroupByPipe().transform(Result, "DocumentCategoryName");
                tempSignReq = new GroupByPipe_1.GroupByPipe().transform(ResultSignReq, "DocumentCategoryName");
                _this.AllHeader = "By Category";
            }
            _this.externalDocs = temp;
            _this.SignReqDocs = tempSignReq; //.filter(a => a.IsDigitalSignRequired);
            var tempArray = [];
            var Count = 0;
            _this.externalDocs.forEach(function (docin) {
                Count += docin.value.length;
            });
            var SignReqCount = 0;
            _this.SignReqDocs.forEach(function (docin) {
                SignReqCount += docin.value.length;
            });
            var TempArr = [];
            if (_this.IsRecentSelected) {
                var Today = _this.externalDocs.filter(function (a) { return a.key == "Today"; });
                var LastDays = _this.externalDocs.filter(function (a) { return a.key == "Last 7 Days"; });
                var AllLeft = _this.externalDocs.filter(function (a) { return a.key != "Last 7 Days" && a.key != "Today"; });
                var TodaySign = _this.SignReqDocs.filter(function (a) { return a.key == "Today"; });
                var LastDaysSign = _this.SignReqDocs.filter(function (a) { return a.key == "Last 7 Days"; });
                var AllLeftSign = _this.SignReqDocs.filter(function (a) { return a.key != "Last 7 Days" && a.key != "Today"; });
                //AllLeft.forEach((docin) => {
                //    TempArr.push(docin);
                //});
                //AllLeft.forEach((docin) => {
                //    TempArr.push(docin);
                //});
                //AllLeft.forEach((docin) => {
                //    TempArr.push(docin);
                //});
                _this.externalDocs = (Today.concat(LastDays)).concat(AllLeft); //this.externalDocs.sort(a => a.key == "Today" ? 1 : a.key == "Last 7 Days" ? 2 : 3);
                _this.SignReqDocs = (TodaySign.concat(LastDaysSign)).concat(AllLeftSign); //this.externalDocs.sort(a => a.key == "Today" ? 1 : a.key == "Last 7 Days" ? 2 : 3);
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
                var OperationalDocuments = _this.externalDocs.filter(function (a) { return a.key == "Operational Documents"; });
                var AccountingDocuments = _this.externalDocs.filter(function (a) { return a.key == "Accounting Documents"; });
                var AllLeft = _this.externalDocs.filter(function (a) { return a.key != "Operational Documents" && a.key != "Accounting Documents"; });
                var OperationalDocumentsSign = _this.SignReqDocs.filter(function (a) { return a.key == "Operational Documents"; });
                var AccountingDocumentsSign = _this.SignReqDocs.filter(function (a) { return a.key == "Accounting Documents"; });
                var AllLeftSign = _this.SignReqDocs.filter(function (a) { return a.key != "Operational Documents" && a.key != "Accounting Documents"; });
                _this.externalDocs = (OperationalDocuments.concat(AccountingDocuments)).concat(AllLeft); //this.externalDocs.sort(a => { return (a.key == "Operational Documents" ? 1 : a.key == "Accounting Documents" ? 2 : 3) });
                _this.SignReqDocs = (OperationalDocumentsSign.concat(AccountingDocumentsSign)).concat(AllLeftSign); //this.externalDocs.sort(a => { return (a.key == "Operational Documents" ? 1 : a.key == "Accounting Documents" ? 2 : 3) });
            }
            if (_this.IsRecentSelected) {
                _this.AllHeader = "By Date ( " + Count + " )";
            }
            else {
                _this.AllHeader = "By Category ( " + Count + " )";
            }
            _this.SignRequiredCount = SignReqCount;
            if (ChangeTab == true && _this.IsRequestedSelected && _this.SignReqPureDocs.length > 0) {
                _this.SelectedTabCode = 'SREQ';
            }
            //this.CurrentSession.StopBusyIndicator();
            _this.StopBusyIndicator();
        }, function (error) {
            var dd = error;
            //this.CurrentSession.StopBusyIndicator();
            _this.StopBusyIndicator();
        });
        this._documentsFilingExtendedPMService.getRequestedDocumentsFilingsByEntityIdAndObjectTable(this.SelectedShipment.Id, ObjectTable.Id, "I", SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var Count = 0;
            var Result = [];
            //this.DeletedDocsCount = res.Result.filter(a => a.IsDeleted == true).length;
            if (!_this.ShowDeleted) {
                if (res.Result) {
                    Result = res.Result.filter(function (a) { return a.IsDeleted == false; });
                }
            }
            else {
                Result = res.Result;
            }
            var temp = [];
            if (_this.IsRecentSelected) {
                temp = new GroupByPipe_1.GroupByPipe().transform(Result, "CreateDateWords");
            }
            else {
                temp = new GroupByPipe_1.GroupByPipe().transform(Result, "DocumentCategoryName");
            }
            _this.externalRequestedDocs = temp;
            _this.externalRequestedDocs.forEach(function (docin) {
                Count += docin.value.length;
            });
            _this.RequestedCount = Count;
        }, function (error) {
            var dd = error;
        });
    };
    LogBoxDocumentsComponent.prototype.DownloadAllClicked = function () {
        var windowArgs = {};
        var OTable = window.ObjectTables.filter(function (a) { return a.Name == "Shipment"; })[0];
        windowArgs.ObjectTableId = OTable.Id;
        windowArgs.ShipmentId = this.SelectedShipment.Id;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Width = 570;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = "Exporting All Documents To ZIP File";
        logitudeWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/DownloadAllFilesComponent');
    };
    LogBoxDocumentsComponent.prototype.ShareDocumentsClick = function () {
        var _this = this;
        if (this.SharedDocs.length > 0) {
            var window = new ConfirmWindow_1.ConfirmWindow();
            window.Title = "Confirm sharing";
            window.Width = 450;
            window.Height = 190;
            window.YesButtonText = "Ok";
            window.NoButtonText = "Cancel";
            window.Show("The shared documents will be send to the agent .");
            window.WindowClosed.subscribe(function (event) {
                if (window.Yes) {
                    var SharedDocsIds = [];
                    _this.CurrentSession.StartBusyIndicator("Sharing ...");
                    _this.SharedDocs.forEach(function (docin) {
                        SharedDocsIds.push(docin.Id);
                    });
                    _this._documentsFilingExtendedPMService.ShareDocumentsWithAgent(SharedDocsIds).subscribe(function (myResult) {
                        if (!myResult.HasError) {
                            _this.DocsSentToAgent = true;
                            _this._EntityStatusExtendedListService.getSingle("INPS").subscribe(function (Status) {
                                if (Status.Result) {
                                    _this._ShipmentPMService.get(_this.SelectedShipment.Id).subscribe(function (myShipmentResult) {
                                        if (!myShipmentResult.HasError) {
                                            _this.ShipmentPM = myShipmentResult.Result;
                                            _this.ShipmentPM.StatusId = Status.Result.Id;
                                            _this.ShipmentPM.ShipperReference1 = _this.ShipmentPM.CustomerReference1;
                                            _this.ShipmentPM.ShipperReference2 = _this.ShipmentPM.CustomerReference2;
                                            _this.ShipmentPM.ShipperId = _this.ShipmentPM.CustomerId;
                                            _this.ShipmentPM.DontAddToForwarderQueue = true;
                                            _this._ShipmentPMService.update(_this.ShipmentPM).subscribe(function (myResult) {
                                                if (!myResult.HasError) {
                                                    _this.DisableAddDocumentButton = true;
                                                    _this.CurrentSession.SessionEvent.emit({ Name: "ReloadShipments" });
                                                    _this.CurrentSession.StopBusyIndicator();
                                                }
                                                //else {
                                                //    this.ValidationErrorsList = myResult.ErrorsArray;
                                                //}
                                            });
                                        }
                                    });
                                }
                                else {
                                    _this.CurrentSession.StopBusyIndicator();
                                    _this.messageWindow.Width = 300;
                                    _this.messageWindow.Height = 150;
                                    _this.messageWindow.Title = "No Status In progress !";
                                    _this.messageWindow.Show("There are no Status In progress.");
                                }
                            });
                        }
                    });
                }
                else {
                }
            });
        }
        else {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "No Shared Documents !";
            this.messageWindow.Show("There are no shared documents, please share the related documents before.");
        }
    };
    LogBoxDocumentsComponent.prototype.SignAllClick = function () {
        var _this = this;
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "LBDS")) {
            var window = new ConfirmWindow_1.ConfirmWindow();
            window.Width = 450;
            window.Height = 190;
            window.Title = "You have no permession";
            window.YesButtonText = "Ok";
            window.ShowNoButton = false;
            window.Show("Your package doesn't include this module..");
        }
        else {
            var SentToCustomesDocs = this.SignReqPureDocs.filter(function (a) { return a.IsCustomReference == true; });
            if (SentToCustomesDocs.length > 0) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Title = "Confirm Deletion";
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.YesButtonText = "Ok";
                confirmWindow.NoButtonText = "Cancel";
                confirmWindow.Show("Some Documents you are trying to sign was already sent to customs and cannot be updated , we will create a copy of them for the customs agent.");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.SignAllLogic();
                    }
                });
            }
            else {
                this.SignAllLogic();
            }
        }
    };
    LogBoxDocumentsComponent.prototype.SignAllLogic = function () {
        var _this = this;
        var Ids = [];
        this.SignReqPureDocs.forEach(function (docin) {
            if (docin.IsDigitallySigned == false) {
                Ids.push(docin.Id);
            }
        });
        this._LogBoxSignatureClientService.GetMultiSignRequestReceived(Ids).subscribe(function (Result) {
            //ServiceLocator.SendTotangoUserActivity("LogBox", "Sign Document");
            if (Result.Result != null && Result.Result.HasError) {
                //this.RunSignBusyIndicator(false, EntityPm.Id);
                _this.messageWindow.Width = 300;
                _this.messageWindow.Height = 150;
                _this.messageWindow.Title = "Warning !";
                _this.messageWindow.Message = Result.Result.ErrorsArray[0];
                _this.messageWindow.Show(_this.messageWindow.Message);
            }
            else if (Result.Result == null) {
                _this.messageWindow.Width = 300;
                _this.messageWindow.Height = 150;
                _this.messageWindow.Title = "Warning !";
                _this.messageWindow.Message = "Please make sure that cloud sign app installed to your computer.";
                _this.messageWindow.Show(_this.messageWindow.Message);
            }
            else {
                if (_this.RefreshTimer) {
                    clearTimeout(_this.RefreshTimer);
                }
                _this.TimerStartDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                _this.ReloadDocuments();
                _this.RefreshTimer = setInterval(function () { return _this.ReloadDocuments(false); }, 5000);
                //if (this.Timers[EntityPm.Id]) {
                //    clearTimeout(this.Timers[EntityPm.Id]);
                //}
                //this.Timers[EntityPm.Id] = setInterval(() => this.CheckIfSignDone(EntityPm.Id), 5000);//setTimeout(() => this.CheckIfSignDone(EntityPm.Id), 2000);
            }
        });
    };
    LogBoxDocumentsComponent.prototype.OnPackagesClick = function (Title) {
        var windowArgs = {};
        windowArgs.ShipmentPM = this.ShipmentPM;
        windowArgs.Title = Title;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Width = 690;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = Title;
        logitudeWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/LogBoxPackagesComponent');
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogBoxDocumentsComponent.prototype, "ArchiveDone", void 0);
    LogBoxDocumentsComponent = __decorate([
        core_1.Component({
            selector: 'LogBoxDocuments',
            moduleId: module.id,
            templateUrl: './LogBoxDocumentsComponent.html',
            //providers: [ EntityListService, DocumentsFilingExtendedPMService],
            inputs: ['ShipmentSelectedEvent', 'OnImporterShipmentsFilterChanged', 'SearchText'],
        }),
        __metadata("design:paramtypes", [http_1.Http, ServiceArgs_1.ServiceArgs, EntityListService_1.EntityListService])
    ], LogBoxDocumentsComponent);
    return LogBoxDocumentsComponent;
}(BaseComponent_1.BaseComponent));
exports.LogBoxDocumentsComponent = LogBoxDocumentsComponent;
//# sourceMappingURL=LogBoxDocumentsComponent.js.map