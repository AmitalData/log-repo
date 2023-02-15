declare var window: any;
import {CustomerPM} from '../../EntityPMs/CustomerPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {TenantPM} from '../../EntityPMs/TenantPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {CustomerPMService} from '../../Services/StandardPMs/CustomerPMService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {PartnersDomainService} from '../../Services/PartnersDomainService';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ActionStepsTemplate, ActionStepsTemplateArgs} from '../../../CommonModules/CommonPartners/Components/Templates/ActionStepsTemplate';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CustomerActivationArgs} from '../../../Common/Args';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';

import {CreateTenantHelper} from '../../../InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantHelper';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {TenantManagementPMService} from '../../../Infrastructure/Services/StandardPMs/TenantManagementPMService';
import {TenantManagementPM} from '../../../Infrastructure/EntityPMs/TenantManagementPM';
import {DownloadManager} from '../../../Infrastructure/Utilities/DownloadManager';
import { CardExtendedPMService } from '../../Services/ExtendedPMs/CardExtendedPMService';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';

export class CustomerMenuButtonsHandler {
    public EntityPM: CustomerPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "Customer"
    cardExtendedPMService: CardExtendedPMService = new CardExtendedPMService();
    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    if (this.isSetAsPotential) {
                        this.isSetAsPotential = false;
                        this.SetAsPotential();
                    }

                    if (this.isInActiveCustomer) {
                        this.isInActiveCustomer = false;
                        this.InActiveCustomer();
                    }

                    if (this.isReActivateCustomer) {
                        this.isReActivateCustomer = false;
                        this.ReActivateCustomer();
                    }

                    if (this.isSetMyCustomer) {
                        this.isSetMyCustomer = false;
                        this.SetMyCustomer();
                    }

                    if (this.isSetNotMyCustomer) {
                        this.isSetNotMyCustomer = false;
                        this.SetNotMyCustomer();
                    }

                    if (this.isActivate) {
                        this.isActivate = false;
                        if (this.activationTag == "activate") {
                            this.ActivateCustomer();
                        }
                    }

                    if (this.isMarkAsReadyActivation) {
                        this.isMarkAsReadyActivation = false;
                        if (this.activationTag == "ready") {
                            this.MarkAsReadyForActivation();
                        }
                    }
                }
            });
        }
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'Customer')[0];
                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "InviteCustomerContacts":
                        case "LoginToCustomerPortal":
                            {
                                if (this.EntityPM.CustomerStatusCode !== "ACT" || !this.TenantPM.IsDigitalPortalAccessActivated) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "SetAsPotential":
                            {
                                if (this.TenantPM.IsHybrid) {
                                    if (this.EntityPM.CustomerStatusCode == "POT") {
                                        button.IsDisabled = true;
                                    }

                                    else {
                                        button.IsDisabled = false;
                                    }
                                }

                                else {
                                    if (this.EntityPM.CustomerStatusCode == "POT" || this.EntityPM.CustomerStatusCode == "INA") {
                                        button.IsDisabled = true;
                                    }

                                    else {
                                        button.IsDisabled = false;
                                    }
                                }                             

                                break;
                            }

                        case "Activate":
                            {
                                if (this.EntityPM.CustomerStatusCode == "ACT" || this.EntityPM.CustomerStatusCode == "INA" || this.EntityPM.InActive) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }

                                break;
                            }

                        case "ReadyActivate":
                            {
                                button.Width = 130;

                                if (this.EntityPM.CustomerStatusCode == "ACT" || this.EntityPM.CustomerStatusCode == "INA" || this.EntityPM.CustomerStatusCode == "WAC") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }

                        case "InActiveCustomer":
                            {
                                if (this.TenantPM.IsHybrid && (this.EntityPM.CustomerStatusCode == "ACT" || this.EntityPM.CustomerStatusCode == "WAC")) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    if (this.TenantPM.IsHybrid) {
                                        if (this.EntityPM.CustomerStatusCode == "POT") {
                                            button.IsDisabled = false;
                                        }
                                        else {
                                            button.IsDisabled = true;
                                        }
                                    }
                                    else {
                                        if (this.EntityPM.CustomerStatusCode == "INA" || this.EntityPM.InActive) {
                                            button.IsDisabled = true;
                                        }
                                        else {
                                            button.IsDisabled = false;
                                        }
                                    }
                                }
                                break;
                            }

                        case "ReActivateCustomer":
                            {
                                if (this.TenantPM.IsHybrid && (this.EntityPM.CustomerStatusCode == "ACT" || this.EntityPM.CustomerStatusCode == "WAC")) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    if (this.EntityPM.CustomerStatusCode == "INA" || this.EntityPM.InActive) {
                                        button.IsDisabled = false;
                                    }
                                    else {
                                        button.IsDisabled = true;
                                    }
                                }

                                break;
                            }

                        case "SetMyCustomer":
                            {
                                if (this.TenantPM.IsHybrid && (this.EntityPM.CustomerStatusCode == "ACT" || this.EntityPM.CustomerStatusCode == "WAC")) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    if (this.EntityPM.IsCustomer) {
                                        button.IsDisabled = true;
                                    }

                                    else {
                                        button.IsDisabled = false;
                                    }
                                }
                                break;
                            }

                        case "SetNotMyCustomer":
                            {
                                if (this.TenantPM.IsHybrid && (this.EntityPM.CustomerStatusCode == "ACT" || this.EntityPM.CustomerStatusCode == "WAC")) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    if (!this.EntityPM.IsCustomer) {
                                        button.IsDisabled = true;
                                    }
                                    else {
                                        button.IsDisabled = false;
                                    }
                                }
                                break;
                            }

                        case "ViewQuestionnaireAnswersCustomer":
                            {
                                if (this.EntityPM.CustomerStatusCode == "WAC") {
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                                break;
                            }

                        case "TenantManagement":
                            {
                                button.Width = 90;
                                break;
                            }

                        case "Totango":
                            {
                                button.Width = 90;
                                break;
                            }

                        case "CreateTenant":
                            {
                                button.Width = 140;
                                if (!FeatureLocator.HasFeaturePermession("Customer", "CREATETENANT")) button.IsHidden = true;
                                else button.IsHidden = false;
                                break;
                            }
                        case "Disconnect": {

                            if (!SessionLocator.TenantPM.AccountingActivated) {
                                button.IsHidden = true;
                            }
                            else {
                                if (this.EntityPM.Card.GLAccountId) {
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                            }




                            break;
                        }
                        //case "More": {
                        //    if (!SessionLocator.TenantPM.AccountingActivated) {
                        //        button.IsHidden = true;
                        //    }
                        //    else {
                        //        button.IsHidden = false;
                        //    }
                        //    break;
                        //}
                    }
                }
            }
        }

        return menuButtons;
    }
    public MenuButtonClick(menuButton: MenuButtonPM) {

        switch (menuButton.EventCode) {
            case "SetAsPotential":
                {
                    this.ResetAllFlags();
                    this.isSetAsPotential = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }

            case "Activate":
                {
                    this.isActivate = true;
                    this.ReloadCurrentCustomer("activate");
                    break;
                }

            case "ReadyActivate":
                {
                    this.isMarkAsReadyActivation = true;
                    this.ReloadCurrentCustomer("ready");
                    break;
                }

            case "InActiveCustomer":
                {
                    this.ResetAllFlags();
                    this.isInActiveCustomer = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }

            case "ReActivateCustomer":
                {
                    this.ResetAllFlags();
                    this.isReActivateCustomer = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }

            case "SetMyCustomer":
                {
                    this.ResetAllFlags();
                    this.isSetMyCustomer = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }

            case "SetNotMyCustomer":
                {
                    this.ResetAllFlags();
                    this.isSetNotMyCustomer = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }

            case "TenantManagement":
                {
                    this.EditTenantManagement();
                    break;
                }

            case "ViewQuestionnaireAnswersCustomer":
                {
                    this.ViewQuestionnaireAnswers();
                    break;
                }

            case "Totango":
                {
                    this.GoTotango();
                    break;
                }


            case "CreateTenant":
                {
                    this.CreateTenantMethod();
                    break;
                }

            case "Disconnect": {
                this.DisconnectGLAccount();
                break;
            }
            case "InviteCustomerContacts": {
                this.InviteCustomerContacts();
                break;
            }
            case "LoginToCustomerPortal": {
                this.RedirctToDigital();
                break;
            }
        }
    }

    // Props 
    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    private isSetAsPotential: boolean;
    private isInActiveCustomer: boolean;
    private isSetMyCustomer: boolean;
    private isSetNotMyCustomer: boolean;
    private isReActivateCustomer: boolean;
    private isActivate: boolean;
    private isMarkAsReadyActivation: boolean;
    private ResetAllFlags() {
        this.isSetAsPotential = false;
        this.isInActiveCustomer = false;
        this.isSetMyCustomer = false;
        this.isSetNotMyCustomer = false;
        this.isReActivateCustomer = false;
        this.isActivate = false;
        this.isMarkAsReadyActivation = false;
    }

    private CurrentSession = SessionLocator.SelectedSession;
    SetAsPotential()
    {
       this.IsCustomerConnectedToEntities();    
    }

    private isCustomerConnectedToEntities: boolean = false;
    private IsCustomerConnectedToEntities() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetIsCustomerConnectedToEntities(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.isCustomerConnectedToEntities = myResponse.Result;

                if (this.isCustomerConnectedToEntities)
                {
                    var messageWindow: MessageWindow = new MessageWindow();
                    var validationErrorMessage = "This customer can't be set as potential since it has shipment(s).";
                    messageWindow.Show(validationErrorMessage);
                }
                else
                {
                    this.EntityPM.SetAsPotential = true;
                    this.EntityPM.CustomerStatusCode = "POT";
                    this.isSetAsPotential = false;
                    this.entityArgs.EditComponent.SaveChanges();
                }
            }
        });
    }

    private activationTag: string = null;
    ReloadCurrentCustomer(tag: string) {
        this.activationTag = tag;
        if (this.EntityPM != null && !AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.GetSingleEntity();
        }
    }
    private GetSingleEntity() {
        var myService: CustomerPMService = new CustomerPMService();
        myService.get(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result;
                }
                this.entityArgs.EditComponent.SaveChanges();
            }
        });
    }
    ActivateCustomer() {
        if (this.EntityPM != null) {
            var args = new CustomerActivationArgs();
            args.EntityPM = this.EntityPM ;
            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Title = "Customer Activation";
            logWindow.Show('./CommonModules/CommonCustomer/Components/CustomerActivation/CustomerActivationComponent');
            logWindow.WindowClosed.subscribe(s => {
                this.isActivate = false;
                if (s) {
                    this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
        }
    }
    MarkAsReadyForActivation() {
        if (this.EntityPM != null) {
            var args = new CustomerActivationArgs();
            args.EntityPM = this.EntityPM;
            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Width = 900;
            logWindow.Height = 700;
            logWindow.Title = "Customer Activation";
            logWindow.Show('./CommonModules/CommonCustomer/Components/CustomerActivation/ReadyForActivationComponent');
            logWindow.WindowClosed.subscribe(s => {
                this.isActivate = false;
                if (s) {
                    this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
        }
    }

    InActiveCustomer() {
        var args = new ActionStepsTemplateArgs();
        args.ObjectTableName = this.ObjectTableName;
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Deactivate Customer Notes";
        args.IsNotesStackPanelVisible = true;
        args.EventNote = "";
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 450;
        logWindow.Height = 300;
        logWindow.Title = "Deactivate Customer";
        //logWindow.WindowClosed.subscribe(($event: any) => this.OnInActiveCustomerWindowClosed($event));
        logWindow.Show('./CommonModules/CommonPartners/Components/Templates/ActionStepsTemplate');

        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var notes = s.EventNotes;
                if (!AppTool.IsNullOrEmpty(notes)) {
                    this.EntityPM.EventNote = notes;
                }

                this.OnInActiveCustomerWindowClosed(d);
            });
        });

    }
    OnInActiveCustomerWindowClosed(arg: any) {
        if (arg == 'confirm') {
            this.EntityPM.SetInActive = true;
            var errors: string[] = [];
            Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

            if (errors.length == 0) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    }

    ReActivateCustomer() {
        var args = new ActionStepsTemplateArgs();
        args.ObjectTableName = this.ObjectTableName;
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Reactivate Customer Notes";
        args.EventNote = "";
        args.IsNotesStackPanelVisible = true;

        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 450;
        logWindow.Height = 300;
        logWindow.Title = "Reactivate Customer";
        //logWindow.WindowClosed.subscribe(($event: any) => this.OnReActivateCustomerWindowClosed($event));
        logWindow.Show('./CommonModules/CommonPartners/Components/Templates/ActionStepsTemplate');

        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var notes = s.EventNotes;
                if (!AppTool.IsNullOrEmpty(notes)) {
                    this.EntityPM.EventNote = notes;
                }

                this.OnReActivateCustomerWindowClosed(d);
            });
        });
    }
    OnReActivateCustomerWindowClosed(arg: any) {
        if (arg == 'confirm') {
            this.EntityPM.SetReActivated = true;
            var errors: string[] = [];
            Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

            if (errors.length == 0) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    }

    SetMyCustomer() {
        this.EntityPM.IsCustomer = true;
        this.StartBusyIndicator("Saving...");
        this.entityArgs.EditComponent.SaveChanges();
        this.StopBusyIndicator();
    }

    SetNotMyCustomer() {
        this.EntityPM.IsCustomer = false;
        this.StartBusyIndicator("Saving...");
        this.entityArgs.EditComponent.SaveChanges();
        this.StopBusyIndicator();
    }


  

    EditTenantManagement() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ReceivablesAccountingCard)) {
            var id: number = parseInt(this.EntityPM.ReceivablesAccountingCard);

            var managementService: TenantManagementPMService = new TenantManagementPMService();
            managementService.get(id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    var window: MessageWindow = new MessageWindow();
                    window.Width = 350;
                    window.Height = 180;
                    window.Show(myResponse.ErrorsArray[0]);
                }

                else {
                    var ten: TenantManagementPM = myResponse.Result;

                    if (ten != null) {
                        this.StartEditing(ten.Id);
                    }
                }
            });

        }
        else {
            var window: MessageWindow = new MessageWindow();
            window.Width = 350;
            window.Height = 180;
            window.Show("Please fill accounting external Id field");
        }
    }

    private DisconnectGLAccount() {
        this.CurrentSession.StartBusyIndicator("Loading...");
        this.cardExtendedPMService.DisconnectGLAccountFromCard(this.EntityPM.Id, "CS", "CSDS").subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.CurrentSession.StopBusyIndicator();
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });

    }

    StartEditing(id) {
        var editWindow: LogitudeWindow = new LogitudeWindow();
        editWindow.IsFillScreen = true;
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = "Edit Tenant Management";
        editWindow.IsEditComponent = true;
        editWindow.WindowClosed.subscribe(event => {
        });
        editWindow.ShowEditComponent(id, "TenantManagement", "", true);
    }


    ViewQuestionnaireAnswers() {
        var table = window.ObjectTables.filter(d => d.Name === 'Customer')[0];
        var customerName = !AppTool.IsNullOrEmpty(this.EntityPM.EnglishName) ? this.EntityPM.EnglishName : "";

        var entityId = this.TenantPM.DefaultQuestionnaireId + "_" + table.Id + "_" + this.EntityPM.Id + "_" + "QuestionnaireAnswers" + "_" + customerName;
        DownloadManager.DownloadPage(entityId);

    }

    GoTotango() {

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ReceivablesAccountingCard)) {
            var link = "https://app.totango.com/#!/customerDetails?customer=" + this.EntityPM.ReceivablesAccountingCard;
            window.open(link, '_blank');
        }
        else {
            var message: MessageWindow = new MessageWindow();
            message.Width = 350;
            message.Height = 180;
            message.Show("Please Fill External Id Field");
        }

    }
   
    CreateTenantMethod() {
        var confirmWindow: ConfirmWindow = new ConfirmWindow();
        confirmWindow.Title = "";
        confirmWindow.Width = 400;
        confirmWindow.Height = 200;
        confirmWindow.Show("Please confirm creating a new tenant for this customer ?");
        confirmWindow.WindowClosed.subscribe(event => {
            if (confirmWindow.Yes) {
                var createTenantHelper: CreateTenantHelper = new CreateTenantHelper("Customer", this.EntityPM.Id, this.EntityPM.EnglishName, this.EntityPM.ReceivablesAccountingCard, this.EntityPM.PrimaryContactId, this.EntityPM.VatNumber, this.EntityPM.CountryName, this.EntityPM.CountryCode);
                createTenantHelper.CreateTenantMethod();
            }

        });

    }

    private RedirctToDigital() {
        window.open(`https://${SessionLocator.TenantManagementJS.CustomerURL}/online-visibility?securitykey=${ServiceHelper.GetLoggedUserToken()}&cid=${this.EntityPM.Id}&ctype=${this.EntityPM.PartnerTypeId}`, "_blank");
    }

    private InviteCustomerContacts() {
        var windowArgs: any = {};
        windowArgs.IsDigitalPortal = true;
        windowArgs.CurrentEntity = this.EntityPM;
        windowArgs.IsFromCustomerEdit = true;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1100;
        logWindow.Height = 570;
        logWindow.Title = this.ObjectTableName == "Card" ? "Invite Partners" : "Invite Contacts";
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./SharedLogistics/Components/InviteCustomersComponent');
        logWindow.WindowClosed.subscribe(($event1: any) => {


        });
    }


    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }
}
