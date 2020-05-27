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
var FollowUpPM_1 = require("../../../EntityPMs/FollowUpPM");
var QuoteFollowUpPM_1 = require("../../../../Quote/EntityPMs/QuoteFollowUpPM");
var ShipmentFollowUpPM_1 = require("../../../../Shipment/EntityPMs/ShipmentFollowUpPM");
var Tools_1 = require("../../../Tools");
var BaseComponent_1 = require("../BaseComponent");
var Validator_1 = require("../../../Validators/Validator");
var SessionLocator_1 = require("../../../Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Utilities/FeatureLocator");
var EventTypeListService_1 = require("../../../Services/StandardLists/EventTypeListService");
var EntityResourceService_1 = require("../../../Services/EntityResourceService");
var Cloner_1 = require("../../../Utilities/Cloner");
var EntityArgs_1 = require("../../../DataContracts/EntityArgs");
var HelperFollowups = /** @class */ (function (_super) {
    __extends(HelperFollowups, _super);
    function HelperFollowups(entityArgs, entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.entityResourceService = entityResourceService;
        _this.ComponentId = null;
        _this.ComponentButtonId = null;
        _this.ComponentContentId = null;
        _this.IsEnabled = true;
        _this.Width = 350;
        _this.Height = 130;
        _this.QuotePM = null;
        _this.ShipmentPM = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "FollowUp";
        _this.ItemsSource = [];
        _this.EventTypes = [];
        _this.IsResourcesReady = false;
        _this.IsComponentVisible = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.FollowupsChangedEvent = null;
        _this.isOpened = false;
        _this.IsMouseOver = false;
        _this.IsMouseOverButton = false;
        _this.IsMouseOverInputBox = false;
        _this.IsInputBoxFocused = false;
        _this.IsAddViewVisible = false;
        _this.IsEditViewVisible = false;
        _this.IsNormalViewVisible = true;
        var idIndex = _this.CurrentSession.GetNewId("HelperFollowups");
        _this.ComponentId = "HelperFollowups_" + idIndex;
        _this.ComponentButtonId = "HelperFollowupsButton_" + idIndex;
        _this.ComponentContentId = "HelperFollowupsContent_" + idIndex;
        return _this;
    }
    HelperFollowups.prototype.Listen = function () {
        var _this = this;
        if (!this.FollowupsChangedEvent) {
            this.FollowupsChangedEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "FollowupsChanged") {
                    _this.BuildItemsSource();
                }
            });
        }
        if (this.entityArgs) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    if (_this.QuotePM) {
                        _this.QuotePM = _this.entityArgs.EditComponent.EntityPM;
                    }
                    else {
                        _this.ShipmentPM = _this.entityArgs.EditComponent.EntityPM;
                    }
                    _this.BuildItemsSource();
                }
            });
        }
    };
    HelperFollowups.prototype.ngOnInit = function () {
        var _this = this;
        if (this.QuotePM != null) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Quote", "Quote.Followups")) {
                this.IsComponentVisible = true;
            }
        }
        else if (this.ShipmentPM != null) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Followups")) {
                this.IsComponentVisible = true;
            }
        }
        if (this.IsComponentVisible) {
            this.entityResourceService.getEntityResourceByTableName("FollowUp").subscribe(function (res) {
                _this.Listen();
                _this.SetIconPath();
                _this.SetPopupHeight();
                _this.BuildItemsSource();
                _this.IsResourcesReady = true;
                var entityId = null;
                var entityObjectTableId = null;
                var entityObjectTableName = null;
                if (_this.QuotePM != null) {
                    entityId = _this.QuotePM.Id;
                    entityObjectTableName = "Quote";
                }
                else if (_this.ShipmentPM != null) {
                    entityId = _this.ShipmentPM.Id;
                    entityObjectTableName = _this.ShipmentPM.ShipmentLevelCode == "C" ? "Master" : "Shipment";
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(entityObjectTableName)) {
                    var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === entityObjectTableName; })[0];
                    if (ObjectTable) {
                        entityObjectTableId = ObjectTable.Id;
                        var myService = new EventTypeListService_1.EventTypeListService();
                        myService.getAll().subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var lists = myResponse.Result;
                                _this.EventTypes = lists.filter(function (f) { return f.ObjectTableId == entityObjectTableId && f.ManualActivatedFollowUp == true; });
                            }
                        });
                    }
                }
            });
        }
    };
    HelperFollowups.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.FollowupsChangedEvent);
        if (this.IsComponentVisible) {
            this.CurrentSession.FireEvent("FollowupsChangedMainMenu");
        }
        this.StopPositionTimer();
    };
    HelperFollowups.prototype.SetIconPath = function () {
        if (this.ItemsSource.length == 0) {
            this.IconPath = "./_Resources/Images/Icons/Followups/Followup.png";
        }
        else {
            if (this.ItemsSource.filter(function (f) { return f.IsOld == true; }).length > 0) {
                this.IconPath = "./_Resources/Images/Icons/Followups/Followup_Red.png";
            }
            else {
                this.IconPath = "./_Resources/Images/Icons/Followups/Followup_Black.png";
            }
        }
    };
    HelperFollowups.prototype.SetPopupHeight = function () {
        var PopupHeight = 130;
        if (this.IsAddViewVisible) {
            PopupHeight = 230;
        }
        else if (this.IsEditViewVisible) {
            PopupHeight = 205;
        }
        else {
            var itemsCount = 0;
            if (this.QuotePM) {
                itemsCount = this.QuotePM.FollowUps.length;
            }
            else if (this.ShipmentPM) {
                itemsCount = this.ShipmentPM.FollowUps.length;
            }
            if (itemsCount > 3) {
                if (itemsCount > 5) {
                    itemsCount = 5;
                }
                PopupHeight = (itemsCount * 34) + 25 + 3;
            }
        }
        this.Height = PopupHeight;
    };
    Object.defineProperty(HelperFollowups.prototype, "IsOpened", {
        get: function () { return this.isOpened; },
        set: function (value) {
            if (value != undefined) {
                if (this.isOpened != value) {
                    this.isOpened = value;
                    if (value) {
                        this.SetPopupHeight();
                        this.RunPositionTimer();
                        this.BuildItemsSource();
                    }
                    else {
                        this.StopPositionTimer();
                        if (this.AddDataContext || this.EditDataContext) {
                            if (this.EditDataContext) {
                                this.EditDataContext.RejectChanges();
                            }
                            this.CloseAddEdit();
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    HelperFollowups.prototype.StopPositionTimer = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    };
    HelperFollowups.prototype.RunPositionTimer = function () {
        var _this = this;
        this.StopPositionTimer();
        this.timerToken = setInterval(function () { return _this.CalculateFixedPosition(); }, 0);
    };
    HelperFollowups.prototype.CalculateFixedPosition = function () {
        var item = document.getElementById(this.ComponentId);
        if (item) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.ComponentContentId).style.top = (itemRect.top + 24) + 'px';
            document.getElementById(this.ComponentContentId).style.left = (itemRect.left + 40 - this.Width) + 'px';
        }
    };
    HelperFollowups.prototype.OnButtonClicked = function () {
        if (this.IsOpened) {
            this.StopPositionTimer();
            this.IsOpened = false;
        }
        else {
            this.CalculateFixedPosition();
            this.IsOpened = true;
        }
    };
    HelperFollowups.prototype.OnButtonLostFocus = function () {
        if (!this.IsMouseOverButton) {
            if (this.IsMouseOver || this.IsMouseOverInputBox) {
                if (!this.IsMouseOverInputBox) {
                    document.getElementById(this.ComponentButtonId).focus();
                }
            }
            else {
                this.IsOpened = false;
            }
        }
    };
    HelperFollowups.prototype.OnInputBoxLostFocus = function () {
        console.log('OnInputBoxLostFocus');
        if (!this.IsMouseOverButton) {
            if (this.IsMouseOver) {
                document.getElementById(this.ComponentButtonId).focus();
            }
            else {
                this.IsOpened = false;
            }
        }
        //if (this.IsInputBoxFocused) {
        //    this.IsInputBoxFocused = false;
        //    if (!this.IsMouseOverButton) {
        //        if (this.IsMouseOver) {
        //            document.getElementById(this.ComponentButtonId).focus();
        //        }
        //        else {
        //            this.IsOpened = false;
        //        }
        //    }
        //}
    };
    HelperFollowups.prototype.InitAllViews = function () {
        this.IsAddViewVisible = false;
        this.IsEditViewVisible = false;
        this.IsNormalViewVisible = false;
    };
    HelperFollowups.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        if (this.QuotePM) {
            this.QuotePM.FollowUps.filter(function (f) { return f.Done == false; }).forEach(function (itemPM) {
                _this.ItemsSource.push(new HelperFollowup(itemPM));
            });
        }
        else if (this.ShipmentPM) {
            this.ShipmentPM.FollowUps.filter(function (f) { return f.Done == false; }).forEach(function (itemPM) {
                _this.ItemsSource.push(new HelperFollowup(itemPM));
            });
        }
        this.SetIconPath();
    };
    HelperFollowups.prototype.AddFollowupClicked = function () {
        this.InitAllViews();
        this.IsAddViewVisible = true;
        this.SetPopupHeight();
        this.AddDataContext = new AddDataContext(this);
    };
    HelperFollowups.prototype.EditFollowupClicked = function (item) {
        this.InitAllViews();
        this.IsEditViewVisible = true;
        this.SetPopupHeight();
        this.EditDataContext = new EditDataContext(item, this);
    };
    HelperFollowups.prototype.CloseAddEdit = function () {
        this.InitAllViews();
        this.IsNormalViewVisible = true;
        this.SetPopupHeight();
        this.AddDataContext = null;
        this.EditDataContext = null;
        this.IsMouseOver = false;
        this.IsMouseOverButton = false;
        this.IsMouseOverInputBox = false;
        this.IsInputBoxFocused = false;
        this.BuildItemsSource();
        document.getElementById(this.ComponentButtonId).focus();
    };
    HelperFollowups = __decorate([
        core_1.Component({
            selector: "HelperFollowups",
            moduleId: module.id,
            templateUrl: './HelperFollowups.html',
            inputs: ['QuotePM', 'ShipmentPM', 'IsEnabled'],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], HelperFollowups);
    return HelperFollowups;
}(BaseComponent_1.BaseComponent));
exports.HelperFollowups = HelperFollowups;
var HelperFollowup = /** @class */ (function () {
    function HelperFollowup(entityPM) {
        this.entityPM = entityPM;
        this.ItemId = null;
        this.ItemTooltipId = null;
        this.Done = false;
        this.Date = null;
        this.Name = null;
        this.Notes = null;
        this.IconPath = null;
        this.TextColor = null;
        this.IsOld = false;
        this.Background = "white";
        this.ListItemHeight = 34;
        this.TooltipHeight = 130;
        this.TooltipWidth = 270;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsShowTooltip = false;
        var idIndex = this.CurrentSession.GetNewId("FollowupItem");
        this.ItemId = "FollowupItem_" + idIndex;
        this.ItemTooltipId = "FollowupItemTooltip_" + idIndex;
        this.Done = entityPM.Done;
        this.Date = entityPM.Date;
        this.Name = entityPM.EventTypeFollowUpName;
        this.Notes = entityPM.Note;
        this.TextColor = this.Done ? '#8F9293' : '#292E30';
        if (this.Date) {
            if (Tools_1.DateTool.GetDateParts(this.Date).DateTicks < Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
                this.IsOld = true;
                this.Background = "#F7E3E3";
            }
        }
        this.SetIconPath();
    }
    HelperFollowup.prototype.SetIconPath = function () {
        var iconPath = "./_Resources/Images/Icons/Followups/Document.png";
        if (this.Done) {
            iconPath = "./_Resources/Images/Icons/Followups/Done.png";
        }
        else if (this.Name) {
            var name = this.Name.toLowerCase();
            if (name.indexOf("arrived") > -1 || name.indexOf("departed") > -1 || name.indexOf("departure") > -1 || name.indexOf("arrival") > -1) {
                iconPath = "./_Resources/Images/Icons/Followups/Routing.png";
            }
            else if (name.indexOf("reminder") > -1 || name.indexOf("arranged") > -1) {
                iconPath = "./_Resources/Images/Icons/Followups/Reminder.png";
            }
        }
        this.IconPath = iconPath;
    };
    HelperFollowup.prototype.ShowTooltip = function (isShowTooltip) {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Notes)) {
            if (isShowTooltip) {
                var item = document.getElementById(this.ItemId);
                var itemRect = item.getBoundingClientRect();
                document.getElementById(this.ItemTooltipId).style.top = (itemRect.top - (this.TooltipHeight / 2) + (this.ListItemHeight / 2)) + 'px';
                document.getElementById(this.ItemTooltipId).style.left = (itemRect.left - this.TooltipWidth + 5) + 'px';
            }
            this.IsShowTooltip = isShowTooltip;
        }
    };
    return HelperFollowup;
}());
exports.HelperFollowup = HelperFollowup;
var AddDataContext = /** @class */ (function (_super) {
    __extends(AddDataContext, _super);
    function AddDataContext(father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.ObjectTableName = "FollowUp";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new FollowUpPM_1.FollowUpPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.OwnerUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.EntityPM.Done = false;
        _this.EntityPM.IsNew = true;
        _this.EntityPM.Deleted = false;
        _this.SelectedEventType = _this.father.EventTypes.filter(function (f) { return f.Code == "REMF"; })[0];
        if (_this.SelectedEventType) {
            _this.ManualActivatedFollowUp = false;
        }
        return _this;
    }
    Object.defineProperty(AddDataContext.prototype, "SelectedEventType", {
        get: function () { return this.selectedEventType; },
        set: function (value) {
            if (this.selectedEventType != value) {
                this.selectedEventType = value;
                var eventTypeId = null;
                var eventTypeName = null;
                if (value) {
                    eventTypeId = value.Id;
                    eventTypeName = value.EnglishName;
                }
                this.EventTypeId = eventTypeId;
                this.EventTypeFollowUpName = eventTypeName;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddDataContext.prototype, "EventTypeId", {
        get: function () { return this.EntityPM.EventTypeId; },
        set: function (value) {
            if (this.EntityPM.EventTypeId != value) {
                this.EntityPM.EventTypeId = value;
                this.UIProperties.SetRequired("EventTypeId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(value) ? true : false);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddDataContext.prototype, "EventTypeFollowUpName", {
        get: function () { return this.EntityPM.EventTypeFollowUpName; },
        set: function (value) {
            if (this.EntityPM.EventTypeFollowUpName != value) {
                this.EntityPM.EventTypeFollowUpName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddDataContext.prototype, "ManualActivatedFollowUp", {
        get: function () { return this.EntityPM.ManualActivatedFollowUp; },
        set: function (value) {
            if (this.EntityPM.ManualActivatedFollowUp != value) {
                this.EntityPM.ManualActivatedFollowUp = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddDataContext.prototype, "Date", {
        get: function () { return this.EntityPM.Date; },
        set: function (value) {
            if (this.EntityPM.Date != value) {
                this.EntityPM.Date = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddDataContext.prototype, "OwnerUserId", {
        get: function () { return this.EntityPM.OwnerUserId; },
        set: function (value) {
            if (this.EntityPM.OwnerUserId != value) {
                this.EntityPM.OwnerUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddDataContext.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddDataContext.prototype.NumericButtonClicked = function (isIncreas) {
        if (this.Date == null) {
            this.Date = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        else {
            var day = isIncreas ? 1 : -1;
            var myDateParts = Tools_1.DateTool.GetDateParts(this.Date);
            var dateObject = new Date();
            dateObject.setUTCMonth(0);
            dateObject.setUTCDate(1);
            dateObject.setUTCFullYear(myDateParts.Year);
            dateObject.setUTCMonth((myDateParts.Month - 1));
            dateObject.setUTCDate(myDateParts.Day + day);
            dateObject.setUTCHours(myDateParts.Hours);
            dateObject.setUTCMinutes(myDateParts.Minutes);
            dateObject.setUTCSeconds(myDateParts.Seconds);
            dateObject.setUTCMilliseconds(myDateParts.Milliseconds);
            this.Date = dateObject;
        }
    };
    AddDataContext.prototype.CancelButtonClicked = function () {
        this.father.CloseAddEdit();
    };
    AddDataContext.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (errors.length == 0) {
            if (this.father.QuotePM) {
                var myQuoteFollowUpPM = new QuoteFollowUpPM_1.QuoteFollowUpPM(null);
                myQuoteFollowUpPM.Tenant = this.father.QuotePM.Tenant;
                myQuoteFollowUpPM.QuoteId = this.father.QuotePM.Id;
                myQuoteFollowUpPM.EventTypeId = this.EventTypeId;
                myQuoteFollowUpPM.EventTypeFollowUpName = this.EventTypeFollowUpName;
                myQuoteFollowUpPM.ManualActivatedFollowUp = this.ManualActivatedFollowUp;
                myQuoteFollowUpPM.Date = this.Date;
                myQuoteFollowUpPM.OwnerUserId = this.OwnerUserId;
                myQuoteFollowUpPM.Note = this.Notes;
                myQuoteFollowUpPM.Done = this.EntityPM.Done;
                myQuoteFollowUpPM.IsNew = this.EntityPM.IsNew;
                this.father.QuotePM.AddQuoteFollowUpPM(myQuoteFollowUpPM);
            }
            else if (this.father.ShipmentPM) {
                var myShipmentFollowUpPM = new ShipmentFollowUpPM_1.ShipmentFollowUpPM(null);
                myShipmentFollowUpPM.Tenant = this.father.ShipmentPM.Tenant;
                myShipmentFollowUpPM.ShipmentId = this.father.ShipmentPM.Id;
                myShipmentFollowUpPM.EventTypeId = this.EventTypeId;
                myShipmentFollowUpPM.EventTypeFollowUpName = this.EventTypeFollowUpName;
                myShipmentFollowUpPM.ManualActivatedFollowUp = this.ManualActivatedFollowUp;
                myShipmentFollowUpPM.Date = this.Date;
                myShipmentFollowUpPM.OwnerUserId = this.OwnerUserId;
                myShipmentFollowUpPM.Note = this.Notes;
                myShipmentFollowUpPM.Done = this.EntityPM.Done;
                myShipmentFollowUpPM.IsNew = this.EntityPM.IsNew;
                this.father.ShipmentPM.AddShipmentFollowUp(myShipmentFollowUpPM);
            }
            this.father.CloseAddEdit();
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    };
    return AddDataContext;
}(BaseComponent_1.BaseComponent));
exports.AddDataContext = AddDataContext;
var EditDataContext = /** @class */ (function (_super) {
    __extends(EditDataContext, _super);
    function EditDataContext(item, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.Name = null;
        _this.IconPath = null;
        _this.EntityPM = null;
        _this.ObjectTableName = "FollowUp";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Name = item.Name;
        _this.IconPath = item.IconPath;
        _this.EntityPM = item.entityPM;
        _this.Clone();
        return _this;
    }
    Object.defineProperty(EditDataContext.prototype, "Date", {
        get: function () { return this.EntityPM.Date; },
        set: function (value) {
            if (this.EntityPM.Date != value) {
                this.EntityPM.Date = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditDataContext.prototype, "OwnerUserId", {
        get: function () { return this.EntityPM.OwnerUserId; },
        set: function (value) {
            if (this.EntityPM.OwnerUserId != value) {
                this.EntityPM.OwnerUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditDataContext.prototype, "Notes", {
        get: function () { return this.EntityPM.Note; },
        set: function (value) {
            if (this.EntityPM.Note != value) {
                this.EntityPM.Note = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditDataContext.prototype, "Done", {
        get: function () { return this.EntityPM.Done; },
        set: function (value) {
            if (this.EntityPM.Done != value) {
                this.EntityPM.Done = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditDataContext.prototype, "DoneDateTime", {
        get: function () { return this.EntityPM.DoneDateTime; },
        set: function (value) {
            if (this.EntityPM.DoneDateTime != value) {
                this.EntityPM.DoneDateTime = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditDataContext.prototype, "DoneNote", {
        get: function () { return this.EntityPM.DoneNote; },
        set: function (value) {
            if (this.EntityPM.DoneNote != value) {
                this.EntityPM.DoneNote = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    EditDataContext.prototype.DeleteFollowupClicked = function () {
        if (this.father.QuotePM) {
            this.father.QuotePM.RemoveQuoteFollowUpPM(this.EntityPM);
        }
        else if (this.father.ShipmentPM) {
            this.father.ShipmentPM.RemoveShipmentFollowUp(this.EntityPM);
        }
        this.father.CloseAddEdit();
        this.CurrentSession.FireEvent("FollowupsChanged");
        this.CurrentSession.FireEvent("FollowupDeleted");
    };
    EditDataContext.prototype.DoneFollowupClicked = function () {
        this.Done = true;
        this.DoneDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this.UIProperties.SetEnabled("Date", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OwnerUserId", this.ObjectTableName, false);
        this.father.Height += 110;
    };
    EditDataContext.prototype.UnDoneFollowupClicked = function () {
        this.Done = false;
        this.DoneDateTime = null;
        this.DoneNote = null;
        this.UIProperties.SetEnabled("Date", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("OwnerUserId", this.ObjectTableName, true);
        this.father.Height -= 110;
    };
    EditDataContext.prototype.DateNumericButtonClicked = function (isIncreas) {
        if (this.Date == null) {
            this.Date = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        else {
            var day = isIncreas ? 1 : -1;
            var myDateParts = Tools_1.DateTool.GetDateParts(this.Date);
            var dateObject = new Date();
            dateObject.setUTCMonth(0);
            dateObject.setUTCDate(1);
            dateObject.setUTCFullYear(myDateParts.Year);
            dateObject.setUTCMonth((myDateParts.Month - 1));
            dateObject.setUTCDate(myDateParts.Day + day);
            dateObject.setUTCHours(myDateParts.Hours);
            dateObject.setUTCMinutes(myDateParts.Minutes);
            dateObject.setUTCSeconds(myDateParts.Seconds);
            dateObject.setUTCMilliseconds(myDateParts.Milliseconds);
            this.Date = dateObject;
        }
    };
    EditDataContext.prototype.DoneDateNumericButtonClicked = function (isIncreas) {
        if (this.DoneDateTime == null) {
            this.DoneDateTime = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        else {
            var day = isIncreas ? 1 : -1;
            var myDateParts = Tools_1.DateTool.GetDateParts(this.Date);
            var dateObject = new Date();
            dateObject.setUTCMonth(0);
            dateObject.setUTCDate(1);
            dateObject.setUTCFullYear(myDateParts.Year);
            dateObject.setUTCMonth((myDateParts.Month - 1));
            dateObject.setUTCDate(myDateParts.Day + day);
            dateObject.setUTCHours(myDateParts.Hours);
            dateObject.setUTCMinutes(myDateParts.Minutes);
            dateObject.setUTCSeconds(myDateParts.Seconds);
            dateObject.setUTCMilliseconds(myDateParts.Milliseconds);
            this.DoneDateTime = dateObject;
        }
    };
    EditDataContext.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.father.CloseAddEdit();
    };
    EditDataContext.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.Notes) {
            if (this.Notes.length > 1000) {
                errors.push("Notes field must be less than 1000 and more than 0");
            }
        }
        if (errors.length == 0) {
            if (this.Done == true) {
                this.CurrentSession.FireEvent("FollowupDeleted");
            }
            this.father.CloseAddEdit();
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    };
    EditDataContext.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this);
        this.myCloner.AddField('Date');
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('OwnerUserId');
        this.myCloner.AddField('Done');
        this.myCloner.AddField('DoneNote');
        this.myCloner.AddField('DoneDateTime');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.EntityPM.EntityParentPM);
    };
    EditDataContext.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    return EditDataContext;
}(BaseComponent_1.BaseComponent));
exports.EditDataContext = EditDataContext;
//# sourceMappingURL=HelperFollowups.js.map