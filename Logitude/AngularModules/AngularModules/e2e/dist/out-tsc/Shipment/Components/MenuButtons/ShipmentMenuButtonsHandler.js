"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Args_1 = require("../../Args");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var EntityWarningsValidator_1 = require("../../../Infrastructure/Validators/EntityWarningsValidator");
var RulesValidator_1 = require("../../../Infrastructure/Validators/RulesValidator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ShipmentDomainService_1 = require("../../Services/ShipmentDomainService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MenuButtonsTemplateComponent_1 = require("./MenuButtonsTemplateComponent");
var Tools_1 = require("../../Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ShipmentValidator_1 = require("../../Validators/ShipmentValidator");
var Tools_2 = require("../../../Infrastructure/Tools");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var ShipmenDirectionConvertComponent_1 = require("./ShipmenDirectionConvertComponent");
var ShipmentMenuButtonsHandler = /** @class */ (function () {
    function ShipmentMenuButtonsHandler() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.isValid = false;
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
        this.Reload = false;
        this.ActionStepsStateList = [];
        this.shipmentService = new ShipmentDomainService_1.ShipmentDomainService();
        this.hasWarnings = false;
        this.currentActionName = null;
        this.IsConvertToLCLClicked = false;
        this.IsConvertToFCLClicked = false;
        this.IsConvertDirectionClicked = false;
    }
    ShipmentMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    ShipmentMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'Shipment'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    if (button.EventCode == "ShowAWB") {
                        button.IsDisabled = buttonEnabled ? (this.EntityPM.TransportModeId != "A") : true;
                    }
                    if (button.EventCode == "CopyShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentLevelCode == "C") {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "OperationalCloseShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsOperationalClosed || this.EntityPM.IsCancelled || this.EntityPM.ShipmentLevelCode == "H") {
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
                    if (button.EventCode == "AccountingCloseShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsAccountingClosed || !this.EntityPM.IsOperationalClosed || this.EntityPM.IsCancelled || this.EntityPM.ShipmentLevelCode == "H") {
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
                    if (button.EventCode == "OperationalReopenShipment") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.IsOperationalClosed || this.EntityPM.IsAccountingClosed || this.EntityPM.IsCancelled || this.EntityPM.ShipmentLevelCode == "H") {
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
                    if (button.EventCode == "AccountedReopenShipment") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.IsAccountingClosed || this.EntityPM.IsCancelled || this.EntityPM.ShipmentLevelCode == "H") {
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
                    if (button.EventCode == "CancelShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsAccountingClosed || this.EntityPM.IsOperationalClosed || this.EntityPM.IsCancelled || (this.EntityPM.MasterShipmentDataId != null && this.EntityPM.MasterShipmentDataId != this.EntityPM.Id)) {
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
                    if (button.EventCode == "ReactivateShipment") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.IsCancelled) {
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
                    if (button.EventCode == "ExceptionResolved") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.HasException) {
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
                    if (button.EventCode == "ConvertShipmentFromHouseToDirect") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.MasterShipmentDataId == null) {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "ConvertShipmentFromDirectToHouse") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentLevelCode == "D" && !this.EntityPM.IsOperationalClosed) {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "SendRequest") {
                        if (buttonEnabled) {
                            if (this.EntityPM.TransportModeId == "A" && this.EntityPM.DirectionId == "E") {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "SendResponse") {
                        if (buttonEnabled) {
                            button.IsDisabled = false;
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "ConvertToCustomFile") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentLevelCode != "D") {
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
                    if (button.EventCode == "SplitShipment") {
                        if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                            button.IsHidden = false;
                            button.IsDisabled = !buttonEnabled;
                        }
                        else {
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "ConvertShipmentToLCL") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsCancelled) {
                                button.IsDisabled = true;
                            }
                            else {
                                if (this.EntityPM.ShipmentTypeId == "FCLD") {
                                    button.IsHidden = false;
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsHidden = true;
                                }
                            }
                        }
                        else {
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "ConvertShipmentToFCL") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsCancelled) {
                                button.IsDisabled = true;
                            }
                            else {
                                if (this.EntityPM.ShipmentTypeId == "LCLD") {
                                    button.IsHidden = false;
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsHidden = true;
                                }
                            }
                        }
                        else {
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "ConvertShipmentDirection") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsCancelled) {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsHidden = false;
                                button.IsDisabled = false;
                            }
                            button.IsHidden = false;
                        }
                        else {
                            button.IsHidden = true;
                        }
                    }
                }
                return menuButtons;
            }
        }
    };
    ShipmentMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (!this.isButtonClicked) {
            this.StopFlags();
            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;
            this.Validate();
            if (this.isValid) {
                switch (this.MenuButtonCode) {
                    case "ShowEvents": {
                        this.ShowEvents();
                        break;
                    }
                    case "CopyShipment": {
                        this.entityArgs.EditComponent.SaveChanges();
                        break;
                    }
                    case "OperationalCloseShipment": {
                        this.OperationalCloseShipment();
                        break;
                    }
                    case "OperationalReopenShipment": {
                        this.OperationalReopenShipment();
                        break;
                    }
                    case "AccountingCloseShipment": {
                        this.AccountingCloseShipment();
                        break;
                    }
                    case "AccountedReopenShipment": {
                        this.AccountedReopenShipment();
                        break;
                    }
                    case "CancelShipment": {
                        this.CancelShipment();
                        break;
                    }
                    case "ReactivateShipment": {
                        this.ReactivateShipment();
                        break;
                    }
                    case "ExceptionResolved": {
                        this.ExceptionResolvedShipment();
                        break;
                    }
                    case "ConvertShipmentFromHouseToDirect": {
                        this.ConvertShipmentFromHouseToDirect();
                        break;
                    }
                    case "ConvertShipmentFromDirectToHouse": {
                        this.ConvertShipmentFromDirectToHouse();
                        break;
                    }
                    case "SendRequest": {
                        this.StopFlags();
                        break;
                    }
                    case "SendResponse": {
                        this.StopFlags();
                        break;
                    }
                    case "ConvertToCustomFile": {
                        this.ConvertShipmentToCustomFile();
                        break;
                    }
                    case "SplitShipment": {
                        this.SplitShipmentClicked();
                        break;
                    }
                    case "ConvertShipmentToLCL": {
                        this.ConvertShipmentToLCLClicked();
                        break;
                    }
                    case "ConvertShipmentToFCL": {
                        this.ConvertShipmentToFCLClicked();
                        break;
                    }
                    case "ConvertShipmentDirection": {
                        this.ConvertShipmentDirectionClicked();
                        break;
                    }
                    default: {
                        this.isButtonClicked = false;
                        break;
                    }
                }
            }
        }
    };
    ShipmentMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                        switch (_this.MenuButtonCode) {
                            case "CopyShipment": {
                                _this.CopyShipment();
                                break;
                            }
                            case "SplitShipment": {
                                _this.SplitShipmentApply();
                                break;
                            }
                        }
                        if (_this.IsConvertToLCLClicked) {
                            _this.DoConvertShipmentType("ToLCL");
                        }
                        if (_this.IsConvertToFCLClicked) {
                            _this.DoConvertShipmentType("ToFCL");
                        }
                        if (_this.IsConvertDirectionClicked) {
                            _this.DoConvertShipmentDirection();
                        }
                        if (_this.Reload) {
                            _this.entityArgs.EditComponent.ReloadEntityPM();
                        }
                    }
                    _this.StopFlags();
                });
            }
            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                        _this.CurrentSession.SessionEvent.emit("ReloadHouses");
                    }
                    _this.StopFlags();
                });
            }
        }
    };
    ShipmentMenuButtonsHandler.prototype.ngOnDestroy = function () {
        Tools_2.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_2.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ShipmentMenuButtonsHandler.prototype.StopFlags = function () {
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
        this.Reload = false;
        this.IsConvertToLCLClicked = false;
        this.IsConvertToFCLClicked = false;
        this.IsConvertDirectionClicked = false;
    };
    ShipmentMenuButtonsHandler.prototype.Validate = function () {
        var validator = new ShipmentValidator_1.ShipmentValidator();
        var errors = validator.Validate(this.EntityPM);
        this.isValid = errors.length == 0 ? true : false;
        this.entityArgs.EditComponent.ValidationErrorsList = errors;
        if (!this.isValid) {
            this.StopFlags();
        }
    };
    Object.defineProperty(ShipmentMenuButtonsHandler.prototype, "WarningList", {
        get: function () { if (this.warningsList == null)
            this.warningsList = new Array(); return this.warningsList; },
        set: function (value) { this.warningsList = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentMenuButtonsHandler.prototype, "HasWarnings", {
        get: function () { return this.hasWarnings; },
        set: function (value) { this.hasWarnings = value; },
        enumerable: true,
        configurable: true
    });
    ShipmentMenuButtonsHandler.prototype.CopyShipment = function () {
        var _this = this;
        var args = new Args_1.NewShipmentComponentArgs();
        args.IsCopyFromShipment = true;
        args.Shipment = this.EntityPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;
        if (this.EntityPM.ShipmentLevelCode == "C") {
            logWindow.Title = "Copy Master";
            logWindow.Show('./Shipment/Components/NewEntity/NewMasterComponent');
        }
        else {
            logWindow.Title = "Copy Shipment";
            logWindow.Show('./Shipment/Components/NewEntity/NewShipmentComponent');
        }
        logWindow.ComponentLoaded.subscribe(function (cmp) {
            _this.StopFlags();
            cmp.MainCarriageFromPortId = _this.EntityPM.MainCarriageFromPortId;
            cmp.MainCarriageToPortId = _this.EntityPM.MainCarriageToPortId;
        });
    };
    ShipmentMenuButtonsHandler.prototype.AccountedReopenShipment = function () {
        var _this = this;
        this.currentActionName = "AccountedReopen";
        this.ActionStepsStateList = new Array();
        var args = new MenuButtonsTemplateComponent_1.MenuButtonsTemplateArgs();
        args.IsNotesStackPanelVisible = true;
        args.NotesHeader = "Shipment Accounting Reopen Notes";
        var state = new ActionsStepsState();
        state.Message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.ShipmentAccountingReopened");
        this.ActionStepsStateList.push(state);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Accounted Shipment Reopen";
        logWindow.WindowArgs = args;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(function (cmp) {
            cmp.ReopenDone.subscribe(function (p) {
                _this.EntityPM.EventNote = p;
            });
        });
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.ResetButtonClicked();
            if ($event == "confirm") {
                _this.EntityPM.IsAccountingClosed = false;
                _this.OkButton();
            }
            else {
            }
        });
    };
    ShipmentMenuButtonsHandler.prototype.AccountingCloseShipment = function () {
        this.currentActionName = "AccountingClose";
        var hasOpenPayables = false;
        var hasOpenReceivables = false;
        if (this.EntityPM.ShipmentReceivables.length > 0) {
            this.EntityPM.ShipmentReceivables.forEach(function (p) {
                if (p.ShipmentReceivableLineStatusCode != "ACCT" && p.ShipmentReceivableLineStatusCode != "EMPT")
                    if (p.TotalAmount != null && p.TotalAmount != 0)
                        hasOpenReceivables = true;
            });
        }
        if (!SessionLocator_1.SessionLocator.AccountingSettingPM.AllowClosureWithoutPayables) {
            if (this.EntityPM.ShipmentPayables.length > 0)
                this.EntityPM.ShipmentPayables.forEach(function (p) {
                    if (p.ShipmentPayableLineStatusCode != "ACCT" && p.ShipmentPayableLineStatusCode != "EMPT")
                        if (p.ShipmentPayableAmountTypeCode == "NEXP") {
                            p.AccountedAmount != null && p.AccountedAmount != null ? hasOpenPayables = true : p.ExpectedAmount != null && p.ExpectedAmount != 0 ? hasOpenPayables = true : -1;
                        }
                        else {
                            if (p.ExpectedAmount != null && p.ExpectedAmount != 0) {
                                hasOpenPayables = true;
                            }
                        }
                });
        }
        if (this.EntityPM.ShipmentLevelCode == "C") {
            if (hasOpenPayables && hasOpenReceivables) {
                this.RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables);
            }
            else {
                this.shipmentService.CheckHousesOpenAmounts(this.EntityPM).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (myResponse.Result != null && myResponse.Result != "") {
                            var Result = myResponse.Result;
                            if (Result.includes('R'))
                                hasOpenReceivables = true;
                            if (SessionLocator_1.SessionLocator.AccountingSettingPM.AllowClosureWithoutPayables) {
                                if (Result.includes('P'))
                                    hasOpenPayables = true;
                            }
                        }
                    }
                });
                this.RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables);
            }
        }
        else {
            this.RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables);
        }
    };
    ShipmentMenuButtonsHandler.prototype.RunAccountingCloseWindow = function (hasOpenPayables, hasOpenReceivables) {
        var _this = this;
        this.currentActionName = "OperationalReopen";
        this.ActionStepsStateList = new Array();
        var args = new MenuButtonsTemplateComponent_1.MenuButtonsTemplateArgs();
        args.IsNotesStackPanelVisible = false;
        args.EventNote = null;
        var ErrorsList = [];
        var success = true;
        if (hasOpenPayables || hasOpenReceivables) {
            success = false;
            var error = "can’t close for accounting if there are any open payables/receivables";
            if (this.EntityPM.ShipmentLevelCode == "C") {
                error = "can’t close for accounting if there are any open payables/receivables in the Master or one \nof the connected shipments. Please check and fix this issue and try again";
            }
            ErrorsList.push(error);
        }
        else if (!this.EntityPM.IsOperationalClosed) {
            success = false;
        }
        var state = new ActionsStepsState();
        state.Message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.CheckingRequiredFields");
        success == true ? state.State = "Succeeded" : state.State = "Failed";
        this.ActionStepsStateList.push(state);
        var hasInvoice = null;
        hasInvoice = this.EntityPM.ShipmentARInvoices.filter(function (p) { return p.StatusCode != 'AC'; })[0];
        if (!hasInvoice) {
            state = new ActionsStepsState();
            state.Message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DoesntContainInvoice");
            state.State = "Warning";
            this.ActionStepsStateList.push(state);
        }
        if (!success)
            args.EnabledOkButton = false;
        args.ValidationErrorsList = ErrorsList;
        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Shipment Accounting Close";
        logWindow.WindowArgs = args;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.ResetButtonClicked();
            if ($event == "confirm") {
                _this.EntityPM.IsAccountingClosed = true;
                _this.EntityPM.EventNote = null;
                _this.EntityPM.AccountingCloseDate = Tools_2.DateTool.GetCurrentDateTimeAsUtc();
                _this.OkButton();
            }
            else {
            }
        });
    };
    ShipmentMenuButtonsHandler.prototype.OperationalReopenShipment = function () {
        var _this = this;
        this.currentActionName = "OperationalReopen";
        this.ActionStepsStateList = new Array();
        var args = new MenuButtonsTemplateComponent_1.MenuButtonsTemplateArgs();
        args.IsNotesStackPanelVisible = true;
        args.NotesHeader = "Shipment Operational Reopen Notes";
        var state = new ActionsStepsState();
        state.Message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.ShipmentOperationalReopened");
        this.ActionStepsStateList.push(state);
        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Shipment Operational Reopen";
        logWindow.WindowArgs = args;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(function (cmp) {
            cmp.ReopenDone.subscribe(function (p) {
                _this.EntityPM.EventNote = p;
                _this.EntityPM.IsOperationalClosed = false;
                _this.EntityPM.OperationalCloseDate = null;
                _this.OkButton();
            });
        });
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.ResetButtonClicked();
        });
    };
    ShipmentMenuButtonsHandler.prototype.ShowEvents = function () {
    };
    ShipmentMenuButtonsHandler.prototype.ExceptionResolvedShipment = function () {
        var _this = this;
        this.currentActionName = "ExceptionResolved";
        var args = new MenuButtonsTemplateComponent_1.MenuButtonsTemplateArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Exception Resolved Notes";
        args.IsNotesStackPanelVisible = true;
        args.EventNote = "";
        this.ActionStepsStateList = new Array();
        var state2 = new ActionsStepsState();
        state2.Message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.ExceptionResolved");
        this.ActionStepsStateList.push(state2);
        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;
        logWindow.Title = "Exception Resolved";
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    if (!Tools_2.AppTool.IsNullOrEmpty(notes))
                        _this.EntityPM.EventNote = notes;
                    _this.EntityPM.IsExceptionResolved = true;
                    _this.EntityPM.HasException = false;
                    _this.OkButton();
                }
                _this.ResetButtonClicked();
            });
        });
    };
    ShipmentMenuButtonsHandler.prototype.ConvertShipmentFromHouseToDirect = function () {
        var _this = this;
        this.currentActionName = "ConvertShipmentFromHouseToDirect";
        var args = new MenuButtonsTemplateComponent_1.MenuButtonsTemplateArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Convert Shipment From House To Direct...";
        args.IsNotesStackPanelVisible = false;
        args.EventNote = null;
        this.ActionStepsStateList = new Array();
        var state = new ActionsStepsState();
        state.Message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.ShipmentConvertedHtoD");
        this.ActionStepsStateList.push(state);
        if (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "D" || this.EntityPM.DirectionId == "R") {
            var settingCode = "HAWBCounter" + this.EntityPM.TransportModeId + "_E_D";
            var tenantSettingPM = SessionLocator_1.SessionLocator.TenantSettings.filter(function (p) { return p.SettingCode == settingCode; })[0];
            if (tenantSettingPM != null) {
                if (tenantSettingPM.DontIncludeDirects) {
                    state = new ActionsStepsState();
                    state.Message = "The HAWB field will be removed";
                    state.State = "Warning";
                    this.ActionStepsStateList.push(state);
                }
            }
        }
        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 970;
        logWindow.Height = 570;
        logWindow.Title = "Convert Shipment From House To Direct";
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    _this.EntityPM.ConvertFromHouseToDirect = true;
                    _this.EntityPM.ConvertFromDirectToHouse = false;
                    _this.OkButton();
                }
                _this.ResetButtonClicked();
            });
        });
    };
    ShipmentMenuButtonsHandler.prototype.ConvertShipmentFromDirectToHouse = function () {
        var _this = this;
        if (this.EntityPM != null) {
            if (Tools_1.ShipmentTool.IsInlandDomestic(this.EntityPM)) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Title = "Converting Shipment";
                messageWindow.Show("Converting inland domestic house to direct is not allowed");
            }
            else {
                this.currentActionName = "ConvertShipmentFromDirectToHouse";
                var args = new MenuButtonsTemplateComponent_1.MenuButtonsTemplateArgs();
                args.ObjectTableName = "Shipment";
                args.EntityPM = this.EntityPM;
                args.NotesHeader = "Convert Shipment From Direct To House...";
                args.IsNotesStackPanelVisible = false;
                args.EventNote = null;
                this.ActionStepsStateList = new Array();
                var state = new ActionsStepsState();
                state.Message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.ShipmentConvertedDToH");
                this.ActionStepsStateList.push(state);
                if (this.EntityPM.MainCarriageIsFromStack) {
                    state = new ActionsStepsState();
                    state.Message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.MasterAWBNumberTakenFromStack");
                    state.State = "Error";
                    this.ActionStepsStateList.push(state);
                    args.EnabledOkButton = false;
                }
                args.ActionStepsStateList = this.ActionStepsStateList;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.WindowArgs = args;
                logWindow.Width = 935;
                logWindow.Height = 570;
                logWindow.Title = "Convert Shipment From Direct To House";
                logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
                logWindow.ComponentLoaded.subscribe(function (s) {
                    logWindow.WindowClosed.subscribe(function (d) {
                        if (d == "confirm") {
                            if (!_this.EntityPM.MainCarriageIsFromStack) {
                                _this.EntityPM.ConvertFromDirectToHouse = true;
                                _this.EntityPM.ConvertFromHouseToDirect = false;
                            }
                            _this.OkButton();
                        }
                        _this.ResetButtonClicked();
                    });
                });
            }
        }
    };
    ShipmentMenuButtonsHandler.prototype.ConvertShipmentToCustomFile = function () {
        this.currentActionName = "ConvertToCustomFile";
        this.EntityPM.ShipmentLevelCode = "A";
        this.EntityPM.DirectionId = "C";
        this.EntityPM.ConvertToCustomFile = true;
        this.entityArgs.EditComponent.SaveChanges();
        this.MenuButtonCode = null;
    };
    ShipmentMenuButtonsHandler.prototype.ReactivateShipment = function () {
        var _this = this;
        this.currentActionName = "ReactivateShipment";
        var args = new MenuButtonsTemplateComponent_1.MenuButtonsTemplateArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Shipment Reactivation Notes";
        args.IsNotesStackPanelVisible = true;
        args.EventNote = "";
        this.ActionStepsStateList = new Array();
        var state2 = new ActionsStepsState();
        state2.Message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.ShipmentReactivated");
        this.ActionStepsStateList.push(state2);
        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;
        logWindow.Title = "Reactivate Shipment";
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    if (!Tools_2.AppTool.IsNullOrEmpty(notes))
                        _this.EntityPM.EventNote = notes;
                    _this.EntityPM.IsCancelled = false;
                    _this.OkButton();
                }
                _this.ResetButtonClicked();
            });
        });
    };
    ShipmentMenuButtonsHandler.prototype.CancelShipment = function () {
        var _this = this;
        this.currentActionName = "CancelShipment";
        if (this.EntityPM.ShipmentReceivables.filter(function (p) { return p.ARInvoiceId != null; })[0]) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Title = "Cancelling Shipment";
            messageWindow.Show("This shipment can't be canceled because it has one or more invoices. all invoices must be disconnect to cancel this shipment");
        }
        else if (this.EntityPM.BookingId != null && this.EntityPM.BookingId != "") {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Title = "Cancel Shipment";
            confirmWindow.Width = 400;
            confirmWindow.Show("Cancelling this shipment will disconnect it from the Booking , are you sure you want to cancel ?");
            confirmWindow.YesButtonText = "Yes";
            confirmWindow.NoButtonText = "No";
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    // console.log("Yes");
                    _this.ConfirmCanceling();
                }
                else if (confirmWindow.No) {
                    // console.log("No");
                }
                _this.ResetButtonClicked();
            });
        }
        else
            this.ConfirmCanceling();
    };
    ShipmentMenuButtonsHandler.prototype.ResetButtonClicked = function () {
        this.isButtonClicked = false;
    };
    ShipmentMenuButtonsHandler.prototype.ConfirmCanceling = function () {
        var _this = this;
        var args = new MenuButtonsTemplateComponent_1.MenuButtonsTemplateArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Shipment Cancel Notes";
        args.IsNotesStackPanelVisible = true;
        args.EventNote = "";
        this.ActionStepsStateList = new Array();
        var state2 = new ActionsStepsState();
        state2.Message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.ShipmentCancelled");
        this.ActionStepsStateList.push(state2);
        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;
        logWindow.Title = "Cancel Shipment";
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    if (!Tools_2.AppTool.IsNullOrEmpty(notes))
                        _this.EntityPM.EventNote = notes;
                    _this.EntityPM.IsCancelled = true;
                    if (_this.EntityPM.MainCarriageIsFromStack || _this.EntityPM.MAWBTakenFromStack) {
                        _this.EntityPM.MAWBReturnedToStack = true;
                        _this.EntityPM.MAWBReturnedToStackWithCancel = true;
                        _this.EntityPM.MAWBStackNumber = _this.EntityPM.Master;
                    }
                    _this.OkButton();
                }
                _this.ResetButtonClicked();
            });
        });
    };
    ShipmentMenuButtonsHandler.prototype.OkButton = function () {
        if (this.currentActionName == "OperationalClose" || this.currentActionName == "AccountingClose") {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Shipment", this.currentActionName);
        }
        else if (this.currentActionName == "ExceptionResolved") {
            this.EntityPM.ExceptionResolvedDescription = this.EntityPM.EventNote;
        }
        this.entityArgs.EditComponent.SaveChanges();
        this.MenuButtonCode = null;
        this.isButtonClicked = false;
    };
    ShipmentMenuButtonsHandler.prototype.OperationalCloseShipment = function () {
        var _this = this;
        this.currentActionName = "OperationalClose";
        var WarningsList = [];
        var ErrorsList = [];
        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.myCloner = new Cloner_1.Cloner(this.EntityPM);
            this.Clone(this.EntityPM);
            var args = this;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Master Operational Close";
            logWindow.WindowArgs = args;
            logWindow.IsFillScreen = true;
            logWindow.Show('./Shipment/Components/MenuButtons/MasterActionConfirmationComponent');
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.ResetButtonClicked();
                if ($event == "confirm") {
                    _this.EntityPM.OperationalCloseDate = Tools_2.DateTool.GetCurrentDateTimeAsUtc();
                    _this.EntityPM.EventNote = null;
                    _this.OkButton();
                }
                else {
                    _this.RejectChanges();
                }
            });
        }
        else {
            this.myCloner = new Cloner_1.Cloner(this.EntityPM);
            this.Clone(this.EntityPM);
            this.ActionStepsStateList = new Array();
            this.EntityPM.IsOperationalClosed = true;
            this.ValidateShipmentRules(WarningsList, ErrorsList, this.EntityPM);
            var tableId = window.ObjectTables.filter(function (t) { return t.Name == "Shipment"; })[0].Id;
            ServiceLocator_1.ServiceLocator.RulesValidator.ValidateAllRequiredFieldRules(this.EntityPM, tableId, ErrorsList);
            this.DisplayErrorsWindow(WarningsList, ErrorsList);
        }
    };
    ShipmentMenuButtonsHandler.prototype.SplitShipmentClicked = function () {
        this.Validate();
        if (this.isValid) {
            if (this.EntityPM.IsOperationalClosed) {
                this.StopFlags();
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("Can't split an operationally closed shipment");
            }
            else {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    };
    ShipmentMenuButtonsHandler.prototype.SplitShipmentApply = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = { EntityPM: this.EntityPM };
        logWindow.IsFillScreen = true;
        logWindow.Title = "Split Shipment";
        logWindow.Show('./Shipment/Components/SplitShipment/SplitShipmentComponent');
        logWindow.ComponentLoaded.subscribe(function (cmp) {
            _this.StopFlags();
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
        });
    };
    ShipmentMenuButtonsHandler.prototype.ConvertShipmentToLCLClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Shipment", errors);
        if (errors.length == 0) {
            this.IsConvertToLCLClicked = true;
            this.OkButton();
        }
    };
    ShipmentMenuButtonsHandler.prototype.ConvertShipmentToFCLClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Shipment", errors);
        if (errors.length == 0) {
            this.IsConvertToFCLClicked = true;
            this.OkButton();
        }
    };
    ShipmentMenuButtonsHandler.prototype.DoConvertShipmentType = function (type) {
        var _this = this;
        var errors = [];
        if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
            errors.push("Cannot change shipment type when connected to a quote");
        }
        else if (this.EntityPM.ShipmentPackages.filter(function (d) { return !Tools_2.AppTool.IsNullOrEmpty(d.DeliveryId); }).length > 0
            || this.EntityPM.ShipmentPackages.filter(function (d) { return !Tools_2.AppTool.IsNullOrEmpty(d.EmptyContainerReturnId); }).length > 0) {
            errors.push("Cannot change shipment type when shipment packages are connected to a delivery or empty container return");
        }
        else if (this.EntityPM.ShipmentLevelCode == "H" && !Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
            errors.push("Cannot change shipment type when connected to a Master shipment");
        }
        else if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
            errors.push("Cannot change shipment type when connected to house shipments ");
        }
        else if (this.EntityPM.IsOperationalClosed) {
            errors.push("Cannot change shipment type when shipment is operationally closed");
        }
        else if (this.EntityPM.MainCarriageATD != null || this.EntityPM.Transshipment1ATD != null || this.EntityPM.Transshipment2ATD != null
            || this.EntityPM.Transshipment3ATD != null || this.EntityPM.MainCarriageATA != null || this.EntityPM.Transshipment1ATA != null
            || this.EntityPM.Transshipment2ATA != null || this.EntityPM.Transshipment3ATA != null) {
            errors.push("Cannot change shipment type when shipment contains actual departure/arrival dates");
        }
        if (errors.length == 0) {
            this.shipmentService.CheckIfConnectedEntryOrRelease(this.EntityPM.Id).subscribe(function (myResponse) {
                if (myResponse != null) {
                    var result = myResponse.Result;
                    if (result) {
                        errors.push("Cannot change shipment type when shipment is connected to Cross Docks Entries / Releases");
                    }
                    _this.ShowNotesWindow(errors, type);
                }
            });
        }
        else {
            this.ShowNotesWindow(errors, type);
        }
    };
    ShipmentMenuButtonsHandler.prototype.ShowNotesWindow = function (errors, type) {
        var _this = this;
        var args = new MenuButtonsTemplateComponent_1.MenuButtonsTemplateArgs();
        var windowTitle;
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.EventNote = null;
        args.IsConvertShipmentType = true;
        args.ValidationErrorsList = errors;
        args.EnabledOkButton = false;
        if (errors.length == 0) {
            args.IsNotesStackPanelVisible = true;
            args.NotesHeader = "Notes";
            args.EnabledOkButton = true;
        }
        switch (type) {
            case "ToLCL":
                {
                    this.currentActionName = "ConvertShipmentToLCL";
                    windowTitle = "Convert Shipment From FCL To LCL";
                    break;
                }
            case "ToFCL":
                {
                    this.currentActionName = "ConvertShipmentToFCL";
                    windowTitle = "Convert Shipment From LCL To FCL";
                    break;
                }
        }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    _this.EntityPM.EventNote = notes;
                    switch (type) {
                        case "ToLCL":
                            {
                                _this.EntityPM.ConvertShipmentToLCL = true;
                                _this.EntityPM.ConvertShipmentToFCL = false;
                                break;
                            }
                        case "ToFCL":
                            {
                                _this.EntityPM.ConvertShipmentToLCL = false;
                                _this.EntityPM.ConvertShipmentToFCL = true;
                                break;
                            }
                    }
                    _this.Reload = true;
                    _this.OkButton();
                    _this.ResetButtonClicked();
                }
            });
        });
    };
    ShipmentMenuButtonsHandler.prototype.ConvertShipmentDirectionClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Shipment", errors);
        if (errors.length == 0) {
            this.IsConvertDirectionClicked = true;
            this.OkButton();
        }
    };
    ShipmentMenuButtonsHandler.prototype.DoConvertShipmentDirection = function () {
        var _this = this;
        var errors = [];
        if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
            errors.push("Shipment is connected to a quote, can't change direction");
        }
        else if (this.EntityPM.ShipmentPackages.filter(function (d) { return !Tools_2.AppTool.IsNullOrEmpty(d.DeliveryId); }).length > 0
            || this.EntityPM.ShipmentPackages.filter(function (d) { return !Tools_2.AppTool.IsNullOrEmpty(d.EmptyContainerReturnId); }).length > 0) {
            errors.push("Shipment packages are connected to a delivery or empty container return, can't change direction");
        }
        else if (this.EntityPM.ShipmentLevelCode == "H" && !Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
            errors.push("Shipment is connected to other shipment/s, can't change direction");
        }
        else if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
            errors.push("Shipment is connected to other shipment/s, can't change direction");
        }
        else if (this.EntityPM.IsOperationalClosed) {
            errors.push("Shipment is closed operationally, can't change direction");
        }
        else if (this.EntityPM.MainCarriageATD != null || this.EntityPM.Transshipment1ATD != null || this.EntityPM.Transshipment2ATD != null
            || this.EntityPM.Transshipment3ATD != null || this.EntityPM.MainCarriageATA != null || this.EntityPM.Transshipment1ATA != null
            || this.EntityPM.Transshipment2ATA != null || this.EntityPM.Transshipment3ATA != null) {
            errors.push("Shipment has departed/arrived, can't change direction");
        }
        if (errors.length == 0) {
            this.shipmentService.CheckIfConnectedEntryOrRelease(this.EntityPM.Id).subscribe(function (myResponse) {
                if (myResponse != null) {
                    var result = myResponse.Result;
                    if (result) {
                        errors.push("Shipment has connected Cross DocKs Entries/Releases, can't change direction");
                    }
                    _this.ShowConvertShipmentDirectionWindow(errors);
                }
            });
        }
        else {
            this.ShowConvertShipmentDirectionWindow(errors);
        }
    };
    ShipmentMenuButtonsHandler.prototype.ShowConvertShipmentDirectionWindow = function (errors) {
        var _this = this;
        this.currentActionName = "ConvertShipmentDirection";
        var args = new ShipmenDirectionConvertComponent_1.ConvertDirectionArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.ValidationErrorsList = errors;
        args.EnabledOkButton = true;
        if (errors.length > 0) {
            args.EnabledOkButton = false;
        }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "Convert Shipment Direction";
        logWindow.Show('./Shipment/Components/MenuButtons/ShipmenDirectionConvertComponent');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                if (d == "ok") {
                    _this.ResetButtonClicked();
                }
            });
        });
    };
    ShipmentMenuButtonsHandler.prototype.Clone = function (EntityPM) {
        this.myCloner.AddField('IsOperationalClosed');
        this.myCloner.AddField('IsMaster');
        this.myCloner.AddEntity(EntityPM);
    };
    ShipmentMenuButtonsHandler.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    ShipmentMenuButtonsHandler.prototype.DisplayErrorsWindow = function (WarningsList, ErrorsList) {
        var _this = this;
        console.log(WarningsList);
        console.log(ErrorsList);
        var args = new MenuButtonsTemplateComponent_1.MenuButtonsTemplateArgs();
        args.ValidationErrorsList = ErrorsList;
        args.ValidationWarningsList = WarningsList;
        args.IsNotesStackPanelVisible = false;
        args.ActionStepsStateList = this.ActionStepsStateList;
        var state = new ActionsStepsState();
        state.Message = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.CheckingRequiredFields");
        if (ErrorsList.length != 0) {
            state.State = "Failed";
        }
        else {
            state.State = "Succeeded";
        }
        this.ActionStepsStateList.push(state);
        if (ErrorsList.length > 0)
            args.EnabledOkButton = false;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Shipment Operational Close";
        logWindow.WindowArgs = args;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.ResetButtonClicked();
            if ($event == "confirm") {
                _this.EntityPM.IsOperationalClosed = true;
                _this.EntityPM.OperationalCloseDate = Tools_2.DateTool.GetCurrentDateTimeAsUtc();
                _this.EntityPM.EventNote = null;
                _this.OkButton();
            }
            else {
                _this.EntityPM.RejectChanges();
                _this.RejectChanges();
            }
        });
    };
    ShipmentMenuButtonsHandler.prototype.ValidateShipmentRules = function (WarningsList, ErrorsList, entityPM) {
        var tableId = window.ObjectTables.filter(function (t) { return t.Name == "Shipment"; })[0].Id;
        var tableId = window.ObjectTables.filter(function (t) { return t.Name == "Shipment"; })[0].Id;
        var warningValidator = new EntityWarningsValidator_1.EntityWarningsValidator();
        var ruleValidator = new RulesValidator_1.RulesValidator();
        var requiredFields = [];
        // ruleValidator.ExecuteRequierdFieldRule(entityPM,
        //ruleValidator.ValidateAllRequiredFieldRules(entityPM, tableId, ErrorsList);
        if (entityPM.ShipmentLevelCode == "H") {
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_AE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_AI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_OE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_OI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_IE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_II", entityPM, requiredFields);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_AE", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_AI", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_OE", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_OI", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_IE", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_II", entityPM, WarningsList);
        }
        if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C") {
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II_D", entityPM, requiredFields);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AE", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AI", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OE", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OI", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_IE", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_II", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AE_D", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AI_D", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OE_D", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OI_D", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_IE_D", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_II_D", entityPM, WarningsList);
        }
        var _tenantObjectFields = window.ObjectFields;
        if (requiredFields.length != 0) {
            for (var k in requiredFields) {
                var field = requiredFields[k];
                var obField = _tenantObjectFields.filter(function (x) { return x.Id === field.ObjectFieldId; })[0]; //ObjectFieldsCachedDataProvider.GetObjectFieldById(field.ObjectFieldId);
                var requiredError = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
                var fieldTrans = TextCodeTranslator_1.TextCodeTranslator.Translate(obField.FullNameTextCodeCode);
                requiredError = requiredError.replace("%FieldName", fieldTrans);
                ErrorsList.push(requiredError);
            }
        }
    };
    return ShipmentMenuButtonsHandler;
}());
exports.ShipmentMenuButtonsHandler = ShipmentMenuButtonsHandler;
var ActionValidationArgs = /** @class */ (function () {
    function ActionValidationArgs() {
        this.WarningsList = [];
        this.ErrorsList = [];
    }
    return ActionValidationArgs;
}());
exports.ActionValidationArgs = ActionValidationArgs;
var ActionsStepsState = /** @class */ (function () {
    function ActionsStepsState() {
    }
    Object.defineProperty(ActionsStepsState.prototype, "Message", {
        get: function () { return this.message; },
        set: function (value) { this.message = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActionsStepsState.prototype, "State", {
        get: function () { return this.state; },
        set: function (value) { this.state = value; },
        enumerable: true,
        configurable: true
    });
    return ActionsStepsState;
}());
exports.ActionsStepsState = ActionsStepsState;
var ValidationErrorInfo = /** @class */ (function () {
    function ValidationErrorInfo() {
    }
    Object.defineProperty(ValidationErrorInfo.prototype, "MessageType", {
        get: function () { return this.messageType; },
        set: function (value) { this.messageType = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ValidationErrorInfo.prototype, "ErrorCode", {
        get: function () { return this.errorCode; },
        set: function (value) { this.errorCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ValidationErrorInfo.prototype, "ErrorMessage", {
        get: function () { return this.errorMessage; },
        set: function (value) { this.errorMessage = value; },
        enumerable: true,
        configurable: true
    });
    ValidationErrorInfo.prototype.ToString = function () {
        return this.ErrorMessage;
    };
    return ValidationErrorInfo;
}());
exports.ValidationErrorInfo = ValidationErrorInfo;
//# sourceMappingURL=ShipmentMenuButtonsHandler.js.map