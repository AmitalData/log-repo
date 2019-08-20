"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var InfraSettings_1 = require("../../../../../Infrastructure/Utilities/InfraSettings");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var SessionInfo_1 = require("../../../../../Infrastructure/Utilities/SessionInfo");
var CorrespondencePM_1 = require("../../../../../CRM/EntityPMs/CorrespondencePM");
var Args_1 = require("../../../../../CRM/Args");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var TicketStageListService_1 = require("../../../../../CRM/Services/StandardLists/TicketStageListService");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var ContactListService_1 = require("../../../../../Common/Services/StandardLists/ContactListService");
var CRMDomainService_1 = require("../../../../../CRM/Services/CRMDomainService");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var DocumentsFilingPM_1 = require("../../../../../Common/EntityPMs/DocumentsFilingPM");
var ServiceHelper_1 = require("../../../../../Infrastructure/Utilities/ServiceHelper");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var DocumentTypeListService_1 = require("../../../../../Common/Services/StandardLists/DocumentTypeListService");
var DocumentsFilingExtendedPMService_1 = require("../../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var UserListService_1 = require("../../../../../Common/Services/StandardLists/UserListService");
var CorrespondencePMService_1 = require("../../../../../CRM/Services/StandardPMs/CorrespondencePMService");
var TicketPMService_1 = require("../../../../../CRM/Services/StandardPMs/TicketPMService");
var Args_2 = require("../../../../../CRM/Args");
var SendEmailComponent = /** @class */ (function (_super) {
    __extends(SendEmailComponent, _super);
    function SendEmailComponent(_documentTypeListService, _documentsFilingExtendedPMService) {
        var _this = _super.call(this) || this;
        _this._documentTypeListService = _documentTypeListService;
        _this._documentsFilingExtendedPMService = _documentsFilingExtendedPMService;
        _this.ObjectTableName = "Correspondence";
        _this.DataContext = _this;
        _this.AttachmentsList = [];
        _this.UsersList = [];
        _this.OnCloseAttachmentDocsInEvent = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.documentTypeId = "";
        // Align Commands 
        _this.FlowDirection = "ltr";
        _this.BackgroundAlignRight = "transparent";
        _this.BackgroundAlignLeft = "transparent";
        _this.stageButtonContent = new Args_1.TicketStagesArgs();
        _this.stageButtonCode = "";
        _this.isOpened = false;
        _this.To = "";
        _this.ToList = [];
        _this.EmailsValidation = [];
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        _this.TicketObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Ticket"; })[0];
        return _this;
    }
    SendEmailComponent.prototype.ngOnInit = function () {
    };
    SendEmailComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.Ticket = args.Ticket;
            this.ContactEmail = this.Ticket.ContactEmail;
            this.IsInternal = args.IsInternal;
            this.contactId = this.Ticket.ContactId;
            this.InternalCorrespondenceLinesCount = args.InternalCorrespondenceLinesCount;
            this.Initialize();
        }
    };
    SendEmailComponent.prototype.Initialize = function () {
        this.InitializeLists();
        this.InitializeServices();
        this.CreateCorrespondence();
        this.InternalExternalEmailChecking();
        this.GetDocumentType();
        if (this.IsInternal) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.InternalUsers)) {
                this.BuildSaveStages();
            }
            else {
                this.BuildSendStages();
            }
        }
        else {
            this.BuildSendStages();
        }
        this.GetFlowDirection();
        this.RefreshTextAlgimentVariables();
    };
    SendEmailComponent.prototype.GetAllUsers = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        this.UserListService.getAllFromCache(filters).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.UsersList = myResponse.Result;
            }
        });
    };
    SendEmailComponent.prototype.InitializeLists = function () {
        this.AttachmentsList = [];
        this.UsersList = [];
        //this.InternalUsersSelectedList = [];
        //this.InternalUsersList = [];
        this.ToList = [];
        //this.CCsList = [];
        //this.ContactsList = [];
    };
    SendEmailComponent.prototype.InitializeServices = function () {
        this.ContactListService = new ContactListService_1.ContactListService();
        this.CRMDomainservice = new CRMDomainService_1.CRMDomainService();
        this.UserListService = new UserListService_1.UserListService();
    };
    SendEmailComponent.prototype.GetDocumentType = function () {
        var _this = this;
        var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = this.EntityPM.Tenant;
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (res) {
            if (!res.HasError) {
                var myResult = res.Result;
                var item = myResult.filter(function (d) { return d.Code == "USA"; })[0];
                _this.DocumentTypeId = item.Id;
            }
        });
    };
    Object.defineProperty(SendEmailComponent.prototype, "DocumentTypeId", {
        get: function () {
            return this.documentTypeId;
        },
        set: function (value) {
            this.documentTypeId = value;
        },
        enumerable: true,
        configurable: true
    });
    SendEmailComponent.prototype.CheckTicketCorrespondenceNumbers = function () {
        if (this.InternalCorrespondenceLinesCount == 1 && !this.EntityPM.IsInternal) {
            this.Ticket.FirstResponseTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        }
    };
    SendEmailComponent.prototype.CreateCorrespondence = function () {
        var _this = this;
        var isRightToLeft = false;
        if (SessionLocator_1.SessionLocator.TenantPM.IsCorrespondenceRightToLeftEnabled == true) {
            isRightToLeft = true;
        }
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new CorrespondencePM_1.CorrespondencePM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.EntityId = this.Ticket.Id;
        this.EntityPM.IsInternal = this.IsInternal;
        this.EntityPM.NotifyMe = false;
        this.EntityPM.NotifyOwner = true;
        this.EntityPM.Direction = "O";
        this.EntityPM.CreatedByContactId = SessionInfo_1.SessionInfo.LoggedUserId;
        this.ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
        this.EntityPM.ObjectTableId = this.ObjectTable.Id;
        this.EntityPM.RightToLeft = isRightToLeft;
    };
    Object.defineProperty(SendEmailComponent.prototype, "CorrespondenceLine", {
        get: function () { return this.correspondenceLine; },
        set: function (value) {
            if (this.correspondenceLine != value) {
                this.correspondenceLine = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendEmailComponent.prototype, "InternalUsers", {
        get: function () { return this.EntityPM.InternalUsers; },
        set: function (value) {
            if (this.EntityPM.InternalUsers != value) {
                this.EntityPM.InternalUsers = value;
                if (this.IsInternal) {
                    if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                        this.BuildSaveStages();
                    }
                    else {
                        this.BuildSendStages();
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendEmailComponent.prototype, "CCs", {
        get: function () { return this.EntityPM.CCs; },
        set: function (value) {
            if (this.EntityPM.CCs != value) {
                this.EntityPM.CCs = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendEmailComponent.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendEmailComponent.prototype, "NotifyMe", {
        get: function () { return this.EntityPM.NotifyMe; },
        set: function (newValue) {
            if (this.EntityPM.NotifyMe != newValue) {
                this.EntityPM.NotifyMe = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendEmailComponent.prototype, "NotifyOwner", {
        get: function () { return this.EntityPM.NotifyOwner; },
        set: function (newValue) {
            if (this.EntityPM.NotifyOwner != newValue) {
                this.EntityPM.NotifyOwner = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendEmailComponent.prototype, "ToVisibility", {
        get: function () {
            var myResult = true;
            if (this.IsInternal) {
                myResult = false;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendEmailComponent.prototype, "CcVisibility", {
        get: function () {
            var myResult = true;
            if (this.IsInternal) {
                myResult = false;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendEmailComponent.prototype, "NotifyOwnerVisibility", {
        get: function () {
            var myResult = true;
            var loggedId = SessionLocator_1.SessionLocator.LoggedUserId; // TenantContext.Current.LoggedContactId;
            if (this.Ticket.OwnerId == loggedId) {
                myResult = false;
                this.NotifyOwner = false;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendEmailComponent.prototype, "TextAlignRegionVisibility", {
        get: function () {
            var myResult = false;
            if (SessionLocator_1.SessionLocator.TenantPM.IsCorrespondenceRightToLeftEnabled == true) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    SendEmailComponent.prototype.GetFlowDirection = function () {
        var myResult = "ltr";
        if (SessionLocator_1.SessionLocator.TenantPM.IsCorrespondenceRightToLeftEnabled == true) {
            myResult = "rtl";
            if (this.EntityPM.RightToLeft) {
                myResult = "rtl";
            }
            else {
                myResult = "ltr";
            }
        }
        this.FlowDirection = myResult;
    };
    SendEmailComponent.prototype.GetBackgroundAlignRight = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FlowDirection)) {
            this.BackgroundAlignRight = this.FlowDirection == "rtl" ? "#FDD59D" : "transparent";
        }
    };
    SendEmailComponent.prototype.GetBackgroundAlignLeft = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FlowDirection)) {
            this.BackgroundAlignLeft = this.FlowDirection == "ltr" ? "#FDD59D" : "transparent";
        }
    };
    SendEmailComponent.prototype.AlignLeftClicked = function () {
        this.EntityPM.RightToLeft = false;
        this.GetFlowDirection();
        this.RefreshTextAlgimentVariables();
    };
    SendEmailComponent.prototype.AlignRightClicked = function () {
        this.EntityPM.RightToLeft = true;
        this.GetFlowDirection();
        this.RefreshTextAlgimentVariables();
    };
    SendEmailComponent.prototype.RefreshTextAlgimentVariables = function () {
        this.GetBackgroundAlignLeft();
        this.GetBackgroundAlignRight();
    };
    Object.defineProperty(SendEmailComponent.prototype, "StageButtonContent", {
        get: function () {
            return this.stageButtonContent;
        },
        set: function (value) {
            this.stageButtonContent = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendEmailComponent.prototype, "StageButtonCode", {
        get: function () {
            return this.stageButtonCode;
        },
        set: function (value) {
            this.stageButtonCode = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SendEmailComponent.prototype, "IsOpened", {
        get: function () {
            return this.isOpened;
        },
        set: function (value) {
            this.isOpened = value;
        },
        enumerable: true,
        configurable: true
    });
    SendEmailComponent.prototype.BuildSendStages = function () {
        this.StagesList = [];
        this.GetTicketStageMethod("Send and set as ");
    };
    SendEmailComponent.prototype.BuildSaveStages = function () {
        this.StagesList = [];
        this.GetTicketStageMethod("Save as ");
    };
    SendEmailComponent.prototype.GetTicketStageMethod = function (msg) {
        var _this = this;
        var myService = new TicketStageListService_1.TicketStageListService();
        myService.getAllFromCache().subscribe(function (resp) {
            if (!resp.HasError) {
                var stages = resp.Result;
                stages.forEach(function (item) {
                    var IsEnabled = true;
                    if ((!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "TicketClosure") && (item.Code == "RE" || item.Code == "CS")) ||
                        (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "SaveAsClosed") && item.Code == "CS") ||
                        (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "SaveAsResolved") && item.Code == "RE") ||
                        (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "SaveAsOpen") && item.Code == "OP")) {
                        IsEnabled = false;
                    }
                    var stage = new Args_1.TicketStagesArgs();
                    stage.Code = item.Code;
                    stage.Name = msg + item.Name;
                    stage.IsEnabled = IsEnabled;
                    stage.ItemOpacity = IsEnabled ? 1 : 0.5;
                    _this.StagesList.push(stage);
                    if (_this.Ticket.StageCode == stage.Code) {
                        _this.StageButtonContent = stage;
                    }
                });
                //this.StageButtonContent.Name = "Save as " + this.EntityPM.StageName;
                _this.StageButtonCode = _this.Ticket.StageCode;
            }
        });
    };
    SendEmailComponent.prototype.TicketStageSelected = function (option) {
        if (option != null) {
            this.StageButtonContent = option;
            this.StageButtonCode = option.Code;
            this.IsOpened = false;
            this.SendEmail();
        }
    };
    SendEmailComponent.prototype.SendEmail = function () {
        this.AddCorrespondenceLine(this.IsInternal);
    };
    SendEmailComponent.prototype.AddCorrespondenceLine = function (isInternal) {
        var _this = this;
        var errors = [];
        if (this.CCs != null) {
            //this.CCs = "";
            var CcEmails = [];
            this.CCs.split(';').forEach(function (item) {
                CcEmails.push(item);
            });
            CcEmails.forEach(function (item) {
                if (!_this.CheckIsValidEmails(item)) {
                    errors.push("\"" + item + "\" email address is not recognised.");
                }
            });
        }
        if (this.InternalUsers != null) {
            var InternalUsersEmails = [];
            this.InternalUsers.split(';').forEach(function (item) {
                InternalUsersEmails.push(item);
            });
            InternalUsersEmails.forEach(function (item) {
                if (!_this.CheckIsValidEmails(item)) {
                    errors.push("\"" + item + "\" email address is not recognised.");
                }
            });
        }
        this.EntityPM.Description = this.CorrespondenceLine;
        this.EntityPM.HTMLFullBody = this.CorrespondenceLine;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Bcc)) {
            this.EntityPM.Bcc = this.Bcc;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.InternalUsers)) {
            this.EntityPM.InternalUsers = this.InternalUsers;
        }
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.Description)) {
            errors.push("Description field is required");
        }
        this.ValidationErrorsList = errors;
        if (this.EmailsValidation != null && this.EmailsValidation.length > 0) {
            this.EmailsValidation.forEach(function (item) {
                if (_this.ValidationErrorsList == null) {
                    _this.ValidationErrorsList = [];
                }
                _this.ValidationErrorsList.push(item);
            });
        }
        if (this.ValidationErrorsList.length == 0) {
            //Update Ticket Stage 
            var myService = new CRMDomainService_1.CRMDomainService();
            myService.GetTicketOwnerPermission(this.Ticket.OwnerId, this.Ticket.OwnerName).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var myService = new TicketStageListService_1.TicketStageListService();
                    myService.getAll().subscribe(function (resp) {
                        if (!resp.HasError) {
                            var list = resp.Result;
                            var myStage = list.filter(function (d) { return d.Code == _this.StageButtonCode && d.Tenant == SessionLocator_1.SessionLocator.TenantPM.Id; })[0];
                            if (myStage != null) {
                                _this.Ticket.StageId = myStage.Id;
                                _this.Ticket.StageCode = myStage.Code;
                                _this.Ticket.StageName = myStage.Name;
                            }
                            _this.CheckTicketCorrespondenceNumbers(); // Fix Ticket First Resopnse Time 
                            // Ticket Ccs & Internal users 
                            var ccs = _this.AddNewEmails(_this.EntityPM.CCs, _this.Ticket.CCs);
                            var internals = _this.AddNewEmails(_this.EntityPM.InternalUsers, _this.Ticket.InternalUsers);
                            if (!Tools_1.AppTool.IsNullOrEmpty(ccs)) {
                                _this.Ticket.CCs = ccs;
                            }
                            if (!Tools_1.AppTool.IsNullOrEmpty(internals)) {
                                _this.Ticket.InternalUsers = internals;
                            }
                            _this.CreateAttachments();
                            if (errors.length == 0) {
                                if (_this.StageButtonCode != "CS") {
                                    _this.Ticket.IsClosed = false;
                                }
                                // Resolve Case or closed case
                                if (_this.StageButtonCode == "RE" || _this.StageButtonCode == "CS") {
                                    var currentDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                                    if (_this.StageButtonCode == "RE") {
                                        if (_this.Ticket.FirstResolveDate == null) {
                                            _this.Ticket.FirstResolveDate = currentDateTime;
                                        }
                                        if (_this.Ticket.FirstResponseTime == null) {
                                            _this.Ticket.FirstResponseTime = currentDateTime;
                                        }
                                        _this.Ticket.FullResolvedTime = currentDateTime;
                                    }
                                    if (_this.StageButtonCode == "CS") {
                                        if (_this.Ticket.FirstResolveDate == null) {
                                            _this.Ticket.FirstResolveDate = currentDateTime;
                                        }
                                        if (_this.Ticket.FullResolvedTime == null) {
                                            _this.Ticket.FullResolvedTime = currentDateTime;
                                        }
                                        if (_this.Ticket.FirstResponseTime == null) {
                                            _this.Ticket.FirstResponseTime = currentDateTime;
                                        }
                                        if (_this.Ticket.FirstCloseDate == null) {
                                            _this.Ticket.FirstCloseDate = currentDateTime;
                                        }
                                        _this.Ticket.LastCloseDate = currentDateTime;
                                    }
                                    _this.ClosuerWindow();
                                }
                                else {
                                    _this.SaveChanges();
                                }
                            }
                        }
                    });
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    SendEmailComponent.prototype.ClosuerWindow = function () {
        var _this = this;
        var windowTitle = "Ticket Closure";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        var args = new Args_2.TicketClosureArgs();
        args.Ticket = this.Ticket;
        args.StageCode = this.StageButtonCode;
        logWindow.WindowArgs = args;
        logWindow.Show('./CRMModules/CRMTickets/Components/EditTabs/Others/TicketClosureComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    if (comp.IsOkClosed == true) {
                        _this.SaveChanges();
                    }
                }
            });
        });
    };
    SendEmailComponent.prototype.AddNewEmails = function (correspondencelist, ticketlist) {
        var emails = "";
        var myList1 = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(correspondencelist)) {
            myList1 = correspondencelist.split(';');
        }
        var myList2 = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(ticketlist)) {
            myList2 = ticketlist.split(';');
        }
        var newList = [];
        // newList = myList1.Except(myList2);
        newList = myList1.filter(function (item) { return myList2.indexOf(item) < 0; });
        if (newList != null && newList.length > 0) {
            emails = newList.join(";");
        }
        return emails;
    };
    SendEmailComponent.prototype.SaveChanges = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetTicketOwnerPermission(this.Ticket.OwnerId, this.Ticket.OwnerName).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.CurrentSession.StartBusyIndicator("Sending");
                var myService = new CorrespondencePMService_1.CorrespondencePMService();
                myService.insert(_this.EntityPM).subscribe(function (myRespone) {
                    if (myRespone != null) {
                        if (!myRespone.HasError) {
                            _this.UpdateTicket();
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.ValidationErrorsList = myRespone.ErrorsArray;
                            _this.CurrentSession.StopBusyIndicator();
                        }
                    }
                });
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    };
    SendEmailComponent.prototype.UpdateTicket = function () {
        var _this = this;
        var myService = new TicketPMService_1.TicketPMService();
        myService.update(this.Ticket).subscribe(function (myRespone) {
            _this.CurrentSession.StopBusyIndicator();
            if (myRespone != null) {
                if (!myRespone.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
                else {
                    _this.ValidationErrorsList = myRespone.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    SendEmailComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    // Internal / External Email Process
    SendEmailComponent.prototype.InternalExternalEmailChecking = function () {
        if (this.IsInternal) {
            this.ToHeaderEnabled = true;
            //this.ContactEmail = "";
            this.EntityPM.InternalUsers = "";
            this.EntityPM.CCs = "";
        }
        if (!this.IsInternal) {
            this.ToHeaderEnabled = false;
            this.getContact(this.contactId);
            this.EntityPM.InternalUsers = this.Ticket.InternalUsers;
            this.EntityPM.CCs = this.Ticket.CCs;
        }
    };
    SendEmailComponent.prototype.getContact = function (contact) {
        var _this = this;
        this.ContactListService.getSingle(contact).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var contact = myResponse.Result;
                _this.To = contact.Email;
                _this.ToList.push(contact);
            }
        });
    };
    SendEmailComponent.prototype.CheckIsValidEmails = function (email) {
        var EMAIL_REGEXP = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var IsOk = true;
        if (email) {
            if (!EMAIL_REGEXP.test(email)) {
                IsOk = false;
                return;
            }
        }
        return IsOk;
    };
    SendEmailComponent.prototype.ValidateEmails = function (errors) {
        this.EmailsValidation = errors;
    };
    Object.defineProperty(SendEmailComponent.prototype, "AttachmentsVisibility", {
        // Attachements
        get: function () {
            var myResult = false;
            if (this.AttachmentsList.length > 0) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    SendEmailComponent.prototype.AttachInternalFile = function () {
        var _this = this;
        this._documentsFilingExtendedPMService.getDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable(this.Ticket.Id, null, this.TicketObjectTable.Id, "I", SessionLocator_1.SessionLocator.Tenant, true).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.documentInPMs = myResult;
                    if (_this.documentInPMs.length > 0) {
                        _this.IsCloseAttachmentDocsIn = false;
                        _this.OnCloseAttachmentDocsInEvent.subscribe(function ($event) {
                            if (!_this.IsCloseAttachmentDocsIn && $event) {
                                _this.IsCloseAttachmentDocsIn = true;
                                _this.BliudInternalAttachmentList($event);
                            }
                        });
                        var windowArgs = {};
                        windowArgs.DocumentsFilingList = _this.documentInPMs;
                        windowArgs.OnCloseAttachmentDocsInEvent = _this.OnCloseAttachmentDocsInEvent;
                        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                        logitudeWindow.Width = 800;
                        logitudeWindow.Height = 500;
                        logitudeWindow.Title = "Attach Docs In";
                        logitudeWindow.WindowArgs = windowArgs;
                        logitudeWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachmentDocsInComponent");
                    }
                    else {
                        _this.ShowMessage("No Docs In found");
                    }
                }
            }
        });
    };
    SendEmailComponent.prototype.BliudInternalAttachmentList = function (attachmentsLists) {
        var _this = this;
        if (attachmentsLists && attachmentsLists.length > 0) {
            attachmentsLists.forEach(function (item) {
                if (_this.AttachmentsList == null) {
                    _this.AttachmentsList = [];
                }
                var attach = new DocumentsFilingPM_1.DocumentsFilingPM();
                attach.FileExtension = item.FileExtension;
                attach.FileName = item.DocumentTypeCopyNameWithDocumentTypeName;
                attach.Tenant = item.Tenant;
                attach.Id = item.DocumentFilingId;
                attach.EntityId = item.EntityId;
                attach.FileSize = item.FileSize;
                _this.AttachmentsList.push(new AttachmentsArgs(attach));
            });
        }
    };
    SendEmailComponent.prototype.ShowMessage = function (message, title) {
        if (title === void 0) { title = ""; }
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
        if (title) {
            messageWindow.Title = title;
        }
    };
    SendEmailComponent.prototype.CreateAttachments = function () {
        var myList = [];
        this.AttachmentsList.forEach(function (item) {
            myList.push(item.DocumentFilingId);
        });
        this.EntityPM.Attachments = myList;
    };
    SendEmailComponent.prototype.AttachExternalFile = function () {
        var _this = this;
        //if (!this.CurrentDocument) {
        this._documentsFilingExtendedPMService.CreateDocumentsFiling(this.documentTypeId, this.EntityPM.EntityId, "", "", this.TicketObjectTable.Id, "I", SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.CurrentDocument = myResult;
                    _this.ShowUplpaderAttachment();
                }
            }
        });
        //}
        //else {
        //    this.ShowUplpaderAttachment();
        //}
    };
    SendEmailComponent.prototype.ShowUplpaderAttachment = function () {
        this.IsLoadUploader = true;
        var windowArgs = {};
        windowArgs.EntityId = this.EntityPM.EntityId;
        var objectTable = window.ObjectTables.filter(function (x) { return x.Name === "Ticket"; })[0];
        windowArgs.ObjectTableId = objectTable.Id;
        windowArgs.RequsetPageName = "SendDocument";
        windowArgs.TiggerViewModel = this;
        windowArgs.CurrentDocument = this.CurrentDocument;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 300;
        logitudeWindow.Title = "File Uploading";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachmentUploaderComponent");
    };
    SendEmailComponent.prototype.OnUploadComplete = function (uploader) {
        if (uploader.IsUploadDone && uploader.CurrentDocument) {
            if (this.AttachmentsList == null) {
                this.AttachmentsList = [];
            }
            if (uploader && uploader.CurrentDocument) {
                this.CurrentDocument = uploader.CurrentDocument;
                this.CurrentDocument.FileExtension = uploader.FileExtension;
                this.AttachmentsList.push(new AttachmentsArgs(this.CurrentDocument));
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SendEmailComponent.prototype, "OnCloseAttachmentDocsInEvent", void 0);
    SendEmailComponent = __decorate([
        core_1.Component({
            moduleId: './CRMModules/CRMTickets/Components/EditTabs/MainTab/',
            templateUrl: 'SendEmailComponent.html',
            providers: [DocumentTypeListService_1.DocumentTypeListService, DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService]
        }),
        __metadata("design:paramtypes", [DocumentTypeListService_1.DocumentTypeListService, DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService])
    ], SendEmailComponent);
    return SendEmailComponent;
}(BaseComponent_1.BaseComponent));
exports.SendEmailComponent = SendEmailComponent;
var AttachmentsArgs = /** @class */ (function () {
    function AttachmentsArgs(documentFilingPM) {
        this.DocumentFilingPM = documentFilingPM;
    }
    Object.defineProperty(AttachmentsArgs.prototype, "FileName", {
        get: function () { return this.DocumentFilingPM.FileName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AttachmentsArgs.prototype, "FileExtension", {
        get: function () { return this.DocumentFilingPM.FileExtension; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AttachmentsArgs.prototype, "Tenant", {
        get: function () { return this.DocumentFilingPM.Tenant; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AttachmentsArgs.prototype, "DocumentFilingId", {
        get: function () { return this.DocumentFilingPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AttachmentsArgs.prototype, "EntityId", {
        get: function () { return this.DocumentFilingPM.EntityId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AttachmentsArgs.prototype, "ObjectTableId", {
        get: function () { return this.DocumentFilingPM.ObjectTableId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AttachmentsArgs.prototype, "CreatedByUserId", {
        get: function () { return this.DocumentFilingPM.CreatedByUserId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AttachmentsArgs.prototype, "CreateDate", {
        get: function () { return this.DocumentFilingPM.CreateDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AttachmentsArgs.prototype, "OwnerId", {
        get: function () { return this.DocumentFilingPM.OwnerId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AttachmentsArgs.prototype, "UpdatedByUserId", {
        get: function () { return this.DocumentFilingPM.UpdatedByUserId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AttachmentsArgs.prototype, "UpdateDate", {
        get: function () { return this.DocumentFilingPM.UpdateDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AttachmentsArgs.prototype, "FileSize", {
        get: function () { return this.DocumentFilingPM.FileSize; },
        enumerable: true,
        configurable: true
    });
    AttachmentsArgs.prototype.ViewAttachment = function () {
        var documentSecurity = this.DocumentFilingPM.SecurityId;
        var link = "/WebPages/CorrespondenceDownloadpage.aspx?id=" + documentSecurity + "~" + this.Tenant;
        window.open(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + link);
    };
    return AttachmentsArgs;
}());
exports.AttachmentsArgs = AttachmentsArgs;
//# sourceMappingURL=SendEmailComponent.js.map