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
var ShipmentArchiveFilter = /** @class */ (function () {
    function ShipmentArchiveFilter() {
        this.SelectedValue = "O";
        this.SelectedTypeValueChanged = new core_1.EventEmitter();
    }
    ShipmentArchiveFilter.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedTypeValueChanged.emit(itemValue);
            //var img_A = document.getElementById("TransportFilter_A");
            //var img_O = document.getElementById("TransportFilter_O");
            //var img_I = document.getElementById("TransportFilter_I");
            //img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
            //img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
            //img_I.setAttribute("src", "./Images/TransportModes/I_G.png");
            //switch (itemValue) {
            //    case "A": {
            //        img_A.setAttribute("src", "./Images/TransportModes/A_w.png");
            //        break;
            //    }
            //    case "O": {
            //        img_O.setAttribute("src", "./Images/TransportModes/O_w.png");
            //        break;
            //    }
            //    case "I": {
            //        img_I.setAttribute("src", "./Images/TransportModes/I_w.png");
            //        break;
            //    }
            //}
        }
    };
    ShipmentArchiveFilter.prototype.itemMouseOver = function (itemValue) {
        //if (this.SelectedValue != itemValue) {
        //    var img_A = document.getElementById("TransportFilter_A");
        //    var img_O = document.getElementById("TransportFilter_O");
        //    var img_I = document.getElementById("TransportFilter_I");
        //    switch (itemValue) {
        //        case "A": {
        //            img_A.setAttribute("src", "./Images/TransportModes/A.png");
        //            break;
        //        }
        //        case "O": {
        //            img_O.setAttribute("src", "./Images/TransportModes/O.png");
        //            break;
        //        }
        //        case "I": {
        //            img_I.setAttribute("src", "./Images/TransportModes/I.png");
        //            //img_I.style.top = "1px";
        //            break;
        //        }
        //    }
        //}
    };
    ShipmentArchiveFilter.prototype.itemMouseLeave = function (itemValue) {
        //if (this.SelectedValue != itemValue) {
        //    var img_A = document.getElementById("TransportFilter_A");
        //    var img_O = document.getElementById("TransportFilter_O");
        //    var img_I = document.getElementById("TransportFilter_I");
        //    switch (itemValue) {
        //        case "A": {
        //            img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
        //            break;
        //        }
        //        case "O": {
        //            img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
        //            break;
        //        }
        //        case "I": {
        //            img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
        //            break;
        //        }
        //    }
        //}
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ShipmentArchiveFilter.prototype, "SelectedTypeValueChanged", void 0);
    ShipmentArchiveFilter = __decorate([
        core_1.Component({
            selector: 'ShipmentArchiveFilter',
            inputs: ['SelectedValue'],
            template: "\n    <ul class=\"FiltersMenu\">\n        <li (click)=\"itemClicked('All')\" (mouseover)=\"itemMouseOver('All')\" style=\"width:55px;\" [class.SelectedFilter]=\"SelectedValue === 'All'\">\n            All\n        </li>\n        <li (click)=\"itemClicked('O')\" (mouseover)=\"itemMouseOver('O')\" style=\"width:55px;\" (mouseleave)=\"itemMouseLeave('O')\" [class.SelectedFilter]=\"SelectedValue === 'O'\">\n           Open\n        </li>\n        <li (click)=\"itemClicked('A')\" (mouseover)=\"itemMouseOver('A')\" style=\"width:55px;\" (mouseleave)=\"itemMouseLeave('A')\" [class.SelectedFilter]=\"SelectedValue === 'A'\">\n           Archived\n        </li> \n    </ul>\n    "
        })
    ], ShipmentArchiveFilter);
    return ShipmentArchiveFilter;
}());
exports.ShipmentArchiveFilter = ShipmentArchiveFilter;
//# sourceMappingURL=ShipmentArchiveFilter.js.map