"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionInfo_1 = require("../Utilities/SessionInfo");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var DocumentOutPMService_1 = require("../../Common/Services/ExtendedPMs/DocumentOutPMService");
var EventTypeExtendedPMService_1 = require("../../Infrastructure/Services/ExtendedPMs/EventTypeExtendedPMService");
var Tools_1 = require("../../Infrastructure/Tools");
var DocumentsFilingExtendedPMService_1 = require("../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var FollowUpPM_1 = require("../../Infrastructure/EntityPMs/FollowUpPM");
var FollowUpPMExtendedService_1 = require("../Services/ExtendedPMs/FollowUpPMExtendedService");
var ServiceLocator_1 = require("../Locators/ServiceLocator");
var GeneralDocumentFollowUpHelper = /** @class */ (function () {
    function GeneralDocumentFollowUpHelper(objecttablename, entityId, childEntityId, childEntityReference, tabName, parentViewModel, entityPM) {
        this.MessageTotango = "";
        this.EventTypeCode = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        var table = window.ObjectTables.filter(function (d) { return d.Name == objecttablename; })[0];
        if (table) {
            this.CurrentObjectTableId = table.Id;
            this.ObjectTableName = table.Name;
        }
        this.CurrentEntityId = entityId;
        this.ChildEntityId = childEntityId;
        this.ChildEntityReference = childEntityReference;
        if (tabName == "DocOut") {
            this.EventTypeCode = "DOCO";
            this.MessageTotango = "Docs Out Adding FollowUp";
        }
        if (tabName == "DocIn") {
            this.EventTypeCode = "DOCI";
            this.MessageTotango = "Docs In Adding FollowUp";
        }
        this.TabName = tabName;
        this.ParentViewModel = parentViewModel;
        this.EntityPM = entityPM;
        this._documentOutPMService = new DocumentOutPMService_1.DocumentOutPMService();
        this._eventTypeExtendedPMService = new EventTypeExtendedPMService_1.EventTypeExtendedPMService();
        this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        this.followUpPMExtendedService = new FollowUpPMExtendedService_1.FollowUpPMExtendedService();
    }
    GeneralDocumentFollowUpHelper.prototype.AddFollowUp = function () {
        var _this = this;
        if (this.ParentViewModel && this.EntityPM && this.EntityPM.FollowUps) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, this.MessageTotango);
            //Add FollowUp
            if (!this.ParentViewModel.HasFollowUp) {
                this.CurrentSession.StartBusyIndicator("Please Wait...");
                this.GetEventTypeByCodeQuery();
            }
            //DeleteFollowUp
            else {
                var followup = null;
                if (this.EntityPM.FollowUps) {
                    this.CurrentSession.StartBusyIndicator("Saving...");
                    followup = this.EntityPM.FollowUps.filter(function (d) { return d.DocumentTypeId == _this.ParentViewModel.DocumentTypeId && d.Done == false; })[0];
                    if (followup) {
                        var items = this.EntityPM.FollowUps.filter(function (d) { return d.DocumentTypeId == _this.ParentViewModel.DocumentTypeId && d.Done == false; });
                        if (this.TabName != "DocOut" && items.length > 1 && this.ParentViewModel.CurrentDocument) {
                            followup = this.EntityPM.FollowUps.filter(function (d) { return d.DocumentTypeId == _this.ParentViewModel.DocumentTypeId && d.Done == false && d.ExternalDocumentId == _this.ParentViewModel.CurrentDocument.Id; })[0];
                            if (!followup) {
                                followup = this.EntityPM.FollowUps.filter(function (d) { return d.DocumentTypeId == _this.ParentViewModel.DocumentTypeId && d.Done == false && Tools_1.AppTool.IsNullOrEmpty(d.ExternalDocumentId); })[0];
                            }
                        }
                        if (followup) {
                            if (this.ObjectTableName == "Shipment") {
                                this.EntityPM.RemoveShipmentFollowUp(followup);
                            }
                            else if (this.ObjectTableName == "Quote") {
                                this.EntityPM.RemoveQuoteFollowUpPM(followup);
                            }
                        }
                        this.ParentViewModel.HasFollowUp = false;
                    }
                    this.CurrentSession.FireEvent("FollowupsChanged");
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        }
    };
    GeneralDocumentFollowUpHelper.prototype.MarkFollowUpAsDone = function () {
        var _this = this;
        if (this.EntityPM && this.EntityPM.FollowUps && this.ParentViewModel && this.ParentViewModel.HasFollowUp) {
            var followup = this.EntityPM.FollowUps.filter(function (d) { return d.DocumentTypeId == _this.ParentViewModel.DocumentTypeId && d.Done == false; })[0];
            if (followup) {
                var items = this.EntityPM.FollowUps.filter(function (d) { return d.DocumentTypeId == _this.ParentViewModel.DocumentTypeId && d.Done == false; });
                if (this.TabName != "DocOut" && items.length > 1 && this.ParentViewModel.CurrentDocument) {
                    followup = this.EntityPM.FollowUps.filter(function (d) { return d.DocumentTypeId == _this.ParentViewModel.DocumentTypeId && d.Done == false && d.ExternalDocumentId == _this.ParentViewModel.CurrentDocument.Id; })[0];
                    if (!followup) {
                        followup = this.EntityPM.FollowUps.filter(function (d) { return d.DocumentTypeId == _this.ParentViewModel.DocumentTypeId && d.Done == false && Tools_1.AppTool.IsNullOrEmpty(d.ExternalDocumentId); })[0];
                    }
                }
                if (followup) {
                    this.followUpPMExtendedService.RemoveFollowUpById(followup.Id).subscribe(function (res) {
                        var pmResponse = res;
                        _this.CurrentSession.StopBusyIndicator();
                        if (!pmResponse.HasError) {
                            _this.ParentViewModel.HasFollowUp = false;
                            _this.EntityPM.FollowUps = _this.EntityPM.FollowUps.filter(function (d) { return d.Id != followup.Id; });
                            _this.CurrentSession.FireEvent("FollowupsChanged");
                        }
                    });
                }
            }
        }
    };
    GeneralDocumentFollowUpHelper.prototype.GetEventTypeByCodeQuery = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EventTypeCode)) {
            this.CurrentSession.StartBusyIndicator("Please Wait...");
            this._eventTypeExtendedPMService.GetEventTypeByCode(this.EventTypeCode, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
                var pmResponse = res;
                _this.CurrentSession.StopBusyIndicator();
                if (!pmResponse.HasError) {
                    _this.EventType = pmResponse.Result;
                    if (_this.EventType) {
                        _this.ShowFollowUpControl();
                    }
                }
            });
        }
        else
            this.CurrentSession.StopBusyIndicator();
    };
    GeneralDocumentFollowUpHelper.prototype.GetNewInStanceFromFollowUp = function () {
        var newFollowUp = new FollowUpPM_1.FollowUpPM();
        if (this.EventType && this.ParentViewModel) {
            if (newFollowUp) {
                newFollowUp.DocumentTypeId = this.ParentViewModel.DocumentTypeId;
                newFollowUp.Area = this.TabName;
                newFollowUp.Notes = this.ParentViewModel.DocumentTypeName;
                newFollowUp.EventTypeId = this.EventType.Id;
                newFollowUp.IsNew = true;
                newFollowUp.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                newFollowUp.Deleted = false;
                newFollowUp.EventTypeFollowUpName = this.EventType.FollowUpEnglishName;
                newFollowUp.OwnerUserId = SessionInfo_1.SessionInfo.LoggedUserId;
                newFollowUp.Date = Tools_1.DateTool.GetCurrentDateAsUtc();
                newFollowUp.Done = false;
                if (this.ParentViewModel.CurrentDocument && this.TabName == "DocIn") {
                    newFollowUp.ExternalDocumentId = this.ParentViewModel.CurrentDocument.Id;
                }
            }
        }
        return newFollowUp;
    };
    GeneralDocumentFollowUpHelper.prototype.ShowFollowUpControl = function () {
        var _this = this;
        var newFollowUp = this.GetNewInStanceFromFollowUp();
        if (newFollowUp) {
            var windowArgs = {};
            windowArgs.ObjectTableName = this.ObjectTableName;
            windowArgs.CurrentFollowUp = newFollowUp;
            windowArgs.EntityPM = this.EntityPM;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 350;
            logWindow.Height = 400;
            logWindow.Title = "New Follow Up";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./Infrastructure/Components/LogitudeComponents/Followups/AddDocumentFollowupComponent");
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event == "AddFollowUpSucceeded") {
                    _this.ParentViewModel.HasFollowUp = true;
                }
                else
                    _this.ParentViewModel.HasFollowUp = false;
            });
        }
    };
    return GeneralDocumentFollowUpHelper;
}());
exports.GeneralDocumentFollowUpHelper = GeneralDocumentFollowUpHelper;
//# sourceMappingURL=GeneralDocumentFollowUpHelper.js.map