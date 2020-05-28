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
var ObjectsLocator_1 = require("../../Infrastructure/Locators/ObjectsLocator");
var HelpIcon = /** @class */ (function () {
    function HelpIcon() {
        this.Width = null;
        this.Height = null;
        this.IconSize = 17;
        this.HideHeader = false;
        this.TooltipId = null;
        this.TooltipContentId = null;
        this.IconPath = "./Images/Help.png";
        this.IconBackground = null;
        this.LayoutDirection = 'ltr';
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.header = "Help";
        this.text = "Help";
        var idIndex = this.CurrentSession.GetNewId("Tooltip");
        this.TooltipId = "Tooltip_" + idIndex;
        this.TooltipContentId = "TooltipContent_" + idIndex;
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    HelpIcon.prototype.ngOnInit = function () {
        if (this.Width == null) {
            this.Width = 265;
        }
        if (this.Height == null) {
            this.Height = 120;
        }
        this.IconBackground = "url(" + this.IconPath + ")";
    };
    Object.defineProperty(HelpIcon.prototype, "Header", {
        get: function () { return this.header; },
        set: function (newValue) {
            if (this.header != newValue) {
                this.header = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HelpIcon.prototype, "Text", {
        get: function () { return this.text; },
        set: function (newValue) {
            if (this.text != newValue) {
                this.text = newValue;
                if (this.text == null) {
                    this.IsVisible = false;
                }
                else {
                    this.IsVisible = true;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    HelpIcon.prototype.mouseover = function () {
        var item = document.getElementById(this.TooltipId);
        var itemRect = item.getBoundingClientRect();
        var isToRight = true;
        var ApplicationSession = document.getElementById("ApplicationSession");
        if (ApplicationSession) {
            var appWidth = ApplicationSession.clientWidth;
            var appHeight = ApplicationSession.clientHeight;
            if ((itemRect.left + this.Width) > appWidth) {
                isToRight = false;
            }
        }
        document.getElementById(this.TooltipContentId).style.position = "fixed";
        document.getElementById(this.TooltipContentId).style.top = (itemRect.top - this.Height + 5) + 'px';
        if (isToRight) {
            document.getElementById(this.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/Tootip.png')";
            document.getElementById(this.TooltipContentId).style.left = (itemRect.left + 5) + 'px';
        }
        else {
            document.getElementById(this.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/TootipFlipped.png')";
            document.getElementById(this.TooltipContentId).style.left = (itemRect.left - this.Width) + 'px';
        }
    };
    HelpIcon = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: "HelpIcon",
            inputs: ['Header', 'Text', 'HideHeader', 'IconSize', 'IconPath'],
            templateUrl: './HelpIcon.html',
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
        }),
        __metadata("design:paramtypes", [])
    ], HelpIcon);
    return HelpIcon;
}());
exports.HelpIcon = HelpIcon;
//# sourceMappingURL=HelpIcon.js.map