"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var CustomerPMService_1 = require("../../Services/StandardPMs/CustomerPMService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var PartnersDomainService_1 = require("../../Services/PartnersDomainService");
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ActionStepsTemplate_1 = require("../../../CommonModules/CommonPartners/Components/Templates/ActionStepsTemplate");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var Args_1 = require("../../../Common/Args");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var CreateTenantHelper_1 = require("../../../InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantHelper");
var TenantManagementPMService_1 = require("../../../Infrastructure/Services/StandardPMs/TenantManagementPMService");
var DownloadManager_1 = require("../../../Infrastructure/Utilities/DownloadManager");
var CustomerMenuButtonsHandler = /** @class */ (function () {
    function CustomerMenuButtonsHandler() {
        this.ObjectTableName = "Customer";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isCustomerConnectedToEntities = false;
        this.activationTag = null;
    }
    CustomerMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    CustomerMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.isSetAsPotential) {
                        _this.isSetAsPotential = false;
                        _this.SetAsPotential();
                    }
                    if (_this.isInActiveCustomer) {
                        _this.isInActiveCustomer = false;
                        _this.InActiveCustomer();
                    }
                    if (_this.isReActivateCustomer) {
                        _this.isReActivateCustomer = false;
                        _this.ReActivateCustomer();
                    }
                    if (_this.isSetMyCustomer) {
                        _this.isSetMyCustomer = false;
                        _this.SetMyCustomer();
                    }
                    if (_this.isSetNotMyCustomer) {
                        _this.isSetNotMyCustomer = false;
                        _this.SetNotMyCustomer();
                    }
                    if (_this.isActivate) {
                        _this.isActivate = false;
                        if (_this.activationTag == "activate") {
                            _this.ActivateCustomer();
                        }
                    }
                    if (_this.isMarkAsReadyActivation) {
                        _this.isMarkAsReadyActivation = false;
                        if (_this.activationTag == "ready") {
                            _this.MarkAsReadyForActivation();
                        }
                    }
                }
            });
        }
    };
    CustomerMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'Customer'; })[0];
                var buttonEnabled = true;
                var eventsTabFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "UPDATE") && f.ObjectTableId == table.Id; })[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "SetAsPotential":
                            {
                                if (this.EntityPM.CustomerStatusCode == "POT") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
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
                                if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "CREATETENANT"))
                                    button.IsHidden = true;
                                else
                                    button.IsHidden = false;
                                break;
                            }
                    }
                }
            }
        }
        return menuButtons;
    };
    CustomerMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
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
        }
    };
    Object.defineProperty(CustomerMenuButtonsHandler.prototype, "Notes", {
        // Props 
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerMenuButtonsHandler.prototype.ResetAllFlags = function () {
        this.isSetAsPotential = false;
        this.isInActiveCustomer = false;
        this.isSetMyCustomer = false;
        this.isSetNotMyCustomer = false;
        this.isReActivateCustomer = false;
        this.isActivate = false;
        this.isMarkAsReadyActivation = false;
    };
    CustomerMenuButtonsHandler.prototype.SetAsPotential = function () {
        if (this.EntityPM.LastShipmentDate != null) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            var validationErrorMessage = "This customer can't be set as potential since it has shipment(s).";
            messageWindow.Show(validationErrorMessage);
        }
        else {
            this.IsCustomerConnectedToEntities();
        }
    };
    CustomerMenuButtonsHandler.prototype.IsCustomerConnectedToEntities = function () {
        var _this = this;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetIsCustomerConnectedToEntities(this.EntityPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.isCustomerConnectedToEntities = myResponse.Result;
                if (_this.isCustomerConnectedToEntities) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    var validationErrorMessage = "This customer can't be set as potential since it has shipment(s).";
                    messageWindow.Show(validationErrorMessage);
                }
                else {
                    _this.EntityPM.SetAsPotential = true;
                    _this.EntityPM.CustomerStatusCode = "POT";
                    _this.isSetAsPotential = false;
                    _this.entityArgs.EditComponent.SaveChanges();
                }
            }
        });
    };
    CustomerMenuButtonsHandler.prototype.ReloadCurrentCustomer = function (tag) {
        this.activationTag = tag;
        if (this.EntityPM != null && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.GetSingleEntity();
        }
    };
    CustomerMenuButtonsHandler.prototype.GetSingleEntity = function () {
        var _this = this;
        var myService = new CustomerPMService_1.CustomerPMService();
        myService.get(this.EntityPM.Id).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result;
                }
                _this.entityArgs.EditComponent.SaveChanges();
            }
        });
    };
    CustomerMenuButtonsHandler.prototype.ActivateCustomer = function () {
        var _this = this;
        if (this.EntityPM != null) {
            var args = new Args_1.CustomerActivationArgs();
            args.EntityPM = this.EntityPM;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Title = "Customer Activation";
            logWindow.Show('./CommonModules/CommonCustomer/Components/CustomerActivation/CustomerActivationComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                _this.isActivate = false;
                if (s) {
                    _this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
        }
    };
    CustomerMenuButtonsHandler.prototype.MarkAsReadyForActivation = function () {
        var _this = this;
        if (this.EntityPM != null) {
            var args = new Args_1.CustomerActivationArgs();
            args.EntityPM = this.EntityPM;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Width = 900;
            logWindow.Height = 700;
            logWindow.Title = "Customer Activation";
            logWindow.Show('./CommonModules/CommonCustomer/Components/CustomerActivation/ReadyForActivationComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                _this.isActivate = false;
                if (s) {
                    _this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
        }
    };
    CustomerMenuButtonsHandler.prototype.InActiveCustomer = function () {
        var _this = this;
        var args = new ActionStepsTemplate_1.ActionStepsTemplateArgs();
        args.ObjectTableName = this.ObjectTableName;
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Deactivate Customer Notes";
        args.IsNotesStackPanelVisible = true;
        args.EventNote = "";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 450;
        logWindow.Height = 300;
        logWindow.Title = "Deactivate Customer";
        //logWindow.WindowClosed.subscribe(($event: any) => this.OnInActiveCustomerWindowClosed($event));
        logWindow.Show('./CommonModules/CommonPartners/Components/Templates/ActionStepsTemplate');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                var notes = s.EventNotes;
                if (!Tools_1.AppTool.IsNullOrEmpty(notes)) {
                    _this.EntityPM.EventNote = notes;
                }
                _this.OnInActiveCustomerWindowClosed(d);
            });
        });
    };
    CustomerMenuButtonsHandler.prototype.OnInActiveCustomerWindowClosed = function (arg) {
        if (arg == 'confirm') {
            this.EntityPM.SetInActive = true;
            var errors = [];
            Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
            if (errors.length == 0) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    };
    CustomerMenuButtonsHandler.prototype.ReActivateCustomer = function () {
        var _this = this;
        var args = new ActionStepsTemplate_1.ActionStepsTemplateArgs();
        args.ObjectTableName = this.ObjectTableName;
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Reactivate Customer Notes";
        args.EventNote = "";
        args.IsNotesStackPanelVisible = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 450;
        logWindow.Height = 300;
        logWindow.Title = "Reactivate Customer";
        //logWindow.WindowClosed.subscribe(($event: any) => this.OnReActivateCustomerWindowClosed($event));
        logWindow.Show('./CommonModules/CommonPartners/Components/Templates/ActionStepsTemplate');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                var notes = s.EventNotes;
                if (!Tools_1.AppTool.IsNullOrEmpty(notes)) {
                    _this.EntityPM.EventNote = notes;
                }
                _this.OnReActivateCustomerWindowClosed(d);
            });
        });
    };
    CustomerMenuButtonsHandler.prototype.OnReActivateCustomerWindowClosed = function (arg) {
        if (arg == 'confirm') {
            this.EntityPM.SetReActivated = true;
            var errors = [];
            Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
            if (errors.length == 0) {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    };
    CustomerMenuButtonsHandler.prototype.SetMyCustomer = function () {
        this.EntityPM.IsCustomer = true;
        this.StartBusyIndicator("Saving...");
        this.entityArgs.EditComponent.SaveChanges();
        this.StopBusyIndicator();
    };
    CustomerMenuButtonsHandler.prototype.SetNotMyCustomer = function () {
        this.EntityPM.IsCustomer = false;
        this.StartBusyIndicator("Saving...");
        this.entityArgs.EditComponent.SaveChanges();
        this.StopBusyIndicator();
    };
    CustomerMenuButtonsHandler.prototype.EditTenantManagement = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ReceivablesAccountingCard)) {
            var id = parseInt(this.EntityPM.ReceivablesAccountingCard);
            var managementService = new TenantManagementPMService_1.TenantManagementPMService();
            managementService.get(id).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    var window = new MessageWindow_1.MessageWindow();
                    window.Width = 350;
                    window.Height = 180;
                    window.Show(myResponse.ErrorsArray[0]);
                }
                else {
                    var ten = myResponse.Result;
                    if (ten != null) {
                        _this.StartEditing(ten.Id);
                    }
                }
            });
        }
        else {
            var window = new MessageWindow_1.MessageWindow();
            window.Width = 350;
            window.Height = 180;
            window.Show("Please fill accounting external Id field");
        }
    };
    CustomerMenuButtonsHandler.prototype.StartEditing = function (id) {
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.IsFillScreen = true;
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = "Edit Tenant Management";
        editWindow.IsEditComponent = true;
        editWindow.WindowClosed.subscribe(function (event) {
        });
        editWindow.ShowEditComponent(id, "TenantManagement", "", true);
    };
    CustomerMenuButtonsHandler.prototype.ViewQuestionnaireAnswers = function () {
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'Customer'; })[0];
        var customerName = !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.EnglishName) ? this.EntityPM.EnglishName : "";
        var entityId = this.TenantPM.DefaultQuestionnaireId + "_" + table.Id + "_" + this.EntityPM.Id + "_" + "QuestionnaireAnswers" + "_" + customerName;
        DownloadManager_1.DownloadManager.DownloadPage(entityId);
    };
    CustomerMenuButtonsHandler.prototype.GoTotango = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ReceivablesAccountingCard)) {
            var link = "https://app.totango.com/#!/customerDetails?customer=" + this.EntityPM.ReceivablesAccountingCard;
            window.open(link, '_blank');
        }
        else {
            var message = new MessageWindow_1.MessageWindow();
            message.Width = 350;
            message.Height = 180;
            message.Show("Please Fill External Id Field");
        }
    };
    CustomerMenuButtonsHandler.prototype.CreateTenantMethod = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = "";
        confirmWindow.Width = 400;
        confirmWindow.Height = 200;
        confirmWindow.Show("Please confirm creating a new tenant for this customer ?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var createTenantHelper = new CreateTenantHelper_1.CreateTenantHelper("Customer", _this.EntityPM.Id, _this.EntityPM.EnglishName, _this.EntityPM.ReceivablesAccountingCard, _this.EntityPM.PrimaryContactId, _this.EntityPM.VatNumber, _this.EntityPM.CountryName, _this.EntityPM.CountryCode);
                createTenantHelper.CreateTenantMethod();
            }
        });
    };
    CustomerMenuButtonsHandler.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    CustomerMenuButtonsHandler.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    return CustomerMenuButtonsHandler;
}());
exports.CustomerMenuButtonsHandler = CustomerMenuButtonsHandler;
//# sourceMappingURL=CustomerMenuButtonsHandler.js.map