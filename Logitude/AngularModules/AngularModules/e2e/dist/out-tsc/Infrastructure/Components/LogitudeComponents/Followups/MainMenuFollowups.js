"use strict";
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
var Tools_1 = require("../../../Tools");
var TextCodeTranslator_1 = require("../../../Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Utilities/FeatureLocator");
var InfrastructureDomainService_1 = require("../../../Services/InfrastructureDomainService");
var MainMenuFollowups = /** @class */ (function () {
    function MainMenuFollowups() {
        this.IconPath = "./_Resources/Images/Icons/Followups/Followup.png";
        this.ItemsSource = [];
        this.IsMainSidebarCollapsed = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.FollowupsChangedEvent = null;
        this.objectTableId = null;
        this.DomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
        this.Listen();
    }
    MainMenuFollowups.prototype.Listen = function () {
        var _this = this;
        if (!this.FollowupsChangedEvent) {
            this.FollowupsChangedEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "FollowupsChangedMainMenu") {
                    _this.LoadData();
                }
            });
        }
    };
    MainMenuFollowups.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.FollowupsChangedEvent);
    };
    Object.defineProperty(MainMenuFollowups.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (value) {
            if (this.objectTableId != value) {
                this.objectTableId = value;
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    MainMenuFollowups.prototype.LoadData = function () {
        var _this = this;
        this.ItemsSource = [];
        this.SetIconPath();
        var objectTable = window.ObjectTables.filter(function (x) { return x.Id === _this.ObjectTableId; })[0];
        if (objectTable) {
            this.ObjectTableName = objectTable.Name;
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "READ")) {
                this.DomainService.GetMainMenuFollowups(this.ObjectTableName).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = [];
                        if (_this.ObjectTableName == "Quote") {
                            _this.BackButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate(" General.MH.Quotes");
                            var myQuotes = myResponse.Result;
                            myQuotes.forEach(function (item) {
                                var newListItem = new MainMenuFollowupItem(_this);
                                newListItem.Name = item.FollowUpType;
                                newListItem.Notes = item.FollowUpNotes;
                                newListItem.Date = item.FollowUpDate;
                                newListItem.EntityId = item.Id;
                                newListItem.EntityNumber = item.QuoteNumber;
                                newListItem.Initialize();
                                list.push(newListItem);
                            });
                        }
                        else {
                            _this.BackButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate(" General.MH.Operations");
                            var myShipments = myResponse.Result;
                            myShipments.forEach(function (item) {
                                var newListItem = new MainMenuFollowupItem(_this);
                                newListItem.Name = item.FollowUpType;
                                newListItem.Notes = item.FollowUpNotes;
                                newListItem.Date = item.FollowUpDate;
                                newListItem.EntityId = item.Id;
                                newListItem.EntityNumber = item.ShipmentNumber;
                                newListItem.Initialize();
                                list.push(newListItem);
                            });
                        }
                        _this.ItemsSource = list;
                        _this.SetIconPath();
                    }
                });
            }
        }
    };
    MainMenuFollowups.prototype.SetIconPath = function () {
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
    MainMenuFollowups = __decorate([
        core_1.Component({
            selector: "MainMenuFollowups",
            moduleId: module.id,
            templateUrl: './MainMenuFollowups.html',
            inputs: ['ObjectTableId', 'IsMainSidebarCollapsed'],
        }),
        __metadata("design:paramtypes", [])
    ], MainMenuFollowups);
    return MainMenuFollowups;
}());
exports.MainMenuFollowups = MainMenuFollowups;
var MainMenuFollowupItem = /** @class */ (function () {
    function MainMenuFollowupItem(father) {
        this.father = father;
        this.ItemId = null;
        this.ItemTooltipId = null;
        this.Name = null;
        this.Date = null;
        this.Notes = null;
        this.EntityId = null;
        this.EntityNumber = null;
        this.IsOld = false;
        this.Background = "white";
        this.ListItemHeight = 70;
        this.TooltipHeight = 130;
        this.TooltipWidth = 270;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsShowTooltip = false;
        var idIndex = this.CurrentSession.GetNewId("FollowupItem");
        this.ItemId = "FollowupItem_" + idIndex;
        this.ItemTooltipId = "FollowupItemTooltip_" + idIndex;
    }
    MainMenuFollowupItem.prototype.Initialize = function () {
        if (this.Date) {
            if (Tools_1.DateTool.GetDateParts(this.Date).DateTicks < Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
                this.IsOld = true;
                this.Background = "#F7E3E3";
            }
        }
        this.SetIconPath();
    };
    MainMenuFollowupItem.prototype.SetIconPath = function () {
        var iconPath = "./_Resources/Images/Icons/Followups/Document.png";
        var iconWidth = 11;
        var iconHeight = 14;
        if (this.Name) {
            var name = this.Name.toLowerCase();
            if (name.indexOf("arrived") > -1 || name.indexOf("departed") > -1 || name.indexOf("departure") > -1 || name.indexOf("arrival") > -1) {
                iconPath = "./_Resources/Images/Icons/Followups/Routing.png";
                iconWidth = 20;
                iconHeight = 14;
            }
            else if (name.indexOf("reminder") > -1 || name.indexOf("arranged") > -1) {
                iconPath = "./_Resources/Images/Icons/Followups/Reminder.png";
                iconWidth = 13;
                iconHeight = 13;
            }
        }
        this.IconPath = iconPath;
        this.IconWidth = iconWidth;
        this.IconHeight = iconHeight;
    };
    MainMenuFollowupItem.prototype.ShowTooltip = function (isShowTooltip) {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Notes)) {
            if (isShowTooltip) {
                var item = document.getElementById(this.ItemId);
                var itemRect = item.getBoundingClientRect();
                document.getElementById(this.ItemTooltipId).style.top = (itemRect.top - (this.TooltipHeight / 2) + (this.ListItemHeight / 2)) + 'px';
                document.getElementById(this.ItemTooltipId).style.left = (itemRect.left + 130) + 'px';
            }
            this.IsShowTooltip = isShowTooltip;
        }
    };
    MainMenuFollowupItem.prototype.ViewEntityClicked = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.EntityId, ObjectTableName: _this.father.ObjectTableName, BackButtonLabel: _this.father.BackButtonLabel });
        });
    };
    return MainMenuFollowupItem;
}());
exports.MainMenuFollowupItem = MainMenuFollowupItem;
//# sourceMappingURL=MainMenuFollowups.js.map