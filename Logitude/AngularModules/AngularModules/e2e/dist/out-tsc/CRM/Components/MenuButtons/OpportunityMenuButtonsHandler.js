"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var OpportunityPM_1 = require("../../EntityPMs/OpportunityPM");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var OpportunityClosingReasonListService_1 = require("../../Services/StandardLists/OpportunityClosingReasonListService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var Tools_1 = require("../../../Infrastructure/Tools");
var TenantManagementPMService_1 = require("../../../Infrastructure/Services/StandardPMs/TenantManagementPMService");
var CustomerListService_1 = require("../../../Common/Services/StandardLists/CustomerListService");
var StageListService_1 = require("../../Services/StandardLists/StageListService");
var Args_1 = require("../../Args");
var OpportunityPMInitService_1 = require("../../EntityPMInitServices/OpportunityPMInitService");
var CreateTenantHelper_1 = require("../../../InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantHelper");
var OpportunityMenuButtonsHandler = /** @class */ (function () {
    function OpportunityMenuButtonsHandler() {
        this.status = false;
        this.DataContext = this;
        this.MenuButtonCode = null;
        this.isButtonClicked = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ValidationErrorsList = [];
    }
    OpportunityMenuButtonsHandler.prototype.StopFlags = function () {
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
    };
    OpportunityMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    switch (_this.MenuButtonCode) {
                        case "CloseAsWon": {
                            _this.CloseWon_SaveCompleted();
                            break;
                        }
                        case "CloseAsLost": {
                            _this.CloseLost_SaveCompleted();
                            break;
                        }
                        case "Copy": {
                            _this.StartCopy_SaveCompleted();
                            break;
                        }
                        case "Cancel": {
                            _this.CancelCommand_SaveCompleted();
                            break;
                        }
                    }
                }
                _this.StopFlags();
                _this.CurrentSession.StopBusyIndicator();
            });
            this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                }
                _this.StopFlags();
            });
        }
    };
    OpportunityMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    OpportunityMenuButtonsHandler.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.EntityPM);
        this.myCloner.AddField('ClosingReasonId');
        this.myCloner.AddField('ClosingReasonCode');
        this.myCloner.AddField('ClosingDescription');
        this.myCloner.AddField('PostToFollowersAsWon');
        this.myCloner.AddField('ClosedToCompetitorId');
        this.myCloner.AddField('ActualClosingDate');
        this.myCloner.AddField('StageId');
        this.myCloner.AddField('IsClosed');
        this.myCloner.AddField('IsClosedLost');
        this.myCloner.AddEntity(this.EntityPM);
    };
    OpportunityMenuButtonsHandler.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    OpportunityMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "Cancel":
                            {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "ReOpen":
                            {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }
                                else if (this.EntityPM.IsClosed) {
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                                break;
                            }
                        case "Copy":
                            {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "CloseAsWon":
                            {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }
                                else if (this.EntityPM.IsClosed) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "CloseAsLost":
                            {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }
                                else if (this.EntityPM.IsClosed) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "OpportunityTotango":
                            {
                                break;
                            }
                        case "OpportunityTenantManagement":
                            {
                                button.Width = 90;
                                break;
                            }
                        case "OpportunityCreateTenant":
                            {
                                button.Width = 120;
                                if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "CREATETENANT")) {
                                    button.IsHidden = true;
                                }
                                else {
                                    button.IsHidden = false;
                                }
                                break;
                            }
                        case "Edit":
                            {
                                button.IsHidden = true;
                                if (!this.EntityPM.IsCancelled) {
                                    if (this.EntityPM.IsClosed) {
                                        button.IsHidden = false;
                                    }
                                }
                                break;
                            }
                    }
                }
            }
        }
    };
    OpportunityMenuButtonsHandler.prototype.ConfirmWindow_Unloaded = function () {
        this.ValidationErrorsList = [];
        if (this.confirmWindow.Yes) {
            if (this.Validate()) {
                this.EntityPM.IsCancelled = true;
                this.CurrentSession.StartBusyIndicatorSaving();
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
        this.StopFlags();
    };
    OpportunityMenuButtonsHandler.prototype.CancelCommand_SaveCompleted = function () {
        this.entityArgs.EditComponent.SaveChanges("Saving");
    };
    OpportunityMenuButtonsHandler.prototype.CancelCommand = function () {
        var _this = this;
        var confirmMsg = "Are you sure you want to cancel this Opportunity ?";
        if (this.confirmWindow == null)
            this.confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        this.confirmWindow.Width = 400;
        this.confirmWindow.ShowCancelButton = false;
        this.confirmWindow.WindowClosed.subscribe(function (event) { _this.ConfirmWindow_Unloaded(); });
        this.confirmWindow.Show(confirmMsg);
    };
    OpportunityMenuButtonsHandler.prototype.CloseWon_SaveCompleted = function () {
        var _this = this;
        var closingListService = new OpportunityClosingReasonListService_1.OpportunityClosingReasonListService();
        closingListService.getAllFromCache().subscribe(function (result) {
            var myClosingReason = result.Result.filter(function (d) { return d.Code == "WN"; })[0];
            if (myClosingReason == null) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("Error closing reason is not found");
            }
            else {
                _this.Clone();
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 500;
                logWindow.Height = 300;
                logWindow.Title = "Close As Won";
                logWindow.WindowArgs = _this.EntityPM;
                logWindow.ComponentLoaded.subscribe(function (cmpRef) {
                    cmpRef.ClosingReasonId = myClosingReason.Id;
                });
                logWindow.WindowClosed.subscribe(function (event) {
                    if (event == "cancle") {
                        _this.RejectChanges();
                    }
                    else {
                        OpportunityPMInitService_1.OpportunityPMInitService.InitValues(_this.EntityPM, false);
                    }
                });
                logWindow.Show('./CRM/Components/MenuButtons/CloseAsWonOrLostComponent');
            }
        });
    };
    OpportunityMenuButtonsHandler.prototype.Validate = function () {
        this.ValidationErrorsList = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Opportunity", this.ValidationErrorsList);
        if (this.ValidationErrorsList.length == 0) {
            this.entityArgs.EditComponent.ValidationErrorsList = this.ValidationErrorsList;
            return true;
        }
        else
            this.entityArgs.EditComponent.ValidationErrorsList = this.ValidationErrorsList;
        return false;
    };
    OpportunityMenuButtonsHandler.prototype.CloseAsWon = function () {
        this.entityArgs.EditComponent.SaveChanges("Saving");
    };
    OpportunityMenuButtonsHandler.prototype.CloseAsLost = function () {
        this.entityArgs.EditComponent.SaveChanges("Saving");
    };
    OpportunityMenuButtonsHandler.prototype.CloseLost_SaveCompleted = function () {
        var _this = this;
        this.Clone();
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 330;
        logWindow.Title = "Close As Lost";
        logWindow.WindowArgs = this.EntityPM;
        logWindow.ComponentLoaded.subscribe(function (cmpRef) {
            cmpRef.IsClosedLost = true;
        });
        logWindow.WindowClosed.subscribe(function (event) {
            if (event == "cancle") {
                _this.RejectChanges();
            }
            else {
                OpportunityPMInitService_1.OpportunityPMInitService.InitValues(_this.EntityPM, false);
            }
        });
        logWindow.Show('./CRM/Components/MenuButtons/CloseAsWonOrLostComponent');
    };
    OpportunityMenuButtonsHandler.prototype.ReOpenOpoortunity = function () {
        var _this = this;
        this.Clone();
        this.EntityPM.StageId = null;
        this.EntityPM.IsClosed = false;
        this.EntityPM.ActualClosingDate = null;
        this.EntityPM.ClosingDescription = null;
        this.EntityPM.ClosingReasonId = null;
        this.EntityPM.EstimatedClosingDate = null;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = 200;
        logWindow.Title = "Select Stage";
        logWindow.WindowArgs = this.EntityPM;
        logWindow.WindowClosed.subscribe(function (event) {
            if (event == "cancle") {
                _this.RejectChanges();
            }
            else {
                OpportunityPMInitService_1.OpportunityPMInitService.InitValues(_this.EntityPM, false);
            }
            _this.StopFlags();
        });
        logWindow.Show('./CRM/Components/MenuButtons/ReOpen_StageComponent');
    };
    OpportunityMenuButtonsHandler.prototype.LoadCustomer = function (tag) {
        var _this = this;
        var cardService = new CardListService_1.CardListService();
        cardService.getSingle(this.EntityPM.CustomerId).subscribe(function (result) {
            var loadedCustomer = result.Result;
            _this.EntityPM.CustomerExternalId = loadedCustomer.ReceivablesAccountingCard;
            if (loadedCustomer != null) {
                if (tag == "manage") {
                    _this.EditTenantManagement();
                }
                else if (tag == "totango") {
                    _this.GoTotango();
                }
            }
        });
    };
    OpportunityMenuButtonsHandler.prototype.GoTotango = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerExternalId)) {
            var link = "https://app.totango.com/#!/customerDetails?customer=" + this.EntityPM.CustomerExternalId;
            window.open(link, '_blank');
        }
        else {
            var message = new MessageWindow_1.MessageWindow();
            message.Width = 350;
            message.Height = 180;
            message.Show("Please Fill External Id Field");
        }
        this.StopFlags();
    };
    OpportunityMenuButtonsHandler.prototype.EditTenantManagement = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerExternalId)) {
            var id = parseInt(this.EntityPM.CustomerExternalId);
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
            window.Show("Please Fill External Id Field");
            this.StopFlags();
        }
    };
    OpportunityMenuButtonsHandler.prototype.StartEditing = function (id) {
        var _this = this;
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.IsFillScreen = true;
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = "Edit Tenant Management";
        editWindow.IsEditComponent = true;
        editWindow.WindowClosed.subscribe(function (event) {
            _this.StopFlags();
        });
        editWindow.ShowEditComponent(id, "TenantManagement", "", true);
    };
    OpportunityMenuButtonsHandler.prototype.CreateTenantMethod = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = "";
        confirmWindow.Width = 400;
        confirmWindow.Height = 200;
        confirmWindow.Show("Please confirm creating a new tenant for this customer ?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            _this.StopFlags();
            if (confirmWindow.Yes) {
                if (_this.EntityPM.Field3 == null || (_this.EntityPM.Field3 != null && Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Field3.Value))) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.CustomerId)) {
                        var customerService = new CustomerListService_1.CustomerListService();
                        customerService.getSingle(_this.EntityPM.CustomerId).subscribe(function (result) {
                            var customer = result.Result;
                            var createTenantHelper = new CreateTenantHelper_1.CreateTenantHelper("Opportunity", customer.Id, customer.EnglishName, customer.ReceivablesAccountingCard, customer.PrimaryContactId, customer.VatNumber, customer.CountryName, customer.CountryCode);
                            createTenantHelper.CreateTenantMethod();
                        });
                    }
                }
                else {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    var validationErrorMessage = "note that this customer has a tenant already " + _this.EntityPM.Field3.Value + ", please erase the tenant# in order to create a new one" + " (" + _this.EntityPM.Field3.Value + " = External ID)";
                    messageWindow.Show(validationErrorMessage);
                }
            }
        });
    };
    OpportunityMenuButtonsHandler.prototype.CopyOpportunity = function () {
        this.entityArgs.EditComponent.SaveChanges("Saving");
    };
    OpportunityMenuButtonsHandler.prototype.StartCopy = function () {
        this.entityArgs.EditComponent.SaveChanges("Saving");
    };
    OpportunityMenuButtonsHandler.prototype.StartCopy_SaveCompleted = function () {
        var _this = this;
        var newOpportunity = new OpportunityPM_1.OpportunityPM();
        newOpportunity.Tenant = this.EntityPM.Tenant;
        newOpportunity.OwnerId = this.EntityPM.OwnerId;
        newOpportunity.Subject = this.EntityPM.Subject;
        newOpportunity.CustomerId = this.EntityPM.CustomerId;
        newOpportunity.LeadSourceId = this.EntityPM.LeadSourceId;
        newOpportunity.ContactId = this.EntityPM.ContactId;
        newOpportunity.CustomerName = this.EntityPM.CustomerName;
        newOpportunity.CustomerRankCode = this.EntityPM.CustomerRankCode;
        newOpportunity.CustomerRankName = this.EntityPM.CustomerRankName;
        newOpportunity.NumberOfShipments = this.EntityPM.NumberOfShipments;
        newOpportunity.OpportunityTypeId = this.EntityPM.OpportunityTypeId;
        newOpportunity.OwnerName = this.EntityPM.OwnerName;
        newOpportunity.RatingName = this.EntityPM.RatingName;
        newOpportunity.EstimatedClosingDate = this.EntityPM.EstimatedClosingDate;
        newOpportunity.ValueField = this.EntityPM.ValueField;
        newOpportunity.CreateDate = this.EntityPM.CreateDate;
        newOpportunity.CreatedByUserId = this.EntityPM.CreatedByUserId;
        newOpportunity.UpdateDate = this.EntityPM.UpdateDate;
        newOpportunity.UpdatedByUserId = this.EntityPM.UpdatedByUserId;
        newOpportunity.RatingCode = this.EntityPM.RatingCode;
        newOpportunity.IsCopy = true;
        newOpportunity.CopyFromEntityId = this.EntityPM.Id;
        newOpportunity.Notes = this.EntityPM.Notes;
        newOpportunity.BusinessUnitId = this.EntityPM.BusinessUnitId;
        var todayDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        var stageService = new StageListService_1.StageListService();
        stageService.getAllFromCache().subscribe(function (result) {
            var myStage = result.Result.filter(function (d) { return d.Code == "QUA" && d.Tenant == SessionLocator_1.SessionLocator.Tenant; })[0];
            if (myStage != null) {
                newOpportunity.StageId = myStage.Id;
                newOpportunity.StageName = myStage.Name;
                newOpportunity.Probability = myStage.Probability;
                if (myStage.MaxDays != null) {
                    newOpportunity.StageDueDate = Tools_1.DateTool.AddDays(Tools_1.DateTool.GetCurrentDateAsUtc(), parseFloat(myStage.MaxDays + ""));
                }
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 850;
                logWindow.Height = 600;
                logWindow.Title = "Copy Opportunity";
                var args = new Args_1.OpportunityArgs();
                args.Entity = newOpportunity;
                args.IsNew = false;
                logWindow.WindowArgs = args;
                logWindow.Show('./CRMModules/CRMOpportunity/Components/NewEntity/NewOpportunityComponent');
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(function (cmpRef) {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityId: s, ObjectTableName: 'Opportunity', BackButtonLabel: "Opportunity" });
                        });
                    }
                });
            }
        });
    };
    OpportunityMenuButtonsHandler.prototype.EditOpportunity = function () {
        var _this = this;
        this.Clone();
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Edit Opportunity";
        logWindow.WindowArgs = this.EntityPM;
        logWindow.WindowClosed.subscribe(function (event) {
            if (event == "cancle") {
                _this.RejectChanges();
            }
            _this.StopFlags();
        });
        logWindow.Show('./CRM/Components/MenuButtons/EditClosedOpportunityComponent');
    };
    OpportunityMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        if (!this.isButtonClicked) {
            this.StopFlags();
            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;
            switch (menuButton.EventCode) {
                case "Cancel":
                    {
                        this.CancelCommand();
                        break;
                    }
                case "CloseAsWon":
                    {
                        this.CloseAsWon();
                        break;
                    }
                case "CloseAsLost":
                    {
                        this.CloseAsLost();
                        break;
                    }
                case "ReOpen":
                    {
                        this.ReOpenOpoortunity();
                        break;
                    }
                case "Copy":
                    {
                        this.CopyOpportunity();
                        break;
                    }
                case "OpportunityTenantManagement":
                    {
                        this.LoadCustomer("manage");
                        break;
                    }
                case "OpportunityTotango":
                    {
                        this.LoadCustomer("totango");
                        break;
                    }
                case "OpportunityCreateTenant":
                    {
                        this.CreateTenantMethod();
                        break;
                    }
                case "Edit":
                    {
                        this.EditOpportunity();
                        break;
                    }
            }
        }
    };
    return OpportunityMenuButtonsHandler;
}());
exports.OpportunityMenuButtonsHandler = OpportunityMenuButtonsHandler;
//# sourceMappingURL=OpportunityMenuButtonsHandler.js.map