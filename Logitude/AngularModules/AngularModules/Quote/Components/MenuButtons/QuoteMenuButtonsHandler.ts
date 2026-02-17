declare var window: any;
import {NewQuoteComponentArgs, QuoteEventNotesArgs} from '../../Args';
import {QuotePM} from '../../EntityPMs/QuotePM';
import {QuoteUtilities} from '../../Utilities/QuoteUtilities';
import {QuoteStageList} from '../../EntityLists/QuoteStageList';
import {QuoteStageListService} from '../../Services/StandardLists/QuoteStageListService';
import {QuoteValidator}  from '../../Validators/QuoteValidator';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {AppTool} from '../../../Infrastructure/Tools';
import {PartnersDomainService} from '../../../Common/Services/PartnersDomainService';
import {CustomerPM} from '../../../Common/EntityPMs/CustomerPM';
import {CustomerActivationArgs} from '../../../Common/Args';
import {ShipmentPM} from '../../../Shipment/EntityPMs/ShipmentPM';
import {NewShipmentComponentArgs} from '../../../Shipment/Args';
import {ShipmentDomainService} from '../../../Shipment/Services/ShipmentDomainService';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {QuoteDomainService} from '../../../Quote/Services/QuoteDomainService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';

export class QuoteMenuButtonsHandler {
    public EntityPM: QuotePM;
    public entityArgs: EntityArgs
    private CurrentSession = SessionLocator.SelectedSession;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.myQuoteStageListService = new QuoteStageListService();
        this.myPartnersDomainService = new PartnersDomainService();
        this.entityResourceService = new EntityResourceService();

        this.Listen();
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            this.allStages = [];
            this.myQuoteStageListService.getAllFromCache().subscribe((resp: any) => {
                if (!resp.HasError) {
                    this.allStages = resp.Result;
                }
            });

            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'Quote')[0];

                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];

                    if (button.EventCode == "BuildShipment") {
                        button.Width = 100;

                        var myDeclinedStageId: string;
                        var myDeclinedStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTDC")[0];

                        if (myDeclinedStage != null) {
                            myDeclinedStageId = myDeclinedStage.Id;
                        }

                        if (this.EntityPM.QuoteTypeCode == "P" || this.EntityPM.IsCancelled || this.EntityPM.StageId == myDeclinedStageId) {
                            button.IsDisabled = true;
                        }

                        else {
                            button.IsDisabled = false;
                        }
                    }

                    if (button.EventCode == "CopyQuote") {
                        button.IsDisabled = false;
                    }

                    if (button.EventCode == "SetAsSentToCustomer") {
                        var mySentStageId = null;
                        var mySentStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTST")[0];

                        if (mySentStage != null) {
                            mySentStageId = mySentStage.Id;
                        }

                        if (this.EntityPM.IsClosed || this.EntityPM.StageId == mySentStageId) {
                            button.IsDisabled = true;
                        }

                        else {
                            button.IsDisabled = false;
                        }
                    }

                    if (button.EventCode == "CancelQuote") {
                        var myDraftStageId;
                        var myCreatedStageId;

                        var myDraftStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTDR")[0];
                        var myCreatedStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTCR")[0];

                        if (myDraftStage != null) {
                            myDraftStageId = myDraftStage.Id;
                        }

                        if (myCreatedStage != null) {
                            myCreatedStageId = myCreatedStage.Id;
                        }

                        if (this.EntityPM.StageId == myDraftStageId || this.EntityPM.StageId == myCreatedStageId) {
                            if (this.EntityPM.IsCancelled) {
                                button.IsDisabled = true;
                            }

                            else {
                                button.IsDisabled = false;
                            }
                        }

                        else {
                            button.IsDisabled = true;
                        }
                    }

                    if (button.EventCode == "ReactivateQuote") {
                        if (!this.EntityPM.IsCancelled) {
                            button.IsDisabled = true;
                        }

                        else {
                            button.IsDisabled = false;
                        }
                    }

                    if (button.EventCode == "ReturnToDraft") {
                        var mySentStageId = null;
                        var mySentStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTST")[0];

                        if (mySentStage != null) {
                            mySentStageId = mySentStage.Id;
                        }

                        if (this.EntityPM.IsClosed) {
                            button.IsDisabled = false;
                        }

                        else if (this.EntityPM.StageId == mySentStageId) {
                            button.IsDisabled = false;
                        }

                        else {
                            button.IsDisabled = true;
                        }
                    }

                    if (button.EventCode == "Accept") {
                        if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
                            button.IsDisabled = true;
                        }

                        else {
                            button.IsDisabled = false;
                        }
                    }

                    if (button.EventCode == "Decline") {
                        if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
                            button.IsDisabled = true;
                        }

                        else {
                            button.IsDisabled = false;
                        }
                    }

                    if (button.EventCode == "Quotation") {
                        button.Width = 70;

                        if (this.EntityPM.QuoteTypeCode != "A") {
                            button.IsDisabled = true;
                        }

                        else {
                            button.IsDisabled = false;
                        }

                        if (this.EntityPM.IsQuoteDataExternal && this.EntityPM.IsQuoteDocumentExternal) {
                            button.IsDisabled = true;
                        }
                    }

                    if (button.EventCode == "ConvertQuotetoLCL") {
                        if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
                            if (this.EntityPM.TransportModeId == "O") {
                                if (this.EntityPM.ShipmentTypeId == "FCLD") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsHidden = true;
                                }
                            }
                        }

                        else {
                            if (this.EntityPM.TransportModeId == "O") {
                                if (this.EntityPM.ShipmentTypeId == "FCLD") {
                                    button.IsHidden = false;
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsHidden = true;
                                }
                            }

                            else {
                                button.IsHidden = true;
                            }
                        }
                    }

                    if (button.EventCode == "ConvertQuotetoFCL") {
                        if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
                            if (this.EntityPM.TransportModeId == "O") {
                                if (this.EntityPM.ShipmentTypeId == "LCLD") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsHidden = true;
                                }
                            }
                        }

                        else {
                            if (this.EntityPM.TransportModeId == "O") {
                                if (this.EntityPM.ShipmentTypeId == "LCLD") {
                                    button.IsHidden = false;
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsHidden = true;
                                }
                            }

                            else {
                                button.IsHidden = true
                            }
                        }
                    } 
                }

                return menuButtons;
            }
        }
    }
    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (!this.isButtonClicked) {

            this.StopFlags();
            this.isButtonClicked = true;

            switch (menuButton.EventCode) {
                case "BuildShipment":
                    {
                        this.BuildShipment();
                        break;
                    }

                case "CopyQuote":
                    {
                        this.CopyQuote();
                        break;
                    }

                case "SetAsSentToCustomer":
                    {
                        this.SetAsSentToCustomer();
                        break;
                    }

                case "CancelQuote":
                    {
                        this.CancelQuote();
                        break;
                    }

                case "ReactivateQuote":
                    {
                        this.ReactivateQuote();
                        break;
                    }

                case "ReturnToDraft":
                    {
                        this.ReturnToDraft();
                        break;
                    }

                case "Accept":
                    {
                        this.SetAsAccepted();
                        break;
                    }

                case "Decline":
                    {
                        this.SetAsDeclined();
                        break;
                    }
                case "Quotation":
                    {
                        this.RunQuotationScreen();
                        break;
                    }

                case "ConvertQuotetoLCL":
                    {
                        this.ConvertQuoteToLCLClicked();
                        break;
                    }


                case "ConvertQuotetoFCL":
                    {
                        this.ConvertQuoteToFCLClicked();
                        break;
                    }

                default: {
                    this.isButtonClicked = false;
                    break;
                }
            }
        }
    }

    private IsConvertToLCLClicked: boolean = false;
    private IsConvertToFCLClicked: boolean = false;
    private ConvertQuoteToLCLClicked() {
        var errors: string[] = [];
        this.Validate();

        if (this.isValid) {
            this.IsConvertToLCLClicked = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
    }
    private ConvertQuoteToFCLClicked() {
        var errors: string[] = [];
        this.Validate();

        if (this.isValid) {
            this.IsConvertToFCLClicked = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
    }
    private DoConvertQuoteType(type: string) {
        var args = new QuoteEventNotesArgs();
        var windowTitle: string;
        
        args.EntityPM = this.EntityPM;        
        args.IsConvertQuoteType = true;         
        args.NotesHeader = "Notes";        

        switch (type) {
            case "ToLCL":
                {
                    windowTitle = "Convert Quote From FCL To LCL";
                    break;
                }

            case "ToFCL":
                {
                    windowTitle = "Convert Quote From LCL To FCL";
                    break;
                }
        }

        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;

        logWindow.Title = windowTitle;
        logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var notes = s.EventNote;
                if (d == "OK") {
                    this.EntityPM.EventNote = notes;

                    switch (type) {
                        case "ToLCL":
                            {
                                this.EntityPM.ConvertToLCL = true;
                                this.EntityPM.ConvertToFCL = false;
                                break;
                            }

                        case "ToFCL":
                            {
                                this.EntityPM.ConvertToLCL = false;
                                this.EntityPM.ConvertToFCL = true;
                                break;
                            }
                    }

                    this.Reload = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    this.isButtonClicked = false;
                }
            });
        });
    }

    private myQuoteStageListService: QuoteStageListService;
    private myPartnersDomainService: PartnersDomainService;
    private entityResourceService: EntityResourceService;
    private allStages = [];
    
    isValid: boolean = false;
    isButtonClicked: boolean = false;
    Reload: boolean = false;
    StopFlags() {
        this.isButtonClicked = false;
        this.isBuildingShipment = false;
        this.isCopyingQuote = false;
        this.IsRunQuotation = false;
        this.Reload = false;
        this.IsConvertToLCLClicked = false;
        this.IsConvertToFCLClicked = false;
    }
    Validate() {
        var validator = new QuoteValidator();
        var errors: string[] = validator.Validate(this.EntityPM);

        this.isValid = errors.length == 0 ? true : false;

        this.entityArgs.EditComponent.ValidationErrorsList = errors;

        if (!this.isValid) {
            this.StopFlags();
        }
    }
    Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                this.isButtonClicked = false;
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    if (this.isBuildingShipment) {
                        this.StartBuildingShipment();
                    }

                    if (this.isCopyingQuote) {
                        this.StartCopyQuote();
                    }

                    if (this.IsRunQuotation) {
                        this.OpenQuotationWindow();
                    }

                    if (this.IsConvertToLCLClicked) {
                        this.DoConvertQuoteType("ToLCL");
                    }

                    if (this.IsConvertToFCLClicked) {
                        this.DoConvertQuoteType("ToFCL");
                    }
                    
                    if (this.Reload) {
                        this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                }

                this.StopFlags();
            });

            this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }

                this.StopFlags();
            });
        }
    }   

    private isBuildingShipment: boolean = false;
    private BuildShipment() {
        this.Validate();

        if (this.isValid) {
            if (this.EntityPM.QuoteTypeCode != "A") {
                var window = new MessageWindow();
                window.Show(TextCodeTranslator.Translate("Quote.M.BuildShipmentMessage"));

                this.isButtonClicked = false;
            }

            else {
                var list: QuoteStageList = this.allStages.filter(d => d.Id == this.EntityPM.StageId)[0];

                if (list != null) {
                    //list.Name == "Used" || 
                    if (list.Code == "QTAC") {
                        this.StartBuildingShipment();
                    }

                    else {
                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Width = 350;
                        logitudeWindow.Height = 170;
                        logitudeWindow.Title = TextCodeTranslator.Translate("Quote.B.BuildShipment");
                        logitudeWindow.Show('./Quote/Components/MenuButtons/ApproveBuildShipmentComponent');

                        logitudeWindow.ComponentLoaded.subscribe(comp => {
                            logitudeWindow.WindowClosed.subscribe(s => {
                                if (comp.Approving) {
                                    this.isBuildingShipment = true;
                                    this.SetAsAccepted();
                                }
                                else {
                                    this.isButtonClicked = false;
                                }
                            });
                        });
                    }
                }
            }
        }
    }
    private StartBuildingShipment() {
        if (this.EntityPM.IsPotentialShipper) {
            this.entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(response => {
                this.ConvertShipper();
            });
        }

        else if (this.EntityPM.IsPotentialConsignee) {
            this.entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(response => {
                this.ConvertConsignee();
            });
        }

        else {
            this.OpenNewShipmentComponent();
        }
    }
    private ConvertShipper() {
        this.myPartnersDomainService.GetCustomerById(this.EntityPM.ShipperId).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                var shipper: CustomerPM = myResponse.Result;

                if (shipper != null) {
                    var args = new CustomerActivationArgs();
                    args.EntityPM = shipper;
                    args.ActivatedFromQuoteSide = true;
                    args.ActivatedPartnerType = "SH";

                    var logWindow = new LogitudeWindow();
                    logWindow.WindowArgs = args;
                    logWindow.Title = "Shipper Activation";
                    logWindow.Show('./CommonModules/CommonCustomer/Components/CustomerActivation/CustomerActivationComponent');

                    logWindow.WindowClosed.subscribe(s => {
                        this.isButtonClicked = false;
                        if (s) {
                            this.entityArgs.EditComponent.ReloadEntityPM();

                            if (this.EntityPM.IsPotentialConsignee) {
                                this.ConvertConsignee();
                            }
                            else {
                                this.OpenNewShipmentComponent();
                            }
                        }
                    });
                }
            }
        });
    }
    private ConvertConsignee() {
        this.myPartnersDomainService.GetCustomerById(this.EntityPM.ConsigneeId).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                var consignee: CustomerPM = myResponse.Result;

                if (consignee != null) {
                    var args = new CustomerActivationArgs();
                    args.EntityPM = consignee;
                    args.ActivatedFromQuoteSide = true;
                    args.ActivatedPartnerType = "CO";
                    
                    var logWindow = new LogitudeWindow();
                    logWindow.WindowArgs = args;
                    logWindow.Title = "Consignee Activation";
                    logWindow.Show('./CommonModules/CommonCustomer/Components/CustomerActivation/CustomerActivationComponent');

                    logWindow.WindowClosed.subscribe(s => {
                        this.isButtonClicked = false;

                        if (s) {
                            this.entityArgs.EditComponent.ReloadEntityPM();
                            this.OpenNewShipmentComponent();
                        }
                    });
                }
            }
        });
    }
    private OpenNewShipmentComponent() {
        this.entityResourceService.getEntityResourceByTableName("Shipment", 0).subscribe(response => {
            var shipmentPM: ShipmentPM = QuoteUtilities.BuildShipment(this.EntityPM);

            var args = new NewShipmentComponentArgs();
            args.Shipment = shipmentPM;
            args.IsBuildFromQuote = true;

            var str: string = TextCodeTranslator.Translate("General.O.NewEntity");
            str = str.replace("%Entity", TextCodeTranslator.TranslateTable("Shipment"));

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.WindowArgs = args;
            logWindow.Title = str;
            logWindow.Show('./Shipment/Components/NewShipment/NewShipmentComponent');
            logWindow.ComponentLoaded.subscribe(cmp => {
                cmp.ShowShipmentLevels = true;
                this.isButtonClicked = false;
            });

            //logWindow.WindowClosed.subscribe(s => {
            //    this.isButtonClicked = false;
            //});
        });        
    }

    private isCopyingQuote: boolean = false;
    private CopyQuote() {
        this.Validate();

        if (this.isValid) {
            this.isCopyingQuote = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
    }
    private StartCopyQuote() {
        var args = new NewQuoteComponentArgs();
        args.Quote = this.EntityPM;
        args.IsCopyFromQuote = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = TextCodeTranslator.Translate("Quote.B.CopyQuote");
        logWindow.Show('./Quote/Components/NewEntity/NewQuoteComponent');

        logWindow.WindowClosed.subscribe(s => {
            this.StopFlags();
        });
    }

    private SetAsSentToCustomer() {
        this.Validate();

        if (this.isValid) {
            var args = new QuoteEventNotesArgs();
            args.EntityPM = this.EntityPM;
            args.NotesHeader = TextCodeTranslator.Translate("Quote.F.Notes");

            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Width = 450;
            logWindow.Height = 300;
            logWindow.Title = TextCodeTranslator.Translate("Quote.B.SetAsSent");
            logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');

            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.OnNotesWindowClosed("sent");
                }
                else {
                    this.isButtonClicked = false;
                }
            });
        }
    }

    private CancelQuote() {
        this.Validate();

        if (this.isValid) {
            var myService: ShipmentDomainService = new ShipmentDomainService();
            myService.GetShipmentsCountByQuoteId(this.EntityPM.Id).subscribe((myResult: ServiceResponse) => {
                if (myResult != null) {
                    if (!myResult.HasError) {
                        var count = myResult.Result;

                        if (count != 0) {
                            var window = new MessageWindow();
                            window.Width = 450;
                            window.Height = 180;
                            window.Title = TextCodeTranslator.Translate("Quote.B.CancelQuote");
                            window.Show(TextCodeTranslator.Translate("Quote.M.QuoteCancelMessage"));
                            this.isButtonClicked = false;
                        }

                        else {
                            var args = new QuoteEventNotesArgs();
                            args.EntityPM = this.EntityPM;
                            args.NotesHeader = TextCodeTranslator.Translate("Quote.F.Notes");

                            var logWindow = new LogitudeWindow();
                            logWindow.WindowArgs = args;
                            logWindow.Width = 450;
                            logWindow.Height = 300;
                            logWindow.Title = TextCodeTranslator.Translate("Quote.B.CancelQuote");
                            logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');

                            logWindow.WindowClosed.subscribe(s => {
                                if (s) {
                                    this.OnNotesWindowClosed("cancel");
                                }
                                else {
                                    this.isButtonClicked = false;
                                }
                            });
                        }
                    }
                }
            });
        }
    }

    private ReactivateQuote() {
        this.Validate();

        if (this.isValid) {
            var args = new QuoteEventNotesArgs();
            args.EntityPM = this.EntityPM;
            args.NotesHeader = TextCodeTranslator.Translate("Quote.F.Notes");

            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Width = 450;
            logWindow.Height = 300;
            logWindow.Title = TextCodeTranslator.Translate("Quote.B.ReactivateQuote");
            logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');

            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.OnNotesWindowClosed("reactivate");
                }
                else {
                    this.isButtonClicked = false;
                }
            });
        }
    }

    private ReturnToDraft() {
        this.Validate();

        if (this.isValid) {
            var quoteDomainService: QuoteDomainService = new QuoteDomainService();
            quoteDomainService.GetIsQuoteConnectedToShipment(this.EntityPM.Id).subscribe(resp => {
                if (!resp.HasError) {
                    var result: boolean = resp.Result;
             
                    if (result) {
                        var window = new MessageWindow();
                        window.Show(TextCodeTranslator.Translate("Quote.M.QuoteReturnMessage"));
                        this.isButtonClicked = false;
                    }
                    else {
                        var args = new QuoteEventNotesArgs();
                        args.EntityPM = this.EntityPM;
                        args.NotesHeader = TextCodeTranslator.Translate("Quote.F.Notes");

                        var logWindow = new LogitudeWindow();
                        logWindow.WindowArgs = args;
                        logWindow.Width = 450;
                        logWindow.Height = 300;
                        logWindow.Title = TextCodeTranslator.Translate("Quote.B.ReturnToDraft");
                        logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');

                        logWindow.WindowClosed.subscribe(s => {
                            if (s) {
                                this.OnNotesWindowClosed("draft");
                            }
                            else {
                                this.isButtonClicked = false;
                            }
                        });

                    }
                }
            });
        }
    }

    private SetAsAccepted() {
        this.Validate();

        if (this.isValid) {
            var args = new QuoteEventNotesArgs();
            args.EntityPM = this.EntityPM;
            args.NotesHeader = TextCodeTranslator.Translate("Quote.F.Notes");

            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Width = 450;
            logWindow.Height = 300;
            logWindow.Title = TextCodeTranslator.Translate("Quote.B.Accept");
            logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');

            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.OnNotesWindowClosed("accept");
                }
                else {
                    this.isButtonClicked = false;
                }
            });
        }
    }
    
    private SetAsDeclined() {
        this.Validate();

        if (this.isValid) {
            var args = new QuoteEventNotesArgs();
            args.EntityPM = this.EntityPM;
            args.NotesHeader = TextCodeTranslator.Translate("Quote.F.Notes");
            args.ShowClosingReason = true;

            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Width = 450;
            logWindow.Height = 300;
            logWindow.Title = TextCodeTranslator.Translate("Quote.B.Decline");
            logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');

            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.OnNotesWindowClosed("decline");
                }
                else {
                    this.isButtonClicked = false;
                }
            });
        }
    }

    private IsRunQuotation: boolean = false;
    private RunQuotationScreen() {

        if (this.EntityPM && this.EntityPM.IsDirty) {
            this.Validate();
            if (this.isValid) {
                this.IsRunQuotation = true;
                this.entityArgs.EditComponent.SaveChanges();
            }
        } else {
            this.OpenQuotationWindow();
        }

    }

    private OpenQuotationWindow() {
        var windowArgs: any = {};
        windowArgs.QuotePM = this.EntityPM;
       
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = window.innerWidth - 150;
        logWindow.Height = window.innerHeight - 150;
        logWindow.IsShowCloseButton = true;
        windowArgs.QuotationWindow = logWindow;
        logWindow.Title = TextCodeTranslator.Translate("Quote.B.Quotation");
        logWindow.Show('./QuoteModules/QuoteOthers/Components/Quotation/QuotationComponent');
 
        logWindow.WindowClosed.subscribe(s => {
            this.isButtonClicked = false;
        });
    }

    private OnNotesWindowClosed(actionType: string) {
        this.isButtonClicked = false;

        if (actionType == "sent") {
            this.EntityPM.IsClosed = false;
            this.EntityPM.ActionType = "SetAsSentToCustomer";
        }

        else if (actionType == "cancel") {
            this.EntityPM.IsCancelled = true;
            this.EntityPM.ActionType = "CancelQuote";
        }

        else if (actionType == "reactivate") {
            this.EntityPM.IsCancelled = false;
            this.EntityPM.ActionType = "ReactivateQuote";
        }

        else if (actionType == "draft") {
            this.EntityPM.IsClosed = false;
            this.EntityPM.ActionType = "ReturnInProgress";
        }

        else if (actionType == "accept") {
            this.EntityPM.IsClosed = true;
            this.EntityPM.ActionType = "Accept";
        }

        else if (actionType == "decline") {
            this.EntityPM.IsClosed = true;
            this.EntityPM.ActionType = "Decline";
        }

        this.entityArgs.EditComponent.SaveChanges();
    }
}
