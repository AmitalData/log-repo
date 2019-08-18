"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ObjectCloner = /** @class */ (function () {
    function ObjectCloner() {
    }
    ObjectCloner.Clone = function (baseObject) {
        var clonedObject;
        clonedObject = (new baseObject.constructor);
        var baseKeys = Object.keys(baseObject);
        for (var key in baseKeys) {
            var property = baseKeys[key];
            clonedObject[property] = baseObject[property];
        }
        return clonedObject;
    };
    ObjectCloner.CopyObject = function (sourceObject, targetObject) {
        var targetObject;
        var baseKeys = Object.keys(sourceObject);
        for (var key in baseKeys) {
            var property = baseKeys[key];
            targetObject[property] = sourceObject[property];
        }
        return targetObject;
    };
    return ObjectCloner;
}());
exports.ObjectCloner = ObjectCloner;
//# sourceMappingURL=ObjectCloner.js.map