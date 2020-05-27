"use strict";
/*
This file has no dependencies
GUID Creation

Example usage:

var id = Guid.newGuid();

*/
Object.defineProperty(exports, "__esModule", { value: true });
var Guid = /** @class */ (function () {
    function Guid() {
    }
    Guid.newGuid = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };
    Guid.NewRandomString = function () {
        var chars = 'abcdefghijklmnopqrstuvwxyz';
        var length = 32;
        var result = '';
        for (var i = length; i > 0; --i)
            result += chars[Math.floor(Math.random() * chars.length)];
        return result;
    };
    return Guid;
}());
exports.Guid = Guid;
//# sourceMappingURL=Guid.js.map