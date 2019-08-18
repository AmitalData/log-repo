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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var CustomerShortTitleComponent = /** @class */ (function () {
    function CustomerShortTitleComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            //this.BuildComponent();
        }
    }
    Object.defineProperty(CustomerShortTitleComponent.prototype, "EnglishName", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                myResult = this.EntityPM.EnglishName;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerShortTitleComponent.prototype, "RankName", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                myResult = this.EntityPM.RankName;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerShortTitleComponent.prototype, "RankSource1", {
        get: function () {
            var myResult = null;
            // silver to lower
            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.RankCode;
                switch (RankCode) {
                    case "1":
                    case "2":
                    case "3": {
                        myResult = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        myResult = "./Images/Icons/StarGray.png";
                        break;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerShortTitleComponent.prototype, "RankSource2", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.RankCode;
                switch (RankCode) {
                    case "2":
                    case "3": {
                        myResult = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        myResult = "./Images/Icons/StarGray.png";
                        break;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerShortTitleComponent.prototype, "RankSource3", {
        get: function () {
            var myResult = null;
            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.RankCode;
                switch (RankCode) {
                    case "3": {
                        myResult = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        myResult = "./Images/Icons/StarGray.png";
                        break;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    CustomerShortTitleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "./CustomerShortTitleComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CustomerShortTitleComponent);
    return CustomerShortTitleComponent;
}());
exports.CustomerShortTitleComponent = CustomerShortTitleComponent;
//# sourceMappingURL=CustomerShortTitleComponent.js.map