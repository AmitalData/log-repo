"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BookingUtilities = /** @class */ (function () {
    function BookingUtilities() {
    }
    BookingUtilities.IsBookingEditEnabled = function (entityPM) {
        var myResult = true;
        if (entityPM.IsCancelled) {
            myResult = false;
        }
        else if (entityPM.BookingStatusCode != "CRT") {
            myResult = false;
        }
        return myResult;
    };
    return BookingUtilities;
}());
exports.BookingUtilities = BookingUtilities;
//# sourceMappingURL=BookingUtilities.js.map