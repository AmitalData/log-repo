"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var TicketClassificationCustomFilter = /** @class */ (function () {
    function TicketClassificationCustomFilter() {
    }
    TicketClassificationCustomFilter.GetFilteredQuery = function (addtionalFiltersValues, data) {
        var mykeys = JSON.parse(addtionalFiltersValues);
        for (var i in mykeys) {
            var propName = mykeys[i];
            if (propName.FieldName == "ParentId") {
                var value = propName.FieldValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    var myCheck = value.split('!')[1];
                    var myId = value.split('!')[0];
                    if (myCheck == "S") {
                        data = data.filter(function (d) { return d.ParentId != null && d.ParentId.startsWith(myId) && d.Inactive == false; });
                    }
                    else if (myCheck == "F") {
                        data = data.filter(function (d) { return d.ParentId == myId && d.Inactive == false; });
                    }
                }
            }
        }
        return data;
    };
    return TicketClassificationCustomFilter;
}());
exports.TicketClassificationCustomFilter = TicketClassificationCustomFilter;
//# sourceMappingURL=TicketClassificationCustomFilter.js.map