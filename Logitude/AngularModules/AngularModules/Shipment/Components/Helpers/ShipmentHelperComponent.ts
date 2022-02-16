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
import { ShipmentContainersWebService } from '../../../Shipment/Services/ShipmentContainersWebService';
import { FeatureToggleList } from '../../../Infrastructure/EntityLists/FeatureToggleList'; 
import { DocumentTypePMExtendedService } from 'Common/Services/ExtendedPMs/DocumentTypePMExtendedService';

@Component({
    
    templateUrl: './ShipmentHelperComponent.html',
})

export class ShipmentHelperComponent implements OnDestroy {
    public EntityPM: ShipmentPM;
    public EntityTitle: string;
    public NotesList: NotesClass[] = [];
    public IsFollowupsVisible: boolean = false;
    public IsAnalyzeChampXMLButtonVisible: boolean = false;
    _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    public IsSimulatorVisible: boolean = false; 
    ShareDocumentsViaEmailDocumentTypeCode = "SDVE"; 
    public documentTypePMExtendedService: DocumentTypePMExtendedService = new DocumentTypePMExtendedService();

    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {        
        this.IsFollowupsVisible = FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Followups");
        this.IsSimulatorVisible = FeatureLocator.HasFeaturePermession("Shipment", "ContainerStatusSimulator");

        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM) {
            this.ShowHideShippingInstructionsButton();
            this.ShowHideShipmentContainersSimulatorButton();
            this.SetIsShipmentContainersVisible();
            this.ShowHideSendBookingButton();
            if (this.EntityPM.DirectionId == "E" && this.EntityPM.TransportModeId == "A") {
                if (FeatureLocator.IsPackage_DVMT()) {
                    this.IsAnalyzeChampXMLButtonVisible = true;
                }
            }
            this.Listen();
            this.BuildComponent();
        }
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

                        else if (this.isSendBookingClicked) {
                            this.ShowINTTRABookingWizard();
                        }

                        else if (this.isShareManifestRequested) {
                            this.StartShareManifest();
                        }

                        else if (this.isUpdateSharedAgentRequested) {
                            this.StartShareManifest(true);
                        }

                        else if (this.isSharingDocumentRequested) {
                            this.StartSharingDocument();
                        }
                        else if (this.ShareDocumentsViaEmailInSendControl) {
                            this.ShowSharedDocument();
                        }
                         
                    }

                    this.ShareDocumentsViaEmailInSendControl = false;
                    this.isShareManifestRequested = false;
                    this.isUpdateSharedAgentRequested = false;
                    this.isSharingDocumentRequested = false;
                    this.isAWBImportButtonClicked = false;
                    this.isAWBButtonClicked = false;
                    this.isSendToCustomClicked = false;
                    this.isShippingInstructionsClicked = false;
                    this.isSendBookingClicked = false;
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                        this.ShowHideShippingInstructionsButton();
                        this.BuildComponent();

                        if (this.EntityPM.DirectionId == "E" && this.EntityPM.TransportModeId == "A") {
                            if (FeatureLocator.IsPackage_DVMT()) {
                                this.IsAnalyzeChampXMLButtonVisible = true;
                            }
                        }
                    }
                });
            }
        }
    }

    private ShowHideShippingInstructionsButton() {
        this.IsShippingInstructionsVisible = false;

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
    }

    private ShowHideSendBookingButton() {
        this.IsSendBookingVisible = false;
        if (FeatureLocator.HasFeaturePermession("Shipment", "INTTRABookingSimulator")) {
            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "E") {
                if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C") {
                    var isFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
                    if (isFCLEntity) {
                        this.IsSendBookingVisible = true;
                    }
                }
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
            this.CurrentSession.FireEvent("AWBWizardClosed");
        });
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public IsUpdateSharedAgentButtonVisible: boolean = false;
    public IsShareDocumentsButtonVisible: boolean = false;
    public IsShareManifestButtonVisible: boolean = false;
    public IsShareDocumentsViaEmailVisible: boolean = false; 
    private ShareDocumentsViaEmailInSendControl: boolean = false;

    BuildComponent() {
        this.EntityTitle = this.EntityPM.ShipmentLevelCode == "C" ? "Master" : "Shipment";
        this.SetAWBWizardButton();
        this.setImportAWBWizardButton();

        var isUpdateSharedAgentButtonVisible: boolean = false;
        var isShareDocumentsButtonVisible: boolean = false;
        var isShareManifestButtonVisible: boolean = false; 
      
        this.SetShareDocumentsViaEmailVisibility(); 

        if (FeatureLocator.HasFeaturePermession("Shipment", "AgentSharedManifest")) {
            if (this.EntityPM.DirectionId == "E" && (this.EntityPM.ShipmentLevelCode == "C" || this.EntityPM.ShipmentLevelCode == "D")) {
                isShareManifestButtonVisible = true;
            }
        }

        if (FeatureLocator.HasFeaturePermession("AgentSharedManifest", "UPDATESHAREDAGENT")) {
            if (this.EntityPM.DirectionId == "E" && (this.EntityPM.ShipmentLevelCode == "C" || this.EntityPM.ShipmentLevelCode == "D")) {
                isUpdateSharedAgentButtonVisible = true;
            }
        }

        if (FeatureLocator.HasFeaturePermession("AgentSharedDocument", "NEW")) {
            if (this.EntityPM.DirectionId == "E" && (this.EntityPM.ShipmentLevelCode == "C" || this.EntityPM.ShipmentLevelCode == "D")) {
                isShareDocumentsButtonVisible = true;
            }
        }
         
        this.IsUpdateSharedAgentButtonVisible = isUpdateSharedAgentButtonVisible;
        this.IsShareDocumentsButtonVisible = isShareDocumentsButtonVisible;
        this.IsShareManifestButtonVisible = isShareManifestButtonVisible;

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
    private isSendBookingClicked: boolean = false;
    public IsAWBWizardButtonVisible: boolean = false
    public IsImportAWBWizardButtonVisible: boolean = false;
    public IsSendToCustomVisible: boolean = false
    public IsShippingInstructionsVisible: boolean = false;
    public IsSendBookingVisible: boolean = false;

    public IsShipmentContainersSimulatorVisible: boolean = false;

    private SetShareDocumentsViaEmailVisibility() {
        if (FeatureLocator.HasFeaturePermession("Shipment", "ShareDocumentsViaEmail") && this.IsShareShipment) { 
                this.IsShareDocumentsViaEmailVisible = true; 
        }
    }

    private IsShareShipment() {
        return ((this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "R" ) && (this.EntityPM.ShipmentLevelCode == "C" || this.EntityPM.ShipmentLevelCode == "H"));
    }

    private ShowHideShipmentContainersSimulatorButton() {
        this.IsShipmentContainersSimulatorVisible = false;
        if (FeatureLocator.HasFeaturePermession("Shipment", "INTTRASimulator")) {
            var isFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            if (this.EntityPM.TransportModeId == "O" && isFCLEntity) {
                this.IsShipmentContainersSimulatorVisible = true;
            }
        }
    }

    public IsShipmentContainersVisible: boolean = false;
    private SetIsShipmentContainersVisible() {
        this.IsShipmentContainersVisible = false;
        var featureToggle: FeatureToggleList = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "OIC")[0];
        var isFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        if (featureToggle && isFCLEntity && this.EntityPM.TransportModeId == "O") {
            this.IsShipmentContainersVisible = true;
        }
    }

    setImportAWBWizardButton() {
        this.IsImportAWBWizardButtonVisible = false;
        if (FeatureLocator.HasFeaturePermession("Shipment", "IMPORTAWBWIZARD")) {
            if (this.EntityPM.DirectionId == "I" && this.EntityPM.TransportModeId == "A") {
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
            this.ComputeFreightChargesFromFreightPayableLine();
           
            if (this.entityArgs.EditComponent) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    }

    ComputeFreightChargesFromFreightPayableLine() {
        if (!this.EntityPM.IsMultipleCommodities && AppTool.IsNullOrZero(this.EntityPM.AWBChargeRate)) {
            var airFreightCode = "AFT";
            var airFreightCharge = this.EntityPM.ShipmentPayables.filter(a => a.ChargesTypeCode == airFreightCode)[0];
            if (airFreightCharge != null && this.ValidateCurrencyOfShipmentAWBPrintOnlies(airFreightCharge)) {
                this.SetAWBFreightChargeFields(airFreightCharge); 
            }
        }
    }

    private SetAWBFreightChargeFields(airFreightCharge) {
        this.EntityPM.AWBChargeRate = airFreightCharge.UnitPrice;
        this.EntityPM.AWBCurrencyId = airFreightCharge.CurrencyId;
        this.EntityPM.AWBChargeAmount = ShipmentTool.ComputeAWBChargeAmount(this.EntityPM);
        if (!AppTool.IsNullOrEmpty(airFreightCharge.PrepaidCollectId)) {
            this.EntityPM.FreightPrepaidCollectId = airFreightCharge.PrepaidCollectId;
            ShipmentTool.BuildAWBChargesCodeCode(this.EntityPM);
            ShipmentTool.ComputeAWBFrieghtAmountCollectAndPrepaid(this.EntityPM);
        }
    }

    private ValidateCurrencyOfShipmentAWBPrintOnlies(airFreightCharge) {
        var isValid = true;
        if(this.EntityPM.ShipmentAWBPrintOnlies != null) {
            this.EntityPM.ShipmentAWBPrintOnlies.forEach(item => {
                if (item.CurrencyId != airFreightCharge.CurrencyId) {
                    isValid = false;
                }
            });
        }
        return isValid;
    }

    AWBImportButtonClicked() {
        if (!this.isAWBImportButtonClicked) {
            this.isAWBImportButtonClicked = true;
            var isFullWizard: boolean = this.IsFullWizard();
            if (isFullWizard) {
                this.ComputeFreightChargesFromFreightPayableLine();
            }
            if (this.entityArgs.EditComponent) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    }

    ImportWizard() {
        var isFullWizard: boolean = this.IsFullWizard();
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
                this.CurrentSession.FireEvent("AWBWizardClosed");
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
                this.CurrentSession.FireEvent("AWBWizardClosed");
            });
        }
    }

    IsFullWizard(): boolean {
        var isFullWizard: boolean = ShipmentTool.IsFullAWBWizard(this.EntityPM.DirectionId);
        return isFullWizard;
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
        this._entityResourceService.getEntityResourceByTableName("ShipmentCustomsTransmission", 0).subscribe((response:any) => {
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


    ShareDocumentsViaEmailClicked() {
        this.ShareDocumentsViaEmailInSendControl = true;
        if (this.EntityPM.IsDirty) { 
           this.CurrentSession.CurrentEditComponent.SaveChanges();
        } 
        else
        {
            this.CheckShareDocumentsViaEmailDocumentTypeCodeExisting();
        }  
    }

    private CheckShareDocumentsViaEmailDocumentTypeCodeExisting() {
        this.documentTypePMExtendedService.GetDoesDocumentTypeCodeExist(this.ShareDocumentsViaEmailDocumentTypeCode, SessionLocator.Tenant).subscribe((res: any) => {
            var serviceResponse: ServiceResponse = res;
            if (!serviceResponse.HasError && serviceResponse.Result == false) {
                this.ShowValidationMessage("Contact your administrator");
            }
            if (!serviceResponse.HasError && serviceResponse.Result == true) {
                this.ShowSharedDocument()
            }
            this.ShareDocumentsViaEmailInSendControl = false;
        });
    }

    private ShowValidationMessage(messsage: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(messsage);
    }

    //ShareDocument
    isSharingDocumentRequested: boolean = false;
    ShareDocumentsClicked() {

        if (this.EntityPM.IsDirty) {
            this.isSharingDocumentRequested = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else { 
            this.StartSharingDocument();
        }

    }
    StartSharingDocument() {

        if (this.EntityPM.IsManifestSentToAgent) {

            this.ShowSharedDocument();

        }
        else {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Title = "Share Documents";
            messageWindow.Show("In order to share documents please share the manifest first");
        }

    }

      //ShareManifest
    isShareManifestRequested: boolean = false;

    private ShowSharedDocument() {
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.ShareDocumentsViaEmail = this.ShareDocumentsViaEmailInSendControl;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.Title = "Share Documents";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SharedDocument/SharedDocumentComponent");
    }

    ShareManifestClicked() {
        if (this.EntityPM.IsDirty) {
            this.isShareManifestRequested = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
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
            this.CurrentSession.CurrentEditComponent.SaveChanges();
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
        logWindow.Title = "Shipping Instructions Wizard";
        logWindow.WindowArgs = { Shipment: this.EntityPM, EntityArgs: this.entityArgs };
        logWindow.Show('./ShipmentModules/ShipmentINTTRA/Components/Wizard/WizardComponent');
    }

    SendBookingClicked() {
        if (!this.isSendBookingClicked) {
            this.isSendBookingClicked = true;

            if (this.entityArgs.EditComponent) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }

    }

    ShowINTTRABookingWizard() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "INTTRA e-booking Wizard";
        logWindow.WindowArgs = { Shipment: this.EntityPM };
        logWindow.Width = 1020;
        logWindow.Height = 570;
        logWindow.Show('./ShipmentModules/ShipmentINTTRA/Components/Wizard/SimulatorBookingComponent');
    }

    AnalyzeChampXMLClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Simulate Champ Message";
        logWindow.Show('./Shipment/Components/Helpers/AnalyzeChampXMLComponent');
    }

    ShipmentContainersSimulatorClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { ShipmentId: this.EntityPM.Id, IsFromContainer: false, ContainerNumber: null};
        logWindow.Title = "Shipment Containers Statuses Simulator";
        logWindow.Show('./ShipmentModules/ShipmentOthers/Components/ShipmentContainersStatuses/ContainersStatusesSimulatorComponent');
    }

    ContainersRequestStatusClicked() {
        this.CurrentSession.StartBusyIndicator("Sending");
        var service = new ShipmentContainersWebService();
        service.GetContainerStatusResult(this.EntityPM.Id, null, false).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
               
            }
            else {
                this.entityArgs.EditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });            
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
