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
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var DropdownButtonComponent = /** @class */ (function () {
    function DropdownButtonComponent(myElement) {
        this.Dropdownbutton_Text = "Show Dropdown Content";
        this._DropdownDisplay = 'none';
        this.Width = -60;
        this.Height = -40;
        this._ElementRef = myElement;
        var curId = DropdownButtonComponent_1.MyId++;
        this._DropdownButtonComponentId = "DropdownButtonComponent_" + curId;
        this._DropdownButtonComponentMenuId = "DropdownButtonComponentMenuId_" + curId;
    }
    DropdownButtonComponent_1 = DropdownButtonComponent;
    Object.defineProperty(DropdownButtonComponent.prototype, "Dropdownbutton_TextCode", {
        get: function () { return this._Dropdownbutton_TextCode; },
        set: function (val) {
            if (!Tools_1.AppTool.IsNullOrEmpty(val) && val != this._Dropdownbutton_TextCode) {
                this._Dropdownbutton_TextCode = val;
                this.Dropdownbutton_Text = TextCodeTranslator_1.TextCodeTranslator.Translate(val);
            }
        },
        enumerable: true,
        configurable: true
    });
    DropdownButtonComponent.prototype.handleClick = function (event) {
        var clickedComponent = event.target;
        var inside = false;
        var conter = 0;
        do {
            if (clickedComponent === this._ElementRef.nativeElement) {
                inside = true;
                break;
            }
            if (clickedComponent.class === "class-dropdown-content") {
                inside = true;
                break;
            }
            if (conter > 50) {
                break;
            }
            conter++;
            clickedComponent = clickedComponent.parentNode;
        } while (clickedComponent);
        if (inside) {
        }
        else {
            if (this._DropdownDisplay == 'block') {
                this.dropdowndisplayToggle();
            }
        }
    };
    DropdownButtonComponent.prototype.ngOnInit = function () {
    };
    DropdownButtonComponent.prototype.DropdownDisplayClose = function () {
        this._DropdownDisplay = 'none';
    };
    DropdownButtonComponent.prototype.dropdowndisplayToggle = function () {
        if (this._DropdownDisplay == 'none') {
            var item = document.getElementById(this._DropdownButtonComponentId);
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this._DropdownButtonComponentMenuId).style.top =
                (itemRect.top + 27) + 'px';
            document.getElementById(this._DropdownButtonComponentMenuId).style.left =
                (itemRect.left - 50) + 'px'; //min-width: 80px
            this._DropdownDisplay = 'block';
        }
        else {
            this._DropdownDisplay = 'none';
        }
    };
    var DropdownButtonComponent_1;
    DropdownButtonComponent.MyId = 0;
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DropdownButtonComponent.prototype, "IsDisabled", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], DropdownButtonComponent.prototype, "Dropdownbutton_Text", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String),
        __metadata("design:paramtypes", [String])
    ], DropdownButtonComponent.prototype, "Dropdownbutton_TextCode", null);
    DropdownButtonComponent = DropdownButtonComponent_1 = __decorate([
        core_1.Component({
            selector: 'dropdown-button',
            moduleId: module.id,
            host: {
                '(document:click)': 'handleClick($event)',
            },
            templateUrl: 'DropdownButtonComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ElementRef])
    ], DropdownButtonComponent);
    return DropdownButtonComponent;
}());
exports.DropdownButtonComponent = DropdownButtonComponent;
//# sourceMappingURL=DropdownButtonComponent.js.map