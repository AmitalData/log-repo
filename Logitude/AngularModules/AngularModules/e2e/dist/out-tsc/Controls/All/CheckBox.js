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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var CheckBox = /** @class */ (function () {
    function CheckBox() {
        this.ControlId = null;
        this.ControlId2 = null;
        this.Top = null;
        this.ZIndex = 0;
        this.Checked = new core_1.EventEmitter();
        this.LostFocus = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isChecked = false;
        this.isEnabled = true;
        this.ControlId = "CheckBox_" + this.CurrentSession.GetNewId("CheckBox");
        this.ControlId2 = this.ControlId + "_LBL";
    }
    Object.defineProperty(CheckBox.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (newValue) {
            if (this.isChecked != newValue) {
                this.isChecked = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CheckBox.prototype, "IsEnabled", {
        get: function () { return this.isEnabled; },
        set: function (newValue) {
            if (this.isEnabled != newValue) {
                this.isEnabled = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CheckBox.prototype, "Text", {
        get: function () { return this.text; },
        set: function (newValue) {
            if (this.text != newValue) {
                this.text = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    CheckBox.prototype.OnClick = function () {
        this.IsChecked = !this.IsChecked;
        this.Checked.emit(this.IsChecked);
    };
    CheckBox.prototype.OnLostFocus = function () {
        this.LostFocus.emit(true);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CheckBox.prototype, "Checked", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CheckBox.prototype, "LostFocus", void 0);
    CheckBox = __decorate([
        core_1.Component({
            selector: "CheckBox",
            inputs: ['IsChecked', 'IsEnabled', 'Text', 'Top', 'ZIndex'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <table>\n        <tr [style.height.px]=\"Top\" *ngIf=\"Top\">\n            <td>\n                <div></div>\n            </td>\n        </tr>\n\n        <tr>\n            <td style=\"width: 16px; min-width: 16px; padding:0 !important;\">\n                <div class=\"LogitudeCheckBox\" [style.zIndex]=\"ZIndex\">\n                    <input [attr.id]=\"ControlId\" type=\"checkbox\" [disabled]=\"!IsEnabled\" [checked]=\"IsChecked\" (click)=\"OnClick()\" (blur)=\"OnLostFocus()\" />\n                    <label [attr.id]=\"ControlId2\" [attr.for]=\"ControlId\"></label>\n                </div>\n            </td>\n\n            <td class=\"Label\" *ngIf=\"Text\" style=\"width: 1px; padding-left: 3px; padding-right: 3px; font-size: 11px !important; color: #6E7172 !important;\">{{Text}}</td>\n\n            <td>\n                <div></div>\n            </td>\n        </tr>\n    </table>\n    ",
            styles: ["\n    .LogitudeCheckBox input:focus + label {\n        border: 1px solid #3BB3E2;\n    }\n    .LogitudeCheckBox input:disabled + label {\n        opacity: 0.5;\n        pointer-events: none;\n    }\n    .LogitudeCheckBox input:checked + label {\n        background: url('./Images/CheckBoxIcon.png') center center no-repeat white;\n    }\n    .LogitudeCheckBox label {\n        border-radius: 2px;\n        -webkit-border-radius: 2px;\n        -moz-border-radius: 2px;\n    }\n    .LogitudeCheckBox label {\n        width: 16px;\n        height: 16px;\n        cursor: pointer;\n        display: block;\n        background: white;\n        border: 1px solid #AAAAAA;\n        user-select: none;\n        -ms-user-select: none;\n        -moz-user-select: none;\n        -webkit-user-select: none;\n        -moz-box-shadow: inset 0 0 3px #AAAAAA;\n        -webkit-box-shadow: inset 0 0 3px #AAAAAA;\n        box-shadow: inset 0 0 3px #AAAAAA;\n        line-height: 15px;\n        text-indent: 20px;\n        font-size: 11px !important;\n        color: #6E7172 !important;\n        position: absolute;\n    }\n    .LogitudeCheckBox input {    \n        opacity: 0;\n        display: none;\n        position: absolute;\n    }\n    .LogitudeCheckBox {\n        position: relative;\n        display: block;\n        height: 16px;\n    }\n    "],
        }),
        __metadata("design:paramtypes", [])
    ], CheckBox);
    return CheckBox;
}());
exports.CheckBox = CheckBox;
//# sourceMappingURL=CheckBox.js.map