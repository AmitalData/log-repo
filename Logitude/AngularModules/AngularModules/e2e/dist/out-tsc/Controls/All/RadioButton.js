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
var IdGeneratorPipe_1 = require("../Pipes/IdGeneratorPipe");
var RadioButton = /** @class */ (function () {
    function RadioButton() {
        this.ControlId = null;
        this.ControlId2 = null;
        this.Top = null;
        this.TextColor = Tools_1.FontTool.Gray;
        this.IsComboBoxWithCheck = false;
        this.Checked = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isChecked = false;
        this.isGreenText = false;
        this.isEnabled = true;
    }
    RadioButton.prototype.ngOnInit = function () {
        var pipe = new IdGeneratorPipe_1.IdGeneratorPipe();
        this.ControlId = pipe.transform(this.Text + "_" + this.Name);
        this.ControlId2 = this.ControlId + "_LBL";
        this.Name += this.CurrentSession.GetNewId("RadioButton");
    };
    Object.defineProperty(RadioButton.prototype, "Name", {
        get: function () { return this.name; },
        set: function (value) {
            if (this.name != value) {
                this.name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RadioButton.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (newValue) {
            if (this.isChecked != newValue) {
                this.isChecked = newValue;
                this.UpdateTextColor();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RadioButton.prototype, "IsGreenText", {
        get: function () { return this.isGreenText; },
        set: function (newValue) {
            if (this.isGreenText != newValue) {
                this.isGreenText = newValue;
                this.UpdateTextColor();
            }
        },
        enumerable: true,
        configurable: true
    });
    RadioButton.prototype.UpdateTextColor = function () {
        if (this.IsChecked && this.IsGreenText) {
            this.TextColor = Tools_1.FontTool.Green;
        }
        else {
            this.TextColor = Tools_1.FontTool.Gray;
        }
    };
    Object.defineProperty(RadioButton.prototype, "IsEnabled", {
        get: function () { return this.isEnabled; },
        set: function (newValue) {
            if (this.isEnabled != newValue) {
                this.isEnabled = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RadioButton.prototype, "Text", {
        get: function () { return this.text; },
        set: function (newValue) {
            if (this.text != newValue) {
                this.text = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    RadioButton.prototype.OnClick = function () {
        if (!this.IsChecked) {
            this.IsChecked = true;
            this.Checked.emit(this.IsChecked);
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], RadioButton.prototype, "Checked", void 0);
    RadioButton = __decorate([
        core_1.Component({
            selector: "RadioButton",
            inputs: ['IsChecked', 'IsEnabled', 'Text', 'Top', 'Name', 'IsGreenText', 'IsComboBoxWithCheck'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <table>\n        <tr [style.height.px]=\"Top\" *ngIf=\"Top\">\n            <td>\n                <div></div>\n            </td>\n        </tr>\n\n        <tr>\n            <td style=\"width: 16px; min-width: 16px;\">\n                <div class=\"LogitudeRadioButton\">\n                    <input [attr.id]=\"ControlId\" type=\"radio\" [attr.name]=\"Name\" [disabled]=\"!IsEnabled\" [checked]=\"IsChecked\" (click)=\"OnClick()\" />\n                    <label  [attr.id]=\"ControlId2\" [attr.for]=\"ControlId\"></label>\n                </div>\n            </td>\n\n            <td  *ngIf=\"Text\" style=\"width:3px;min-width:3px\">\n                <div></div>\n            </td>\n\n  <td  *ngIf=\"IsComboBoxWithCheck\" style=\"width:10px;min-width:10px\">\n                <div></div>\n            </td>\n\n\n\n            <td class=\"Label\" *ngIf=\"Text\" style=\"width: 1px;\" [ngStyle]=\"{'color': TextColor}\">{{Text}}</td>\n\n            <td>\n                <div></div>\n            </td>\n        </tr>\n    </table>\n    ",
            styles: ["\n    .LogitudeRadioButton input:focus + label {\n        border: 1px solid #3BB3E2;\n    }\n\n    .LogitudeRadioButton input:disabled + label {\n        opacity: 0.5;\n        pointer-events: none;    \n    }\n\n    .LogitudeRadioButton input:checked + label {\n        background-image: url('./Images/RadioButtonChecked.png');\n        background-repeat: no-repeat;\n        background-position: 3px 3px;\n        background-color: white;\n        background-size: 8.2px 8px;\n        cursor: default;\n    }\n\n    .LogitudeRadioButton label {\n        border-radius: 10px;\n        -webkit-border-radius: 10px;\n        -moz-border-radius: 10px;\n    }\n\n    .LogitudeRadioButton label {\n        width: 16px;\n        height: 16px;\n        cursor: pointer;\n        display: block;\n        background: white;\n        border: 1px solid #AAAAAA;\n        user-select: none;\n        -ms-user-select: none;\n        -moz-user-select: none;\n        -webkit-user-select: none;\n        box-sizing: border-box;\n        -moz-box-sizing: border-box;\n        -webkit-box-sizing: border-box;\n        -moz-box-shadow: inset 0 0 3px #AAAAAA;\n        -webkit-box-shadow: inset 0 0 3px #AAAAAA;\n        box-shadow: inset 0 0 3px #AAAAAA;\n        line-height: 15px;\n        text-indent: 20px;\n        font-size: 11px;\n        color: #6E7172;\n        position: absolute;\n    }\n\n    .LogitudeRadioButton input {\n        opacity:0;\n        display: none;\n        position: absolute;\n    }\n\n    .LogitudeRadioButton {\n        position: relative;\n        display: block;\n        height: 16px;\n    }\n\n    "],
        }),
        __metadata("design:paramtypes", [])
    ], RadioButton);
    return RadioButton;
}());
exports.RadioButton = RadioButton;
//# sourceMappingURL=RadioButton.js.map