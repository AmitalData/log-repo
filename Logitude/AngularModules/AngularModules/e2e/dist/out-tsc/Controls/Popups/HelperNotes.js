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
var HelperNotes = /** @class */ (function () {
    function HelperNotes() {
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
        this.TextChanged = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.HelperNotesChangedEvent = null;
        this.isTextLoaded = false;
        this.text = null;
        this.isOpened = false;
        this.IsMouseOver = false;
        this.IsMouseOverButton = false;
        this.IsMouseOverTextBox = false;
        this.IsTextBoxFocused = false;
        var idIndex = this.CurrentSession.GetNewId("HelperNotes");
        this.ComponentId = "HelperNotes_" + idIndex;
        this.ComponentButtonId = "HelperNotesButton_" + idIndex;
        this.ComponentContentId = "HelperNotesContent_" + idIndex;
    }
    HelperNotes.prototype.ngOnInit = function () {
        this.SetIconPath();
        this.SetNotesList();
        this.SetPopupHeight();
        this.Listen();
    };
    HelperNotes.prototype.Listen = function () {
        var _this = this;
        if (!this.HelperNotesChangedEvent) {
            this.HelperNotesChangedEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                switch (s) {
                    case "QuotePartnersChanged":
                    case "ShipmentPartnersChanged":
                        {
                            _this.SetNotesList();
                            _this.SetPopupHeight();
                            break;
                        }
                }
            });
        }
    };
    HelperNotes.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.HelperNotesChangedEvent);
        this.HelperNotesChangedEvent = null;
        this.StopPositionTimer();
    };
    HelperNotes.prototype.SetIconPath = function () {
        var file = "Gray.png";
        switch (this.IconCode) {
            case "G": {
                file = "Green.png";
                this.IconOpacity = Tools_1.AppTool.IsNullOrEmpty(this.Text) ? 0.5 : 1;
                break;
            }
            case "B": {
                file = "Blue.png";
                this.IconOpacity = Tools_1.AppTool.IsNullOrEmpty(this.Text) ? 0.5 : 1;
                break;
            }
            default: {
                file = Tools_1.AppTool.IsNullOrEmpty(this.Text) ? "Gray.png" : "Orange.png";
                this.IconOpacity = 1;
                break;
            }
        }
        var iconPath = "./_Resources/Images/Icons/Notes/" + file;
        this.IconPath = "url(" + iconPath + ")";
    };
    HelperNotes.prototype.SetPopupHeight = function () {
        var myResult = 130;
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityTitle)) {
            myResult -= 15;
        }
        if (this.NotesList) {
            var itemHeight = 70;
            var itemsCount = this.NotesList.length;
            if (itemsCount <= 3) {
                myResult += itemsCount * itemHeight;
                myResult += 5;
            }
            else {
                myResult = 400;
            }
        }
        this.Height = myResult;
    };
    Object.defineProperty(HelperNotes.prototype, "Text", {
        get: function () { return this.text; },
        set: function (value) {
            if (this.text != value) {
                this.text = value;
                this.SetIconPath();
                if (this.isTextLoaded) {
                    this.TextChanged.emit(value);
                }
            }
            this.isTextLoaded = true;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HelperNotes.prototype, "IsOpened", {
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
    HelperNotes.prototype.StopPositionTimer = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    };
    HelperNotes.prototype.RunPositionTimer = function () {
        var _this = this;
        this.StopPositionTimer();
        this.timerToken = setInterval(function () { return _this.CalculateFixedPosition(); }, 0);
    };
    HelperNotes.prototype.CalculateFixedPosition = function () {
        var item = document.getElementById(this.ComponentId);
        if (item) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.ComponentContentId).style.top = (itemRect.top + 24) + 'px';
            document.getElementById(this.ComponentContentId).style.left = (itemRect.left + 24 - this.Width) + 'px';
        }
    };
    HelperNotes.prototype.OnButtonClicked = function () {
        if (this.IsOpened) {
            this.StopPositionTimer();
            this.IsOpened = false;
        }
        else {
            this.CalculateFixedPosition();
            this.IsOpened = true;
        }
    };
    HelperNotes.prototype.OnButtonLostFocus = function () {
        if (!this.IsMouseOverButton) {
            if (this.IsMouseOver || this.IsMouseOverTextBox) {
                if (!this.IsMouseOverTextBox) {
                    document.getElementById(this.ComponentButtonId).focus();
                }
            }
            else {
                this.IsOpened = false;
            }
        }
    };
    HelperNotes.prototype.OnTextBoxFocus = function () {
        this.IsTextBoxFocused = true;
    };
    HelperNotes.prototype.OnTextBoxLostFocus = function () {
        if (this.IsTextBoxFocused) {
            this.IsTextBoxFocused = false;
            if (!this.IsMouseOverButton) {
                if (this.IsMouseOver) {
                    document.getElementById(this.ComponentButtonId).focus();
                }
                else {
                    this.IsOpened = false;
                }
            }
        }
    };
    HelperNotes.prototype.OnTextBoxKeydown = function (e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode == 9) {
            e.preventDefault();
        }
    };
    HelperNotes.prototype.SetNotesList = function () {
        if (this.QuotePM) {
            this.BuildQuoteNotesList();
        }
        else if (this.ShipmentPM) {
            this.BuildShipmentNotesList();
        }
    };
    HelperNotes.prototype.BuildQuoteNotesList = function () {
        this.NotesList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.ShipperNote)) {
            this.NotesList.push({ Header: "Shipper", Notes: this.QuotePM.ShipperNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.ConsigneeNote)) {
            this.NotesList.push({ Header: "Consignee", Notes: this.QuotePM.ConsigneeNote });
        }
        //if (!AppTool.IsNullOrEmpty(this.QuotePM.AgentNote)) {
        //    this.NotesList.push({ Header: "Agent", Notes: this.QuotePM.AgentNote });
        //}
    };
    HelperNotes.prototype.BuildShipmentNotesList = function () {
        this.NotesList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentPM.ShipperNote)) {
            this.NotesList.push({ Header: "Shipper", Notes: this.ShipmentPM.ShipperNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentPM.ConsigneeNote)) {
            this.NotesList.push({ Header: "Consignee", Notes: this.ShipmentPM.ConsigneeNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentPM.AgentNote)) {
            this.NotesList.push({ Header: "Agent", Notes: this.ShipmentPM.AgentNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentPM.CustomAgentExportNote)) {
            this.NotesList.push({ Header: "Custom Agent Export", Notes: this.ShipmentPM.CustomAgentExportNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentPM.CustomAgentImportNote)) {
            this.NotesList.push({ Header: "Custom Agent Import", Notes: this.ShipmentPM.CustomAgentImportNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentPM.Notify1Note)) {
            this.NotesList.push({ Header: "Notify1", Notes: this.ShipmentPM.Notify1Note });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentPM.Notify2Note)) {
            this.NotesList.push({ Header: "Notify2", Notes: this.ShipmentPM.Notify2Note });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentPM.ConsigneeNotImporterNote)) {
            this.NotesList.push({ Header: "Consignee Not Importer", Notes: this.ShipmentPM.ConsigneeNotImporterNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentPM.ShipperNotExporterNote)) {
            this.NotesList.push({ Header: "Shipper Not Exporter", Notes: this.ShipmentPM.ShipperNotExporterNote });
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], HelperNotes.prototype, "TextChanged", void 0);
    HelperNotes = __decorate([
        core_1.Component({
            selector: "HelperNotes",
            moduleId: module.id,
            templateUrl: './HelperNotes.html',
            inputs: ['Title', 'EntityTitle', 'Text', 'IconCode', 'IsEnabled', 'ShipmentPM', 'QuotePM'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
        }),
        __metadata("design:paramtypes", [])
    ], HelperNotes);
    return HelperNotes;
}());
exports.HelperNotes = HelperNotes;
//# sourceMappingURL=HelperNotes.js.map