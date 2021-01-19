import {Component} from '@angular/core';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {AWBWizardComponent} from './AWBWizardComponent';
import {ShipmentTool} from '../../../../Shipment/Tools';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CCSWebService, CCSResult, AWBResultClass, FHLShipmentValidator} from '../../../../Infrastructure/Services/WebServices/CCSWebService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    templateUrl: './SendWindowComponent.html',
})

export class SendWindowComponent {
    private Tenant: number;
    private entityPM: ShipmentPM;
    private Wizard: AWBWizardComponent;
    private isGLSHK: boolean;
    private isSendingFHLs: boolean;
    private isSendingDEXX: boolean;
    private isSendingCargonaut: boolean;
    private tenantZeroAirlineField: string;
    public ValidationErrorsList: string[];
    public ValidationWarningsList: string[];
    private myCCSWebService: CCSWebService;
    public IsRecipientsVisible: boolean = false;
    public PurchaseStockUri: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.ValidationErrorsList = [];
        this.ValidationWarningsList = [];
        this.PurchaseStockUri = "https://ws.bluesnap.com/buynow/checkout?sku3233898=1&language=ENGLISH&currency=USD&custom1=" + SessionLocator.Tenant + "&quantity=1";
     }

    public SetWindowArgs(args: any) {
        this.Tenant = args.EnttiyPM.Tenant;
        this.entityPM = args.EnttiyPM;
        this.Wizard = args.Wizard;
        this.isSendingFHLs = args.IsSendingFHLs;
        this.isSendingDEXX = args.IsSendingDEXX;
        this.isSendingCargonaut = args.IsSendingCargonaut;
        this.InitializeComponent();        
    }

    private InitializeComponent() {
        this.isGLSHK = ShipmentTool.IsGLSHK();
        this.tenantZeroAirlineField = ShipmentTool.GetTenantZeroAirlineField(this.entityPM);
        this.OtherRecipient = this.tenantZeroAirlineField;
        this.SetRecipientsVisibility();
        this.SetDefaultRecipient();
        this.SetWarnings();
        this.SetMessageType();
        this.SetSendButton();

        if (this.isSendingFHLs) {
            this.IsFHLsStatusVisible = true;
            this.LoadFHLValidators();
        }

        else {
            this.SetMessageStatus();
        }
    }
    private InitializeWebService() {
        if (this.myCCSWebService == null) {
            this.myCCSWebService = new CCSWebService();
        }
    }

    // Warnings
    private SetWarnings() {
        var warnings: string[] = [];

        if (this.isSendingFHLs || this.entityPM.ShipmentLevelCode == "H") {
            if (this.entityPM.FWBStatusCode == "NSEN") {
                warnings.push("The FWB is not sent");
            }
        }

        else if (this.entityPM.ShipmentLevelCode == "C") {
            if (this.entityPM.FHLStatusCode == "PSEN") {
                warnings.push("Not all the FHL(s) are sent");
            }
        }

        this.ValidationWarningsList = warnings;
    }

    // Message Type
    public MessageType: string;
    private SetMessageType() {
        var myResult: string = "";

        if (this.isSendingCargonaut) {
            myResult = "Cargonaut ";
        }

        else if (this.isSendingDEXX) {
            myResult = "DEXX ";
        }

        if (this.entityPM.ShipmentLevelCode == "H") {
            myResult += "FHL";
        }

        else {
            myResult += "FWB";
        }

        this.MessageType = myResult;
    }

    // Message Status
    public IsMessageValid: boolean = false;
    public IsMessageWarning: boolean = false;
    public IsMessageError: boolean = false;
    public MessageWarningText: string = null;
    public MessageErrorText: string = null;
    private SetMessageStatus() {
        if (this.isSendingFHLs) {
            this.SetFWBStatus();
            this.SetFHLsStatus();
        }

        else if (this.entityPM.ShipmentLevelCode == "H") {
            this.SetFHLStatus();
        }

        else {
            this.SetFWBStatus();
        }
    }
    private SetFWBStatus() {
        if (this.entityPM.ShipmentLevelCode == "C") {
            var isConsolidationValid: boolean = this.IsConsolidationValid();
            if (!isConsolidationValid) {
                this.MessageWarningText = "This FWB is not connected to any FHL";
                this.IsMessageWarning = true;
            }

            else {                
                this.IsMessageValid = true;
            }
        }

        else {
            this.IsMessageValid = true;
        }
    }
    private SetFHLStatus() {
        if (AppTool.IsNullOrEmpty(this.entityPM.MasterShipmentDataId)) {
            this.MessageErrorText = "This FHL is not connected to FWB";
            this.IsMessageError = true;
        }

        else {
            this.IsMessageValid = true;
        }
    }
    private IsConsolidationValid() {
        var isValid = false;

        if (this.entityPM.ShipmentConsoleShipments.length > 0) {
            isValid = true;
        }

        return isValid;
    }

    // FHLs Status
    public IsFHLsValid: boolean = false;
    public IsFHLsWarning: boolean = false;
    public IsFHLsError: boolean = false;
    public FHLsWarningText: string = null;
    public FHLsErrorText: string = null;
    public IsFHLsStatusVisible: boolean = false;
    public FHLsValidationList: FHLStatusItemViewModel[] = [];
    get FHLsStatusLabel() {
        var myResult: string = "";

        if (this.isSendingCargonaut) {
            myResult = "Cargonaut FHL(s) Status";
        }

        else if (this.isSendingDEXX) {
            myResult = "DEXX FHL(s) Status";
        }

        else {
            myResult = "FHL(s) Status";
        }

        return myResult;
    }
    private SetFHLsStatus() {
        this.IsFHLsValid = false;
        this.IsFHLsWarning = false;
        this.IsFHLsError = false;

        if (this.FHLsValidationList.length == 0) {
            this.FHLsErrorText = "No connected FHLs found";
            this.IsFHLsError = true;
        }

        else if (this.FHLsValidationList.filter(d => d.IsFHLValid).length == 0) {
            this.FHLsErrorText = "All FHL(s) are invalid! none will be sent";
            this.IsFHLsError = true;
        }

        else if (this.FHLsValidationList.filter(d => d.IsFHLValid).length > 0 && this.FHLsValidationList.filter(d => !d.IsFHLValid).length > 0) {
            this.FHLsWarningText = "Warning: some of the FHL(s) has got errors and will not be sent, Only valid FHL(s) will be sent";
            this.IsFHLsWarning = true;
        }

        else {
            this.IsFHLsValid = true;
        }
    }
    public LoadFHLValidators() {
        if (this.isSendingFHLs) {

            this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));

            this.InitializeWebService();

            this.myCCSWebService.GetFHLsValidation(this.entityPM.Id).subscribe((myResponse: ServiceResponse) => {

                this.FHLsValidationList = [];

                if (myResponse != null) {
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        var myResult = myResponse.Result;

                        var list1: FHLShipmentValidator[] = [];
                        var list2: FHLShipmentValidator[] = [];

                        if (this.isSendingCargonaut || this.isSendingDEXX) {
                            list1 = myResult.filter(d => d.CargonautFHLStatusCode == "SENT");
                            list2 = myResult.filter(d => d.CargonautFHLStatusCode != "SENT");
                        }

                        else {
                            list1 = myResult.filter(d => d.FHLStatusCode == "SENT");
                            list2 = myResult.filter(d => d.FHLStatusCode != "SENT");
                        }

                        list1.forEach(item => {
                            this.FHLsValidationList.push(new FHLStatusItemViewModel(item, this, this.isSendingCargonaut));
                        });

                        list2.forEach(item => {
                            this.FHLsValidationList.push(new FHLStatusItemViewModel(item, this, this.isSendingCargonaut));
                        });
                    }
                }

                this.SetMessageStatus();
                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    // Recipients
    public CargRecipient: string = "QIFFMXS"; // Cargo Test System
    public XXArRecipient: string = "LUXX9CV"; // Luxembourg
    public Fra1Recipient: string = "REUDLHT"; // Frankfurt
    public Fra2Recipient: string = "ZCSSMIT"; // Frankfurt Simulator
    public RichRecipient: string = "QIFRHXS"; // Richard Email
    public IhabRecipient: string = "QIFFMAL"; // Ihab Email
    public CargonautRecipient: string = "REUCGNP"; 
    public DEXXRecipient: string = "REUBCSP";
    public OtherRecipient: string;

    public IsDEXXVisible: boolean = false;
    public IsCargonautVisible: boolean = false;
    private SetRecipientsVisibility() {
        if (FeatureLocator.IsPackage_DVMT()) {
            this.IsRecipientsVisible = true;

            switch (this.entityPM.MainCarriageFromPortCode) {
                case "SPL":
                case "AMS":
                case "RTM":
                case "MST":
                    {
                        this.IsCargonautVisible = true;
                        break;
                    }

                case "LGG":
                case "BRU":
                    {
                        this.IsDEXXVisible = true;
                        break;
                    }

                default:
                    {
                        this.IsDEXXVisible = false;
                        this.IsCargonautVisible = false;
                        break;
                    }
            }
        }
    }
    private SetDefaultRecipient() {
        if (this.isSendingCargonaut && this.IsCargonautVisible) {
            this.CargonautRecipientIsChecked = true;
        }

        else if (this.isSendingDEXX && this.IsDEXXVisible) {
            this.DEXXRecipientIsChecked = true;
        }

        else if (!AppTool.IsNullOrEmpty(this.tenantZeroAirlineField)) {
            this.OtheRecipientIsChecked = true;
        }

        else {
            this.CargRecipientIsChecked = true;
        }
    }
    private FalseAllRadioButtons() {
        this.cargRecipientIsChecked = false;
        this.xXArRecipientIsChecked = false;
        this.fra1RecipientIsChecked = false;
        this.fra2RecipientIsChecked = false;
        this.richRecipientIsChecked = false;
        this.ihabRecipientIsChecked = false;
        this.cargonautRecipientIsChecked = false;
        this.dEXXRecipientIsChecked = false;
        this.otheRecipientIsChecked = false;
    }

    private selectedRecipient: string = "QIFFMXS";
    get SelectedRecipient() { return this.selectedRecipient; }
    set SelectedRecipient(newValue: string) {
        if (this.selectedRecipient != newValue) {
            this.selectedRecipient = newValue;
        }
    }

    private cargRecipientIsChecked: boolean;
    get CargRecipientIsChecked() { return this.cargRecipientIsChecked; }
    set CargRecipientIsChecked(newValue: boolean) {
        if (this.cargRecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.cargRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.CargRecipient;
            }
        }
    }

    private xXArRecipientIsChecked: boolean;
    get XXArRecipientIsChecked() { return this.xXArRecipientIsChecked; }
    set XXArRecipientIsChecked(newValue: boolean) {
        if (this.xXArRecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.xXArRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.XXArRecipient;
            }
        }
    }

    private fra1RecipientIsChecked: boolean;
    get Fra1RecipientIsChecked() { return this.fra1RecipientIsChecked; }
    set Fra1RecipientIsChecked(newValue: boolean) {
        if (this.fra1RecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.fra1RecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.Fra1Recipient;
            }
        }
    }

    private fra2RecipientIsChecked: boolean;
    get Fra2RecipientIsChecked() { return this.fra2RecipientIsChecked; }
    set Fra2RecipientIsChecked(newValue: boolean) {
        if (this.fra2RecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.fra2RecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.Fra2Recipient;
            }
        }
    }

    private richRecipientIsChecked: boolean;
    get RichRecipientIsChecked() { return this.richRecipientIsChecked; }
    set RichRecipientIsChecked(newValue: boolean) {
        if (this.richRecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.richRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.RichRecipient;
            }
        }
    }

    private ihabRecipientIsChecked: boolean;
    get IhabRecipientIsChecked() { return this.ihabRecipientIsChecked; }
    set IhabRecipientIsChecked(newValue: boolean) {
        if (this.ihabRecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.ihabRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.IhabRecipient;
            }
        }
    }

    private cargonautRecipientIsChecked: boolean;
    get CargonautRecipientIsChecked() { return this.cargonautRecipientIsChecked; }
    set CargonautRecipientIsChecked(newValue: boolean) {
        if (this.cargonautRecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.cargonautRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.CargonautRecipient;
            }
        }
    }

    private dEXXRecipientIsChecked: boolean;
    get DEXXRecipientIsChecked() { return this.dEXXRecipientIsChecked; }
    set DEXXRecipientIsChecked(newValue: boolean) {
        if (this.dEXXRecipientIsChecked != newValue) {
            this.dEXXRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.DEXXRecipient;
            }
        }
    }

    private otheRecipientIsChecked: boolean;
    get OtheRecipientIsChecked() { return this.otheRecipientIsChecked; }
    set OtheRecipientIsChecked(newValue: boolean) {
        if (this.otheRecipientIsChecked != newValue) {
            this.FalseAllRadioButtons();
            this.otheRecipientIsChecked = newValue;

            if (newValue) {
                this.SelectedRecipient = this.OtherRecipient;
            }
        }
    }

    public IsSendButtonVisible: boolean = false;
    private SetSendButton() {
        var isVisible = false;

        if (FeatureLocator.IsPackage_DVMT()) {
            isVisible = true;
        }

        else {
            if (this.ValidationWarningsList.length > 0) {
                isVisible = true;
            }

            else {
                var errors: string[] = this.ValidateSending();

                if (errors.length > 0) {
                    isVisible = true;
                }
            }
        }

        this.IsSendButtonVisible = isVisible;

        if (!isVisible) {
            this.SendClicked();
        }
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    
    SendClicked() {

        this.CurrentSession.StartBusyIndicator("Prepairing data...");

        this.StockErrorMessage = null;
        this.StockErrorIsVisible = false;
        this.InitializeWebService();

        var errors: string[] = this.ValidateSending();

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            this.myCCSWebService.GetSendingValidations(this.entityPM.Id, this.SelectedRecipient, this.isSendingFHLs, this.isSendingCargonaut, this.isSendingDEXX, this.entityPM.MainCarriageCarrierId).subscribe((myResponse: ServiceResponse) => {

                if (myResponse == null) {
                    this.CurrentSession.StopBusyIndicator();
                }

                else if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    var myResult: AWBResultClass = myResponse.Result;

                    this.myValidationResultClass = myResult;

                    if (myResult.IsValid) {
                        this.InitializeSendingData();
                    }

                    else {
                        
                        this.ValidationErrorsList = myResult.ErrorsList;

                        if (myResult.HasStockErrors) {
                            this.StockErrorMessage = "You are trying to send (" + myResult.SendingCount + ") messages, your remaining stock is (" + myResult.StockRemainingBefore + ") which is insufficient for this operation. Please purchase another messaging stock via the link";
                            this.StockErrorIsVisible = true;
                        }

                        this.SendingResultForeground = this.redForeground;

                        if (this.entityPM.ShipmentLevelCode == "C" && this.isSendingFHLs) {
                            this.SendingResultMessage = "Error sending FHL(s)";
                        }

                        else {
                            this.SendingResultMessage = "Error sending " + this.MessageType;
                        }

                        this.CurrentSession.StopBusyIndicator();
                    }
                }                
            });
        }

        else {
            this.CurrentSession.StopBusyIndicator();
        }
    }
   
    // Validate
    private ValidateSending() {

        this.SendingResultMessage = "";
        var errors: string[] = [];

        this.ValidateRecipient(errors);

        if (this.entityPM.ShipmentLevelCode == "H") {
            this.ValidateFHL(errors);
        }

        else {
            if (this.entityPM.ShipmentLevelCode == "C" && this.isSendingFHLs) {
                this.ValidateFHLs(errors);
            }
        }

        return errors;
    }
    private ValidateRecipient(errors: string[]) {
        if (FeatureLocator.IsPackage_DVMT()) {
            if (AppTool.IsNullOrEmpty(this.SelectedRecipient)) {
                errors.push("Please fill your Recipient");
            }

            else {
                this.SelectedRecipient = this.SelectedRecipient.toUpperCase();

                if (this.isGLSHK) {

                }

                else {
                    //if (this.SelectedRecipient.length != 7) {
                    //    errors.push("Recipient length must be 7");
                    //}
                }
            }
        }
    }
    private ValidateFHL(errors: string[]) {
        if (AppTool.IsNullOrEmpty(this.entityPM.MasterShipmentDataId)) {
            errors.push("This FHL is not connected to FWB");
        }
    }
    private ValidateFHLs(errors: string[]) {
        if (this.entityPM.ShipmentConsoleShipments.length == 0) {
            errors.push("This FWB is not connected to any FHL");
        }

        else if (this.FHLsValidationList.filter(d => d.IsFHLValid).length == 0) {
            errors.push("All FHL(s) are invalid");
        }
    }

    // Validate Online
    private ValidateOnline() {
        
    }

    // Sending Report
    get StockAreaIsVisible() {
        var myResult = false;

        if (!this.DemoAreaIsVisible) {
            if (SessionLocator.TenantManagementJS.IsAWBStockPrepaid) {
                myResult = true;
            }
        }

        return myResult;
    }
    get DemoAreaIsVisible() {
        var myResult = false;

        if (ObjectsLocator.IsDemoTenant(this.Tenant.toString()) || SessionLocator.TenantManagementJS.IsEAWBOnlyDemo) {
            myResult = true;
        }

        return myResult;
    }
    public StockResultIsVisible: boolean;
    public StockErrorMessage: string;
    public StockErrorIsVisible: boolean;

    private mySendingResultClass: CCSResult = new CCSResult();
    private myValidationResultClass: AWBResultClass = new AWBResultClass();

    private redForeground = "#E53030";
    private greenForeground = "#009161";
    
    public SendingResultMessage: string;
    public SendingResultForeground: string;
    public SendingCount: number;
    public StockRemainingBefore: number;
    public StockRemainingAfter: number;
    private SetStockScreen() {
        if (this.isSendingFHLs) {
            this.SendingCount = this.myValidationResultClass.SendingCount;
            this.StockRemainingBefore = this.myValidationResultClass.StockRemainingBefore;
        }

        else {
            this.SendingCount = this.mySendingResultClass.SendingCount;
            this.StockRemainingBefore = this.mySendingResultClass.StockRemainingBefore;
        }

        this.StockRemainingAfter = this.mySendingResultClass.StockRemainingAfter;

        this.StockResultIsVisible = false;
        this.StockErrorIsVisible = false;

        if (this.StockRemainingAfter < this.StockRemainingBefore) {
            this.StockResultIsVisible = true;
        }
    }

    private sendingQueueIndex: number;
    private entitiesDataOnQueue: string[];
    private InitializeSendingData() {
        this.sendingQueueIndex = 0;
        this.entitiesDataOnQueue = [];

        if (this.myValidationResultClass.IsSendingFHLs) {
            this.entitiesDataOnQueue = this.myValidationResultClass.ValidFHLsDataStringList;
        }

        else {
            this.entitiesDataOnQueue.push(this.entityPM.Id + ":" + this.entityPM.ShipmentNumber);
        }

        this.SendData();
    }
    private SendData() {

        if (this.entitiesDataOnQueue.length == 0) {
            this.SetStockScreen();            

            this.SendingResultForeground = this.greenForeground;

            if (this.entityPM.ShipmentLevelCode == "C" && this.isSendingFHLs) {
                this.SendingResultMessage = "All valid FHL(s) have been sent Successfully";
            }

            else {
                this.SendingResultMessage = this.MessageType + " has been sent Successfully";
            }

            this.CurrentSession.StopBusyIndicator();

            this.ReloadEntity();

            //this.currentAssemlyLocator.ListControl.GetSingleList(shipmentPM.Id);
            //this.currentAssemlyLocator.CurrentSimplogWindow.StopBusyIndicator();

            //RefreshScreenEvent myEvent = SessionLocator.CurrentAssemblyLocator.EventAggregator.GetEvent<RefreshScreenEvent>();
            //myEvent.Publish(new RefreshScreenEventArgs("ChampService"));
        }

        else {
            this.sendingQueueIndex += 1;
            var itemDataString: string = this.entitiesDataOnQueue[0];
            var itemStringArray: string[] = itemDataString.split(':');
            var entityId: string = itemStringArray[0];
            var entityNumber: string = itemStringArray[1];

            var indexOfItem = this.entitiesDataOnQueue.indexOf(itemDataString);
            if (indexOfItem > -1) {
                this.entitiesDataOnQueue.splice(indexOfItem, 1);
            }

            this.Sending(entityId, entityNumber);
        }
    }
    private Sending(entityId: string, entityNumber: string) {

        var busyIndicatorText:string = "Sending in Progress..";

        if (this.isSendingFHLs) {
            busyIndicatorText = "Sending FHL (" + this.sendingQueueIndex + " of " + this.myValidationResultClass.ValidHousesCount + ") " + entityNumber;
        }

        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.StartBusyIndicator(busyIndicatorText);

        this.myCCSWebService.Send(entityId, this.SelectedRecipient, this.isSendingCargonaut, this.isSendingDEXX).subscribe((myResponse: ServiceResponse) => {
            if (myResponse == null) {
                this.CurrentSession.StopBusyIndicator();
            }

            else if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }

            else {
                var myResult: CCSResult = myResponse.Result;

                this.mySendingResultClass = myResult;

                if (myResult == null) {
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    if (myResult.HasStockError) {
                        this.SendingResultForeground = this.redForeground;
                        this.SendingResultMessage = "Error sending: No remaining stock";
                        this.CurrentSession.StopBusyIndicator();
                    }

                    if (myResult.IsFNAValidationLong) {
                        this.ValidationErrorsList.push(TextCodeTranslator.Translate("Shipment.M.AWB.ValidateFNA"));
                        this.CurrentSession.StopBusyIndicator();
                    }

                    else {
                        this.SendData();
                    }
                }
            }
        });
    }

    private ReloadEntity() {

        this.Wizard.LoadCompleted.subscribe(($event: any) => {
            this.LoadFHLValidators();
        });

        this.Wizard.ReloadEntity();
    }
}

export class FHLStatusItemViewModel {
    public IsFHLValid: boolean = false;
    public Shipper: string;
    public StatusName: string;
    public ShipmentNumber: string;    
    constructor(private item: FHLShipmentValidator, private fatherComponent: SendWindowComponent, isSendingCargonaut: boolean) {
        this.Shipper = item.Shipper;
        this.StatusName = isSendingCargonaut ? item.CargonautFHLStatusName : item.FHLStatusName;
        this.ShipmentNumber = item.ShipmentNumber;
        this.IsFHLValid = item.IsFHLValid;
    }

    ViewShipment() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = "Edit HAWB Wizard";
        logWindow.WindowArgs = this.item.ShipmentId;
        logWindow.WindowClosed.subscribe(($event: any) => this.fatherComponent.LoadFHLValidators());
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');
    }
}
