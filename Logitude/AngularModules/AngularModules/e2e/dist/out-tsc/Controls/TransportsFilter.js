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
var SessionLocator_1 = require("../Infrastructure/Utilities/SessionLocator");
var TransportsFilter = /** @class */ (function () {
    function TransportsFilter() {
        this.SelectedValueChanged = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.selectedValue = "All";
        if (this.CurrentSession == null) {
            this.FilterId_A = "TransportFilter_A_-1_-1";
            this.FilterId_O = "TransportFilter_O_-1_-1";
            this.FilterId_I = "TransportFilter_I_-1_-1";
        }
        else {
            var idIndex = this.CurrentSession.GetNewId("TransportsFilter");
            this.FilterId_A = "TransportFilter_A_" + idIndex;
            this.FilterId_O = "TransportFilter_O_" + idIndex;
            this.FilterId_I = "TransportFilter_I_" + idIndex;
        }
    }
    Object.defineProperty(TransportsFilter.prototype, "SelectedValue", {
        get: function () {
            return this.selectedValue;
        },
        set: function (value) {
            if (this.selectedValue != value) {
                this.selectedValue = value;
                this.ApplySelectedStyle();
            }
        },
        enumerable: true,
        configurable: true
    });
    TransportsFilter.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);
        }
    };
    TransportsFilter.prototype.itemMouseOver = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.FilterId_A);
            var img_O = document.getElementById(this.FilterId_O);
            var img_I = document.getElementById(this.FilterId_I);
            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A.png");
                    break;
                }
                case "O": {
                    img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O.png");
                    break;
                }
                case "I": {
                    img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I.png");
                    //img_I.style.top = "1px";
                    break;
                }
            }
        }
    };
    TransportsFilter.prototype.itemMouseLeave = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.FilterId_A);
            var img_O = document.getElementById(this.FilterId_O);
            var img_I = document.getElementById(this.FilterId_I);
            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A_g.png");
                    break;
                }
                case "O": {
                    img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O_g.png");
                    break;
                }
                case "I": {
                    img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I_g.png");
                    break;
                }
            }
        }
    };
    TransportsFilter.prototype.ApplySelectedStyle = function () {
        var img_A = document.getElementById(this.FilterId_A);
        var img_O = document.getElementById(this.FilterId_O);
        var img_I = document.getElementById(this.FilterId_I);
        if (img_A) {
            img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A_g.png");
            img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O_g.png");
            img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I_G.png");
            switch (this.SelectedValue) {
                case "A": {
                    img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A_w.png");
                    break;
                }
                case "O": {
                    img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O_w.png");
                    break;
                }
                case "I": {
                    img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I_w.png");
                    break;
                }
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], TransportsFilter.prototype, "SelectedValueChanged", void 0);
    TransportsFilter = __decorate([
        core_1.Component({
            selector: 'TransportsFilter',
            inputs: ['SelectedValue'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <ul class=\"FiltersMenu\">\n        <li (click)=\"itemClicked('All')\" (mouseover)=\"itemMouseOver('All')\" [class.SelectedFilter]=\"SelectedValue === 'All'\">\n            All\n        </li>\n        <li (click)=\"itemClicked('A')\" (mouseover)=\"itemMouseOver('A')\" (mouseleave)=\"itemMouseLeave('A')\" [class.SelectedFilter]=\"SelectedValue === 'A'\" title=\"Air\">\n            <img [attr.id]=\"FilterId_A\" class=\"CenterCenter\"  [attr.src]=\"SelectedValue === 'A' ? './_Resources/Images/Icons/TransportModes/Filters/A_w.png' : './_Resources/Images/Icons/TransportModes/Filters/A_g.png' \"   style=\"top: 1px;\" />\n        </li>\n        <li (click)=\"itemClicked('O')\" (mouseover)=\"itemMouseOver('O')\" (mouseleave)=\"itemMouseLeave('O')\" [class.SelectedFilter]=\"SelectedValue === 'O'\" title=\"Ocean\">\n            <img [attr.id]=\"FilterId_O\"  [attr.src]=\"SelectedValue === 'O' ? './_Resources/Images/Icons/TransportModes/Filters/O_w.png' : './_Resources/Images/Icons/TransportModes/Filters/O_g.png' \"    class=\"CenterCenter\"  style=\"top: 1px;\" />\n        </li>\n        <li (click)=\"itemClicked('I')\" (mouseover)=\"itemMouseOver('I')\" (mouseleave)=\"itemMouseLeave('I')\" [class.SelectedFilter]=\"SelectedValue === 'I'\" title=\"Inland\">\n            <img [attr.id]=\"FilterId_I\" class=\"CenterCenter\" [attr.src]=\"SelectedValue === 'I' ? './_Resources/Images/Icons/TransportModes/Filters/I_w.png' : './_Resources/Images/Icons/TransportModes/Filters/I_g.png' \"  style=\"top: 1px;\" />\n        </li>\n    </ul>\n    "
        }),
        __metadata("design:paramtypes", [])
    ], TransportsFilter);
    return TransportsFilter;
}());
exports.TransportsFilter = TransportsFilter;
//# sourceMappingURL=TransportsFilter.js.map