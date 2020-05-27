"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CodeNameClass = /** @class */ (function () {
    function CodeNameClass(myCode, myName) {
        if (myCode === void 0) { myCode = null; }
        if (myName === void 0) { myName = null; }
        this.Code = myCode;
        this.Name = myName;
    }
    Object.defineProperty(CodeNameClass.prototype, "Checked", {
        get: function () { return this.checked; },
        set: function (value) { this.checked = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CodeNameClass.prototype, "Code", {
        get: function () { return this.code; },
        set: function (value) { this.code = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CodeNameClass.prototype, "Name", {
        get: function () { return this.name; },
        set: function (value) { this.name = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CodeNameClass.prototype, "IntegerCode", {
        get: function () { return this.integerCode; },
        set: function (value) { this.integerCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CodeNameClass.prototype, "AdditionalField", {
        get: function () { return this.additionalField; },
        set: function (value) { this.additionalField = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CodeNameClass.prototype, "DateTime", {
        get: function () { return this.dateTime; },
        set: function (value) { this.dateTime = value; },
        enumerable: true,
        configurable: true
    });
    return CodeNameClass;
}());
exports.CodeNameClass = CodeNameClass;
//# sourceMappingURL=CodeNameClass.js.map