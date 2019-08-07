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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../Tools");
var Args_1 = require("../../Args");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var Tools_2 = require("../../../Infrastructure/Tools");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ShipmentDomainService_1 = require("../../../Shipment/Services/ShipmentDomainService");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var ShipmentHelperComponent = /** @class */ (function () {
    function ShipmentHelperComponent(entityArgs, cd) {
        this.entityArgs = entityArgs;
        this.cd = cd;
        this.NotesList = [];
        this.IsFollowupsVisible = false;
        this.IsAnalyzeChampXMLButtonVisible = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.IsUpdateSharedAgentButtonVisible = false;
        this.IsShareDocumentsButtonVisible = false;
        this.IsShareManifestButtonVisible = false;
        this.AWBWizardButtonLabel = null;
        this.AWBImportWizardButtonLabel = null;
        this.isAWBButtonClicked = false;
        this.isAWBImportButtonClicked = false;
        this.isSendToCustomClicked = false;
        this.isShippingInstructionsClicked = false;
        this.IsAWBWizardButtonVisible = false;
        this.IsImportAWBWizardButtonVisible = false;
        this.IsSendToCustomVisible = false;
        this.IsShippingInstructionsVisible = false;
        this.IsABMVisible = false;
        this.IsAESVisible = false;
        this.IsATMSVisible_BOL = false;
        this.IsATMSVisible_VOG = false;
        //ShareDocument
        this.isSharingDocumentRequested = false;
        //ShareManifest
        this.isShareManifestRequested = false;
        //UpdateAgentShareManifest
        this.isUpdateSharedAgentRequested = false;
        this.IsFollowupsVisible = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Followups");
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            this.ShowHideShippingInstructionsButton();
            if (this.EntityPM.DirectionId == "E" && this.EntityPM.TransportModeId == "A") {
                if (FeatureLocator_1.FeatureLocator.IsPackage_DVMT()) {
                    this.IsAnalyzeChampXMLButtonVisible = true;
                }
            }
            this.Listen();
            this.BuildComponent();
        }
    }
    ShipmentHelperComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                        if (_this.isAWBButtonClicked) {
                            _this.ImportWizard();
                        }
                        else if (_this.isAWBImportButtonClicked) {
                            _this.ImportAWBWizard();
                        }
                        else if (_this.isSendToCustomClicked) {
                            _this.SendToCustom();
                        }
                        else if (_this.isShippingInstructionsClicked) {
                            _this.ShowINTTRAWizard();
                        }
                        else if (_this.isShareManifestRequested) {
                            _this.StartShareManifest();
                        }
                        else if (_this.isUpdateSharedAgentRequested) {
                            _this.StartShareManifest(true);
                        }
                        else if (_this.isSharingDocumentRequested) {
                            _this.StartSharingDocument();
                        }
                    }
                    _this.isShareManifestRequested = false;
                    _this.isUpdateSharedAgentRequested = false;
                    _this.isSharingDocumentRequested = false;
                    _this.isAWBImportButtonClicked = false;
                    _this.isAWBButtonClicked = false;
                    _this.isSendToCustomClicked = false;
                    _this.isShippingInstructionsClicked = false;
                });
            }
            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                        _this.ShowHideShippingInstructionsButton();
                        _this.BuildComponent();
                        if (_this.EntityPM.DirectionId == "E" && _this.EntityPM.TransportModeId == "A") {
                            if (FeatureLocator_1.FeatureLocator.IsPackage_DVMT()) {
                                _this.IsAnalyzeChampXMLButtonVisible = true;
                            }
                        }
                    }
                });
            }
        }
    };
    ShipmentHelperComponent.prototype.ShowHideShippingInstructionsButton = function () {
        this.IsShippingInstructionsVisible = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "ShippingInstructions")) {
            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "E") {
                if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C") {
                    var isFCLEntity = Tools_2.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                    if (isFCLEntity) {
                        this.IsShippingInstructionsVisible = true;
                    }
                }
            }
        }
    };
    ShipmentHelperComponent.prototype.ImportAWBWizard = function () {
        var _this = this;
        var myAWBWizardArgs = new Args_1.AWBWizardArgs();
        myAWBWizardArgs.EntityPM = this.EntityPM;
        myAWBWizardArgs.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = Tools_1.ShipmentTool.GetAWBWizardHeader(this.EntityPM.ShipmentLevelCode, "E");
        logWindow.WindowArgs = myAWBWizardArgs;
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            _this.CurrentSession.FireEvent("AWBWizardClosed");
        });
    };
    ShipmentHelperComponent.prototype.ngOnDestroy = function () {
        Tools_2.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_2.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ShipmentHelperComponent.prototype.BuildComponent = function () {
        this.EntityTitle = this.EntityPM.ShipmentLevelCode == "C" ? "Master" : "Shipment";
        this.SetAWBWizardButton();
        this.setImportAWBWizardButton();
        var isUpdateSharedAgentButtonVisible = false;
        var isShareDocumentsButtonVisible = false;
        var isShareManifestButtonVisible = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "AgentSharedManifest")) {
            if (this.EntityPM.DirectionId == "E" && (this.EntityPM.ShipmentLevelCode == "C" || this.EntityPM.ShipmentLevelCode == "D")) {
                isShareManifestButtonVisible = true;
            }
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AgentSharedManifest", "UPDATESHAREDAGENT")) {
            if (this.EntityPM.DirectionId == "E" && (this.EntityPM.ShipmentLevelCode == "C" || this.EntityPM.ShipmentLevelCode == "D")) {
                isUpdateSharedAgentButtonVisible = true;
            }
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("AgentSharedDocument", "NEW")) {
            if (this.EntityPM.DirectionId == "E" && (this.EntityPM.ShipmentLevelCode == "C" || this.EntityPM.ShipmentLevelCode == "D")) {
                isShareDocumentsButtonVisible = true;
            }
        }
        this.IsUpdateSharedAgentButtonVisible = isUpdateSharedAgentButtonVisible;
        this.IsShareDocumentsButtonVisible = isShareDocumentsButtonVisible;
        this.IsShareManifestButtonVisible = isShareManifestButtonVisible;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "ShipmentCustomsTransmission")) {
            this.CheckArtemusVisibility_BOL();
            this.CheckArtemusVisibility_VOG();
            this.CheckABMVisibility();
            this.CheckAESVisibility();
            if (this.IsABMVisible || this.IsAESVisible || this.IsATMSVisible_BOL || this.IsATMSVisible_VOG) {
                this.IsSendToCustomVisible = true;
            }
        }
    };
    ShipmentHelperComponent.prototype.setImportAWBWizardButton = function () {
        this.IsImportAWBWizardButtonVisible = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "IMPORTAWBWIZARD")) {
            if (this.EntityPM.DirectionId == "I") {
                this.IsImportAWBWizardButtonVisible = true;
            }
        }
    };
    ShipmentHelperComponent.prototype.SetAWBWizardButton = function () {
        this.AWBWizardButtonLabel = Tools_1.ShipmentTool.GetAWBWizardHeader(this.EntityPM.ShipmentLevelCode, this.EntityPM.DirectionId);
        this.AWBImportWizardButtonLabel = Tools_1.ShipmentTool.GetAWBWizardHeader(this.EntityPM.ShipmentLevelCode, "E");
        var isButtonVisible = false;
        if (this.EntityPM.TransportModeId == "A" && !SessionLocator_1.SessionLocator.TenantPM.IsHybrid) {
            var isFullWizard = false;
            if (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "R") {
                isFullWizard = true;
            }
            else if (this.EntityPM.DirectionId == "D") {
                if (!FeatureLocator_1.FeatureLocator.IsPackage_EAWB()) {
                    isFullWizard = true;
                }
            }
            if (isFullWizard) {
                isButtonVisible = true;
            }
            else {
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SENDREQUEST")) {
                    isButtonVisible = true;
                }
            }
        }
        this.IsAWBWizardButtonVisible = isButtonVisible;
    };
    ShipmentHelperComponent.prototype.AWBButtonClicked = function () {
        if (!this.isAWBButtonClicked) {
            this.isAWBButtonClicked = true;
            if (this.entityArgs.EditComponent) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    };
    ShipmentHelperComponent.prototype.AWBImportButtonClicked = function () {
        if (!this.isAWBImportButtonClicked) {
            this.isAWBImportButtonClicked = true;
            if (this.entityArgs.EditComponent) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    };
    ShipmentHelperComponent.prototype.ImportWizard = function () {
        var _this = this;
        var isFullWizard = Tools_1.ShipmentTool.IsFullAWBWizard(this.EntityPM.DirectionId);
        if (isFullWizard) {
            var myAWBWizardArgs = new Args_1.AWBWizardArgs();
            myAWBWizardArgs.EntityPM = this.EntityPM;
            myAWBWizardArgs.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = Tools_1.ShipmentTool.GetAWBWizardHeader(this.EntityPM.ShipmentLevelCode, this.EntityPM.DirectionId);
            logWindow.WindowArgs = myAWBWizardArgs;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                _this.CurrentSession.FireEvent("AWBWizardClosed");
            });
        }
        else {
            var myFSRWizardArgs = new Args_1.FSRWizardArgs();
            myFSRWizardArgs.EntityPM = this.EntityPM;
            myFSRWizardArgs.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = Tools_1.ShipmentTool.GetAWBWizardHeader(this.EntityPM.ShipmentLevelCode, this.EntityPM.DirectionId);
            logWindow.WindowArgs = myFSRWizardArgs;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/FSRWizard/FSRWizardComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                _this.CurrentSession.FireEvent("AWBWizardClosed");
            });
        }
    };
    // Send To Custom 
    ShipmentHelperComponent.prototype.SendToCustomsClicked = function () {
        if (!this.isSendToCustomClicked) {
            this.isSendToCustomClicked = true;
            if (this.entityArgs.EditComponent) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    };
    ShipmentHelperComponent.prototype.SendToCustom = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("ShipmentCustomsTransmission", 0).subscribe(function (response) {
            var check = _this.CheckSettingsWindowVisibility();
            if (check) {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Title = "Customs Transmissions";
                logWindow.Height = 180;
                logWindow.WindowArgs = _this.EntityPM;
                logWindow.Show('./ShipmentModules/ShipmentOthers/Components/SentToCustomComponent/SentToCustomLinkComponent');
            }
            else {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Title = "Customs Transmissions";
                logWindow.Height = 600;
                logWindow.WindowArgs = _this.EntityPM;
                logWindow.Show('./ShipmentModules/ShipmentOthers/Components/SentToCustomComponent/SentToCustomComponent');
            }
        });
    };
    ShipmentHelperComponent.prototype.CheckSettingsWindowVisibility = function () {
        var check = false;
        if ((ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == null || ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == "NO")
            &&
                (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == null || ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "NO")
            &&
                (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == null || ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == "NO")) {
            check = true;
        }
        return check;
    };
    ShipmentHelperComponent.prototype.CheckABMVisibility = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SendToCustoms")) {
            if (this.EntityPM.ShipmentLevelCode != "C") {
                this.IsABMVisible = true;
            }
            else {
                this.IsABMVisible = false;
            }
        }
    };
    ShipmentHelperComponent.prototype.CheckArtemusVisibility_BOL = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SendToArtemus")) {
            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H")) {
                this.IsATMSVisible_BOL = true;
            }
            else {
                this.IsATMSVisible_BOL = false;
            }
        }
    };
    ShipmentHelperComponent.prototype.CheckArtemusVisibility_VOG = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SendToArtemus")) {
            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C")) {
                this.IsATMSVisible_VOG = true;
            }
            else {
                this.IsATMSVisible_VOG = false;
            }
        }
    };
    ShipmentHelperComponent.prototype.CheckAESVisibility = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SENDTOAES")) {
            this.IsAESVisible = true;
        }
    };
    Object.defineProperty(ShipmentHelperComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Shipment", "Notes update");
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentHelperComponent.prototype.ShareDocumentsClicked = function () {
        if (this.EntityPM.IsDirty) {
            this.isSharingDocumentRequested = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.StartSharingDocument();
        }
    };
    ShipmentHelperComponent.prototype.StartSharingDocument = function () {
        if (this.EntityPM.IsManifestSentToAgent) {
            var windowArgs = {};
            windowArgs.EntityPM = this.EntityPM;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 1000;
            logWindow.Height = 600;
            logWindow.Title = "Share Documents";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SharedDocument/SharedDocumentComponent");
        }
        else {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Title = "Share Documents";
            messageWindow.Show("In order to share documents please share the manifest first");
        }
    };
    ShipmentHelperComponent.prototype.ShareManifestClicked = function () {
        if (this.EntityPM.IsDirty) {
            this.isShareManifestRequested = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.StartShareManifest();
        }
    };
    ShipmentHelperComponent.prototype.StartShareManifest = function (isUpdateAgent) {
        if (isUpdateAgent === void 0) { isUpdateAgent = false; }
        var windowArgs = {};
        windowArgs.EntityPM = this.EntityPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        if (!isUpdateAgent) {
            logWindow.Width = 600;
            logWindow.Height = 350;
        }
        logWindow.Title = !isUpdateAgent ? "Sharing Manifest" : "Share Updated Agent";
        windowArgs.IsShareUpdatedAgent = isUpdateAgent;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestStarted");
    };
    ShipmentHelperComponent.prototype.UpdateSharedAgentClicked = function () {
        if (this.EntityPM.IsDirty) {
            this.isUpdateSharedAgentRequested = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.StartShareManifest(true);
        }
    };
    ShipmentHelperComponent.prototype.ArtemusClicked = function () {
        var myService = new ShipmentDomainService_1.ShipmentDomainService();
        myService.GetArtemusStatus(this.EntityPM.ShipmentNumber).subscribe(function (myResult) {
            if (myResult != null) {
                if (!myResult.HasError) {
                }
            }
        });
    };
    ShipmentHelperComponent.prototype.ShippingInstructionsClicked = function () {
        if (!this.isShippingInstructionsClicked) {
            this.isShippingInstructionsClicked = true;
            if (this.entityArgs.EditComponent) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    };
    ShipmentHelperComponent.prototype.ShowINTTRAWizard = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Shipping Instructions Wizard";
        logWindow.WindowArgs = { Shipment: this.EntityPM, EntityArgs: this.entityArgs };
        logWindow.Show('./ShipmentModules/ShipmentINTTRA/Components/Wizard/WizardComponent');
    };
    ShipmentHelperComponent.prototype.AnalyzeChampXMLClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Simulate Champ Message";
        logWindow.Show('./Shipment/Components/Helpers/AnalyzeChampXMLComponent');
    };
    ShipmentHelperComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ShipmentHelperComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], ShipmentHelperComponent);
    return ShipmentHelperComponent;
}());
exports.ShipmentHelperComponent = ShipmentHelperComponent;
var NotesClass = /** @class */ (function () {
    function NotesClass(header, notes) {
        this.Header = header;
        this.Notes = notes;
    }
    return NotesClass;
}());
exports.NotesClass = NotesClass;
//# sourceMappingURL=ShipmentHelperComponent.js.map