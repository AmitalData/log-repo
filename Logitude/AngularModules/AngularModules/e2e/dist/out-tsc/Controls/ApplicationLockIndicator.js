"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var ApplicationLockIndicator = /** @class */ (function () {
    function ApplicationLockIndicator() {
        this.IsLocked = false;
    }
    ApplicationLockIndicator = __decorate([
        core_1.Component({
            selector: 'ApplicationLockIndicator',
            inputs: ['IsLocked'],
            styles: ["\n    .ApplicationLockIndicatorControlLayout {\n        position: absolute;\n        top: 0;\n        bottom: 0;\n        left: 0;\n        right: 0;\n        margin: auto;\n        opacity: 0.5;\n        background: white;\n        z-index: 99;\n    }\n\n    .ApplicationLockIndicatorControl {\n        position: absolute;\n        top: 0;\n        bottom: 0;\n        left: 0;\n        right: 0;\n        margin: auto;\n        z-index: 100;\n    }\n  "],
            template: "\n    <div [hidden]=\"!IsLocked\" class=\"ApplicationLockIndicatorControlLayout\" tabindex=\"-1\" contenteditable=\"false\"></div>\n    <div [hidden]=\"!IsLocked\" class=\"ApplicationLockIndicatorControl\">\n       \n    </div>\n    ",
        })
    ], ApplicationLockIndicator);
    return ApplicationLockIndicator;
}());
exports.ApplicationLockIndicator = ApplicationLockIndicator;
//# sourceMappingURL=ApplicationLockIndicator.js.map