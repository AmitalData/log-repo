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
var KeyControl = /** @class */ (function () {
    function KeyControl() {
        this.SelectedValue = "All";
        this.SelectedValueChanged = new core_1.EventEmitter();
    }
    KeyControl.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);
            var img_A = document.getElementById("KeyControl_A");
            var img_O = document.getElementById("KeyControl_B");
            img_A.setAttribute("src", "./Images/Icons/IsOpened.png");
            img_O.setAttribute("src", "./Images/Icons/IsClosed.png");
            switch (itemValue) {
                case "Open": {
                    //  img_A.setAttribute("src", "./Images/Icons/IsOpenedClicked.png");
                    break;
                }
                case "Close": {
                    //     img_O.setAttribute("src", "./Images/Icons/IsClosedClicked.png");
                    break;
                }
            }
        }
    };
    KeyControl.prototype.itemMouseOver = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById("KeyControl_A");
            var img_O = document.getElementById("KeyControl_B");
            img_A.setAttribute("src", "./Images/Icons/IsOpened.png");
            img_O.setAttribute("src", "./Images/Icons/IsClosed.png");
            switch (itemValue) {
                case "Open": {
                    //     img_A.setAttribute("src", "./Images/Icons/IsOpenedHoverd.png");
                    break;
                }
                case "Close": {
                    //     img_O.setAttribute("src", "./Images/Icons/IsClosedHoverd.png");
                    break;
                }
            }
        }
    };
    KeyControl.prototype.itemMouseLeave = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById("KeyControl_A");
            var img_O = document.getElementById("KeyControl_B");
            img_A.setAttribute("src", "./Images/Icons/IsOpened.png");
            img_O.setAttribute("src", "./Images/Icons/IsClosed.png");
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], KeyControl.prototype, "SelectedValueChanged", void 0);
    KeyControl = __decorate([
        core_1.Component({
            selector: 'KeyControl',
            inputs: ['SelectedValue'],
            template: "\n    <ul class=\"FiltersMenuImage\">\n        <li (click)=\"itemClicked('All')\" (mouseover)=\"itemMouseOver('All')\" [class.SelectedFilter]=\"SelectedValue === 'All'\" >\n            All\n        </li>\n        <li (click)=\"itemClicked('Open')\" (mouseover)=\"itemMouseOver('Open')\" (mouseleave)=\"itemMouseLeave('Open')\" [class.SelectedFilter]=\"SelectedValue === 'Open'\" title=\"Open\"  >\n            <img id=\"KeyControl_A\" class=\"CenterCenter\" src=\"./Images/Icons/IsOpened.png\" style=\"top: 1px;height:17px\"  />\n        </li>\n        <li (click)=\"itemClicked('Close')\" (mouseover)=\"itemMouseOver('Close')\" (mouseleave)=\"itemMouseLeave('Close')\" [class.SelectedFilter]=\"SelectedValue === 'Close'\" title=\"Close\" >\n            <img id=\"KeyControl_B\" class=\"CenterCenter\" src=\"./Images/Icons/IsClosed.png\" style=\"top: 1px;height:17px\" />\n        </li>\n       \n    </ul>\n    "
        })
    ], KeyControl);
    return KeyControl;
}());
exports.KeyControl = KeyControl;
//# sourceMappingURL=KeyControl.js.map