"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Args_1 = require("../../Args");
var QuoteUtilities_1 = require("../../Utilities/QuoteUtilities");
var QuoteStageListService_1 = require("../../Services/StandardLists/QuoteStageListService");
var QuoteValidator_1 = require("../../Validators/QuoteValidator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var PartnersDomainService_1 = require("../../../Common/Services/PartnersDomainService");
var Args_2 = require("../../../Common/Args");
var Args_3 = require("../../../Shipment/Args");
var ShipmentDomainService_1 = require("../../../Shipment/Services/ShipmentDomainService");
var QuoteDomainService_1 = require("../../../Quote/Services/QuoteDomainService");
var QuoteMenuButtonsHandler = /** @class */ (function () {
    function QuoteMenuButtonsHandler() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsConvertToLCLClicked = false;
        this.IsConvertToFCLClicked = false;
        this.allStages = [];
        this.isValid = false;
        this.isButtonClicked = false;
        this.Reload = false;
        this.isBuildingShipment = false;
        this.isCopyingQuote = false;
        this.IsRunQuotation = false;
    }
    QuoteMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.myQuoteStageListService = new QuoteStageListService_1.QuoteStageListService();
        this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.Listen();
    };
    QuoteMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        var _this = this;
        if (this.EntityPM != null) {
            this.allStages = [];
            this.myQuoteStageListService.getAllFromCache().subscribe(function (resp) {
                if (!resp.HasError) {
                    _this.allStages = resp.Result;
                }
            });
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'Quote'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    if (button.EventCode == "BuildShipment") {
                        button.Width = 100;
                        var myDeclinedStageId;
                        var myDeclinedStage = this.allStages.filter(function (d) { return d.Code == "QTDC"; })[0];
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
                        var mySentStage = this.allStages.filter(function (d) { return d.Code == "QTST"; })[0];
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
                        var myDraftStage = this.allStages.filter(function (d) { return d.Code == "QTDR"; })[0];
                        var myCreatedStage = this.allStages.filter(function (d) { return d.Code == "QTCR"; })[0];
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
                        var mySentStage = this.allStages.filter(function (d) { return d.Code == "QTST"; })[0];
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
                        var isShowRoutingRatesQuotation = false;
                        var featureToggle = SessionLocator_1.SessionLocator.FeatureToggles.filter(function (d) { return d.ToggleCode == "QRR" && d.TenantNumber == SessionLocator_1.SessionLocator.Tenant; })[0];
                        if (featureToggle) {
                            isShowRoutingRatesQuotation = true;
                        }
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
                                button.IsHidden = true;
                            }
                        }
                    }
                }
                return menuButtons;
            }
        }
    };
    QuoteMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
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
    };
    QuoteMenuButtonsHandler.prototype.ConvertQuoteToLCLClicked = function () {
        var errors = [];
        this.Validate();
        if (this.isValid) {
            this.IsConvertToLCLClicked = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    QuoteMenuButtonsHandler.prototype.ConvertQuoteToFCLClicked = function () {
        var errors = [];
        this.Validate();
        if (this.isValid) {
            this.IsConvertToFCLClicked = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    QuoteMenuButtonsHandler.prototype.DoConvertQuoteType = function (type) {
        var _this = this;
        var args = new Args_1.QuoteEventNotesArgs();
        var windowTitle;
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
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                var notes = s.EventNote;
                if (d == "OK") {
                    _this.EntityPM.EventNote = notes;
                    switch (type) {
                        case "ToLCL":
                            {
                                _this.EntityPM.ConvertToLCL = true;
                                _this.EntityPM.ConvertToFCL = false;
                                break;
                            }
                        case "ToFCL":
                            {
                                _this.EntityPM.ConvertToLCL = false;
                                _this.EntityPM.ConvertToFCL = true;
                                break;
                            }
                    }
                    _this.Reload = true;
                    _this.entityArgs.EditComponent.SaveChanges();
                    _this.isButtonClicked = false;
                }
            });
        });
    };
    QuoteMenuButtonsHandler.prototype.StopFlags = function () {
        this.isButtonClicked = false;
        this.isBuildingShipment = false;
        this.isCopyingQuote = false;
        this.IsRunQuotation = false;
        this.Reload = false;
        this.IsConvertToLCLClicked = false;
        this.IsConvertToFCLClicked = false;
    };
    QuoteMenuButtonsHandler.prototype.Validate = function () {
        var validator = new QuoteValidator_1.QuoteValidator();
        var errors = validator.Validate(this.EntityPM);
        this.isValid = errors.length == 0 ? true : false;
        this.entityArgs.EditComponent.ValidationErrorsList = errors;
        if (!this.isValid) {
            this.StopFlags();
        }
    };
    QuoteMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                _this.isButtonClicked = false;
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.isBuildingShipment) {
                        _this.StartBuildingShipment();
                    }
                    if (_this.isCopyingQuote) {
                        _this.StartCopyQuote();
                    }
                    if (_this.IsRunQuotation) {
                        _this.OpenQuotationWindow();
                    }
                    if (_this.IsConvertToLCLClicked) {
                        _this.DoConvertQuoteType("ToLCL");
                    }
                    if (_this.IsConvertToFCLClicked) {
                        _this.DoConvertQuoteType("ToFCL");
                    }
                    if (_this.Reload) {
                        _this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                }
                _this.StopFlags();
            });
            this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                }
                _this.StopFlags();
            });
        }
    };
    QuoteMenuButtonsHandler.prototype.BuildShipment = function () {
        var _this = this;
        this.Validate();
        if (this.isValid) {
            if (this.EntityPM.QuoteTypeCode != "A") {
                var window = new MessageWindow_1.MessageWindow();
                window.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.M.BuildShipmentMessage"));
                this.isButtonClicked = false;
            }
            else {
                var list = this.allStages.filter(function (d) { return d.Id == _this.EntityPM.StageId; })[0];
                if (list != null) {
                    //list.Name == "Used" || 
                    if (list.Code == "QTAC") {
                        this.StartBuildingShipment();
                    }
                    else {
                        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                        logitudeWindow.Width = 350;
                        logitudeWindow.Height = 170;
                        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.BuildShipment");
                        logitudeWindow.Show('./Quote/Components/MenuButtons/ApproveBuildShipmentComponent');
                        logitudeWindow.ComponentLoaded.subscribe(function (comp) {
                            logitudeWindow.WindowClosed.subscribe(function (s) {
                                if (comp.Approving) {
                                    _this.isBuildingShipment = true;
                                    _this.SetAsAccepted();
                                }
                                else {
                                    _this.isButtonClicked = false;
                                }
                            });
                        });
                    }
                }
            }
        }
    };
    QuoteMenuButtonsHandler.prototype.StartBuildingShipment = function () {
        var _this = this;
        if (this.EntityPM.IsPotentialShipper) {
            this.entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(function (response) {
                _this.ConvertShipper();
            });
        }
        else if (this.EntityPM.IsPotentialConsignee) {
            this.entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(function (response) {
                _this.ConvertConsignee();
            });
        }
        else {
            this.OpenNewShipmentComponent();
        }
    };
    QuoteMenuButtonsHandler.prototype.ConvertShipper = function () {
        var _this = this;
        this.myPartnersDomainService.GetCustomerById(this.EntityPM.ShipperId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var shipper = myResponse.Result;
                if (shipper != null) {
                    var args = new Args_2.CustomerActivationArgs();
                    args.EntityPM = shipper;
                    args.ActivatedFromQuoteSide = true;
                    args.ActivatedPartnerType = "SH";
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.WindowArgs = args;
                    logWindow.Title = "Shipper Activation";
                    logWindow.Show('./CommonModules/CommonCustomer/Components/CustomerActivation/CustomerActivationComponent');
                    logWindow.WindowClosed.subscribe(function (s) {
                        _this.isButtonClicked = false;
                        if (s) {
                            _this.entityArgs.EditComponent.ReloadEntityPM();
                            if (_this.EntityPM.IsPotentialConsignee) {
                                _this.ConvertConsignee();
                            }
                            else {
                                _this.OpenNewShipmentComponent();
                            }
                        }
                    });
                }
            }
        });
    };
    QuoteMenuButtonsHandler.prototype.ConvertConsignee = function () {
        var _this = this;
        this.myPartnersDomainService.GetCustomerById(this.EntityPM.ConsigneeId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var consignee = myResponse.Result;
                if (consignee != null) {
                    var args = new Args_2.CustomerActivationArgs();
                    args.EntityPM = consignee;
                    args.ActivatedFromQuoteSide = true;
                    args.ActivatedPartnerType = "CO";
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.WindowArgs = args;
                    logWindow.Title = "Consignee Activation";
                    logWindow.Show('./CommonModules/CommonCustomer/Components/CustomerActivation/CustomerActivationComponent');
                    logWindow.WindowClosed.subscribe(function (s) {
                        _this.isButtonClicked = false;
                        if (s) {
                            _this.entityArgs.EditComponent.ReloadEntityPM();
                            _this.OpenNewShipmentComponent();
                        }
                    });
                }
            }
        });
    };
    QuoteMenuButtonsHandler.prototype.OpenNewShipmentComponent = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("Shipment", 0).subscribe(function (response) {
            var shipmentPM = QuoteUtilities_1.QuoteUtilities.BuildShipment(_this.EntityPM);
            var args = new Args_3.NewShipmentComponentArgs();
            args.Shipment = shipmentPM;
            args.IsBuildFromQuote = true;
            var str = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
            str = str.replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable("Shipment"));
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.WindowArgs = args;
            logWindow.Title = str;
            logWindow.Show('./Shipment/Components/NewShipment/NewShipmentComponent');
            logWindow.ComponentLoaded.subscribe(function (cmp) {
                cmp.ShowShipmentLevels = true;
                _this.isButtonClicked = false;
            });
            //logWindow.WindowClosed.subscribe(s => {
            //    this.isButtonClicked = false;
            //});
        });
    };
    QuoteMenuButtonsHandler.prototype.CopyQuote = function () {
        this.Validate();
        if (this.isValid) {
            this.isCopyingQuote = true;
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    QuoteMenuButtonsHandler.prototype.StartCopyQuote = function () {
        var _this = this;
        var args = new Args_1.NewQuoteComponentArgs();
        args.Quote = this.EntityPM;
        args.IsCopyFromQuote = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.CopyQuote");
        logWindow.Show('./Quote/Components/NewEntity/NewQuoteComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            _this.StopFlags();
        });
    };
    QuoteMenuButtonsHandler.prototype.SetAsSentToCustomer = function () {
        var _this = this;
        this.Validate();
        if (this.isValid) {
            var args = new Args_1.QuoteEventNotesArgs();
            args.EntityPM = this.EntityPM;
            args.NotesHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.Notes");
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Width = 450;
            logWindow.Height = 300;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.SetAsSent");
            logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.OnNotesWindowClosed("sent");
                }
                else {
                    _this.isButtonClicked = false;
                }
            });
        }
    };
    QuoteMenuButtonsHandler.prototype.CancelQuote = function () {
        var _this = this;
        this.Validate();
        if (this.isValid) {
            var myService = new ShipmentDomainService_1.ShipmentDomainService();
            myService.GetShipmentsCountByQuoteId(this.EntityPM.Id).subscribe(function (myResult) {
                if (myResult != null) {
                    if (!myResult.HasError) {
                        var count = myResult.Result;
                        if (count != 0) {
                            var window = new MessageWindow_1.MessageWindow();
                            window.Width = 450;
                            window.Height = 180;
                            window.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.CancelQuote");
                            window.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.M.QuoteCancelMessage"));
                            _this.isButtonClicked = false;
                        }
                        else {
                            var args = new Args_1.QuoteEventNotesArgs();
                            args.EntityPM = _this.EntityPM;
                            args.NotesHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.Notes");
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.WindowArgs = args;
                            logWindow.Width = 450;
                            logWindow.Height = 300;
                            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.CancelQuote");
                            logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');
                            logWindow.WindowClosed.subscribe(function (s) {
                                if (s) {
                                    _this.OnNotesWindowClosed("cancel");
                                }
                                else {
                                    _this.isButtonClicked = false;
                                }
                            });
                        }
                    }
                }
            });
        }
    };
    QuoteMenuButtonsHandler.prototype.ReactivateQuote = function () {
        var _this = this;
        this.Validate();
        if (this.isValid) {
            var args = new Args_1.QuoteEventNotesArgs();
            args.EntityPM = this.EntityPM;
            args.NotesHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.Notes");
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Width = 450;
            logWindow.Height = 300;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.ReactivateQuote");
            logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.OnNotesWindowClosed("reactivate");
                }
                else {
                    _this.isButtonClicked = false;
                }
            });
        }
    };
    QuoteMenuButtonsHandler.prototype.ReturnToDraft = function () {
        var _this = this;
        this.Validate();
        if (this.isValid) {
            var quoteDomainService = new QuoteDomainService_1.QuoteDomainService();
            quoteDomainService.GetIsQuoteConnectedToShipment(this.EntityPM.Id).subscribe(function (resp) {
                if (!resp.HasError) {
                    var result = resp.Result;
                    if (result) {
                        var window = new MessageWindow_1.MessageWindow();
                        window.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.M.QuoteReturnMessage"));
                        _this.isButtonClicked = false;
                    }
                    else {
                        var args = new Args_1.QuoteEventNotesArgs();
                        args.EntityPM = _this.EntityPM;
                        args.NotesHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.Notes");
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.WindowArgs = args;
                        logWindow.Width = 450;
                        logWindow.Height = 300;
                        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.ReturnToDraft");
                        logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');
                        logWindow.WindowClosed.subscribe(function (s) {
                            if (s) {
                                _this.OnNotesWindowClosed("draft");
                            }
                            else {
                                _this.isButtonClicked = false;
                            }
                        });
                    }
                }
            });
        }
    };
    QuoteMenuButtonsHandler.prototype.SetAsAccepted = function () {
        var _this = this;
        this.Validate();
        if (this.isValid) {
            var args = new Args_1.QuoteEventNotesArgs();
            args.EntityPM = this.EntityPM;
            args.NotesHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.Notes");
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Width = 450;
            logWindow.Height = 300;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.Accept");
            logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.OnNotesWindowClosed("accept");
                }
                else {
                    _this.isButtonClicked = false;
                }
            });
        }
    };
    QuoteMenuButtonsHandler.prototype.SetAsDeclined = function () {
        var _this = this;
        this.Validate();
        if (this.isValid) {
            var args = new Args_1.QuoteEventNotesArgs();
            args.EntityPM = this.EntityPM;
            args.NotesHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.Notes");
            args.ShowClosingReason = true;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Width = 450;
            logWindow.Height = 300;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.Decline");
            logWindow.Show('./Quote/Components/MenuButtons/QuoteEventNotesComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.OnNotesWindowClosed("decline");
                }
                else {
                    _this.isButtonClicked = false;
                }
            });
        }
    };
    QuoteMenuButtonsHandler.prototype.RunQuotationScreen = function () {
        if (this.EntityPM && this.EntityPM.IsDirty) {
            this.Validate();
            if (this.isValid) {
                this.IsRunQuotation = true;
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
        else {
            this.OpenQuotationWindow();
        }
    };
    QuoteMenuButtonsHandler.prototype.OpenQuotationWindow = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.QuotePM = this.EntityPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = window.innerWidth - 150;
        logWindow.Height = window.innerHeight - 150;
        logWindow.IsShowCloseButton = true;
        windowArgs.QuotationWindow = logWindow;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.Quotation");
        logWindow.Show('./QuoteModules/QuoteOthers/Components/Quotation/QuotationComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            _this.isButtonClicked = false;
        });
    };
    QuoteMenuButtonsHandler.prototype.OnNotesWindowClosed = function (actionType) {
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
    };
    return QuoteMenuButtonsHandler;
}());
exports.QuoteMenuButtonsHandler = QuoteMenuButtonsHandler;
//# sourceMappingURL=QuoteMenuButtonsHandler.js.map