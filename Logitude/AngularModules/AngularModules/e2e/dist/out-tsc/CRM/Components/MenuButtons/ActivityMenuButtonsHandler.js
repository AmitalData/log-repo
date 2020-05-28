"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ActivityPM_1 = require("../../EntityPMs/ActivityPM");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ActivityStatusListService_1 = require("../../Services/StandardLists/ActivityStatusListService");
var ActivityValidator_1 = require("../../Validators/ActivityValidator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var ActivityInviteePM_1 = require("../../EntityPMs/ActivityInviteePM");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var Args_1 = require("../../Args");
var ActivityMenuButtonsHandler = /** @class */ (function () {
    function ActivityMenuButtonsHandler() {
        this.status = false;
        this.isValid = false;
        this.isCompleteActivity = false;
        this.isCopyActivity = false;
    }
    ActivityMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.InitializeServices();
        this.Listen();
    };
    ActivityMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'Ticket'; })[0];
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "Cancel":
                            {
                                if (this.EntityPM.ActivityStatusCode == "X") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "MarkAsComplete":
                            {
                                if (this.EntityPM.ActivityStatusCode == "X") {
                                    button.IsDisabled = true;
                                }
                                else if (this.EntityPM.ActivityTypeCode == "EO" || this.EntityPM.ActivityTypeCode == "EI") {
                                    button.IsDisabled = true;
                                }
                                else if (!this.EntityPM.IsOpen) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "ReOpen":
                            {
                                if (this.EntityPM.ActivityStatusCode == "X") {
                                    button.IsDisabled = true;
                                }
                                else if (this.EntityPM.ActivityTypeCode == "EO" || this.EntityPM.ActivityTypeCode == "EI") {
                                    button.IsDisabled = true;
                                }
                                else if (this.EntityPM.IsOpen) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "Copy":
                            {
                                if (this.EntityPM.ActivityStatusCode == "X") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                    }
                }
            }
        }
    };
    ActivityMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        switch (menuButton.EventCode) {
            case "Cancel":
                {
                    this.CancelActivity();
                    break;
                }
            case "MarkAsComplete":
                {
                    this.isCompleteActivity = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }
            case "ReOpen":
                {
                    this.ReOpenActivity();
                    break;
                }
            case "Copy":
                {
                    this.isCopyActivity = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }
        }
    };
    ActivityMenuButtonsHandler.prototype.InitializeServices = function () {
        this.ActivityStatusListService = new ActivityStatusListService_1.ActivityStatusListService();
    };
    ActivityMenuButtonsHandler.prototype.StopFlags = function () {
        this.isCompleteActivity = false;
        this.isCopyActivity = false;
    };
    ActivityMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.isCompleteActivity) {
                        _this.CompleteActivity();
                    }
                    if (_this.isCopyActivity) {
                        _this.CopyActivity();
                    }
                }
                _this.StopFlags();
            });
        }
        this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
            if (isLoadSuccess) {
                _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
            }
            _this.StopFlags();
        });
    };
    ActivityMenuButtonsHandler.prototype.Validate = function () {
        var validator = new ActivityValidator_1.ActivityValidator();
        var errors = validator.Validate(this.EntityPM);
        this.isValid = errors.length == 0 ? true : false;
        this.entityArgs.EditComponent.ValidationErrorsList = errors;
        if (!this.isValid) {
            this.StopFlags();
        }
    };
    ActivityMenuButtonsHandler.prototype.CancelActivity = function () {
        var _this = this;
        var confirmMsg = "Are you sure you want to cancel this activity?";
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(confirmMsg);
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.Validate();
                if (_this.isValid) {
                    _this.CompleteSave('X');
                }
            }
        });
    };
    ActivityMenuButtonsHandler.prototype.CompleteActivity = function () {
        this.CompleteSave('C');
    };
    ActivityMenuButtonsHandler.prototype.ReOpenActivity = function () {
        this.Validate();
        if (this.isValid) {
            this.CompleteSave('N');
        }
    };
    ActivityMenuButtonsHandler.prototype.CopyActivity = function () {
        var newActivity = new ActivityPM_1.ActivityPM();
        newActivity.Tenant = this.EntityPM.Tenant;
        newActivity.ActivityTypeCode = this.EntityPM.ActivityTypeCode;
        newActivity.ActivityTypeName = this.EntityPM.ActivityTypeName;
        newActivity.Subject = this.EntityPM.Subject;
        newActivity.OwnerId = this.EntityPM.OwnerId;
        newActivity.OwnerName = this.EntityPM.OwnerName;
        newActivity.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newActivity.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newActivity.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newActivity.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        ;
        newActivity.IsOpen = true;
        newActivity.ActivityStatusCode = "N";
        newActivity.PriorityCode = this.EntityPM.PriorityCode;
        newActivity.ActivityStatusName = this.EntityPM.ActivityStatusName;
        newActivity.BranchId = this.EntityPM.BranchId;
        newActivity.AllDayEvent = this.EntityPM.AllDayEvent;
        newActivity.IsLeftVoiceMail = this.EntityPM.IsLeftVoiceMail;
        newActivity.NeedSynchronization = this.EntityPM.NeedSynchronization;
        newActivity.BusinessUnitId = this.EntityPM.BusinessUnitId;
        newActivity.PriorityName = this.EntityPM.PriorityName;
        newActivity.Description = this.EntityPM.Description;
        newActivity.CustomerId = this.EntityPM.CustomerId;
        newActivity.CustomerName = this.EntityPM.CustomerName;
        newActivity.OpportunityId = this.EntityPM.OpportunityId;
        newActivity.QuoteId = this.EntityPM.QuoteId;
        newActivity.Notes = this.EntityPM.Notes;
        newActivity.IsCopy = true;
        newActivity.OriginalActivitySubject = this.EntityPM.Subject;
        //appointment fields
        newActivity.Location = this.EntityPM.ActivityTypeCode == "AP" ? this.EntityPM.Location : null;
        newActivity.ActivityTimeTypeCode = this.EntityPM.ActivityTypeCode == "AP" ? this.EntityPM.ActivityTimeTypeCode : null;
        newActivity.Duration = this.EntityPM.ActivityTypeCode == "AP" ? this.EntityPM.Duration : null;
        //phone fields
        newActivity.CallTypeCode = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.CallTypeCode : null;
        newActivity.CallPurpose = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.CallPurpose : null;
        newActivity.CallDetails = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.CallDetails : null;
        newActivity.CallResult = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.CallResult : null;
        newActivity.CallWithId = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.CallWithId : null;
        newActivity.PhoneNumber = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.PhoneNumber : null;
        newActivity.SenderEmail = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.SenderEmail : null;
        newActivity.SenderContactId = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.SenderContactId : null;
        newActivity.SenderContactName = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.SenderContactName : null;
        if (newActivity.ActivityTypeCode == "AP") {
            this.EntityPM.ActivityInvitees.forEach(function (item) {
                var activity = new ActivityInviteePM_1.ActivityInviteePM(null);
                activity.ContactId = item.ContactId;
                activity.ContactName = item.ContactName;
                activity.Tenant = newActivity.Tenant;
                activity.Email = item.Email;
                activity.IsRequired = item.IsRequired;
                newActivity.ActivityInvitees.push(activity);
            });
        }
        var windowTitle = "";
        var windowTitleIcon = "";
        var path = "";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        switch (newActivity.ActivityTypeCode) {
            case "TS":
                {
                    windowTitle = "Copy Task";
                    windowTitleIcon = "./Images/Activities/TS.png";
                    //logWindow.ShowActivityTitle = true;
                    //logWindow.ActivityTypeCode = "TS";
                    //logWindow.IsCopyCase = true;
                    break;
                }
            case "CL": {
                windowTitle = "Copy Phone Call";
                windowTitleIcon = "./Images/Activities/CL.png";
                break;
            }
            case "AP": {
                windowTitle = "Copy Appointment";
                windowTitleIcon = "./Images/Activities/AP.png";
                logWindow.Width = 800;
                logWindow.Height = 600;
                break;
            }
            default: {
                break;
            }
        }
        var windowArgs = new Args_1.ActivityInputArgs();
        windowArgs.TypeCode = newActivity.ActivityTypeCode;
        windowArgs.IsAddCustomerAllowed = true;
        windowArgs.Activity = newActivity;
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivityComponent');
    };
    ActivityMenuButtonsHandler.prototype.CompleteSave = function (type) {
        var _this = this;
        this.EntityPM.IsOpen = true;
        this.EntityPM.ActivityStatusCode = type;
        this.ActivityStatusListService.getAllFromCache().subscribe(function (resp) {
            if (!resp.HasError) {
                var list = resp.Result;
                var activity = list.filter(function (d) { return d.Code == _this.EntityPM.ActivityStatusCode; })[0];
                if (activity != null) {
                    _this.EntityPM.ActivityStatusName = activity.Name;
                }
                _this.entityArgs.EditComponent.SaveChanges(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
            }
        });
    };
    return ActivityMenuButtonsHandler;
}());
exports.ActivityMenuButtonsHandler = ActivityMenuButtonsHandler;
//# sourceMappingURL=ActivityMenuButtonsHandler.js.map