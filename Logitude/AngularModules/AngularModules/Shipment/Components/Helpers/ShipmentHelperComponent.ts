import {Component, OnDestroy, ChangeDetectorRef} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {ShipmentTool} from '../../Tools';
import {AWBWizardArgs, FSRWizardArgs} from '../../Args';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../Infrastructure/Tools';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ShipmentDomainService} from '../../../Shipment/Services/ShipmentDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './ShipmentHelperComponent.html',
})

export class ShipmentHelperComponent implements OnDestroy {
    public EntityPM: ShipmentPM;
    public EntityTitle: string;
    public NotesList: NotesClass[] = [];
    public IsFollowupsVisible: boolean = false;
    public IsAnalyzeChampXMLButtonVisible: boolean = false;
    _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {

        //this.cd.detach();
        this.IsFollowupsVisible = FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Followups");

        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM) {

            if (FeatureLocator.HasFeaturePermession("Shipment", "ShippingInstructions")) {
                if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "E") {
                    if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C") {
                        var isFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                        if (isFCLEntity) {
                            this.IsShippingInstructionsVisible = true;
                        }
                    }
                }
            }

            if (this.EntityPM.DirectionId == "E" && this.EntityPM.TransportModeId == "A") {
                if (FeatureLocator.IsPackage_DVMT()) {
                    this.IsAnalyzeChampXMLButtonVisible = true;
                }
            }

            this.Listen();
            this.BuildComponent();
        }

        //this.cd.detectChanges();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                        if (this.isAWBButtonClicked) {
                            this.ImportWizard();
                        }
                        else if (this.isAWBImportButtonClicked) {
                            this.ImportAWBWizard();
                        }

                        else if (this.isSendToCustomClicked) {
                            this.SendToCustom();
                        }

                        else if (this.isShippingInstructionsClicked) {
                            this.ShowINTTRAWizard();
                        }
                        else if (this.isShareManifestRequested) {
                            this.StartShareManifest();
                        }
                        else if (this.isUpdateSharedAgentRequested) {
                            this.StartShareManifest(true);
                        } else if (this.isSharingDocumentRequested) {
                            this.StartSharingDocument();
                        }
                        

                    }
                    
                    this.isShareManifestRequested = false;
                    this.isUpdateSharedAgentRequested = false;
                    this.isSharingDocumentRequested = false;
                    this.isAWBImportButtonClicked = false;
                    this.isAWBButtonClicked = false;
                    this.isSendToCustomClicked = false;
                    this.isShippingInstructionsClicked = false;
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;                        
                    }
                });
            }
        }
    }
    ImportAWBWizard() {
        var myAWBWizardArgs: AWBWizardArgs = new AWBWizardArgs();
        myAWBWizardArgs.EntityPM = this.EntityPM;
        myAWBWizardArgs.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = ShipmentTool.GetAWBWizardHeader(this.EntityPM.ShipmentLevelCode, "E");
        logWindow.WindowArgs = myAWBWizardArgs;
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent');
        logWindow.WindowClosed.subscribe(s => {
            SessionLocator.CurrentSession.FireEvent("AWBWizardClosed");
        });
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
    BuildComponent() {
        this.EntityTitle = this.EntityPM.ShipmentLevelCode == "C" ? "Master" : "Shipment";
        this.SetAWBWizardButton();
        this.setImportAWBWizardButton();
        if (FeatureLocator.HasFeaturePermession("Shipment", "AgentSharedManifest")) {
            if (this.EntityPM.DirectionId == "E" && (this.EntityPM.ShipmentLevelCode == "C" || this.EntityPM.ShipmentLevelCode == "D")) {
                this.IsShareManifestButtonVisible = true;
            }
        }


        if (FeatureLocator.HasFeaturePermession("AgentSharedManifest", "UPDATESHAREDAGENT")) {
            if (this.EntityPM.DirectionId == "E" && (this.EntityPM.ShipmentLevelCode == "C" || this.EntityPM.ShipmentLevelCode == "D")) {
                this.IsUpdateSharedAgentButtonVisible = true;

            }
        }


        if (FeatureLocator.HasFeaturePermession("AgentSharedDocument", "NEW")) {
            if (this.EntityPM.DirectionId == "E" && (this.EntityPM.ShipmentLevelCode == "C" || this.EntityPM.ShipmentLevelCode == "D")) {
                this.IsShareDocumentsButtonVisible = true;
            }
        }



        if (FeatureLocator.HasFeaturePermession("Shipment", "ShipmentCustomsTransmission")) {
            this.CheckArtemusVisibility_BOL();
            this.CheckArtemusVisibility_VOG();
            this.CheckABMVisibility();
            this.CheckAESVisibility();

            if (this.IsABMVisible || this.IsAESVisible || this.IsATMSVisible_BOL || this.IsATMSVisible_VOG) {
                this.IsSendToCustomVisible = true;
            }
        }
    }

    public AWBWizardButtonLabel: string = null;
    public AWBImportWizardButtonLabel: string = null;

    private isAWBButtonClicked: boolean = false;
    private isAWBImportButtonClicked: boolean = false;

    private isSendToCustomClicked: boolean = false;  
    private isShippingInstructionsClicked: boolean = false;
    public IsAWBWizardButtonVisible: boolean = false
    public IsImportAWBWizardButtonVisible: boolean = false;
    public IsSendToCustomVisible: boolean = false
    public IsShippingInstructionsVisible: boolean = false;
    setImportAWBWizardButton() {
        this.IsImportAWBWizardButtonVisible = false;
        if (FeatureLocator.HasFeaturePermession("Shipment", "IMPORTAWBWIZARD")) {
            if (this.EntityPM.DirectionId == "I") {
                this.IsImportAWBWizardButtonVisible = true;
            }
        }

    }
    SetAWBWizardButton() {
        this.AWBWizardButtonLabel = ShipmentTool.GetAWBWizardHeader(this.EntityPM.ShipmentLevelCode, this.EntityPM.DirectionId);
        this.AWBImportWizardButtonLabel = ShipmentTool.GetAWBWizardHeader(this.EntityPM.ShipmentLevelCode, "E");

        var isButtonVisible: boolean = false;

     
        

        if (this.EntityPM.TransportModeId == "A" && !SessionLocator.TenantPM.IsHybrid) {
            var isFullWizard = false;

            if (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "R") {
                isFullWizard = true;
            }




            else if (this.EntityPM.DirectionId == "D") {
                if (!FeatureLocator.IsPackage_EAWB()) {
                    isFullWizard = true;
                }
            }

            if (isFullWizard) {
                isButtonVisible = true;
            }

            else {
                if (FeatureLocator.HasFeaturePermession("Shipment", "SENDREQUEST")) {
                    isButtonVisible = true;
                }
            }
        }

        this.IsAWBWizardButtonVisible = isButtonVisible;
    }
    AWBButtonClicked() {
        if (!this.isAWBButtonClicked) {
            this.isAWBButtonClicked = true;

            if (this.entityArgs.EditComponent) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    }

    AWBImportButtonClicked() {
        if (!this.isAWBImportButtonClicked) {
            this.isAWBImportButtonClicked = true;

            if (this.entityArgs.EditComponent) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    }
    ImportWizard() {

        var isFullWizard: boolean = ShipmentTool.IsFullAWBWizard(this.EntityPM.DirectionId);

        if (isFullWizard) {
            var myAWBWizardArgs: AWBWizardArgs = new AWBWizardArgs();
            myAWBWizardArgs.EntityPM = this.EntityPM;
            myAWBWizardArgs.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = ShipmentTool.GetAWBWizardHeader(this.EntityPM.ShipmentLevelCode, this.EntityPM.DirectionId);
            logWindow.WindowArgs = myAWBWizardArgs;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent');
            logWindow.WindowClosed.subscribe(s => {
                SessionLocator.CurrentSession.FireEvent("AWBWizardClosed");
            });
        }

        else {
            var myFSRWizardArgs: FSRWizardArgs = new FSRWizardArgs();
            myFSRWizardArgs.EntityPM = this.EntityPM;
            myFSRWizardArgs.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = ShipmentTool.GetAWBWizardHeader(this.EntityPM.ShipmentLevelCode, this.EntityPM.DirectionId);
            logWindow.WindowArgs = myFSRWizardArgs;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/FSRWizard/FSRWizardComponent');
            logWindow.WindowClosed.subscribe(s => {
                SessionLocator.CurrentSession.FireEvent("AWBWizardClosed");
            });
        }
    }

    // Send To Custom 
    SendToCustomsClicked() {
        if (!this.isSendToCustomClicked) {
            this.isSendToCustomClicked = true;

            if (this.entityArgs.EditComponent) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    }
    SendToCustom() {
        this._entityResourceService.getEntityResourceByTableName("ShipmentCustomsTransmission", 0).subscribe(response => {
            var check = this.CheckSettingsWindowVisibility();

            if (check) {
                var logWindow = new LogitudeWindow();
                logWindow.Title = "Customs Transmissions";
                logWindow.Height = 180;
                logWindow.WindowArgs = this.EntityPM;
                logWindow.Show('./ShipmentModules/ShipmentOthers/Components/SentToCustomComponent/SentToCustomLinkComponent');
            } else {
                var logWindow = new LogitudeWindow();
                logWindow.Title = "Customs Transmissions";
                logWindow.Height = 600;
                logWindow.WindowArgs = this.EntityPM;
                logWindow.Show('./ShipmentModules/ShipmentOthers/Components/SentToCustomComponent/SentToCustomComponent');
            }
        });
    }
    CheckSettingsWindowVisibility(): boolean {
        var check = false;
        if ((ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == null || ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == "NO")
            &&
            (ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == null || ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "NO")
            &&
            (ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == null || ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == "NO")
        ) {
            check = true;
        }
        return check;
    }
    public IsABMVisible = false;
    public IsAESVisible = false;
    public IsATMSVisible_BOL = false;
    public IsATMSVisible_VOG = false;
    CheckABMVisibility() {
        if (FeatureLocator.HasFeaturePermession("Shipment", "SendToCustoms")) {
            if (this.EntityPM.ShipmentLevelCode != "C") {
                this.IsABMVisible = true;
            }
            else {
                this.IsABMVisible = false;
            }
        }
    }
    CheckArtemusVisibility_BOL() {
        if (FeatureLocator.HasFeaturePermession("Shipment", "SendToArtemus")) {
            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H")) {
                    this.IsATMSVisible_BOL = true;
            }
            else {
                this.IsATMSVisible_BOL = false;
            }
        }
    }
    CheckArtemusVisibility_VOG() {
        if (FeatureLocator.HasFeaturePermession("Shipment", "SendToArtemus")) {
            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C")) {
                    this.IsATMSVisible_VOG = true;
            }
            else {
                this.IsATMSVisible_VOG = false;
            }
        }
    }
    CheckAESVisibility() {
        if (FeatureLocator.HasFeaturePermession("Shipment", "SENDTOAES")) {
            this.IsAESVisible = true;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
            ServiceLocator.SendTotangoUserActivity("Shipment", "Notes update");
        }
    }

    public IsUpdateSharedAgentButtonVisible: boolean = false;
    public IsShareDocumentsButtonVisible: boolean = false;
    public IsShareManifestButtonVisible: boolean = false;


  

    //ShareDocument
    isSharingDocumentRequested: boolean = false;
    ShareDocumentsClicked() {

        if (this.EntityPM.IsDirty) {
            this.isSharingDocumentRequested = true;
            SessionLocator.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.StartSharingDocument();
        }

    }
    StartSharingDocument() {

        if (this.EntityPM.IsManifestSentToAgent) {

            var windowArgs: any = {};
            windowArgs.EntityPM = this.EntityPM;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 1000;
            logWindow.Height = 600;
            logWindow.Title = "Share Documents";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SharedDocument/SharedDocumentComponent");

        }
        else {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Title = "Share Documents";
            messageWindow.Show("In order to share documents please share the manifest first");
        }

    }

      //ShareManifest
    isShareManifestRequested: boolean = false;
    ShareManifestClicked() {
        if (this.EntityPM.IsDirty) {
            this.isShareManifestRequested = true;
            SessionLocator.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.StartShareManifest();
        }

    }
    public StartShareManifest(isUpdateAgent: boolean = false) {

        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        var logWindow = new LogitudeWindow();

        if (!isUpdateAgent) {
            logWindow.Width = 600;
            logWindow.Height = 350;
        }
        logWindow.Title = !isUpdateAgent ? "Sharing Manifest" : "Share Updated Agent";
        windowArgs.IsShareUpdatedAgent = isUpdateAgent;

        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestStarted");

    }

    
       //UpdateAgentShareManifest
    isUpdateSharedAgentRequested: boolean = false;
    UpdateSharedAgentClicked() {
        if (this.EntityPM.IsDirty) {
            this.isUpdateSharedAgentRequested = true;
            SessionLocator.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.StartShareManifest(true);
        }

    }


    ArtemusClicked() {
        var myService: ShipmentDomainService = new ShipmentDomainService();
        myService.GetArtemusStatus(this.EntityPM.ShipmentNumber).subscribe((myResult: ServiceResponse) => {
            if (myResult != null) {
                if (!myResult.HasError) {
                   
                }
            }
        });
    }
    ShippingInstructionsClicked() {
        if (!this.isShippingInstructionsClicked) {
            this.isShippingInstructionsClicked = true;

            if (this.entityArgs.EditComponent) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    }

    ShowINTTRAWizard() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "INTTRA Wizard";
        logWindow.WindowArgs = { Shipment: this.EntityPM, EntityArgs: this.entityArgs };
        logWindow.Show('./ShipmentModules/ShipmentINTTRA/Components/Wizard/WizardComponent');
    }

    AnalyzeChampXMLClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Simulate Champ Message";
        logWindow.Show('./Shipment/Components/Helpers/AnalyzeChampXMLComponent');
    }
}

export class NotesClass {
    public Header: string;
    public Notes: string;
    constructor(header: string, notes: string) {
        this.Header = header;
        this.Notes = notes;
    }
}
