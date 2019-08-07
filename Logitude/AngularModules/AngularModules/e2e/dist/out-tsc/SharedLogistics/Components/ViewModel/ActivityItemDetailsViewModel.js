"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ActivityItemDetailsViewModel = /** @class */ (function () {
    function ActivityItemDetailsViewModel(details, activity) {
        this.Details = details;
        this.ActivityZoomItem = activity;
        this.CardName = activity.CardName;
        this.ContactName = activity.ContactName;
        this.Module = details.Module;
        this.GMTLogDateTime = details.GMTLogDateTime;
        this.Activity = details.Activity;
    }
    return ActivityItemDetailsViewModel;
}());
exports.ActivityItemDetailsViewModel = ActivityItemDetailsViewModel;
//# sourceMappingURL=ActivityItemDetailsViewModel.js.map