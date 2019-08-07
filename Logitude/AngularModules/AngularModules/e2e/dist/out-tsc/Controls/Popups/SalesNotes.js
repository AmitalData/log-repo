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
var Tools_1 = require("../../Infrastructure/Tools");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var EntityArgs_1 = require("../../Infrastructure/DataContracts/EntityArgs");
var ConfirmWindow_1 = require("../Windows/ConfirmWindow");
var SalesNotes = /** @class */ (function () {
    function SalesNotes(entityArgs) {
        this.entityArgs = entityArgs;
        this.ComponentId = null;
        this.ComponentButtonId = null;
        this.ComponentContentId = null;
        this.Width = 250;
        this.Height = 130;
        this.MinHeight = 130;
        this.MaxHeight = 350;
        this.Title = "Notes";
        this.IsEnabled = true;
        this.IconOpacity = 1;
        this.IsEditingEnabled = true;
        this.NotesList = [];
        this.OnAddButtonClicked = new core_1.EventEmitter();
        this.OnEditButtonClicked = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SalesNotesChangedEvent = null;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.isOpened = false;
        this.IsMouseOver = false;
        this.IsMouseOverButton = false;
        var idIndex = this.CurrentSession.GetNewId("SalesNotes");
        this.ComponentId = "SalesNotes_" + idIndex;
        this.ComponentButtonId = "SalesNotesButton_" + idIndex;
        this.ComponentContentId = "SalesNotesContent_" + idIndex;
    }
    SalesNotes.prototype.ngOnInit = function () {
        this.UpdateComponent();
        this.Listen();
    };
    SalesNotes.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            if (!this.SalesNotesChangedEvent) {
                this.SalesNotesChangedEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                    switch (s) {
                        case "CustomerSalesNotesChanged":
                            {
                                _this.UpdateComponent();
                                break;
                            }
                    }
                });
            }
            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                        _this.UpdateComponent();
                    }
                });
            }
            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                        _this.UpdateComponent();
                    }
                });
            }
        }
    };
    SalesNotes.prototype.UpdateComponent = function () {
        this.BuildNotesList();
        this.SetPopupHeight();
        this.SetIconPath();
    };
    SalesNotes.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SalesNotesChangedEvent);
        this.SalesNotesChangedEvent = null;
        this.StopPositionTimer();
    };
    SalesNotes.prototype.SetIconPath = function () {
        var file = "Gray.png";
        switch (this.IconCode) {
            case "G": {
                file = "Green.png";
                this.IconOpacity = this.NotesList.length == 0 ? 0.5 : 1;
                break;
            }
            case "B": {
                file = "Blue.png";
                this.IconOpacity = this.NotesList.length == 0 ? 0.5 : 1;
                break;
            }
            default: {
                file = this.NotesList.length == 0 ? "Gray.png" : "Orange.png";
                this.IconOpacity = 1;
                break;
            }
        }
        var iconPath = "./_Resources/Images/Icons/Notes/" + file;
        this.IconPath = "url(" + iconPath + ")";
    };
    SalesNotes.prototype.SetPopupHeight = function () {
        var myResult = 100;
        if (this.NotesList) {
            if (this.NotesList.length > 0) {
                var itemHeight = 80;
                var itemsCount = this.NotesList.length;
                if (itemsCount <= 3) {
                    myResult = itemsCount * itemHeight;
                    myResult += 10;
                    myResult += 25;
                }
                else {
                    myResult = 360;
                }
            }
        }
        this.Height = myResult;
    };
    Object.defineProperty(SalesNotes.prototype, "IsOpened", {
        get: function () { return this.isOpened; },
        set: function (value) {
            if (value != undefined) {
                if (this.isOpened != value) {
                    this.isOpened = value;
                    if (value) {
                        this.SetPopupHeight();
                        this.RunPositionTimer();
                        if (document.getElementById(this.ComponentContentId).style.visibility != "visible") {
                            document.getElementById(this.ComponentContentId).style.visibility = "visible";
                        }
                    }
                    else {
                        this.StopPositionTimer();
                        if (document.getElementById(this.ComponentContentId).style.visibility != "hidden") {
                            document.getElementById(this.ComponentContentId).style.visibility = "hidden";
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    SalesNotes.prototype.StopPositionTimer = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    };
    SalesNotes.prototype.RunPositionTimer = function () {
        var _this = this;
        this.StopPositionTimer();
        this.timerToken = setInterval(function () { return _this.CalculateFixedPosition(); }, 0);
    };
    SalesNotes.prototype.CalculateFixedPosition = function () {
        var item = document.getElementById(this.ComponentId);
        if (item) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.ComponentContentId).style.top = (itemRect.top + 24) + 'px';
            document.getElementById(this.ComponentContentId).style.left = (itemRect.left + 24 - this.Width) + 'px';
        }
    };
    SalesNotes.prototype.OnButtonClicked = function () {
        if (this.IsOpened) {
            this.StopPositionTimer();
            this.IsOpened = false;
        }
        else {
            this.CalculateFixedPosition();
            this.IsOpened = true;
        }
    };
    SalesNotes.prototype.OnButtonLostFocus = function () {
        if (!this.IsMouseOverButton) {
            if (this.IsMouseOver) {
                document.getElementById(this.ComponentButtonId).focus();
            }
            else {
                this.IsOpened = false;
            }
        }
    };
    SalesNotes.prototype.BuildNotesList = function () {
        var _this = this;
        this.NotesList = [];
        var list = [];
        if (this.EntityPM) {
            this.EntityPM.SalesNotes.forEach(function (item) {
                list.push(new SalesNoteItem(item));
            });
        }
        list.sort(function (a, b) { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? 1 : -1; }).forEach(function (item) {
            _this.NotesList.push(item);
        });
    };
    SalesNotes.prototype.AddButtonClicked = function () {
        this.IsOpened = false;
        this.OnAddButtonClicked.emit(true);
    };
    SalesNotes.prototype.EditButtonClicked = function (itemClass) {
        if (itemClass) {
            if (itemClass.EntityPM) {
                this.IsOpened = false;
                this.OnEditButtonClicked.emit(itemClass.EntityPM);
            }
        }
    };
    SalesNotes.prototype.DeleteButtonClicked = function (itemClass) {
        var _this = this;
        if (itemClass) {
            if (itemClass.EntityPM) {
                this.IsOpened = false;
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Show("Delete this note?");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.EntityPM.RemoveCustomerSalesNotePM(itemClass.EntityPM);
                        _this.UpdateComponent();
                    }
                });
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SalesNotes.prototype, "OnAddButtonClicked", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SalesNotes.prototype, "OnEditButtonClicked", void 0);
    SalesNotes = __decorate([
        core_1.Component({
            selector: "SalesNotes",
            moduleId: module.id,
            templateUrl: './SalesNotes.html',
            inputs: ['Title', 'IconCode', 'IsEnabled', 'EntityPM'],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], SalesNotes);
    return SalesNotes;
}());
exports.SalesNotes = SalesNotes;
var SalesNoteItem = /** @class */ (function () {
    function SalesNoteItem(item) {
        this.SortingValue = 0;
        this.IsButtonsHidden = true;
        this.EntityPM = item;
        this.SortingValue = Tools_1.DateTool.GetDateParts(item.UpdateDate).DateTicks;
        this.Notes = item.Notes;
        this.UpdateLabel = item.CreateDate == item.UpdateDate ? "Created by" : "Modified by";
        this.UpdateByUser = item.UpdatedByUserName;
        this.UpdateDateString = Tools_1.DateTool.GetDateFormats(item.UpdateDate).DateString;
        var width = 250 - 10;
        this.UpdateLabelWidth = Tools_1.AppTool.GetTextWidth(this.UpdateLabel, 10) + 5;
        this.UpdateDateStringWidth = Tools_1.AppTool.GetTextWidth(this.UpdateDateString, 10) + 10;
        var updateByUserWidth = Tools_1.AppTool.GetTextWidth(this.UpdateByUser, 10) + 5;
        var emptySpaceWidth = width - (this.UpdateLabelWidth + this.UpdateDateStringWidth);
        this.UpdateByUserWidth = emptySpaceWidth < updateByUserWidth ? emptySpaceWidth : updateByUserWidth;
        //if (emptySpaceWidth < updatedByUserWidth) {
        //    this.UpdateByUserWidth = emptySpaceWidth;
        //}
        //else {
        //    this.UpdateByUserWidth = updatedByUserWidth;
        //}
    }
    return SalesNoteItem;
}());
exports.SalesNoteItem = SalesNoteItem;
//# sourceMappingURL=SalesNotes.js.map