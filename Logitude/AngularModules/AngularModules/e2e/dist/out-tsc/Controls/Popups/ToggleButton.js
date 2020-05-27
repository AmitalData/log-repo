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
var ToggleButton = /** @class */ (function () {
    function ToggleButton() {
        this.ComponentId = null;
        this.ComponentButtonId = null;
        this.ComponentContentId = null;
        this.Title = null;
        this.IconPath = null;
        this.IsEnabled = true;
        this.Position = "Right";
        this.DropDownWidth = 0;
        this.DropDownHeight = 0;
        this.Opened = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SessionEvent = null;
        this.isOpened = false;
        this.IsMouseOver = false;
        this.IsMouseOverButton = false;
        this.IsMouseOverTextBox = false;
        var idIndex = this.CurrentSession.GetNewId("HelperNotes");
        this.ComponentId = "HelperNotes_" + idIndex;
        this.ComponentButtonId = "HelperNotesButton_" + idIndex;
        this.ComponentContentId = "HelperNotesContent_" + idIndex;
        this.CurrentSession.MouseDownEvent;
    }
    ToggleButton.prototype.ngOnInit = function () {
        var _this = this;
        this.SessionEvent = this.CurrentSession.MouseDownEvent.subscribe(function (s) {
            if (_this.IsMouseOverButton == false && _this.IsMouseOver == false) {
                _this.IsOpened = false;
            }
        });
    };
    ToggleButton.prototype.ngOnDestroy = function () {
        this.StopPositionTimer();
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        this.SessionEvent = null;
    };
    Object.defineProperty(ToggleButton.prototype, "IsOpened", {
        get: function () { return this.isOpened; },
        set: function (value) {
            if (value != undefined) {
                if (this.isOpened != value) {
                    this.isOpened = value;
                    this.Opened.emit(value);
                    if (value) {
                        this.SetPopupSize();
                        this.RunPositionTimer();
                    }
                    else {
                        this.StopPositionTimer();
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ToggleButton.prototype.StopPositionTimer = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    };
    ToggleButton.prototype.RunPositionTimer = function () {
        var _this = this;
        this.StopPositionTimer();
        this.timerToken = setInterval(function () { return _this.CalculateFixedPosition(); }, 0);
    };
    ToggleButton.prototype.CalculateFixedPosition = function () {
        var item = document.getElementById(this.ComponentId);
        if (item) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.ComponentContentId).style.top = (itemRect.top + 23) + 'px';
            if (this.Position == "Left") {
                document.getElementById(this.ComponentContentId).style.left = (itemRect.left) + 'px';
            }
            else {
                document.getElementById(this.ComponentContentId).style.left = (itemRect.left + itemRect.width - this.DropDownWidth) + 'px';
            }
        }
    };
    ToggleButton.prototype.SetPopupSize = function () {
        var item = document.getElementById(this.ComponentId);
        if (item) {
            var itemRect = item.getBoundingClientRect();
            if (this.DropDownWidth == 0) {
                this.DropDownWidth = itemRect.width;
            }
            if (this.DropDownHeight == 0) {
                this.DropDownHeight = 100;
            }
        }
    };
    ToggleButton.prototype.OnButtonClicked = function () {
        if (this.IsOpened) {
            this.StopPositionTimer();
            this.IsOpened = false;
        }
        else {
            this.CalculateFixedPosition();
            this.IsOpened = true;
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ToggleButton.prototype, "Opened", void 0);
    ToggleButton = __decorate([
        core_1.Component({
            selector: "ToggleButton",
            moduleId: module.id,
            templateUrl: './ToggleButton.html',
            inputs: ['Title', 'IconPath', 'DropDownWidth', 'DropDownHeight', 'Position', 'IsEnabled', 'IsOpened'],
        }),
        __metadata("design:paramtypes", [])
    ], ToggleButton);
    return ToggleButton;
}());
exports.ToggleButton = ToggleButton;
var ToggleButtonItem = /** @class */ (function () {
    function ToggleButtonItem(toggleButton) {
        this.toggleButton = toggleButton;
        this.Text = "Item";
        this.click = new core_1.EventEmitter();
        this.Clicked = new core_1.EventEmitter();
    }
    ToggleButtonItem.prototype.OnClick = function () {
        this.toggleButton.IsOpened = false;
        //setTimeout(() => this.Emit(), 10);
    };
    ToggleButtonItem.prototype.Emit = function () {
        //this.click.emit();
        //this.Clicked.emit();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ToggleButtonItem.prototype, "click", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ToggleButtonItem.prototype, "Clicked", void 0);
    ToggleButtonItem = __decorate([
        core_1.Component({
            selector: 'ToggleButtonItem',
            inputs: ['Text'],
            template: "\n    <button class=\"LogitudeToggleButtonItem\" (click)=\"OnClick()\">{{Text}}</button>\n    ",
            styles: [
                "\n\n    .LogitudeToggleButtonItem:hover:not(:disabled):not(.IconToggleButton):not(.HyperlinkButton):not(.CustomButton) {\n        background: rgba(255, 239, 43, 0.2);\n        border: 1px solid #FFC92B;\n    }\n\n    .LogitudeToggleButtonItem:disabled {\n        opacity: 0.5;\n        cursor: default;\n    }\n\n    .LogitudeToggleButtonItem:not(.HyperlinkButton):not(.LogitudeIconButton):not(.CustomButton) {\n        display: block;\n        height: 23px;\n        line-height: 23px;\n        text-align: left;\n        /*width: 100%;*/\n        width:auto;\n        min-width: 100%;\n        border: 1px solid white;\n        background: white;\n        padding-left: 5px;\n        padding-right: 3px;\n    }\n\n    "
            ]
        }),
        __metadata("design:paramtypes", [ToggleButton])
    ], ToggleButtonItem);
    return ToggleButtonItem;
}());
exports.ToggleButtonItem = ToggleButtonItem;
//# sourceMappingURL=ToggleButton.js.map