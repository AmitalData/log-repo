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
var CellTooltip = /** @class */ (function () {
    function CellTooltip() {
        this.TooltipId = null;
        this.TooltipButtonId = null;
        this.TooltipContentId = null;
        this.IconWidth = 18;
        this.IconHeight = 18;
        this.IconPath = "./Images/Help.png";
        this.IconBackground = null;
        this.Width = 296;
        this.MinHeight = 130;
        this.MaxHeight = 130;
        this.IsOnClick = false;
        this.Head = null;
        this.Body = null;
        this.IsMouseOver = false;
        this.Opened = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.height = 130;
        this.isOpened = false;
        this.isToRight = false;
        var idIndex = this.CurrentSession.GetNewId("Tooltip");
        this.TooltipId = "Tooltip_" + idIndex;
        this.TooltipButtonId = "TooltipButton_" + idIndex;
        this.TooltipContentId = "TooltipContent_" + idIndex;
    }
    CellTooltip.prototype.ngOnInit = function () {
        this.IconBackground = "url(" + this.IconPath + ") no-repeat";
    };
    CellTooltip.prototype.ngAfterViewInit = function () {
        if (this.IsOnClick) {
            document.getElementById(this.TooltipButtonId).style.cursor = "pointer";
        }
        else {
            document.getElementById(this.TooltipButtonId).style.cursor = "default";
        }
    };
    Object.defineProperty(CellTooltip.prototype, "Height", {
        get: function () { return this.height; },
        set: function (value) {
            if (this.height != value) {
                if (value > this.MinHeight) {
                    this.height = value;
                    this.SetTooltipSize();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CellTooltip.prototype, "IsOpened", {
        get: function () { return this.isOpened; },
        set: function (value) {
            if (value != undefined) {
                if (this.isOpened != value) {
                    this.isOpened = value;
                    if (!value) {
                        document.getElementById(this.TooltipContentId).style.visibility = "hidden";
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CellTooltip.prototype, "IsToRight", {
        get: function () { return this.isToRight; },
        set: function (value) {
            if (this.isToRight != value) {
                this.isToRight = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CellTooltip.prototype.mouseover = function () {
        if (!this.IsOnClick) {
            var item = document.getElementById(this.TooltipId);
            var itemRect = item.getBoundingClientRect();
            if (this.IsToRight) {
                document.getElementById(this.TooltipContentId).style.position = "fixed";
                document.getElementById(this.TooltipContentId).style.top = (itemRect.top - this.Height + 5) + 'px';
                document.getElementById(this.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/Tootip.png')";
                document.getElementById(this.TooltipContentId).style.left = (itemRect.left + 5) + 'px';
            }
            else {
                document.getElementById(this.TooltipContentId).style.top = (itemRect.top - (this.Height / 2) + 7) + 'px';
                document.getElementById(this.TooltipContentId).style.left = (itemRect.left - this.Width + 5) + 'px';
            }
            document.getElementById(this.TooltipContentId).style.visibility = "visible";
        }
    };
    CellTooltip.prototype.mouseleave = function () {
        if (!this.IsOnClick) {
            document.getElementById(this.TooltipContentId).style.visibility = "hidden";
        }
    };
    CellTooltip.prototype.click = function () {
        if (this.IsOnClick) {
            if (document.getElementById(this.TooltipContentId).style.visibility == "visible") {
                document.getElementById(this.TooltipContentId).style.visibility = "hidden";
                this.isOpened = false;
                this.Opened.emit(this.isOpened);
            }
            else {
                var item = document.getElementById(this.TooltipId);
                var itemRect = item.getBoundingClientRect();
                document.getElementById(this.TooltipContentId).style.top = (itemRect.top - (this.Height / 2) + 7) + 'px';
                document.getElementById(this.TooltipContentId).style.left = (itemRect.left - this.Width + 5) + 'px';
                document.getElementById(this.TooltipContentId).style.visibility = "visible";
                this.isOpened = true;
                this.Opened.emit(this.isOpened);
            }
        }
    };
    CellTooltip.prototype.blur = function () {
        if (this.IsOnClick) {
            if (this.IsMouseOver) {
                document.getElementById(this.TooltipButtonId).focus();
            }
            else {
                document.getElementById(this.TooltipContentId).style.visibility = "hidden";
                this.isOpened = false;
                this.Opened.emit(this.isOpened);
            }
        }
    };
    CellTooltip.prototype.SetTooltipSize = function () {
        if (this.TooltipId) {
            var item = document.getElementById(this.TooltipId);
            if (item) {
                var itemRect = item.getBoundingClientRect();
                var fixedHeight = this.Height;
                if (fixedHeight > this.MaxHeight) {
                    fixedHeight = this.MaxHeight;
                }
                document.getElementById(this.TooltipContentId).style.top = (itemRect.top - (fixedHeight / 2) + 7) + 'px';
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CellTooltip.prototype, "Opened", void 0);
    CellTooltip = __decorate([
        core_1.Component({
            selector: 'CellTooltip',
            moduleId: module.id,
            templateUrl: './CellTooltip.html',
            inputs: ['IconWidth', 'IconHeight', 'IconPath', 'Width', 'Height', 'Head', 'Body', 'MaxHeight', 'IsOnClick', 'IsOpened', 'IsToRight'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
        }),
        __metadata("design:paramtypes", [])
    ], CellTooltip);
    return CellTooltip;
}());
exports.CellTooltip = CellTooltip;
//# sourceMappingURL=CellTooltip.js.map