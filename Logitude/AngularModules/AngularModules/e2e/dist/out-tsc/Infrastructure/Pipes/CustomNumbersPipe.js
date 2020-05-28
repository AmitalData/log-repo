"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var CustomNumbersPipe = /** @class */ (function () {
    function CustomNumbersPipe() {
    }
    CustomNumbersPipe.prototype.transform = function (myNumber, myFractionDigits) {
        var myResult = "";
        if (myNumber != null && !isNaN(myNumber)) {
            if (myFractionDigits == 0) {
                //myResult = myNumber.toLocaleString('en-US'); (not working with Safari)
                myResult = myNumber.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
            //else if (myFormat.length == 0) {
            //    //myResult = myNumber.toLocaleString('en-US'); (not working with Safari)
            //    myResult = myNumber.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            //}
            else {
                //var myFractionDigits = 0;
                //switch (myFormat.toString().toLowerCase()) {
                //    case "n1": { myFractionDigits = 1; break; }
                //    case "n2": { myFractionDigits = 2; break; }
                //    case "n3": { myFractionDigits = 3; break; }
                //    case "n4": { myFractionDigits = 4; break; }
                //    case "n5": { myFractionDigits = 5; break; }
                //}
                myResult = myNumber.toFixed(myFractionDigits).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                //(not working with Safari)
                //myResult = myNumber.toLocaleString('en-US', { minimumFractionDigits: myMinFractionDigits, maximumFractionDigits: myMaxFractionDigits });
                //myResult = parseFloat((Math.round(myNumber * 100) / 100).toString()).toFixed(myMinFractionDigits).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
        }
        return myResult;
    };
    CustomNumbersPipe = __decorate([
        core_1.Pipe({ name: 'CustomNumbersPipe' })
    ], CustomNumbersPipe);
    return CustomNumbersPipe;
}());
exports.CustomNumbersPipe = CustomNumbersPipe;
//# sourceMappingURL=CustomNumbersPipe.js.map