"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ActivityZoomItemViewModel = /** @class */ (function () {
    function ActivityZoomItemViewModel(logDetails) {
        this.LogDetails = logDetails;
        this.CardName = logDetails.CardName;
        this.Activities = logDetails.NumberOfActivities;
        this.ContactName = logDetails.ContactName;
    }
    return ActivityZoomItemViewModel;
}());
exports.ActivityZoomItemViewModel = ActivityZoomItemViewModel;
//# sourceMappingURL=ActivityZoomItemViewModel.js.map