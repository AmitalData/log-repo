"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var Args_1 = require("../../Args");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var TicketValidator_1 = require("../../Validators/TicketValidator");
var CRMDomainService_1 = require("../../Services/CRMDomainService");
var TicketMenuButtonsHandler = /** @class */ (function () {
    function TicketMenuButtonsHandler() {
        this.status = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
    }
    TicketMenuButtonsHandler.prototype.SetEntityPM = function (entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    };
    TicketMenuButtonsHandler.prototype.ResetAllFlags = function () {
        this.isCancelled = false;
        this.isReactivate = false;
        this.isClosuerWindow = false;
    };
    TicketMenuButtonsHandler.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                        if (_this.isCancelled) {
                            _this.isCancelled = false;
                            _this.CancelActivateTicket();
                        }
                        if (_this.isReactivate) {
                            _this.isReactivate = false;
                            _this.CancelActivateTicket();
                        }
                        if (_this.isClosuerWindow) {
                            _this.isClosuerWindow = false;
                            var isValid = true;
                            var validator = new TicketValidator_1.TicketValidator();
                            var errors = validator.ValidateCurrenctEntity(_this.EntityPM);
                            if (errors != null && errors.length > 0) {
                                isValid = false;
                                if (_this.CurrentSession.CurrentEditComponent != null) {
                                    _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
                                }
                            }
                            if (isValid) {
                                _this.CheckTicketOwnerPermission();
                            }
                        }
                    }
                });
            }
            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    }
                });
            }
        }
    };
    TicketMenuButtonsHandler.prototype.CheckButtonState = function (menuButtons) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(function (d) { return d.Name === 'Ticket'; })[0];
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "Cancel":
                            {
                                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "TicketMore")) {
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
                                break;
                            }
                        case "Reactivate":
                            {
                                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "TicketMore")) {
                                    if (this.EntityPM.IsCancelled) {
                                        button.IsDisabled = false;
                                    }
                                    else {
                                        button.IsDisabled = true;
                                    }
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                                break;
                            }
                        case "ClosewithoutNotifying":
                            {
                                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "TicketMore")) {
                                    if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "TicketClosure")) {
                                        button.IsDisabled = true;
                                    }
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                                break;
                            }
                    }
                }
            }
        }
    };
    TicketMenuButtonsHandler.prototype.MenuButtonClick = function (menuButton) {
        switch (menuButton.EventCode) {
            case "Cancel":
                {
                    this.status = true;
                    this.ResetAllFlags();
                    this.isCancelled = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }
            case "Reactivate":
                {
                    this.status = false;
                    this.ResetAllFlags();
                    this.isReactivate = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }
            case "ClosewithoutNotifying":
                {
                    this.ResetAllFlags();
                    this.isClosuerWindow = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }
        }
    };
    // [Cancel Ticket]
    TicketMenuButtonsHandler.prototype.CancelActivateTicket = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        var title = "";
        if (this.status) {
            title = "Are you sure you want to cancel this Ticket ?";
        }
        else {
            title = "Are you sure you want to Re-active this Ticket ?";
        }
        confirmWindow.Show(title);
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                if (_this.entityArgs.EditComponent.ValidationErrorsList != null && _this.entityArgs.EditComponent.ValidationErrorsList.length == 0) {
                    if (_this.status) {
                        _this.EntityPM.IsCancelled = true;
                    }
                    else {
                        _this.EntityPM.IsCancelled = false;
                    }
                    _this.entityArgs.EditComponent.SaveChanges();
                }
            }
        });
    };
    // [Close Ticket]
    TicketMenuButtonsHandler.prototype.ClosuerWindow = function () {
        var _this = this;
        var windowTitle = "Ticket Closure";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        var args = new Args_1.TicketClosureArgs();
        args.Ticket = this.EntityPM;
        args.StageCode = "CS";
        logWindow.WindowArgs = args;
        logWindow.Show('./CRMModules/CRMTickets/Components/EditTabs/Others/TicketClosureComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    if (comp.IsOkClosed == true) {
                        _this.entityArgs.EditComponent.SaveChanges();
                    }
                }
            });
        });
    };
    TicketMenuButtonsHandler.prototype.CheckTicketOwnerPermission = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetTicketOwnerPermission(this.EntityPM.OwnerId, this.EntityPM.OwnerName).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ClosuerWindow();
            }
            else {
                _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    };
    return TicketMenuButtonsHandler;
}());
exports.TicketMenuButtonsHandler = TicketMenuButtonsHandler;
//# sourceMappingURL=TicketMenuButtonsHandler.js.map