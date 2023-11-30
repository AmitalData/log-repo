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
var ObjectsLocator_1 = require("../Infrastructure/Locators/ObjectsLocator");
var IconButton = /** @class */ (function () {
    function IconButton() {
        this.Left = 0;
        this.Top = "0px";
        this.IsIconOnly = false;
        this.LeftIndent = "1px";
        this.TopIndent = "0px";
        this.LayoutDirection = 'ltr';
        this.ExternalId = "";
        this.isdisabled = false;
        this.isEnabled = true;
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    IconButton.prototype.ngOnInit = function () {
        if (this.Name != null) {
            if (this.Width == null || this.Width === undefined || this.Width == 0) {
                this.Width = this.IsSmall ? 18 : 21;
            }
            if (this.Height == null || this.Height === undefined || this.Height == 0) {
                this.Height = this.IsSmall ? 18 : 21;
            }
            if (this.IsSmall) {
                this.LeftIndent = "2px";
            }
            switch (this.Name.toLowerCase()) {
                case "settings": {
                    this.mySource = './Images/Icons/Settings.png';
                    this.mySourceOver = './Images/Icons/Settings_Blue.png';
                    break;
                }
                case "payments": {
                    this.mySource = './Images/Icons/payments.png';
                    this.mySourceOver = './Images/Icons/payments_Blue.png';
                    break;
                }
                case "refresh": {
                    this.mySource = './Images/Buttons/Refresh.png';
                    break;
                }
                case "add": {
                    this.mySource = "./Images/Buttons/Add.png";
                    this.mySourceOver = "./Images/Buttons/Add.over.png";
                    break;
                }
                case "edit": {
                    this.mySource = "./Images/Buttons/Edit.png";
                    this.mySourceOver = "./Images/Buttons/Edit.over.png";
                    break;
                }
                case "editorange": {
                    this.mySource = "./Images/Buttons/Edit.over.png";
                    this.mySourceOver = "./Images/Buttons/Edit.png";
                    break;
                }
                case "additional": {
                    this.mySource = "./Images/ThickTick.png";
                    this.mySourceOver = "./Images/ThickTick.png";
                    break;
                }
                case "delete": {
                    this.mySource = "./Images/Buttons/Delete.png";
                    this.mySourceOver = "./Images/Buttons/Delete.over.png";
                    break;
                }
                case "connect": {
                    this.mySource = "./Images/Buttons/Connect.png";
                    this.mySourceOver = "./Images/Buttons/Connect.png";
                    break;
                }
                case "disconnect": {
                    this.mySource = "./Images/Buttons/Disconnect.png";
                    this.mySourceOver = "./Images/Buttons/Disconnect.png";
                    break;
                }
                case 'help': {
                    this.mySource = "./Images/Icons/Help.png";
                    this.mySourceOver = "./Images/Icons/Help_Blue.png";
                    break;
                }
                case 'signout': {
                    this.mySource = "./Images/Icons/Signout.png";
                    this.mySourceOver = "./Images/Icons/Signout_Blue.png";
                    break;
                }
                case "call": {
                    this.mySource = "./Images/Buttons/Call.png";
                    break;
                }
                case "task": {
                    this.mySource = "./Images/Buttons/Task.png";
                    break;
                }
                case "appoint": {
                    this.mySource = "./Images/Buttons/Appointment.png";
                    break;
                }
                case "email": {
                    this.mySource = "./Images/Buttons/Email.png";
                    break;
                }
                case "excel": {
                    this.mySource = "./Images/Buttons/excelicon.png";
                    break;
                }
                case "watch": {
                    this.mySource = "./Images/Watch.png";
                    break;
                }
                case "disabledwatch": {
                    this.mySource = "./Images/DisabledWatch.png";
                    break;
                }
                case "bell": {
                    this.mySource = "./Images/Bell.png";
                    break;
                }
                case "deletefollowup": {
                    this.mySource = "./_Resources/Images/Icons/Followups/DeleteFollowup.png";
                    this.mySourceOver = "./_Resources/Images/Icons/Followups/DeleteFollowup_Red.png";
                    break;
                }
                case "donefollowup": {
                    this.mySource = "./_Resources/Images/Icons/Followups/DoneButton.png";
                    this.mySourceOver = "./_Resources/Images/Icons/Followups/DoneButton_Green.png";
                    break;
                }
                case "search": {
                    this.mySource = "./Images/LOVSearch.png";
                    this.LeftIndent = "0px";
                    break;
                }
                case "copy": {
                    this.mySource = "./Images/Buttons/copy.png";
                    break;
                }
                case "return": {
                    this.mySource = "./Images/return.png";
                    break;
                }
            }
            this.Source = this.mySource;
            if (this.mySourceOver == null) {
                this.mySourceOver = this.mySource;
            }
        }
    };
    Object.defineProperty(IconButton.prototype, "disabled", {
        get: function () { return this.isdisabled; },
        set: function (value) {
            if (this.isdisabled != value) {
                this.isdisabled = value;
                this.IsEnabled = !value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(IconButton.prototype, "IsEnabled", {
        get: function () { return this.isEnabled; },
        set: function (value) {
            if (this.isEnabled != value) {
                this.isEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    IconButton.prototype.OnMouseEnter = function () {
        this.Source = this.mySourceOver;
    };
    IconButton.prototype.OnMouseLeave = function () {
        this.Source = this.mySource;
    };
    IconButton = __decorate([
        core_1.Component({
            selector: 'IconButton',
            inputs: ['Name', 'IsSmall', 'Width', 'Height', 'IsEnabled', 'disabled', 'IsIconOnly', 'TopIndent', 'Left', 'Top', 'Title', 'ExternalId'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <button id=\"{{ Name+ExternalId | IdGeneratorAsyncPipe | async}}\"   title=\"{{Title}}\" *ngIf=\"!IsIconOnly\" \n            class=\"LogitudeIconButton\" \n            [style.width.px]=\"Width\" \n            [style.height.px]=\"Height\" \n            [style.margin-left.px]=\"Left\" \n            [disabled]=\"!IsEnabled\" \n            [ngStyle]=\"{top: Top}\"\n            (mouseenter)=\"OnMouseEnter()\" \n            (mouseleave)=\"OnMouseLeave()\" \n            tabindex=\"-1\">\n\n                <img [class.FlipImgHoriz]=\"LayoutDirection == 'rtl'\" \n                    [attr.src]=\"Source\" \n                    style=\"visibility:inherit;\" \n                    [style.width]=\"Name=='return' ? '13px' : 'initial'\"\n                    [ngStyle]=\"LayoutDirection == 'rtl' ? {top: TopIndent,'right': LeftIndent , 'left' : 0} : {top: TopIndent,'left': LeftIndent, 'right':0}\" />\n\n    </button>\n\n    <div *ngIf=\"IsIconOnly\"  title=\"{{Title}}\"\n            style=\"width: 20px; height: 20px; cursor:pointer; position: relative;\" \n            (mouseenter)=\"OnMouseEnter()\" \n            (mouseleave)=\"OnMouseLeave()\">\n\n                <img [class.FlipImgHoriz]=\"LayoutDirection == 'rtl'\"\n                    [attr.src]=\"Source\" \n                    [style.width]=\"Name=='return' ? '13px' : 'initial'\"\n                    style=\"visibility:inherit; vertical-align: middle; position: absolute; top:0; bottom:0; right:0; margin: auto; transform:none;\" />\n\n    </div>\n\n    ",
            styles: ["\n\n    .LogitudeIconButton img {\n        position: absolute;\n        top: 1px;\n        bottom: 0;\n        margin: auto;\n    }\n\n    .LogitudeIconButton:focus:not(:disabled) {\n        border: 1px solid #3BB3E2;\n        -webkit-box-shadow: 0px 0px 6px 0px #3BB3E2;\n        -moz-box-shadow: 0px 0px 6px 0px #3BB3E2;\n        box-shadow: 0px 0px 6px 0px #3BB3E2;\n    }\n    .LogitudeIconButton:hover:not(:disabled) {\n        cursor: pointer;\n        background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(221, 232, 245, 1) 100%);\n        background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(221, 232, 245, 1) 100%);\n        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(221, 232, 245, 1) ));\n        background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(221, 232, 245, 1) 100%);\n        background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(221, 232, 245, 1) 100%);\n    }\n\n    .LogitudeIconButton:disabled {\n        opacity: 0.5;\n        pointer-events: none !important; \n        cursor: default !important;;\n    }\n\n    .LogitudeIconButton {\n        display: block !important;\n        float: left !important;\n        width: 21px;\n        height: 21px;\n        outline: none;\n        border: 1px solid #6A8299;\n        border-radius: 3px;\n        -moz-border-radius: 3px;\n        -webkit-border-radius: 3px;\n        position: relative;     \n        background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(186, 206, 227, 1) 100%);\n        background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);\n        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(186, 206, 227, 1) ));\n        background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);\n        background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);\n    }\n    .FlipImgHoriz{\n        -moz-transform: scaleX(-1);\n        -o-transform: scaleX(-1);\n        -webkit-transform: scaleX(-1);\n        transform: scaleX(-1);\n        filter: FlipH;\n        -ms-filter: \"FlipH\";\n    }\n    "],
        }),
        __metadata("design:paramtypes", [])
    ], IconButton);
    return IconButton;
}());
exports.IconButton = IconButton;
//# sourceMappingURL=IconButton.js.map