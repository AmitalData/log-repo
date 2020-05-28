"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var LocalStorageManager = /** @class */ (function () {
    function LocalStorageManager() {
    }
    LocalStorageManager.SetItem = function (key, item) {
        var isMac = navigator.platform.toUpperCase().indexOf('MAC') >= 0; //MAC//WIN32
        if (isMac) {
            console.warn("============>This is a mac machine no caching is used!!!!");
            return false;
        }
        try {
            window.localStorage.setItem(key, item);
            return true;
        }
        catch (ex) {
            console.warn(ex);
            return false;
        }
    };
    LocalStorageManager.GetItem = function (key) {
        return window.localStorage.getItem(key);
    };
    LocalStorageManager.RemoveItem = function (key) {
        try {
            window.localStorage.removeItem(key);
        }
        catch (ex) {
            console.warn(ex);
        }
    };
    return LocalStorageManager;
}());
exports.LocalStorageManager = LocalStorageManager;
//# sourceMappingURL=LocalStorageManager.js.map