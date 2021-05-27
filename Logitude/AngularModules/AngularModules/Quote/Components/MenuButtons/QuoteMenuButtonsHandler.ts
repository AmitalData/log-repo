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
import {AppTool, ArrayTool} from '../../../Infrastructure/Tools';
import {PartnersDomainService} from '../../../Common/Services/PartnersDomainService';
import {CustomerPM} from '../../../Common/EntityPMs/CustomerPM';
import {CustomerActivationArgs} from '../../../Common/Args';
import {ShipmentPM} from '../../../Shipment/EntityPMs/ShipmentPM';
import {NewShipmentComponentArgs} from '../../../Shipment/Args';
import {ShipmentDomainService} from '../../../Shipment/Services/ShipmentDomainService';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {QuoteDomainService} from '../../../Quote/Services/QuoteDomainService';

export class QuoteMenuButtonsHandler {
    public EntityPM: QuotePM;
    public entityArgs: EntityArgs
    private CurrentSession = SessionLocator.SelectedSession;
    private isLCL: boolean = false;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.myQuoteStageListService = new QuoteStageListService();
        this.myPartnersDomainService = new PartnersDomainService();
        this.entityResourceService = new EntityResourceService();
        this.Listen();
    }

    private allMenuButtons: MenuButtonPM[] = [];
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        this.allMenuButtons = menuButtons;

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

                        var isShowRoutingRatesQuotation: boolean = true;
                       

                        if ((this.EntityPM.QuoteTypeCode == "P" && isShowRoutingRatesQuotation) || this.EntityPM.QuoteTypeCode == "A") {
                            button.IsDisabled = false;
                        }
                        else {
                            button.IsDisabled = true;
                        }

                        if (this.EntityPM.IsQuoteDataExternal && this.EntityPM.IsQuoteDocumentExternal) {
                            button.IsDisabled = true;
                        }
                    }

                    if (button.EventCode == "ConvertQuotetoLCL") {
                        if (buttonEnabled) {
                            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.ShipmentTypeId == "FCLD") {
                                if (this.EntityPM.IsCancelled || this.EntityPM.IsClosed) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsHidden = false;
                                    button.IsDisabled = false;
                                }
                            }

                            else {
                                button.IsHidden = true;
                            }
                        }

                        else {
                            button.IsHidden = true;
                        }                        
                    }

                    if (button.EventCode == "ConvertQuotetoFCL") {
                        if (buttonEnabled) {
                            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.ShipmentTypeId == "LCLD") {
                                if (this.EntityPM.IsCancelled || this.EntityPM.IsClosed) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsHidden = false;
                                    button.IsDisabled = false;
                                }
                            }

                            else {
                                button.IsHidden = true;
                            }
                        }

                        else {
                            button.IsHidden = true;
                        }  
                    }

                    if (button.EventCode == "ConvertQuoteTransportMode") {
                        if (buttonEnabled) {
                            var myDraftStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTDR")[0];
                            var myCreatedStage: QuoteStageList = this.allStages.filter(d => d.Code == "QTCR")[0];

                            if (myDraftStage != null) {
                                myDraftStageId = myDraftStage.Id;
                            }

                            if (myCreatedStage != null) {
                                myCreatedStageId = myCreatedStage.Id;
                            }

                            if (this.EntityPM.StageId == myDraftStageId || this.EntityPM.StageId == myCreatedStageId) {
                                button.IsHidden = false;
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsHidden = true;
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsHidden = true;
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

                case "ConvertQuoteTransportMode": {
                    this.ConvertQuoteTransportModeClicked();
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
    private IsConvertQuoteTransportModeClicked: boolean = false;
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
            case "Transport":
                {
                    windowTitle = "Convert Quote Transport Mode";
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

    private ConvertQuoteTransportModeClicked() {
        this.Validate();
        if (this.isValid) {
            this.IsConvertQuoteTransportModeClicked = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
    }

    private DoConvertQuoteTransportMode() {
        var args = new NewQuoteComponentArgs();
        args.Quote = this.EntityPM;
        args.ConvertTransportMode = true;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = "Convert Quote Transport Mode";
        logWindow.Show('./Quote/Components/NewEntity/NewQuoteComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.StopFlags();
                this.entityArgs.EditComponent.ReloadEntityPM();
            }
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
        this.IsSetAsSentQuote = false;
        this.IsConvertQuoteTransportModeClicked = false;
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

                    if (this.IsConvertQuoteTransportModeClicked) {
                        this.DoConvertQuoteTransportMode();
                    }

                    if (this.IsSetAsSentQuote) {
                        this.StartSetAsSentToCustomer();
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

        this.CurrentSession.SessionEvent.subscribe((res) => {
            if (res == "QuantitiesUpdated") {
                //this.CheckButtonState(this.allMenuButtons);
            }
        });

        this.EntityPM.PropertyChanged.subscribe(s => {
            if (s) {
                if (s.PropertyName == "GrossWeight" || s.PropertyName == "ChargeableWeight" || s.PropertyName == "Volume" || s.PropertyName == "TEU"
                    || s.PropertyName == "NumberOfPackages" || s.PropertyName == "ValueOfGoods") {
                    this.CheckButtonState(this.allMenuButtons);
                }
            }
        });
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
            this.entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe((response:any) => {
                this.ConvertShipper();
            });
        }

        else if (this.EntityPM.IsPotentialConsignee) {
            this.entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe((response:any) => {
                this.ConvertConsignee();
            });
        }

        else {
            this.OpenNewShipmentComponent();
        }
    }
    private ConvertShipper() {
        this.myPartnersDomainService.GetCustomerById(this.EntityPM.ShipperId).subscribe((myResult:any) => {
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
        this.myPartnersDomainService.GetCustomerById(this.EntityPM.ConsigneeId).subscribe((myResult:any) => {
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
        this.entityResourceService.getEntityResourceByTableName("Shipment", 0).subscribe((response:any) => {
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

    IsSetAsSentQuote: boolean = false;
    private SetAsSentToCustomer() { 
        this.isLCL = QuoteUtilities.IsLCLQuote(this.EntityPM);
        this.CheckUpdateQuantities();

        if (this.IsUpdateQuantitiesVisible) {
            var messageWindow = new MessageWindow();
            messageWindow.Width = 400;
            messageWindow.Height = 150;
            messageWindow.Title = "Message";
            messageWindow.ShowErrorIcon = true;
            messageWindow.Show(this.UpdateQuantitiesMessage);
            messageWindow.WindowClosed.subscribe(s => {
                this.isButtonClicked = false;
            });
        }

        else {
            this.Validate();
            if (this.isValid) {
                this.IsSetAsSentQuote = true;
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    }
    StartSetAsSentToCustomer() {
        this.StopFlags();
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

    private CancelQuote() {
        this.Validate(); 
        if (this.isValid) {
            var myService: ShipmentDomainService = new ShipmentDomainService();
            myService.GetShipmentsCountByQuoteId(this.EntityPM.Id).subscribe((myResult: ServiceResponse) => {
                if (myResult != null) {
                    if (!myResult.HasError) {
                        var count = myResult.Result;

                        if (count != null && count != 0) {
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
            quoteDomainService.GetIsQuoteConnectedToShipment(this.EntityPM.Id).subscribe((resp:any) => {
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
        if (this.EntityPM.QuoteTypeCode == "A") {
            this.isLCL = QuoteUtilities.IsLCLQuote(this.EntityPM);
            this.CheckUpdateQuantities();
        }

        if (this.IsUpdateQuantitiesVisible) {
            var messageWindow = new MessageWindow();
            messageWindow.Width = 400;
            messageWindow.Height = 150;
            messageWindow.Title = "Message";
            messageWindow.ShowErrorIcon = true;
            messageWindow.Show(this.UpdateQuantitiesMessage);
            messageWindow.WindowClosed.subscribe(s => {
                this.isButtonClicked = false;
            });
        }

        else {
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
    }

    public UpdateQuantitiesMessage: string;
    public IsUpdateQuantitiesVisible: boolean = false;
    CheckUpdateQuantities() {
        var updateMessage = null;
        this.IsUpdateQuantitiesVisible = false;

        var isAdhoc = this.EntityPM.QuoteTypeCode == "A" ? true : false;

        if (isAdhoc) {
            if (this.isLCL) {
                if (this.EntityPM.QuoteCharges.filter(d => d.SaleUnitPrice != null || d.CostUnitPrice != null).length > 0) {

                    var list: any[] = [];
                    var entityQuantity: number = null;
                    var displayUpdateMessage: boolean = false;

                    //"GRWT"
                    entityQuantity = this.EntityPM.GrossWeight;
                    if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "GRWT" && f.CostQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }
                    else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "GRWT" && f.SaleQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }

                    //"GWTN"
                    entityQuantity = this.EntityPM.GrossWeightPerTon;
                    if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "GWTN" && f.CostQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }
                    else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "GWTN" && f.SaleQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }

                    //"CHWT"
                    entityQuantity = this.EntityPM.ChargeableWeight;
                    if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "CHWT" && f.CostQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }
                    else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "CHWT" && f.SaleQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }

                    //CWKG
                    entityQuantity = this.EntityPM.ChargeableWeightInKG;
                    if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "CWKG" && f.CostQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }
                    else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "CWKG" && f.SaleQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }

                    //GWKG
                    entityQuantity = this.EntityPM.GrossWeightInKG;
                    if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "GWKG" && f.CostQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }
                    else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "GWKG" && f.SaleQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }

                    //"VOLU"
                    entityQuantity = this.EntityPM.Volume;
                    if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "VOLU" && f.CostQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }
                    else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "VOLU" && f.SaleQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }

                    //"VCBM"
                    entityQuantity = this.EntityPM.VolumeInCBM;
                    if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "VCBM" && f.CostQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }
                    else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "VCBM" && f.SaleQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }

                    //"BTEU"
                    entityQuantity = this.EntityPM.TEU;
                    if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "BTEU" && f.CostQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }
                    else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "BTEU" && f.SaleQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }

                    //"QTY"
                    entityQuantity = this.EntityPM.NumberOfPackages;
                    if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "QTY" && f.CostQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }
                    else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "QTY" && f.SaleQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }

                    //"PRVL"
                    entityQuantity = this.EntityPM.ValueOfGoods;
                    if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PRVL" && f.CostQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }
                    else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "PRVL" && f.SaleQuantity != entityQuantity).length > 0) {
                        displayUpdateMessage = true;
                    }

                    //"PRFR"
                    if (this.EntityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT").length > 0) {
                        if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PRFR" || f.SaleMeasurementCode == "PRFR").length > 0) {

                            var FRT_CostQuantity = this.EntityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT")[0].CostTotalAmount;
                            var FRT_SaleQuantity = this.EntityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT")[0].SaleTotalAmount;

                            if (AppTool.IsNullOrZero(FRT_CostQuantity)) {
                                FRT_CostQuantity = 0;
                            }

                            if (AppTool.IsNullOrZero(FRT_SaleQuantity)) {
                                FRT_SaleQuantity = 0;
                            }

                            if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PRFR" && f.CostQuantity != null && f.CostQuantity != 0 && f.CostQuantity != FRT_CostQuantity).length > 0) {
                                displayUpdateMessage = true;
                            }

                            if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "PRFR" && f.SaleQuantity != null && f.SaleQuantity != 0 && f.SaleQuantity != FRT_SaleQuantity).length > 0) {
                                displayUpdateMessage = true;
                            }
                        }
                    }

                    if (displayUpdateMessage) {
                        updateMessage = "Please update charge screen by pressing on \"Update\" button first";
                    }
                }

                this.UpdateQuantitiesMessage = updateMessage;
                this.IsUpdateQuantitiesVisible = AppTool.IsNullOrEmpty(updateMessage) ? false : true;
            }

            else {
                var updateMessage = null;
                var entityQuantity: number = null;


                entityQuantity = this.EntityPM.TEU;
                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "BTEU" && f.CostQuantity != entityQuantity).length > 0) {
                    displayUpdateMessage = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "BTEU" && f.SaleQuantity != entityQuantity).length > 0) {
                    displayUpdateMessage = true;
                }

                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "QTY" && f.CostQuantity != this.EntityPM.NumberOfContainers).length > 0) {
                    displayUpdateMessage = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "QTY" && f.SaleQuantity != this.EntityPM.NumberOfContainers).length > 0) {
                    displayUpdateMessage = true;
                }

                var PFCL_CostQuantity = ArrayTool.Sum(this.EntityPM.QuoteCharges.filter(d => d.CostCurrencyId != SessionLocator.LocalCurrencyId && d.CostMeasurementCode != "PFCL"), "CostTotalAmountLocal");
                var PFCL_SaleQuantity = ArrayTool.Sum(this.EntityPM.QuoteCharges.filter(d => d.SaleCurrencyId != SessionLocator.LocalCurrencyId && d.SaleMeasurementCode != "PFCL"), "SaleTotalAmountLocal");;

                if (AppTool.IsNullOrZero(PFCL_CostQuantity)) {
                    PFCL_CostQuantity = 0;
                }

                if (AppTool.IsNullOrZero(PFCL_SaleQuantity)) {
                    PFCL_SaleQuantity = 0;
                }

                if (this.EntityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PFCL" && f.CostQuantity != null && f.CostQuantity != 0 && f.CostQuantity != PFCL_CostQuantity).length > 0) {
                    displayUpdateMessage = true;
                }

                else if (this.EntityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "PFCL" && f.SaleQuantity != null && f.SaleQuantity != 0 && f.SaleQuantity != PFCL_SaleQuantity).length > 0) {
                    displayUpdateMessage = true;
                }

                if (this.EntityPM.QuoteCharges.filter(d => d.SaleUnitPrice != null || d.CostUnitPrice != null).length > 0) {
                    if (this.EntityPM.QuoteCharges.filter(d => (d.CostMeasurementCode == "PRVL" && d.CostQuantity != this.EntityPM.ValueOfGoods) || (d.CostMeasurementCode == "PRVL" && d.CostQuantity != this.EntityPM.ValueOfGoods)).length > 0) {
                        updateMessage = "Please update charge screen by pressing on \"Update\" button first";
                    }

                    else if (this.EntityPM.QuoteCharges.filter(d => d.SaleMeasurementCode == "PRVL" && d.SaleQuantity != this.EntityPM.ValueOfGoods).length > 0) {
                        updateMessage = "Please update charge screen by pressing on \"Update\" button first";
                    }
                }

                if (displayUpdateMessage && AppTool.IsNullOrEmpty(updateMessage)) {
                    updateMessage = "Please update charge screen by pressing on \"Update\" button first";
                }

                this.UpdateQuantitiesMessage = updateMessage;
                this.IsUpdateQuantitiesVisible = AppTool.IsNullOrEmpty(updateMessage) ? false : true;
            }
        }
    }

    private OpenQuotationWindow() {
        var quoteCharges = this.EntityPM.QuoteCharges.filter(a => (!AppTool.IsNullOrZero(a.SaleAmountInSaleCurrency) || !AppTool.IsNullOrZero(a.CostAmountInSaleCurrency))
            && ((a.HasPickup && this.EntityPM.IncludePickUp == false) || (a.HasDelivery && this.EntityPM.IncludeDelivery == false)));

        if (quoteCharges != null && quoteCharges.length > 0) {
            var msg = "Can't have charges marked for pickup/delivery without having pickup/delivery defined in the quote";
            var msgwindow = new MessageWindow();
            msgwindow.Width = 400;
            msgwindow.Height = 150;
            msgwindow.ShowErrorIcon = true;
            this.isButtonClicked = false;
            msgwindow.Show(msg);
        }
        else {
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
