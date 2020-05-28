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
///////////////////////////////////////////////
var elementType;
(function (elementType) {
    elementType[elementType["header"] = 1] = "header";
    elementType[elementType["separator"] = 2] = "separator";
    elementType[elementType["field"] = 3] = "field";
})(elementType || (elementType = {}));
/////////////////////////////////////////////////
//<span *ngSwitchCase="2" > <hr/></span>
var ObjectViewerComponent = /** @class */ (function () {
    function ObjectViewerComponent() {
        this.MyNgStyleHeight = "100px";
        this._SearchValue = "";
        this._MyObjectElements = new Array();
    }
    Object.defineProperty(ObjectViewerComponent.prototype, "MyObject", {
        get: function () {
            return this._MyObject;
        },
        set: function (newValue) {
            if (this._MyObject != newValue) {
                this._SearchValue = "";
                this._MyObject = newValue;
                this._MyObjectElements = null;
                this._MyObjectElements = new Array();
                this.parseObjectToArrayElements(this._MyObject);
                // console.log(this._MyObjectElements);
            }
        },
        enumerable: true,
        configurable: true
    });
    ObjectViewerComponent.prototype.SetWindowArgs = function (myArg) {
        ///alert("SetWindowArgs" + myArg);
        this.MyNgStyleHeight = "420px";
        this.MyObject = myArg;
    };
    ObjectViewerComponent.prototype.parseObjectToArrayElements = function (myStartObj) {
        if (myStartObj == null || myStartObj == undefined)
            return;
        var mytype = typeof myStartObj;
        this._MyObjectElements.push({ id: this._id++, key: mytype, value: null, myElementType: elementType.separator });
        var tst = Object.keys(myStartObj).length;
        var currKey;
        for (currKey in myStartObj) {
            if (myStartObj.hasOwnProperty(currKey)) {
                var currPropValue = myStartObj[currKey];
                if (currPropValue instanceof Object) {
                    this._MyObjectElements.push({ id: this._id++, key: currKey, value: null, myElementType: elementType.header });
                    if (currPropValue instanceof Array) {
                        for (var iCounter = 0; iCounter < currPropValue.length; iCounter++) {
                            this.parseObjectToArrayElements(currPropValue[iCounter]);
                        }
                    }
                    else {
                        this.parseObjectToArrayElements(currPropValue);
                    }
                    continue;
                }
                var currKeyValue = {
                    id: this._id++,
                    key: currKey,
                    value: currPropValue,
                    myElementType: elementType.field,
                };
                this._MyObjectElements.push(currKeyValue);
            }
        }
    };
    ObjectViewerComponent.prototype.toShow = function (myele) {
        if (this._SearchValue == '')
            return true;
        var search = myele.id + myele.key + myele.value;
        search = search.toLowerCase();
        if (search.indexOf(this._SearchValue.toLowerCase()) == -1) {
            return false;
        }
        else {
            return true;
        }
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], ObjectViewerComponent.prototype, "MyObject", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], ObjectViewerComponent.prototype, "MyNgStyleHeight", void 0);
    ObjectViewerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'object-viewer',
            templateUrl: './ObjectViewerComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ObjectViewerComponent);
    return ObjectViewerComponent;
}());
exports.ObjectViewerComponent = ObjectViewerComponent;
//# sourceMappingURL=ObjectViewerComponent.js.map